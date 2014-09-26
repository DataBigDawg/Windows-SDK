using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.WindowsStoreSDK
{   /// <summary>
    /// Model class to set/get userToken and  <see cref="HttpWebRequestWrapper"/>
    /// </summary>
    class HttpWebRequestCreatedEventArgs : EventArgs
    {
        private readonly object _userToken;
        private readonly HttpWebRequestWrapper _httpWebRequestWrapper;

        public HttpWebRequestCreatedEventArgs(object userToken, HttpWebRequestWrapper httpWebRequestWrapper)
        {
            _userToken = userToken;
            _httpWebRequestWrapper = httpWebRequestWrapper;
        }

        public HttpWebRequestWrapper HttpWebRequest
        {
            get { return _httpWebRequestWrapper; }
        }

        public object UserState
        {
            get { return _userToken; }
        }
    }
}
