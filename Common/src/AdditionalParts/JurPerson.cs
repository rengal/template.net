using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class JurPerson : IComparable
    {
        protected override CorporatedEntityType GetCEType()
        {
            return CorporatedEntityType.JURPERSON;
        }

        public string Company
        {
            get { return NameLocal; }
        }

        /// <summary>
        /// Получает и устанавливает имя руководителя.
        /// </summary>
        /// <value>Имя руководителя.</value>
        [NotNull]
        public string LeaderName
        {
            get { return GetOfficialEmployee(OfficialEmployeeRole.LEADER).Name; }
            set { GetOfficialEmployee(OfficialEmployeeRole.LEADER).Name = value; }
        }

        /// <summary>
        /// Получает и устанавливает имя бухгалтера.
        /// </summary>
        /// <value>Имя бухгалтера.</value>
        [NotNull]
        public string AccountantName
        {
            get { return GetOfficialEmployee(OfficialEmployeeRole.ACCOUNTANT).Name; }
            set { GetOfficialEmployee(OfficialEmployeeRole.ACCOUNTANT).Name = value; }
        }

        /// <summary>
        /// Получает и устанавливает имя технолога.
        /// </summary>
        /// <value>Имя технолога.</value>
        [NotNull]
        public string TechnologistName
        {
            get { return GetOfficialEmployee(OfficialEmployeeRole.TECHNOLOGIST).Name; }
            set { GetOfficialEmployee(OfficialEmployeeRole.TECHNOLOGIST).Name = value; }
        }

        /// <summary>
        /// Получает и устанавливает имя заведующего производством.
        /// </summary>
        /// <value>Заведующий производством.</value>
        [NotNull]
        public string WorksManagerName
        {
            get { return GetOfficialEmployee(OfficialEmployeeRole.WORKS_MANAGER).Name; }
            set { GetOfficialEmployee(OfficialEmployeeRole.WORKS_MANAGER).Name = value; }
        }

        /// <summary>
        /// Получить руководителя предприятия.
        /// </summary>
        /// <param name="role">Роль.</param>
        /// <returns>Руководитель предприятия с данной ролью.</returns>
        [NotNull]
        public OfficialEmployee GetOfficialEmployee(OfficialEmployeeRole role)
        {
            if (!OfficialEmployees.ContainsKey(role))
                OfficialEmployees.Add(role, new OfficialEmployee());
            return OfficialEmployees[role];
        }

        /// <summary>
        /// Получить GLN юр. лица. Если непосредственно у юр. лица GLN не задан, возвращаем GLN корпорации
        /// </summary>
        /// <returns>GLN</returns>
        public string GetGln(bool own = false)
        {
            return !Gln.IsNullOrEmpty() || own ? Gln : Parent.Gln;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            JurPerson item = obj as JurPerson;
            if (item == null)
            {
                return -1;
            }

            return Comparer<string>.Default.Compare(NameLocal, item.NameLocal);
        }

        #endregion
    }
}