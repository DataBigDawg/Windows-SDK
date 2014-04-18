using System;
using System.Runtime.Serialization;
using System.Text;

namespace Accela.WindowsStoreSDK
{
    internal class TokenInfo
    {


        /// <summary>
        /// Access token
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// The type of the token issued. 
        /// Value is case insensitive.
        /// It contains fixed value “bearer”.
        /// </summary>
        public string Token_Type { get { return "bearer"; } }
        /// <summary>
        /// The lifetime in seconds of the access token. 
        /// For example, the value "3600" denotes that the access token will expire in one hour from the time the response was generated.
        /// </summary>
        public int Expires_In { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Expires_Time { get; set; }
        /// <summary>
        /// Refresh token.
        /// </summary>
        public string Refresh_Token { get; set; }
        /// <summary>
        /// Scopes of resource authenticated by Authorization server.
        /// </summary>
        public string Scope { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Access_Token: {0}", Access_Token));
            sb.AppendLine(string.Format("Token_Type: {0}", Token_Type));
            sb.AppendLine(string.Format("Expires_In: {0}", Expires_In));
            sb.AppendLine(string.Format("Expires_Time: {0}", Expires_Time));
            sb.AppendLine(string.Format("Refresh_Token: {0}", Refresh_Token));
            sb.AppendLine(string.Format("Scope: {0}", Scope));
            return sb.ToString();
        }
    }
}
