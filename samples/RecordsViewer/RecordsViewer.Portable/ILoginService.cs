using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{
    public interface ILoginService
    {
        Task LoginAsync(LoginViewModel loginInfo);

        void Logout();
    }
}
