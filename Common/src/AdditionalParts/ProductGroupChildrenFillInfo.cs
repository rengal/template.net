using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Сведения о заполняемых данных для <see cref="Resto.Data.ProductsService.FillProductGroupChildrenRecursive(ProductGroup, HashSet{ProductGroupChildrenFillInfo}, bool)"/>.
    /// Должно быть задано либо имя поля <see cref="ProductGroupChildrenFillInfo.FieldName"/>, чье значение задается,
    /// либо имена геттера и сеттера <see cref="ProductGroupChildrenFillInfo.GetterName"/> 
    /// и <see cref="ProductGroupChildrenFillInfo.SetterName"/>.
    /// </summary>
    partial class ProductGroupChildrenFillInfo
    {
        /// <summary>
        /// Рекомендуемый конструктор в случае если значение на стороне сервера должно заполняться по имени поля.
        /// </summary>
        /// <param name="fieldName">Имя поля.</param>
        public ProductGroupChildrenFillInfo(string fieldName)
            : this(fieldName, null, null)
        {

        }

        /// <summary>
        /// Рекомендуемый конструктор в случае если значение на стороне сервера должно заполняться через сеттер
        /// (геттер используется для сравнения старого значения поля с новым).
        /// </summary>
        /// <param name="getterName">Имя метода-геттера.</param>
        /// <param name="setterName">Имя метода-сеттера.</param>
        public ProductGroupChildrenFillInfo(string getterName, string setterName)
            : this(null, getterName, setterName)
        {
            
        }

        /// <summary>
        /// Генерирует коллекцию объектов <see cref="ProductGroupChildrenFillInfo"/> для заполнения значений по именам полей.
        /// </summary>
        /// <param name="fieldNames">Имена полей.</param>
        public static IEnumerable<ProductGroupChildrenFillInfo> CreateFieldFillInfos(params ProductTreeEntityFieldName[] fieldNames)
        {
            return fieldNames.Select(fieldName => new ProductGroupChildrenFillInfo(fieldName.FieldName));
        }

        /// <summary>
        /// Генерирует коллекцию объектов <see cref="ProductGroupChildrenFillInfo"/> для заполнения значений по именам геттеров и сеттеров.
        /// </summary>
        /// <param name="fieldNames">Имена полей (имена геттеров и сеттеров будут сгенерированы автоматически).</param>
        public static IEnumerable<ProductGroupChildrenFillInfo> CreateGetterAndSetterFillInfos(params ProductTreeEntityFieldName[] fieldNames)
        {
            return fieldNames.Select(fieldName =>
                {
                    string capitalizedFieldName = fieldName.FieldName.Capitalize();
                    return new ProductGroupChildrenFillInfo("get" + capitalizedFieldName, "set" + capitalizedFieldName);
                });
        }
    }
}
