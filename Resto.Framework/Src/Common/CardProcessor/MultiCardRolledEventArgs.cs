using System;
using System.Collections.Generic;

namespace Resto.Framework.Common.CardProcessor
{
    public class MultiCardRolledEventArgs : EventArgs
    {
        /// <summary>
        /// Результат распознавания карт
        /// </summary>
        public Dictionary<string, ICardInfo> CardInfos { get; private set; }
        public Dictionary<string, object> FailInfos { get; private set; }

        public MultiCardRolledEventArgs(Dictionary<string, ICardInfo> cardInfos, Dictionary<string, object> failInfos)
        {
            FailInfos = failInfos;
            CardInfos = cardInfos;
        }
    }
}