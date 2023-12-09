using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Data;
using Resto.Front.Localization.Resources;
#pragma warning disable 618

namespace Resto.Front.PrintTemplates.Reports
{
    /// <summary>
    /// Стандартные отчёты, переведённые на razor-шаблоны
    /// </summary>
    [UsedImplicitly]
    public static class DefaultReports
    {
        private static readonly Dictionary<ReportSource, Func<FrontReport>> ReportSourceToSettingsFactory;

        static DefaultReports()
        {
            ReportSourceToSettingsFactory =
                new Dictionary<ReportSource, Func<FrontReport>>
                {
                    { ReportSource.X_REPORT, GetXReport },
                    { ReportSource.SESSION_WRITEOFF_REPORT, GetSessionWriteoffReport },
                    { ReportSource.SERVER_WRITEOFF_REPORT, GetServerWriteoffReport },
                    { ReportSource.SESSION_SALES_REPORT, GetSessionSalesReport },
                    { ReportSource.SERVER_SALES_REPORT, GetServerSalesReport },
                    { ReportSource.DISH_EXPENSE_SESSION_REPORT, GetSessionExpenseReport },
                    { ReportSource.DISH_EXPENSE_SERVER_REPORT, GetServerExpenseReport },
                    { ReportSource.DELIVERY_REPORT, GetSessionDeliveriesReport },
                    { ReportSource.SALES_BY_WAITER_SERVER_REPORT, GetSalesByWaiterServerReport },
                    { ReportSource.SALES_BY_TYPE_WITH_TAX_SESSION_REPORT, GetSalesByTypeWithTaxSessionReport },
                    { ReportSource.SALES_BY_TYPE_WITH_TAX_SERVER_REPORT, GetSalesByTypeWithTaxServerReport },
                    { ReportSource.ORDER_GAP_REPORT, GetOrderGapReport },
                    { ReportSource.SESSION_DISCOUNT_REPORT, GetSessionDiscountReport },
                    { ReportSource.PROBLEM_OPERATIONS_REPORT, GetProblemOperationsReport },
                    { ReportSource.EMPLOYEE_DEBTS_REPORT, GetEmployeeDebtsReport },
                    { ReportSource.DELIVERY_DISHES_REPORT, GetDeliveryDishesReport },
                    { ReportSource.ORDERS_SUMMARY_REPORT, GetOrdersSummaryReport },
                    { ReportSource.SERVER_HOURLY_REPORT, GetServerHourlyReport },
                    { ReportSource.SESSION_HOURLY_REPORT, GetSessionHourlyReport },
                    { ReportSource.SALES_BY_PAYMENT_TYPE_SESSION_REPORT, GetSalesByPaymentTypeSessionReport },
                    { ReportSource.SALES_BY_PAYMENT_TYPE_SERVER_REPORT, GetSalesByPaymentTypeServerReport },
                    { ReportSource.CAFE_SESSION_SALES_REGISTER_REPORT, GetSessionSalesRegisterReport },
                    { ReportSource.CAFE_SESSION_SALES_EXTENDED_REGISTER_REPORT, GetSessionSalesExtendedRegisterReport },
                    { ReportSource.PAY_IN_OUT_REPORT, GetPayInOutReport },
                    { ReportSource.CAFE_SESSION_SUMMARY_REPORT, GetCafeSessionSummaryReport },
                    { ReportSource.EGAIS_PRODUCT_UNSEAL_REPORT, GetEgaisProductUnsealReport },
                    { ReportSource.DONATIONS_REPORT, GetDonationsReport },
                };
        }

        [NotNull, UsedImplicitly]
        public static IEnumerable<ReportSource> ReportSources => ReportSourceToSettingsFactory.Keys;

        [NotNull, UsedImplicitly]
        public static FrontReport GetReportFor([NotNull] ReportSource reportSource)
        {
            if (reportSource == null)
                throw new ArgumentNullException(nameof(reportSource));

            if (ReportSourceToSettingsFactory.TryGetValue(reportSource, out var settingsFactory))
                return settingsFactory();

            throw new ArgumentOutOfRangeException(nameof(reportSource), reportSource, $"Default report for '{reportSource}' doesn't exist.");
        }

        [NotNull]
        private static FrontReport GetXReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.XReport
                };
        }

        private static FrontReport GetSessionWriteoffReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionWriteoffReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetServerWriteoffReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.ServerWriteoffReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            },
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSessionSalesReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionSalesReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowAdvancePays",
                                            label: ReportLocalResources.ShowAdvancePays)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetServerSalesReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.ServerSalesReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            },
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowAdvancePays",
                                            label: ReportLocalResources.ShowAdvancePays)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSessionExpenseReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionExpenseReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetServerExpenseReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.ServerExpenseReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            },
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                               Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CustomEnumReportParameter(
                                            name: "GroupDishes",
                                            label: ReportLocalResources.Group)
                                        {
                                            Persistent = true,
                                            Values =
                                                new List<CustomEnumReportParameterValue>
                                                {
                                                    new CustomEnumReportParameterValue("GroupByDishes", ReportLocalResources.GroupByDishes) { IsDefault = true },
                                                    new CustomEnumReportParameterValue("GroupByCategories", ReportLocalResources.GroupByCategories),
                                                    new CustomEnumReportParameterValue("GroupByWaiters", ReportLocalResources.GroupByWaiters),
                                                    new CustomEnumReportParameterValue("GroupByCookingPlaceTypes", ReportLocalResources.GroupByCookingPlaceTypes)
                                                }
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowOneDishPrice",
                                            label: ReportLocalResources.ShowOneDishPrice)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSessionDeliveriesReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.SessionDeliveriesReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new BooleanReportParameter(
                                            name: "ShowClosedDeliveries",
                                            label: ReportLocalResources.ShowClosedDeliveries)
                                        {
                                            Persistent = true,
                                            DefaultValue = true
                                        },
                                        new BooleanReportParameter(
                                            name: "ShowCancelledDeliveries",
                                            label: ReportLocalResources.ShowCancelledDeliveries)
                                        {
                                            Persistent = true,
                                            DefaultValue = true
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSalesByWaiterServerReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.SalesByWaiterServerReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSalesByTypeWithTaxSessionReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SalesByTypeWithTaxSessionReport
                };
        }

        private static FrontReport GetSalesByTypeWithTaxServerReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.SalesByTypeWithTaxServerReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetOrderGapReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.OrderGapReport
                };
        }

        private static FrontReport GetSessionDiscountReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.SessionDiscountReport
                };
        }

        private static FrontReport GetProblemOperationsReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.ProblemOperationsReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.ProblemOperationsSettingsStepName,
                                prompt: ReportLocalResources.ProblemOperationsSettingsStepHint)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new TerminalsScopeReportParameter(
                                            name: "Scope",
                                            label: ReportLocalResources.Terminals,
                                            defaultValue: TerminalsScopeReportParameterValue.CURRENT_TERMINAL)
                                        {
                                            Persistent = false
                                        },
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.CAFE_SESSION)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = true
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetEmployeeDebtsReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.EmployeeDebtsReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new CounteragentsReportParameter(
                                            name: "Employees",
                                            label: ReportLocalResources.Employees,
                                            counteragentType: CounteragentsReportParameterKind.EMPLOYEE)
                                        {
                                            Persistent = true
                                        },
                                        new BooleanReportParameter(
                                            name: "DetailByOrders",
                                            label: ReportLocalResources.DetailByOrders)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        },
                                        new BooleanReportParameter(
                                            name: "OnlyNonZeroDebt",
                                            label: ReportLocalResources.OnlyNonZeroDebt)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetDeliveryDishesReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.DeliveryDishesReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.CURRENT_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            },
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new BooleanReportParameter(
                                            name: "GroupByCookingPlaces",
                                            label: ReportLocalResources.GroupByCookingPlaces)
                                        {
                                            Persistent = false,
                                            DefaultValue = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetOrdersSummaryReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.OrdersSummaryReport
                };
        }

        private static FrontReport GetServerHourlyReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.ServerHourlyReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            },
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new BooleanReportParameter(
                                            name: "ShowAverage",
                                            label: ReportLocalResources.ShowAverage)
                                        {
                                            Persistent = true,
                                            DefaultValue = true
                                        },
                                        new BooleanReportParameter(
                                            name: "TakeIntoAccountHoursWithZeroSells",
                                            label: ReportLocalResources.TakeIntoAccountHoursWithZeroSells)
                                        {
                                            Persistent = true,
                                            DefaultValue = true
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSessionHourlyReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionHourlyReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.AdditionalSettings,
                                prompt: ReportLocalResources.SpecifyAdditionalSettings)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new BooleanReportParameter(
                                            name: "ShowAverage",
                                            label: ReportLocalResources.ShowAverage)
                                        {
                                            Persistent = true,
                                            DefaultValue = false
                                        },
                                        new BooleanReportParameter(
                                            name: "TakeIntoAccountHoursWithZeroSells",
                                            label: ReportLocalResources.TakeIntoAccountHoursWithZeroSells)
                                        {
                                            Persistent = true,
                                            DefaultValue = true
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSalesByPaymentTypeServerReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_SERVER,
                    Template = Templates.SalesByPaymentTypeServerReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.BUSINESS_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetSalesByPaymentTypeSessionReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SalesByPaymentTypeSessionReport
                };
        }

        private static FrontReport GetSessionSalesRegisterReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionSalesRegisterReport
                };
        }

        private static FrontReport GetSessionSalesExtendedRegisterReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.SessionSalesExtendedRegisterReport
                };
        }

        private static FrontReport GetPayInOutReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.PayInOutReport
                };
        }

        private static FrontReport GetCafeSessionSummaryReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.CafeSessionSummaryReport
                };
        }

        private static FrontReport GetEgaisProductUnsealReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_ORDERS,
                    Template = Templates.EgaisProductUnsealReport,
                    Pages =
                        new List<ReportParametersPage>
                        {
                            new ReportParametersPage(
                                name: ReportLocalResources.Period,
                                prompt: ReportLocalResources.SpecifyPeriod)
                            {
                                Parameters =
                                    new List<ReportParameter>
                                    {
                                        new DateTimePeriodReportParameter(
                                            name: "ReportInterval",
                                            label: ReportLocalResources.Period,
                                            defaultValue: DateTimePeriodReportParameterValue.CURRENT_DAY)
                                        {
                                            Persistent = false,
                                            ShowTimeEditor = false
                                        }
                                    }
                            }
                        }
                };
        }

        private static FrontReport GetDonationsReport()
        {
            return
                new FrontReport
                {
                    ReportRequirements = ReportRequirements.REQUIRES_EVENTS,
                    Template = Templates.DonationsReport
                };
        }
    }
}