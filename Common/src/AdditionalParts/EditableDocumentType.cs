using System;
using Resto.Common.Properties;

namespace Resto.Data
{
    public class EditableDocumentType : IComparable
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        private readonly int weigth = -1;

        private EditableDocumentType(string name, string fullName, int weigth)
        {
            Name = name;
            FullName = fullName;
            this.weigth = weigth;
        }

        public override string ToString()
        {
            return Name ?? "";
        }

        public static EditableDocumentType GetDocumentType(EditableDocumentListRecord record)
        {
            if (!record.IsAutomatic)
                return MANUAL_DOCUMENT;
            if (record.IsEditable)
                return CLOSED_AUTO_DOCUMENT;
            return OPEN_AUTO_DOCUMENT;
        }

        public bool IsOpenedAutoDocument
        {
            get
            {
                return this == OPEN_AUTO_DOCUMENT;
            }
        }

        public static EditableDocumentType DEFAULT_DOCUMENT = new EditableDocumentType("", "", 0);
        public static EditableDocumentType MANUAL_DOCUMENT = new EditableDocumentType(Resources.EditableDocumentTypeManualDocumentAbb, Resources.EditableDocumentTypeManualDoc, 1);
        public static EditableDocumentType OPEN_AUTO_DOCUMENT = new EditableDocumentType(Resources.EditableDocumentTypeOpenAutoDocAbb, Resources.EditableDocumentTypeOpenAutoDoc, 2);
        public static EditableDocumentType CLOSED_AUTO_DOCUMENT = new EditableDocumentType(Resources.EditableDocumentTypeClosedAutoDocAbb, Resources.EditableDocumentTypeClosedAutoDoc, 3);

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is EditableDocumentType))
                return 1;
            else if (String.IsNullOrEmpty(Name))
                return obj.ToString().CompareTo("");
            else
            {
                EditableDocumentType salesDocumentType = (EditableDocumentType)obj;
                return weigth.CompareTo(salesDocumentType.weigth);
            }
        }

        #endregion
    }
}