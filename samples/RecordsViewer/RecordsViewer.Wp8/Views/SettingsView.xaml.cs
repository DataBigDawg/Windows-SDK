using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace RecordsViewer.Views
{   /// <summary>
    /// Provides view of settings
    /// </summary>
    public partial class SettingsView : PhoneApplicationPage
    {
        public SettingsView()
        {
            InitializeComponent();

            this.DataContext = App.SettingsViewModel;
        }

    }
}