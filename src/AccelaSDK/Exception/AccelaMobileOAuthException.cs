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
using System.Runtime.Serialization;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Represent errors that occur while calling a AccelaSDK API.
    /// </summary>
    public class AccelaOAuthException : AccelaApiException
    {
        /// <summary>
        /// 
        /// </summary>
        public AccelaOAuthErrorResult OAuthError { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        public AccelaOAuthException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AccelaOAuthException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">The error code.</param>
        public AccelaOAuthException(string message, string errorCode)
            : base(message, errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="httpErrorStatus">The http error code.</param>
        public AccelaOAuthException(string message, int httpErrorStatus)
            : base(message, httpErrorStatus)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">Code of the error.</param>
        /// <param name="httpErrorStatus">Http status of the error.</param>
        public AccelaOAuthException(string message, string errorCode, int httpErrorStatus)
            : base(message, errorCode, httpErrorStatus)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">Type of the error.</param>
        /// <param name="httpErrorStatus">Code of the error.</param>
        /// <param name="traceId">Traceid of the error.</param>
        public AccelaOAuthException(string message, string errorCode, int httpErrorStatus, string traceId)
            : base(message, errorCode, httpErrorStatus, traceId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">Type of the error.</param>
        /// <param name="httpErrorStatus">Code of the error.</param>
        /// <param name="traceId">Traceid of the error.</param>
        /// <param name="errorResult">Error Result of the error</param>
        public AccelaOAuthException(string message, string errorCode, int httpErrorStatus, string traceId, AccelaOAuthErrorResult errorResult)
            : base(message, errorCode, httpErrorStatus, traceId)
        {
            OAuthError = errorResult;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaOAuthException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AccelaOAuthException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
