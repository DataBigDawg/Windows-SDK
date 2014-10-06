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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accela.WindowsStoreSDK
{
    /// <summary>
    /// Model to store and retrive user information
    /// </summary>
    internal class NativeUserInfo : BindableBase
    {
        private string _agency;

        private string _username;

        private string _password;

        private string _environment;


        private ObservableCollection<string> _env = new ObservableCollection<string>() { 
            "PROD",
            "TEST"
        };

        public ObservableCollection<string> EnvList
        {
            get { return _env; }
        }

        public string Agency
        {
            get { return _agency; }
            set { SetProperty(ref _agency, value); }
        }

        public string UserName
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public string Enviroment
        {
            get { return _environment; }
            set { SetProperty(ref _environment, value); }
        }
        /// <summary>
        /// construct method to init the default agency/username/environment 
        /// </summary>
        public NativeUserInfo()
        {
#if DEBUG
            this._agency = "islandton";
            this._password = "accela";
            this._username = "mdeveloper";
            this._environment = this._env[0];
#endif
        }
    }
}
