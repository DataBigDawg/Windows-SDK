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

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// accela settings
    /// </summary>
    internal class AccelaSettings
    {
        #region const
        /// <summary>
        /// accela sdk name
        /// </summary>
        public const String AM_SDK_NAME =
#if !(SILVERLIGHT || WINDOWS_PHONE)
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

        /// <summary>
        /// setting name
        /// </summary>
        private const String STORAGESETTING_NAME = "ACCELA_SDK";

        /// <summary>
        /// setting api id name
        /// </summary>
        private const String STORAGESETTING_APIID_NAME = "api_id";

        /// <summary>
        /// setting api secret name
        /// </summary>
        private const String STORAGESETTING_APISECRET_NAME = "api_secret";

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
        public static AccelaTokenResult ReadTokenSetting(string appId, string appSecret)
        {
#if !(SILVERLIGHT || WINDOWS_PHONE)
            string settings = Accela.WindowsStoreSDK.Helper.FileHelper.GetTextFromFile(appId, SETTING_FILE);

            if (!string.IsNullOrWhiteSpace(settings))
            {
                var settingsObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(settings);
                var tmpId = settingsObject[STORAGESETTING_APIID_NAME];
                var tmpSecret = settingsObject[STORAGESETTING_APISECRET_NAME];
                if (appId == tmpId && appSecret == tmpSecret)
                {
                    return SimpleJson.DeserializeObject<AccelaTokenResult>(settingsObject[STORAGESETTING_TOKEN_NAME]);
                }
            }
#else
            if (IsolatedStorageSettings.ApplicationSettings.Contains(STORAGESETTING_NAME + appId))
            {
                var settings = SimpleJson.DeserializeObject<IDictionary<string, object>>(
                    IsolatedStorageSettings.ApplicationSettings[STORAGESETTING_NAME + appId].ToString());

                string tmpId = string.Empty,
                    tmpSecret = string.Empty;
                if (settings.ContainsKey(STORAGESETTING_APIID_NAME))
                    tmpId = (string)settings[STORAGESETTING_APIID_NAME];
                if (settings.ContainsKey(STORAGESETTING_APISECRET_NAME))
                    tmpSecret = (string)settings[STORAGESETTING_APISECRET_NAME];

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
        public static void SaveTokenSetting(string appId, string appSecret, AccelaTokenResult info)
        {
#if !(SILVERLIGHT || WINDOWS_PHONE)
            Dictionary<string, string> settings = new Dictionary<string, string>();
            settings.Add(STORAGESETTING_TOKEN_NAME, SimpleJson.SerializeObject(info));
            settings.Add(STORAGESETTING_APIID_NAME, appId);
            settings.Add(STORAGESETTING_APISECRET_NAME, appSecret);
            Accela.WindowsStoreSDK.Helper.FileHelper.SaveTextToFile(appId, SETTING_FILE, SimpleJson.SerializeObject(settings));
#else
            var settings = new Dictionary<string, object>();
            settings.Add(STORAGESETTING_APIID_NAME, appId);
            settings.Add(STORAGESETTING_APISECRET_NAME, appSecret);
            settings.Add(STORAGESETTING_TOKEN_NAME, info);
            IsolatedStorageSettings.ApplicationSettings[STORAGESETTING_NAME + appId] = SimpleJson.SerializeObject(settings);
            IsolatedStorageSettings.ApplicationSettings.Save();
#endif
        }

        public static void RemoveTokenSetting(string appId, string appSecret)
        {
#if !(SILVERLIGHT || WINDOWS_PHONE)
            Accela.WindowsStoreSDK.Helper.FileHelper.DeleteFile(appId, SETTING_FILE);
#else
            IsolatedStorageSettings.ApplicationSettings.Remove(STORAGESETTING_NAME + appId);
#endif
        }
    }
}
