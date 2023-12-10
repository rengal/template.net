using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public class IncomingInvoiceItemBuilder : InvoiceItemBuilder
    {
        internal IncomingInvoiceItemBuilder()
        {

        }

        protected override AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record, AbstractInvoiceDocument document)
        {
            return new IncomingInvoiceItem(
                record.ItemId ?? Guid.NewGuid(),
                record.Num,
                record.Product,
                record.InCount,
                record.InCountM,
                document,
                record.Store,
                (record.Product ?? new Product()).Num,
                record.Price,
                record.PriceWithoutVat,
                // AbstractInvoiceItem.PriceUnit нигде не используется (ни на бэке, ни на сервере)
                null,
                record.InPrice,
                "",
                "")
                       {
                           ContainerId = record.Container.Id,
                           ActualUnitWeight = record.GetActualUnitWeight(),
                           ActualAmount = record.ActualAmount ?? record.InCount,
                           CustomsDeclarationNumber = record.CustomsDeclarationNumber,
                           SupplierProduct = record.SupplierProduct,
                           OrderNum = record.OrderNum,
                           Producer = record.Producer,
                           AdditionalExpenses = record.AdditionalExpenses,
                           AdditionalExpensesNds = record.AdditionalExpensesNds,
                           IsAdditionalExpense = record.IsAdditionalExpense
            };
        }
    }

    public static class InvoiceHelper
    {
        public static decimal? GetActualUnitWeight(this InvoiceRecord record)
        {
            return record.Container != null && record.Product != null && record.Product.UseRangeForInvoices && !record.Container.IsEmptyContainer
                       ? record.ActualUnitWeight
                       : (decimal?)null;
        }
    }

    public class OutgoingInvoiceItemBuilder : InvoiceItemBuilder
    {
        internal OutgoingInvoiceItemBuilder()
        {

        }

        protected override AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record, AbstractInvoiceDocument document)
        {
            return new OutgoingInvoiceItem(
                record.ItemId ?? Guid.NewGuid(),
                record.Num,
                record.Product,
                record.InCount,
                record.InCountM,
                document,
                record.Store,
                record.Product.Num,
                record.Price,
                record.PriceWithoutVat,
                // AbstractInvoiceItem.PriceUnit нигде не используется (ни на бэке, ни на сервере)
                null,
                record.InPrice,
                record.ProductSize,
                record.AmountFactor)
            {
                ContainerId = record.Container.Id,
                ActualUnitWeight = record.GetActualUnitWeight(),
                ActualAmount = record.ActualAmount,
                Comment = record.Comment
            };
        }
    }

    public class IncomingServiceItemBuilder : InvoiceItemBuilder
    {
        internal IncomingServiceItemBuilder()
        {

        }

        protected override AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record, AbstractInvoiceDocument document)
        {
            return new IncomingServiceItem(
                record.ItemId ?? Guid.NewGuid(),
                record.Num,
                record.Product,
                record.InCount,
                record.InCountM,
                document,
                record.Store,
                (record.Product ?? new Product()).Num,
                record.Price,
                record.PriceWithoutVat,
                null,
                record.InPrice,
                record.Account)
            {
                ContainerId = record.Container.Id,
                ActualUnitWeight = record.GetActualUnitWeight(),
                SplitVat = record.SplitVat
            };
        }
    }

    public class OutgoingServiceItemBuilder : InvoiceItemBuilder
    {
        internal OutgoingServiceItemBuilder()
        {

        }

        protected override AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record, AbstractInvoiceDocument document)
        {
            return new OutgoingServiceItem(
                record.ItemId ?? Guid.NewGuid(),
                record.Num,
                record.Product,
                record.InCount,
                record.InCountM,
                document,
                record.Store,
                (record.Product ?? new Product()).Num,
                record.Price,
                record.PriceWithoutVat,
                null,
                record.InPrice,
                record.Account)
            {
                ContainerId = record.Container.Id,
                ActualUnitWeight = record.GetActualUnitWeight(), 
                SplitVat = record.SplitVat
            };
        }
    }

    public class SalesDocumentItemBuilder : InvoiceItemBuilder
    {
        internal SalesDocumentItemBuilder()
        {

        }

        protected override AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record, AbstractInvoiceDocument document)
        {
            return new SalesDocumentItem(
                record.ItemId ?? Guid.NewGuid(),
                record.Num,
                record.Product,
                record.InCount,
                record.InCountM,
                document,
                record.Store,
                record.Product.Num,
                record.Price,
                record.PriceWithoutVat,
                // AbstractInvoiceItem.PriceUnit нигде не используется (ни на бэке, ни на сервере)
                null,
                record.InPrice,
                record.ProductSize,
                record.AmountFactor)
            {
                ContainerId = record.Container.Id, 
                SplitVat = record.SplitVat
            };
        }
    }

    public abstract class InvoiceItemBuilder
    {
        internal InvoiceItemBuilder()
        {

        }

        public static InvoiceItemBuilder CreateItemBuilder(DocumentType documentType)
        {
            if (documentType == DocumentType.INCOMING_INVOICE)
                return new IncomingInvoiceItemBuilder();
            if (documentType == DocumentType.OUTGOING_INVOICE)
                return new OutgoingInvoiceItemBuilder();
            if (documentType == DocumentType.SALES_DOCUMENT)
                return new SalesDocumentItemBuilder();
            if (documentType == DocumentType.INCOMING_SERVICE)
                return new IncomingServiceItemBuilder();
            if (documentType == DocumentType.OUTGOING_SERVICE)
                return new OutgoingServiceItemBuilder();

            return null;
        }

        protected abstract AbstractInvoiceItem CreateInvoiceItemInternal(InvoiceRecord record,
                                                                AbstractInvoiceDocument document);

        public AbstractInvoiceItem CreateInvoiceItem(InvoiceRecord record,
            AbstractInvoiceDocument document)
        {
            AbstractInvoiceItem result = CreateInvoiceItemInternal(record, document);
            result.Discount = record.Discount;
            result.SumWithoutDiscount = record.InPrice;
            result.PriceWithoutDiscount = record.Price;
            result.NdsPercent = record.Nds;
            result.SumWithoutNds = record.SummWithoutNDS;

            return result;
        }

    }
}