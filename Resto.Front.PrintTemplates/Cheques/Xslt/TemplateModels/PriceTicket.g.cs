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
    /// Этикетка
    /// </summary>
    public interface IPriceTicket
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Наименование номерклатуры
        /// </summary>
        string ProductName { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Стоимость
        /// </summary>
        decimal Cost { get; }
        /// <summary>
        /// Единица измерения
        /// </summary>
        string UnitName { get; }
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
        /// Срок хранения
        /// </summary>
        TimeSpan ExpirationPeriod { get; }
        /// <summary>
        /// Годен до
        /// </summary>
        DateTime BestBeforeTime { get; }
        /// <summary>
        /// Время изготовления
        /// </summary>
        DateTime MakeTime { get; }
        /// <summary>
        /// Штрихкод
        /// </summary>
        string BarcodeString { get; }
    }

    /// <summary>
    /// Пищевая ценность
    /// </summary>
    public interface IFoodValue
    {
        /// <summary>
        /// Заполнено ли
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// Жиры
        /// </summary>
        decimal Fat { get; }
        /// <summary>
        /// Белки
        /// </summary>
        decimal Protein { get; }
        /// <summary>
        /// Углеводы
        /// </summary>
        decimal Carbohydrate { get; }
        /// <summary>
        /// Калорийность
        /// </summary>
        decimal Caloricity { get; }
    }

    public static class PriceTicketToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IPriceTicket obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "PriceTicket");
            
            return document;
        }
        
        internal static void AppendToXml(this IPriceTicket obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.ProductName != null)
                container.Add(new XAttribute("ProductName", obj.ProductName));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            container.Add(new XAttribute("Cost", obj.Cost));
                
            if (obj.UnitName != null)
                container.Add(new XAttribute("UnitName", obj.UnitName));
                
            if (obj.Composition != null)
                container.Add(new XAttribute("Composition", obj.Composition));
                
            if (obj.FoodValue != null)
                obj.FoodValue.AppendToXml(container, "FoodValue");
            
            container.Add(new XAttribute("IsExpirationPeriodSet", obj.IsExpirationPeriodSet));
                
            container.Add(new XAttribute("ExpirationPeriod", obj.ExpirationPeriod.ToString()));
                
            container.Add(new XAttribute("BestBeforeTime", obj.BestBeforeTime));
                
            container.Add(new XAttribute("MakeTime", obj.MakeTime));
                
            if (obj.BarcodeString != null)
                container.Add(new XAttribute("BarcodeString", obj.BarcodeString));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IFoodValue obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("IsEmpty", obj.IsEmpty));
                
            container.Add(new XAttribute("Fat", obj.Fat));
                
            container.Add(new XAttribute("Protein", obj.Protein));
                
            container.Add(new XAttribute("Carbohydrate", obj.Carbohydrate));
                
            container.Add(new XAttribute("Caloricity", obj.Caloricity));
                
            parent.Add(container);
        }
        
        
    }
}
