using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EdiSystem : IComparable
    {
        #region Fields

        /// <summary>
        /// Вспомогательный словарь для сортировки систем EDI
        /// </summary>
        private static readonly Dictionary<EdiSystemType, int> SystemTypeOrder = new Dictionary<EdiSystemType, int>
        {
            {EdiSystemType.UNSPECIFIED_EDI_SYSTEM, 0},
            {EdiSystemType.KONTUR_EDI_SYSTEM, 1},
            {EdiSystemType.INTERNAL_EDI_SYSTEM, 2},
            {EdiSystemType.EXTERNAL_EDI_SYSTEM, 3}
        };

        #endregion

        #region Properties

        /// <summary>
        /// true - если данная система EDI является внешней
        /// </summary>
        public bool IsExternal
        {
            get { return SystemType == EdiSystemType.EXTERNAL_EDI_SYSTEM; }
        }

        /// <summary>
        /// Системы EDI по умолчанию
        /// </summary>
        public static EdiSystem EdiSystemUnspecified
        {
            get
            {
                var id = PredefinedGuids.EDI_SYSTEM_UNSPECIFIED_API_GUID.Id;
                return EntityManager.INSTANCE.Get<EdiSystem>(id);
            }
        }

        /// <summary>
        /// Системы EDI Контур
        /// </summary>
        public static EdiSystem EdiSystemKontur
        {
            get
            {
                var id = PredefinedGuids.EDI_SYSTEM_KONTUR_GUID.Id;
                return EntityManager.INSTANCE.Get<EdiSystem>(id);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Возвращает все доступные EDI-системы, кроме Контура.
        /// </summary>
        /// <remarks>
        /// https://jira.iiko.ru/browse/RMS-47618
        /// </remarks>
        public static IEnumerable<EdiSystem> GetAllAvailable(bool includeDeleted = true)
        {
            return (includeDeleted
                    ? EntityManager.INSTANCE.GetAll<EdiSystem>()
                    : EntityManager.INSTANCE.GetAllNotDeleted<EdiSystem>())
                .Where(system => !system.Id.Equals(PredefinedGuids.EDI_SYSTEM_KONTUR_GUID.Id));
        }

        /// <summary>
        /// <c>true</c>, если <see cref="EdiSystem"/> указанного
        /// поставщика является Контуром
        /// </summary>
        public static bool IsKontur([CanBeNull] User supplier)
        {
            return supplier != null && supplier.EdiSystem.Id == PredefinedGuids.EDI_SYSTEM_KONTUR_GUID.Id;
        }

        public override string ToString()
        {
            return NameLocal;
        }

        public bool Equals(EdiSystem system)
        {
            return Equals(Id, system.Id);
        }

        public override bool Equals(object obj)
        {
            var system = obj as EdiSystem;
            return Equals(system);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion

        #region IComparable implementation

        public int CompareTo(EdiSystem ediSystem)
        {
            if (ediSystem == null)
            {
                return -1;
            }

            var index1 = SystemTypeOrder[SystemType];
            var index2 = SystemTypeOrder[ediSystem.SystemType];

            var result = index1.CompareTo(index2);

            if (result == 0)
            {
                result = string.CompareOrdinal(ToString(), ediSystem.ToString());
            }

            return result;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as EdiSystem);
        }

        #endregion
    }
}
