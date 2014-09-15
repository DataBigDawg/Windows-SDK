using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.Portable;
using RecordsViewer.ViewModels;
using Accela.WindowsStoreSDK;
using RecordsViewer.Portable.Resources;

namespace RecordsViewer
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Accela SDK initialization
            InitializeSDK();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(Strings.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), Strings.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        #region AccelaSDK

        /// <summary>
        /// Provides easy access to the Accela SDK of the Phone Application.
        /// </summary>
        public static AccelaSDK SharedSDK { get; private set; }

        public static string[] ApiPermissions = { "records" };

        private void InitializeSDK()
        {
            string appid = "com.accela.inspector";
            string appsecret = "839ffae0ee244cb4b558f252ca84524c";
            SharedSDK = new AccelaSDK(appid, appsecret);
            SharedSDK.ApiHost = "apps-apis.dev.accela.com";
            SharedSDK.OAuthHost = "apps-auth.dev.accela.com";
            SharedSDK.SessionChanged += SharedSDK_SessionChanged;
        }

        void SharedSDK_SessionChanged(object sender, AccelaSessionEventArgs e)
        {
            switch (e.SessionStatus)
            {
                case AccelaSessionStatus.InvalidSession:
                    // todo: invalid session handle
                    break;
                case AccelaSessionStatus.LoginCancelled:
                    // todo: login cancelled handle
                    break;
                case AccelaSessionStatus.LoginFailed:
                    // todo: login failed handle
                    break;
                case AccelaSessionStatus.LoginSucceeded:
                    // todo: login succeeded handle
                    break;
                case AccelaSessionStatus.LogoutSucceeded:
                    // todo: logout succeeded handle
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Service

        private static ILoginService _loginService;
        public static ILoginService LoginService
        {
            get
            {
                lock (typeof(App))
                {
                    if (_loginService == null)
                    {
                        _loginService = new LoginService();
                    }
                }

                return _loginService;
            }
        }

        private static IRecordService _recordService;
        public static IRecordService RecordService
        {
            get
            {
                lock (typeof(App))
                {
                    if (_recordService == null)
                    {
                        _recordService = new RecordService();
                    }
                }
                return _recordService;
            }
        }

        #endregion

        #region ViewModel

        private static RecordsViewModel _recordsViewModel;
        public static RecordsViewModel RecordsViewModel
        {
            get
            {
                lock (typeof(App))
                {
                    if (_recordsViewModel == null)
                    {
                        _recordsViewModel = new RecordsViewModel(RecordService);
                    }
                }
                return _recordsViewModel;
            }
        }

        private static LoginViewModel _loginViewModel;
        public static LoginViewModel LoginViewModel
        {
            get
            {
                lock (typeof(App))
                {
                    if (_loginViewModel == null)
                    {
                        _loginViewModel = new LoginViewModel(LoginService);
                    }
                }
                return _loginViewModel;
            }
        }

        private static SettingsViewModel _settingsViewModel;
        public static SettingsViewModel SettingsViewModel
        {
            get
            {
                lock (typeof(App))
                {
                    if (_settingsViewModel == null)
                    {
                        _settingsViewModel = new SettingsViewModel();
                    }
                }
                return _settingsViewModel;
            }
        }

        #endregion
    }
}