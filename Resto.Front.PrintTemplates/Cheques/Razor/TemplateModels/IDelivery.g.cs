// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using Resto.Front.PrintTemplates.RmsEntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels
{
    /// <summary>
    /// Доставка
    /// </summary>
    public interface IDelivery
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid DeliveryId { get; }

        /// <summary>
        /// Курьер
        /// </summary>
        [CanBeNull]
        IUser Courier { get; }

        /// <summary>
        /// Оператор доставки
        /// </summary>
        [CanBeNull]
        IUser DeliveryOperator { get; }

        /// <summary>
        /// Статус
        /// </summary>
        DeliveryStatus Status { get; }

        /// <summary>
        /// Номер
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CanBeNull]
        string Comment { get; }

        /// <summary>
        /// Время создания доставки
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// Время подтверждения доставки
        /// </summary>
        DateTime? ConfirmTime { get; }

        /// <summary>
        /// Время, к которому надо доставить заказ
        /// </summary>
        DateTime DeliverTime { get; }

        /// <summary>
        /// Время последней сервисной печати
        /// </summary>
        DateTime? PrintTime { get; }

        /// <summary>
        /// Время окончания приготовления доставки
        /// </summary>
        DateTime? CookingFinishTime { get; }

        /// <summary>
        /// Время печати накладной
        /// </summary>
        DateTime? BillTime { get; }

        /// <summary>
        /// Фактическое время доставки
        /// </summary>
        DateTime? ActualDeliverTime { get; }

        /// <summary>
        /// Время отмены доставки
        /// </summary>
        DateTime? CancelTime { get; }

        /// <summary>
        /// Время отправки курьера
        /// </summary>
        DateTime? CourierSendTime { get; }

        /// <summary>
        /// Время закрытия доставки
        /// </summary>
        DateTime? CloseTime { get; }

        /// <summary>
        /// Причина отмены
        /// </summary>
        [CanBeNull]
        string CancelCause { get; }

        /// <summary>
        /// Клиент
        /// </summary>
        [NotNull]
        ICustomer Customer { get; }

        /// <summary>
        /// Адрес доставки
        /// </summary>
        [CanBeNull]
        IAddress Address { get; }

        /// <summary>
        /// Телефон для связи с клиентом
        /// </summary>
        [NotNull]
        string Phone { get; }

        /// <summary>
        /// Email для связи с клиентом
        /// </summary>
        [CanBeNull]
        string Email { get; }

        /// <summary>
        /// Признак проблемной доставки
        /// </summary>
        bool HasProblem { get; }

        /// <summary>
        /// Описание проблемы
        /// </summary>
        [CanBeNull]
        string ProblemComment { get; }

        /// <summary>
        /// Количество персон
        /// </summary>
        int PersonCount { get; }

        /// <summary>
        /// Нужно ли разбивать заказ по гостям
        /// </summary>
        bool SplitBetweenPersons { get; }

        /// <summary>
        /// Признак самовывоза
        /// </summary>
        bool IsSelfService { get; }

        /// <summary>
        /// Заказ
        /// </summary>
        [NotNull]
        IOrder Order { get; }

        /// <summary>
        /// Прогнозируемое время окончания приготовления доставки
        /// </summary>
        DateTime? PredictedCookingCompleteTime { get; }

    }

    internal sealed class Delivery : TemplateModelBase, IDelivery
    {
        #region Fields
        private readonly Guid deliveryId;
        private readonly User courier;
        private readonly User deliveryOperator;
        private readonly DeliveryStatus status;
        private readonly int number;
        private readonly string comment;
        private readonly DateTime created;
        private readonly DateTime? confirmTime;
        private readonly DateTime deliverTime;
        private readonly DateTime? printTime;
        private readonly DateTime? cookingFinishTime;
        private readonly DateTime? billTime;
        private readonly DateTime? actualDeliverTime;
        private readonly DateTime? cancelTime;
        private readonly DateTime? courierSendTime;
        private readonly DateTime? closeTime;
        private readonly string cancelCause;
        private readonly Customer customer;
        private readonly Address address;
        private readonly string phone;
        private readonly string email;
        private readonly bool hasProblem;
        private readonly string problemComment;
        private readonly int personCount;
        private readonly bool splitBetweenPersons;
        private readonly bool isSelfService;
        private readonly Order order;
        private readonly DateTime? predictedCookingCompleteTime;
        #endregion

        #region Ctor
        [UsedImplicitly]
        private Delivery()
        {}

        private Delivery([NotNull] CopyContext context, [NotNull] IDelivery src)
        {
            System.Diagnostics.Debug.Assert(context != null);
            System.Diagnostics.Debug.Assert(src != null);

            context.Register(src, this);

            deliveryId = src.DeliveryId;
            courier = context.GetConverted(src.Courier, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            deliveryOperator = context.GetConverted(src.DeliveryOperator, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.User.Convert);
            status = src.Status;
            number = src.Number;
            comment = src.Comment;
            created = src.Created;
            confirmTime = src.ConfirmTime;
            deliverTime = src.DeliverTime;
            printTime = src.PrintTime;
            cookingFinishTime = src.CookingFinishTime;
            billTime = src.BillTime;
            actualDeliverTime = src.ActualDeliverTime;
            cancelTime = src.CancelTime;
            courierSendTime = src.CourierSendTime;
            closeTime = src.CloseTime;
            cancelCause = src.CancelCause;
            customer = context.GetConverted(src.Customer, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Customer.Convert);
            address = context.GetConverted(src.Address, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Address.Convert);
            phone = src.Phone;
            email = src.Email;
            hasProblem = src.HasProblem;
            problemComment = src.ProblemComment;
            personCount = src.PersonCount;
            splitBetweenPersons = src.SplitBetweenPersons;
            isSelfService = src.IsSelfService;
            order = context.GetConverted(src.Order, Resto.Front.PrintTemplates.Cheques.Razor.TemplateModels.Order.Convert);
            predictedCookingCompleteTime = src.PredictedCookingCompleteTime;
        }

        [CanBeNull]
        [ContractAnnotation("source:null => null; source:notnull => notnull")]
        internal static Delivery Convert([NotNull] CopyContext context, [CanBeNull] IDelivery source)
        {
            if (source == null)
                return null;

            return new Delivery(context, source);
        }
        #endregion

        #region Props
        public Guid DeliveryId
        {
            get { return deliveryId; }
        }

        public IUser Courier
        {
            get { return courier; }
        }

        public IUser DeliveryOperator
        {
            get { return deliveryOperator; }
        }

        public DeliveryStatus Status
        {
            get { return status; }
        }

        public int Number
        {
            get { return number; }
        }

        public string Comment
        {
            get { return GetLocalizedValue(comment); }
        }

        public DateTime Created
        {
            get { return created; }
        }

        public DateTime? ConfirmTime
        {
            get { return confirmTime; }
        }

        public DateTime DeliverTime
        {
            get { return deliverTime; }
        }

        public DateTime? PrintTime
        {
            get { return printTime; }
        }

        public DateTime? CookingFinishTime
        {
            get { return cookingFinishTime; }
        }

        public DateTime? BillTime
        {
            get { return billTime; }
        }

        public DateTime? ActualDeliverTime
        {
            get { return actualDeliverTime; }
        }

        public DateTime? CancelTime
        {
            get { return cancelTime; }
        }

        public DateTime? CourierSendTime
        {
            get { return courierSendTime; }
        }

        public DateTime? CloseTime
        {
            get { return closeTime; }
        }

        public string CancelCause
        {
            get { return GetLocalizedValue(cancelCause); }
        }

        public ICustomer Customer
        {
            get { return customer; }
        }

        public IAddress Address
        {
            get { return address; }
        }

        public string Phone
        {
            get { return GetLocalizedValue(phone); }
        }

        public string Email
        {
            get { return GetLocalizedValue(email); }
        }

        public bool HasProblem
        {
            get { return hasProblem; }
        }

        public string ProblemComment
        {
            get { return GetLocalizedValue(problemComment); }
        }

        public int PersonCount
        {
            get { return personCount; }
        }

        public bool SplitBetweenPersons
        {
            get { return splitBetweenPersons; }
        }

        public bool IsSelfService
        {
            get { return isSelfService; }
        }

        public IOrder Order
        {
            get { return order; }
        }

        public DateTime? PredictedCookingCompleteTime
        {
            get { return predictedCookingCompleteTime; }
        }

        #endregion
    }

}
