using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// <see cref="AccelaSDK"/> is the main object of the Accela SDK for Windows.
    /// </summary>
    public partial class AccelaSDK
    {

        #region Private Property
        private const int BufferSize = 1024 * 4; // 4kb 
        private const string AttachmentMustHavePropertiesSetError = "Attachment (AccelaMediaObject/AccelaMediaStream) must have a content type, file name, and value set.";
        private const string AttachmentValueIsNull = "The value of attachment (AccelaMediaObject/AccelaMediaStream) is null.";
        private const string UnknownResponse = "Unknown AccelaSDK response.";
        private const string MultiPartFormPrefix = "--";
        private const string MultiPartNewLine = "\r\n";
        private const string HttpRequestAgencyHeader = "x-accela-agency";
        private const string EMPTY_JSONSTRING = "{}";

        private AccelaTokenResult _tokenInfo;

        private Func<object, string> _serializeJson;
        private static Func<object, string> _defaultJsonSerializer;

        private Func<string, Type, object> _deserializeJson;
        private static Func<string, Type, object> _defaultJsonDeserializer;

        #endregion

        #region public property

        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        public String AppId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the app secret.
        /// </summary>
        public String AppSecret
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the agency name.
        /// </summary>
        public String Agency
        {
            get;
            private set;
        }

        /// <summary>
        /// Serialize object to json.
        /// </summary>
        public Func<object, string> SerializeJson
        {
            get { return _serializeJson ?? (_serializeJson = _defaultJsonSerializer); }
            set { _serializeJson = value; }
        }

        /// <summary>
        /// Deserialize json to object.
        /// </summary>
        public Func<string, Type, object> DeserializeJson
        {
            get { return _deserializeJson; }
            set { _deserializeJson = value ?? (_deserializeJson = _defaultJsonDeserializer); }
        }

        /// <summary>
        /// Get or Set log display of relevant levels
        /// </summary>
        public static AccelaLogLevel LogLevel
        {
            get;
            set;
        }

        #endregion

        #region structure
        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaSDK"/> class.
        /// </summary>
        static AccelaSDK()
        {
            LogLevel =
                //#if DEBUG
 AccelaLogLevel.Info;
            //#else
            // AccelaLogLevel.None;
            //#endif

            SetDefaultJsonSerializers(null, null);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaSDK"/> class.
        /// </summary>
        private AccelaSDK()
        {
            _deserializeJson = _defaultJsonDeserializer;
            _environment = AccelaEnvironment.PROD;
            _oauthHost = AccelaSettings.AM_HOST_OAUTH;
            _apiHost = AccelaSettings.AM_HOST_API;
            _customHttpHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaSDK"/> class.
        /// </summary>
        /// <param name="appId">The Accele Mobile application id. it is created and listed in https://developer.accela.com/ApplicationList/index </param>
        /// <param name="appSecret">App secret generated in Developer Portal</param>
        public AccelaSDK(String appId, String appSecret)
            : this()
        {
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            if (string.IsNullOrWhiteSpace(appSecret))
                throw new ArgumentNullException("appSecret");

            this.AppId = appId;

            this.AppSecret = appSecret;

            this._tokenInfo = AccelaSettings.ReadTokenSetting(appId, appSecret);
        }


        #endregion

        #region public static

        /// <summary>
        /// Sets the default json seriazliers and deserializers.
        /// </summary>
        /// <param name="jsonSerializer">Json serializer</param>
        /// <param name="jsonDeserializer">Json deserializer</param>
        internal static void SetDefaultJsonSerializers(Func<object, string> jsonSerializer, Func<string, Type, object> jsonDeserializer)
        {
            _defaultJsonSerializer = jsonSerializer ?? SimpleJson.SerializeObject;
            _defaultJsonDeserializer = jsonDeserializer ?? SimpleJson.DeserializeObject;
        }

        #endregion

        #region private method

        private AccelaOAuthResult ParseOAuthCallbackUrl(Uri uri)
        {
            bool flag = false;

            var dictionary = new Dictionary<string, object>();
            IDictionary<string, object> queryString;
            UrlHelper.ParseUrlQueryString(uri.Query, dictionary, false, out queryString);

            if (queryString.ContainsKey("code") || queryString.ContainsKey("error"))
            {
                flag = true;
            }

            if (!flag)
            {
                throw new InvalidOperationException("Could not parse Accela OAuth url.");
            }

            return new AccelaOAuthResult(queryString);
        }

        private Uri GetLoginUri(string[] permissions, string agency = null)
        {
            if (permissions == null)
                permissions = default(string[]);

            this._permissions = permissions;
            this.Agency = agency;

            dynamic param = new ExpandoObject();
            param.client_id = this.AppId;
            param.scope = string.Join(" ", permissions);
            param.redirect_uri = AccelaSettings.AM_CALLBACK_URL;
            param.response_type = "code";
            param.state = (new Random().Next(int.MaxValue)).ToString();
            //v3 2013-03-05 lex.xiong
            param.environment = _environment.ToString();

            if (!string.IsNullOrWhiteSpace(this.Agency))
            {
                param.agency_name = this.Agency;

                if (!_customHttpHeaders.ContainsKey(HttpRequestAgencyHeader))
                    _customHttpHeaders.Add(HttpRequestAgencyHeader, this.Agency);

                _customHttpHeaders[HttpRequestAgencyHeader] = this.Agency;
            }

            var dictionary = UrlHelper.ToDictionary(param);

            return UrlHelper.BuildUri(_oauthHost, AccelaSettings.AM_PATH_OAUTH_AUTHORIZE, dictionary);

        }

        private async Task<AccelaTokenResult> GetToken(string code = null, bool isRefresh = false)
        {
            if (!this._isNativeAuthorization && code == null)
                throw new ArgumentNullException("code");

            dynamic param = new ExpandoObject();
            param.client_id = AppId;
            param.client_secret = AppSecret;
            param.grant_type = isRefresh ? "refresh_token" :
                (this._isNativeAuthorization ? "password" : "authorization_code");
            if (!string.IsNullOrEmpty(this.Agency))
                param.agency_name = this.Agency;

            if (isRefresh == false)
            {
                if (this._isNativeAuthorization == true)
                {
                    param.username = this._userName;
                    param.password = this._passWord;
                    param.scope = string.Join(" ", this._permissions);
                    param.environment = this._environment.ToString();
                }
                else
                {
                    param.redirect_uri = AccelaSettings.AM_CALLBACK_URL;
                    param.code = code;
                }
            }
            else
            {
                param.refresh_token = _tokenInfo.refresh_token;
            }

            var tokenResult = await this.PostAsync<AccelaTokenResult>(AccelaSettings.AM_PATH_ACCESS_TOKEN, param);
            if (tokenResult != null)
                tokenResult.CalculateExpiresTime();

            return tokenResult;
        }

        private HttpHelper PrepareRequest(HttpMethod httpMethod, string path, object parameters, Type resultType, out Stream input)
        {
            input = null;

            // Body Type: text, form, file
            // If you override this parameter is "true", then post a request is sent using a binary file
            bool bOverrideBodyType;
            IDictionary<string, AccelaMediaStream> mediaStreams;
            IDictionary<string, object> paramsWithQueryStrings;
            String jsonString;
            IDictionary<string, object> paramsWithoutMediaObjects = UrlHelper.ToDictionary(parameters, out jsonString, /*out mediaObjects,*/ out mediaStreams, out bOverrideBodyType) ?? new Dictionary<string, object>();


            Uri uri;
            bool isLegacyRestApi = false;
            path = UrlHelper.ParseUrlQueryString(path, paramsWithoutMediaObjects, false, out paramsWithQueryStrings, out uri, out isLegacyRestApi);

            // if using a global language
            if (!paramsWithQueryStrings.ContainsKey("lang"))
                paramsWithQueryStrings["lang"] = "en_US";

            UriBuilder uriBuilder;
            if (uri == null)
            {
                uriBuilder = new UriBuilder { Scheme = "https" };

                if (path.EndsWith(AccelaSettings.AM_PATH_OAUTH_AUTHORIZE))
                {
                    uriBuilder.Host = _oauthHost;
                }
                else
                {
                    uriBuilder.Host = _apiHost;
                }

                path = path ?? string.Empty;
            }
            else
            {
                uriBuilder = new UriBuilder { Host = uri.Host, Scheme = uri.Scheme };
            }

            uriBuilder.Path = path;

            string contentType = null;

            long? contentLength = null;

            var queryString = new StringBuilder();

            SerializeParameters(paramsWithoutMediaObjects);

            SerializeParameters(paramsWithQueryStrings);

            // get put delete post
            if (httpMethod == HttpMethod.Get ||
                httpMethod == HttpMethod.Delete)
            {
                // for GET,DELETE
                if (/*mediaObjects.Count > 0 && */mediaStreams.Count > 0)
                    throw new InvalidOperationException("Attachments (AccelaMediaStream) are valid only in POST requests.");


                foreach (var kvp in paramsWithoutMediaObjects)
                    queryString.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(UrlHelper.BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));

                foreach (var kvp in paramsWithQueryStrings)
                    queryString.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(kvp.Key), HttpHelper.UrlEncode(UrlHelper.BuildHttpQuery(kvp.Value, HttpHelper.UrlEncode)));

            }
            else
            {
                // for POST, PUT
                if (/*mediaObjects.Count == 0 && */mediaStreams.Count == 0 && !paramsWithoutMediaObjects.ContainsKey("json-data"))
                {
                    queryString.Append(UrlHelper.BuildHttpQuery(paramsWithQueryStrings, HttpHelper.UrlEncode));

                    if (uriBuilder.Path.EndsWith(AccelaSettings.AM_PATH_ACCESS_TOKEN))
                    {
                        contentType = "application/x-www-form-urlencoded";

                        queryString.Append(UrlHelper.BuildHttpQuery(paramsWithoutMediaObjects, HttpHelper.UrlEncode));

                        AccelaSDKLogger.logInfo("AccelaSDK_RequestData", queryString, AccelaLogLevel.Info);

                        input = queryString.Length == 0 ? null : (Stream)new MemoryStream(Encoding.UTF8.GetBytes(queryString.ToString()));
                    }
                    else
                    {
                        contentType = "application/json";

                        AccelaSDKLogger.logInfo("AccelaSDK_RequestData", jsonString, AccelaLogLevel.Info);

                        if (string.IsNullOrWhiteSpace(jsonString))
                            jsonString = EMPTY_JSONSTRING;
                        input = jsonString.Length == 0 ? null : (Stream)new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                    }
                }
                else
                {
                    if (bOverrideBodyType == true)
                    {
                        contentType = "application/octet-stream";
                        Stream stream = null;
                        foreach (var media in mediaStreams)
                        {
                            if (media.Value != null)
                            {
                                stream = media.Value.GetValue();
                                break;
                            }
                        }

                        input = stream ?? new MemoryStream(Encoding.UTF8.GetBytes("{}"));
                    }
                    else
                    {
                        #region multipart/form-data

                        // timestamp
                        string strBoundary = DateTime.Now.Ticks.ToString("x");

                        contentType = string.Concat("multipart/form-data; boundary=", strBoundary);

                        var streams = new List<Stream>();

                        var indexOfDisposableStreams = new List<int>();

                        queryString.Append(UrlHelper.BuildHttpQuery(paramsWithQueryStrings, HttpHelper.UrlEncode));

                        // Build up the post message header
                        #region Add Form Data
                        foreach (var param in paramsWithoutMediaObjects)
                        {
                            StringBuilder formBuilder = new StringBuilder();

                            formBuilder.Append(MultiPartFormPrefix).Append(strBoundary).Append(MultiPartNewLine);

                            formBuilder.Append("Content-Disposition: form-data; name=\"").Append(param.Key).Append("\";").Append(MultiPartNewLine).Append(MultiPartNewLine);

                            indexOfDisposableStreams.Add(streams.Count);

                            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(formBuilder.ToString())));

                            indexOfDisposableStreams.Add(streams.Count);

                            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(param.Value.ToString()));

                            streams.Add(stream);

                            indexOfDisposableStreams.Add(streams.Count);

                            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                        }
                        #endregion

                        #region Add Media Objects
                        //var strBuilder = new StringBuilder();

                        //foreach (var param in parametersWithoutMediaObjects)
                        //{
                        //    strBuilder.Append(MultiPartFormPrefix).Append(strBoundary).Append(MultiPartNewLine);

                        //    strBuilder.Append("Content-Disposition: form-data; name=\"").Append(param.Key).Append("\"");

                        //    strBuilder.Append(MultiPartNewLine).Append(MultiPartNewLine);

                        //    strBuilder.Append(UrlHelper.BuildHttpQuery(param.Value, HttpHelper.UrlEncode));

                        //    strBuilder.Append(MultiPartNewLine);
                        //}

                        //indexOfDisposableStreams.Add(streams.Count);

                        //streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(strBuilder.ToString())));

                        //foreach (var param in mediaObjects)
                        //{
                        //    var objBuilder = new StringBuilder();

                        //    var obj = param.Value;

                        //    if (obj.ContentType == null || obj.GetValue() == null || string.IsNullOrWhiteSpace(obj.FileName))
                        //    {
                        //        throw new InvalidOperationException(AttachmentMustHavePropertiesSetError);
                        //    }

                        //    objBuilder.Append(MultiPartFormPrefix).Append(strBoundary).Append(MultiPartNewLine);

                        //    objBuilder.Append("Content-Disposition: form-data; name=\"").Append(param.Key).Append("\"; filename=\"").Append(obj.FileName).Append("\"").Append(MultiPartNewLine);

                        //    objBuilder.Append("Content-Type: ").Append(obj.ContentType).Append(MultiPartNewLine).Append(MultiPartNewLine);

                        //    indexOfDisposableStreams.Add(streams.Count);

                        //    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(objBuilder.ToString())));

                        //    byte[] buffer = obj.GetValue();

                        //    if (buffer == null)
                        //    {
                        //        throw new InvalidOperationException(AttachmentValueIsNull);
                        //    }

                        //    indexOfDisposableStreams.Add(streams.Count);

                        //    streams.Add(new MemoryStream(buffer));

                        //    indexOfDisposableStreams.Add(streams.Count);

                        //    streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                        //}
                        #endregion

                        #region Add Media Streams
                        foreach (var param in mediaStreams)
                        {
                            StringBuilder streamBuilder = new StringBuilder();

                            var obj = param.Value;

                            if (obj.GetValue() == null || string.IsNullOrWhiteSpace(obj.FileName))
                            {
                                throw new InvalidOperationException(AttachmentMustHavePropertiesSetError);
                            }

                            streamBuilder.Append(MultiPartFormPrefix).Append(strBoundary).Append(MultiPartNewLine);

                            streamBuilder.Append("Content-Disposition: form-data; name=\"").Append(param.Key).Append("\"; filename=\"").Append(obj.FileName).Append("\"").Append(MultiPartNewLine);

                            streamBuilder.Append("Content-Type: ").Append(obj.ContentType ?? "application/octet-stream").Append(MultiPartNewLine).Append(MultiPartNewLine);

                            indexOfDisposableStreams.Add(streams.Count);

                            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(streamBuilder.ToString())));

                            indexOfDisposableStreams.Add(streams.Count);

                            Stream stream = obj.GetValue();

                            if (stream == null)
                            {
                                throw new InvalidOperationException(AttachmentValueIsNull);
                            }

                            streams.Add(stream);

                            indexOfDisposableStreams.Add(streams.Count);

                            streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(MultiPartNewLine)));
                        }
                        #endregion

                        indexOfDisposableStreams.Add(streams.Count);

                        streams.Add(new MemoryStream(Encoding.UTF8.GetBytes(string.Concat(new string[] { MultiPartNewLine, MultiPartFormPrefix, strBoundary, MultiPartFormPrefix, MultiPartNewLine }))));

                        input = new CombinationStream(streams, indexOfDisposableStreams);

                        //AccelaSDKLogger.logInfo("AccelaSDK_PrepareRequest", new StreamReader(input).ReadToEnd(), AccelaLogLevel.Info);

                        #endregion
                    }
                }

                contentLength = (input == null) ? 0L : input.Length;

            }

            if (queryString.Length > 0)
                queryString.Length--;

            uriBuilder.Query = queryString.ToString();

            var request = new HttpWebRequestWrapper((HttpWebRequest)WebRequest.Create(uriBuilder.Uri));

            switch (httpMethod)
            {
                case HttpMethod.Get:
                    request.Method = "GET";
                    break;
                case HttpMethod.Delete:
                    request.Method = "DELETE";
                    request.TrySetContentLength(0);
                    break;
                case HttpMethod.Post:
                    request.Method = "POST";
                    break;
                case HttpMethod.Put:
                    request.Method = "PUT";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }

            request.ContentType = contentType;



            #region setHttpHeaders
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";

            if (this._tokenInfo != null && !String.IsNullOrWhiteSpace(this._tokenInfo.access_token))
                request.Headers[HttpRequestHeader.Authorization] = this._tokenInfo.access_token;

            //request.Headers[HttpRequestHeader.Authorization] = "sE2iTyp-BJ0aDflCFV6uMpYbZdbgamqP3GUNCmh0mPbYWgRHTiE8ZkO60l2CnbukaEUEU7iDgCrHLFMtZ-KWW7F1Nm1UfyNUdwFb0DuhUqV1Jr1aiG8Vx5KJeNGkCH0yJON_RpHWTSmo7xsvZFuDOTKRLr64i3YcYDQhD9-_9h_zgd_OUCqZBzBBYcdO1GRz0yDJjBAGlS9sMhLiASFAsGYvUD1KtZVF60iMGNVsDyRo1s3106XclgEkjEgkZe-YfcgTT5T3bKQntbXxE4pxgwa2jgiulPx1KT3zZgvl2BJBWCxK1a4ghS7EsPttF0XJWS3n5i1GbK4vJJuf4AznN_Od4MLqZjNIirdETJzrjbhzaShFDS6CuttxOeFh1eG4TfRZy2WZH9wBzmSc5Y2XTYJ9ePeGWS-Lllv-TBttZx45Im8gOuhe9XM42l_qAeGF6eCN5UzRjyRZipGWuSYBsDqLgI02ohieVKpopBxp3Pyd9jCImnDxqdGi-Qko_kHmzNGdjgI3NcAV2RZykX4gRA2";

            if (!String.IsNullOrWhiteSpace(this.AppId))
                request.Headers["x-accela-appid"] = this.AppId;

            if (!String.IsNullOrWhiteSpace(this.AppSecret))
                request.Headers["x-accela-appsecret"] = this.AppSecret;

            if (!String.IsNullOrWhiteSpace(this.Agency))
            {
                request.Headers["x-accela-agency"] = this.Agency;
            }

            if (_customHttpHeaders != null && _customHttpHeaders.Count > 0)
            {
                foreach (var header in _customHttpHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            request.Headers["x-accela-environment"] = this._environment.ToString();

            request.Headers["x-accela-appversion"] = AccelaSettings.AM_SDK_VERSION;

#if ( WINDOWS_PHONE)
            request.Headers["x-accela-appplatform"] = String.Format("{0}|{1}|{2}", System.Environment.OSVersion.Platform,
                                                                                   System.Environment.OSVersion.Version,
                                                                                   Microsoft.Phone.Info.DeviceStatus.DeviceName);
#else
            request.Headers["x-accela-appplatform"] = String.Format("{0}|{1}|{2}", "Windows",
                                                                                   "8.0",
                                                                                   "Windows Device");
#endif


            #endregion

            if (contentLength.HasValue)
                request.TrySetContentLength(contentLength.Value);

            //request.TrySetUserAgent(AccelaSettings.AM_SDK_NAME);

            AccelaSDKLogger.logInfo("AccelaSDK_PrepareRequest", request, AccelaLogLevel.Info);

            return new HttpHelper(request);
        }

        private object ProcessResponse(HttpHelper httpHelper, object responseObject, Type resultType)
        {

            try
            {
                object result = null;

                AccelaApiException exception = null;
                var response = httpHelper.HttpWebResponse;

                if (response == null)
                    throw new InvalidOperationException(UnknownResponse);

                AccelaSDKLogger.logInfo("AccelaSDK_ProcessResponse", response, AccelaLogLevel.Info);

                if (response.ContentType.Contains("text/javascript") ||
                    response.ContentType.Contains("application/json") ||
                    response.ContentType.Contains("text/html"))
                {
                    result = DeserializeJson((string)responseObject, null);
                    exception = GetException(httpHelper, result);
                    if (exception == null)
                    {
                        AccelaSDKLogger.logInfo("AccelaSDK_ResponseData", responseObject, AccelaLogLevel.Info);
                        if (resultType != null)
                            result = DeserializeJson((string)responseObject, resultType);
                    }
                }
                else //if (response.ContentType.Contains("application/octet-stream"))
                {
                    result = responseObject;
                }
                //else
                //{
                //    throw new InvalidOperationException(UnknownResponse);
                //}

                if (exception == null)
                {
                    return result;
                }

                throw exception;
            }
            catch (AccelaApiException)
            {
                throw;
            }
            catch (Exception)
            {
                if (httpHelper != null && httpHelper.InnerException != null)
                    throw httpHelper.InnerException;

                throw;
            }
        }

        private AccelaApiException GetException(HttpHelper httpHelper, object result)
        {
            if (result == null)
                return null;

            var responseDict = result as IDictionary<string, object>;
            if (responseDict == null)
                return null;

            AccelaApiException resultException = null;

            if (httpHelper != null)
            {
                var response = httpHelper.HttpWebResponse;
                var responseUri = response.ResponseUri;

                if (httpHelper.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    int httpErrorStatus = (int)httpHelper.HttpWebResponse.StatusCode;//int.Parse(responseDict["status"].ToString());

                    string errorMsg = null, errorCode = null, traceId = null, more = null;

                    AccelaOAuthErrorResult oAuthErrorResult = (AccelaOAuthErrorResult)DeserializeJson(result.ToString(), typeof(AccelaOAuthErrorResult));
                    if (!string.IsNullOrEmpty(oAuthErrorResult.error))
                    {
                        errorCode = oAuthErrorResult.error;
                        errorMsg = oAuthErrorResult.error_description;
                        if (!string.IsNullOrEmpty(response.Headers["x-accela-traceId"]))
                            traceId = response.Headers["x-accela-traceId"];
                        resultException = new AccelaOAuthException(errorMsg)
                        {
                            OAuthError = oAuthErrorResult,
                            ErrorCode = errorCode,
                            ErrorStatus = httpErrorStatus,
                            TraceId = traceId
                        };
                    }
                    else
                    {
                        AccelaApiErrorResult errorResult = (AccelaApiErrorResult)DeserializeJson(result.ToString(), typeof(AccelaApiErrorResult));
                        errorCode = errorResult.code;
                        errorMsg = errorResult.message;
                        traceId = errorResult.traceId;
                        more = errorResult.more;
                        //resultException = new AccelaApiException("Status:"+ httpErrorStatus + " ErrorCode:" + errorCode + "ErrorMsg:" + errorMsg + "More:" + more + "TraceId:" + traceId)
                        resultException = new AccelaApiException(errorMsg)
                        {
                            ApiError = errorResult,
                            ErrorCode = errorCode,
                            ErrorStatus = httpErrorStatus,
                            TraceId = traceId
                        };
                    }

                    AccelaSDKLogger.logInfo("AccelaSDK_GetException", resultException, AccelaLogLevel.Error);

                    return resultException;
                }

            }
            return null;
        }

        private void SerializeParameters(IDictionary<string, object> parameters)
        {
            var keysThatAreNotString = new List<string>();
            foreach (var key in parameters.Keys)
            {
                if (!(parameters[key] is string))
                    keysThatAreNotString.Add(key);
            }

            foreach (var key in keysThatAreNotString)
                parameters[key] = SerializeJson(parameters[key]);
        }

        private void ParseParameters(ref IDictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                if (parameters.ContainsKey("client_id") && string.IsNullOrEmpty(this.AppId))
                {
                    this.AppId = parameters["client_id"] as String;
                }
                else if (!string.IsNullOrEmpty(this.AppId))
                {
                    parameters.Add("client_id", this.AppId);
                }

                if (parameters.ContainsKey("client_secret") && string.IsNullOrEmpty(this.AppSecret))
                {
                    this.AppSecret = parameters["client_secret"] as String;
                }
                else if (!string.IsNullOrEmpty(this.AppSecret))
                {
                    parameters.Add("client_secret", this.AppId);
                }

                if (parameters.ContainsKey("token") && this._tokenInfo != null && string.IsNullOrEmpty(this._tokenInfo.access_token))
                {
                    this._tokenInfo.access_token = parameters["token"] as String;
                }
                else if (this._tokenInfo != null && !string.IsNullOrEmpty(this._tokenInfo.access_token))
                {
                    parameters.Add("token", this._tokenInfo.access_token);
                }
            }
        }

        #endregion

    }
}
