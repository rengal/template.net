namespace Resto.Common.Plugins
{
    /// <summary>
    /// Базовый класс настройки.
    /// </summary>
    public abstract class Setting<T> : Setting
    {
        /// <summary>
        /// Значение.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Текст метки.
        /// </summary>
        public string LabelText { get; set; }

        /// <summary>
        /// Доступность редактирования.
        /// </summary>
        public bool IsReadonly { get; set; }
    }
}
