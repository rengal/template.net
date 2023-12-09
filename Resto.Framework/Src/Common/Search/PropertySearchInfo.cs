using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.ObjectPaths;

namespace Resto.Framework.Common.Search
{
    /// <summary>
    /// Пара &lt;<see cref="PropertyGetter"/> свойство, <see cref="Mode"/> критерий поиска&gt;. Создаётся при
    /// вызове <see cref="SearchAlgorithm{TObj}.AddProperty{TProp}(System.Func{TObj,TProp},PropertySearchMode,Resto.Framework.Common.ObjectPaths.ObjectPath.Path)"/>.
    /// </summary>
    internal sealed class PropertySearchInfo<TObj>
    {
        #region Fields
        private readonly Func<TObj, object> propertyGetter;
        private readonly Func<object, object> listItemPropertyGetter;
        private readonly Type listItemType;
        private readonly string searchPropertyKey;
        private readonly PropertySearchMode mode;
        private readonly string format;
        private readonly Type type;
        [CanBeNull]
        private readonly ObjectPath.Path propertyPath;
        #endregion

        #region Ctors
        private PropertySearchInfo(
            Func<TObj, object> propertyGetter, string searchPropertyKey,
            PropertySearchMode mode, string format, Type type, ObjectPath.Path propertyPath)
        {
            this.propertyGetter = propertyGetter;
            this.searchPropertyKey = searchPropertyKey;
            this.mode = mode;
            this.format = format;
            this.type = type;
            this.propertyPath = propertyPath;
        }

        private PropertySearchInfo(
            Func<TObj, object> propertyGetter, Func<object, object> listItemPropertyGetter,
            Type listItemType, string searchPropertyKey,
            PropertySearchMode mode, string format, Type type)
        {
            this.propertyGetter = propertyGetter;
            this.listItemPropertyGetter = listItemPropertyGetter;
            this.listItemType = listItemType;
            this.searchPropertyKey = searchPropertyKey;
            this.mode = mode;
            this.format = format;
            this.type = type;
        }
        #endregion

        #region Factory methods
        [NotNull]
        public static PropertySearchInfo<TObj> CreateSingleSearcherPropertyInfo<TProp>(Func<TObj, TProp> propertyGetter, string searchPropertyKey,
            PropertySearchMode mode, string format, ObjectPath.Path propertyPath)
        {
            return new PropertySearchInfo<TObj>(
                obj => propertyGetter(obj),
                searchPropertyKey,
                mode,
                format,
                !mode.Contains(PropertySearchMode.List) ? typeof(TProp) : GetIEnumerableTypeArgument(typeof(TProp)),
                propertyPath);
        }

        [NotNull]
        public static PropertySearchInfo<TObj> CreateListSearcherPropertyInfo<TItem, TValue>(Func<TObj, IList<TItem>> propertyExpr, Func<TItem, TValue> listItemPropertyExpr,
            string searchPropertyKey, PropertySearchMode mode, string format)
        {
            return new PropertySearchInfo<TObj>(
                propertyExpr,
                obj => listItemPropertyExpr((TItem)obj),
                typeof(TItem),
                searchPropertyKey,
                mode | PropertySearchMode.List,
                format,
                typeof(TValue));
        }

        [NotNull]
        public static PropertySearchInfo<TObj> CreateListSearcherPropertyInfo<TItem, TValue>(Func<TObj, IReadOnlyList<TItem>> propertyExpr, Func<TItem, TValue> listItemPropertyExpr,
            string searchPropertyKey, PropertySearchMode mode, string format)
        {
            return new PropertySearchInfo<TObj>(
                propertyExpr,
                obj => listItemPropertyExpr((TItem)obj),
                typeof(TItem),
                searchPropertyKey,
                mode | PropertySearchMode.List,
                format,
                typeof(TValue));
        }

        #endregion

        #region Properties
        public Func<TObj, object> PropertyGetter
        {
            get { return propertyGetter; }
        }

        public Func<object, object> ListItemPropertyGetter
        {
            get { return listItemPropertyGetter; }
        }

        public Type ListItemType
        {
            get { return listItemType; }
        }

        public string SearchPropertyKey
        {
            get { return searchPropertyKey; }
        }

        public PropertySearchMode Mode
        {
            get { return mode; }
        }

        public string Format
        {
            get { return format; }
        }

        public Type Type
        {
            get { return type; }
        }

        public ObjectPath.Path PropertyPath
        {
            get { return propertyPath; }
        }

        public Func<object, SearchToken, bool> SearchMethod { get; private set; }
        public Func<TObj, PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, bool> MatchMethod { get; private set; }
        public Func<PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, object, bool> MatchListPropertyGenericMethod { get; private set; }
        #endregion

        public void Freeze(Func<object, SearchToken, bool> searchMethod, 
                           Func<TObj, PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, bool> matchMethod, 
                           Func<PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, object, bool> matchListPropertyGenericMethod)
        {
            SearchMethod = searchMethod;
            MatchMethod = matchMethod;
            MatchListPropertyGenericMethod = matchListPropertyGenericMethod;
        }

        #region Helper methods
        private static Type GetIEnumerableTypeArgument(Type propertyEnumerableType)
        {
            return propertyEnumerableType
                .GetInterfaces()
                .StartWith(propertyEnumerableType)
                .First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .GetGenericArguments()
                .Single();
        }
        #endregion
    }
}