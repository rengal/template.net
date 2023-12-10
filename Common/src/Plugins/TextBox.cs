namespace Resto.Common.Plugins
{
    /// <summary>
    /// Текстовое поле.
    /// </summary>
    public sealed class TextBox : Setting<string>
    {
        /// <summary>
        /// Максимальная длина.
        /// </summary>
        public int MaxLength { get; set; }
    }
}
