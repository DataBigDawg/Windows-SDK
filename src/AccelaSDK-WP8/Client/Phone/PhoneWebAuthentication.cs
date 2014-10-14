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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
#if !WINDOWS_PHONE_APP
using Microsoft.Phone.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Accela.WindowsStoreSDK.Client.Phone;
#endif

namespace Accela.WindowsStoreSDK.Client
{
    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationOptions available in Win8.
    /// </summary>
    internal enum WebAuthenticationOptions
    {
        None,
        SilentMode
    }

    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationStatus available in Win8.
    /// </summary>
    internal enum WebAuthenticationStatus
    {
        Success = 0,

        UserCancel = 1,

        ErrorHttp = 2
    }

    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationResult available in Win8.
    /// </summary>
    internal sealed class WebAuthenticationResult
    {
        public string ResponseData { get; private set; }

        public WebAuthenticationStatus ResponseStatus { get; private set; }

        public uint ResponseErrorDetail { get; private set; }

        public WebAuthenticationResult(string data, WebAuthenticationStatus status, uint error)
        {
            ResponseData = data;
            ResponseStatus = status;
            ResponseErrorDetail = error;
        }
    }

    /// <summary>
    /// This class mimics the functionality provided by WebAuthenticationBroker available in Win8.
    /// </summary>
    internal sealed class WebAuthenticationBroker
    {
        private static string responseData = "";
        private static uint responseErrorDetail = 0;
        private static WebAuthenticationStatus responseStatus = WebAuthenticationStatus.UserCancel;
        private static AutoResetEvent authenticateFinishedEvent = new AutoResetEvent(false);

        static public bool AuthenticationInProgress { get; private set; }
        static public Uri StartUri { get; private set; }
        static public Uri EndUri { get; private set; }

        /// <summary>
        /// Mimics the WebAuthenticationBroker's AuthenticateAsync method.
        /// </summary>
        public static Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, Uri startUri, Uri endUri)
        {
            if (options != WebAuthenticationOptions.None)
            {
                throw new NotImplementedException("This method does not support authentication options other than 'None'.");
            }
#if !WINDOWS_PHONE_APP
            PhoneApplicationFrame rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
#else 
            Frame rootFrame = Window.Current.Content as Frame;
#endif
            if (rootFrame == null)
            {
                throw new InvalidOperationException();
            }

            WebAuthenticationBroker.StartUri = startUri;
            WebAuthenticationBroker.EndUri = endUri;
            WebAuthenticationBroker.AuthenticationInProgress = true;

            // Navigate to the login page.
#if !WINDOWS_PHONE_APP
            rootFrame.Navigate(new Uri("/AccelaSDK;component/Client/Phone/LoginPage.xaml", UriKind.Relative));
#else
            rootFrame.Navigate(typeof(LoginPage));
#endif
            Task<WebAuthenticationResult> task = Task<WebAuthenticationResult>.Factory.StartNew(() =>
            {
                authenticateFinishedEvent.WaitOne();
                return new WebAuthenticationResult(responseData, responseStatus, responseErrorDetail);
            });

            return task;
        }

        public static void OnAuthenticationFinished(string data, WebAuthenticationStatus status, uint error)
        {
            WebAuthenticationBroker.responseData = data;
            WebAuthenticationBroker.responseStatus = status;
            WebAuthenticationBroker.responseErrorDetail = error;

            WebAuthenticationBroker.AuthenticationInProgress = false;

            // Signal the waiting task that the authentication operation has finished.
            authenticateFinishedEvent.Set();
        }
    }
}
