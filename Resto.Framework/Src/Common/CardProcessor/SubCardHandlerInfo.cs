using System.Collections.Generic;

namespace Resto.Framework.Common.CardProcessor
{
    public class SubCardHandlerInfo
    {
        public string CardType { get; private set; }
        public bool Mandatory { get; private set; }

        private SubCardHandlerInfo(string cardType, bool mandatory)
        {
            CardType = cardType;
            Mandatory = mandatory;
        }

        public class List : List<SubCardHandlerInfo>
        {
            public void Add(string cardType)
            {
                Add(new SubCardHandlerInfo(cardType, true));
            }

            public void Add(string cardType, bool mandatory)
            {
                Add(new SubCardHandlerInfo(cardType, mandatory));
            }
        }
    }
}
