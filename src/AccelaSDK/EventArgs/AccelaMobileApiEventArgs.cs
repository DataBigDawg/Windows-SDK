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
