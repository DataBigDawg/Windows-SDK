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

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Server error result for api
    /// </summary>
    public class AccelaApiErrorResult
    {
        /// <summary>
        /// HTTP Status
        /// </summary>
        public int status { get;  set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string message { get;  set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public string code { get;  set; }

        /// <summary>
        /// More something
        /// </summary>
        public string more { get;  set; }

        /// <summary>
        /// Server trace Id
        /// </summary>
        public string traceId { get;  set; }
    }

    /// <summary>
    /// Server error result for authorization
    /// </summary>
    public class AccelaOAuthErrorResult
    {
        /// <summary>
        /// A single error code, for more error code,
        /// please refer to http://tools.ietf.org/html/rfc6749#section-5.2 
        /// </summary>
        public string error { get;  set; }

        /// <summary>
        /// A human-readable UTF-8 encoded text providing additional information, 
        /// used to assist the client developer in understanding the error that occurred
        /// </summary>
        public string error_description { get;  set; }

        /// <summary>
        /// A URI identifying a human-readable web page with information about the error, 
        /// used to provide the client developer with additional information about the error.
        /// </summary>
        public string error_uri { get;  set; }

        /// <summary>
        /// REQUIRED if a "state" parameter was present in the client authorization request.  
        /// The exact value received from the client.
        /// </summary>
        public string state { get;  set; }
    }
}
