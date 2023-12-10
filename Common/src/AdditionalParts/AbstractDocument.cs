using System;
using System.Collections.Generic;
using Resto.Common.Extensions;
using Resto.Common.Interfaces;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Data;

namespace Resto.Data
{
    public abstract partial class AbstractDocument : IGroupProcessableDocument
    {
        // true, если документ получен копированием(DuplicateDocument)
        [Transient]
        private Boolean isDuplicate;

        public Boolean IsDuplicate
        {
            get { return isDuplicate; }
            set { isDuplicate = value; }
        }

        public DateTime DateCreated
        {
            get
            {
                return createdInfo != null ? createdInfo.Date.GetValueOrFakeDefault() : new DateTime();
            }
            set
            {
                if (createdInfo == null)
                {
                    createdInfo = new OperationInfo();
                }

                createdInfo.Date = value;
            }
        }

        public DateTime DateModified
        {
            get
            {
                return modifiedInfo != null ? modifiedInfo.Date.GetValueOrFakeDefault() : new DateTime();
            }
            set
            {
                if (modifiedInfo == null)
                {
                    modifiedInfo = new OperationInfo();
                }

                modifiedInfo.Date = value;
            }
        }

        public DateTime? NullableDateModified
        {
            get
            {
                return modifiedInfo != null ? modifiedInfo.Date : null;
            }
            set
            {
                if (modifiedInfo == null)
                {
                    modifiedInfo = new OperationInfo();
                }

                modifiedInfo.Date = value;
            }
        }

        public User UserCreated
        {
            get
            {
                return createdInfo != null ? createdInfo.User : null;
            }
            set
            {
                if (createdInfo == null)
                {
                    createdInfo = new OperationInfo();
                }

                createdInfo.User = value;
            }
        }

        public User UserModified
        {
            get
            {
                return modifiedInfo != null ? modifiedInfo.User : null;
            }
            set
            {
                if (modifiedInfo == null)
                {
                    modifiedInfo = new OperationInfo();
                }

                modifiedInfo.User = value;
            }
        }

        public abstract Store DocStore { get; set; }
        public abstract Store DocStoreTo { get; set; }
        public abstract DepartmentEntity DocDepartmentTo { get; set; }
        public abstract Account DocAccount { get; set; }
        public abstract IEnumerable<IncomingDocumentItem> DocItems { get; set; }
        public abstract DocumentType DocumentType { get; }

        public virtual string DocumentFullCaption
        {
            get
            {
                string duplicateAddition = IsDuplicate ? Resources.AbstractDocumentCopy : string.Empty;
                return string.Format(Resources.AbstractDocumentNumberFrom2, DocumentType.GetLocalName(), DocumentNumber, DateIncoming.ToShortDateString(), duplicateAddition);
            }
        }

        public virtual string DocumentShortCaption
        {
            get
            {
                string duplicateAddition = IsDuplicate ? Resources.AbstractDocumentCopy : string.Empty;
                return string.Format(Resources.AbstractDocumentNumberFrom2, DocumentType.GetLocalShortName(), DocumentNumber, DateIncoming.ToShortDateString(), duplicateAddition);
            }
        }

        /// <summary>
        /// Список ошибок документа, который не должны валидироваться на сервере.
        /// </summary>
        public virtual ICollection<ValidationWarning> SuppressedWarnings { get { return new List<ValidationWarning>(); } }

        /// <summary>
        /// <para>Выясняет, имеет ли текущий пользователь право редактировать данный документ.</para>
        /// <para>Учитывает, что редактирование документа может быть запрещено не только в зависимости от типа, но и от статуса документа.</para>
        /// </summary>
        public virtual bool IsEditAllowedForCurrentUser()
        {
            return DocumentType.HasEditPermission(Status);
        }

        /// <summary>
        /// Разрешать создавать документы с одинаковыми номерами
        /// </summary>
        public virtual bool AllowDuplicateDocumentNumber
        {
            get { return true; }
        }

        #region IGroupProcessableDocument implementation

        public Guid? DocumentID
        {
            get { return Id; }
        }

        public string Number
        {
            get { return DocumentNumber; }
        }

        public DocumentType Type
        {
            get { return DocumentType.WRITEOFF_DOCUMENT; }
        }

        public DateTime? Date
        {
            get { return DateIncoming; }
        }

        public bool Deleted
        {
            get { return Status == DocumentStatus.DELETED; }
        }

        public bool Processed
        {
            get { return Status == DocumentStatus.PROCESSED; }
        }

        #endregion
    }
}