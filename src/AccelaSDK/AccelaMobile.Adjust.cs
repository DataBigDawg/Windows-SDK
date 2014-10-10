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
using System.Threading;
using System.Threading.Tasks;
#if !( WINDOWS_PHONE)
using Windows.Security.Authentication.Web;
#else
using Accela.WindowsStoreSDK.Client;
#endif
using Windows.Storage;
using System.IO;

/// <summary>
/// Contains Accela objects that allow your Windows Phone app to interact with the Accela Construct API and the Civic Platform.
/// This namespace also includes HTTP and JSON helper objects.
/// </summary>
namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// <see cref="AccelaSDK"/> is the main object of the Accela SDK for Windows Store.
    /// </summary>
    public partial class AccelaSDK
    {
        static AccelaSDK _instance;

        private String _userName;
        private String _passWord;
        private String _oauthHost;
        private String _apiHost;
        private String[] _permissions;
        private Boolean _isNativeAuthorization;
        private AccelaEnvironment _environment;
        private IDictionary<string, string> _customHttpHeaders;


        /// <summary>
        /// Gets or sets the event that will be called during user authentication.
        /// </summary>
        public event EventHandler<AccelaSessionEventArgs> SessionChanged;

        #region public property

        /// <summary>
        /// Get or set Accela SDK Environment.
        /// </summary>
        public AccelaEnvironment Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

        /// <summary>
        /// Get or set authentication server host.
        /// </summary>
        public String OAuthHost
        {
            get { return _oauthHost; }
            set { _oauthHost = value; }
        }

        /// <summary>
        /// Get or set REST API server host.
        /// </summary>
        public String ApiHost
        {
            get { return _apiHost; }
            set { _apiHost = value; }
        }

        /// <summary>
        /// Get or set a custom HTTP request header.
        /// </summary>
        public IDictionary<string, string> CustomHttpHeaders
        {
            get { return _customHttpHeaders; }
            set { _customHttpHeaders = value; }
        }
        #endregion

        #region private method

        private void OnSessionChanged(AccelaSessionEventArgs args)
        {
            if (SessionChanged != null)
                SessionChanged(this, args);
        }

        private void ClearAuthorizationInfo()
        {
            this._tokenInfo = null;
            this.Agency = null;
            this._userName = null;
            this._passWord = null;
            this._environment = AccelaEnvironment.PROD;
            this._isNativeAuthorization = false;
            AccelaSettings.RemoveTokenSetting(AppId, AppSecret);
        }


        #endregion

        #region public static method
        /// <summary>
        /// Get a default instance of the current class.
        /// </summary>
        /// <returns>An initialized AccelaSDK instance</returns>
        public static AccelaSDK DefaultInstance()
        {
            if (_instance == null)
                _instance = new AccelaSDK();

            return _instance;
        }

        #endregion

        #region public method
        /// <summary>
        /// Authorizes the user of the application with default agency definded in cloud, the login form was wrapped as a native dialog
        /// The Login Dialog will be presented for the user to use to login if does not exit the valid token.
        /// If the user has authenticated and still has a valid session the the user will be authorized without seeing the Login Dialog
        /// The schema of this app MUST be defined, and handleOpenURL MUST be invoked in the delegate of this app. refer to the method handleOpenURL of <see cref="AccelaSDK"/>.
        /// </summary>
        /// <param name="permissions">resources list to be accessed. refer to the Scope in https://developer.accela.com/Resource/ApisAbout</param>
        /// <param name="agency">default agency name for the citizen App</param>
        /// <returns></returns>
        public async Task Authorize(string[] permissions, string agency = null)
        {

            this._isNativeAuthorization = false;

            var loginUri = this.GetLoginUri(permissions, agency);
            var callbackUri = new Uri(AccelaSettings.AM_CALLBACK_URL);

            AccelaSDKLogger.logInfo("LoginRequestUri", loginUri.AbsoluteUri, AccelaLogLevel.Info);

            var webResult = await WebAuthenticationBroker.AuthenticateAsync(
                                    WebAuthenticationOptions.None,
                                    loginUri,
                                    callbackUri);

            AccelaOAuthException exception = null;

            if (webResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var oAuthResult = this.ParseOAuthCallbackUrl(new Uri(webResult.ResponseData.ToString()));

                AccelaSDKLogger.logInfo("LoginCallbackUri", oAuthResult, AccelaLogLevel.Info);

                if (string.IsNullOrEmpty(oAuthResult.error))
                {
                    this.Environment = (AccelaEnvironment)Enum.Parse(typeof(AccelaEnvironment), oAuthResult.Environment);
                    this.Agency = oAuthResult.Agency.ToUpper();
                    this._tokenInfo = await this.GetToken(code: oAuthResult.Code, isRefresh: false);
                    if (this._tokenInfo != null)
                    {
                        AccelaSettings.SaveTokenSetting(this.AppId, this.AppSecret, _tokenInfo);
                        OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LoginSucceeded));
                    }
                    else
                    {
                        exception = new AccelaOAuthException("Token is invalid.");
                    }
                }
                else
                {
                    exception = new AccelaOAuthException(oAuthResult.error);
                }
            }
            else if (webResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                exception = new AccelaOAuthException("Http request error.", (int)webResult.ResponseErrorDetail);
            }
            else
            {
                OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LoginCancelled));
            }

            if (exception != null)
            {
                OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LoginFailed, exception));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="permissions"></param>
        /// <param name="agency"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public async Task Authorize(string username,
                    string password,
                    string agency,
                    string[] permissions,
                    AccelaEnvironment environment)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(agency)) throw new ArgumentNullException("agency");

            this._userName = username;
            this._passWord = password;
            this.Agency = agency;
            this._environment = environment;
            this._permissions = permissions;
            this._isNativeAuthorization = true;

            AccelaOAuthException exception = null;

            this._tokenInfo = await this.GetToken();
            if (this._tokenInfo != null)
            {
                AccelaSettings.SaveTokenSetting(this.AppId, this.AppSecret, _tokenInfo);
                OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LoginSucceeded));
            }
            else
            {
                exception = new AccelaOAuthException("Token is invalid.");
            }

            if (exception != null)
            {
                OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LoginFailed, exception));
            }
        }

        /// <summary>
        /// Access tokens have a limited lifetime and, 
        /// in some cases, an application needs access to an API beyond the lifetime of a single access token. 
        /// When this is the case, your application can obtain a new access token using the refresh token.
        /// Access tokens have a limited lifetime and, in some cases, 
        /// an application needs access to an API beyond the lifetime of a single access token. 
        /// When this is the case, your application can obtain a new access token using the refresh token.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReAuthorize()
        {
            var localToken = AccelaSettings.ReadTokenSetting(this.AppId, this.AppSecret);
            if (localToken != null)
            {
                this._tokenInfo = await GetToken(isRefresh: true, refreshToken: localToken.refresh_token);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the session is available.
        /// </summary>
        /// <returns>True if the session is valid, False if it is invalid.</returns>
        public bool IsSessionValid()
        {
            if (this._tokenInfo == null)
            {
                this._tokenInfo = AccelaSettings.ReadTokenSetting(AppId, AppSecret);
                if (_tokenInfo == null)
                    return false;
            }
            if (_tokenInfo.expires_time < DateTime.Now)
            {
                ClearAuthorizationInfo();
                //OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.InvalidSession));
                return false;
            }
            AccelaSDKLogger.logInfo("IsSessionValid", this._tokenInfo, AccelaLogLevel.Info);
            return true;
        }

        /// <summary>
        /// Logs off the user from a session.
        /// </summary>
        public void Logout()
        {
            ClearAuthorizationInfo();

            OnSessionChanged(new AccelaSessionEventArgs(AccelaSessionStatus.LogoutSucceeded));
        }

        #endregion

        #region GetAsync

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> GetAsync(string path)
        {
            return this.GetAsync(path, null);
        }

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> GetAsync(string path, object parameters)
        {
            return this.GetAsync(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> GetAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return this.GetTaskAsync(path, parameters, cancellationToken).Then(result => (JsonObject)result);
        }

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        public Task<T> GetAsync<T>(string path)
        {
            return this.GetAsync<T>(path, null);
        }

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> GetAsync<T>(string path, object parameters)
        {
            return this.GetAsync<T>(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous GET request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> GetAsync<T>(string path, object parameters, CancellationToken cancellationToken)
        {
            return this.GetTaskAsync<T>(path, parameters, cancellationToken);
        }

        #endregion

        #region PostAsync

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> PostAsync(string path)
        {
            return this.PostAsync(path, "{}");
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> PostAsync(string path, object parameters)
        {
            return this.PostAsync(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="jsonString">JSON format string</param>
        /// <param name="files">Upload files</param>
        /// <returns>JSON object results</returns>
        [Obsolete("Use PostAsync(string, JsonObject, IList<AccelaMediaStream>) instead.", false)]
        public Task<JsonObject> PostAsync(string path, string jsonString, IList<AccelaMediaStream> files)
        {
            var param = new Dictionary<string, object>();
            param.Add("json-data", jsonString);
            foreach (var file in files)
                param.Add(file.FileName, file);

            return this.PostAsync(path, param, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="json">JSON object</param>
        /// <param name="files">Upload files</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> PostAsync(string path, JsonObject json, IList<AccelaMediaStream> files)
        {
            return PostAsync(path, json.ToString(), files);
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> PostAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return this.PostTaskAsync(path, parameters, cancellationToken).Then(result => (JsonObject)result);
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PostAsync<T>(string path) where T : new()
        {
            return PostAsync<T>(path, "{}");
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PostAsync<T>(string path, object parameters) where T : new()
        {
            return this.PostTaskAsync<T>(path, parameters, null, CancellationToken.None, null);
        }

        /// <summary>
        /// Asynchronous POST request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PostAsync<T>(string path, object parameters, CancellationToken cancellationToken) where T : new()
        {
            return this.PostTaskAsync<T>(path, parameters, null, cancellationToken, null);
        }

        #endregion

        #region PutAsync

        /// <summary>
        /// Asynchronous PUT request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PutAsync<T>(string path, object parameters) where T : new()
        {
            return this.PutAsync<T>(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous PUT request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PutAsync<T>(string path, object parameters, CancellationToken cancellationToken) where T : new()
        {
            return this.PutAsync<T>(path, parameters, null, cancellationToken);
        }

        /// <summary>
        /// Asynchronous PUT request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="userState">User State</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> PutAsync<T>(string path, object parameters, object userState, CancellationToken cancellationToken) where T : new()
        {
            return this.ApiTaskAsync(HttpMethod.Put, path, parameters, typeof(T), userState, cancellationToken).Then(result => (T)result);
        }

        /// <summary>
        /// Asynchronous PUT request
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> PutAsync(string path, object parameters)
        {
            return this.PutAsync<JsonObject>(path, parameters);
        }

        #endregion

        #region DeleteAsync

        /// <summary>
        /// Asynchronous Delete request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> DeleteAsync<T>(string path) where T : new()
        {
            return this.DeleteAsync<T>(path, null);
        }

        /// <summary>
        /// Asynchronous Delete request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> DeleteAsync<T>(string path, object parameters) where T : new()
        {
            return this.DeleteAsync<T>(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous Delete request
        /// </summary>
        /// <typeparam name="T">The type of dynamic result to return.</typeparam>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Reutrn <see cref="T"/> result.</returns>
        public Task<T> DeleteAsync<T>(string path, object parameters, CancellationToken cancellationToken) where T : new()
        {
            return this.ApiTaskAsync(HttpMethod.Delete, path, parameters, typeof(T), null, cancellationToken).Then(result => (T)result);
        }

        #endregion

        #region DownloadAttachmentAsync



        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <param name="filePath">The path of the file to get a StorageFile to represent</param>
        /// <returns>Download Attachment Task</returns>
        public async Task DownloadAttachmentAsync(string path, object urlParams, string filePath)
        {

            var file = await StorageFile.GetFileFromPathAsync(filePath);

            await this.DownloadAttachmentAsync(path, urlParams, file);
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <param name="fileFolder">The file-system path of the folder to retrieve</param>
        /// <param name="fileName">The desired name of the file to create.If there is an existing file in the current folder that already has the specified desiredName, the specified CreationCollisionOption determines how Windows responds to the conflict.</param>
        /// <returns>Download Attachment Task</returns>
        public async Task DownloadAttachmentAsync(string path, object urlParams, StorageFolder fileFolder, string fileName)
        {
            var file = await fileFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            await this.DownloadAttachmentAsync(path, urlParams, file);
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <param name="file">File as a StorageFile </param>
        /// <returns>Download Attachment Task</returns>
        public async Task DownloadAttachmentAsync(string path, object urlParams, StorageFile file)
        {
            await this.DownloadAttachmentAsync(path, urlParams, CancellationToken.None, file);
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <param name="file">File as a StorageFile</param>
        /// <returns>Download Attachment Task</returns>
        public async Task DownloadAttachmentAsync(string path, object urlParams, CancellationToken cancellationToken, StorageFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            var bytes = await this.DownloadAttachmentAsync(path, urlParams, cancellationToken);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <returns>Returns the byte stream of the download file</returns>
        public Task<byte[]> DownloadAttachmentAsync(string path)
        {
            return DownloadAttachmentAsync(path, null);
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <returns>Returns the byte stream of the download file</returns>
        public Task<byte[]> DownloadAttachmentAsync(string path, object urlParams)
        {
            return this.DownloadAttachmentAsync(path, urlParams, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronous request Download documents
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="urlParams">Request parameters</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>Returns the byte stream of the download file</returns>
        public Task<byte[]> DownloadAttachmentAsync(string path, object urlParams, CancellationToken cancellationToken)
        {
            return this.GetTaskAsync(path, urlParams, cancellationToken).Then(result => (byte[])result);
        }

        public Task<byte[]> DownloadAttachmentAsync(string path, IDictionary<string, object> urlParams, string postJson, CancellationToken cancellationToken)
        {
            IDictionary<string, object> @params = new Dictionary<string, object>();
            if (urlParams != null && urlParams.Count > 0)
            {
                foreach (var param in urlParams)
                {
                    if (!@params.ContainsKey(param.Key))
                    {
                        @params.Add(param);
                    }
                }
            }
            if (!string.IsNullOrEmpty(postJson))
            {
                @params.Add("json", postJson);
            }
            return this.PostTaskAsync(path, @params, cancellationToken).Then(result => (byte[])result);
        }

        #endregion

        #region UploadAttachmentAsync

        /// <summary>
        /// Uploads a stream file represented by an AccelaMediaStream object as an asynchronous operation.
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">AccelaMediaStream objects</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> UploadAttachmentAsync(string path, object parameters)
        {
            return this.UploadAttachmentAsync(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Uploads a stream file represented by an AccelaMediaStream object as an asynchronous operation.
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">AccelaMediaStream objects</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> UploadAttachmentAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return this.UploadAttachmentAsync(path, parameters, cancellationToken, null);
        }

        /// <summary>
        /// Uploads a stream file represented by an AccelaMediaStream object as an asynchronous operation.
        /// </summary>
        /// <param name="path">REST API path</param>
        /// <param name="parameters">AccelaMediaStream objects</param>
        /// <param name="cancellationToken">Whether or not notice of cancellation request.</param>
        /// <param name="uploadProgress">Defines a provider for progress updates.</param>
        /// <returns>JSON object results</returns>
        public Task<JsonObject> UploadAttachmentAsync(string path, object parameters, CancellationToken cancellationToken, IProgress<AccelaUploadProgressChangedEventArgs> uploadProgress)
        {
            return this.PostTaskAsync(path, parameters, null, cancellationToken, uploadProgress).Then(result => (JsonObject)result);
        }

        #endregion
    }

    /// <summary>
    /// Environment Type
    /// </summary>
    public enum AccelaEnvironment
    {
        /// <summary>
        /// PROD
        /// </summary>
        PROD,
        /// <summary>
        /// TEST
        /// </summary>
        TEST,
        /// <summary>
        /// DEV
        /// </summary>
        DEV,
        /// <summary>
        /// STAGE
        /// </summary>
        STAGE,
        /// <summary>
        /// CONFIG
        /// </summary>
        CONFIG,
        /// <summary>
        /// SUPP
        /// </summary>
        SUPP
    }

}
