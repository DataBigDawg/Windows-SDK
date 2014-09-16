using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.Portable;

namespace RecordsViewer.Views
{   /// <summary>
    /// Provides view of record detail
    /// </summary>
    public partial class RecordView : ViewBase
    {
        public RecordView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string recordId;

            NavigationContext.QueryString.TryGetValue("id", out recordId);

            this.DataContext = new RecordViewModel(App.RecordService, recordId);
        }

    }
}