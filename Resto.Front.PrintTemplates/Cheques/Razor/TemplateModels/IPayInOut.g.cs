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
    /// Внесение/изъятие
    /// </summary>
    public interface IPayInOut : ITemplateRootModel
    {
        /// <summary>
        /// Общая информация
        /// </summary>
        [NotNull]
        IChequeCommonInfo CommonInfo { get; }

        /// <summary>
        /// Информация о кассовом чеке
        /// </summary>
        [NotNull]
        ICashChequeInfo ChequeInfo { get; }

        /// <summary>
        /// Событие внесения/изъятия
        /// </summary>
        [NotNull]
        IPayInOutEvent Event { get; }

    }

    internal sealed class PayInOut : TemplateModelBase, IPayInOut
    {
        #region Fields
        private readonly ChequeCommonInfo commonInfo;
        private readonly CashChequeInfo chequeInfo;
        private readonly PayInOutEvent @event;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private PayInOut()
        {}

        internal PayInOut([NotNull] CopyContext context, [NotNull] IPayInOut src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            commonInfo = context.GetConverted(src.CommonInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.ChequeCommonInfo.Convert);
            chequeInfo = context.GetConverted(src.ChequeInfo, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.CashChequeInfo.Convert);
            @event = context.GetConverted(src.Event, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.PayInOutEvent.Convert);
        }

        #endregion

        #region Props
        public IChequeCommonInfo CommonInfo
        {
            get { return commonInfo; }
        }

        public ICashChequeInfo ChequeInfo
        {
            get { return chequeInfo; }
        }

        public IPayInOutEvent Event
        {
            get { return @event; }
        }

        #endregion
    }

    public static partial class ChequeSerializer
    {
        [NotNull]
        public static string Serialize([NotNull] this IPayInOut cheque)
        {
            if (cheque == null)
                throw new ArgumentNullException("cheque");

            var context = new CopyContext();
            var copy = new PayInOut(context, cheque);

            return Resto.Framework.Common.XmlSerialization.Serializer.GetXmlText<IPayInOut>(copy, "PayInOut");
        }
    }
}
