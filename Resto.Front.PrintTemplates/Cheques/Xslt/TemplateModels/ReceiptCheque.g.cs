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
    /// Оплата/возврат
    /// </summary>
    public interface IReceipt
    {
        IChequeSettings Settings { get; }
        ICashChequeInfo Info { get; }
        bool IsFullCheque { get; }
        bool IsStorno { get; }
        int OrderNumber { get; }
        decimal Sum { get; }
        /// <summary>
        /// Итоговая сумма НДС
        /// </summary>
        decimal VatSum { get; }
        decimal ResultSum { get; }
        decimal PaymentsSum { get; }
        decimal ChangeSum { get; }
        bool HasMultiplePayments { get; }
        IEnumerable<IReceiptSale> Sales { get; }
        IEnumerable<INonCategorialDiscountIncrease> Increases { get; }
        IEnumerable<INonCategorialDiscountIncrease> Discounts { get; }
        IEnumerable<IReceiptPayment> Prepays { get; }
        IEnumerable<IReceiptPayment> Payments { get; }
        IEnumerable<XElement> BeforeCheque { get; }
        IEnumerable<XElement> AfterCheque { get; }
        /// <summary>
        /// Номер стола
        /// </summary>
        int TableNumber { get; }
        /// <summary>
        /// Название отделения
        /// </summary>
        string SectionName { get; }
        /// <summary>
        /// Количество гостей
        /// </summary>
        int GuestsAmount { get; }
        /// <summary>
        /// Имя официанта
        /// </summary>
        string WaiterName { get; }
        IEnumerable<IReceiptVat> Vats { get; }
    }

    /// <summary>
    /// Некатегориальная скидка/надбавка
    /// </summary>
    public interface INonCategorialDiscountIncrease
    {
        ISimpleCorrectionItem SimpleCorrectionItem { get; }
        IDiscountCardItem DiscountCardItem { get; }
    }

    /// <summary>
    /// Ручная скидка/надбавка
    /// </summary>
    public interface ISimpleCorrectionItem
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Процент
        /// </summary>
        decimal Percent { get; }
        /// <summary>
        /// Значение
        /// </summary>
        decimal Price { get; }
    }

    /// <summary>
    /// Скидка/надбавка по карте
    /// </summary>
    public interface IDiscountCardItem
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Процент
        /// </summary>
        decimal Percent { get; }
        /// <summary>
        /// Значение
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Номер карты
        /// </summary>
        string CardNumber { get; }
    }

    /// <summary>
    /// Элемент продаж
    /// </summary>
    public interface IReceiptSale
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Цена
        /// </summary>
        decimal Price { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Процент надбавки
        /// </summary>
        decimal Increase { get; }
        /// <summary>
        /// Сумма надбавки
        /// </summary>
        decimal IncreaseSum { get; }
        /// <summary>
        /// Процент скидки
        /// </summary>
        decimal Discount { get; }
        /// <summary>
        /// Сумма скидки
        /// </summary>
        decimal DiscountSum { get; }
    }

    /// <summary>
    /// Платёж
    /// </summary>
    public interface IReceiptPayment
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Коментарий
        /// </summary>
        string Comment { get; }
        /// <summary>
        /// Фискальный ли тип
        /// </summary>
        bool IsFiscal { get; }
        /// <summary>
        /// Номер чека для платежа
        /// </summary>
        int ChequeNumber { get; }
    }

    /// <summary>
    /// налог, не включенный в стоимость блюда
    /// </summary>
    public interface IReceiptVat
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

    public static class ReceiptChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IReceipt obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "Receipt");
            
            return document;
        }
        
        internal static void AppendToXml(this IReceipt obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Info != null)
                obj.Info.AppendToXml(container, "Info");
            
            container.Add(new XAttribute("IsFullCheque", obj.IsFullCheque));
                
            container.Add(new XAttribute("IsStorno", obj.IsStorno));
                
            container.Add(new XAttribute("OrderNumber", obj.OrderNumber));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            container.Add(new XAttribute("ResultSum", obj.ResultSum));
                
            container.Add(new XAttribute("PaymentsSum", obj.PaymentsSum));
                
            container.Add(new XAttribute("ChangeSum", obj.ChangeSum));
                
            container.Add(new XAttribute("HasMultiplePayments", obj.HasMultiplePayments));
                
            if (obj.Sales != null && obj.Sales.Any(o => o != null))
            {
                var salesContainer = new XElement("Sales");
                foreach (var item in obj.Sales)
                    item.AppendToXml(salesContainer, "Sale");
                container.Add(salesContainer);
            }
            
            if (obj.Increases != null && obj.Increases.Any(o => o != null))
            {
                var increasesContainer = new XElement("Increases");
                foreach (var item in obj.Increases)
                    item.AppendToXml(increasesContainer, "DiscountIncrease");
                container.Add(increasesContainer);
            }
            
            if (obj.Discounts != null && obj.Discounts.Any(o => o != null))
            {
                var discountsContainer = new XElement("Discounts");
                foreach (var item in obj.Discounts)
                    item.AppendToXml(discountsContainer, "DiscountIncrease");
                container.Add(discountsContainer);
            }
            
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
            
            if (obj.BeforeCheque != null && obj.BeforeCheque.Any(o => o != null))
            {
                var beforeChequeContainer = new XElement("BeforeCheque");
                foreach (var item in obj.BeforeCheque)
                    beforeChequeContainer.Add(item);
                container.Add(beforeChequeContainer);
            }
            
            if (obj.AfterCheque != null && obj.AfterCheque.Any(o => o != null))
            {
                var afterChequeContainer = new XElement("AfterCheque");
                foreach (var item in obj.AfterCheque)
                    afterChequeContainer.Add(item);
                container.Add(afterChequeContainer);
            }
            
            container.Add(new XAttribute("TableNumber", obj.TableNumber));
                
            if (obj.SectionName != null)
                container.Add(new XAttribute("SectionName", obj.SectionName));
                
            container.Add(new XAttribute("GuestsAmount", obj.GuestsAmount));
                
            if (obj.WaiterName != null)
                container.Add(new XAttribute("WaiterName", obj.WaiterName));
                
            if (obj.Vats != null && obj.Vats.Any(o => o != null))
            {
                var vatsContainer = new XElement("Vats");
                foreach (var item in obj.Vats)
                    item.AppendToXml(vatsContainer, "ReceiptVat");
                container.Add(vatsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this INonCategorialDiscountIncrease obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.SimpleCorrectionItem != null)
                obj.SimpleCorrectionItem.AppendToXml(container, "SimpleCorrectionItem");
            
            if (obj.DiscountCardItem != null)
                obj.DiscountCardItem.AppendToXml(container, "DiscountCardItem");
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ISimpleCorrectionItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Percent", obj.Percent));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDiscountCardItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Percent", obj.Percent));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            if (obj.CardNumber != null)
                container.Add(new XAttribute("CardNumber", obj.CardNumber));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IReceiptSale obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Price", obj.Price));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("Increase", obj.Increase));
                
            container.Add(new XAttribute("IncreaseSum", obj.IncreaseSum));
                
            container.Add(new XAttribute("Discount", obj.Discount));
                
            container.Add(new XAttribute("DiscountSum", obj.DiscountSum));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IReceiptPayment obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            container.Add(new XAttribute("IsFiscal", obj.IsFiscal));
                
            container.Add(new XAttribute("ChequeNumber", obj.ChequeNumber));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IReceiptVat obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("VatPercent", obj.VatPercent));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            parent.Add(container);
        }
        
        
    }
}
