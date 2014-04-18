using RecordsViewer.Portable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accela.WindowsStoreSDK;

namespace RecordsViewer.ViewModels
{
    public class LoginService : ILoginService
    {
        public Task LoginAsync(LoginViewModel loginInfo)
        {
            if (string.IsNullOrEmpty(loginInfo.AgencyName))
                throw new ArgumentNullException("Agency");
            if (string.IsNullOrEmpty(loginInfo.UserName))
                throw new ArgumentNullException("UserName");
            if (string.IsNullOrEmpty(loginInfo.PassWord))
                throw new ArgumentNullException("PassWord");
            if (string.IsNullOrEmpty(loginInfo.Environment))
                throw new ArgumentNullException("Environment");

            var environment = (AccelaEnvironment)Enum.Parse(typeof(AccelaEnvironment), loginInfo.Environment);

            return App.SharedSDK.Authorize(loginInfo.UserName,
                                     loginInfo.PassWord,
                                     loginInfo.AgencyName,
                                     App.ApiPermissions,
                                     environment);
        }

        public void Logout()
        {
            App.SharedSDK.Logout();
        }
    }
}
