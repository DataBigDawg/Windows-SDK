using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    internal class UrlHelper
    {
        public static IDictionary<string, object> ToDictionary(object parameters)
        {
            //IDictionary<string, AccelaSDKMediaObject> mediaObject;

            IDictionary<string, AccelaMediaStream> mediaStream;

            String jsonString;

            bool bodyType;

            return ToDictionary(parameters, out jsonString, /*out mediaObject, */out mediaStream, out bodyType);
        }

        public static IDictionary<string, object> ToDictionary(object parameters,
            out string parametersWithJsonString,
            /*out IDictionary<string, AccelaSDKMediaObject> mediaObject, */
            out IDictionary<string, AccelaMediaStream> mediaStream,
            out bool overrideBodyType)
        {
            //mediaObject = new Dictionary<string, AccelaSDKMediaObject>();

            mediaStream = new Dictionary<string, AccelaMediaStream>();

            parametersWithJsonString = String.Empty;

            overrideBodyType = false;

            var dictionary = parameters as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();

                if (parameters == null)
                {
                    return dictionary;
                }
                else if (parameters is string)
                {
                    parametersWithJsonString = (string)parameters;
                }
                else if (parameters is AccelaMediaStream)
                {
                    overrideBodyType = true;
                    mediaStream.Add("file", (AccelaMediaStream)parameters);
                }
                else if (parameters is IDictionary<string, string>)
                {
                    foreach (KeyValuePair<string, string> parameter in (IDictionary<string, string>)parameters)
                    {
                        if (parameter.Key.Equals("json", StringComparison.CurrentCultureIgnoreCase))
                        {
                            parametersWithJsonString = parameter.Value;
                            continue;
                        }
                        dictionary.Add(parameter.Key, parameter.Value);
                    }
                }
                else
                {
                    foreach (PropertyInfo info in from p in IntrospectionExtensions.GetTypeInfo(parameters.GetType()).DeclaredProperties select p)
                    {
                        dictionary.Add(info.Name, info.GetValue(parameters, null));
                    }
                }
            }

            foreach (var dic in dictionary)
            {
                //if (dic.Value is AccelaSDKMediaObject)
                //{
                //    mediaObject.Add(dic.Key, (AccelaSDKMediaObject)dic.Value);
                //} else
                if (dic.Value is AccelaMediaStream)
                {
                    mediaStream.Add(dic.Key, (AccelaMediaStream)dic.Value);
                }
            }

            //foreach (var obj in mediaObject)
            //{
            //    dictionary.Remove(obj.Key);
            //}

            foreach (var obj in mediaStream)
            {
                dictionary.Remove(obj.Key);
            }

            return dictionary;
        }

        public static Uri BuildUri(String host, String path, IDictionary<string, object> parameters)
        {

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            var strBuilder = new StringBuilder("https://");

            strBuilder.Append(host);

            if (host.EndsWith("/"))
            {
                strBuilder.Length--;
            }

            path = FillUrlParameters(path, parameters);


            if (!path.StartsWith("/"))
            {
                strBuilder.Append("/");
            }

            strBuilder.Append(path);

            strBuilder.Append(BuildParameters(parameters));

            return new Uri(strBuilder.ToString());
        }

        public static string BuildHttpQuery(object parameter, Func<string, string> encode)
        {
            if (parameter == null)
                return "null";

            if (parameter is string)
                return (string)parameter;

            if (parameter is bool)
                return (bool)parameter ? "true" : "false";

            if (parameter is int || parameter is long ||
                parameter is float || parameter is double || parameter is decimal ||
                parameter is byte || parameter is sbyte ||
                parameter is short || parameter is ushort ||
                parameter is uint || parameter is ulong)
                return parameter.ToString();

            if (parameter is Uri)
                return parameter.ToString();

            var sb = new StringBuilder();

            if (parameter is IEnumerable<KeyValuePair<string, string>>)
            {
                foreach (var kvp in (IEnumerable<KeyValuePair<string, string>>)parameter)
                    sb.AppendFormat("{0}={1}&", encode(kvp.Key), encode(kvp.Value));
            }
            else if (parameter is IEnumerable<KeyValuePair<string, object>>)
            {
                foreach (var kvp in (IEnumerable<KeyValuePair<string, object>>)parameter)
                    sb.AppendFormat("{0}={1}&", encode(kvp.Key), encode(BuildHttpQuery(kvp.Value, encode)));
            }
            else if (parameter is IEnumerable)
            {
                foreach (var obj in (IEnumerable)parameter)
                    sb.AppendFormat("{0},", encode(BuildHttpQuery(obj, encode)));
            }
            else if (parameter is DateTime)
            {
                return DateTimeConvertor.ToIso8601FormattedDateTime((DateTime)parameter);
            }
            else
            {
                //IDictionary<string, AccelaSDKMediaObject> mediaObjects;

                IDictionary<string, AccelaMediaStream> mediaStreams;

                String jsonString;

                bool bBodyType;

                var dict = ToDictionary(parameter, out jsonString, /*out mediaObjects,*/ out mediaStreams, out bBodyType);

                if (/*mediaObjects.Count > 0 || */mediaStreams.Count > 0)
                    throw new InvalidOperationException("Parameter can contain attachements (AccelaSDKMediaObject/AccelaSDKMediaStream) only in the top most level.");

                return BuildHttpQuery(dict, encode);
            }

            //if (sb.Length > 0)
            //    sb.Length--;

            return sb.ToString();
        }

        public static string ParseUrlQueryString(string path, IDictionary<string, object> parameters, bool forceParseAllUrls, out IDictionary<string, object> queryStrings)
        {
            Uri uri;
            bool flag;
            return ParseUrlQueryString(path, parameters, forceParseAllUrls, out queryStrings, out uri, out flag);
        }

        public static string ParseUrlQueryString(string path, IDictionary<string, object> parameters, bool forceParseAllUrls, out IDictionary<string, object> queryStrings, out Uri uri, out bool isLegacyRestApi)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            queryStrings = new Dictionary<string, object>();

            isLegacyRestApi = false;

            uri = null;

            path = FillUrlParameters(path, parameters);

            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (path.Length > 0 && path.StartsWith("/"))
            {
                path = path.Substring(1);
            }


            // split path and query
            string[] strArray = path.Split(new char[] { '?' });

            path = strArray[0];

            if (strArray.Length == 2 && strArray[1] != null)
            {
                string str = strArray[1];

                foreach (string param in str.Split(new char[] { '&' }))
                {
                    if (!string.IsNullOrEmpty(param))
                    {
                        string[] valueDic = param.Split(new char[] { '=' });

                        if (valueDic.Length != 2 || string.IsNullOrEmpty(valueDic[0]))
                        {
                            throw new ArgumentException("Invalid path", "path");
                        }

                        string key = HttpHelper.UrlDecode(valueDic[0]);

                        //if (!parameters.ContainsKey(key))
                        //{
                        //    parameters.Add(key, HttpHelper.UrlDecode(valueDic[1]));
                        //}
                        if (!queryStrings.ContainsKey(key))
                        {
                            queryStrings.Add(key, HttpHelper.UrlDecode(valueDic[1]));
                        }
                    }
                }

                return path;
            }
            if (strArray.Length > 2)
            {
                throw new ArgumentException("Invalid path", "path");
            }
            return path;
        }

        private static string BuildParameters(IDictionary<string, object> parameters)
        {
            if (parameters == null)
                return "";

            StringBuilder builder = new StringBuilder();

            builder.Append("?");

            foreach (var pair in parameters)
            {
                builder.AppendFormat("{0}={1}&", HttpHelper.UrlEncode(pair.Key), HttpHelper.UrlEncode(BuildHttpQuery(pair.Value, HttpHelper.UrlEncode)));
            }

            builder.Length--;

            return builder.ToString();
        }

        private static string FillUrlParameters(string path, IDictionary<string, object> parameters)
        {
            var regex = new Regex("{(?'parameter'[^{]+)}", RegexOptions.IgnoreCase);
            if (regex.IsMatch(path))
            {
                var argumentsMatch = regex.Matches(path);

                foreach (Match match in argumentsMatch)
                {
                    string key = match.Groups["parameter"].Value;
                    if (parameters.ContainsKey(key))
                    {
                        var param = (string)parameters[key];
                        path = path.Replace("{" + key + "}", HttpHelper.UrlEncode(param ?? string.Empty));
                        parameters.Remove(key);
                    }
                }
            }
            return path;
        }
    }
}
