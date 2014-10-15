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
#if (SILVERLIGHT || WINDOWS_PHONE)
using System.IO.IsolatedStorage;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ComponentModel;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// accela settings
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class AccelaSettings
    {
#if !(SILVERLIGHT || WINDOWS_PHONE)
        public AccelaTokenResult Token { get; set; }

        public String AppId { get; set; }

        public String AppSecret { get; set; }

        public String Environment { get; set; }
#endif

        #region const
        /// <summary>
        /// accela sdk name
        /// </summary>
        public const String AM_SDK_NAME =
#if !(SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP)
 "Accela SDK for Windows Store";
#else
 "Accela SDK for Windows Phone";
#endif
        /// <summary>
        /// accela sdk version
        /// </summary>
        public static String AM_SDK_VERSION
        {
            get
            {
                AssemblyName assemblyName =
#if WINDOWS_PHONE
                new AssemblyName(Assembly.GetExecutingAssembly().FullName);
#else
 new AssemblyName(typeof(AccelaSettings).GetTypeInfo().Assembly.FullName);
#endif
                return assemblyName.Version.ToString();
            }
        }

        /// <summary>
        /// accela oauth host
        /// </summary>
        public const String AM_HOST_OAUTH = "auth.accela.com";

        /// <summary>
        /// accela apis host
        /// </summary>
        public const String AM_HOST_API = "apis.accela.com";

        /// <summary>
        /// accela authorize path
        /// </summary>
        public const String AM_PATH_OAUTH_AUTHORIZE = "oauth2/authorize";

        /// <summary>
        /// accela sso ticket path
        /// </summary>
        public const String AM_PATH_SSO_TICKET = "sso/ticket";

        /// <summary>
        /// accela access token path
        /// </summary>
        public const String AM_PATH_ACCESS_TOKEN = "oauth2/token";

        /// <summary>
        /// accela callback url
        /// </summary>
        public const String AM_CALLBACK_URL = "http://localhost/";

        /// <summary>
        /// request timeout
        /// The connection without attachments for each single request
        /// </summary>
        public const int REQUEST_TIMEOUT_WITHOUTATTACHMENTS = 120 * 1000;

        /// <summary>
        /// request timeout
        /// The connection with attachments for each single request
        /// </summary>
        public const int REQUEST_TIMEOUT_WITHATTACHMENTS = 300 * 1000;

#if (SILVERLIGHT || WINDOWS_PHONE)
        /// <summary>
        /// setting name
        /// </summary>
        private const String STORAGESETTING_NAME = "ACCELA_SDK";

        /// <summary>
        /// setting api id name
        /// </summary>
        private const String STORAGESETTING_APPID_NAME = "api_id";

        /// <summary>
        /// setting api secret name
        /// </summary>
        private const String STORAGESETTING_APPSECRET_NAME = "api_secret";

        private const String STORAGESETTING_APPENVIRONMENT = "environment";

        /// <summary>
        /// setting token name
        /// </summary>
        private const String STORAGESETTING_TOKEN_NAME = "TOKEN";

        /// <summary>
        /// setting access token name
        /// </summary>
        private const String STORAGESETTING_TOKEN_ACTOKEN_NAME = "access_token";
        /// <summary>
        /// setting refresh token name
        /// </summary>
        private const String STORAGESETTING_TOKEN_RFTOKEN_NAME = "refresh_token";
        /// <summary>
        /// setting expires in name
        /// </summary>
        private const String STORAGESETTING_TOKEN_EXPIRESIN_NAME = "expires_in";
#endif
        /// <summary>
        /// Setting file name
        /// </summary>
        private const String SETTING_FILE = "settings.json";

        #endregion


        /// <summary>
        /// Read token information from storage.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static AccelaTokenResult ReadTokenSetting(string appId, string appSecret, out string environment)
        {
            environment = null;
#if !(SILVERLIGHT || WINDOWS_PHONE)
            string settings = Accela.WindowsStoreSDK.FileHelper.GetTextFromFile(appId, SETTING_FILE);
            settings = Accela.WindowsStoreSDK.EncryptHelper.Decrypt(settings);

            if (!string.IsNullOrWhiteSpace(settings))
            {
                var settingsObject = SimpleJson.DeserializeObject<AccelaSettings>(settings);
                environment = settingsObject.Environment;
                if (appId == settingsObject.AppId && appSecret == settingsObject.AppSecret)
                {
                    return settingsObject.Token;
                }
            }
#else
            if (IsolatedStorageSettings.ApplicationSettings.Contains(STORAGESETTING_NAME + appId))
            {
                var settings = SimpleJson.DeserializeObject<IDictionary<string, object>>(
                    IsolatedStorageSettings.ApplicationSettings[STORAGESETTING_NAME + appId].ToString());

                string tmpId = string.Empty,
                    tmpSecret = string.Empty;

                if (settings.ContainsKey(STORAGESETTING_APPID_NAME))
                    tmpId = (string)settings[STORAGESETTING_APPID_NAME];
                if (settings.ContainsKey(STORAGESETTING_APPSECRET_NAME))
                    tmpSecret = (string)settings[STORAGESETTING_APPSECRET_NAME];
                if(settings.ContainsKey(STORAGESETTING_APPENVIRONMENT))
                    environment = (string)settings[STORAGESETTING_APPENVIRONMENT];

                if (appId == tmpId && appSecret == tmpSecret &&
                    settings.ContainsKey(STORAGESETTING_TOKEN_NAME))
                {
                    return SimpleJson.DeserializeObject<AccelaTokenResult>(settings[STORAGESETTING_TOKEN_NAME].ToString());
                }
            }
#endif

            return null;
        }

        /// <summary>
        /// save token information to storage
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="info"></param>
        public static void SaveTokenSetting(string appId, string appSecret, string environment, AccelaTokenResult info)
        {
#if !(SILVERLIGHT || WINDOWS_PHONE)
            AccelaSettings settings = new AccelaSettings();
            settings.AppId = appId;
            settings.AppSecret = appSecret;
            settings.Token = info;
            settings.Environment = environment;
            string content = SimpleJson.SerializeObject(settings);
            content = Accela.WindowsStoreSDK.EncryptHelper.Encrypt(content);
            FileHelper.SaveTextToFile(appId, SETTING_FILE, content);
#else
            var settings = new Dictionary<string, object>();
            settings.Add(STORAGESETTING_APPID_NAME, appId);
            settings.Add(STORAGESETTING_APPSECRET_NAME, appSecret);
            settings.Add(STORAGESETTING_TOKEN_NAME, info);
            settings.Add(STORAGESETTING_APPENVIRONMENT, environment);
            IsolatedStorageSettings.ApplicationSettings[STORAGESETTING_NAME + appId] = SimpleJson.SerializeObject(settings);
            IsolatedStorageSettings.ApplicationSettings.Save();
#endif
        }

        public static void RemoveTokenSetting(string appId, string appSecret)
        {
#if !(SILVERLIGHT || WINDOWS_PHONE)
            FileHelper.DeleteFile(appId, SETTING_FILE);
#else
            IsolatedStorageSettings.ApplicationSettings.Remove(STORAGESETTING_NAME + appId);
#endif
        }
    }
}
