using Accela.WindowsStoreSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace $safeprojectname$
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AccelaSDK _sharedAccelaSDK;

        public MainPage()
        {
            this.InitializeComponent();
            _sharedAccelaSDK = ((App)App.Current).SharedAccelaSDK;
            _sharedAccelaSDK.SessionChanged += _sharedAccelaSDK_SessionChanged;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageDialog msg = null;
            // The Permission Scope is documented at https://developer.dev.accela.com/Resource/ApisAbout
            string[] _permissions = { "get_records" };
            try
            {
                await _sharedAccelaSDK.Authorize(_permissions);
            }
            catch (Exception ex)
            {
                msg = new MessageDialog(ex.Message);
            }
            if (msg != null)
                await msg.ShowAsync();
        }

        void _sharedAccelaSDK_SessionChanged(object sender, AccelaSessionEventArgs e)
        {
            switch (e.SessionStatus)
            {
                case AccelaSessionStatus.LoginSucceeded:
                    lbResult.Text = "Login succeeded.";
                    break;
                case AccelaSessionStatus.InvalidSession:
                    lbResult.Text = "Invalid session.";
                    break;
                case AccelaSessionStatus.LoginCancelled:
                    lbResult.Text = "Login cancelled.";
                    break;
                case AccelaSessionStatus.LoginFailed:
                    lbResult.Text = "Login failed.";
                    break;
                case AccelaSessionStatus.LogoutSucceeded:
                    lbResult.Text = "Logout succeeded.";
                    break;
                default:
                    break;
            }
        }
    }
}
