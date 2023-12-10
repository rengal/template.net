using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class LocalizableNamePersistedEntity : INamed
    {
        [NotNull]
        private LocalizableValue name = new LocalizableValue(string.Empty);

        public string NameLocal
        {
            get { return name.Local; }
            set { name = new LocalizableValue(value); }
        }

        [NotNull]
        public LocalizableValue Name
        {
            get { return name; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value), "Unable to assign null to property marked as NotNull");

                name.CurrentResourceId = value.CurrentResourceId;
                name.CustomValue = value.CustomValue;
            }
        }
    }
}
