using System;
using Resto.Common.Extensions;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class UserEvent
    {
        public UserEventAttribute FindAttributeByName(string attrName)
        {
            foreach (UserEventAttribute attr in Attributes)
            {
                if (attr.Id.Name == attrName)
                    return attr;
            }
            return null;
        }

        public DateTime? GetAttributeDateTime(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            return (eventAttr != null) ? eventAttr.ValueDate : null;
        }

        public decimal? GetAttributeDecimal(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            return (eventAttr != null) ? eventAttr.ValueNumber : null;
        }

        public int? GetAttributeInt(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            return (eventAttr != null && eventAttr.ValueNumber.HasValue) ? (int?)eventAttr.ValueNumber : null;
        }

        public Boolean? GetAttributeBoolean(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            if (eventAttr != null && eventAttr.ValueNumber.HasValue)
            {
                return eventAttr.ValueNumber.Value != 0;
            }
            return null;
        }

        public string GetAttributeString(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            return (eventAttr != null) ? eventAttr.ValueString : string.Empty;
        }

        public Guid? GetAttributeGuid(string name)
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            return (eventAttr != null) ? eventAttr.ValueGuid : null;
        }

        public T GetAttributeEntity<T>(string name) where T : PersistedEntity
        {
            UserEventAttribute eventAttr = FindAttributeByName(name);
            if (eventAttr == null || !eventAttr.ValueGuid.HasValue) return null;
            Guid guid = eventAttr.ValueGuid.GetValueOrFakeDefault();
            if (!EntityManager.INSTANCE.Contains(guid)) return null;
            return EntityManager.INSTANCE.Get(guid) as T;
        }
    }
}