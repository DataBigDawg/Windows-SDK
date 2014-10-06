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
using System.IO;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// AccelaMediaStream
    /// </summary>
    public class AccelaMediaStream : IDisposable
    {
        private Stream _value;

        /// <summary>
        /// [Required]Set Value
        /// </summary>
        /// <param name="value">file stream</param>
        /// <returns></returns>
        public AccelaMediaStream SetValue(Stream value)
        {
            this._value = value;
            return this;
        }

        /// <summary>
        /// Get Value
        /// </summary>
        /// <returns></returns>
        public Stream GetValue()
        {
            return this._value;
        }

        /// <summary>
        /// Content Type
        /// </summary>
        public String ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// [Required]File 
        /// </summary>
        public String FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Stream stream = this.GetValue();
            if (stream != null)
            {
                stream.Dispose();
            }
        }
    }
}
