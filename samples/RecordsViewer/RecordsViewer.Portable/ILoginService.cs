using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Shared component between windows phone and windows store sample.
/// </summary>
namespace RecordsViewer.Portable
{
    /// <summary>
    /// Login service interface
    /// </summary>
    public interface ILoginService
    {
        Task LoginAsync();

        void Logout();
    }
}
