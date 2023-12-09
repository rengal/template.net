using System;

namespace Resto.Framework.Common.CardProcessor
{
    public class CardRolledEventArgs : EventArgs
    {
        /// <summary>
        /// Результат распознавания карты
        /// </summary>
        public readonly ICardInfo CardInfo;
        /// <summary>
        /// Указывает, что событие обработано.
        /// Другие обработчики для этого события не будут вызваны
        /// </summary>
        public bool Handled { get; set; }

        public CardRolledEventArgs(ICardInfo cardInfo)
        {
            CardInfo = cardInfo;
            Handled = false;
        }
    }
}