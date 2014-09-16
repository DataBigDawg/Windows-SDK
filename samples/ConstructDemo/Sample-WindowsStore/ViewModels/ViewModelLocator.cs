#if WINDOWS_PHONE
using Accela.WindowsPhone8.Sample.Services;
#else
using Accela.WindowsStore.Sample.Services;
#endif
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.ViewModels
#else
namespace Accela.WindowsStore.Sample.ViewModels
#endif
{    /// <summary>
    /// Specify which view modle need to be loaded
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IDisplayMessageService, DisplayMessageService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CitizenViewModel>();
            SimpleIoc.Default.Register<AgencyViewModel>();

        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public CitizenViewModel Citizen
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CitizenViewModel>();
            }
        }

        public AgencyViewModel Agency
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AgencyViewModel>();
            }
        }
    }
}
