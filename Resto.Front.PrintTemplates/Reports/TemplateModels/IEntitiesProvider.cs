using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public interface IEntitiesProvider
    {
        [PublicAPI, NotNull]
        IEnumerable<IReserve> GetAllReserves();

        [PublicAPI, NotNull]
        IEnumerable<IOrder> GetAllOrders();

        [PublicAPI, NotNull]
        IEnumerable<IOrder> GetAllNotDeletedNotBoundToNonStartedReservesOrdersBySession([NotNull] ICafeSession session);

        [PublicAPI, NotNull]
        IEnumerable<IRestaurantSection> GetAllNotDeletedSectionsWithAnyTablesByGroup([NotNull] IGroup group);
        
        [PublicAPI, NotNull]
        IEnumerable<IPaymentType> GetAllPaymentTypes();

        [PublicAPI, NotNull]
        IEnumerable<IPaymentSystem> GetAllPaymentSystems();

        [PublicAPI, NotNull]
        IEnumerable<IDelivery> GetAllNotDeletedDeliveries();

        [PublicAPI, NotNull]
        IEnumerable<IEgaisUnsealProduct> GetAllEgaisUnsealedProducts();
        
        [PublicAPI, NotNull]
        IEnumerable<ICommonGroup> GetProblemOperationsEvents(bool allTerminalsEvents, DateTime dateBegin, DateTime dateEnd);

        [PublicAPI, NotNull]
        IEnumerable<IWaiterDebtItem> GetAllWaiterDebtItemsByCashRegister([NotNull] ICashRegister cashRegister);
    }
}