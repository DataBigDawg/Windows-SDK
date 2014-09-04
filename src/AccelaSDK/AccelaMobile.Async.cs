using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Accela.WindowsStoreSDK
{
    public partial class AccelaSDK
    {
        private HttpWebRequestWrapper _httpWebRequest;
        private object _httpWebRequestLocker = new object();

        #region event

        /// <summary>
        /// Event handler for get completion.
        /// </summary>
        public event EventHandler<AccelaApiEventArgs> GetCompleted;

        /// <summary>
        /// Event handler for post completion.
        /// </summary>
        public event EventHandler<AccelaApiEventArgs> PostCompleted;

        /// <summary>
        /// Event handler for delete completion.
        /// </summary>
        public event EventHandler<AccelaApiEventArgs> DeleteCompleted;

        /// <summary>
        /// Event handler for put completion.
        /// </summary>
        public event EventHandler<AccelaApiEventArgs> PutCompleted;

        /// <summary>
        /// Event handler for upload progress changed.
        /// </summary>
        public event EventHandler<AccelaUploadProgressChangedEventArgs> UploadProgressChanged;

        /// <summary>
        /// Event handler for download progress changed.
        /// </summary>
        public event EventHandler<AccelaDownloadProgressChangedEventArgs> DownloadProgressChanged;

        /// <summary>
        /// Event handler when http web request wrapper is created for async api only.
        /// (used internally by TPL for cancellation support)
        /// </summary>
        private event EventHandler<HttpWebRequestCreatedEventArgs> HttpWebRequestWrapperCreated;

        #endregion

        /// <summary>
        /// Cancels asynchronous requests.
        /// </summary>
        /// <remarks>
        /// Does not cancel requests created using XTaskAsync methods.
        /// </remarks>
        public virtual void CancelAsync()
        {
            lock (_httpWebRequestLocker)
            {
                if (_httpWebRequest != null)
                    _httpWebRequest.Abort();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="path"></param>
        /// <param name="parameters"></param>
        /// <param name="resultType"></param>
        /// <param name="userState"></param>
        private void ApiAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState)
        {
            Stream input;
            var httpHelper = PrepareRequest(httpMethod, path, parameters, resultType, out input);
            _httpWebRequest = httpHelper.HttpWebRequest;

            if (HttpWebRequestWrapperCreated != null)
                HttpWebRequestWrapperCreated(this, new HttpWebRequestCreatedEventArgs(userState, httpHelper.HttpWebRequest));
            var uploadProgressChanged = UploadProgressChanged;
            bool notifyUploadProgressChanged = uploadProgressChanged != null && httpHelper.HttpWebRequest.Method == "POST";

            httpHelper.OpenReadCompleted +=
                (o, e) =>
                {
                    AccelaApiEventArgs args;
                    if (e.Cancelled)
                    {
                        args = new AccelaApiEventArgs(e.Error, true, userState, null);
                    }
                    else if (e.Error == null)
                    {
                        object responseObject = null;

                        try
                        {
                            using (var stream = e.Result)
                            {

                                bool binary = false;

                                //if (httpHelper.HttpWebResponse.ContentType
                                //        .Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
                                if (!(httpHelper.HttpWebResponse.ContentType.Contains("text/javascript") ||
                                      httpHelper.HttpWebResponse.ContentType.Contains("application/json") ||
                                      httpHelper.HttpWebResponse.ContentType.Contains("text/html")))
                                {
                                    binary = true;
                                }

                                if (!binary)
                                {
                                    //read string
#if NETFX_CORE
                                    //read compressed string
                                    bool compressed = false;
                                    var contentEncoding = httpHelper.HttpWebResponse.Headers.AllKeys.Contains("Content-Encoding") ? httpHelper.HttpWebResponse.Headers["Content-Encoding"] : null;
                                    if (contentEncoding != null)
                                    {
                                        if (contentEncoding.IndexOf("gzip") != -1)
                                        {
                                            using (var uncompressedStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                                            {
                                                using (var reader = new StreamReader(uncompressedStream))
                                                {
                                                    responseObject = reader.ReadToEnd();
                                                }
                                            }

                                            compressed = true;
                                        }
                                        else if (contentEncoding.IndexOf("deflate") != -1)
                                        {
                                            using (var uncompressedStream = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Decompress))
                                            {
                                                using (var reader = new StreamReader(uncompressedStream))
                                                {
                                                    responseObject = reader.ReadToEnd();
                                                }
                                            }

                                            compressed = true;
                                        }
                                    }

                                    if (!compressed)
                                    {
                                        using (var reader = new StreamReader(stream))
                                        {
                                            responseObject = reader.ReadToEnd();
                                        }
                                    }
#else
                                    //read uncompressed string
                                    using (var reader = new StreamReader(stream))
                                    {
                                        responseObject = reader.ReadToEnd();
                                    }
#endif
                                }
                                else
                                {
                                    //read stream
                                    using (var reader = new MemoryStream())
                                    {
                                        byte[] buffer = new byte[1024];

                                        int size = stream.Read(buffer, 0, buffer.Length);

                                        while (size > 0)
                                        {
                                            reader.Write(buffer, 0, size);

                                            size = stream.Read(buffer, 0, buffer.Length);
                                        }

                                        responseObject = reader.ToArray();
                                    }
                                }
                            }

                            try
                            {
                                object result = ProcessResponse(httpHelper, responseObject, resultType);
                                args = new AccelaApiEventArgs(null, false, userState, result);
                            }
                            catch (AccelaApiException ex)
                            {
                                args = new AccelaApiEventArgs(ex, false, userState, null);
                            }
                            catch (Exception ex)
                            {
                                args = new AccelaApiEventArgs(ex, false, userState, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            args = httpHelper.HttpWebRequest.IsCancelled ? new AccelaApiEventArgs(ex, true, userState, null) : new AccelaApiEventArgs(ex, false, userState, null);
                        }
                    }
                    else
                    {
                        var webEx = e.Error as WebExceptionWrapper;
                        if (webEx == null)
                        {
                            args = new AccelaApiEventArgs(e.Error, httpHelper.HttpWebRequest.IsCancelled, userState, null);
                        }
                        else
                        {
                            if (webEx.GetResponse() == null)
                            {
                                args = new AccelaApiEventArgs(webEx, false, userState, null);
                            }
                            else
                            {
                                var response = httpHelper.HttpWebResponse;
                                if (response.StatusCode == HttpStatusCode.NotModified)
                                {
                                    var jsonObject = new JsonObject();
                                    var headers = new JsonObject();

                                    foreach (var headerName in response.Headers.AllKeys)
                                        headers[headerName] = response.Headers[headerName];

                                    jsonObject["headers"] = headers;
                                    args = new AccelaApiEventArgs(null, false, userState, jsonObject);
                                }
                                else
                                {
                                    httpHelper.OpenReadAsync();
                                    return;
                                }
                            }
                        }
                    }

                    OnCompleted(httpMethod, args);
                };

            if (input == null)
            {
                httpHelper.OpenReadAsync();
            }
            else
            {
                // we have a request body so write
                httpHelper.OpenWriteCompleted +=
                    (o, e) =>
                    {
                        AccelaApiEventArgs args;
                        if (e.Cancelled)
                        {
                            input.Dispose();
                            args = new AccelaApiEventArgs(e.Error, true, userState, null);
                        }
                        else if (e.Error == null)
                        {
                            try
                            {
                                using (var stream = e.Result)
                                {
                                    // write input to requestStream
                                    var buffer = new byte[BufferSize];
                                    int nread;

                                    if (notifyUploadProgressChanged)
                                    {
                                        long totalBytesToSend = input.Length;
                                        long bytesSent = 0;

                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();

                                            // notify upload progress changed
                                            bytesSent += nread;
                                            OnUploadProgressChanged(new AccelaUploadProgressChangedEventArgs(0, 0, bytesSent, totalBytesToSend, ((int)(bytesSent * 100 / totalBytesToSend)), userState));
                                        }
                                    }
                                    else
                                    {
                                        while ((nread = input.Read(buffer, 0, buffer.Length)) != 0)
                                        {
                                            stream.Write(buffer, 0, nread);
                                            stream.Flush();
                                        }
                                    }
                                }

                                httpHelper.OpenReadAsync();
                                return;
                            }
                            catch (Exception ex)
                            {
                                args = new AccelaApiEventArgs(ex, httpHelper.HttpWebRequest.IsCancelled, userState, null);
                            }
                            finally
                            {
                                input.Dispose();
                            }
                        }
                        else
                        {
                            input.Dispose();
                            var webExceptionWrapper = e.Error as WebExceptionWrapper;
                            if (webExceptionWrapper != null)
                            {
                                var ex = webExceptionWrapper;
                                if (ex.GetResponse() != null)
                                {
                                    httpHelper.OpenReadAsync();
                                    return;
                                }
                            }

                            args = new AccelaApiEventArgs(e.Error, false, userState, null);
                        }

                        OnCompleted(httpMethod, args);
                    };

                httpHelper.OpenWriteAsync();
            }
        }

        private void OnCompleted(HttpMethod httpMethod, AccelaApiEventArgs args)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    OnGetCompleted(args);
                    break;
                case HttpMethod.Post:
                    OnPostCompleted(args);
                    break;
                case HttpMethod.Delete:
                    OnDeleteCompleted(args);
                    break;
                case HttpMethod.Put:
                    OnPutCompleted(args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("httpMethod");
            }
        }

        /// <summary>
        /// Raise OnUploadProgressCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        protected void OnUploadProgressChanged(AccelaUploadProgressChangedEventArgs args)
        {
            if (UploadProgressChanged != null)
                UploadProgressChanged(this, args);
        }

        /// <summary>
        /// Raise OnDownloadProgressCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        protected void OnDownloadProgressChanged(AccelaDownloadProgressChangedEventArgs args)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, args);
        }

        /// <summary>
        /// Raise OnPutCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        protected void OnPutCompleted(AccelaApiEventArgs args)
        {
            if (PutCompleted != null)
                PutCompleted(this, args);
        }

        /// <summary>
        /// Raise OnPostCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        private void OnPostCompleted(AccelaApiEventArgs args)
        {
            if (PostCompleted != null)
                PostCompleted(this, args);
        }

        /// <summary>
        /// Raise OnGetCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        private void OnGetCompleted(AccelaApiEventArgs args)
        {
            if (GetCompleted != null)
                GetCompleted(this, args);
        }

        /// <summary>
        /// Raise OnDeletedCompleted event handler.
        /// </summary>
        /// <param name="args">The <see cref="AccelaApiEventArgs"/>.</param>
        private void OnDeleteCompleted(AccelaApiEventArgs args)
        {
            if (DeleteCompleted != null)
                DeleteCompleted(this, args);
        }


    }
}
