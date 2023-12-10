using System;
using System.Diagnostics.Contracts;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class Store : ICorporatedEntityProps, IComparable
    {
        [Transient]
        private bool? shouldBeUsedForInternalTransfers;

        public Store(Store store)
            : this(GuidGenerator.Next(), new LocalizableValue(store.Name), store.Type, store.Description, store.NpeParent)
        {
        }

        public new string IdString
        {
            get
            {
                return Id.ToString();
            }
        }

        public string FullName
        {
            get
            {
                DepartmentEntity department = GetDepartmentEntity();
                return (department != null &&
                        (Edition.CurrentEdition == BackVisualizationMode.IIKO_CHAIN ||
                         !department.Id.Equals(CompanySetup.INSTANCE.Department.Id)))
                           ? string.Format("{0} ({1})", department.NameLocal, NameLocal)
                           : NameLocal;
            }
        }

        /// <summary>
        /// <see cref="GetDepartmentEntity"/>.
        /// </summary>
        public DepartmentEntity DepartmentEntity
        {
            get { return GetDepartmentEntity(); }
        }

        /// <summary>
        /// Возвращает признак возможности использования склада для внутренних перемещений между торговыми предприятиями.
        /// </summary>
        public bool IsUsedForInternalTransfers
        {
            get
            {
                return RepresentativeSupplier != null && !RepresentativeSupplier.Deleted;
            }
        }

        /// <summary>
        /// Возвращает или устанавливает признак, будет ли использоваться склад
        /// для внутренних перемещений между предприятиями после сохранения на сервере.
        /// </summary>
        public bool ShouldBeUsedForInternalTransfers
        {
            get
            {
                return shouldBeUsedForInternalTransfers
                       ?? IsUsedForInternalTransfers;
            }
            set
            {
                Contract.Requires(Edition.CurrentEdition == BackVisualizationMode.IIKO_CHAIN);
                shouldBeUsedForInternalTransfers = value;
            }
        }

        /// <summary>
        /// Возвращает склад для автоматического внутреннего перемещения (RMS-35989)
        /// </summary>
        public Object StoreForAutoTransfer
        {
            get
            {
                Contract.Requires(Edition.CurrentEdition == BackVisualizationMode.IIKO_CHAIN);
                if (IsUsedForInternalTransfers &&
                    (ExternalStoreGuid != null))
                {
                    return EntityManager.INSTANCE.Get<Store>(ExternalStoreGuid.GetValueOrFakeDefault());
                }
                return null;
            }
            set
            {
                Contract.Requires(Edition.CurrentEdition == BackVisualizationMode.IIKO_CHAIN);
                var store = value as Store;
                if (IsUsedForInternalTransfers &&
                    store != null)
                {
                    ExternalStoreGuid = store.Id;
                }
                else
                {
                    ExternalStoreGuid = null;
                }
            }
        }

        #region ICorporatedEntityProps Members

        public string CEDescription
        {
            get { return Description; }
            set { Description = value; }
        }

        public PersistedEntity CEParent
        {
            get { return NpeParent; }
            set
            {
                var corporatedEntity = value as CorporatedEntity;
                if (corporatedEntity == null)
                {
                    throw new ArgumentException("CEParent must be CorporatedEntity");
                }
                NpeParent = corporatedEntity;
            }
        }

        public CorporatedEntityType CEType
        {
            get { return CorporatedEntityType.STORE; }
        }

        #endregion

        private T GetCorporatedEntity<T>() where T : CorporatedEntity
        {
            var entity = NpeParent;

            // Проходимся вверх по дереву, пока не упремся в корень, или не найдем торговое предприятие
            while (entity != null)
            {
                if (entity is T result)
                    return result;

                entity = entity.Parent;
            }

            return null;
        }

        /// <summary>
        /// Возвращает юридическое лицо, связанное со складом.
        /// </summary>
        /// <returns>Юридическое лицо, связанное со складом.</returns>
        public JurPerson GetJurPerson()
        {
            return GetCorporatedEntity<JurPerson>();
        }

        /// <summary>
        /// Возвращает подразделение склада.
        /// Chain: Возвращает один из элементов дерева корпорации:
        /// - Торговое предприятие (<see cref="Department"/>);
        /// - Производство (<see cref="Manufacture"/>);
        /// - Центральный склад (<see cref="CentralStore"/>);
        /// - Центральный офис (<see cref="CentralOffice"/>).
        /// 
        /// RMS: Возвращает экземпляр <see cref="Department"/> (Содержит свойства ресторана из "мастера настройки ресторана").
        /// </summary>
        /// <remarks>
        /// См. в коде сервера CorporatedEntity.getDepartment() и DepartmentEntity.getDepartment().
        /// </remarks>
        /// <returns>Подразделение склада.</returns>
        [NotNull]
        public DepartmentEntity GetDepartmentEntity()
        {
            return GetCorporatedEntity<DepartmentEntity>()
                   ?? throw new InvalidOperationException($"Cannot get department for store {Name} ({Id}) with npe parent {NpeParent.GetType().Name} ({NpeParent.Id})");
        }

        /// <summary>
        /// Возвращает торговое предприятие склада.
        /// </summary>
        /// <returns>Торговое предприятие склада.</returns>
        public Department GetDepartment()
        {
            return GetCorporatedEntity<Department>();
        }

        public override string ToString()
        {
            return FullName;
        }

        public Store Copy()
        {
            return (Store)CreateCopy();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                // Так как у нас объекты кэшируются, то такая ситуация довольно частая
                return 0;
            }
            else if (obj == null)
            {
                return 1;
            }
            else if (!(obj is Store))
            {
                throw new ArgumentException(string.Format("Parameter must be of type {0}.", GetType().Name));
            }

            Store other = (Store)obj;
            int result = FullName.CompareTo(other.FullName);
            // В принципе разные склады могут иметь одно и то же имя...
            if (result == 0)
            {
                // Тогда смотрим на Id.
                result = Id.CompareTo(other.Id);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Является ли склад локальным, или принадлежит текущему подразделению (только для РМС)
        /// </summary>
        public bool IsLocalStore()
        {
            DepartmentEntity storeDepartment = GetDepartmentEntity();
            return storeDepartment == null || (storeDepartment is Department &&
                                               CompanySetup.INSTANCE.Department.Id.Equals(
                                                   storeDepartment.Id));
        }
    }
}