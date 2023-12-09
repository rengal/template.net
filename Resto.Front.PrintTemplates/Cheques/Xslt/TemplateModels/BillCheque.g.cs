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
    /// Данные для пречека, корневой элемент
    /// </summary>
    public interface IBill
    {
        IChequeSettings Settings { get; }
        IChequeExtensions Extensions { get; }
        /// <summary>
        /// Название отделения
        /// </summary>
        string SectionName { get; }
        /// <summary>
        /// Номер стола
        /// </summary>
        int TableNumber { get; }
        /// <summary>
        /// Имя официанта
        /// </summary>
        string WaiterName { get; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        int OrderNumber { get; }
        /// <summary>
        /// Время открытия заказа
        /// </summary>
        DateTime OrderOpenTime { get; }
        /// <summary>
        /// Полная сумма заказа
        /// </summary>
        decimal FullSum { get; }
        /// <summary>
        /// Подытог, полная сумма с учётом категориальных скидок/надбавок
        /// </summary>
        decimal SubTotal { get; }
        /// <summary>
        /// Сумма заказа с учётом всех скидок/надбавок
        /// </summary>
        decimal TotalWithoutDiscounts { get; }
        /// <summary>
        /// Сумма предоплаты
        /// </summary>
        decimal Prepay { get; }
        /// <summary>
        /// Итоговая сумма НДС
        /// </summary>
        decimal VatSum { get; }
        /// <summary>
        /// Итоговая сумма, к оплате
        /// </summary>
        decimal Total { get; }
        /// <summary>
        /// Гости
        /// </summary>
        IEnumerable<IGuest> Guests { get; }
        /// <summary>
        /// Карты гостей
        /// </summary>
        IEnumerable<string> GuestCardsInfo { get; }
        /// <summary>
        /// Маркетинговые акции для заказа
        /// </summary>
        IEnumerable<IDiscountMarketingCampaign> DiscountMarketingCampaigns { get; }
        /// <summary>
        /// Категориальные скидки/надбавки
        /// </summary>
        IEnumerable<IDiscountIncrease> CategorizedDiscountsAndIncreases { get; }
        /// <summary>
        /// Некатегориальные скидки/надбавки
        /// </summary>
        IEnumerable<IDiscountIncrease> NonCategorizedDiscountsAndIncreases { get; }
        /// <summary>
        /// Нужно ли в пречеке место для подписи гостя
        /// </summary>
        bool IsAdditionalServiceCheque { get; }
        /// <summary>
        /// Промежуточная квитанция дозаказа
        /// </summary>
        IAdditionalService AdditionalServiceInfo { get; }
        /// <summary>
        /// Налоги
        /// </summary>
        IEnumerable<IVatItem> Vats { get; }
    }

    /// <summary>
    /// Маркетинговая акция
    /// </summary>
    public interface IDiscountMarketingCampaign
    {
        /// <summary>
        /// Название акции
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Полная скидка по всем операциям
        /// </summary>
        decimal TotalDiscount { get; }
        /// <summary>
        /// Комментарий для чека
        /// </summary>
        string BillComment { get; }
        /// <summary>
        /// Операции
        /// </summary>
        IEnumerable<IDiscountMarketingCampaignOperation> Operations { get; }
    }

    /// <summary>
    /// Скидка по маркетинговой акции
    /// </summary>
    public interface IDiscountMarketingCampaignOperation
    {
        /// <summary>
        /// Название продукта
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Скидка по продукту
        /// </summary>
        decimal Discount { get; }
        /// <summary>
        /// Комментарий, описывающий происхождение значения скидки
        /// </summary>
        string Comment { get; }
    }

    /// <summary>
    /// Гость
    /// </summary>
    public interface IGuest
    {
        /// <summary>
        /// Имя
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Подытог, сумма гостя с учётом категориальных скидок/надбавок
        /// </summary>
        decimal Subtotal { get; }
        /// <summary>
        /// Итого по гостю с учётом всех скидок/надбавок
        /// </summary>
        decimal Total { get; }
        /// <summary>
        /// Заказанные блюда
        /// </summary>
        IEnumerable<IProduct> Products { get; }
        /// <summary>
        /// Некатегориальные скидки/надбавки
        /// </summary>
        IEnumerable<IDiscountIncrease> NonCategorisedDiscountsAndIncreases { get; }
    }

    /// <summary>
    /// Блюдо
    /// </summary>
    public interface IProduct
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
        /// Стоимость
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Модификаторы
        /// </summary>
        IEnumerable<IModifier> Modifiers { get; }
        /// <summary>
        /// Категориальные скидки/надбавки
        /// </summary>
        IEnumerable<IDiscountIncrease> CategorisedDiscountsAndIncreases { get; }
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
    /// Модификатор
    /// </summary>
    public interface IModifier
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
        /// Стоимость
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Категориальные скидки/надбавки
        /// </summary>
        IEnumerable<IDiscountIncrease> CategorisedDiscountsAndIncreases { get; }
    }

    /// <summary>
    /// Скидка/надбавка
    /// </summary>
    public interface IDiscountIncrease
    {
        /// <summary>
        /// Является ли скидкой
        /// </summary>
        bool IsDiscount { get; }
        /// <summary>
        /// Нужно ли детализировать в пречеке
        /// </summary>
        bool PrintDetailed { get; }
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Процент
        /// </summary>
        decimal Percent { get; }
        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; }
        /// <summary>
        /// Сумма строкой
        /// </summary>
        string SumString { get; }
        /// <summary>
        /// Описание для подвала
        /// </summary>
        string DescriptionString { get; }
        /// <summary>
        /// Описание для блюда/модификатора
        /// </summary>
        string DescriptionStringForOrderItem { get; }
    }

    /// <summary>
    /// налог, не включенный в стоимость блюда
    /// </summary>
    public interface IVatItem
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

    /// <summary>
    /// Промежуточная квитанция дозаказа
    /// </summary>
    public interface IAdditionalService
    {
        /// <summary>
        /// Привязанная к заказу карты гостей
        /// </summary>
        string ClientBindCardInfo { get; }
        /// <summary>
        /// Итого добавлено
        /// </summary>
        decimal AdditionalSum { get; }
        /// <summary>
        /// Есть ли лимит
        /// </summary>
        bool HasLimitSum { get; }
        /// <summary>
        /// Oсталось до лимита
        /// </summary>
        decimal LimitLeftSum { get; }
    }

    public static class BillChequeToXmlConverterExtensions
    {
        [NotNull]
        public static XDocument ToXml([NotNull] this IBill obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
        
            var document = new XDocument();
            
            obj.AppendToXml(document, "Bill");
            
            return document;
        }
        
        internal static void AppendToXml(this IBill obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Settings != null)
                obj.Settings.AppendToXml(container, "Settings");
            
            if (obj.Extensions != null)
                obj.Extensions.AppendToXml(container, "Extensions");
            
            if (obj.SectionName != null)
                container.Add(new XAttribute("SectionName", obj.SectionName));
                
            container.Add(new XAttribute("TableNumber", obj.TableNumber));
                
            if (obj.WaiterName != null)
                container.Add(new XAttribute("WaiterName", obj.WaiterName));
                
            container.Add(new XAttribute("OrderNumber", obj.OrderNumber));
                
            container.Add(new XAttribute("OrderOpenTime", obj.OrderOpenTime));
                
            container.Add(new XAttribute("FullSum", obj.FullSum));
                
            container.Add(new XAttribute("SubTotal", obj.SubTotal));
                
            container.Add(new XAttribute("TotalWithoutDiscounts", obj.TotalWithoutDiscounts));
                
            container.Add(new XAttribute("Prepay", obj.Prepay));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            container.Add(new XAttribute("Total", obj.Total));
                
            if (obj.Guests != null && obj.Guests.Any(o => o != null))
            {
                var guestsContainer = new XElement("Guests");
                foreach (var item in obj.Guests)
                    item.AppendToXml(guestsContainer, "Guest");
                container.Add(guestsContainer);
            }
            
            if (obj.GuestCardsInfo != null && obj.GuestCardsInfo.Any(o => o != null))
            {
                var guestCardsInfoContainer = new XElement("GuestCardsInfo");
                foreach (var item in obj.GuestCardsInfo.Where(o => o != null))
                    guestCardsInfoContainer.Add(new XElement("Value", item));
                container.Add(guestCardsInfoContainer);
            }
            
            if (obj.DiscountMarketingCampaigns != null && obj.DiscountMarketingCampaigns.Any(o => o != null))
            {
                var discountMarketingCampaignsContainer = new XElement("DiscountMarketingCampaigns");
                foreach (var item in obj.DiscountMarketingCampaigns)
                    item.AppendToXml(discountMarketingCampaignsContainer, "DiscountMarketingCampaign");
                container.Add(discountMarketingCampaignsContainer);
            }
            
            if (obj.CategorizedDiscountsAndIncreases != null && obj.CategorizedDiscountsAndIncreases.Any(o => o != null))
            {
                var categorizedDiscountsAndIncreasesContainer = new XElement("CategorizedDiscountsAndIncreases");
                foreach (var item in obj.CategorizedDiscountsAndIncreases)
                    item.AppendToXml(categorizedDiscountsAndIncreasesContainer, "DiscountIncrease");
                container.Add(categorizedDiscountsAndIncreasesContainer);
            }
            
            if (obj.NonCategorizedDiscountsAndIncreases != null && obj.NonCategorizedDiscountsAndIncreases.Any(o => o != null))
            {
                var nonCategorizedDiscountsAndIncreasesContainer = new XElement("NonCategorizedDiscountsAndIncreases");
                foreach (var item in obj.NonCategorizedDiscountsAndIncreases)
                    item.AppendToXml(nonCategorizedDiscountsAndIncreasesContainer, "DiscountIncrease");
                container.Add(nonCategorizedDiscountsAndIncreasesContainer);
            }
            
            container.Add(new XAttribute("IsAdditionalServiceCheque", obj.IsAdditionalServiceCheque));
                
            if (obj.AdditionalServiceInfo != null)
                obj.AdditionalServiceInfo.AppendToXml(container, "AdditionalServiceInfo");
            
            if (obj.Vats != null && obj.Vats.Any(o => o != null))
            {
                var vatsContainer = new XElement("Vats");
                foreach (var item in obj.Vats)
                    item.AppendToXml(vatsContainer, "VatItem");
                container.Add(vatsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDiscountMarketingCampaign obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("TotalDiscount", obj.TotalDiscount));
                
            if (obj.BillComment != null)
                container.Add(new XAttribute("BillComment", obj.BillComment));
                
            if (obj.Operations != null && obj.Operations.Any(o => o != null))
            {
                var operationsContainer = new XElement("Operations");
                foreach (var item in obj.Operations)
                    item.AppendToXml(operationsContainer, "DiscountMarketingCampaignOperation");
                container.Add(operationsContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDiscountMarketingCampaignOperation obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Discount", obj.Discount));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IGuest obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Subtotal", obj.Subtotal));
                
            container.Add(new XAttribute("Total", obj.Total));
                
            if (obj.Products != null && obj.Products.Any(o => o != null))
            {
                var productsContainer = new XElement("Products");
                foreach (var item in obj.Products)
                    item.AppendToXml(productsContainer, "Product");
                container.Add(productsContainer);
            }
            
            if (obj.NonCategorisedDiscountsAndIncreases != null && obj.NonCategorisedDiscountsAndIncreases.Any(o => o != null))
            {
                var nonCategorisedDiscountsAndIncreasesContainer = new XElement("NonCategorisedDiscountsAndIncreases");
                foreach (var item in obj.NonCategorisedDiscountsAndIncreases)
                    item.AppendToXml(nonCategorisedDiscountsAndIncreasesContainer, "DiscountIncrease");
                container.Add(nonCategorisedDiscountsAndIncreasesContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IProduct obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.Modifiers != null && obj.Modifiers.Any(o => o != null))
            {
                var modifiersContainer = new XElement("Modifiers");
                foreach (var item in obj.Modifiers)
                    item.AppendToXml(modifiersContainer, "Modifier");
                container.Add(modifiersContainer);
            }
            
            if (obj.CategorisedDiscountsAndIncreases != null && obj.CategorisedDiscountsAndIncreases.Any(o => o != null))
            {
                var categorisedDiscountsAndIncreasesContainer = new XElement("CategorisedDiscountsAndIncreases");
                foreach (var item in obj.CategorisedDiscountsAndIncreases)
                    item.AppendToXml(categorisedDiscountsAndIncreasesContainer, "DiscountIncrease");
                container.Add(categorisedDiscountsAndIncreasesContainer);
            }
            
            container.Add(new XAttribute("CommentExists", obj.CommentExists));
                
            if (obj.Comment != null)
                container.Add(new XAttribute("Comment", obj.Comment));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IModifier obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Amount", obj.Amount));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.CategorisedDiscountsAndIncreases != null && obj.CategorisedDiscountsAndIncreases.Any(o => o != null))
            {
                var categorisedDiscountsAndIncreasesContainer = new XElement("CategorisedDiscountsAndIncreases");
                foreach (var item in obj.CategorisedDiscountsAndIncreases)
                    item.AppendToXml(categorisedDiscountsAndIncreasesContainer, "DiscountIncrease");
                container.Add(categorisedDiscountsAndIncreasesContainer);
            }
            
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IDiscountIncrease obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("IsDiscount", obj.IsDiscount));
                
            container.Add(new XAttribute("PrintDetailed", obj.PrintDetailed));
                
            if (obj.Name != null)
                container.Add(new XAttribute("Name", obj.Name));
                
            container.Add(new XAttribute("Percent", obj.Percent));
                
            container.Add(new XAttribute("Sum", obj.Sum));
                
            if (obj.SumString != null)
                container.Add(new XAttribute("SumString", obj.SumString));
                
            if (obj.DescriptionString != null)
                container.Add(new XAttribute("DescriptionString", obj.DescriptionString));
                
            if (obj.DescriptionStringForOrderItem != null)
                container.Add(new XAttribute("DescriptionStringForOrderItem", obj.DescriptionStringForOrderItem));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IVatItem obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            container.Add(new XAttribute("VatPercent", obj.VatPercent));
                
            container.Add(new XAttribute("VatSum", obj.VatSum));
                
            parent.Add(container);
        }
        
        internal static void AppendToXml(this IAdditionalService obj, XContainer parent, string elementName)
        {
            var container = new XElement(elementName);
            
            if (obj.ClientBindCardInfo != null)
                container.Add(new XAttribute("ClientBindCardInfo", obj.ClientBindCardInfo));
                
            container.Add(new XAttribute("AdditionalSum", obj.AdditionalSum));
                
            container.Add(new XAttribute("HasLimitSum", obj.HasLimitSum));
                
            container.Add(new XAttribute("LimitLeftSum", obj.LimitLeftSum));
                
            parent.Add(container);
        }
        
        
    }
}
