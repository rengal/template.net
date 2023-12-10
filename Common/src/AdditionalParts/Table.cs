using System;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class Table : IComparable<Table>
    {
        [CanBeNull]
        public RestaurantSection Section
        {
            get
            {
                return EntityManager.INSTANCE
                    .GetAllNotDeleted<RestaurantSection>()
                    .FirstOrDefault(section => section.NotDeletedTables.Contains(this));
            }
        }

        public int CompareTo(Table other)
        {
            return ToString().CompareTo(other.ToString());
        }

        public override string ToString()
        {
            RestaurantSection section = Section;

            string result = Convert.ToString(section);
            if (result.Trim().Length > 0)
                result += " - ";

            result += Num + (NameLocal.Length > 0 ? " (" + NameLocal + ")" : "");
            return result;
        }

        public string GetSectionName()
        {
            var section = EntityManager.INSTANCE.GetAll<RestaurantSection>()
                                       .FirstOrDefault(s => s.Tables != null && s.Tables.Contains(this));

            return section != null ? section.NameLocal : string.Empty;
        }
    }
}