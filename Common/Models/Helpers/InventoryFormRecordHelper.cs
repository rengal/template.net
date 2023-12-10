using System;
using System.Collections.Generic;
using System.Linq;

using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Framework.Common;

namespace Resto.Common.Models.Helpers
{
    public static class InventoryFormRecordHelper
    {
        #region Methods

        /// <summary>
        /// Считает и присваивает общие для смёрженых рекордов поля.
        /// </summary>
        public static void UpdateMergedRecords(IEnumerable<InventoryFormRecord> records, [NotNull] InventoryFormRecord record, Dictionary<Product, decimal> turnovers, bool isTurnover)
        {
            var mergedRecords = new List<InventoryFormRecord>();
            decimal firstStepCount = 0;
            decimal secondStepCount = 0;

            foreach (var rec in records)
            {
                if (rec.Merged &&
                    InventoryFormRecord.CanMerge(rec, record))
                {
                    mergedRecords.Add(rec);
                    if (rec.IsDisassembled)
                    {
                        firstStepCount += rec.FirstStepAdditionCount;
                    }
                    else
                    {
                        secondStepCount += rec.CurrentActualAmount;
                    }
                }
            }

            decimal totalCount = firstStepCount + secondStepCount;

            foreach (InventoryFormRecord rec in mergedRecords)
            {
                rec.FirstStepAdditionCount = firstStepCount;
                rec.CurrentAmountWithAdditions = totalCount;
            }

            if (isTurnover)
            {
                decimal? turnover = record.IsSaved
                    ? turnovers[record.Product]
                    : mergedRecords.Select(rec => rec.NullableTurnover).FirstOrDefault(val => val.HasValue);
                mergedRecords.ForEach(rec => rec.NullableTurnover = turnover);
            }
        }

        /// <summary>
        /// Создание объекта InventoryFormRecord (1-й этап) из серверной модели
        /// </summary>
        public static InventoryFormRecord CreateFirstStepRecord(IncomingInventoryFirstStepItem inventoryItem)
        {
            return new InventoryFormRecord(inventoryItem)
            {
                CurrentActualAmount = inventoryItem.Amount,
                CountState = inventoryItem.Status != InventoryItemStatus.NEW ? 1 : 0
            };
        }

        /// <summary>
        /// Возвращает на основе документа Инвентаризации, список записей 2-го шага.
        /// </summary>
        public static List<InventoryFormRecord> GetSecondStepRecords(this IncomingInventory doc, out Dictionary<Product, decimal> turnovers)
        {
            var records = new List<InventoryFormRecord>();
            turnovers = new Dictionary<Product, decimal>();

            // Инициализируем данные второго шага
            InventoryFormRecord cachedRecord = null;

            foreach (var item in doc.Items.Where(item => item != null).OrderBy(item => item.Num))
            {
                var inventoryRecord = item.ToSecondStepInventoryFormRecord();

                if (item.Status != InventoryItemStatus.NEW_CLEAR)
                {
                    // Для инвентаризаций в середине дня строим информацию о движениях по сохраненны позициям и только одна запись из группы объединенных содержит информацию о движении
                    if (doc.RegisterTurnover && item.Status == InventoryItemStatus.SAVE && item.Turnover.HasValue)
                    {
                        turnovers[item.Product] = item.Turnover.Value;
                    }
                    inventoryRecord.CountState = 1;
                }
                else
                {
                    inventoryRecord.CountState = inventoryRecord.CurrentActualAmount != 0 || inventoryRecord.FirstStepAdditionCount != 0
                        ? 1
                        : (inventoryRecord.Product != null ? 1 : 0);
                }

                if (cachedRecord != null && InventoryFormRecord.CanMerge(cachedRecord, inventoryRecord))
                {
                    inventoryRecord.Merged = true;
                    cachedRecord.Merged = true;
                }

                cachedRecord = inventoryRecord;

                records.Add(inventoryRecord);
            }

            // Производим объединение строк
            for (int i = 0; i < records.Count - 1; i++)
            {
                InventoryFormRecord record = records[i];
                InventoryFormRecord nextRecord = records[i + 1];

                if (record.Merged && nextRecord.Product.Equals(record.Product))
                {
                    UpdateMergedRecords(records, record, turnovers, doc.RegisterTurnover && record.Action == InventoryItemStatus.SAVE && record.NullableTurnover.HasValue);
                }

                if (record.Product == null || nextRecord.Product == null)
                {
                    continue;
                }

                if (InventoryFormRecord.CanMerge(record, nextRecord))
                {
                    record.Merged = true;
                    nextRecord.Merged = true;
                }
            }

            return records;
        }

        /// <summary>
        /// Возвращает на основе документа Инвентаризации, список записей 2-го шага.
        /// </summary>
        public static IEnumerable<InventoryFormRecord> GetSecondStepRecords(this IncomingInventory doc)
        {
            return doc.GetSecondStepRecords(out _);
        }

        /// <summary>
        /// Возвращает записи 3-го шага инвентаризации (себестоимость и книжное кол-во) с учётом даты и склада.
        /// </summary>
        public static ICollection<InventoryFormRecord> GetThirdStepRecords(
            IAsyncCallHelper<StoresProductsBalance> callHelperStoresProductsBalance,
            IAsyncCallHelper<Dictionary<Product, EvaluableDecimalValue>> callHelpeSlidingAvgProductCosts,
            Dictionary<Product, decimal> corrections,
            List<InventoryFormRecord> records, 
            DateTime dt, 
            Store store, 
            Guid id)
        {
            StoresProductsBalance balances = null;
            if (callHelperStoresProductsBalance.Call(
                    ServiceClientFactory.StoreService.GetStoresProductsBalance(dt)))
            {
                balances = callHelperStoresProductsBalance.Value;
            }

            Dictionary<Product, decimal> storeAmountBalances = null;
            if (balances != null && balances.Balances.ContainsKey(store))
            {
                storeAmountBalances = balances.Balances[store].Amounts;
            }

            Dictionary<Product, EvaluableDecimalValue> costPrices = null;
            if (callHelpeSlidingAvgProductCosts.Call(
                    ServiceClientFactory.StoreService.GetSlidingAvgProductCosts(
                        dt, new List<Store> { store }, false)))
            {
                costPrices = callHelpeSlidingAvgProductCosts.Value;
            }

            foreach (InventoryFormRecord record in records.Where(r => r.Product != null))
            {
                record.CostPrice = costPrices != null && costPrices.ContainsKey(record.Product)
                    ? costPrices[record.Product] : EvaluableDecimalValue.EVAL_ZERO;

                record.BookCount = storeAmountBalances != null && storeAmountBalances.ContainsKey(record.Product)
                    ? storeAmountBalances[record.Product] : 0;
            }

            records.ForEach(r =>
            {
                if (r.IsSaved && corrections.ContainsKey(r.Product))
                {
                    r.FullAdditionCostPrice = new EvaluableDecimalValue(corrections[r.Product], false);
                }
            });

            var recordListThree = new List<InventoryFormRecord>();

            foreach (InventoryFormRecord record in records.Where(r => r.IsSaved && r.Product != null))
            {
                if (store != null && balances != null && balances.Balances.ContainsKey(store)
                    && balances.Balances[store].Sums.ContainsKey(record.Product))
                {
                    record.SumBefore = new EvaluableDecimalValue(balances.Balances[store].Sums[record.Product], false);
                }
                else
                {
                    var costPrice = costPrices != null && costPrices.ContainsKey(record.Product)
                        ? costPrices[record.Product] : EvaluableDecimalValue.EVAL_ZERO;

                    record.SumBefore = costPrice.Multiply(record.BookCount);
                }

                record.SumAfter = record.SumBefore.Add(record.FullAdditionCostPrice);

                if (!recordListThree.Any(r => Equals(r.Product, record.Product)))
                {
                    recordListThree.Add(record.MakeCopy());
                }
            }

            return recordListThree;
        }

        /// <summary>
        /// Создание объекта InventoryFormRecord (2-й этап) из серверной модели
        /// </summary>
        public static InventoryFormRecord ToSecondStepInventoryFormRecord(this IncomingInventoryItem inventoryItem)
        {
            return new InventoryFormRecord(inventoryItem)
            {
                Action = inventoryItem.Status == InventoryItemStatus.NEW_CLEAR
                    ? InventoryItemStatus.NEW
                    : inventoryItem.Status,
                // кол-во, введённое пользователем
                CurrentActualAmount = inventoryItem.IsDisassembled ? 0 : inventoryItem.CurrentActualAmount.GetValueOrDefault(),
                // кол-во с учётом блюд и заготовок
                // инициализация нужна если товар имеет только одну позицию на 2-м шаге, 
                // для смердженных позиций, значение будет пересчитано
                CurrentAmountWithAdditions = inventoryItem.Amount + (inventoryItem.Turnover ?? 0),
                // При сохранении IsDisassembled-записей (см. MakeNewInventory()) из FirstStepAdditionCount в Amount,
                // соответсвеннно, при восстановлении - наоборот.
                IsDisassembled = inventoryItem.IsDisassembled,
                NullableFirstStepAdditionCount = inventoryItem.IsDisassembled ? inventoryItem.Count.GetValueOrDefault() : null,
                RecalculationNumber = inventoryItem.RecalculationNumber.GetValueOrDefault(),
                NullableTurnover = inventoryItem.Turnover,
                RecalculationDate = inventoryItem.RecalculationDate,
                NullableCurrentAmountWithPackage = inventoryItem.CountGross.GetValueOrDefault().ZeroAsNull()
            };
        }

        /// <summary>
        /// Возвращает остатки на складе за конкретную дату.
        /// </summary>
        [NotNull]
        public static Dictionary<Product, decimal> GetFixedItemsCosts(
            IAsyncCallHelper<List<StoreTransactionInfo>> callHelper,
            Guid id)
        {
            var corrections = new Dictionary<Product, decimal>();
            
            var call = ServiceClientFactory.DocumentService.GetDocumentTransactions(id, null);

            List<StoreTransactionInfo> infoList = null;
            if (callHelper.Call(call))
            {
                infoList = callHelper.Value;
            }

            if (infoList != null)
            {
                // За счет этой строчки мы проверим уникальность продуктов для транзакций документа
                // и заодно увеличим производительность второго шага - заполнение записей.
                corrections = infoList.ToDictionary(transactionInfo => transactionInfo.Product,
                    transactionInfo => transactionInfo.Sum.GetValueOrDefault());
            }

            return corrections;
        }

        /// <summary>
        /// Определение общей суммы недостачи
        /// </summary>
        public static decimal GetShortageSum(IEnumerable<InventoryFormRecord> items, Func<bool> predicate)
        {
            decimal shortageSum = items.Where(r => r.HasDifference(predicate) && r.FullAdditionCostPrice.Value.GetValueOrDefault() < 0)
                .Sum(r => r.FullAdditionCostPrice.Value.GetValueOrDefault());

            return -shortageSum;
        }

        /// <summary>
        /// Определение общей суммы излишков
        /// </summary>
        public static decimal GetSurplusSum(IEnumerable<InventoryFormRecord> items, Func<bool> predicate)
        {
            return items.Where(r => r.HasDifference(predicate) && r.FullAdditionCostPrice.Value.GetValueOrDefault() > 0)
                .Sum(r => r.FullAdditionCostPrice.Value.GetValueOrDefault());
        }

        #endregion
    }
}
