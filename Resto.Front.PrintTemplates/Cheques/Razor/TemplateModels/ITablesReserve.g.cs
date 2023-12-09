// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Табличка о зарезервированных столах
    /// </summary>
    public interface ITablesReserve : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Резерв/банкет
        /// </summary>
        [NotNull]
        IReserve Reserve { get; }

    }

    internal sealed class TablesReserve : TemplateModelBase, ITablesReserve
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly Reserve reserve;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private TablesReserve()
        {}

        internal TablesReserve([NotNull] CopyContext context, [NotNull] ITablesReserve src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            reserve = context.GetConverted(src.Reserve, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Reserve.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IReserve Reserve
        {
            get { return reserve; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this ITablesReserve cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new TablesReserve(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<ITablesReserve>(copy, "TablesReserve");
        }
    }
}
