using RecordsViewer.Portable;
using RecordsViewer.Portable.Resources;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecordsViewer.ViewModels
{
    public class SettingsViewModel : NotifyPropertyBase
    {
        private const String LOCATION_CONSENT = "LocationConsent";

        private bool _canUseLocationConsent;

        public bool CanUseLocationConsent
        {
            get { return _canUseLocationConsent; }
            set { SetProperty<bool>(ref _canUseLocationConsent, value); }
        }

        public void SaveLocationConsent(bool status)
        {
            IsolatedStorageSettings.ApplicationSettings[LOCATION_CONSENT] = status;
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public bool CheckLocationConsent()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(LOCATION_CONSENT))
            {
                MessageBoxResult result =
                    MessageBox.Show(Strings.Map_LocationConsent_Message,
                    Strings.Map_LocationConsent_Title,
                    MessageBoxButton.OKCancel);

                CanUseLocationConsent = (result == MessageBoxResult.OK);

                SaveLocationConsent(CanUseLocationConsent);
            }
            else
            {
                CanUseLocationConsent = (bool)IsolatedStorageSettings.ApplicationSettings[LOCATION_CONSENT];
            }


            return CanUseLocationConsent;

        }

    }
}
