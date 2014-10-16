#if WINDOWS_PHONE || WINDOWS_PHONE_APP
using Accela.WindowsPhone.Sample.ViewModels;
using Accela.WindowsPhone.Sample.Views;
#else
using Accela.WindowsStore.Sample.ViewModels;
using Accela.WindowsStore.Sample.Views;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP
namespace Accela.WindowsPhone.Sample.Services
#else
namespace Accela.WindowsStore.Sample.Services
#endif
{
    public class NavigationService : INavigationService
    {
        private static readonly IDictionary<Type, Type> ViewModelRouting =
            new Dictionary<Type, Type>() { 
                                            { typeof(AgencyViewModel), typeof(AgencyPage) }, 
                                            { typeof(CitizenViewModel), typeof(CitizenPage) } 
                                        };

        /// <summary> 
        /// Gets a value indicating whether can go back. 
        /// </summary> 
        public bool CanGoBack
        {
            get
            {
                return RootFrame.CanGoBack;
            }
        }

        /// <summary> 
        /// Gets the root frame. 
        /// </summary> 
        private static Frame RootFrame
        {
            get { return Window.Current.Content as Frame; }
        }

        /// <summary> 
        /// The go back. 
        /// </summary> 
        public void GoBack()
        {
            RootFrame.GoBack();
        }

        /// <summary> 
        /// Navigates the specified parameter. 
        /// </summary> 
        /// <typeparam name="TDestinationViewModel"> 
        /// The type of the destination view model. 
        /// </typeparam> 
        /// <param name="parameter"> 
        /// The parameter. 
        /// </param> 
        public void Navigate<TDestinationViewModel>(object parameter)
        {
            var dest = ViewModelRouting[typeof(TDestinationViewModel)];

            RootFrame.Navigate(dest, parameter);
        }
    }
}
