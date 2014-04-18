using Accela.WindowsPhone8.Sample.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;



namespace Accela.WindowsPhone8.Sample.Services
{
    public class NavigationService : INavigationService
    {
        private static readonly Dictionary<Type, string> ViewModelRouting =
            new Dictionary<Type, string> { 
                                            { typeof(AgencyViewModel), "Views/AgencyPage.xaml" }, 
                                            { typeof(CitizenViewModel), "Views/CitizenPage.xaml" } 
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
        private Frame RootFrame
        {
            get { return Application.Current.RootVisual as Frame; }
        }

        /// <summary> 
        /// Decodes the navigation parameter. 
        /// </summary> 
        /// <typeparam name="TJson">The type of the json.</typeparam> 
        /// <param name="context">The context.</param> 
        /// <returns>The json result.</returns> 
        public static TJson DecodeNavigationParameter<TJson>(NavigationContext context)
        {
            if (context.QueryString.ContainsKey("param"))
            {
                var param = context.QueryString["param"];
                return string.IsNullOrWhiteSpace(param) ? default(TJson) : JsonConvert.DeserializeObject<TJson>(param);
            }

            throw new KeyNotFoundException();
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
        /// <typeparam name="TDestinationViewModel">The type of the destination view model.</typeparam> 
        /// <param name="parameter">The parameter.</param> 
        public void Navigate<TDestinationViewModel>(object parameter)
        {
            var navParameter = string.Empty;
            if (parameter != null)
            {
                navParameter = "?param=" + JsonConvert.SerializeObject(parameter);
            }

            if (ViewModelRouting.ContainsKey(typeof(TDestinationViewModel)))
            {
                var page = ViewModelRouting[typeof(TDestinationViewModel)];

                this.RootFrame.Navigate(new Uri("/" + page + navParameter, UriKind.Relative));
            }
        }
    }
}
