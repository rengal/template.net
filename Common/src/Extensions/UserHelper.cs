using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.UI.Common;

namespace Resto.Data
{
    /// <summary>
    /// Extension-методы для класса <see cref="User"/>
    /// </summary>
    public static class UserHelper
    {
        /// <summary>
        /// Видимость указанного пользователя для склада.
        /// </summary>
        public static bool IsVisibleForStore([CanBeNull] this User user, [CanBeNull] Store store)
        {
            if (user == null)
            {
                return false;
            }

            return store == null || user.IsVisibleForDepartment(store.DepartmentEntity);
        }

        /// <summary>
        /// Видимость указанного пользователя для подразделения.
        /// </summary>
        public static bool IsVisibleForDepartment([CanBeNull] this User user, [CanBeNull] DepartmentEntity department)
        {
            if (user == null)
            {
                return false;
            }

            return department == null ||
                   user.AssignedToDepartments == null ||
                   user.AssignedToDepartments.Contains(department);
        }

        /// <summary>
        /// Проверяем видимость пользователя в зависимости от множества видимых текущему пользователю торговых предприятий,
        /// и множества торговых предприятий, к которым принадлежит проверяемый пользователь.
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns><c>true</c>, если текущий пользователь "видит" проверяемого пользователя; иначе <c>false</c></returns>
        public static bool IsVisible([CanBeNull] this User user)
        {
            if (user == null)
            {
                return false;
            }

            // Если проверяемый пользователь принадлежит ко всем торговым предприятиям или является клиентом
            // и при этом не является сотрудником или поставщиком - показываем.
            if (user.AssignedToDepartments == null || !(user.Employee || user.Supplier) && user.Client)
            {
                return true;
            }

            // Текущий пользователь ответственный во всех ТП и при входе в чейн выбрал все ТП. 
            // Значит, он видит все торговые предприятия, а значит и всех юзеров.
            // В том числе и тех юзеров, которые не принадлежат ни к одному ТП
            // (и это единственный случай, когда такие юзеры видны).
            // Сейчас таких пользователей, не относящихся ни к одному ТП, через UI бека создать
            // нельзя, но они могут быть в базе из более ранних версий.
            if (MultiDepartments.Instance.IsChainNotFilteredMode)
            {
                return true;
            }

            // Показываем пользователя, если есть ненулевое пересечение двух множеств
            // 1) Торговые предприятия, доступные текущему пользователю
            // 2) Торговые предприятия, к которым принадлежит проверяемый пользователь
            return MultiDepartments.Instance.ChainOrRmsDepartments.Intersect(user.AssignedToDepartments).Any();
        }
    }
}