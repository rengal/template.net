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
    /// Информация о кухонном чеке
    /// </summary>
    public interface IKitchenChequeInfo
    {
        /// <summary>
        /// Сведения о заказе
        /// </summary>
        IOrderInfo Order { get; }
        /// <summary>
        /// Счётчик печати
        /// </summary>
        int PrinterCounter { get; }
        /// <summary>
        /// Флаг использования курсов
        /// </summary>
        bool HasCourses { get; }
    }

    /// <summary>
    /// Сведения о заказе
    /// </summary>
    public interface IOrderInfo
    {
        /// <summary>
        /// Номер заказа
        /// </summary>
        int Number { get; }
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
        int GuestsCount { get; }
        /// <summary>
        /// Имя официанта
        /// </summary>
        string WaiterName { get; }
        /// <summary>
        /// Время открытия заказа
        /// </summary>
        DateTime OrderOpenTime { get; }
    }

    /// <summary>
    /// Элемент сервисного чека
    /// </summary>
    public interface IKitchenChequeItem
    {
        /// <summary>
        /// Курс блюд
        /// </summary>
        int Course { get; }
        /// <summary>
        /// Объединяет ли элемент все смиксованные блюда
        /// </summary>
        bool IsWholeMix { get; }
        /// <summary>
        /// Блюда элемента
        /// </summary>
        IEnumerable<IKitchenChequeProduct> Products { get; }
    }

    /// <summary>
    /// Элементы сервисного чека с одинаковым курсом
    /// </summary>
    public interface IKitchenChequeCourseGroup
    {
        /// <summary>
        /// Курс блюд
        /// </summary>
        int Course { get; }
        /// <summary>
        /// Элементы чека
        /// </summary>
        IEnumerable<IKitchenChequeItem> ChequeItems { get; }
    }

    /// <summary>
    /// Блюдо сервисного чека
    /// </summary>
    public interface IKitchenChequeProduct
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Название в меню
        /// </summary>
        string NameInMenu { get; }
        /// <summary>
        /// Код быстрого набора
        /// </summary>
        string Code { get; }
        /// <summary>
        /// Штрихкод
        /// </summary>
        string BarCode { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Название места приготовления
        /// </summary>
        string CookingPlaceName { get; }
        /// <summary>
        /// Место гостя за столом
        /// </summary>
        int GuestPlace { get; }
        /// <summary>
        /// Модификаторы
        /// </summary>
        IEnumerable<IKitchenChequeModifier> Modifiers { get; }
        /// <summary>
        /// Блюдо, модификатором для которого является данное
        /// </summary>
        string ParentProduct { get; }
    }

    /// <summary>
    /// Модификатор в сервисном чеке
    /// </summary>
    public interface IKitchenChequeModifier
    {
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Название в меню
        /// </summary>
        string NameInMenu { get; }
        /// <summary>
        /// Код быстрого набора
        /// </summary>
        string Code { get; }
        /// <summary>
        /// Количество
        /// </summary>
        decimal Amount { get; }
        /// <summary>
        /// Задано ли количество абсолютным значением или множителем к количеству родительского блюда
        /// </summary>
        bool IsAmountAbsolute { get; }
        /// <summary>
        /// Является ли комментарием
        /// </summary>
        bool IsComment { get; }
        /// <summary>
        /// Название места приготовления
        /// </summary>
        string CookingPlaceName { get; }
    }

    public static class ServiceChequesCommonToXmlConverterExtensions
    {
        internal static void AppendToXml(this IKitchenChequeInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Order != null)
                obj.Order.AppendToXml(container, "Order");
            
            container.Add(new XAttribute("PrinterCounter", obj.PrinterCounter));
                
            container.Add(new XAttribute("HasCourses", obj.HasCourses));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IOrderInfo obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("Number", obj.Number));
                
            container.Add(new XAttribute("TableNumber", obj.TableNumber));
                
            if (obj.SectionName != null)
                container.Add(new XAttribute("SectionName", obj.SectionName));
                
            container.Add(new XAttribute("GuestsCount", obj.GuestsCount));
                
            if (obj.WaiterName != null)
                container.Add(new XAttribute("WaiterName", obj.WaiterName));
                
            container.Add(new XAttribute("OrderOpenTime", obj.OrderOpenTime));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IKitchenChequeItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("Course", obj.Course));
                
            container.Add(new XAttribute("IsWholeMix", obj.IsWholeMix));
                
            if (obj.Products != null && obj.Products.Any(o => o != null))
            {
                var productsContainer = new XElement("Products");
                foreach (var item in obj.Products)
                    item.AppendToXml(productsContainer, "Product");
                container.Add(productsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IKitchenChequeCourseGroup obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("Course", obj.Course));
                
            if (obj.ChequeItems != null && obj.ChequeItems.Any(o => o != null))
            {
                var chequeItemsContainer = new XElement("ChequeItems");
                foreach (var item in obj.ChequeItems)
                    item.AppendToXml(chequeItemsContainer, "ChequeItem");
                container.Add(chequeItemsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IKitchenChequeProduct obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.NameInMenu != null)
                container.Add(new XAttribute("NameInMenu", obj.NameInMenu));
                
            if (obj.Code != null)
                container.Add(new XAttribute("Code", obj.Code));
                
            if (obj.BarCode != null)
                container.Add(new XAttribute("BarCode", obj.BarCode));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            if (obj.CookingPlaceName != null)
                container.Add(new XAttribute("CookingPlaceName", obj.CookingPlaceName));
                
            container.Add(new XAttribute("GuestPlace", obj.GuestPlace));
                
            if (obj.Modifiers != null && obj.Modifiers.Any(o => o != null))
            {
                var modifiersContainer = new XElement("Modifiers");
                foreach (var item in obj.Modifiers)
                    item.AppendToXml(modifiersContainer, "Modifier");
                container.Add(modifiersContainer);
            }
            
            if (obj.ParentProduct != null)
                container.Add(new XAttribute("ParentProduct", obj.ParentProduct));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IKitchenChequeModifier obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            if (obj.NameInMenu != null)
                container.Add(new XAttribute("NameInMenu", obj.NameInMenu));
                
            if (obj.Code != null)
                container.Add(new XAttribute("Code", obj.Code));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("IsAmountAbsolute", obj.IsAmountAbsolute));
                
            container.Add(new XAttribute("IsComment", obj.IsComment));
                
            if (obj.CookingPlaceName != null)
                container.Add(new XAttribute("CookingPlaceName", obj.CookingPlaceName));
                
            parent.Add(container);
        }
        
        
    }
}
