using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Common
{
    /// <summary>
    /// Класс, отображающий название организации, реквизиты и т.д.
    /// Работает по-разному, в зависимости от того, в каком Edition мы находимся -
    /// РМС или Чейн
    /// </summary>
    public sealed class CompanySetup : IDisposable
    {
        #region INSTANCE

        /// <summary>
        /// Единственный экземпляр класса
        /// </summary>
        private static CompanySetup instance;

        /// <summary>
        /// Создаем экземпляр классаю затем инициализируем его
        /// </summary>
        private static void CreateInstance()
        {
            instance = new CompanySetup();
            instance.InitCompanySetup();
            UpdateManager.INSTANCE.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        private static readonly object syncObj = new object();

        /// <summary>
        /// Получаем экземпляр класса
        /// </summary>
        public static CompanySetup INSTANCE
        {
            get
            {
                // блокируем, во избежание создания нескольких экземпляров из разных потоков
                lock (syncObj)
                {
                    // Если клас не создан, создаем его экземпляр
                    if (instance == null)
                    {
                        try
                        {
                            CreateInstance();
                        }
                        catch (Exception)
                        {
                            // Если произошла ошибка в процессе создания экземпляра класса, удаляем его
                            instance = null;
                            throw;
                        }
                    }

                    // Иначе возвращаем ранее созданный экземпляр класса

                    return instance;
                }
            }
        }

        #endregion

        #region Static Non Instance Methods

        /// <summary>
        /// Получаем заголовок компании
        /// Вызывается метод только тогда, когда мы не можем создать
        /// Экземпляр данного объекта,
        /// Из-за того, что не все еще загружено с сервера
        /// </summary>
        public static string CaptionForMainControl
        {
            get
            {
                string result = string.Empty;
                if (IsChain)
                {
                    result = Corporation != null ? Corporation.NameLocal : string.Empty;
                }
                else if (IsRMS)
                {
                    result = ServerInstance.INSTANCE.Department != null
                                 ? ServerInstance.INSTANCE.Department.NameLocal
                                 : string.Empty;
                }
                return result;
            }
        }

        /// <summary>
        /// Получаем корпорацию.
        /// Нельзя вызывать от INSTANCE,
        /// поскольку корпорацию в бэке нужно узнать раньше,
        /// чем создастся экземпляр класса
        /// </summary>
        public static Corporation Corporation
        {
            get { return corporation ?? (corporation = GetCorporationFromCache()); }
        }

        [CanBeNull]
        public static Corporation NullableCorporation => corporation;

        /// <summary>
        /// Получаем экземпляр корпорации из кэша
        /// </summary>
        private static Corporation GetCorporationFromCache()
        {
            var result = EntityManager.INSTANCE.GetAllNotDeleted<Corporation>().SingleOrDefault();

            if (result == null)
            {
                throw new RestoException("Corporation must be in one exemplar");
            }

            return result;
        }

        /// <summary>
        /// Заменяет сохраненную корпорацию на корпорацию из EntityManager, если они отличаются.
        /// </summary>
        public static void UpdateCorporatiotionIfNeeded()
        {
            var emCorporation = GetCorporationFromCache();
            if (corporation == null || corporation.Id.Equals(emCorporation.Id))
            {
                return;
            }
            corporation = emCorporation;
        }

        //Определение текущего времени документов из настроек для Chain
        public DateTime GetDocumentDefaultTime(DocumentType docType, DateTime curDate)
        {
            DateTime docTime = curDate;
            if (Corporation.DocumentSettings != null && Corporation.DocumentSettings.DefaultDocumentTimes.ContainsKey(docType))
            {
                DefaultDocumentTime defTime = Corporation.DocumentSettings.DefaultDocumentTimes[docType];
                if (defTime.TimeType != DocumentTimeType.CURRENT_TIME)
                {
                    docTime = (new DateTime(docTime.Year, docTime.Month, docTime.Day)).AddMinutes(defTime.DocumentTime.Minutes);
                }
            }
            return docTime;
        }

        public static DocumentSettings DocumentSettings
        {
            get
            {
                return IsChain ? Corporation.DocumentSettings : CafeSetup.INSTANCE.DocSettings;
            }
        }

        /// <summary>
        /// Проверка настройки <c>enable-replication</c> в RestoProperties
        /// </summary>
        /// <remarks>
        /// Для проверки на реплицируемость использовать <see cref="IsReplicationConfigured"/>
        /// </remarks>
        public static bool IsReplicationEnabled
        {
            get
            {
                if (IsRMS && isReplicationEnabled == null)
                {
                    isReplicationEnabled = ServiceClientFactory.ReplicationSlaveServerService.IsReplicationEnabled().CallSync();
                }

                return isReplicationEnabled.HasValue && isReplicationEnabled.Value;
            }
        }

        /// <summary>
        /// Настроена репликация для RMS.
        /// </summary>
        /// <remarks>
        /// В отличие от <see cref="IsReplicationEnabled"/> смотрит не на настройку в RestoProperties,
        /// а на наличие настроенного подключения к Чейну.
        /// </remarks>
        public static bool IsReplicationConfigured => IsRMS && ServerInstance.INSTANCE.ChainNode != null;

        /// <summary>
        /// Возвращает true, если лицензия IikoLite.
        /// </summary>
        public static bool IsIikoLite
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Возвращает true, если лицензия не <see cref="IsIikoLite"/> или <see cref="ModuleId.Evotor"/>.
        /// </summary>
        /// <remarks>
        /// пример расчёта параметра (поскольку нет именно такой лицензии как IikoLite):
        /// Если <see cref="ModuleId.Evotor"/>, то получаем = false || true, результат true
        /// Если <see cref="ModuleId.FullEdition"/>, то получаем = true || false, результат true
        /// Если <see cref="ModuleId.BackOffice"/>, то получаем = false || false, результат false
        /// </remarks>
        public static bool IsEvotorOrFull
        {
            get
            {
                bool isEvotor = false;
                return !IsIikoLite || isEvotor;
            }
        }

        /// <summary>
        /// Обновляет информацию.
        /// </summary>
        public void UpdateCompanySetup()
        {
            InitCompanySetup();
        }

        #endregion

        #region Members

        /// <summary>
        /// Корпорация
        /// </summary>
        private static Corporation corporation;

        /// <summary>
        /// ЮрЛицо
        /// </summary>
        private JurPerson jurPerson;

        /// <summary>
        /// Ресторан
        /// </summary>
        private DepartmentEntity department;

        /// <summary>
        /// Склад
        /// </summary>
        private Store store;

        /// <summary>
        /// Поддерживается ли репликация для РМС.
        /// </summary>
        private static bool? isReplicationEnabled;

        #endregion

        #region Constructor

        /// <summary>
        /// Запрещаем создавать экземпляры объекта извне
        /// </summary>
        private CompanySetup()
        {
        }

        /// <summary>
        /// Инициализируем объект класса
        /// </summary>
        private void InitCompanySetup()
        {
            if (IsRMS)
            {
                // В РМС инициализируем Корпорацию, Юрлицо и Подразделение из серверного объекта CorporatedHierarchy

                CorporatedHierarchy corporatedHierarchy =
                    ServiceClientFactory.CorporationService.GetRMSCorporatedHierarchy().CallSync();

                if (corporatedHierarchy == null)
                {
                    throw new RestoException("RMS Corporated Hierarchy does not exists");
                }

                jurPerson = corporatedHierarchy.JurPerson;
                department = corporatedHierarchy.Department;

                if (corporatedHierarchy.Corporation == null || jurPerson == null || department == null)
                {
                    throw new RestoException("RMS Corporated Hierarchy is not full");
                }
            }
        }

        #endregion

        public void Dispose()
        {
            instance = null;
        }

        #region Event Handlers

        private static void OnConnectionStatusChanged(bool error, Exception exception)
        {
            if (error)
            {
                isReplicationEnabled = null;
            }
        }

        #endregion Event Handlers

        #region Properties

        #region Corporation Objects

        /// <summary>
        /// Юрлицо
        /// </summary>
        public JurPerson JurPerson
        {
            get
            {
                Contract.Ensures(jurPerson != null);
                if (jurPerson == null)
                    throw new IllegalStateException("Context in Chain not set. Use Store, Department or JurPerson properties to set context.");
                return jurPerson;
            }
            set
            {
                // В РМС игнорируем установку ЮрЛица, поскольку оно инициализируется один раз при создании INSTANCE
                if (IsChain)
                {
                    jurPerson = value;
                }
            }
        }

        /// <summary>
        /// Подразделение.
        /// Для РМС возвращает торговое предприятие ресторана системы.
        /// </summary>
        public DepartmentEntity Department
        {
            get
            {
                if (IsRMS)
                {
                    return department;
                }

                // В чейн нельзя получить Подразделение, поскольку их может быть несколько
                throw new RestoException("Chain may have several instances of Department");
            }
            set
            {
                // В РМС игнорируем установку подразделения, поскольку оно инициализируется один раз при создании INSTANCE
                if (IsChain)
                {
                    department = value;

                    if (department != null)
                    {
                        JurPerson = department.GetJurPerson();
                    }
                    else
                    {
                        JurPerson = null;
                    }
                }
            }
        }

        #endregion

        #region Corporation Collections

        /// <summary>
        /// Юрлицо для каждого из подразделений
        /// </summary>
        public Dictionary<DepartmentEntity, JurPerson> JurPersonsByDepartment
        {
            get
            {
                var result = new Dictionary<DepartmentEntity, JurPerson>();

                if (IsRMS)
                {
                    if (department != null && jurPerson != null)
                    {
                        result.Add(department, jurPerson);
                    }
                }
                if (IsChain)
                {
                    foreach (var dep in MultiDepartments.Instance.ChainDepartments)
                    {
                        result.Add(dep, dep.GetJurPerson());
                    }
                }

                return result;
            }
        }

        #endregion

        #region Current Edition Company

        /// <summary>
        /// Низвание компании для заголовков
        /// </summary>
        public string CompanyName
        {
            get
            {
                if (IsRMS)
                {
                    return department != null ? department.NameLocal : string.Empty;
                }
                else if (IsChain)
                {
                    return Corporation != null ? Corporation.NameLocal : string.Empty;
                }

                return string.Empty;
            }
        }

        #endregion

        #region Jur Person Properties

        /// <summary>
        /// Название юрлица
        /// </summary>
        public string LegalName
        {
            get { return jurPerson != null ? jurPerson.NameLocal : string.Empty; }
        }

        /// <summary>
        /// Название юрлиц
        /// </summary>
        public string AllLegalNames
        {
            get
            {
                if (jurPerson != null)
                {
                    return jurPerson.NameLocal;
                }

                string jurPersons = string.Empty;

                if (IsChain)
                {
                    foreach (var jp in EntityManager.INSTANCE.GetAllNotDeleted<JurPerson>())
                    {
                        if (jurPersons.Length > 0)
                        {
                            jurPersons += ", ";
                        }

                        jurPersons += jp.NameLocal;
                    }
                }

                return jurPersons;
            }
        }

        /// <summary>
        /// Юр. адрес
        /// </summary>
        public string LegalAddress
        {
            get { return jurPerson != null ? jurPerson.Address : string.Empty; }
        }

        /// <summary>
        /// ИНН
        /// </summary>
        public string TaxId
        {
            get { return jurPerson != null ? jurPerson.Inn : string.Empty; }
        }

        /// <summary>
        /// Имя руководителя.
        /// </summary>
        [NotNull]
        public string LeaderName
        {
            get { return jurPerson != null ? jurPerson.LeaderName : string.Empty; }
        }

        /// <summary>
        /// Имя бухгалтера.
        /// </summary>
        [NotNull]
        public string AccountantName
        {
            get { return jurPerson != null ? jurPerson.AccountantName : string.Empty; }
        }

        /// <summary>
        /// Имя технолога.
        /// </summary>
        [NotNull]
        public string TechnologistName
        {
            get { return jurPerson != null ? jurPerson.TechnologistName : string.Empty; }
        }

        /// <summary>
        /// Имя заведующего производством.
        /// </summary>
        [NotNull]
        public string WorksManagerName
        {
            get { return jurPerson != null ? jurPerson.WorksManagerName : string.Empty; }
        }

        /// <summary>
        /// Подробная информация
        /// </summary>
        public string Details
        {
            get
            {
                return LegalName + (TaxId.Length > 0 ? Resources.CompanySetupINN + TaxId : string.Empty) +
                       (LegalAddress.Length > 0 ? "," + LegalAddress : string.Empty);
            }
        }

        #endregion

        #region Department Properties

        /// <summary>
        /// Название компании для отчетов.
        /// </summary>
        public string OrgDevelopment
        {
            get { return department != null ? department.NameLocal : string.Empty; }
        }

        /// <summary>
        /// Счет НДС
        /// </summary>
        public Account NdsAccount
        {
            get { return department != null ? department.VatAccumulator : null; }
        }

        /// <summary>
        /// Код подразделения
        /// </summary>
        public string DepartmentId
        {
            get { return department != null ? department.DepartmentId ?? string.Empty : string.Empty; }
        }

        /// <summary>
        /// Код подразделения
        /// </summary>
        public string NullableDepartmentId
        {
            get { return department != null ? department.DepartmentId : null; }
        }

        /// <summary>
        /// Адрес подразделения (фактический).
        /// </summary>
        public string Address
        {
            get { return department != null ? department.Address : string.Empty; }
        }

        #endregion

        #endregion Properties

        #region ChainSetters

        /// <summary>
        /// Установщик для Склада в Чейне
        /// </summary>
        public Store Store
        {
            set
            {
                if (IsChain)
                {
                    Department = null;
                    store = value;

                    if (store != null)
                    {
                        Department = store.GetDepartmentEntity();
                    }
                }
            }
        }

        #endregion

        #region Private Utils

        public static bool IsStandaloneRms => IsRMS && !ServerInstance.INSTANCE.Registered && ServerInstance.INSTANCE.ChainNode == null;

        public static bool IsRMS
        {
            get { return Edition.CurrentEdition == BackVisualizationMode.IIKO_RMS; }
        }

        public static bool IsChain
        {
            get { return Edition.CurrentEdition == BackVisualizationMode.IIKO_CHAIN; }
        }

        public static bool IsChainOrStandaloneRms => IsChain || IsStandaloneRms;

        /// <summary>
        /// Иконка приложения Rms или Chain (в зависимости от проекта)
        /// </summary>
        public static Icon AppIcon
        {
            get
            {
                return IsRMS ? Resources.BackOfficeIcon : Resources.ChainOfficeIcon;
            }
        }

        #endregion
    }
}
