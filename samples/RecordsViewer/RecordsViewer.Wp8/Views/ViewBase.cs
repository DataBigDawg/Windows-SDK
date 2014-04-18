using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecordsViewer.Views
{
    public class ViewBase : PhoneApplicationPage
    {
        private ProgressIndicator _progressIndicator;
        protected MessageBoxResult ShowMessage(string message, string title, MessageBoxButton button)
        {
            return MessageBox.Show(message, title, button);
        }

        protected void ShowSystemTrayBar(string message)
        {
            if (_progressIndicator == null)
            {
                _progressIndicator = new ProgressIndicator()
                {
                    IsIndeterminate = true,
                };
                SystemTray.ProgressIndicator = _progressIndicator;
            }
            _progressIndicator.Text = message;
            _progressIndicator.IsVisible = true;
        }

        protected void ShowSystemTrayLoadingBar()
        {
            ShowSystemTrayBar("Loading...");
        }

        protected void HideSystemTrayBar()
        {
            if (_progressIndicator != null)
                _progressIndicator.IsVisible = false;
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (!NavigationService.CanGoBack)
            {
                var msgResult = ShowMessage(Strings.Exit_Message, Strings.Exit_Title, MessageBoxButton.OKCancel);
                if (msgResult == MessageBoxResult.Cancel)
                    e.Cancel = true;
                else
                    App.LoginViewModel.Logout();
            }
        }
    }
}
