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
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Encrypt or Descrypt operation for this class
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="clearString">Clear string that need to encrypt</param>
        /// <returns>return encrypt string</returns>
        public static string Encrypt(string clearString)
        {
            if (clearString == null)
                return null;
            string encryptedString = "";
            Task task = Task.Run(async () =>
            {
                string descriptor = "LOCAL=user";
                var buffer = CryptographicBuffer.ConvertStringToBinary(clearString, BinaryStringEncoding.Utf8);
                var provider = new DataProtectionProvider(descriptor);
                var encryptedBuffer = await provider.ProtectAsync(buffer);
                encryptedString = CryptographicBuffer.EncodeToBase64String(encryptedBuffer);
            });

            task.Wait();
            return encryptedString;
        }

        /// <summary>
        /// Descrypt string
        /// </summary>
        /// <param name="encryptedString">encrypted string that need to decrypt</param>
        /// <returns>return descrypt string</returns>
        public static string Decrypt(string encryptedString)
        {
            if (encryptedString == null)
                return null;
            string clearString = "";
            Task task = Task.Run(async () =>
            {
                var encryptedBuffer = CryptographicBuffer.DecodeFromBase64String(encryptedString);
                var provider = new DataProtectionProvider();
                var buffer = await provider.UnprotectAsync(encryptedBuffer);
                clearString = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffer);
            });

            task.Wait();
            return clearString;
        }
    }
}