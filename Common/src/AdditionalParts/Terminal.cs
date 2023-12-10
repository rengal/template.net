using System;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Data
{
    public partial class Terminal : IComparable
    {
        [NotNull]
        public PbxAuthorizationSettings NotNullPbxAuthorizationSettings
        {
            get { return PbxAuthorizationSettings ?? (PbxAuthorizationSettings = new PbxAuthorizationSettings()); }
        }

        public string Description
        {
            get
            {
                return string.IsNullOrEmpty(computerName)
                    ? NameLocal
                    : string.Format("{0} \"{1}\"", NameLocal, ComputerName);
            }
        }

        public bool IsRegistered => !Deleted && !Anonymous;

        public override string ToString()
        {
            return ComputerName;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null && obj is Terminal)
            {
                return ComputerName.CompareTo((obj as Terminal).ComputerName);
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }
}