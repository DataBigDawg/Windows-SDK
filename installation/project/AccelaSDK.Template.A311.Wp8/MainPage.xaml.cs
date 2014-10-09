using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using $safeprojectname$.Resources;

namespace $safeprojectname$
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            App.ShareSDK.SessionChanged += ShareSDK_SessionChanged;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        void ShareSDK_SessionChanged(object sender, Accela.WindowsStoreSDK.AccelaSessionEventArgs e)
        {
            switch (e.SessionStatus)
            {
                case Accela.WindowsStoreSDK.AccelaSessionStatus.InvalidSession:
                    DisplayMessage("Invalid Session.");
                    break;
                case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginCancelled:
                    DisplayMessage("Login Cancelled.");
                    break;
                case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginFailed:
                    DisplayMessage("Login Failed.");
                    break;
                case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginSucceeded:
                    DisplayMessage("Login Succeeded.");
                    break;
                case Accela.WindowsStoreSDK.AccelaSessionStatus.LogoutSucceeded:
                    DisplayMessage("Logout Succeeded.");
                    break;
                default:
                    DisplayMessage("Invalid Operation.");
                    break;
            }
        }

        void DisplayMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string[] permissions = { };
            try
            {
                App.ShareSDK.Authorize(permissions);
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message);
            }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}