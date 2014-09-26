using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Accela.WindowsPhone8.Sample.Services
{   /// <summary>
    /// Show API response message
    /// </summary>
    public class DisplayMessageService : IDisplayMessageService
    {
        public void Show(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public void Show(string message)
        {
            MessageBox.Show(message);
        }
    }
}
