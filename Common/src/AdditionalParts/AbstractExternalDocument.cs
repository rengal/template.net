using System;
using System.Linq;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class AbstractExternalDocument
    {
        #region Properties

        public abstract string DocumentFullCaption { get; }

        /// <summary>
        /// Дата создания документа
        /// </summary>
        public DateTime DateCreated
        {
            get { return NullableDateCreated ?? new DateTime(); }
        }

        /// <summary>
        /// Дата создания документа
        /// </summary>
        public DateTime? NullableDateCreated
        {
            get { return CreatedInfo != null ? (DateTime?)CreatedInfo.Date.GetValueOrFakeDefault() : null; }
        }

        /// <summary>
        /// Пользователь создавший документ
        /// </summary>
        public User UserCreated
        {
            get { return CreatedInfo != null ? CreatedInfo.User : null; }
        }

        /// <summary>
        /// Дата изменения документа
        /// </summary>
        public DateTime DateModified
        {
            get { return NullableDateModified ?? new DateTime(); }
        }

        /// <summary>
        /// Дата изменения документа
        /// </summary>
        public DateTime? NullableDateModified
        {
            get { return ModifiedInfo != null ? (DateTime?)ModifiedInfo.Date.GetValueOrFakeDefault() : null; }
        }

        /// <summary>
        /// Пользователь изменивший документ последним
        /// </summary>
        public User UserModified
        {
            get { return ModifiedInfo != null ? ModifiedInfo.User : null; }
        }

        /// <summary>
        /// Итоговая сумма по документу
        /// </summary>
        public virtual decimal DocumentSum
        {
            get { return Items.Sum(item => item.Sum); }
        }

        public abstract string DocumentStatusStr { get; }

        #endregion
    }
}
