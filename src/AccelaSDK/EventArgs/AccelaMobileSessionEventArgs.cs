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
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// User session state
    /// </summary>
    public enum AccelaSessionStatus
    {
        /// <summary>
        /// Login succeeded.
        /// </summary>
        LoginSucceeded,
        /// <summary>
        /// Invalid session.
        /// </summary>
        InvalidSession,
        /// <summary>
        /// Login cancelled.
        /// </summary>
        LoginCancelled,
        /// <summary>
        /// Login failed.
        /// </summary>
        LoginFailed,
        /// <summary>
        /// Logout succeeded.
        /// </summary>
        LogoutSucceeded
    }

    /// <summary>
    /// accela session event args.
    /// </summary>
    public class AccelaSessionEventArgs : EventArgs
    {
        private AccelaSessionStatus _sessionStatus;

        private AccelaOAuthException _exception;

        /// <summary>
        /// user session status.
        /// </summary>
        public AccelaSessionStatus SessionStatus { get { return _sessionStatus; } }

        /// <summary>
        /// Authorize Exception
        /// </summary>
        public AccelaOAuthException Exception { get { return _exception; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaSessionEventArgs"/> class.
        /// </summary>
        /// <param name="sessionStatus">session status</param>
        public AccelaSessionEventArgs(AccelaSessionStatus sessionStatus)
            : this(sessionStatus, null)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaSessionEventArgs"/> class.
        /// </summary>
        /// <param name="sessionStatus">session status</param>
        /// <param name="exception">authorize Exception</param>
        public AccelaSessionEventArgs(AccelaSessionStatus sessionStatus, AccelaOAuthException exception)
        {
            this._sessionStatus = sessionStatus;

            this._exception = exception;
        }
    }
}
