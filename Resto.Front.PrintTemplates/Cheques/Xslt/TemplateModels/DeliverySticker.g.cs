// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective
// ReSharper disable PartialTypeWithSinglePart

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Xslt.TemplateModels
{
    /// <summary>
    /// Стикер
    /// </summary>
    public interface IDeliverySticker
    {
        /// <summary>
        /// Настройки чеков
        /// </summary>
        IChequeSettings Settings { get; }
        /// <summary>
        /// Номер доставки
        /// </summary>
        int DeliveryNumber { get; }
        /// <summary>
        /// Время доставки
        /// </summary>
        DateTime DeliverTime { get; }
        /// <summary>
        /// Блюдо
        /// </summary>
        IDeliveryStickerOrderItemInfo OrderItem { get; }
    }

    public interface IDeliveryStickerOrderEntryInfo
    {
        /// <summary>
        /// Наименование номерклатуры
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
    }

    public interface IDeliveryStickerOrderItemInfo
    {
        /// <summary>
        /// Наименование номенклатуры
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Имя гостя
        /// </summary>
        string GuestName { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Категория
        /// </summary>
        string CategoryName { get; }
        /// <summary>
        /// Весовое ли блюдо
        /// </summary>
        bool UseBalanceForSell { get; }
        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Состав
        /// </summary>
        string Composition { get; }
        /// <summary>
        /// Пищевая ценность
        /// </summary>
        IFoodValue FoodValue { get; }
        /// <summary>
        /// Задан ли срок хранения
        /// </summary>
        bool IsExpirationPeriodSet { get; }
        /// <summary>
        /// Срок хранения в часах
        /// </summary>
        string ExpirationPeriodHours { get; }
        /// <summary>
        /// Модификаторы
        /// </summary>
        IEnumerable<IDeliveryStickerOrderEntryInfo> Modifiers { get; }
        /// <summary>
        /// Время сервисной печати
        /// </summary>
        DateTime PrintTime { get; }
    }

    public static class DeliveryStickerToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IDeliverySticker obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "DeliverySticker");
            
            return document;
        }
        
        internal static void AppendToXml(this IDeliverySticker obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            container.Add(new XAttribute("DeliveryNumber", obj.DeliveryNumber));
                
            container.Add(new XAttribute("DeliverTime", obj.DeliverTime));
                
            if (obj.OrderItem != null)
                obj.OrderItem.AppendToXml(container, "OrderItem");
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryStickerOrderEntryInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryStickerOrderItemInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.GuestName != null)
                container.Add(new XAttribute("GuestName", obj.GuestName));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            if (obj.CategoryName != null)
                container.Add(new XAttribute("CategoryName", obj.CategoryName));
                
            container.Add(new XAttribute("UseBalanceForSell", obj.UseBalanceForSell));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            if (obj.Composition != null)
                container.Add(new XAttribute("Composition", obj.Composition));
                
            if (obj.FoodValue != null)
                obj.FoodValue.AppendToXml(container, "FoodValue");
            
            container.Add(new XAttribute("IsExpirationPeriodSet", obj.IsExpirationPeriodSet));
                
            if (obj.ExpirationPeriodHours != null)
                container.Add(new XAttribute("ExpirationPeriodHours", obj.ExpirationPeriodHours));
                
            if (obj.Modifiers != null && obj.Modifiers.Any(o => o != null))
            {
                var modifiersContainer = new XElement("Modifiers");
                foreach (var item in obj.Modifiers)
                    item.AppendToXml(modifiersContainer, "Modifier");
                container.Add(modifiersContainer);
            }
            
            container.Add(new XAttribute("PrintTime", obj.PrintTime));
                
            parent.Add(container);
        }
        
        
    }
}
