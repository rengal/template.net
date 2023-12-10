using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;
using OfficeCommonResources = Resto.Common.Properties.Resources;

namespace Resto.Data
{
    public partial class User : IComparable
    {
        #region Properties

        /// <summary>
        /// <para>Возвращает <c>true</c>, если текущий пользователь является администратором системы, и <c>false</c> в противном случае.</para>
        /// <para>Пользователь считается системным администратором, если истинно хотя бы одно из следующих условий:</para>
        /// <para>1. учетная запись пользователя - встроенная учетная запись системного администратора;</para>
        /// <para>2. пользователь назначен на должность системного администратора.</para>
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return Administrator
                       || (MainRole != null && MainRole.Administrator)
                       || Roles.Any(r => r.Administrator);
            }
        }

        /// <summary>
        /// <para>Признак того, что данная учетная запись является ВСТРОЕННОЙ учетной записью администратора.</para>
        /// <para>ВНИМАНИЕ! Данное свойство может быть не задано для пользователя, созданного вручную 
        /// и обладающего встроенной ролью (должностью) системного администратора.</para>
        /// <para>Поэтому для проверки на наличие прав системного администратора необходимо использовать свойство <see cref="IsAdministrator"/>.</para>
        /// </summary>
        /// <remarks>
        /// Данное свойство добавлено исключительно ради того, чтобы разработчики видели предупреждение в XML-комментарии.
        /// </remarks>
        public new bool Administrator
        {
            get { return base.Administrator; }
            set { base.Administrator = value; }
        }

        public string IdString
        {
            get { return Id.ToString(); }
        }

        public UserType UserType
        {
            get
            {
                return (Employee ? UserType.EMPLOYEE : UserType.NONE)
                       | (Client ? UserType.CLIENT : UserType.NONE)
                       | (Supplier ? UserType.SUPPLIER : UserType.NONE);
            }
        }

        public string Type
        {
            get
            {
                string result = "";
                if (employee)
                {
                    result += Resources.UserTypeEmployee;
                }
                if (supplier)
                {
                    if (result.Length != 0) result += ",";
                    result += Resources.UserTypeSupplier;
                }
                if (client)
                {
                    if (result.Length != 0) result += ",";
                    result += Resources.UserTypeGuest;
                }
                return result;
            }
        }

        public string Slip
        {
            get { return AuthCard != null && AuthCard.Slip != null ? AuthCard.Slip : string.Empty; }
        }

        public string Role
        {
            get { return MainRole != null ? MainRole.NameLocal: null; }
        }

        public string AssignedToDepartmentsText
        {
            get
            {
                var departments = AssignedToDepartments ?? EntityManager.INSTANCE.GetAllNotDeleted<DepartmentEntity>().ToHashSet();

                var singleDepartment = MultiDepartments.Instance.ChainOrRmsSingleDepartment;
                return singleDepartment != null
                           ? singleDepartment.ToString()
                           : departments.OrderBy(department => department.NameLocal).AsString();
            }
        }

        public string ResponsibilityDepartmentsText
        {
            get
            {
                var singleDepartment = MultiDepartments.Instance.ChainOrRmsSingleDepartment;
                if (singleDepartment != null)
                {
                    return singleDepartment.ToString();
                }

                return ResponsibilityDepartments != null
                           ? ResponsibilityDepartments.AsString()
                           : DepartmentEntity.ALL_DEPARTMENT_ENTITIES;
            }
        }

        public string PriceCategoryText
        {
            get
            {
                return PriceCategory == null
                           ? OfficeCommonResources.PriceCategoryModelAllPriceCategories
                           : PriceCategory.NameLocal;
            }
        }

        public string ConceptionText
        {
            get
            {
                return Conception == null ? Conception.NoConception.NameLocal: Conception.NameLocal;
            }
        }

        public bool HasPermission(Permission permission)
        {
            if (permission == null)
            {
                return true;
            }
            if (Denied(permission))
            {
                return false;
            }
            if (Allowed(permission))
            {
                return true;
            }
            if (MainRole != null && MainRole.Denied(permission))
            {
                return false;
            }
            if (Roles.Any(role => role.Denied(permission)))
            {
                return false;
            }

            if (MainRole != null && MainRole.Allowed(permission))
            {
                return true;
            }
            return Roles.Any(role => role.Allowed(permission));
        }

        public AllowableDeviationAction NotNullableDeviationAction
        {
            get { return DeviationAction ?? (DeviationAction = AllowableDeviationAction.DISABLE); }
        }

        [NotNull]
        public PbxAuthorizationSettings NotNullPbxAuthorizationSettings
        {
            get { return PbxAuthorizationSettings ?? (PbxAuthorizationSettings = new PbxAuthorizationSettings()); }
        }

        /// <summary>
        /// Cерия и номер (паспорта/лицензии).
        /// </summary>
        public string PassportSeriesNumber
        {
            get
            {
                if(passportInfo == null)
                {
                    return string.Empty;
                }

                var result = passportInfo.Series;

                if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(passportInfo.Number))
                    result += ", " + passportInfo.Number;
                else if (!string.IsNullOrEmpty(passportInfo.Number))
                    return passportInfo.Number;

                return result ?? string.Empty;
            }
        }

        /// <summary>
        /// Дата окончания (паспорта/лицензии).
        /// </summary>
        public DateTime? PassportValidTillDate
        {
            get
            {
                return passportInfo == null || passportInfo.ValidTillDate == null
                           ? (DateTime?) null
                           : passportInfo.ValidTillDate.ToDateTime();
            }
        }

        /// <summary>
        /// Дата выдачи (паспорта/лицензии).
        /// </summary>
        public DateTime? PassportIssueDate
        {
            get
            {
                return passportInfo == null || PassportInfo.IssueDate == null
                           ? (DateTime?) null
                           : passportInfo.IssueDate.ToDateTime();
            }
        }

        /// <summary>
        /// Организация выдавшая (паспорт/лицензию).
        /// </summary>
        public string PassportIssuer
        {
            get
            {
                return passportInfo == null ? string.Empty : passportInfo.Issuer;
            }
        }

        /// Фамилия, инициалы (Ф.И.О.)
        public string ShortFullName
        {
            get
            {
                return
                    (lastName ?? string.Empty)
                    + (string.IsNullOrEmpty(firstName) ? string.Empty : string.Format(" {0}.", firstName[0]))
                    + (string.IsNullOrEmpty(middleName) ? string.Empty : string.Format(" {0}.", middleName[0]));
            }
        }

        /// <summary>
        /// Фамилия Имя Отчество
        /// </summary>
        public string FullName
        {
            get
            {
                return (string.IsNullOrEmpty(LastName) ? "" : LastName + " ")
                       + (string.IsNullOrEmpty(FirstName) ? "" : FirstName + " ")
                       + (string.IsNullOrEmpty(MiddleName) ? "" : MiddleName);
            }
        }

        /// <summary>
        /// Паспортные данные
        /// </summary>
        public string PassportData
        {
            get
            {
                return PassportInfo != null
                           ? string.Format(Resources.UserPassportData, PassportInfo.Series, PassportInfo.Number, PassportInfo.Issuer, PassportInfo.IssueDate.ToDateTime().ToShortDateString())
                           : string.Empty;
            }
        }

        /// <summary>
        /// Возвращает имя компании если оно задано, иначе имя в системе.
        /// </summary>
        public string CompanyOrName
        {
            get { return string.IsNullOrEmpty(Company) ? NameLocal: Company; }
        }

        /// <summary>
        /// Возвращает true, если текущий пользователь является администратором или редактируемый им не является.
        /// Только администратор может редактировать администраторов.
        /// </summary>
        public bool CanEdit
        {
            get
            {
                return ServerSession.CurrentSession.GetCurrentUser().IsAdministrator || !IsAdministrator;
            }
        }

        /// <summary>
        /// Пользователь сессии может открывать каторчку этого пользователя на редактирвоание
        /// </summary>
        /// <returns>true - открывается на редактирвоание, false - не открывается</returns>
        public bool IsEditableByCurrentUser
        {
            get
            {
                var currentSessionUser = ServerSession.CurrentSession.GetCurrentUser();
                return CanEdit && !System && currentSessionUser.HasPermission(Permission.CAN_EDIT_EMPLOYEES) ;
            }
        }

        #endregion

        # region CTOR

        public User(Guid id, LocalizableValue name, Role mainRole, string pin, Card authCard)
            : this(id, name, mainRole, pin, authCard, EdiSystem.EdiSystemUnspecified)
        {
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            User item = obj as User;
            if (item == null)
            {
                return -1;
            }

            return Comparer<string>.Default.Compare(NameLocal, item.NameLocal);
        }

        #endregion

        #region Methods

        public bool IsSameTaxId(string taxpayerIdNumber, string accountingReasonCode)
        {
            if (string.IsNullOrEmpty(this.taxpayerIdNumber) || string.IsNullOrEmpty(taxpayerIdNumber))
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.accountingReasonCode))
            {
                return string.Equals(this.taxpayerIdNumber, taxpayerIdNumber, StringComparison.OrdinalIgnoreCase);
            }
            return string.Equals(this.taxpayerIdNumber, taxpayerIdNumber, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(this.accountingReasonCode, accountingReasonCode, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsSameTaxId(User user)
        {
            Contract.Requires(user != null);

            return IsSameTaxId(user.taxpayerIdNumber, user.accountingReasonCode);
        }

        public bool HasPermissionWhereResponsible(Permission permission)
        {
            if (Denied(permission))
            {
                return false;
            }
            if (AllowedWhereResponsible(permission))
            {
                return true;
            }
            if (MainRole != null && MainRole.Denied(permission))
            {
                return false;
            }
            if (Roles.Any(role => role.Denied(permission)))
            {
                return false;
            }

            if (MainRole != null && MainRole.AllowedWhereResponsible(permission))
            {
                return true;
            }

            return Roles.Any(role => role.AllowedWhereResponsible(permission));
        }

        /// <summary>
        /// Возвращает ФИО из карточки сотрудника, если они не заполнены, то имя в системе.
        /// </summary>
        public string GetFullNameOrDefault()
        {
            return string.IsNullOrEmpty(FullName) ? NameLocal: FullName;
        }

        /// <summary>
        /// Определят, возможно ли использовать сотрудника в RMS
        /// </summary>
        public bool AllowToUseInRMS()
        {
            bool allow = false;

            if (Edition.CurrentEdition == BackVisualizationMode.IIKO_RMS)
            {
                // показываем сотрудника, если он Администратор, имеет как минимум один привязанный департамент 
                // или список департаментов не инициализирован (т.е. сотрудник создан в RMS до первой синхронизации)
                if (AssignedToDepartments == null || AssignedToDepartments.Count > 0 || Administrator)
                {
                    allow = true;
                }
            }
            else
            {
                // для Chain показываем всех сотрудникв
                allow = true;
            }
            return allow;
        }

        /// <summary>
        /// Возвращает число расположенное с правой стороны табельного номера/кода или -1 в противном случае.        
        /// </summary>
        public int RightNumericCode
        {
            get
            {
                int result;

                return int.TryParse(ParseCode(code).Value, out result) ? result : -1;
            }
        }

        /// <summary>
        /// Парсит строку табельного номера/кода и возвращает пару строка-число.
        /// пример: #TB123, результат строка-#TB и число-123
        /// </summary>
        public static KeyValuePair<string, string> ParseCode(string userCode)
        {
            KeyValuePair<string, string> keyValue;

            // получаем справа на лево все цифры, которые идут до любого символа не являющегося цифрой
            Regex re = new Regex(@"[\d]*?[^\D]*", RegexOptions.RightToLeft);
            MatchCollection mc = re.Matches(userCode);

            if (mc.Count > 0)
            {
                keyValue = new KeyValuePair<string, string>(userCode.Substring(0, userCode.Length - mc[0].Value.Length), mc[0].Value);
            }
            else
            {
                keyValue = new KeyValuePair<string, string>(userCode, string.Empty);
            }

            return keyValue;
        }

        /// <summary>
        /// Возвращает результат попытки получения представляемого поставщиком склада.
        /// Имеет смысл только для внутренних поставщиков, являющихся частью механизма
        /// распределенных внутренних перемещений.
        /// </summary>
        /// <param name="store">Склад, представляемый данным поставщиком, или <c>null</c>.</param>
        /// <returns><c>true</c> в случае нахождения склада, иначе - <c>false</c>.</returns>
        /// <remarks>
        /// Если метод вернул <c>false</c>, это еще не значит, что поставщик не представляет ни одного склада
        ///  - возможно, склад не входит в доступные на данный момент подразделения (РМС, мультиресторанность).
        /// </remarks>
        public bool TryGetRepresentedStore(out Store store)
        {
            store = null;

            if (Supplier && RepresentsStore)
            {
                store = EntityManager.INSTANCE.GetAll<Store>().FirstOrDefault(s => Equals(s.RepresentativeSupplier));
                return store != null;
            }

            return false;
        }

        public JurPerson EvaluateJurPerson()
        {
            if (jurPerson != null)
            {
                return jurPerson;
            }

            return EvaluateJurPersonFor(new HashSet<DepartmentEntity>(assignedToDepartments));
        }

        public override string ToString()
        {
            return NameLocal;
        }

        /// <summary>
        /// Возвращает итоговое значение для привилегии как комбинацию индивидуально назначенного
        /// по этой привилегии значения и значений, унаследованных от назначенных ролей (с учетом принятых приоритетов:
        /// индивидуально назначенное право приоритетнее права роли, значение "Запретить" приоритетнее значения "Разрешить"
        /// и т.д.)
        /// </summary>
        /// <param name="permission">привилегия</param>
        /// <returns>значение привилегии</returns>
        public PermissionState GetPermissionState(Permission permission)
        {
            Contract.Requires(permission != null);

            if (HasPermission(permission))
                return PermissionState.ALLOW;

            if (HasPermissionWhereResponsible(permission))
                return PermissionState.WHERE_RESPONSIBLE;

            if (Permissions.ContainsKey(permission)) 
                return Permissions[permission];

            if (MainRole != null && MainRole.Permissions.ContainsKey(permission) && MainRole.Denied(permission))
                return MainRole.Permissions[permission];

            foreach (Role role in Roles)
                if (role.Permissions.ContainsKey(permission) && role.Denied(permission))
                    return role.Permissions[permission];

            if (MainRole != null && MainRole.Permissions.ContainsKey(permission) && MainRole.Allowed(permission))
                return MainRole.Permissions[permission];

            foreach (Role role in Roles)
                if (role.Permissions.ContainsKey(permission) && role.Allowed(permission))
                    return role.Permissions[permission];

            return PermissionState.DEFAULT;
        }

        /// <summary>
        /// Возвращает все суммарные права данного юзера, т.е. окончательный набор привилегий как комбинация
        /// индивидуально назначенных прав и прав, наследуемых от назначенных ролей (с учетом принятых приоритетов:
        /// индивидуально назначенное право приоритетнее права роли, значение "Запретить" приоритетнее значения "Разрешить"
        /// и т.д.)
        /// </summary>
        /// <param name="allPermissions">множество всех возможных в системе прав</param>
        /// <returns>множество пар "привилегия - значение привилегии"; возвращаются только привилегии, для которых значение 
        /// не равно PermissionState.DEFAULT</returns>
        public IDictionary<Permission, PermissionState> GetPermissions(IEnumerable<Permission> allPermissions)
        {
            var result = new Dictionary<Permission, PermissionState>();
            if (allPermissions == null) 
                return result;

            foreach (var permission in allPermissions)
            {
                var state = GetPermissionState(permission);
                if (state != PermissionState.DEFAULT)
                    result.Add(permission, state);
            }

            return result;
        }

        /// <summary>
        /// Возвращает срок оплаты по отношению к указанной дате с учётом настроек
        /// "Отсрочка платежа" и "День платежа" данного поставщика
        /// 
        /// Расчет дублируется в сервере в resto.back.store.DocumentHelper#getPaymentDate
        /// </summary>
        /// <param name="now">Дата, по отношению к которой рассчитывается срок оплаты</param>
        /// <returns>Срок оплаты или <c>null</c>, если настройки отсрочки не заданы</returns>
        public DateTime? GetPaymentDate(DateTime now)
        {
            if (!PaymentDelay.HasValue)
            {
                return null;
            }

            // добавляем количество дней, указанное в настройке "Отсрочка платежа"
            var result = now.AddDays(PaymentDelay.GetValueOrFakeDefault());

            // если задан "День платежа", то ищем ближайший подходящий день недели
            if (PaymentWeekday != null)
            {
                var dayOfWeek = PaymentWeekday.ToDayOfWeek();
                while (result.DayOfWeek != dayOfWeek)
                {
                    result = result.AddDays(1);
                }
            }

            return result.Date;
        }

        /// <summary>
        /// Возвращает набор доступных для сотрудника должностей.
        /// Набор отсортирован следующим образом:
        /// 1. вначале основная должность;
        /// 2. затем остальные должности в алфавитном порядке в соответствии с текущей культурой.
        /// <para>
        /// Этим методом НЕ надо пользоваться для проверки, есть ли у пользователя соответствующая должность!
        /// Пользуйтесь <see cref="HasRole"/>.
        /// </para> 
        /// </summary>
        /// <seealso cref="HasRole"/>
        [NotNull]
        public SortedSet<Role> GetEmployeeRoles()
        {
            if (MainRole == null)
            {
                throw new NullReferenceException(string.Format("User \"{0}\" is not set main role", NameLocal));
            }

            return new SortedSet<Role>(MainRole.AsSequence()
                .Union(Roles)
                // RMS-41957 У всех сотрудников основная должность должна быть задана.
                // Но грид сотрудников - это не то место, где мы можем падать из-за этого.
                .Where(r => r != null)
                .OrderBy(r => !Equals(r, MainRole))
                .ThenBy(r => r.NameLocal, StringComparer.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Возвращает <c>true</c>, если у пользователя есть переданная должность.
        /// </summary>
        /// <param name="role">Должность, наличие которой проверяется у пользователя.</param>
        public bool HasRole(Role role)
        {
            return Equals(MainRole, role) ||
                   Roles.Contains(role);
        }

        /// <summary>
        /// Возвращает <see cref="PreferredDepartment"/>, либо, 
        /// если null, первый из списка <see cref="AssignedToDepartments"/> отсортированного по имени
        /// </summary>
        public DepartmentEntity GetPreferredDepartment()
        {
            var notNullAssignedToDepartments = AssignedToDepartments ??
                                               MultiDepartments.Instance.ChainOrRmsDepartmentsAll;
            return PreferredDepartment ?? notNullAssignedToDepartments.OrderBy(dep => dep.NameLocal).FirstOrDefault();
        }

        /// <summary>
        /// Возвращает <see cref="PreferredDepartment"/>, 
        /// либо первое ТП доступное пользователю из переданного списка <see cref="departments"/>,
        /// либо null
        /// </summary>
        public DepartmentEntity GetPreferredDepartmentAmongMany(IEnumerable<DepartmentEntity> departments)
        {
            var notNullAssignedToDepartments = AssignedToDepartments ??
                                               MultiDepartments.Instance.ChainOrRmsDepartmentsAll;
            var intersect = notNullAssignedToDepartments.Intersect(departments).OrderBy(dep => dep.NameLocal);
            return intersect.Contains(PreferredDepartment) ? PreferredDepartment : intersect.FirstOrDefault();
        }

        #endregion

        #region Static methods

        public static UserType GetUserType(bool isEmployee, bool isClient, bool isSupplier)
        {
            var type = UserType.NONE;
            if (isEmployee)
                type |= UserType.EMPLOYEE;
            if (isClient)
                type |= UserType.CLIENT;
            if (isSupplier)
                type |= UserType.SUPPLIER;
            return type;
        }

        /// <summary>
        /// Определяет общее юр. лицо подразделений, если такого нет то возвращает null
        /// </summary>
        public static JurPerson EvaluateJurPersonFor<TDep>(HashSet<TDep> departments)
            where TDep : DepartmentEntity
        {
            if (departments != null)
            {
                IEnumerable<JurPerson> jurPersons = departments.Select(d => d.GetJurPerson()).Where(j => j != null).Distinct().ToList();

                if (jurPersons.Count() == 1)
                {
                    return jurPersons.Single();
                }
            }

            return null;
        }

        /// <summary>
        /// Чтение логина текущего пользователя
        /// </summary>
        /// <remarks>
        /// Введено для сохранения layout, которые используют имя пользователя, как составляющую часть имени файла
        /// </remarks>
        public static string GetCurrentLogin()
        {
            if (!ServerSession.IsConnected)
            {
                return string.Empty;
            }

            string login = ServerSession.CurrentSession.GetCurrentUser().LoginName;

            Contract.Assert(login.IndexOfAny(global::System.IO.Path.GetInvalidPathChars()) < 0);

            return login;
        }

        /// <summary>
        /// Возвращает счёт, соответствующий типу пользователя
        /// </summary>
        [CanBeNull]
        public static Account GetAccountByUserType(UserType userType)
        {
            switch (userType)
            {
                case UserType.EMPLOYEE:
                    return CafeSetup.INSTANCE.ChartOfAccounts.EmployeeDeposits;
                case UserType.CLIENT:
                    return CafeSetup.INSTANCE.ChartOfAccounts.ClientDeposits;
                case UserType.SUPPLIER:
                    return CafeSetup.INSTANCE.ChartOfAccounts.SuppliersAccount;
                default:
                    return null;
            }
        }

        #endregion
    }
}