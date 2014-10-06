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
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    internal class AccelaSDKLogger
    {
        public static void logInfo(string action, object obj, AccelaLogLevel level)
        {
            string format = @"[{0}, Method: {1}, {2}] " + Environment.NewLine + "{3}";

            string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            string message = string.Empty;

            var dic = obj as IDictionary<string, object>;

            if (dic != null)
            {
                var strBuilder = new StringBuilder();
                foreach (var d in dic)
                {
                    strBuilder.AppendLine(string.Format("{0}: {1}", d.Key, d.Value));
                }
                message = strBuilder.ToString();
            }
            else
                message = obj.ToString();

            if (level <= AccelaSDK.LogLevel && level != AccelaLogLevel.None)
                Debug.WriteLine(format, level, action, time, message);
        }

    }

    /// <summary>
    /// Accela Log Level 
    /// </summary>
    public enum AccelaLogLevel
    {
        /// <summary>
        /// Fatal error or application crash.
        /// </summary>
        Critical = 1,
        /// <summary>
        /// Recoverable error.
        /// </summary>
        Error = 2,
        /// <summary>
        /// Noncritical problem.
        /// </summary>
        Warning = 4,
        /// <summary>
        /// Informational message.
        /// </summary>
        Info = 8,
        /// <summary>
        /// Debugging trace.
        /// </summary>
        Debug = 16,
        /// <summary>
        /// No output log.
        /// </summary>
        None = 32,

    }
}
