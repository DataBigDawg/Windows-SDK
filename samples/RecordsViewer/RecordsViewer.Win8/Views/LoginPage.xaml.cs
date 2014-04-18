using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using RecordsViewer.Portable;
using RecordsViewer.Portable.Resources;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace RecordsViewer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : RecordsViewer.Common.LayoutAwarePage
    {
        private LoginViewModel _loginViewModel;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _loginViewModel = new LoginViewModel(App.LoginService);
            this.DataContext = _loginViewModel;
        }

        private async void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            MessageDialog md = null;
            progressBar.IsIndeterminate = true;
            btnLogin.IsEnabled = false;
            try
            {
                await _loginViewModel.AuthorizationAsync();
                var rootFrame = new Frame();
                Window.Current.Content = rootFrame;
                rootFrame.Navigate(typeof(RecordListPage));
            }
            catch (ArgumentNullException)
            {
                md = new MessageDialog(Strings.Login_Field_Empty_Message, Strings.Login_Error_Message_Title);
            }
            catch (Exception ex)
            {
                var innerErr = ex.InnerException;
                var msg = innerErr == null ? ex.Message : innerErr.Message;
                md = new MessageDialog(msg, Strings.Login_Error_Message_Title);
            }
            finally
            {
                progressBar.IsIndeterminate = false;
                btnLogin.IsEnabled = true;
            }

            if (md != null)
                await md.ShowAsync();

        }
    }
}
