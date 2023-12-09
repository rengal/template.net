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
    /// Товарный чек
    /// </summary>
    public interface ICashMemo
    {
        IChequeSettings Settings { get; }
        ICashChequeInfo Info { get; }
        int OrderNumber { get; }
        /// <summary>
        /// Имя официанта
        /// </summary>
        string WaiterName { get; }
        /// <summary>
        /// Номер стола
        /// </summary>
        int TableNumber { get; }
        int ItemsCount { get; }
        /// <summary>
        /// Сдача
        /// </summary>
        decimal ChangeSum { get; }
        /// <summary>
        /// Итоговая сумма
        /// </summary>
        decimal ResultSum { get; }
        /// <summary>
        /// Итоговая сумма без НДС
        /// </summary>
        decimal VatSum { get; }
        /// <summary>
        /// Подытог
        /// </summary>
        decimal SubTotal { get; }
        IEnumerable<ICashMemoChequePaymentItem> Prepays { get; }
        IEnumerable<ICashMemoChequePaymentItem> Payments { get; }
        IEnumerable<ICashMemoChequeProductItem> Products { get; }
        IEnumerable<ICashMemoChequeDiscountItem> Discounts { get; }
        IEnumerable<ICashMemoChequeVatItem> Vats { get; }
    }

    public interface ICashMemoChequePaymentItem
    {
        /// <summary>
        /// Название типа оплаты
        /// </summary>
        string Name { get; }
        decimal Sum { get; }
    }

    /// <summary>
    /// Элемент заказа
    /// </summary>
    public interface ICashMemoChequeProductItem
    {
        /// <summary>
        /// Название товара
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Цена за единицу количества
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Название единицы измерения
        /// </summary>
        string MeasuringUnitName { get; }
        IEnumerable<ICashMemoChequeDiscountItem> Discounts { get; }
        IEnumerable<ICashMemoChequeModifierItem> Modifiers { get; }
        /// <summary>
        /// Есть комментарий
        /// </summary>
        bool CommentExists { get; }
        /// <summary>
        /// Комментарий к блюду
        /// </summary>
        string Comment { get; }
    }

    /// <summary>
    /// Модификатор товара
    /// </summary>
    public interface ICashMemoChequeModifierItem
    {
        /// <summary>
        /// Название модификатора
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Цена за единицу количества
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Название единицы измерения
        /// </summary>
        string MeasuringUnitName { get; }
    }

    /// <summary>
    /// Скидка
    /// </summary>
    public interface ICashMemoChequeDiscountItem
    {
        /// <summary>
        /// Название скидки
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Номер скидочной карты, если есть
        /// </summary>
        string CardNumber { get; }
        /// <summary>
        /// Сумма скидки
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Процент скидки
        /// </summary>
        decimal Percent { get; }
    }

    /// <summary>
    /// Налог, не включенный в стоимость блюда
    /// </summary>
    public interface ICashMemoChequeVatItem
    {
        /// <summary>
        /// Процент налога
        /// </summary>
        decimal VatPercent { get; }
        /// <summary>
        /// Процент сумма
        /// </summary>
        decimal VatSum { get; }
    }

    public static class CashMemoChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this ICashMemo obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "CashMemo");
            
            return document;
        }
        
        internal static void AppendToXml(this ICashMemo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            container.Add(new XAttribute("OrderNumber", obj.OrderNumber));
                
            if (obj.WaiterName != null)
                container.Add(new XAttribute("WaiterName", obj.WaiterName));
                
            container.Add(new XAttribute("TableNumber", obj.TableNumber));
                
            container.Add(new XAttribute("ItemsCount", obj.ItemsCount));
                
            container.Add(new XAttribute("ChangeSum", obj.ChangeSum));
                
            container.Add(new XAttribute("ResultSum", obj.ResultSum));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            container.Add(new XAttribute("SubTotal", obj.SubTotal));
                
            if (obj.Prepays != null && obj.Prepays.Any(o => o != null))
            {
                var prepaysContainer = new XElement("Prepays");
                foreach (var item in obj.Prepays)
                    item.AppendToXml(prepaysContainer, "Prepay");
                container.Add(prepaysContainer);
            }
            
            if (obj.Payments != null && obj.Payments.Any(o => o != null))
            {
                var paymentsContainer = new XElement("Payments");
                foreach (var item in obj.Payments)
                    item.AppendToXml(paymentsContainer, "Payment");
                container.Add(paymentsContainer);
            }
            
            if (obj.Products != null && obj.Products.Any(o => o != null))
            {
                var productsContainer = new XElement("Products");
                foreach (var item in obj.Products)
                    item.AppendToXml(productsContainer, "Product");
                container.Add(productsContainer);
            }
            
            if (obj.Discounts != null && obj.Discounts.Any(o => o != null))
            {
                var discountsContainer = new XElement("Discounts");
                foreach (var item in obj.Discounts)
                    item.AppendToXml(discountsContainer, "Discount");
                container.Add(discountsContainer);
            }
            
            if (obj.Vats != null && obj.Vats.Any(o => o != null))
            {
                var vatsContainer = new XElement("Vats");
                foreach (var item in obj.Vats)
                    item.AppendToXml(vatsContainer, "Vat");
                container.Add(vatsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICashMemoChequePaymentItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICashMemoChequeProductItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.MeasuringUnitName != null)
                container.Add(new XAttribute("MeasuringUnitName", obj.MeasuringUnitName));
                
            if (obj.Discounts != null && obj.Discounts.Any(o => o != null))
            {
                var discountsContainer = new XElement("Discounts");
                foreach (var item in obj.Discounts)
                    item.AppendToXml(discountsContainer, "Discount");
                container.Add(discountsContainer);
            }
            
            if (obj.Modifiers != null && obj.Modifiers.Any(o => o != null))
            {
                var modifiersContainer = new XElement("Modifiers");
                foreach (var item in obj.Modifiers)
                    item.AppendToXml(modifiersContainer, "Modifier");
                container.Add(modifiersContainer);
            }
            
            container.Add(new XAttribute("CommentExists", obj.CommentExists));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICashMemoChequeModifierItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.MeasuringUnitName != null)
                container.Add(new XAttribute("MeasuringUnitName", obj.MeasuringUnitName));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICashMemoChequeDiscountItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.CardNumber != null)
                container.Add(new XAttribute("CardNumber", obj.CardNumber));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("Percent", obj.Percent));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ICashMemoChequeVatItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("VatPercent", obj.VatPercent));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            parent.Add(container);
        }
        
        
    }
}
