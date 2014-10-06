/** 
  * Copyright 2014 Accela, Inc. 
  * 
  * You are hereby granted a non-exclusive, worldwide, royalty-free license to 
  * use, copy, modify, and distribute this software in source code or binary 
  * form for use in connection with the web services and APIs provided by 
  * Accela. 
  * 
  * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
  * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
  * DEALINGS IN THE SOFTWARE. 
  * 
  */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// This class will process file related operation.
    /// </summary>
    public class FileHelper
    {
        private static Dictionary<string, SemaphoreSlim> lockArray = new Dictionary<string, SemaphoreSlim>();
        private static SemaphoreSlim getLock(string folderName, string fileName)
        {
            lock (lockArray)
            {
                string fullName = folderName + "/" + fileName;
                if (!lockArray.ContainsKey(fullName))
                {
                    SemaphoreSlim slim = new SemaphoreSlim(1);
                    lockArray.Add(fullName, slim);
                }

                return lockArray[fullName];
            }
        }

        /// <summary>
        /// Get text from specify folder and file based on storage folder.
        /// It is thread safely, Only one thread will be invoke the method based on same folder and file name.
        /// </summary>
        /// <param name="folderName">folder name</param>
        /// <param name="fileName">file name</param>
        /// <returns>Return null if the folder or file didn't exist. Otherwise, the file content will be return.</returns>
        public static string GetTextFromFile(string folderName, string fileName)
        {
            string result = null;

            Task task=Task.Run(async () =>
            {
                var lockObject = getLock(folderName, fileName);
                await lockObject.WaitAsync();
                try
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder folder = null;
                    try
                    {
                        folder = await localFolder.GetFolderAsync(folderName);
                    }
                    catch { }
                    if(folder==null)
                    {
                        folder = await localFolder.CreateFolderAsync(folderName);
                    }

                    if (folder != null)
                    {
                        try
                        {
                            StorageFile file = await folder.GetFileAsync(fileName);
                            result = await FileIO.ReadTextAsync(file);
                        }
                        catch { }
                    }
                }
                finally
                {
                    lockObject.Release();
                }
            });

            task.Wait();
            return result;
        }

        /// <summary>
        /// Save text to specify file.
        /// It is thread safely, Only one thread will be invoke the method based on same folder and file name.
        /// </summary>
        /// <param name="folderName">folder name</param>
        /// <param name="fileName">file name</param>
        /// <param name="content">the content will be save the file</param>
        public static void SaveTextToFile(string folderName, string fileName, string content)
        {
            Task task = Task.Run(async () =>
            {
                var lockObject = getLock(folderName, fileName);
                await lockObject.WaitAsync();
                try
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder folder = null;
                    try
                    {
                        folder = await localFolder.GetFolderAsync(folderName);
                    }
                    catch
                    {

                    }

                    //Task.Run(new Func<Task<StorageFile>>())

                    if (folder == null)
                    {
                        folder = await localFolder.CreateFolderAsync(folderName);
                    }

                    StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, content);
                }
                finally
                {
                    lockObject.Release();
                }
            });

            task.Wait();
        }

        /// <summary>
        /// Remove the file if the file exist.
        /// </summary>
        /// <param name="folderName">folder name</param>
        /// <param name="fileName">file name</param>
        /// <returns></returns>
        public static void DeleteFile(string folderName, string fileName)
        {
            Task task = Task.Run(async () =>
            {
                var lockObject = getLock(folderName, fileName);
                await lockObject.WaitAsync();
                try
                {
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFolder folder = null;
                    try
                    {
                        folder = await localFolder.GetFolderAsync(folderName);
                    }
                    catch { }

                    if (folder != null)
                    {
                        folder = await localFolder.CreateFolderAsync(folderName);
                        try
                        {
                            StorageFile file = await folder.GetFileAsync(fileName);
                            if (file != null)
                            {
                                await file.DeleteAsync();
                            }
                        }
                        catch { }
                    }
                }
                finally
                {
                    lockObject.Release();
                }
            });

            task.Wait();
        }
    }
}
