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
    public interface IChequeSettings
    {
        string ChequeHeader { get; }
        string ChequeFooter { get; }
        string UserName { get; }
        string GroupName { get; }
        string CurrencyName { get; }
        string ShortCurrencyName { get; }
        DateTime Now { get; }
        string EnterpriseName { get; }
    }

    public interface IChequeExtensions
    {
        IEnumerable<XElement> BeforeHeader { get; }
        IEnumerable<XElement> AfterHeader { get; }
        IEnumerable<XElement> BeforeFooter { get; }
        IEnumerable<XElement> AfterFooter { get; }
    }

    public static class ChequesCommonToXmlConverterExtensions
    {
        internal static void AppendToXml(this IChequeSettings obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.ChequeHeader != null)
                container.Add(new XAttribute("ChequeHeader", obj.ChequeHeader));
                
            if (obj.ChequeFooter != null)
                container.Add(new XAttribute("ChequeFooter", obj.ChequeFooter));
                
            if (obj.UserName != null)
                container.Add(new XAttribute("UserName", obj.UserName));
                
            if (obj.GroupName != null)
                container.Add(new XAttribute("GroupName", obj.GroupName));
                
            if (obj.CurrencyName != null)
                container.Add(new XAttribute("CurrencyName", obj.CurrencyName));
                
            if (obj.ShortCurrencyName != null)
                container.Add(new XAttribute("ShortCurrencyName", obj.ShortCurrencyName));
                
            container.Add(new XAttribute("Now", obj.Now));
                
            if (obj.EnterpriseName != null)
                container.Add(new XAttribute("EnterpriseName", obj.EnterpriseName));
                
			ChequeResources.AppendToXml(container, "Resources");
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IChequeExtensions obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.BeforeHeader != null && obj.BeforeHeader.Any(o => o != null))
            {
                var beforeHeaderContainer = new XElement("BeforeHeader");
                foreach (var item in obj.BeforeHeader)
                    beforeHeaderContainer.Add(item);
                container.Add(beforeHeaderContainer);
            }
            
            if (obj.AfterHeader != null && obj.AfterHeader.Any(o => o != null))
            {
                var afterHeaderContainer = new XElement("AfterHeader");
                foreach (var item in obj.AfterHeader)
                    afterHeaderContainer.Add(item);
                container.Add(afterHeaderContainer);
            }
            
            if (obj.BeforeFooter != null && obj.BeforeFooter.Any(o => o != null))
            {
                var beforeFooterContainer = new XElement("BeforeFooter");
                foreach (var item in obj.BeforeFooter)
                    beforeFooterContainer.Add(item);
                container.Add(beforeFooterContainer);
            }
            
            if (obj.AfterFooter != null && obj.AfterFooter.Any(o => o != null))
            {
                var afterFooterContainer = new XElement("AfterFooter");
                foreach (var item in obj.AfterFooter)
                    afterFooterContainer.Add(item);
                container.Add(afterFooterContainer);
            }
            
            parent.Add(container);
        }
        
        
    }
}
