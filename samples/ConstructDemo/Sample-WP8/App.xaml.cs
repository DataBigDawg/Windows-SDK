using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Accela.WindowsPhone.Sample.Resources;
using Accela.WindowsStoreSDK;
using Newtonsoft.Json;

namespace Accela.WindowsPhone.Sample
{
    public partial class App : Application
    {
        /// <summary>
        ///Give access to the root frame for the phone
        /// </summary>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application construct method
        /// </summary>
        public App()
        {
            
            UnhandledException += Application_UnhandledException;

            // XAML initialization
            InitializeComponent();

            InitializePhoneApplication();

            InitializeLanguage();

            // Graphic analysis information in debug mode.
            if (Debugger.IsAttached)
            {
                // Displays the current frame rate
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                //Application.Current.Host.Settings.EnableRedrawRegions = true；

                //Application.Current.Host.Settings.EnableCacheVisualization = true；

                // Disbale mode to make the screen on in debug mode
                //  Notice: Only used in debug mode since it will keep running and consume battery
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }


        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }


        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        #region PhoneApplicationInitialization

        // avoid duplicate initialization
        private bool phoneApplicationInitialized = false;

        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            RootFrame.Navigated += CheckForResetNavigation;
            phoneApplicationInitialized = true;
        }

        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            RootFrame.Navigated -= ClearBackStackAfterReset;
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;
            while (RootFrame.RemoveBackEntry() != null)
            {
                ;
            }
        }

        #endregion

        //Initialization of the localizaed resouces
        private void InitializeLanguage()
        {
            try
            {
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}