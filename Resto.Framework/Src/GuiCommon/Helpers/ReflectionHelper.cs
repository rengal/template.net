using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace Resto.Framework.UI.Common
{
    /// <summary>
    /// Класс для минимизации кода простейших обращений к объектам через Reflection
    /// (получение/установка свойств, вызов методов)
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Получить значение свойства
        /// </summary>
        /// <param name="obj">объект содержащий свойство</param>
        /// <param name="propName">название свойства</param>
        /// <param name="prms">параметры вызова (например, для индексаторов)</param>
        /// <returns>значение свойства</returns>
        public static object GetProperty(object obj, string propName, params object[] prms)
        {
            return obj.GetType().InvokeMember(propName, BindingFlags.GetProperty, null, obj, prms);
        }

        public static object GetField(object obj, string fieldName)
        {
            return obj.GetType().InvokeMember(fieldName,
                BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Public, 
                null, obj, null);
        }

        /// <summary>
        /// Установить значение свойства
        /// </summary>
        /// <param name="obj">объект содрежащий свойство</param>
        /// <param name="propName">название свойства</param>
        /// <param name="value">значение свойства</param>
        public static void SetProperty(object obj, string propName, object value)
        {
            obj.GetType().InvokeMember(propName, BindingFlags.SetProperty, null, obj, new[] { value });
        }

        public static void SetField(object obj, string fieldName, object value)
        {
            obj.GetType().InvokeMember(fieldName, 
                BindingFlags.Instance | BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Public, 
                null, obj, new[] { value });
        }


        /// <summary>
        /// Вызвать метод
        /// </summary>
        /// <param name="obj">объект содержащий метод</param>
        /// <param name="methodName">название метода</param>
        /// <param name="prms">параметры метода</param>
        /// <returns>результат выполнения</returns>
        public static object CallMethod(object obj, string methodName, params object[] prms)
        {
            return obj.GetType().InvokeMember(methodName, 
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, obj, prms);
        }

        public static void DumpObjectToHtml(object o, StringBuilder sb)
        {
           DumpObjectToHtml(o, sb, true);
        }
  
        public static void DumpObjectToHtml(object o, StringBuilder sb, bool ulli)
        {
           if (ulli)
              sb.Append("<ul>");
  
           if (o is string || o is int || o is long || o is double)
           {
              if(ulli)
                 sb.Append("<li>");
  
              sb.Append(o.ToString());
  
              if(ulli)
                 sb.Append("</li>");
           }
           else
           {
              var t = o.GetType();
              foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
              {
                 sb.Append("<li><b>" + p.Name + ":</b> ");
                 object val = null;
  
                 try
                 {
                    val = p.GetValue(o, null);
                 }
                 catch {}
  
                 if (val is string || val is int || val is long || val is double)
                    sb.Append(val);
                 else
  
                 if (val != null)
                 {
                    var arr = val as Array;
                    if (arr == null)
                    {
                       var nv = val as NameValueCollection;
                       if (nv == null)
                      {
                         var ie = val as IEnumerable;
                         if (ie == null)
                            sb.Append(val.ToString());
                         else
                            foreach (var oo in ie)
                               DumpObjectToHtml(oo, sb);
                      }
                      else
                      {
                         sb.Append("<ul>");
                         foreach (var key in nv.AllKeys)
                         {
                            sb.AppendFormat("<li>{0} = ", key);
                            DumpObjectToHtml(nv[key],sb,false);
                            sb.Append("</li>");
                         }
                         sb.Append("</ul>");
                      }
                   }
                   else
                      foreach (var oo in arr)
                         DumpObjectToHtml(oo, sb);
                }
                else
                {
                   sb.Append("<i>null</i>");
                }
                sb.Append("</li>");
             }
          }
          if (ulli)
             sb.Append("</ul>");
       }
    
    }
}