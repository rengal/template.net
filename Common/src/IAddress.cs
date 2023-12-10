using Resto.Framework.Attributes.JetBrains;

// ReSharper disable once CheckNamespace
namespace Resto.Data
{
    /// <summary>
    /// Адрес.
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        /// Line1 - Часть адреса, по которой можно найти точку при геокодировании (город, улица, номер дома, корпус).
        /// </summary>
        [CanBeNull]
        string Line1 { get; set; }

        /// <summary>
        /// Line2 - Часть адреса, по которой можно точно найти клиента на найденном объекте (подъезд, этаж, квартира и т.д.).
        /// </summary>
        [CanBeNull]
        string Line2 { get; set; }

        /// <summary>
        /// Номер дома.
        /// </summary>
        string House { get; set; }

        /// <summary>
        /// Корпус/строение.
        /// </summary>
        string Building { get; set; }

        /// <summary>
        /// Номер квартиры.
        /// </summary>
        string Flat { get; set; }

        /// <summary>
        /// Подъезд.
        /// </summary>
        string Entrance { get; set; }

        /// <summary>
        /// Этаж.
        /// </summary>
        string Floor { get; set; }

        /// <summary>
        /// Домофон.
        /// </summary>
        string Doorphone { get; set; }

        /// <summary>
        /// Идентификатор адреса во внешней системе картографии.
        /// </summary>
        string ExternalCartographyId { get; set; }

        /// <summary>
        /// Дополнительная информация по адресу (подъезд, этаж, с какой стороны вход и т.п.).
        /// </summary>
        string AdditionalInfo { get; set; }

        /// <summary>
        /// Улица.
        /// </summary>
        Street Street { get; set; }

        /// <summary>
        /// Регион.
        /// </summary>
        Region Region { get; set; }

        /// <summary>
        /// Индекс. Он же используется как Postcode для адресации UK.
        /// </summary>
        string Index { get; set; }
    }
}