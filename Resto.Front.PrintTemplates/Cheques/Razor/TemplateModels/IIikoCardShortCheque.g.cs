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
    /// Чек iikoCard
    /// </summary>
    public interface IIikoCardShortCheque : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Информация об операции и карте
        /// </summary>
        [NotNull]
        IIikoCardInfo CardInfo { get; }

    }

    internal sealed class IikoCardShortCheque : TemplateModelBase, IIikoCardShortCheque
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly IikoCardInfo cardInfo;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private IikoCardShortCheque()
        {}

        internal IikoCardShortCheque([NotNull] CopyContext context, [NotNull] IIikoCardShortCheque src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            cardInfo = context.GetConverted(src.CardInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.IikoCardInfo.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public IIikoCardInfo CardInfo
        {
            get { return cardInfo; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IIikoCardShortCheque cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new IikoCardShortCheque(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IIikoCardShortCheque>(copy, "IikoCardShortCheque");
        }
    }
}
