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
    /// Адрес
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        /// Куда везти.
        /// </summary>
        [NotNull]
        string Line1 { get; }

        /// <summary>
        /// Line2
        /// </summary>
        [NotNull]
        string Line2 { get; }

        /// <summary>
        /// Регион
        /// </summary>
        [CanBeNull]
        string Region { get; }

        /// <summary>
        /// Город
        /// </summary>
        [NotNull]
        string City { get; }

        /// <summary>
        /// Улица
        /// </summary>
        [NotNull]
        string Street { get; }

        /// <summary>
        /// Дом
        /// </summary>
        [NotNull]
        string House { get; }

        /// <summary>
        /// Здание/корпус
        /// </summary>
        [CanBeNull]
        string Building { get; }

        /// <summary>
        /// Подъезд
        /// </summary>
        [CanBeNull]
        string Entrance { get; }

        /// <summary>
        /// Этаж
        /// </summary>
        [CanBeNull]
        string Floor { get; }

        /// <summary>
        /// Квартира
        /// </summary>
        [CanBeNull]
        string Flat { get; }

        /// <summary>
        /// Домофон
        /// </summary>
        [CanBeNull]
        string Doorphone { get; }

        /// <summary>
        /// Индекс
        /// </summary>
        [CanBeNull]
        string Index { get; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        [CanBeNull]
        string AdditionalInfo { get; }

        /// <summary>
        /// Адресая система ОАЭ
        /// </summary>
        bool UaeAddressingSystem { get; }

    }

    internal sealed class Address : TemplateModelBase, IAddress
    {
        #region Fields
        private readonly string line1;
        private readonly string line2;
        private readonly string region;
        private readonly string city;
        private readonly string street;
        private readonly string house;
        private readonly string building;
        private readonly string entrance;
        private readonly string floor;
        private readonly string flat;
        private readonly string doorphone;
        private readonly string index;
        private readonly string additionalInfo;
        private readonly bool uaeAddressingSystem;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Address()
        {}

        private Address([NotNull] CopyContext context, [NotNull] IAddress src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            line1 = src.Line1;
            line2 = src.Line2;
            region = src.Region;
            city = src.City;
            street = src.Street;
            house = src.House;
            building = src.Building;
            entrance = src.Entrance;
            floor = src.Floor;
            flat = src.Flat;
            doorphone = src.Doorphone;
            index = src.Index;
            additionalInfo = src.AdditionalInfo;
            uaeAddressingSystem = src.UaeAddressingSystem;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Address Convert([NotNull] CopyContext context, [CanBeNull] IAddress source)
        {
            if (source == null)
                return null;

            return new Address(context, source);
        }
        #endregion

        #region Props
        public string Line1
        {
            get { return GetLocalizedValue(line1); }
        }

        public string Line2
        {
            get { return GetLocalizedValue(line2); }
        }

        public string Region
        {
            get { return GetLocalizedValue(region); }
        }

        public string City
        {
            get { return GetLocalizedValue(city); }
        }

        public string Street
        {
            get { return GetLocalizedValue(street); }
        }

        public string House
        {
            get { return GetLocalizedValue(house); }
        }

        public string Building
        {
            get { return GetLocalizedValue(building); }
        }

        public string Entrance
        {
            get { return GetLocalizedValue(entrance); }
        }

        public string Floor
        {
            get { return GetLocalizedValue(floor); }
        }

        public string Flat
        {
            get { return GetLocalizedValue(flat); }
        }

        public string Doorphone
        {
            get { return GetLocalizedValue(doorphone); }
        }

        public string Index
        {
            get { return GetLocalizedValue(index); }
        }

        public string AdditionalInfo
        {
            get { return GetLocalizedValue(additionalInfo); }
        }

        public bool UaeAddressingSystem
        {
            get { return uaeAddressingSystem; }
        }

        #endregion
    }

}
