using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace RecordsViewer
{
    public sealed partial class CalloutControl : UserControl
    {
        private string _recordID;

        public CalloutControl()
        {
            this.InitializeComponent();
        }

        public CalloutControl(string recordID, string address)
        {
            this.InitializeComponent();
            _recordID = recordID;

            if (!string.IsNullOrEmpty(address))
            {
                this.txtAddress.Text = address;
            }
        }

        private void btnShowInspectionDetail_Click_1(object sender, RoutedEventArgs e)
        {
            Frame rootFram = Window.Current.Content as Frame;
            RecordListPage rootPage = rootFram.Content as RecordListPage;
            rootPage.Frame.Navigate(typeof(RecordDetailPage), _recordID);
        }
    }
}
