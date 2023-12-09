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
    /// Чек iikoCard
    /// </summary>
    public interface IIikoCard
    {
        IChequeSettings Settings { get; }
        /// <summary>
        /// Информация об операции и картой
        /// </summary>
        IIikoCardInfo CardInfo { get; }
        /// <summary>
        /// Есть ли заказ у чека
        /// </summary>
        bool HasOrder { get; }
        /// <summary>
        /// Информация о заказе
        /// </summary>
        IOrderInfo Order { get; }
        /// <summary>
        /// Успешна ли операция
        /// </summary>
        bool IsSuccessful { get; }
    }

    /// <summary>
    /// Общая информация iikoCard
    /// </summary>
    public interface IIikoCardInfo
    {
        /// <summary>
        /// Владелец карты
        /// </summary>
        string CardOwner { get; }
        /// <summary>
        /// Номер карты
        /// </summary>
        string CardNumber { get; }
        /// <summary>
        /// Сумма операции
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Название операции, отображаемое на чеке
        /// </summary>
        string OperationName { get; }
        /// <summary>
        /// Имеет ли чек место для подписи покупателем
        /// </summary>
        bool HasSignature { get; }
        /// <summary>
        /// Имеет ли чек строку с суммой операции
        /// </summary>
        bool HasAmount { get; }
        /// <summary>
        /// Является ли оплатой
        /// </summary>
        bool IsPayment { get; }
        /// <summary>
        /// Отображать идентификаторы операций
        /// </summary>
        bool HasOperationIds { get; }
        /// <summary>
        /// Название серверного номера операции, отображаемое на чеке
        /// </summary>
        string OperationNumTitle { get; }
        /// <summary>
        /// Название клиентского номера операции (контрольного кода), отображаемое на чеке
        /// </summary>
        string RequestNumTitle { get; }
        /// <summary>
        /// Данные с сервера iikoCard
        /// </summary>
        IEnumerable<IIikoCardRow> ChequeRows { get; }
        /// <summary>
        /// Серверный номер операции
        /// </summary>
        string OperationId { get; }
        /// <summary>
        /// Клиентский номер операции (контрольный код)
        /// </summary>
        string RequestId { get; }
        /// <summary>
        /// Дата и время на сервере
        /// </summary>
        DateTime HostTime { get; }
        /// <summary>
        /// Есть ли дата и время на сервере
        /// </summary>
        bool HasHostTime { get; }
        /// <summary>
        /// Описание ошибки
        /// </summary>
        string ErrorString { get; }
        /// <summary>
        /// Имя кассира
        /// </summary>
        string CashierName { get; }
        /// <summary>
        /// Терминал
        /// </summary>
        string Terminal { get; }
    }

    /// <summary>
    /// Пары, получаемые с сервера
    /// </summary>
    public interface IIikoCardRow
    {
        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Значение
        /// </summary>
        decimal Value { get; }
        /// <summary>
        /// Дополнительные строки
        /// </summary>
        string Lines { get; }
        /// <summary>
        /// Есть ли дополнительные строки
        /// </summary>
        bool HasLines { get; }
    }

    public static class IikoCardChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IIikoCard obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "IikoCard");
            
            return document;
        }
        
        internal static void AppendToXml(this IIikoCard obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.CardInfo != null)
                obj.CardInfo.AppendToXml(container, "CardInfo");
            
            container.Add(new XAttribute("HasOrder", obj.HasOrder));
                
            if (obj.Order != null)
                obj.Order.AppendToXml(container, "Order");
            
            container.Add(new XAttribute("IsSuccessful", obj.IsSuccessful));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IIikoCardInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.CardOwner != null)
                container.Add(new XAttribute("CardOwner", obj.CardOwner));
                
            if (obj.CardNumber != null)
                container.Add(new XAttribute("CardNumber", obj.CardNumber));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            if (obj.OperationName != null)
                container.Add(new XAttribute("OperationName", obj.OperationName));
                
            container.Add(new XAttribute("HasSignature", obj.HasSignature));
                
            container.Add(new XAttribute("HasAmount", obj.HasAmount));
                
            container.Add(new XAttribute("IsPayment", obj.IsPayment));
                
            container.Add(new XAttribute("HasOperationIds", obj.HasOperationIds));
                
            if (obj.OperationNumTitle != null)
                container.Add(new XAttribute("OperationNumTitle", obj.OperationNumTitle));
                
            if (obj.RequestNumTitle != null)
                container.Add(new XAttribute("RequestNumTitle", obj.RequestNumTitle));
                
            if (obj.ChequeRows != null && obj.ChequeRows.Any(o => o != null))
            {
                var chequeRowsContainer = new XElement("ChequeRows");
                foreach (var item in obj.ChequeRows)
                    item.AppendToXml(chequeRowsContainer, "IikoCardRow");
                container.Add(chequeRowsContainer);
            }
            
            if (obj.OperationId != null)
                container.Add(new XAttribute("OperationId", obj.OperationId));
                
            if (obj.RequestId != null)
                container.Add(new XAttribute("RequestId", obj.RequestId));
                
            container.Add(new XAttribute("HostTime", obj.HostTime));
                
            container.Add(new XAttribute("HasHostTime", obj.HasHostTime));
                
            if (obj.ErrorString != null)
                container.Add(new XAttribute("ErrorString", obj.ErrorString));
                
            if (obj.CashierName != null)
                container.Add(new XAttribute("CashierName", obj.CashierName));
                
            if (obj.Terminal != null)
                container.Add(new XAttribute("Terminal", obj.Terminal));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IIikoCardRow obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Value", obj.Value));
                
            if (obj.Lines != null)
                container.Add(new XAttribute("Lines", obj.Lines));
                
            container.Add(new XAttribute("HasLines", obj.HasLines));
                
            parent.Add(container);
        }
        
        
    }
}
