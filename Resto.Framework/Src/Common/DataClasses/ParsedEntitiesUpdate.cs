using System;
using System.Collections.Generic;

namespace Resto.Framework.Data
{
    [DataClass("ParsedEntitiesUpdate")]
    public class ParsedEntitiesUpdate : DataUpdate
    {
        private List<ParsedEntitiesUpdateItem> items;
        private bool fullUpdate;

        public ParsedEntitiesUpdate() { }

        public ParsedEntitiesUpdate(Guid? serverInstanceId, int revision, bool fullUpdate)
            : base(serverInstanceId, revision)
        {
            items = new List<ParsedEntitiesUpdateItem>();
            this.fullUpdate = fullUpdate;
        }

        public List<ParsedEntitiesUpdateItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public bool FullUpdate
        {
            get { return fullUpdate; }
            set { fullUpdate = value; }
        }

    }

}