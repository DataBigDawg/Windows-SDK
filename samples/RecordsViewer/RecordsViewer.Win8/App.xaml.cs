using RecordsViewer.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Accela.WindowsStoreSDK;
using RecordsViewer.Portable.Entities;
using RecordsViewer.Portable;
using RecordsViewer.ViewModels;

// The Split App template is documented at http://go.microsoft.com/fwlink/?LinkId=234228

namespace RecordsViewer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {

        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            InitializeSDK();

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(LoginPage)))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }


        #region AccelaSDK

        public static AccelaSDK SharedSDK { get; private set; }

        public static string[] ApiPermissions = { "records" };

        private void InitializeSDK()
        {
            string appid = "635331259526036859";
            string appsecret = "e26cca2018b44bb6836be0a04d719688";
            SharedSDK = new AccelaSDK(appid, appsecret);
            SharedSDK.ApiHost = "apps-apis.dev.accela.com";
            SharedSDK.OAuthHost = "apps-auth.dev.accela.com";
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

        #endregion
    }
}
