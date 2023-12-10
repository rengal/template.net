// ReSharper disable CheckNamespace
namespace Resto.Data
// ReSharper restore CheckNamespace
{
    public partial class NotSoldItemRecord
    {
        #region Constants

        private const char Separator = '|';

        #endregion

        #region Properties

        /// <summary>
        /// Название продукта
        /// </summary>
        public string ItemName
        {
            get { return product.NameLocal; }
        }

        /// <summary>
        /// Название типа продукта
        /// </summary>
        public string ItemTypeName
        {
            get
            {
                return GetPropertyCollectionName(product.Type, product.Type.Code);
            }
        }

        /// <summary>
        /// Название группы продукта
        /// </summary>
        public string GroupName
        {
            get { return product.Parent != null ? product.Parent.NameLocal: null; }
        }
        
        /// <summary>
        /// Название категории продукта
        /// </summary>
        public string CategoryName
        {
            get { return product.Category != null ? product.Category.NameLocal: null; }
        }

        /// <summary>
        /// Иерархия продукта
        /// </summary>
        public string ItemHierarchy
        {
            get
            {
                var hierarchyPath = product.GroupsHierarchyPath;
                return string.IsNullOrEmpty(hierarchyPath) ? null : hierarchyPath;
            }
        }

        /// <summary>
        /// Название торгового предприятия
        /// </summary>
        public string DepartmentName
        {
            get
            {
                return Department == null ? null : GetPropertyCollectionName(Department, Department.Id);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Получает отображаемый текст для поля pivot'а: значение без разделителя и идентификатора
        /// </summary>
        /// <param name="value">Исходное значение отображаемого текста в формате [Name][Separator][Guid]</param>
        /// <returns>Отображаемый текст без разделителя и guid'а</returns>
        public static string GetPropertyDispalyText(string value)
        {
            return value == null ? null : value.Split(Separator)[0];
        }

        /// <summary>
        /// Формирует уникальную строку для коллекций полей
        /// </summary>
        /// <param name="value">Имя которое должно отображаться в ячейках поля</param>
        /// <param name="unicPart">Уникальная часть идентифицирующая конкретный элемент коллекции</param>
        /// <returns>Уникальная строка в формате [Name][Separator][Guid]</returns>
        public static string GetPropertyCollectionName(object value, object unicPart)
        {
            return string.Format("{0}{1}{2}", value, Separator, unicPart);
        }

        #endregion
    }
}
