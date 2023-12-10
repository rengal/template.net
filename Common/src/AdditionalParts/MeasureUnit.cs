﻿using System;
﻿using System.Globalization;
﻿using System.Linq;
using Resto.OfficeCommon;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class MeasureUnit : IComparable
    {
        //Странный метод
        //Почему-то если требуемой единицы измерения нет в списке
        //то возвращаем то число, которое передавали
        //Логичне было бы генерировать ошибку
        public decimal CountInProductMainUnit(decimal count, Product product)
        {
            if ((this == product.MainUnit) || (!product.AdditionalUnits.ContainsKey(this)))
                return count;
            return (count * Convert.ToDecimal(product.AdditionalUnits[this]));
        }

        /// <summary>
        /// Возвращает единицу измерения по имени (короткому названию).
        /// </summary>
        /// <param name="measureUnitName">Название (короткое, например, "кг") единицы измерения.</param>
        /// <param name="searchSimilar"><code>true</code> для дополнительного поиска единиц измерения с похожим названием, иначе - <code>false</code></param>
        /// <returns>первая найденная единица измерения или <code>null</code>, если ничего не найдено.</returns>
        /// <remarks>
        /// Поиск ведется только по активным (неудаленным) единицам измерения без учета регистра.
        /// Сначала ищется объект с точным совпадением имени. В случае неудачи, если параметр <paramref name="searchSimilar"/>
        /// равен <code>true</code>, ведется дальнейший поиск по похожим названиям (с расстоянием Левинштейна &lt;= 1).
        /// </remarks>
        public static MeasureUnit GetExistingMeasureUnitByName(string measureUnitName, bool searchSimilar)
        {
            MeasureUnit exactMeasureUnit = EntityManager.INSTANCE.GetAllNotDeleted<MeasureUnit>().
                                                         FirstOrDefault(mu => mu.NameLocal.Equals(measureUnitName, StringComparison.CurrentCultureIgnoreCase));
            if (exactMeasureUnit != null)
            {
                return exactMeasureUnit;
            }

            if (searchSimilar)
            {
                MeasureUnit similarMeasureUnit = EntityManager.INSTANCE.GetAllNotDeleted<MeasureUnit>().
                                                               FirstOrDefault(mu => LevinsteinDistanceUtil.LevenshteinDistance(mu.NameLocal, measureUnitName) <= 1);
                if (similarMeasureUnit != null)
                    return similarMeasureUnit;
            }
            return null;
        }

        [CanBeNull]
        public static MeasureUnit GetMainMeasureUnit()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<MeasureUnit>().
                                 FirstOrDefault(mu => mu.MainUnit);
        }

        [CanBeNull]
        public static MeasureUnit GetSystemMesureUnit(SystemMeasureUnit type)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<MeasureUnit>()
                                .FirstOrDefault(item => item.IsSystemMeasuringUnit(type));
        }

        public bool IsSystemMeasuringUnit(SystemMeasureUnit type)
        {
            return string.Compare(SystemName, type.ToString(), true, CultureInfo.InvariantCulture) == 0;
        }

        public static MeasureUnit DefaultKgUnit
        {
            get { return GetSystemMesureUnit(SystemMeasureUnit.KG); }
        }

        public static MeasureUnit DefaultPortionUnit
        {
            get { return GetSystemMesureUnit(SystemMeasureUnit.PORTION); }
        }

        public static MeasureUnit DefaultPieceUnit
        {
            get { return GetSystemMesureUnit(SystemMeasureUnit.PIECES); }
        }

        public static MeasureUnit DefaultLitreUnit
        {
            get { return GetSystemMesureUnit(SystemMeasureUnit.LT); }
        }

        public override string ToString()
        {
            return NameLocal;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var measureUnit = obj as MeasureUnit;
            if (null == measureUnit)
            {
                return base.Equals(obj);
            }

            return NameLocal.Equals(measureUnit.NameLocal);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj != null)
                return NameLocal.CompareTo((obj as MeasureUnit).NameLocal);
            else return 1;
        }

        #endregion
    }
}