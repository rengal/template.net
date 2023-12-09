using System;

namespace Resto.Framework.Common
{
    [Serializable]
    public class NamedValue : ICloneable
    {
        private readonly string name = string.Empty;
        private readonly object val = null;

        public NamedValue()
        {
        }

        /// <summary>
        /// Создание нового объекта
        /// </summary>
        /// <param name="val">В классе обязательно должен быть переопределен ToString()</param>
        public NamedValue(object val)
        {
            this.val = val;
        }

        /// <summary>
        /// Создание нового объекта
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val">В классе обязательно должен быть переопределен ToString()</param>
        public NamedValue(string name, object val)
        {
            this.name = name;
            this.val = val;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Value
        {
            get
            {
                return val;
            }
        }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!(obj is NamedValue))
            {
                return false;
            }
            else if ((obj as NamedValue).Value == null && Value == null)
            {
                return true;
            }
            else if ((obj as NamedValue).Value == null || Value == null)
            {
                return false;
            }
            else
            {
                return Value.ToString() == (obj as NamedValue).Value.ToString();
            }
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #region ICloneable Members

        public object Clone()
        {
            return new NamedValue(name, val);
        }

        #endregion

        public static bool operator ==(NamedValue obj1, NamedValue obj2)
        {
            if ((object)obj1 == null && (object)obj2 == null)
            {
                return true;
            }
            else if ((object)obj1 == null || (object)obj2 == null)
            {
                return false;
            }
            else if (obj1.Value == null && obj2.Value == null)
            {
                return true;
            }
            else if (obj1.Value == null || obj2.Value == null)
            {
                return false;
            }
            else
            {
                return obj1.Value.ToString() == obj2.Value.ToString();
            }
        }

        public static bool operator !=(NamedValue obj1, NamedValue obj2)
        {
            if ((object)obj1 == null && (object)obj2 == null)
            {
                return false;
            }
            else if ((object)obj1 == null || (object)obj2 == null)
            {
                return true;
            }
            if (obj1.Value == null && obj2.Value == null)
            {
                return false;
            }
            else if (obj1.Value == null || obj2.Value == null)
            {
                return true;
            }
            else
            {
                return obj1.Value.ToString() != obj2.Value.ToString();
            }
        }
    }
}
