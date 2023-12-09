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
    /// Клиент
    /// </summary>
    public interface ICustomer
    {
        /// <summary>
        /// Имя
        /// </summary>
        [NotNull]
        string Name { get; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [CanBeNull]
        string Surname { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Номер карты
        /// </summary>
        [CanBeNull]
        string CardNumber { get; }

        /// <summary>
        /// Тип скидки по карте клиента
        /// </summary>
        [CanBeNull]
        [Obsolete("Свойство всегда возвращает null, так как соответствующее значение удалено из модели данных")]
        IDiscountType CardDiscountType { get; }

        /// <summary>
        /// Клиент в статусе "высокий риск"
        /// </summary>
        bool InBlackList { get; }

        /// <summary>
        /// Причина внесения клиента в статус "высокий риск"
        /// </summary>
        [CanBeNull]
        string BlackListReason { get; }

        /// <summary>
        /// Телефоны
        /// </summary>
        [NotNull]
        IEnumerable<IPhone> Phones { get; }

        /// <summary>
        /// Адреса
        /// </summary>
        [NotNull]
        IEnumerable<IAddress> Addresses { get; }

        /// <summary>
        /// Адреса электронной почты
        /// </summary>
        [NotNull]
        IEnumerable<IEmail> Emails { get; }

    }

    internal sealed class Customer : TemplateModelBase, ICustomer
    {
        #region Fields
        private readonly string name;
        private readonly string surname;
        private readonly string comment;
        private readonly string cardNumber;
        private readonly DiscountType cardDiscountType;
        private readonly bool inBlackList;
        private readonly string blackListReason;
        private readonly List<Phone> phones = new List<Phone>();
        private readonly List<Address> addresses = new List<Address>();
        private readonly List<Email> emails = new List<Email>();
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Customer()
        {}

        private Customer([NotNull] CopyContext context, [NotNull] ICustomer src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            name = src.Name;
            surname = src.Surname;
            comment = src.Comment;
            cardNumber = src.CardNumber;
            cardDiscountType = context.GetConverted(src.CardDiscountType, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.DiscountType.Convert);
            inBlackList = src.InBlackList;
            blackListReason = src.BlackListReason;
            phones = src.Phones.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Phone.Convert)).ToList();
            addresses = src.Addresses.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Address.Convert)).ToList();
            emails = src.Emails.Select(i => context.GetConverted(i, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Email.Convert)).ToList();
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Customer Convert([NotNull] CopyContext context, [CanBeNull] ICustomer source)
        {
            if (source == null)
                return null;

            return new Customer(context, source);
        }
        #endregion

        #region Props
        public string Name
        {
            get { return GetLocalizedValue(name); }
        }

        public string Surname
        {
            get { return GetLocalizedValue(surname); }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public string CardNumber
        {
            get { return GetLocalizedValue(cardNumber); }
        }

        public IDiscountType CardDiscountType
        {
            get { return cardDiscountType; }
        }

        public bool InBlackList
        {
            get { return inBlackList; }
        }

        public string BlackListReason
        {
            get { return GetLocalizedValue(blackListReason); }
        }

        public IEnumerable<IPhone> Phones
        {
            get { return phones; }
        }

        public IEnumerable<IAddress> Addresses
        {
            get { return addresses; }
        }

        public IEnumerable<IEmail> Emails
        {
            get { return emails; }
        }

        #endregion
    }

}
