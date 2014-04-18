using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// 
    /// </summary>
    public class AccelaDownloadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        private readonly long _received;
        private readonly long _sent;
        private readonly long _totalRecived;
        private readonly long _totalSend;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaUploadProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="bytesReceived">Bytes received.</param>
        /// <param name="totalBytesToReceive">Total bytes to receive.</param>
        /// <param name="bytesSent">Bytes sent.</param>
        /// <param name="totalBytesToSend">Total bytes to send.</param>
        /// <param name="progressPercentage">Progress percentage.</param>
        /// <param name="userToken">User token.</param>
        public AccelaDownloadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive, long bytesSent, long totalBytesToSend, int progressPercentage, object userToken)
            : base(progressPercentage, userToken)
        {
            _received = bytesReceived;
            _totalRecived = totalBytesToReceive;
            _sent = bytesSent;
            _totalSend = totalBytesToSend;
        }

    }
}
