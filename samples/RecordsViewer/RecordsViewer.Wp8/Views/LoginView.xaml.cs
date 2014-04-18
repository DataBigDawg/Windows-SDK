using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.Portable;
using RecordsViewer.Portable.Resources;

namespace RecordsViewer.Views
{
    public partial class LoginView : ViewBase
    {
        public LoginView()
        {
            InitializeComponent();

            this.Loaded += LoginView_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.DataContext = App.LoginViewModel;

        }

        void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            App.SettingsViewModel.CheckLocationConsent();
        }

        private async void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowSystemTrayBar("Logining in...");
                btnLogin.IsEnabled = false;
                await App.LoginViewModel.AuthorizationAsync();
                NavigationService.Navigate(new Uri("/Views/RecordsView.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            catch (ArgumentNullException)
            {
                ShowMessage(Strings.Login_Field_Empty_Message, Strings.Login_Error_Message_Title, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                var innerErr = ex.InnerException;
                var msg = innerErr == null ? ex.Message : innerErr.Message;
                ShowMessage(msg, Strings.Login_Error_Message_Title, MessageBoxButton.OK);
            }
            finally
            {
                HideSystemTrayBar();
                btnLogin.IsEnabled = true;
            }
        }
    }
}