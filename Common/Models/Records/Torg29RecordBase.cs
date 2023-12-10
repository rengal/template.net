using System;
using System.Collections.Generic;
using System.Linq;

using Resto.Common.Localization;
using Resto.Data;

namespace Resto.Common.Models.Records
{
    /// <summary>
    /// Базовый класс UI-записи товарного отчета
    /// </summary>
    public class Torg29RecordBase
    {
        #region Properties

        /// <summary>
        /// Тип документа: входящий/исходящий.
        /// </summary>
        public bool IsIncome { get; set; }

        /// <summary>
        /// Тип документа: приход/расход.
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// Дата проводки документа.
        /// </summary>
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Комментарий к документу.
        /// </summary>
        public string DocumentComment { get; set; }

        /// <summary>
        /// Номер входящего документа (если есть).
        /// </summary>
        public string IncomingDocumentNumber { get; set; }

        /// <summary>
        /// Счёт выручки.
        /// </summary>
        public Account RevenueAccount { get; set; }

        /// <summary>
        /// Счёт расхода.
        /// </summary>
        public Account ExpenseAccount { get; set; }

        /// <summary>
        /// Тип документа.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Тип транзакции.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Название документа.
        /// </summary>
        public string DocumentName
        {
            get
            {
                return DocumentType != null ? DocumentType.GetLocalName()
                    : (TransactionType != null ? TransactionType.GetLocalName() : string.Empty);
            }
        }

        /// <summary>
        /// Основной склад прихода.
        /// </summary>
        public Store IncomeStore { get; set; }

        /// <summary>
        /// Список складов прихода.
        /// </summary>
        public IEnumerable<Store> IncomeStores
        {
            get { return IncomeStore != null ? new List<Store> { IncomeStore } : (HasIncomeStoresItems ? StoresItems : null); }
        }

        /// <summary>
        /// Склады прихода строкой.
        /// </summary>
        public string IncomeStoreAsString
        {
            get { return IncomeStores != null ? StoreExtentions.AsString(IncomeStores) : string.Empty; }
        }

        /// <summary>
        /// Основной склад расхода.
        /// </summary>
        public Store OutgoneStore { get; set; }

        /// <summary>
        /// Список складов расхода.
        /// </summary>
        public IEnumerable<Store> OutgoneStores
        {
            get { return OutgoneStore != null ? new List<Store> { OutgoneStore } : (HasOutgoneStoresItems ? StoresItems : null); }
        }

        /// <summary>
        /// Склады расхода строкой.
        /// </summary>
        public string OutgoneStoreAsString
        {
            get { return OutgoneStores != null ? StoreExtentions.AsString(OutgoneStores) : string.Empty; }
        }

        /// <summary>
        /// Список складов по позициям документов.
        /// </summary>
        public List<Store> StoresItems { get; set; }

        /// <summary>
        /// Возвращает true, есди список складов по позициям не пустой и тип документа, расходная накладная, 
        /// возвратная накладная, акт реализации или акт списания.
        /// </summary>
        public bool HasOutgoneStoresItems
        {
            get
            {
                return StoresItems != null && StoresItems.Any() &&
                       (Equals(DocumentType, DocumentType.OUTGOING_INVOICE) || Equals(DocumentType, DocumentType.RETURNED_INVOICE) ||
                        Equals(DocumentType, DocumentType.SALES_DOCUMENT) || Equals(DocumentType, DocumentType.WRITEOFF_DOCUMENT));
            }
        }

        /// <summary>
        /// Возвращает true, есди список складов по позициям не пустой и тип документа, приходная накладная.
        /// </summary>
        public bool HasIncomeStoresItems
        {
            get { return StoresItems != null && StoresItems.Any() && Equals(DocumentType, DocumentType.INCOMING_INVOICE); }
        }

        #endregion
    }
}