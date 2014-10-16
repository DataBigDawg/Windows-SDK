using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP
namespace Accela.WindowsPhone.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{
    public class StorageService : IStorageService
    {
        private readonly ISerializerService serializer;

        public StorageService(ISerializerService serializer)
        {
            this.serializer = serializer;
        }
        /// <summary> 
        /// Deserialize from file to a specific type of instance.
        /// </summary> 
        public async Task<T> LoadAsync<T>(string fileName)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string serializedData = null;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    serializedData = await streamReader.ReadToEndAsync();
                }
            }

            T data = default(T);
            if (!string.IsNullOrWhiteSpace(serializedData))
            {
                data = this.serializer.Deserialize<T>(serializedData);
            }

            return data;
        }
        /// <summary> 
        /// Serialize data instance to file.
        /// </summary> 
        public async Task SaveAsync(string fileName, object data)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            var serializedData = this.serializer.Serialize(data);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                byte[] fileBytes = Encoding.UTF8.GetBytes(serializedData.ToCharArray());
                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
        }

        public Task MoveAsync(string originalFileName, string targetFileName)
        {
            return null;
        }
    }
}
