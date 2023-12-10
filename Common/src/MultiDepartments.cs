using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Data;
using Resto.Framework.Common;

using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Data;

namespace Resto.UI.Common
{
    /// <summary>
    /// Набор методов для всевозможных проверок режима мультиресторанности в Чейне
    /// </summary>
    public sealed class MultiDepartments
    {
        #region Instance

        [NotNull]
        private static readonly MultiDepartments instance = new MultiDepartments();

        /// <summary>
        /// Singleton
        /// </summary>
        [NotNull]
        public static MultiDepartments Instance
        {
            get { return instance; }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Поле для свойства <see cref="ChainDepartments"/>
        /// </summary>
        private IList<DepartmentEntity> chainDepartments;

        /// <summary>
        /// Поле для свойства <see cref="HasDepartmentFilter"/>
        /// </summary>
        private bool hasDepartmentFilter;

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает множество торговых предприятий, по которым текущий пользователь может смотреть/изменять данные.
        /// Пересечение двух множеств:
        /// 1) торговые предприятия, где ответственным является текущий пользователь
        /// 2) торговые предприятия, доступные в текущем режиме работы (РМС / Чейн)
        /// В чейне второе второе множество всегда является подмножеством первого, поэтому в чейне вместо этого свойства
        /// можно использовать <see cref="ChainDepartments"/>; в РМС это не так, т.к. пользователь может войти в ТП, в котором
        /// он не является ответственным
        /// </summary>
        public IEnumerable<DepartmentEntity> WorkingDepartments
        {
            get { return GetWorkingDepartments(false); }
        }

        /// <summary>
        /// Возвращает множество торговых предприятий, по которым текущий пользователь может смотреть/изменять данные
        /// (см. <see cref="WorkingDepartments"/>). Если это множество совпадает с множеством всех ТП в корпорации,
        /// возвращает null.
        /// </summary>
        [CanBeNull]
        public IEnumerable<DepartmentEntity> WorkingDepartmentsNullable
        {
            get
            {
                var workingDepartments = WorkingDepartments.ToArray();
                bool areAllDepartmentsWorking = NotDeletedDepartments.Except(workingDepartments).IsEmpty();
                return areAllDepartmentsWorking ? null : workingDepartments;
            }
        }

        /// <summary>
        /// Просто обёртка <see cref="WorkingDepartmentsNullable"/>, для удобства вызова серверных методов,
        /// которые на вход принимают List, а не IEnumerable
        /// </summary>
        [CanBeNull]
        public List<DepartmentEntity> WorkingDepartmentsNullableList
        {
            get
            {
                var workingDepartments = WorkingDepartmentsNullable;
                return workingDepartments == null ? null : workingDepartments.ToList();
            }
        }

        /// <summary>
        /// Просто обёртка <see cref="WorkingDepartmentsNullable"/>, для удобства вызова серверных методов,
        /// которые на вход принимают Set, а не IEnumerable
        /// </summary>
        [CanBeNull]
        public HashSet<DepartmentEntity> WorkingDepartmentsNullableSet
        {
            get
            {
                var workingDepartments = WorkingDepartmentsNullable;
                return workingDepartments == null ? null : new HashSet<DepartmentEntity>(workingDepartments);
            }
        }

        /// <summary>
        /// Возвращает <c>false</c>, если по условиям мультиресторанности текущему пользователю
        /// доступны все ТП корпорации (т.е. пользователь ответственный во всех ТП корпорации
        /// и выбрал все их при входе в Чейн); во всех остальных случаях (пользователь ответственный
        /// не во всех ТП, либо не все выбрал) возвращает <c>true</c>
        /// </summary>
        /// <remarks>
        /// Доступно только в Чейне и только после успешного входа (после вызова <see cref="SetMultiDepartmentMode"/>;
        /// если вызвать свойство с нарушением этих условий, будет выброшено исключение
        /// </remarks>
        public bool HasDepartmentFilter
        {
            get
            {
                AssertInitialized();
                return hasDepartmentFilter;
            }
        }

        /// <summary>
        /// Возвращает текущее ТП, если приложение находится в режиме одноресторанности; иначе null
        /// </summary>
        /// <remarks>
        /// Доступно только в Чейне и только после успешного входа (после вызова <see cref="SetMultiDepartmentMode"/>;
        /// если вызвать свойство с нарушением этих условий, будет выброшено исключение
        /// </remarks>
        public DepartmentEntity ChainSingleDepartment
        {
            get
            {
                return ChainDepartments.Count() == 1 ? ChainDepartments.Single() : null;
            }
        }

        /// <summary>
        /// Возвращает доступные в текущей сессии торговые предприятия - те, которые
        /// пользователь выбрал при входе в Чейн. Всегда является подмножеством
        /// множества ТП, в которых пользователь является ответственным. Не может
        /// быть пустым.
        /// </summary>
        /// <remarks>
        /// Доступно только в Чейне и только после успешного входа (после вызова <see cref="SetMultiDepartmentMode"/>;
        /// если вызвать свойство с нарушением этих условий, будет выброшено исключение
        /// </remarks>
        [NotNull]
        public IEnumerable<DepartmentEntity> ChainDepartments
        {
            get { return GetChainDepartments(false); }
        }

        /// <summary>
        /// В Чейне эквивалентно свойству <see cref="ChainDepartments"/>;
        /// в РМС возвращает множество, содержащее единственный элемент - текущее ТП
        /// </summary>
        [NotNull]
        public IEnumerable<DepartmentEntity> ChainOrRmsDepartments
        {
            get { return GetChainOrRmsDepartments(false); }
        }

        /// <summary>
        /// В Чейне эквивалентно свойству <see cref="NotDeletedDepartments"/>;
        /// в РМС возвращает множество, содержащее единственный элемент - текущее ТП
        /// </summary>
        public IEnumerable<DepartmentEntity> ChainOrRmsDepartmentsAll
        {
            get
            {
                return CompanySetup.IsRMS
                    ? new List<DepartmentEntity>() { CompanySetup.INSTANCE.Department }
                    : NotDeletedDepartments;
            }
        }

        /// <summary>
        /// Возвращает текущее подразделение для Chain в одноресторанном режиме или в RMS, иначе возвращает null
        /// </summary>
        [CanBeNull]
        public DepartmentEntity ChainOrRmsSingleDepartment
        {
            get
            {
                return CompanySetup.IsRMS
                    ? CompanySetup.INSTANCE.Department
                    : ChainSingleDepartment;
            }
        }

        /// <summary>
        /// Возвращает список активных (неудалённых) подразделений.
        /// </summary>
        public IEnumerable<DepartmentEntity> NotDeletedDepartments
        {
            get { return EntityManager.INSTANCE.GetAllNotDeleted<DepartmentEntity>(); }
        }

        /// <summary>
        /// <c>true</c> - мы в RMS или в Chain в режиме одноресторанности, иначе <c>false</c>.
        /// Свойство является взаимоисключающим со свойством <see cref="IsMultiDepartmentMode"/>
        /// </summary>
        public bool IsRmsOrSingleDepartmentMode
        {
            get { return CompanySetup.IsRMS || ChainDepartments.Count() == 1; }
        }

        /// <summary>
        /// <c>true</c> - мы в Chain в режиме мультиресторанности, иначе <c>false</c>.
        /// Свойство является взаимоисключающим со свойством <see cref="IsRmsOrSingleDepartmentMode"/>
        /// </summary>
        public bool IsMultiDepartmentMode
        {
            // Проверка ChainDepartments.Count > 1 не подходит, т.к. возможна еще ситуация,
            // когда ChainDepartments.Count == 0 (при первом заходе в чейн, когда еще не создано
            // ни одного ТП) - такую ситуацию тоже считаем мультиресторанностью, чтобы был виден
            // пункт "Настройка корпорации"
            get { return CompanySetup.IsChain && ChainDepartments.Count() != 1; }
        }

        /// <summary>
        /// <c>true</c> - Чейн, доступны не все ТП корпорации. Фактически это "безопасный"
        /// вариант <see cref="HasDepartmentFilter"/>, который сначала проверяет, находится
        /// ли приложение в режиме Чейна, и, следовательно, не выбрасывает исключение, даже
        /// если вызывается из РМС
        /// </summary>
        public bool IsChainFilteredMode
        {
            get { return CompanySetup.IsChain && HasDepartmentFilter; }
        }

        /// <summary>
        /// <c>true</c> - Чейн, доступны все ТП корпорации ("суперпользователь"). Фактически это "безопасный"
        /// вариант <see cref="HasDepartmentFilter"/>, который сначала проверяет, находится
        /// ли приложение в режиме Чейна, и, следовательно, не выбрасывает исключение, даже
        /// если вызывается из РМС
        /// </summary>
        public bool IsChainNotFilteredMode
        {
            get { return CompanySetup.IsChain && !HasDepartmentFilter; }
        }

        /// <summary>
        /// Находимся ли мы в режиме одноресторанности в чейне
        /// </summary>
        public bool IsChainSingleDepartmentMode
        {
            get { return CompanySetup.IsChain && ChainDepartments.Count() == 1; }
        }

        /// <summary>
        /// Находимся ли мы в нереплицируемом РМС или в режиме одноресторанного Чейна
        /// </summary>
        public bool IsNotReplicatingRmsOrChainSingleDepartmentMode => !CompanySetup.IsReplicationConfigured ||
                                                                      IsChainSingleDepartmentMode;

        #endregion

        #region Private methods

        /// <summary>
        /// Проверяет, был ли осуществлён вход в Чейн (был ли вызван метод <see cref="SetMultiDepartmentMode"/>
        /// и если нет выбрасывает исключение
        /// </summary>
        private void AssertInitialized()
        {
            if (chainDepartments == null)
            {
                throw new RestoException("MultiDepartments is not intialized. Call SetMultiDepartmentMode() method.");
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверяем принадлежность торгового предприятия множеству видимых текущем пользователю торговых предприятий
        /// </summary>
        /// <param name="departmentEntity">Торговое предприятие</param>
        /// <returns><c>true</c>, если текущий пользователь "видит" проверяемое торговое предприятие; иначе <c>false</c></returns>
        public bool IsDepartmentVisible(DepartmentEntity departmentEntity)
        {
            if (IsChainNotFilteredMode)
            {
                return true;
            }

            return departmentEntity != null && ChainOrRmsDepartments.Any(department => department.Id.Equals(departmentEntity.Id));
        }

        /// <summary>
        /// Проверяем видимость склада в зависимости от множества видимых текущему пользователю торговых предприятий
        /// </summary>
        /// <param name="store">Склад</param>
        /// <returns><c>true</c>, если текущий пользователь "видит" проверяемый склад; иначе <c>false</c></returns>
        public bool IsStoreVisible(Store store)
        {
            if (IsChainNotFilteredMode)
            {
                return true;
            }

            var storeDepartment = store.GetDepartmentEntity();
            return storeDepartment != null && ChainOrRmsDepartments.Any(department => department.Id.Equals(storeDepartment.Id));
        }

        /// <summary>
        /// Проверяем видимость элемента корпорации в соответствиями с множеством доступных текущему пользователю торговых предприятий
        /// </summary>
        /// <param name="entity">Элемент корпорации</param>
        /// <returns><c>true</c>, если элемент корпорации виден текущему пользователю; иначе <c>false</c></returns>
        public bool IsCorporatedEntityVisible(CorporatedEntity entity)
        {
            if (IsChainNotFilteredMode)
            {
                return true;
            }

            foreach (var department in ChainOrRmsDepartments)
            {
                if (entity.Id.Equals(department.Id))
                {
                    return true;
                }

                // Проверяем вверх
                {
                    var parent = entity.Parent;
                    while (parent != null)
                    {
                        if (parent.Id.Equals(department.Id))
                        {
                            return true;
                        }
                        parent = parent.Parent;
                    }
                }

                // Проверяем вниз
                {
                    var parent = department.Parent;
                    while (parent != null)
                    {
                        if (parent.Id.Equals(entity.Id))
                        {
                            return true;
                        }
                        parent = parent.Parent;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Проверяем видимость сущности в соответствиями с множеством доступных текущему пользователю торговых предприятий
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns><c>true</c>, если элемент сущность видна текущему пользователю; иначе <c>false</c></returns>
        public bool IsEntityVisible(PersistedEntity entity)
        {
            var store = entity as Store;
            if (store != null)
            {
                return IsStoreVisible(store);
            }

            var departmentEntity = entity as DepartmentEntity;
            if (departmentEntity != null)
            {
                return IsDepartmentVisible(departmentEntity);
            }

            var corporatedEntity = entity as CorporatedEntity;
            if (corporatedEntity != null)
            {
                return IsCorporatedEntityVisible(corporatedEntity);
            }

            var user = entity as User;
            if (user != null)
            {
                return user.IsVisible();
            }

            return entity is ClientPriceCategory;
        }

        /// <summary>
        /// В Чейне возвращает множество доступных пользователю ТП или null,
        /// если пользователю доступны все ТП корпорации.
        /// В РМС возвращает null, если параметр returnNullForRms == true;
        /// иначе множество, содержащее единственный элемент - текущее ТП.
        /// Возвращает ICollection, а не IEnumerable, потому что этот метод
        /// используется только для серверных методов, поэтому если возвращать
        /// IEnumerable, его везде нужно будет кастовать к ICollection
        /// </summary>
        [CanBeNull]
        public ICollection<DepartmentEntity> ChainOrRmsDepartmentsNullable(bool returnNullForRms)
        {
            if (CompanySetup.IsRMS)
            {
                return returnNullForRms
                    ? null
                    : new List<DepartmentEntity>() { CompanySetup.INSTANCE.Department };
            }


            return HasDepartmentFilter
                ? ChainDepartments.ToList()
                : null;
        }

        /// <summary>
        /// Просто обёртка над <see cref="ChainOrRmsDepartmentsNullable(bool)"/>,
        /// возвращающая Set. Нужна для удобства вызова некоторых серверных методов
        /// </summary>
        public HashSet<DepartmentEntity> ChainOrRmsDepartmentsNullableSet()
        {
            var departments = ChainOrRmsDepartmentsNullable(true);
            return departments == null ? null : new HashSet<DepartmentEntity>(departments);
        }

        /// <summary>
        /// Устанавливает режим мультиресторанности в Чейне - задаёт множество ТП,
        /// достпуных пользователю в текущей сессии. Это множество не может быть пустым
        /// </summary>
        /// <param name="departments">Множество торговых предприятий.
        /// Если null - пользователю доступны все торговые предприятия корпорации</param>
        public void SetMultiDepartmentMode([CanBeNull] IEnumerable<DepartmentEntity> departments)
        {
            if (!CompanySetup.IsChain)
            {
                return;
            }

            if (chainDepartments == null)
            {
                chainDepartments = new List<DepartmentEntity>();
            }
            else
            {
                chainDepartments.Clear();
            }

            if (departments == null)
            {
                chainDepartments.AddRange(NotDeletedDepartments);

                hasDepartmentFilter = false;
            }
            else
            {
                var departmentList = NotDeletedDepartments.Where(departments.Contains).ToList();
                if (departmentList.IsEmpty() && NotDeletedDepartments.Any())
                {
                    // Список ТП может быть пустым только в одном случае - при первом входе в чейн,
                    // когда в корпорации еще нет ни одного ТП
                    throw new RestoException("Departments list can't be empty");
                }
                chainDepartments.AddRange(departmentList);

                hasDepartmentFilter = NotDeletedDepartments.Except(chainDepartments).Any();
            }
        }

        /// <summary>
        /// Возвращает <c>true</c>, если переданная сущность должна быть доступна текущему пользователю (с учетом ответственности).
        /// </summary>
        public bool IsAvailableForCurrentUser<TDepartment>(IMultiDepartmentable<TDepartment> multiDepartmentable)
            where TDepartment : DepartmentEntity
        {
            return WorkingDepartments.Intersect(multiDepartmentable.DepartmentsCollection).Any();
        }

        /// <summary>
        /// Возвращает множество торговых предприятий, по которым текущий пользователь может смотреть/изменять данные.
        /// Пересечение двух множеств:
        /// 1) торговые предприятия, где ответственным является текущий пользователь
        /// 2) торговые предприятия, доступные в текущем режиме работы (РМС / Чейн)
        /// В чейне второе второе множество всегда является подмножеством первого, поэтому в чейне вместо этого свойства
        /// можно использовать <see cref="ChainDepartments"/>; в РМС это не так, т.к. пользователь может войти в ТП, в котором
        /// он не является ответственным
        /// </summary>
        /// <param name="includeDeleted"><c>true</c>, если нужно получать удалённые ТП</param>
        /// <remarks>
        /// Случай, когда нужно получать удалённые ТП, достаточно редкий. В подавляющем большинстве
        /// случаев вместо этого метода можно использовать <see cref="WorkingDepartments"/>.
        /// </remarks>
        public IEnumerable<DepartmentEntity> GetWorkingDepartments(bool includeDeleted)
        {
            var sessionDepartments = GetChainOrRmsDepartments(includeDeleted);
            var responsibilityDepartments = ServerSession.CurrentSession.GetCurrentUser().ResponsibilityDepartments;

            if (responsibilityDepartments == null)
            {
                // если ответсвенный везде, только рестораны, которые доступны в данной сессии
                return sessionDepartments;
            }

            return sessionDepartments.Intersect(responsibilityDepartments);
        }

        /// <summary>
        /// В Чейне эквивалентно свойству <see cref="ChainDepartments"/>;
        /// в РМС возвращает множество, содержащее единственный элемент - текущее ТП
        /// </summary>
        /// <param name="includeDeleted"><c>true</c>, если нужно получать удалённые ТП</param>
        /// <remarks>
        /// Случай, когда нужно получать удалённые ТП, достаточно редкий. В подавляющем большинстве
        /// случаев вместо этого метода можно использовать <see cref="ChainOrRmsDepartments"/>.
        /// </remarks>
        [NotNull]
        public IEnumerable<DepartmentEntity> GetChainOrRmsDepartments(bool includeDeleted)
        {
            return CompanySetup.IsRMS
                    ? CompanySetup.INSTANCE.Department.AsSequence()
                    : GetChainDepartments(includeDeleted);
        }

        /// <summary>
        /// Возвращает доступные в текущей сессии торговые предприятия - те, которые
        /// пользователь выбрал при входе в Чейн. Всегда является подмножеством
        /// множества ТП, в которых пользователь является ответственным. Не может
        /// быть пустым.
        /// </summary>
        /// <param name="includeDeleted"><c>true</c>, если нужно получать удалённые ТП</param>
        /// <remarks>
        /// Доступно только в Чейне и только после успешного входа (после вызова <see cref="SetMultiDepartmentMode"/>;
        /// если вызвать свойство с нарушением этих условий, будет выброшено исключение.
        /// Случай, когда нужно получать удалённые ТП, достаточно редкий. В подавляющем большинстве
        /// случаев вместо этого метода можно использовать <see cref="ChainDepartments"/>.
        /// </remarks>
        [NotNull]
        public IEnumerable<DepartmentEntity> GetChainDepartments(bool includeDeleted)
        {
            Contract.Assert(CompanySetup.IsChain);
            AssertInitialized();

            var allDepartments = includeDeleted
                ? EntityManager.INSTANCE.GetAll<DepartmentEntity>()
                : EntityManager.INSTANCE.GetAllNotDeleted<DepartmentEntity>();
            return HasDepartmentFilter ? chainDepartments : allDepartments;
        }

        /// <summary>
        /// Преобразует переданное множество ТП таким образом, чтобы в нём не осталось подразделений,
        /// которые недоступны в данный момент по условиям мультиресторанности.
        /// </summary>
        /// <param name="departments">
        /// Фильтр по ТП (<c>null</c> - все ТП)
        /// </param>
        /// <returns>
        /// Новый фильтр по ТП; <c>null</c> - все ТП
        /// </returns>
        /// <remarks>
        /// Удобно использовать перед передачей фильтра по ТП в серверные методы, либо при получении
        /// этого фильтра из ненадёжного источника (если фильтр по ТП получаем из какого-то контрола,
        /// то контрол обычно сам гарантирует, что в нём не будет "неподходящих" ТП; в остальных случаях
        /// лучше перепроверить).
        /// </remarks>
        [CanBeNull]
        public IEnumerable<DepartmentEntity> ValidateDepartmentFilter([CanBeNull] IEnumerable<DepartmentEntity> departments)
        {
            // В РМС фильтр по ТП не нужен.
            if (CompanySetup.IsRMS)
            {
                return null;
            }

            // В одноресторанном чейне в качестве фильтра используется единственное ТП, под которым зашли.
            if (IsChainSingleDepartmentMode)
            {
                return ChainSingleDepartment.AsSequence();
            }

            // В многоресторанном чейне значение null может быть установлено только если зашли под всеми ТП
            // и на вход данного метода тоже получили null. В остальных случаях в качестве фильтра устанавливается
            // пересечение множества доступных ТП и входного множества.
            if (departments == null)
            {
                return HasDepartmentFilter ? ChainDepartments : null;
            }

            return departments.Intersect(ChainDepartments);
        }

        /// <summary>
        /// Преобразует переданное множество складов таким образом, чтобы в нём не осталось складов,
        /// которые недоступны в данный момент по условиям мультиресторанности.
        /// </summary>
        /// <param name="stores">
        /// Фильтр по складам (<c>null</c> - все склады)
        /// </param>
        /// <returns>
        /// Новый фильтр по складам; <c>null</c> - все склады
        /// </returns>
        /// <remarks>
        /// Удобно использовать перед передачей фильтра по складам в серверные методы, либо при получении
        /// этого фильтра из ненадёжного источника (если фильтр по складам получаем из какого-то контрола,
        /// то контрол обычно сам гарантирует, что в нём не будет "неподходящих" складов; в остальных случаях
        /// лучше перепроверить).
        /// </remarks>
        [CanBeNull]
        public IEnumerable<Store> ValidateStoreFilter([CanBeNull] IEnumerable<Store> stores)
        {
            // В РМС возвращаем склады как есть
            if (CompanySetup.IsRMS)
            {
                return stores;
            }

            // Все склады, доступные в чейне по условиям мультиресторанности
            var chainStores = ChainDepartments.SelectMany(department => department.GetDepartmentStoresByHierarchy());

            // В многоресторанном чейне значение null может быть установлено только если зашли под всеми ТП
            // и на вход данного метода тоже получили null. В остальных случаях в качестве фильтра устанавливается
            // пересечение множества доступных ТП и входного множества.
            if (stores == null)
            {
                return HasDepartmentFilter ? chainStores : null;
            }

            return stores.Intersect(chainStores);
        }

        #endregion
    }
}
