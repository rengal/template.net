using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;

namespace Resto.Data
{
    public partial class EventsJournalPreset : IComparable<EventsJournalPreset>, IComparable
    {
        public override string ToString()
        {
            return NameLocal;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((EventsJournalPreset)obj);
        }

        public int CompareTo(EventsJournalPreset other)
        {
            if (other == null)
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            if (other.System)
            {
                return -1;
            }
            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);
            if (result == 0)
            {
                result = Comparer<Guid>.Default.Compare(Id, other.Id);
            }
            return result;
        }

        /// <summary>
        /// Возвращает значение признака "сохранять выбранные типы событий"
        /// </summary>
        public bool IsEventsSavingEnabled
        {
            get { return EventSeverity != null; }
        }

        /// <summary>
        /// Список подразделений. Только для Chain. Для RMS - <c>null</c>
        /// </summary>
        [CanBeNull]
        public HashSet<DepartmentEntity> Departments
        {
            get { return CompanySetup.IsChain ? departments : null; }
            set { departments = value; }
        }
    }
}