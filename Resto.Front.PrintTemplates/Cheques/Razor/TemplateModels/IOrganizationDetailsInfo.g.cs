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
    /// Информация об организации
    /// </summary>
    public interface IOrganizationDetailsInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// ИНН
        /// </summary>
        [CanBeNull]
        string Code { get; }

        /// <summary>
        /// Код причины постановки на учет (КПП) / Фискальный код
        /// </summary>
        [CanBeNull]
        string AccountingReasonCode { get; }

        /// <summary>
        /// Код получателя
        /// </summary>
        [CanBeNull]
        string RecipientCode { get; }

        /// <summary>
        /// Адрес одной строкой
        /// </summary>
        [CanBeNull]
        string Address { get; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        [CanBeNull]
        string AddressPostCode { get; }

        /// <summary>
        /// Страна
        /// </summary>
        [CanBeNull]
        string AddressCountry { get; }

        /// <summary>
        /// Регион
        /// </summary>
        [CanBeNull]
        string AddressRegion { get; }

        /// <summary>
        /// Город
        /// </summary>
        [CanBeNull]
        string AddressCity { get; }

        /// <summary>
        /// Улица
        /// </summary>
        [CanBeNull]
        string AddressStreet { get; }

        /// <summary>
        /// Дом, корпус или строение
        /// </summary>
        [CanBeNull]
        string AddressHouse { get; }

    }

    internal sealed class OrganizationDetailsInfo : TemplateModelBase, IOrganizationDetailsInfo
    {
        #region Fields
        private readonly string name;
        private readonly string code;
        private readonly string accountingReasonCode;
        private readonly string recipientCode;
        private readonly string address;
        private readonly string addressPostCode;
        private readonly string addressCountry;
        private readonly string addressRegion;
        private readonly string addressCity;
        private readonly string addressStreet;
        private readonly string addressHouse;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private OrganizationDetailsInfo()
        {}

        private OrganizationDetailsInfo([NotNull] CopyContext context, [NotNull] IOrganizationDetailsInfo src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            code = src.Code;
            accountingReasonCode = src.AccountingReasonCode;
            recipientCode = src.RecipientCode;
            address = src.Address;
            addressPostCode = src.AddressPostCode;
            addressCountry = src.AddressCountry;
            addressRegion = src.AddressRegion;
            addressCity = src.AddressCity;
            addressStreet = src.AddressStreet;
            addressHouse = src.AddressHouse;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static OrganizationDetailsInfo Convert([NotNull] CopyContext context, [CanBeNull] IOrganizationDetailsInfo source)
        {
            if (source == null)
                return null;

            return new OrganizationDetailsInfo(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string Code
        {
            get { return GetLocalizedValue(code); }
        }

        public string AccountingReasonCode
        {
            get { return GetLocalizedValue(accountingReasonCode); }
        }

        public string RecipientCode
        {
            get { return GetLocalizedValue(recipientCode); }
        }

        public string Address
        {
            get { return GetLocalizedValue(address); }
        }

        public string AddressPostCode
        {
            get { return GetLocalizedValue(addressPostCode); }
        }

        public string AddressCountry
        {
            get { return GetLocalizedValue(addressCountry); }
        }

        public string AddressRegion
        {
            get { return GetLocalizedValue(addressRegion); }
        }

        public string AddressCity
        {
            get { return GetLocalizedValue(addressCity); }
        }

        public string AddressStreet
        {
            get { return GetLocalizedValue(addressStreet); }
        }

        public string AddressHouse
        {
            get { return GetLocalizedValue(addressHouse); }
        }

        #endregion
    }

}
