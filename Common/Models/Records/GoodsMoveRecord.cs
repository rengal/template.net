namespace Resto.Data
{
    /// <summary>
    /// UI-запись в таблице "Расширенная оборотно-сальдовая ведомость"
    /// </summary>
    public class GoodsMoveRecord
    {
        #region Constructors

        public GoodsMoveRecord()
        {
            RemainsStart = new DataContainer();
            RemainsEnd = new DataContainer();
            Invoice = new DataContainer();
            Transfer = new DataContainer();
            Writeoff = new DataContainer();
            InventoryExcess = new DataContainer();
            InventoryShortage = new DataContainer();
            Sale = new DataContainer();
            Inventory = new DataContainer();
            OutgoingInvoice = new DataContainer();
            Production = new DataContainer();
            Transformation = new DataContainer();
            Returned = new DataContainer();
            Dissasemble = new DataContainer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Продукт
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Цена 
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Остатки на начало
        /// </summary>
        public DataContainer RemainsStart { get; set; }

        /// <summary>
        /// Остатки на конец
        /// </summary>
        public DataContainer RemainsEnd { get; set; }

        /// <summary>
        /// Приход
        /// </summary>
        public DataContainer Invoice { get; set; }

        /// <summary>
        /// Внутреннее перемещение
        /// </summary>
        public DataContainer Transfer { get; set; }

        /// <summary>
        /// Списание
        /// </summary>
        public DataContainer Writeoff { get; set; }

        /// <summary>
        /// Излишки
        /// </summary>
        public DataContainer InventoryExcess { get; set; }

        /// <summary>
        /// Недостача
        /// </summary>
        public DataContainer InventoryShortage { get; set; }

        /// <summary>
        /// Продажа
        /// </summary>
        public DataContainer Sale { get; set; }

        /// <summary>
        /// Инвентаризация (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer Inventory { get; set; }

        /// <summary>
        /// Расходные накладные (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer OutgoingInvoice { get; set; }

        /// <summary>
        /// Приготовление (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer Production { get; set; }

        /// <summary>
        /// Переработка (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer Transformation { get; set; }

        /// <summary>
        /// Возврат (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer Returned { get; set; }

        /// <summary>
        /// Разбор (заполняется для вида отчета "Расширенный")
        /// </summary>
        public DataContainer Dissasemble { get; set; }

        #endregion

        #region Nested type: DataContainer

        /// <summary>
        /// Класс-контейнер сводных данных
        /// </summary>
        public class DataContainer
        {
            #region Properties

            /// <summary>
            /// Количество
            /// </summary>
            public decimal? Amount { get; set; }

            /// <summary>
            /// Сумма
            /// </summary>
            public decimal? Sum { get; set; }

            /// <summary>
            /// Значение НДС
            /// </summary>
            public decimal? Nds { get; set; }

            #endregion
        }

        #endregion
    }
}