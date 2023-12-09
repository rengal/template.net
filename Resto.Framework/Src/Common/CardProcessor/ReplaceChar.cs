namespace Resto.Framework.Common.CardProcessor
{
    public sealed class ReplaceChar
    {
        /// <summary>
        /// Старое значение, которое нужно заменить. Допускается указывать как символ, так и виртуальный код клавиши в квадратных скобках (например, "[Enter]")
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// Новый символ. Допускается использовать hex-код, например, \x1D
        /// </summary>
        public string NewValue { get; set; }
    }
}
