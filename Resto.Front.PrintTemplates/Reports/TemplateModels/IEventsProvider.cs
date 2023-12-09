using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public interface IEventsProvider
    {
        [PublicAPI, NotNull]
        IEnumerable<IItemSaleEvent> GetItemSaleEventsBySession([NotNull] ICafeSession session);

        /// <summary>
        /// Получить события внесений/изъятий по кассовой смене, у которых есть тип внесения/изъятия
        /// </summary>
        /// <param name="session">Кассовая смена</param>
        /// <returns></returns>
        [PublicAPI, NotNull]
        IEnumerable<IPayInOutEvent> GetPayInOutEventsBySession([NotNull] ICafeSession session);

        /// <summary>
        /// Получить все события внесений/изъятий по кассовой смене
        /// </summary>
        /// <param name="session">Кассовая смена</param>
        /// <returns></returns>
        [PublicAPI, NotNull]
        IEnumerable<IPayInOutEvent> GetAllPayInOutEventsBySession([NotNull] ICafeSession session);
    }
}