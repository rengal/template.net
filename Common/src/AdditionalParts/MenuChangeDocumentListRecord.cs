using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.UI.Common;

namespace Resto.Data
{
    public partial class MenuChangeDocumentListRecord
    {
        [UsedImplicitly]
        public override string DepartmentsString
        {
            get
            {
                if (MultiDepartments.Instance.ChainOrRmsSingleDepartment != null)
                {
                    return MultiDepartments.Instance.ChainOrRmsSingleDepartment.NameLocal;
                }

                var currentDepartment = MultiDepartments.Instance.ChainDepartments.ToArray();
                if (MultiDepartments.Instance.HasDepartmentFilter || departments.Count != currentDepartment.Length)
                {
                    return departments.Intersect(currentDepartment)
                        .Select(department => department.NameLocal)
                        .JoinWithComma();
                }

                return DepartmentEntity.ALL_DEPARTMENT_ENTITIES;
            }
        }

        public override DateTime? DateToValue
        {
            get
            {
                return DateTo;
            }
        }

        public override string ScheduleNameValue
        {
            get
            {
                return ScheduleName;
            }
        }

        public override List<Store> StoresList
        {
            get
            {
                return departments.SelectMany(department => department.GetDepartmentStoresByHierarchy()).ToList();
            }
        }

        /// <summary>
        /// <para>Возвращает статус приказа об изменении прейскуранта.</para>
        /// <para>Статус вычисляется на основании признаков Deleted и Processed.</para>
        /// </summary>
        public DocumentStatus Status
        {
            get
            {
                Contract.Assert(!(Deleted && Processed), "A document cannot be deleted and processed at the same time.");

                DocumentStatus result = DocumentStatus.NEW;

                if (Deleted)
                {
                    result = DocumentStatus.DELETED;
                }
                else if (Processed)
                {
                    result = DocumentStatus.PROCESSED;
                }

                return result;
            }
        }
    }
}