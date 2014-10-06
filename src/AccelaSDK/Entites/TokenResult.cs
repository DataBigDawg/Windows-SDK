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
using System.Runtime.Serialization;
using System.Text;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Server token result for api
    /// </summary>
    internal class AccelaTokenResult: AccelaOAuthErrorResult
    {

        public AccelaTokenResult()
        {
            create_time = DateTime.Now;
        }
        /// <summary>
        /// Access token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// The type of the token issued. 
        /// Value is case insensitive.
        /// It contains fixed value “bearer”.
        /// </summary>
        public string token_type { get { return "bearer"; } }

        /// <summary>
        /// The lifetime in seconds of the access token. 
        /// For example, the value "3600" denotes that the access token will expire in one hour from the time the response was generated.
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime expires_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime create_time { get; set; }

        public void CalculateExpiresTime()
        {
            double expires = 0d;
            if (double.TryParse(expires_in, out expires))
            {
                expires_time = create_time.AddSeconds(expires);
            }
        }

        /// <summary>
        /// Refresh token.
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// Scopes of resource authenticated by Authorization server.
        /// </summary>
        //public string scope { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Access_Token: {0}", access_token));
            sb.AppendLine(string.Format("Refresh_Token: {0}", refresh_token));
            sb.AppendLine(string.Format("Token_Type: {0}", token_type));
            sb.AppendLine(string.Format("Expires_In: {0}", expires_in));
            sb.AppendLine(string.Format("Expires_Time: {0}", expires_time));
            sb.AppendLine(string.Format("Create_Time: {0}", create_time));
            //sb.AppendLine(string.Format("Scope: {0}", scope));
            sb.AppendLine(string.Format("Error: {0}", base.error));
            sb.AppendLine(string.Format("Error_Description: {0}", base.error_description));
            sb.AppendLine(string.Format("Error_Uri: {0}", base.error_uri));
            sb.AppendLine(string.Format("State: {0}", base.state));
            return sb.ToString();
        }
    }
}
