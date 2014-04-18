using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Represent errors that occur while calling a AccelaSDK API.
    /// </summary>

    public class AccelaApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        public AccelaApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AccelaApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorCode">Code of the error.</param>
        public AccelaApiException(string message, string errorCode)
            : this(String.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorCode ?? "Unknown", message))
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorStatus">Status of the error.</param>
        public AccelaApiException(string message, int errorStatus)
            : this(string.Format(CultureInfo.InvariantCulture, "({0}) {1}", errorStatus, message))
        {
            ErrorStatus = errorStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorStatus">Http of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        public AccelaApiException(string message, string errorCode, int errorStatus)
            : this(String.Format(CultureInfo.InvariantCulture, "({1} - #{0}) {2}", errorCode ?? "Unknown", errorStatus, message))
        {
            ErrorStatus = errorStatus;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorStatus">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        /// <param name="traceId">TraceId of the error.</param>
        public AccelaApiException(string message, string errorCode, int errorStatus, string traceId)
            : this(message, errorCode, errorStatus)
        {
            TraceId = traceId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorStatus">Type of the error.</param>
        /// <param name="errorCode">Code of the error.</param>
        /// <param name="traceId">TraceId of the error.</param>
        /// <param name="errorResult">Error Result of the error</param>
        public AccelaApiException(string message, string errorCode, int errorStatus, string traceId, AccelaApiErrorResult errorResult)
            : this(message, errorCode, errorStatus)
        {
            TraceId = traceId;
            ApiError = errorResult;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AccelaApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Gets or sets the status of the error.
        /// </summary>
        /// <value>The status of the error.</value>
        public int ErrorStatus { get; set; }

        /// <summary>
        /// Gets or sets the code of the error.
        /// </summary>
        /// <value>The code of the error.</value>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error traceid.
        /// </summary>
        /// <value>The code of the error traceid.</value>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the error result.
        /// </summary>
        public AccelaApiErrorResult ApiError { get; set; }
    }
}
