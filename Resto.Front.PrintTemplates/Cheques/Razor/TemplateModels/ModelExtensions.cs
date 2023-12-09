using Resto.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Front.Localization.Resources;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    public static class ModelExtensions
    {
        [PublicAPI, Pure]
        public static decimal GetCost([NotNull] this IChequeTaskSale chequeTaskSale)
        {
            if (chequeTaskSale == null)
                throw new ArgumentNullException(nameof(chequeTaskSale));

            return (chequeTaskSale.Price * chequeTaskSale.Amount).RoundMoney();
        }

        [PublicAPI, Pure]
        [Obsolete("Правильнее использовать свойство Cost.")]
        public static decimal GetCost([NotNull] this IOrderEntry orderEntry)
        {
            if (orderEntry == null)
                throw new ArgumentNullException(nameof(orderEntry));

            return orderEntry.Cost;
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

            IEnumerable<IOrderEntry> children;

            if (orderItem is IProductItem productItem)
                children = productItem.ModifierEntries;
            else
                children = ((ITimePayServiceItem)orderItem).RateScheduleEntries;

            return children.Where(m => m.DeletionInfo == null);
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
        [Obsolete("Правильнее использовать свойство IncludedVat.")]
        public static decimal GetVatSumIncludedInPriceForOrderEntry([NotNull] this IOrderEntry orderEntry, IEnumerable<IDiscountItem> discountItems)
        {
            if (orderEntry == null)
                throw new ArgumentNullException(nameof(orderEntry));

            return orderEntry.IncludedVat;
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
        public static decimal GetDiscountSum([NotNull] this IDiscountItem discountItem)
        {
            if (discountItem == null)
                throw new ArgumentNullException(nameof(discountItem));

            return discountItem.DiscountSums.Values.Sum();
        }

        [PublicAPI, Pure]
        public static bool IsDiscount([NotNull] this IDiscountItem discountItem)
        {
            if (discountItem == null)
                throw new ArgumentNullException(nameof(discountItem));

            return discountItem.GetDiscountSum() >= 0m;
        }

        [PublicAPI, Pure]
        public static decimal GetFullSum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.GetIncludedEntries().Sum(orderEntry => orderEntry.Cost);
        }

        [PublicAPI, Pure]
        public static decimal GetCategorizedDiscountsSum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.DiscountItems
                .Where(discountItem => discountItem.IsCategorized)
                .Sum(discountItem => discountItem.GetDiscountSum());
        }

        [PublicAPI, Pure]
        public static decimal GetNonCategorizedDiscountsSum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.DiscountItems
                .Where(discountItem => !discountItem.IsCategorized)
                .Sum(discountItem => discountItem.GetDiscountSum());
        }

        [PublicAPI, Pure]
        public static decimal GetVatSumExcludedFromPrice([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.GetIncludedEntries().Sum(orderEntry => orderEntry.ExcludedVat);
        }

        [PublicAPI, Pure]
        public static decimal GetPrepaySum([NotNull] this IOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return order.PrePayments
                .Where(payment => (payment.Status != PaymentItemStatus.Deleted && payment.Status != PaymentItemStatus.Storned) || payment.FailedToRemovePrepayOnStorno)
                .Sum(payment => payment.Sum);
        }

        [PublicAPI, Pure]
        public static decimal GetChangeSum([NotNull] this IOrder order, decimal resultSum)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var change = order.Payments.Sum(p => p.Sum) + GetPrepaySum(order) - resultSum;
            return Math.Max(change, decimal.Zero);
        }

        [PublicAPI, Pure, NotNull]
        public static string GetNameOrEmpty([CanBeNull] this IUser user)
        {
            return user?.Name ?? string.Empty;
        }

        [PublicAPI, Pure, NotNull]
        public static string NameSurname([NotNull] this ICustomer customer)
        {
            return string.Format(ChequeLocalResources.ClientFullNameFormat, customer.Surname, customer.Name).Trim();
        }

        [PublicAPI, Pure, NotNull]
        public static string StringView([NotNull] this IAddress address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            var fields = new List<Pair<string, string>>
            {
                Tuples.Pair(BrdLocalResources.ShortAddressLine1Format, address.Line1),
                Tuples.Pair(BrdLocalResources.ShortAddressLine2Format, address.Line2),
            };

            return StringExtensions.FormatPairs(BrdLocalResources.Line12AddressFormat, fields, BrdLocalResources.AddressPartsDelimiter);
        }

        [PublicAPI, Pure, NotNull]
        public static string GetKitchenOrDefaultName([NotNull] this IProduct product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return string.IsNullOrWhiteSpace(product.KitchenName) ? product.Name : product.KitchenName;
        }

        [PublicAPI, Pure]
        public static bool IsAmountIndependentOfParentAmount([NotNull] this IModifierEntry modifier)
        {
            if (modifier == null)
                throw new ArgumentNullException(nameof(modifier));

            if (modifier.SimpleModifier != null)
                return modifier.SimpleModifier.AmountIndependentOfParentAmount;

            Debug.Assert(modifier.ChildModifier != null);
            return modifier.ChildModifier.AmountIndependentOfParentAmount;
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<HashSet<IProductItem>> GetNotDeletedProductItemsByMix([NotNull] this IGuest guest)
        {
            if (guest == null)
                throw new ArgumentNullException(nameof(guest));

            HashSet<IProductItem> currentGroup = null;

            foreach (var productItem in guest.Items.Select(item => item as IProductItem))
            {
                if (productItem == null)
                {
                    if (currentGroup != null)
                    {
                        yield return currentGroup;
                        currentGroup = null;
                    }
                }
                else
                {
                    if (productItem.DeletionInfo != null)
                    {
                        if (currentGroup != null)
                        {
                            yield return currentGroup;
                            currentGroup = null;
                        }
                    }
                    else
                    {
                        if (currentGroup != null)
                            currentGroup.Add(productItem);
                        else
                            currentGroup = new HashSet<IProductItem> { productItem };

                        if (!productItem.HasMix || productItem.MixDeleted)
                        {
                            yield return currentGroup;
                            currentGroup = null;
                        }
                    }
                }
            }

            if (currentGroup != null)
                yield return currentGroup;
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<HashSet<IProductItem>> GetProductItemsByMix([NotNull] this IGuest guest)
        {
            if (guest == null)
                throw new ArgumentNullException(nameof(guest));

            HashSet<IProductItem> currentGroup = null;

            foreach (var productItem in guest.Items.Select(item => item as IProductItem))
            {
                if (productItem == null)
                {
                    if (currentGroup != null)
                    {
                        yield return currentGroup;
                        currentGroup = null;
                    }
                }
                else
                {
                    if (currentGroup != null)
                        currentGroup.Add(productItem);
                    else
                        currentGroup = new HashSet<IProductItem> { productItem };

                    if (!productItem.HasMix || productItem.MixDeleted)
                    {
                        yield return currentGroup;
                        currentGroup = null;
                    }
                }
            }

            if (currentGroup != null)
                yield return currentGroup;
        }

        [PublicAPI, Pure, NotNull]
        public static IEnumerable<HashSet<IProductItem>> GetProductItemsByMixOrCompound([NotNull] this IGuest guest)
        {
            if (guest == null)
                throw new ArgumentNullException(nameof(guest));

            HashSet<IProductItem> currentGroup = null;

            foreach (var productItem in guest.Items.Select(item => item as IProductItem))
            {
                if (productItem == null)
                {
                    if (currentGroup != null)
                    {
                        yield return currentGroup;
                        currentGroup = null;
                    }
                }
                else
                {
                    if (currentGroup != null)
                        currentGroup.Add(productItem);
                    else
                        currentGroup = new HashSet<IProductItem> { productItem };

                    if (productItem.HasMix && !productItem.MixDeleted || productItem.CompoundsInfo != null && productItem.CompoundsInfo.IsPrimaryComponent)
                        continue;

                    yield return currentGroup;
                    currentGroup = null;
                }
            }

            if (currentGroup != null)
                yield return currentGroup;
        }
    }
}