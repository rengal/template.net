using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using log4net;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.Services
{
    public static class JsonHelper
    {
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(JsonHelper));

        [NotNull]
        public static string ToJson<T>([NotNull] T request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            using (var ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, request);
                var json = ms.ToArray();
                return Encoding.UTF8.GetString(json);
            }
        }

        [CanBeNull]
        public static T FromJson<T>([NotNull] string json) where T : class, new()
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));
            try
            {
                var obj = new T();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(obj.GetType());
                    obj = (T)serializer.ReadObject(ms);
                }
                return obj;
            }
            catch (SerializationException e)
            {
                Log.Warn($"Can't deserialize entity of type: {typeof(T)}. Received json: {json}", e);
                return null;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}
