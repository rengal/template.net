using System;
using System.Data;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс, содержащий методы расширения IDataRecord.
    /// </summary>
    public static class DataRecordExtensions
    {
        public static DateTime? TryReadDateTime(this IDataRecord record, int columnIndex)
        {
            return !record.IsDBNull(columnIndex)
                ? record.GetDateTime(columnIndex)
                : (DateTime?)null;
        }

        public static T TryGetEntity<T>(this IDataRecord record, int columnIndex, Func<Guid, T> resolveObject)
            where T: class
        {
            return !record.IsDBNull(columnIndex)
                ? resolveObject(record.GetGuid(columnIndex))
                : null;
        }
    }
}
