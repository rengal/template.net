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
    public interface IDeliveryBill
    {
        IChequeSettings Settings { get; }
        IDeliveryInfo DeliveryInfo { get; }
        IDeliveryOrderInfo DeliveryOrderInfo { get; }
        IBillFooter BillFooter { get; }
    }

    public interface IDeliveryInfo
    {
        int DeliveryNumber { get; }
        string RestourantLegalName { get; }
        string TaxId { get; }
        string CustomerNameAndPatronymic { get; }
        string CustomerSurname { get; }
        string PhoneNumber { get; }
        string Address { get; }
        string Region { get; }
        string AdressComment { get; }
        string Comments { get; }
        string DiscountCardNumber { get; }
        string Manager { get; }
        string Courier { get; }
        string DeliveryOperator { get; }
        DateTime DeliveryReceiveTime { get; }
        DateTime DeliveryBillPrintTime { get; }
        DateTime DeliverTime { get; }
        bool IsPayed { get; }
        bool IsSelfService { get; }
        bool SplitBetweenPersons { get; }
        int PersonsCount { get; }
    }

    public interface IDeliveryOrderInfo
    {
        ITotalSumsInfo TotalSumsInfo { get; }
        /// <summary>
        /// Гости
        /// </summary>
        IEnumerable<IDeliveryGuest> EntriesByGuests { get; }
        IEnumerable<IDiscountInfo> Discounts { get; }
        IEnumerable<IPaymentInfo> Prepays { get; }
        IEnumerable<IPaymentInfo> Payments { get; }
        IEnumerable<IVatInfo> Vats { get; }
    }

    public interface IDeliveryOrderEntryInfo
    {
        bool IsModifier { get; }
        bool IsPrechequePrintable { get; }
        string Name { get; }
        decimal Amount { get; }
        decimal Price { get; }
        decimal Sum { get; }
        string MeasuringUnitName { get; }
        IEnumerable<IDiscountInfo> Discounts { get; }
    }

    /// <summary>
    /// Гость
    /// </summary>
    public interface IDeliveryGuest
    {
        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Итого по гостю с учётом всех скидок/надбавок
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Заказанные блюда
        /// </summary>
        IEnumerable<IDeliveryOrderEntryInfo> DeliveryItems { get; }
    }

    public interface ITotalSumsInfo
    {
        decimal SubTotal { get; }
        decimal ResultSum { get; }
        decimal ChangeSum { get; }
        decimal VatSum { get; }
        string AllSumsCurrencyUnitLabel { get; }
    }

    public interface IDiscountInfo
    {
        string Name { get; }
        decimal Percent { get; }
        decimal Sum { get; }
        string CardNumber { get; }
    }

    public interface IPaymentInfo
    {
        string Name { get; }
        decimal Sum { get; }
        bool IsCash { get; }
        bool IsCard { get; }
        string CounteragentName { get; }
        string CounteragentCardNumber { get; }
    }

    /// <summary>
    /// налог, не включенный в стоимость блюда
    /// </summary>
    public interface IVatInfo
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

    public interface IBillFooter
    {
        string Value { get; }
    }

    public static class DeliveryBillChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IDeliveryBill obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "DeliveryBill");
            
            return document;
        }
        
        internal static void AppendToXml(this IDeliveryBill obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.DeliveryInfo != null)
                obj.DeliveryInfo.AppendToXml(container, "DeliveryInfo");
            
            if (obj.DeliveryOrderInfo != null)
                obj.DeliveryOrderInfo.AppendToXml(container, "DeliveryOrderInfo");
            
            if (obj.BillFooter != null)
                obj.BillFooter.AppendToXml(container, "BillFooter");
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("DeliveryNumber", obj.DeliveryNumber));
                
            if (obj.RestourantLegalName != null)
                container.Add(new XAttribute("RestourantLegalName", obj.RestourantLegalName));
                
            if (obj.TaxId != null)
                container.Add(new XAttribute("TaxId", obj.TaxId));
                
            if (obj.CustomerNameAndPatronymic != null)
                container.Add(new XAttribute("CustomerNameAndPatronymic", obj.CustomerNameAndPatronymic));
                
            if (obj.CustomerSurname != null)
                container.Add(new XAttribute("CustomerSurname", obj.CustomerSurname));
                
            if (obj.PhoneNumber != null)
                container.Add(new XAttribute("PhoneNumber", obj.PhoneNumber));
                
            if (obj.Address != null)
                container.Add(new XAttribute("Address", obj.Address));
                
            if (obj.Region != null)
                container.Add(new XAttribute("Region", obj.Region));
                
            if (obj.AdressComment != null)
                container.Add(new XAttribute("AdressComment", obj.AdressComment));
                
            if (obj.Comments != null)
                container.Add(new XAttribute("Comments", obj.Comments));
                
            if (obj.DiscountCardNumber != null)
                container.Add(new XAttribute("DiscountCardNumber", obj.DiscountCardNumber));
                
            if (obj.Manager != null)
                container.Add(new XAttribute("Manager", obj.Manager));
                
            if (obj.Courier != null)
                container.Add(new XAttribute("Courier", obj.Courier));
                
            if (obj.DeliveryOperator != null)
                container.Add(new XAttribute("DeliveryOperator", obj.DeliveryOperator));
                
            container.Add(new XAttribute("DeliveryReceiveTime", obj.DeliveryReceiveTime));
                
            container.Add(new XAttribute("DeliveryBillPrintTime", obj.DeliveryBillPrintTime));
                
            container.Add(new XAttribute("DeliverTime", obj.DeliverTime));
                
            container.Add(new XAttribute("IsPayed", obj.IsPayed));
                
            container.Add(new XAttribute("IsSelfService", obj.IsSelfService));
                
            container.Add(new XAttribute("SplitBetweenPersons", obj.SplitBetweenPersons));
                
            container.Add(new XAttribute("PersonsCount", obj.PersonsCount));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryOrderInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.TotalSumsInfo != null)
                obj.TotalSumsInfo.AppendToXml(container, "TotalSumsInfo");
            
            if (obj.EntriesByGuests != null && obj.EntriesByGuests.Any(o => o != null))
            {
                var entriesByGuestsContainer = new XElement("EntriesByGuests");
                foreach (var item in obj.EntriesByGuests)
                    item.AppendToXml(entriesByGuestsContainer, "DeliveryGuest");
                container.Add(entriesByGuestsContainer);
            }
            
            if (obj.Discounts != null && obj.Discounts.Any(o => o != null))
            {
                var discountsContainer = new XElement("Discounts");
                foreach (var item in obj.Discounts)
                    item.AppendToXml(discountsContainer, "Discount");
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
            
            if (obj.Vats != null && obj.Vats.Any(o => o != null))
            {
                var vatsContainer = new XElement("Vats");
                foreach (var item in obj.Vats)
                    item.AppendToXml(vatsContainer, "Vat");
                container.Add(vatsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryOrderEntryInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("IsModifier", obj.IsModifier));
                
            container.Add(new XAttribute("IsPrechequePrintable", obj.IsPrechequePrintable));
                
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
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDeliveryGuest obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.DeliveryItems != null && obj.DeliveryItems.Any(o => o != null))
            {
                var deliveryItemsContainer = new XElement("DeliveryItems");
                foreach (var item in obj.DeliveryItems)
                    item.AppendToXml(deliveryItemsContainer, "DeliveryOrderEntryInfo");
                container.Add(deliveryItemsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this ITotalSumsInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("SubTotal", obj.SubTotal));
                
            container.Add(new XAttribute("ResultSum", obj.ResultSum));
                
            container.Add(new XAttribute("ChangeSum", obj.ChangeSum));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            if (obj.AllSumsCurrencyUnitLabel != null)
                container.Add(new XAttribute("AllSumsCurrencyUnitLabel", obj.AllSumsCurrencyUnitLabel));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDiscountInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Percent", obj.Percent));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.CardNumber != null)
                container.Add(new XAttribute("CardNumber", obj.CardNumber));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IPaymentInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            container.Add(new XAttribute("IsCash", obj.IsCash));
                
            container.Add(new XAttribute("IsCard", obj.IsCard));
                
            if (obj.CounteragentName != null)
                container.Add(new XAttribute("CounteragentName", obj.CounteragentName));
                
            if (obj.CounteragentCardNumber != null)
                container.Add(new XAttribute("CounteragentCardNumber", obj.CounteragentCardNumber));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IVatInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("VatPercent", obj.VatPercent));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IBillFooter obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Value != null)
                container.Add(new XAttribute("Value", obj.Value));
                
            parent.Add(container);
        }
        
        
    }
}
