using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{
    public class LoginViewModel : NotifyPropertyBase
    {
        private ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            if (loginService == null)
                throw new ArgumentNullException("loginService");
            _loginService = loginService;
            InitLoginData();
        }

        private void InitLoginData()
        {
            EnvList = new ObservableCollection<string>();
            EnvList.Add("PROD");
            EnvList.Add("TEST");
            EnvList.Add("DEV");
            EnvList.Add("STAGE");
            EnvList.Add("CONFIG");
            EnvList.Add("SUPP");


            this._agencyName = "BPTDEV";
            this._userName = "admin";
            this._passWord = "admin";
            this._environment = "DEV";
        }

        private string _userName;
        private string _passWord;
        private string _agencyName;
        private string _environment;

        public string UserName
        {
            get { return this._userName; }
            set { SetProperty<string>(ref this._userName, value); }
        }
        public string PassWord
        {
            get { return this._passWord; }
            set { SetProperty<string>(ref this._passWord, value); }
        }
        public string AgencyName
        {
            get { return this._agencyName; }
            set { SetProperty<string>(ref this._agencyName, value); }
        }
        public string Environment
        {
            get { return this._environment; }
            set { SetProperty<string>(ref this._environment, value); }
        }
        public ObservableCollection<string> EnvList { get; private set; }

        public Task AuthorizationAsync()
        {
            return _loginService.LoginAsync(this);
        }

        public void Logout()
        {
            _loginService.Logout();
        }
    }
}
