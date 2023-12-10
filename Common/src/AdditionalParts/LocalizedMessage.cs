using System;
using System.Collections.Generic;
using Resto.Common.Localization;

namespace Resto.Data
{
    public partial class LocalizedMessage : LocalizableName<LocalizedMessage>
    {
        public override string NameResId
        {
            get { return resId; }
        }

        public override bool Equals(object obj)
        {
            var localizableMessage = obj as LocalizedMessage;
            return localizableMessage != null && resId == localizableMessage.resId;
        }

        public override int GetHashCode()
        {
            return resId.GetHashCode();
        }

        public override string ToString()
        {
            return this.GetLocalName();
        }
    }
}