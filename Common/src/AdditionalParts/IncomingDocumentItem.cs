using System;

using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public sealed class IncomingDocumentItem : IItemWithCost
    {
        #region Fields

        [NotNull]
        private readonly Container container;

        private decimal amountFactor = ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;

        #endregion

        #region Constructors

        /// <param name="productSize">
        /// Размер блюда.
        /// Если документ не поддерживает размеры блюд --- null.
        /// </param>
        /// <param name="amountFactor">
        /// Зафиксированный коэффициент списания.
        /// Если документ не поддерживает размеры блюд --- ProductSizeServerConstants.INSTANCE.DefaultAmountFactor.
        /// </param>
        public IncomingDocumentItem(
            Guid id,
            int number,
            Product product,
            ProductSize productSize,
            Decimal amountFactor,
            Decimal amount,
            MeasureUnit unit,
            Guid containerId,
            [CanBeNull] User producer = null
        )
            : this(
              id,
              number,
              product,
              productSize,
              amountFactor,
              amount,
              unit,
              product != null ? product.GetContainerById(containerId) : Container.GetEmptyContainer(),
              producer)
        {
        }

        /// <param name="productSize">
        /// Размер блюда.
        /// Если документ не поддерживает размеры блюд --- null.
        /// </param>
        /// <param name="amountFactor">
        /// Зафиксированный коэффициент списания.
        /// Если документ не поддерживает размеры блюд --- ProductSizeServerConstants.INSTANCE.DefaultAmountFactor.
        /// </param>
        public IncomingDocumentItem(
            Guid id,
            int number,
            Product product,
            ProductSize productSize,
            Decimal amountFactor,
            Decimal amount,
            MeasureUnit unit,
            [NotNull] Container container,
            [CanBeNull] User producer = null)
        {
            Deleted = false;
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            Id = id;
            Number = number;
            Product = product;
            ProductSize = productSize;
            AmountFactor = amountFactor;
            Amount = amount;
            Unit = unit;
            Producer = producer;
            this.container = container;
        }

        #endregion

        #region Properties

        public Guid Id { get; set; }

        public decimal MainProductAmountPercent { get; set; }

        public decimal ContainerAmount { get; set; }

        [NotNull]
        public Container Container
        {
            get { return container; }
        }

        public int Number { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// Размер блюда.
        /// </summary>
        /// <remarks>
        /// Можно задавать только в документах списания при списании по ингредиентам.
        /// </remarks>
        public ProductSize ProductSize { get; set; }

        /// <summary>
        /// Зафиксированный коэффициент списания блюда.
        /// </summary>
        /// <remarks>
        /// Можно задавать только в документах списания при списании по ингредиентам.
        /// </remarks>
        public decimal AmountFactor
        {
            get { return amountFactor; }
            set { amountFactor = value; }
        }

        public decimal Amount { get; set; }

        public MeasureUnit Unit { get; set; }

        public string ContainerName { get; set; }

        public User User { get; set; }

        public DateTime? PayRollDate { get; set; }

        public bool Deleted { get; set; }

        /// <summary>
        /// Себестоимость за единицу продукта
        /// </summary>
        public decimal CostForOne { get; set; }

        /// <summary>
        /// Себестоимость за весь продукт
        /// </summary>
        public decimal CostWhole { get; set; }

        /// <summary>
        /// Производитель/Изготовитель
        /// </summary>
        public User Producer { get; set; }

        #endregion
    }
}