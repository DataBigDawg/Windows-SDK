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
