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
