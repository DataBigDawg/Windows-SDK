using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.Portable.Resources;
using System.Windows.Controls.Primitives;

namespace RecordsViewer.Views
{
    public partial class RecordsView : ViewBase
    {
        bool loaded = false;

        public RecordsView()
        {
            InitializeComponent();
            BuildApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (loaded)
                return;

            LoadRecords();
        }

        public async void LoadRecords()
        {
            try
            {
                ShowSystemTrayLoadingBar();
                await App.RecordsViewModel.LoadRecordsAsync();
                this.DataContext = App.RecordsViewModel;
                loaded = true;
            }
            catch (AggregateException ex)
            {
                var innerErr = ex.InnerExceptions[0];
                ShowMessage(innerErr.Message, Strings.Records_Error_Message_Title, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Strings.Records_Error_Message_Title, MessageBoxButton.OK);
            }
            finally
            {
                HideSystemTrayBar();
            }
        }

        #region ApplicationBar
        private void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.Mode = ApplicationBarMode.Default;
            this.ApplicationBar.Opacity = 1.0;

            var mapIconBtn = new ApplicationBarIconButton()
            {
                Text = Strings.Records_Map,
                IconUri = new Uri("/Assets/Icon/Map-Location.png", UriKind.Relative)
            };
            mapIconBtn.Click += mapIconBtn_Click;

            var refreshIconBtn = new ApplicationBarIconButton()
            {
                Text = Strings.Records_Map_Refresh,
                IconUri = new Uri("/Assets/Icon/Reload.png", UriKind.Relative)
            };
            refreshIconBtn.Click += refreshIconBtn_Click;

            var settingsMenuItem = new ApplicationBarMenuItem()
            {
                Text = Strings.Map_Settings
            };
            settingsMenuItem.Click += settingsMenuItem_Click;

            var logoutMenuItem = new ApplicationBarMenuItem()
            {
                Text = Strings.Login_SignOut
            };
            logoutMenuItem.Click += logoutMenuItem_Click;


            this.ApplicationBar.Buttons.Insert(0, refreshIconBtn);
            this.ApplicationBar.Buttons.Insert(1, mapIconBtn);
            this.ApplicationBar.MenuItems.Insert(0, settingsMenuItem);
            this.ApplicationBar.MenuItems.Insert(1, logoutMenuItem);
        }

        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.Relative));
        }

        void logoutMenuItem_Click(object sender, EventArgs e)
        {
            App.LoginViewModel.Logout();
            NavigationService.Navigate(new Uri("/Views/LoginView.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }

        void refreshIconBtn_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        void mapIconBtn_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/RecordsMapView.xaml", UriKind.Relative));
        }
        #endregion
    }
}