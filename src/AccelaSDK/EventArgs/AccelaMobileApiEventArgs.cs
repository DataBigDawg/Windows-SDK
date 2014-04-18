using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Represents the AccelaSDK api event args.
    /// </summary>
    public class AccelaApiEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// The result.
        /// </summary>
        private readonly object _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaApiEventArgs"/> class.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="cancelled">
        /// The cancelled.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        /// <param name="result">
        /// The result.
        /// </param>
        public AccelaApiEventArgs(Exception error, bool cancelled, object userState, object result)
            : base(error, cancelled, userState)
        {
            _result = result;
        }

        /// <summary>
        /// Get the json result.
        /// </summary>
        /// <returns>
        /// The json result.
        /// </returns>
        public object GetResultData()
        {
            RaiseExceptionIfNecessary();
            return _result;
        }

        /// <summary>
        /// Get the json result.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <returns>The json result.</returns>
        public TResult GetResultData<TResult>()
        {
            RaiseExceptionIfNecessary();
            return (TResult)_result;
        }
    }
}
