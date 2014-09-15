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

        private bool isLogining = false;

        async void LoginView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLogining)
            {
                isLogining = true;
                App.SettingsViewModel.CheckLocationConsent();
                await App.LoginService.LoginAsync();
                isLogining = false;
                 if (App.SharedSDK.IsSessionValid()) {
                      NavigationService.Navigate(new Uri("/Views/RecordsView.xaml", UriKind.Relative));
                 }
            }
        }



        private async void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            await App.LoginService.LoginAsync();
        }
    }
}