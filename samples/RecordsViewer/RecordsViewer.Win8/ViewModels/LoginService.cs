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
        public void Logout()
        {
            App.SharedSDK.Logout();
        }

        public Task LoginAsync()
        {
            return App.SharedSDK.Authorize(App.ApiPermissions);
        }
    }
}
