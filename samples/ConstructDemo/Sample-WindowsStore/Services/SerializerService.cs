using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP
namespace Accela.WindowsPhone.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{
    public class SerializerService : ISerializerService
    {
        /// <summary> 
        /// Deserialize from json to a specific type of instance
        /// </summary> 
        /// <typeparam name="type">The type of instace deserialize from json.</typeparam> 
        /// <param name="json">The json string.</param> 
        /// <returns>The deserialized object.</returns>
        
        public object Deserialize(Type type, string json)
        {
            var decoded = System.Net.WebUtility.UrlDecode(json);
            var bytes = Encoding.Unicode.GetBytes(decoded);
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var serializer = new DataContractJsonSerializer(type);
                return serializer.ReadObject(stream);
            }
        }

        public T Deserialize<T>(string json)
        {
            return (T)Deserialize(typeof(T), json);
        }

        /// <summary> 
        /// Serialize from an instance to Json string
        /// </summary> 
        /// <typeparam name="instance">The instace to be serialized to json.</typeparam> 
        /// <returns>The serialized json string.</returns>
        public string Serialize(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            using (MemoryStream stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(stream, instance);
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream))
                {
                    var serialized = reader.ReadToEnd();
                    var urlEncoded = System.Net.WebUtility.UrlEncode(serialized);
                    return urlEncoded;
                }
            }
        }
    }
}
