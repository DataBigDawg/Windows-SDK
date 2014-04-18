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
