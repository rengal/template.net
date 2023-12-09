using System;

namespace Resto.Front.PrintTemplates.Reports
{
    /// <summary>
    /// Класс, который можно использовать в качестве ключа в словаре, хранящем шаблоны чеков
    /// </summary>
    internal sealed class TemplateCacheKey : IEquatable<TemplateCacheKey>
    {
        /// <summary>
        /// Шаблон чека
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// Тип модели чека
        /// </summary>
        public Type ModelType { get; }

        public TemplateCacheKey(string template, Type modelType)
        {
            Template = template;
            ModelType = modelType;
        }

        public bool Equals(TemplateCacheKey obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return string.Equals(Template, obj.Template) && ModelType == obj.ModelType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj is TemplateCacheKey && Equals((TemplateCacheKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Template != null ? Template.GetHashCode() : 0)*397) ^ (ModelType != null ? ModelType.GetHashCode() : 0);
            }
        }
    }
}
