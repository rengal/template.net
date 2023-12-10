using System;
using System.Collections.Generic;
using System.Diagnostics;
using Resto.Common.Interfaces;
using Resto.Data;
using System.Linq;
using Resto.Framework.Common;

namespace Resto.Common.Services
{
    /// <summary>
    /// Сервис для калькуляции автоматически добавляемых блюд
    /// </summary>
    public static class AutoAdditionService
    {
        public delegate bool IsValidAutoAddition(Guid productId, IDelivery delivery);

        private static bool? isReplicatingRms;

        /// <summary>
        /// Возвращает true, если данный сервер - это RMS участвующий в репликации, false - если Чейн или StandaloneRMS
        /// </summary>
        public static bool IsReplicatingRms()
        {
            if (isReplicatingRms == null)
            {
                isReplicatingRms = ServiceClientFactory.ReplicationSlaveServerService.IsReplicatingRMS().CallSync();
            }
            return isReplicatingRms.Value;
        }

        /// <summary>
        /// Возвращает список элементов автодобавления
        /// </summary>
        /// <param name="delivery">Доставка</param>
        /// <param name="autoAdditionSettings">Настройки автодобавления</param>
        /// <param name="isValidAutoAddition">Делегат, возвращающий флаг, является ли блюдо валидным для автодобавления</param>
        public static List<AutoAdditionElement> CalculateAutoAdditionItems(IDelivery delivery, IAutoAdditionSettings autoAdditionSettings, IsValidAutoAddition isValidAutoAddition, Guid? deliveryCostProductId)
        {
            // Если блюд в заказе нет вообще, то возвращаем пустой лист автодобавляемых позиций
            if (!delivery.GetOrderItems().Any())
                return new List<AutoAdditionElement>();

            var additions = new Dictionary<ProductGuestAutoAddType, decimal>();
            foreach (var item in autoAdditionSettings.GetItems().Where(item => !item.Deleted && isValidAutoAddition(item.ProductId, delivery)))
            {
                if (item.AutoAdditionType == AutoAdditionType.PER_PRODUCT)
                    AutoAdditionPerProduct(delivery, item, additions);
                else if (item.AutoAdditionType == AutoAdditionType.PER_PERSON)
                    AutoAdditionPerPerson(delivery, item, additions);
                else if (item.AutoAdditionType == AutoAdditionType.FOR_ORDER)
                    AutoAdditionForOrder(delivery, item, additions);
            }
            // добавление услуги доставки
            if (deliveryCostProductId.HasValue && isValidAutoAddition(deliveryCostProductId.Value, delivery))
                AddDeliveryCost(delivery, deliveryCostProductId.Value, additions);

            return additions.Select(elem => new AutoAdditionElement(elem.Key.ProductId, elem.Value, elem.Key.GuestId)).ToList();
        }

        /// <summary>
        /// Автодобавление для блюд
        /// </summary>
        private static void AutoAdditionPerProduct(IDelivery delivery, IAutoAdditionSettingsItem item, IDictionary<ProductGuestAutoAddType, decimal> additions)
        {
            if (delivery.GetSplitBetweenPersons())
            {
                foreach (var group in delivery.GetOrderItems(excludeNotDeleted: true).GroupBy(i => i.GetGuestId()))
                {
                    // количество блюд заказа, для которых сработает данное правило
                    var countProductsForRule = group.Where(oi => item.InitiatorProductsId.Contains(oi.GetProductId())).Sum(oi => oi.GetAmount());
                    if (countProductsForRule == 0)
                        continue;
                    var additionAmount = Math.Ceiling(countProductsForRule * item.Quantity);
                    AutoAdditionIncreaseAmount(additions, item.ProductId, group.Key, additionAmount);
                }
            }
            else
            {
                // количество блюд заказа, для которых сработает данное правило
                var countProductsForRule = delivery.GetOrderItems(excludeNotDeleted: true).Where(oi => item.InitiatorProductsId.Contains(oi.GetProductId())).Sum(oi => oi.GetAmount());
                // гостя полагаем равным владельцу первого блюда
                var guestId = delivery.GetOrderItems().First().GetGuestId();
                if (countProductsForRule == 0)
                    return;
                var additionAmount = Math.Ceiling(countProductsForRule * item.Quantity);
                AutoAdditionIncreaseAmount(additions, item.ProductId, guestId, additionAmount);
            }
        }

        /// <summary>
        /// Автодобавление по персонам
        /// </summary>
        private static void AutoAdditionPerPerson(IDelivery delivery, IAutoAdditionSettingsItem item, IDictionary<ProductGuestAutoAddType, decimal> additions)
        {
            if (item.InitiatorProductsId.Any() && !delivery.GetOrderItems().Select(i => i.GetProductId()).Intersect(item.InitiatorProductsId).Any())
                return;

            if (delivery.GetSplitBetweenPersons())
            {
                foreach (var guestId in delivery.GetGuestsIds())
                    AutoAdditionIncreaseAmount(additions, item.ProductId, guestId, Math.Ceiling(1 * item.Quantity));
            }
            else
            {
                // гостя полагаем равным владельцу первого блюда
                var guestId = delivery.GetOrderItems().First().GetGuestId();
                AutoAdditionIncreaseAmount(additions, item.ProductId, guestId, Math.Ceiling(delivery.GetPersonsCount() * item.Quantity));
            }
        }

        /// <summary>
        /// Автодобавление на заказ
        /// </summary>
        private static void AutoAdditionForOrder(IDelivery delivery, IAutoAdditionSettingsItem item, IDictionary<ProductGuestAutoAddType, decimal> additions)
        {
            if (delivery.GetSplitBetweenPersons())
            {
                var countGuestsIds = delivery.GetGuestsIds().Count();
                Debug.Assert(delivery.GetPersonsCount() == countGuestsIds);

                // отпечатанные "куски" блюда, автодобавленного на весь заказ, удалять нельзя; поэтому при расчёте возвращаем количество за их вычетом
                var itemAmount = item.Quantity;
                itemAmount -= delivery.GetOrderItems().Where(i => i.IsItemPrinted() && i.GetProductId() == item.ProductId).Sum(i => i.GetAmount());
                if (itemAmount <= 0)
                    return;

                // разделяем блюда, автодобавляемые на заказ, на равные части между гостями
                var parts = itemAmount.SplitWeight(countGuestsIds); 
                var index = 0;
                foreach (var guestId in delivery.GetGuestsIds())
                    AutoAdditionIncreaseAmount(additions, item.ProductId, guestId, parts[index++]);
            }
            else
            {
                // гостя полагаем равным владельцу первого блюда
                var guestId = delivery.GetOrderItems().First().GetGuestId();
                AutoAdditionIncreaseAmount(additions, item.ProductId, guestId, item.Quantity);
            }
        }

        /// <summary>
        /// Добавление услуги доставки
        /// </summary>
        private static void AddDeliveryCost(IDelivery delivery, Guid deliveryCostProductId, IDictionary<ProductGuestAutoAddType, decimal> additions)
        {
            var amount = 1.0m;
            if (delivery.GetSplitBetweenPersons())
            {
                var countGuestsIds = delivery.GetGuestsIds().Count();
                Debug.Assert(delivery.GetPersonsCount() == countGuestsIds);

                // отпечатанные "куски" блюда - услуги доставки, удалять нельзя; поэтому при расчёте возвращаем количество за их вычетом
                amount -= delivery.GetOrderItems().Where(i => i.IsItemPrinted() && i.GetProductId() == deliveryCostProductId).Sum(i => i.GetAmount());
                if (amount <= 0)
                    return;

                // разделяем услугу доставки на равные части между гостями
                var parts = amount.SplitWeight(delivery.GetPersonsCount());
                var index = 0;
                foreach (var guestId in delivery.GetGuestsIds())
                    AutoAdditionIncreaseAmount(additions, deliveryCostProductId, guestId, parts[index++]);
            }
            else
            {
                // гостя полагаем равным владельцу первого блюда
                var guestId = delivery.GetOrderItems().First().GetGuestId();
                AutoAdditionIncreaseAmount(additions, deliveryCostProductId, guestId, amount);
            }
        }

        private static void AutoAdditionIncreaseAmount(IDictionary<ProductGuestAutoAddType, decimal> dict, Guid productId, Guid guestId, decimal amount)
        {
            var key = new ProductGuestAutoAddType(productId, guestId);
            if (!dict.ContainsKey(key))
                dict.Add(key, 0);
            dict[key] += amount;
        }

        private sealed class ProductGuestAutoAddType
        {
            /// <summary>
            /// Идентификатор блюда.
            /// </summary>
            public Guid ProductId { get; private set; }

            /// <summary>
            /// Имя гостя (персоны).
            /// </summary>
            public Guid GuestId { get; private set; }

            public ProductGuestAutoAddType(Guid productId, Guid guestId)
            {
                ProductId = productId;
                GuestId = guestId;
            }

            private bool Equals(ProductGuestAutoAddType other)
            {
                return ProductId.Equals(other.ProductId) && GuestId.Equals(other.GuestId);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ProductGuestAutoAddType && Equals((ProductGuestAutoAddType)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (ProductId.GetHashCode() * 397) ^ GuestId.GetHashCode();
                }
            }
        }
    }
}
