using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class AbstractScheduledPaymentRequest : IDeletable
    {
        public abstract ScheduledPaymentType PaymentType { get; }

        public override Store DocStore
        {
            get { return null; }
            set { }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return null; }
            set { }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get { return null; }
            set { }
        }

        public override string DocumentFullCaption
        {
            get
            {
                return ToString();
            }
        }

        /// <summary>
        /// Текст, представляющий дату или диапазон дат + периодичность
        /// </summary>
        [UsedImplicitly]
        public string DateText
        {
            get
            {
                var knownPeriod = KnownScheduledPeriod.Get(Period);
                return knownPeriod.IsPeriodic
                    ? string.Format("{0} - {1}, {2}", Start.ToShortDateString(), End.ToShortDateString(), knownPeriod.Name)
                    : Start.ToShortDateString();
            }
        }

        /// <summary>
        /// Торговые предприятия
        /// </summary>
        /// <remarks>
        /// "Все торговые предприятия", если <see cref="Departments"/> == null,
        /// иначе все подразделения через запятую
        /// </remarks>
        [UsedImplicitly]
        public string DepartmentsText
        {
            get
            {
                if (Departments == null)
                {
                    if (MultiDepartments.Instance.IsRmsOrSingleDepartmentMode || MultiDepartments.Instance.HasDepartmentFilter)
                    {
                        return MultiDepartments.Instance.ChainOrRmsDepartments
                            .Select(department => department.NameLocal)
                            .JoinWithComma();
                    }

                    return Resources.DepartmentAllTraders;
                }

                return Departments
                    .Intersect(MultiDepartments.Instance.ChainOrRmsDepartments)
                    .Select(department => department.NameLocal)
                    .JoinWithComma();
            }
        }

        public abstract AbstractScheduledPaymentRequest Copy();

        public override string ToString()
        {
            return string.Format(Resources.PayrollDocumentStringFormat,
                                 DocumentType.GetLocalName(),
                                 DocumentNumber,
                                 Start.ToShortDateString(),
                                 End.ToShortDateString());
        }
    }
}