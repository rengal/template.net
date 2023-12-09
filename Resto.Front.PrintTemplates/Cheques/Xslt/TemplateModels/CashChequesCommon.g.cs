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
    /// Информация о квитанциях
    /// </summary>
    public interface ICashChequeInfo
    {
        bool IsForReport { get; }
        /// <summary>
        /// Номер смены
        /// </summary>
        int CafeSessionNumber { get; }
        /// <summary>
        /// Время открытия кассовой смены
        /// </summary>
        DateTime CafeSessionOpenTime { get; }
        /// <summary>
        /// Номер ФРа
        /// </summary>
        int CashRegisterNumber { get; }
        /// <summary>
        /// Имя кассира
        /// </summary>
        string CashierName { get; }
        /// <summary>
        /// Текущее время
        /// </summary>
        DateTime OperationTime { get; }
    }

    public static class CashChequesCommonToXmlConverterExtensions
    {
        internal static void AppendToXml(this ICashChequeInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("IsForReport", obj.IsForReport));
                
            container.Add(new XAttribute("CafeSessionNumber", obj.CafeSessionNumber));
                
            container.Add(new XAttribute("CafeSessionOpenTime", obj.CafeSessionOpenTime));
                
            container.Add(new XAttribute("CashRegisterNumber", obj.CashRegisterNumber));
                
            if (obj.CashierName != null)
                container.Add(new XAttribute("CashierName", obj.CashierName));
                
            container.Add(new XAttribute("OperationTime", obj.OperationTime));
                
            parent.Add(container);
        }
        
        
    }
}
