using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public static class ModelExtensions
    {
        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> GetAllEntries([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.Guests
                .SelectMany(guest => guest.Items)
                .SelectMany(item => item.ExpandAllEntries());
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> ExpandAllEntries([NotNull] this IOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            return orderItem.GetChildren().StartWith(orderItem);
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> GetChildren([NotNull] this IOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            if (orderItem is IProductItem productItem)
                return productItem.ModifierEntries;

            return ((ITimePayServiceItem)orderItem).RateScheduleEntries;
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> GetIncludedEntries([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.Guests
                .SelectMany(guest => guest.Items)
                .SelectMany(item => item.ExpandIncludedEntries());
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> ExpandIncludedEntries([NotNull] this IOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            if (orderItem.DeletionInfo != null)
                return Enumerable.Empty<IOrderEntry>();

            return orderItem.GetNotDeletedChildren().StartWith(orderItem);
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IOrderEntry> GetNotDeletedChildren([NotNull] this IOrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            if (orderItem.DeletionInfo != null)
                return Enumerable.Empty<IOrderEntry>();

            var productItem = orderItem as IProductItem;

            IEnumerable<IOrderEntry> children;

            if (productItem != null)
                children = productItem.ModifierEntries;
            else
                children = ((ITimePayServiceItem)orderItem).RateScheduleEntries;

            return children.Where(m => m.DeletionInfo == null);
        }

        [PublicAPI, Pure]
        [Obsolete("Правильнее использовать свойство Cost.")]
        public static decimal GetCost([NotNull] this IOrderEntry orderEntry)
        {
            if (orderEntry == null)
                throw new ArgumentNullException(nameof(orderEntry));

            return orderEntry.Cost;
        }

        [PublicAPI, Pure]
        public static decimal GetFullSum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.GetIncludedEntries().Sum(orderEntry => orderEntry.Cost);
        }

        [PublicAPI, Pure]
        public static decimal GetDiscountSum([NotNull] this IDiscountItem discountItem)
        {
            if (discountItem == null)
                throw new ArgumentNullException(nameof(discountItem));

            return discountItem.DiscountSums.Values.Sum();
        }

        [PublicAPI, Pure]
        public static decimal GetDiscountSumFor([NotNull] this IDiscountItem discountItem, [NotNull] IOrderEntry orderEntry)
        {
            if (discountItem == null)
                throw new ArgumentNullException(nameof(discountItem));
            if (orderEntry == null)
                throw new ArgumentNullException(nameof(orderEntry));

            discountItem.DiscountSums.TryGetValue(orderEntry, out var sum);
            return sum;
        }

        [PublicAPI, Pure]
        public static decimal GetResultSum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.GetResultSumWithoutExcludedVat() + order.GetVatSumExcludedFromPrice();
        }

        [PublicAPI, Pure]
        public static decimal GetResultSumWithoutExcludedVat([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return (order.GetFullSum() - order.DiscountItems.Sum(discount => discount.GetDiscountSum())).RoundMoney();
        }

        [PublicAPI, Pure]
        public static decimal GetVatSumExcludedFromPrice([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.GetIncludedEntries().Sum(orderEntry => orderEntry.ExcludedVat);
        }

        [PublicAPI, Pure]
        [Obsolete("Правильнее использовать свойство ExcludedVat.")]
        public static decimal GetVatSumExcludedFromPriceForOrderEntry([NotNull] this IOrderEntry orderEntry, IEnumerable<IDiscountItem> discountItems)
        {
            if (orderEntry == null)
                throw new ArgumentNullException(nameof(orderEntry));

            return orderEntry.ExcludedVat;
        }

        [PublicAPI, Pure]
        public static decimal CalculateDiscountPercent(decimal fullSum, decimal discountSum)
        {
            // TODO RMS-35322 п.5 Может быть так, что итоговая стоимость 0, но в заказе есть айтемы.
            // пока не придумано, как быть со скидками в этом случае.
            // if (fullSum == 0m && discountSum != 0m)
            //   throw new ArithmeticException(string.Format("Cannot calculate discount percent for zero full sum and non-zero ({0}) discount sum", discountSum));
            // Концепция будет определна в RMS-35388
            return fullSum == 0m ? 0m : Math.Round((discountSum / fullSum) * 100m, 2, MidpointRounding.AwayFromZero);
        }

        [PublicAPI, Pure]
        public static DateTime GetPeriodBegin([NotNull] this ISettings settings, [NotNull] string name = "ReportInterval")
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (DateTime)settings.GetValue(name + ".Begin");
        }

        [PublicAPI, Pure]
        public static DateTime GetPeriodEnd([NotNull] this ISettings settings, [NotNull] string name = "ReportInterval")
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (DateTime)settings.GetValue(name + ".End");
        }

        [PublicAPI, Pure]
        public static bool GetBool([NotNull] this ISettings settings, [NotNull] string name)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (bool)settings.GetValue(name);
        }

        [PublicAPI, Pure, NotNull]
        public static string GetEnum([NotNull] this ISettings settings, [NotNull] string name)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (string)settings.GetValue(name);
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<IUser> GetCounteragents([NotNull] this ISettings settings, [NotNull] string name)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (IEnumerable<IUser>)settings.GetValue(name);
        }
    }
}