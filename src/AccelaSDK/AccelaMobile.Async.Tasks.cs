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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    public partial class AccelaSDK
    {

        private static void TransferCompletionToTask<T>(TaskCompletionSource<T> tcs, System.ComponentModel.AsyncCompletedEventArgs e, Func<T> getResult, Action unregisterHandler)
        {
            if (e.UserState != tcs)
                return;

            try
            {
                unregisterHandler();
            }
            finally
            {
                if (e.Cancelled) tcs.TrySetCanceled();
                else if (e.Error != null) tcs.TrySetException(e.Error);
                else tcs.TrySetResult(getResult());
            }
        }

        private void RemoveTaskAsyncHandlers(HttpMethod httpMethod, EventHandler<AccelaApiEventArgs> handler)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    GetCompleted -= handler;
                    break;
                case HttpMethod.Post:
                    PostCompleted -= handler;
                    break;
                case HttpMethod.Delete:
                    DeleteCompleted -= handler;
                    break;
            }
        }

        #region ApiTaskAsync

        /// <summary>
        /// Makes an asynchronous request to the AccelaSDK server.
        /// </summary>
        /// <param name="httpMethod">Http method. (GET/POST/DELETE)</param>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="resultType">The type of deserialize object into.</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The task of json result with headers.</returns>
        private Task<object> ApiTaskAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(httpMethod, path, parameters, resultType, userState, cancellationToken, null);
        }

        private Task<object> ApiTaskAsync(HttpMethod httpMethod, string path, object parameters, Type resultType, object userState, CancellationToken cancellationToken, IProgress<AccelaUploadProgressChangedEventArgs> uploadProgress)
        {
            var tcs = new TaskCompletionSource<object>(userState);
            if (cancellationToken.IsCancellationRequested)
            {
                tcs.TrySetCanceled();
                return tcs.Task;
            }

            HttpWebRequestWrapper httpWebRequest = null;

            EventHandler<HttpWebRequestCreatedEventArgs> httpWebRequestCreatedHandler = null;
            httpWebRequestCreatedHandler += (o, e) =>
            {
                if (e.UserState != tcs)
                    return;
                httpWebRequest = e.HttpWebRequest;
            };

            var ctr = cancellationToken.Register(() =>
            {
                try
                {
                    if (httpWebRequest != null) httpWebRequest.Abort();
                }
                catch
                {
                }
            });

            EventHandler<AccelaUploadProgressChangedEventArgs> uploadProgressHandler = null;
            if (uploadProgress != null)
            {
                uploadProgressHandler = (sender, e) =>
                {
                    if (e.UserState != tcs)
                        return;
                    uploadProgress.Report(new AccelaUploadProgressChangedEventArgs(e.BytesReceived, e.TotalBytesToReceive, e.BytesSent, e.TotalBytesToSend, e.ProgressPercentage, userState));
                };

                UploadProgressChanged += uploadProgressHandler;
            }

            EventHandler<AccelaApiEventArgs> handler = null;
            handler = (sender, e) =>
            {
                TransferCompletionToTask(tcs, e, e.GetResultData, () =>
                {
                    if (ctr != null) ctr.Dispose();
                    RemoveTaskAsyncHandlers(httpMethod, handler);
                    HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                    if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
                });
            };



            if (httpMethod == HttpMethod.Get)
                GetCompleted += handler;
            else if (httpMethod == HttpMethod.Post)
                PostCompleted += handler;
            else if (httpMethod == HttpMethod.Delete)
                DeleteCompleted += handler;
            else if (httpMethod == HttpMethod.Put)
                PutCompleted += handler;
            else
                throw new ArgumentOutOfRangeException("httpMethod");

            HttpWebRequestWrapperCreated += httpWebRequestCreatedHandler;

            try
            {
                ApiAsync(httpMethod, path, parameters, resultType, tcs);
            }
            catch
            {
                RemoveTaskAsyncHandlers(httpMethod, handler);
                HttpWebRequestWrapperCreated -= httpWebRequestCreatedHandler;
                if (uploadProgressHandler != null) UploadProgressChanged -= uploadProgressHandler;
                throw;
            }

            return tcs.Task;
        }

        #endregion

        #region GetTaskAsync

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <returns>The json result task.</returns>
        internal virtual Task<object> GetTaskAsync(string path)
        {
            return GetTaskAsync(path, null, CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result task.</returns>
        internal Task<object> GetTaskAsync(string path, object parameters)
        {
            return GetTaskAsync(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> GetTaskAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return GetTaskAsync(path, parameters, cancellationToken, null);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="resultType">The result type.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> GetTaskAsync(string path, object parameters, CancellationToken cancellationToken, Type resultType)
        {
            return ApiTaskAsync(HttpMethod.Get, path, parameters, resultType, null, cancellationToken);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <returns>The json result task.</returns>
        internal Task<TResult> GetTaskAsync<TResult>(string path, object parameters, CancellationToken cancellationToken)
        {
            return GetTaskAsync(path, parameters, cancellationToken, typeof(TResult))
                .Then(result => (TResult)result);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <returns>The json result task.</returns>
        internal Task<TResult> GetTaskAsync<TResult>(string path, object parameters)
        {
            return GetTaskAsync<TResult>(path, parameters, CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous GET request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <returns>The json result task.</returns>
        internal Task<TResult> GetTaskAsync<TResult>(string path)
        {
            return GetTaskAsync<TResult>(path, null);
        }

        #endregion

        #region PostTaskAsync

        internal Task<object> PostTaskAsync(string path, object parameters, object userState, CancellationToken cancellationToken, IProgress<AccelaUploadProgressChangedEventArgs> uploadProgress, Type resultType)
        {
            return ApiTaskAsync(HttpMethod.Post, path, parameters, resultType, userState, cancellationToken, uploadProgress);
        }


        /// <summary>
        /// Makes an asynchronous POST request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="uploadProgress">The upload progress</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PostTaskAsync(string path, object parameters, object userState, CancellationToken cancellationToken, IProgress<AccelaUploadProgressChangedEventArgs> uploadProgress)
        {
            return ApiTaskAsync(HttpMethod.Post, path, parameters, null, userState, cancellationToken, uploadProgress);
        }

        /// <summary>
        /// Makes an asynchronous POST request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="userState">The user state.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PostTaskAsync(string path, object parameters, object userState, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(HttpMethod.Post, path, parameters, null, userState, cancellationToken);
        }


        /// <summary>
        /// Makes an asynchronous POST request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PostTaskAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(HttpMethod.Post, path, parameters, null, null, cancellationToken);
        }

        internal Task<TResult> PostTaskAsync<TResult>(string path, object parameters, object userState, CancellationToken cancellationToken, IProgress<AccelaUploadProgressChangedEventArgs> uploadProgress) where TResult : new()
        {
            return PostTaskAsync(path, parameters, userState, cancellationToken, uploadProgress, typeof(TResult)).Then(result => (TResult)result);
        }


        /// <summary>
        /// Makes an asynchronous POST request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PostTaskAsync(string path, object parameters)
        {
            return PostTaskAsync(path, parameters, CancellationToken.None);
        }

        #endregion

        #region DeleteTaskAsync

        /// <summary>
        /// Makes an asynchronous DELETE request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> DeleteTaskAsync(string path)
        {
            return DeleteTaskAsync(path, null, CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous DELETE request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> DeleteTaskAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(HttpMethod.Delete, path, parameters, null, null, cancellationToken);
        }


        #endregion

        #region PutTaskAsync

        /// <summary>
        /// Makes an asynchronous PUT request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PutTaskAsync(string path)
        {
            return PutTaskAsync(path, null, CancellationToken.None);
        }

        /// <summary>
        /// Makes an asynchronous PUT request to the AccelaSDK server.
        /// </summary>
        /// <param name="path">The resource path or the resource url.</param>
        /// <param name="parameters">The parameters</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The json result task.</returns>
        internal Task<object> PutTaskAsync(string path, object parameters, CancellationToken cancellationToken)
        {
            return ApiTaskAsync(HttpMethod.Put, path, parameters, null, null, cancellationToken);
        }


        #endregion
    }
}
