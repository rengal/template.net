using System.Linq;

namespace Resto.Data
{
    public partial class StoreOperationsColumnNames
    {
        public static StoreOperationsColumnNames GetColumnNameByFieldIdentifer(string fieldIdentifer)
        {
            return VALUES.FirstOrDefault(colName => colName.FieldIdentifer.Equals(fieldIdentifer));
        }
    }
}