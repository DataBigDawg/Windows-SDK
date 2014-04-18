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
using Windows.UI.Popups;
using Bing.Maps;
using RecordsViewer.Portable;
using RecordsViewer.Portable.Entities;
using System.Threading.Tasks;
using RecordsViewer.Portable.Resources;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace RecordsViewer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class RecordListPage : RecordsViewer.Common.LayoutAwarePage
    {
        bool loaded = false;

        public RecordListPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (loaded)
                return;
            MessageDialog md = null;
            try
            {
                progressRing.IsActive = true;
                if (!App.RecordsViewModel.Records.Any())
                    await App.RecordsViewModel.LoadRecordsAsync();
                this.DataContext = App.RecordsViewModel;
                loaded = true;
                AddPushPins();
            }
            catch (AggregateException ex)
            {
                var innerErr = ex.InnerExceptions[0];
                md = new MessageDialog(innerErr.Message, Strings.Records_Error_Message_Title);
            }
            catch (Exception ex)
            {
                md = new MessageDialog(ex.Message, Strings.Records_Error_Message_Title);
            }
            finally
            {
                progressRing.IsActive = false;
            }

            if (md != null)
                await md.ShowAsync();
        }

        private void JobList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            WSRecord curRecord = (sender as ListView).SelectedItem as WSRecord;

            if (curRecord.Addresses != null && curRecord.Addresses.Count > 0)
            {
                var address = curRecord.Addresses[0];
                var location = new Location(Double.Parse(address.YCoordinate), Double.Parse(address.XCoordinate));

                this.bingMapControl.SetView(location);

                CalloutControl callout = new CalloutControl(curRecord.Id, curRecord.Id);
                MapLayer.SetPosition(callout, location);
                bingMapControl.Children.Add(callout);
            }
            else
            {
                this.Frame.Navigate(typeof(RecordDetailPage), curRecord.Id);
            }
        }

        #region Map

        private async void AddPushPins()
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    bingMapControl.Children.Clear();
                });

            var locCollection = new LocationCollection();
            var records = App.RecordsViewModel.Records;

            foreach (var r in records)
            {
                if (r.Addresses == null || r.Addresses.Count == 0)
                    continue;

                WSAddress address = r.Addresses[0];

                if (string.IsNullOrEmpty(address.YCoordinate) ||
                    string.IsNullOrEmpty(address.XCoordinate))
                {
                    continue;
                }

                var pushPin = new Pushpin();
                pushPin.Tapped += pushPin_Tapped;
                pushPin.Tag = r.Id;
                var latitude = Double.Parse(address.YCoordinate);
                var longitude = Double.Parse(address.XCoordinate);
                if (-180.0 > longitude || longitude > 180.0)
                    continue;
                if (longitude > 90.0)
                    continue;
                var location = new Location(latitude, longitude);

                MapLayer.SetPosition(pushPin, location);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        bingMapControl.Children.Add(pushPin);
                    });
                locCollection.Add(location);
            }

            if (locCollection.Count() > 0)
            {
                LocationRect rect = new LocationRect(locCollection);
                // zoom out the map to show all pins in the view
                rect.Height += (0.1 * rect.Height);
                rect.Width += (0.1 * rect.Width);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        bingMapControl.SetView(rect);
                    });
            }
        }

        void pushPin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ResetCallout();

            Pushpin pp = sender as Pushpin;
            var record = App.RecordsViewModel.Records.FirstOrDefault(c => c.Id.Equals(pp.Tag));
            var address = record.Addresses[0];

            var location = new Location(Double.Parse(address.YCoordinate), Double.Parse(address.XCoordinate));

            this.bingMapControl.SetView(location);

            var callout = new CalloutControl(record.Id, record.Id);

            MapLayer.SetPosition(callout, location);

            bingMapControl.Children.Add(callout);
        }

        private void ResetCallout()
        {
            for (int i = 0; i < this.bingMapControl.Children.Count; i++)
            {
                if (this.bingMapControl.Children[i] is CalloutControl)
                {
                    this.bingMapControl.Children.RemoveAt(i);
                }
            }
        }


        #endregion
    }
}
