using System;
using System.Collections.Generic;

namespace Resto.Framework.Data
{
    [DataClass("EntitiesUpdate")]
    public class EntitiesUpdate : DataUpdate
    {
        private List<EntitiesUpdateItem> items;
        private bool fullUpdate;
        private string version;

        public EntitiesUpdate() { }

        public EntitiesUpdate(Guid? serverInstanceId, int revision, bool fullUpdate)
            : base(serverInstanceId, revision)
        {
            items = new List<EntitiesUpdateItem>();
            this.fullUpdate = fullUpdate;
        }

        public List<EntitiesUpdateItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public bool FullUpdate
        {
            get { return fullUpdate; }
            set { fullUpdate = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }
    }

}