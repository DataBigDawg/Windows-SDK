using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{   /// <summary>
    /// Model class integrated with login service on authorization and logout
    /// </summary>
    public class LoginViewModel : NotifyPropertyBase
    {
        private ILoginService _loginService;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public Task SSOAuthorization() {
            return _loginService.LoginAsync();
        }

        public void Logout()
        {
            _loginService.Logout();
        }
    }
}
