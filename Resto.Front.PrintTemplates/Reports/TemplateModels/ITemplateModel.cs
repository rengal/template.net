using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Front.PrintTemplates.Reports.OlapReports;
using Resto.Front.PrintTemplates.RmsEntityWrappers;

namespace Resto.Front.PrintTemplates.Reports.TemplateModels
{
    public interface ITemplateModel
    {
        [NotNull]
        string Name { get; }

        [CanBeNull]
        ICashRegister CashRegister { get; }

        [CanBeNull]
        ICafeSession CafeSession { get; }

        DateTime CurrentTime { get; }

        [CanBeNull]
        IUser CurrentUser { get; }

        [NotNull]
        IGroup Group { get; }

        [NotNull]
        ICafeSetup CafeSetup { get; }

        [NotNull]
        string CurrentTerminal { get; }

        /// <summary>
        /// Нужна ли только разметка тела отчета
        /// </summary>
        bool IsOnlyBodyMarkupRequired { get; }

        /// <summary>
        /// Является ли X-отчёт окончательным для кассовой смены (Z-отчётом):
        /// <list type="table">
        ///     <item>
        ///         <term>true</term>
        ///         <description>Z-отчёт</description>
        ///     </item>
        ///     <item>
        ///         <term>false</term>
        ///         <description>X-отчёт</description>
        ///     </item>
        ///     <item>
        ///         <term>null</term>
        ///         <description>Отчёт не является X-отчётом</description>
        ///     </item>
        /// </list>
        /// </summary>
        bool? IsXReportFinal { get; }

        [CanBeNull]
        ISettings ReportSettings { get; }

        [NotNull]
        IEntitiesProvider Entities { get; }

        [NotNull]
        IEventsProvider Events { get; }

        [NotNull]
        ITransactionsProvider Transactions { get; }

        [NotNull]
        IOlapReportsProvider OlapReports { get; }

        bool UseNonFiscalCash { get; }
    }
}