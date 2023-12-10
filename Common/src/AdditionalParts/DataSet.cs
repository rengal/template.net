using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class DataSet
    {
        public string GetString(DataSetRow row, DataSetColumn column)
        {
            object obj = GetValue(row, column);
            return obj != null ? obj.ToString() : String.Empty;
        }

        public string GetDecimalSumString(DataSetRow row, DataSetColumn column1, DataSetColumn column2, string format)
        {
            var value1 = (decimal?)GetValue(row, column1);
            var value2 = (decimal?)GetValue(row, column2);

            decimal decimalValue1 = value1 != null ? value1.Value : 0;
            decimal decimalValue2 = value2 != null ? value2.Value : 0;

            return (decimalValue1 + decimalValue2).ToString(format);
        }

        public string GetDecimalMinusString(DataSetRow row, DataSetColumn column1, DataSetColumn column2, string format)
        {
            var value1 = (decimal?)GetValue(row, column1);
            var value2 = (decimal?)GetValue(row, column2);

            decimal decimalValue1 = value1 != null ? value1.Value : 0;
            decimal decimalValue2 = value2 != null ? value2.Value : 0;

            return (decimalValue1 - decimalValue2).ToString(format);
        }

        public string GetDecimalString(DataSetRow row, DataSetColumn column, string format)
        {
            var value = (decimal?)GetValue(row, column);
            return value != null ? value.Value.ToString(format) : 0.ToString(format);
        }

        public decimal? GetDecimal(DataSetRow row, DataSetColumn column)
        {
            return (decimal?)GetValue(row, column);
        }

        public int? GetInt(DataSetRow row, DataSetColumn column)
        {
            return (int?)GetValue(row, column);
        }

        public Guid? GetGuid(DataSetRow row, DataSetColumn column)
        {
            return (Guid?)GetValue(row, column);
        }

        public DateTime? GetDateTime(DataSetRow row, DataSetColumn column)
        {
            return (DateTime?)GetValue(row, column);
        }

        public object GetValue(DataSetRow row, DataSetColumn column)
        {
            if (!rowToIndex.ContainsKey(row))
            {
                throw new ArgumentException("Row not found: " + row);
            }
            if (!data.ContainsKey(column))
            {
                throw new ArgumentException("Column not found: " + column);
            }
            List<object> values = data[column];
            int rowIndex = rowToIndex[row];
            return (rowIndex < values.Count ? values[rowIndex] : null);
        }

        public List<object> GetValues(DataSetColumn column)
        {
            if (!data.ContainsKey(column))
            {
                throw new ArgumentException("Column not found: " + column);
            }
            return data[column];
        }

    }
}