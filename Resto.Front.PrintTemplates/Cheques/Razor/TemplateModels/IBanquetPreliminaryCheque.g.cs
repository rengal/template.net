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
    /// Чек предварительного заказа на банкет
    /// </summary>
    public interface IBanquetPreliminaryCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Банкет
        /// </summary>
        [NotNull]
        IReserve Banquet { get; }

    }

    internal sealed class BanquetPreliminaryCheque : TemplateModelBase, IBanquetPreliminaryCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly Reserve banquet;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private BanquetPreliminaryCheque()
        {}

        internal BanquetPreliminaryCheque([NotNull] CopyContext context, [NotNull] IBanquetPreliminaryCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            banquet = context.GetConverted(src.Banquet, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Reserve.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IReserve Banquet
        {
            get { return banquet; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IBanquetPreliminaryCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new BanquetPreliminaryCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IBanquetPreliminaryCheque>(copy, "BanquetPreliminaryCheque");
        }
    }
}
