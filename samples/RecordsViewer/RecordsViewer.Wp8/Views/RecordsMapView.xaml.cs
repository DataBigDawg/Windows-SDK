using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RecordsViewer.ViewModels;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using System.Collections.ObjectModel;
using System.Reflection;
using RecordsViewer.Portable.Resources;
using RecordsViewer.Entities;

namespace RecordsViewer.Views
{
    public partial class RecordsMapView : ViewBase
    {
        private readonly RecordsMapViewModel _viewModel;

        private readonly double userLocationMarkerZoomLevel = 16;

        private bool loaded = false;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var barIcoMe = (ApplicationBarIconButton)this.ApplicationBar.Buttons[2];
            barIcoMe.IsEnabled = App.SettingsViewModel.CanUseLocationConsent;
        }

        public RecordsMapView()
        {
            InitializeComponent();

            this.MapExtensionsSetup(this.Map);

            this._viewModel = new RecordsMapViewModel(App.RecordsViewModel.Records);

            this.DataContext = this._viewModel;

            this.RecordsMapItemsControl.ItemsSource = this._viewModel.Coordinates;

            this.MultiLanguage_ApplicationBar();

            this.Dispatcher.BeginInvoke(new Action(this.MapFlightToRecords));
        }

        #region helper

        private void MultiLanguage_ApplicationBar()
        {
            foreach (ApplicationBarIconButton button in this.ApplicationBar.Buttons)
            {
                var text = button.Text.ToLower();
                switch (text)
                {
                    case "list":
                        text = Strings.Map_List;
                        break;
                    case "reload":
                        text = Strings.Map_Reload;
                        break;
                    case "me":
                        text = Strings.Map_Me;
                        break;
                }
                button.Text = text;
            }

            foreach (ApplicationBarMenuItem item in this.ApplicationBar.MenuItems)
            {
                var text = item.Text.ToLower();
                switch (text)
                {
                    case "settings":
                        text = Strings.Map_Settings;
                        break;
                }
                item.Text = text;
            }
        }

        private void MapExtensionsSetup(Map map)
        {
            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(map);
            var runtimeFields = this.GetType().GetRuntimeFields();

            foreach (DependencyObject i in children)
            {
                var info = i.GetType().GetProperty("Name");

                if (info != null)
                {
                    string name = (string)info.GetValue(i);

                    if (name != null)
                    {
                        foreach (FieldInfo j in runtimeFields)
                        {
                            if (j.Name == name)
                            {
                                j.SetValue(this, i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MapFlightToRecords()
        {
            LocationRectangle locationRectangle;
            var locations = from record in this._viewModel.Coordinates select record.Coordinate;
            if (!locations.Any())
                return;
            locationRectangle = LocationRectangle.CreateBoundingRectangle(locations);

            this.Map.SetView(locationRectangle, new Thickness(20, 20, 20, 20));
        }

        private async Task ShowUserLocation()
        {
            Geolocator geolocator;
            Geoposition geoposition;

            this.UserLocationMarker = (UserLocationMarker)this.FindName("UserLocationMarker");

            geolocator = new Geolocator();

            geoposition = await geolocator.GetGeopositionAsync();

            this.UserLocationMarker.GeoCoordinate = geoposition.Coordinate.ToGeoCoordinate();
            this.UserLocationMarker.Visibility = System.Windows.Visibility.Visible;
        }

        private async Task ReloadRecords()
        {
            try
            {
                ShowSystemTrayBar("Reloading...");
                await App.RecordsViewModel.LoadRecordsAsync();

                this._viewModel.InitViewModel(App.RecordsViewModel.Records);

                this.Dispatcher.BeginInvoke(new Action(this.MapFlightToRecords));
            }
            catch (AggregateException ex)
            {
                var innerErr = ex.InnerExceptions[0];
                ShowMessage(innerErr.Message, Strings.Records_Error_Message_Title, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, Strings.Records_Error_Message_Title, MessageBoxButton.OK);
            }
            finally
            {
                HideSystemTrayBar();
            }

        }

        #endregion

        #region ApplicationBar

        private void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.Mode = ApplicationBarMode.Default;
            this.ApplicationBar.Opacity = 1.0;

            var listIconBtn = new ApplicationBarIconButton()
            {
                Text = Strings.Map_List,
                IconUri = new Uri("/Assets/Icon/Bullets.png", UriKind.Relative)
            };
            listIconBtn.Click += listIconBtn_Click;

            var reloadIconBtn = new ApplicationBarIconButton()
            {
                Text = Strings.Records_Map_Refresh,
                IconUri = new Uri("/Assets/Icon/Reload.png", UriKind.Relative)
            };
            reloadIconBtn.Click += reloadIconBtn_Click;

            var meIconBtn = new ApplicationBarIconButton()
            {
                Text = Strings.Map_Me,
                IconUri = new Uri("/Assets/Icon/Me.png", UriKind.Relative)
            };
            meIconBtn.Click += meIconBtn_Click;

            this.ApplicationBar.Buttons.Add(listIconBtn);
            this.ApplicationBar.Buttons.Add(reloadIconBtn);
            this.ApplicationBar.Buttons.Add(meIconBtn);
        }

        async void meIconBtn_Click(object sender, EventArgs e)
        {
            await this.ShowUserLocation();

            this.Map.SetView(this.UserLocationMarker.GeoCoordinate, this.userLocationMarkerZoomLevel);

        }

        async void reloadIconBtn_Click(object sender, EventArgs e)
        {
            await this.ReloadRecords();
            this.MapFlightToRecords();
        }

        void listIconBtn_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        #endregion


        private void MyPushpin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                var txt = (TextBlock)e.OriginalSource;
                var recordid = txt.Text;

                string relativeUri = string.Format("/Views/RecordView.xaml?id={0}", recordid);

                NavigationService.Navigate(new Uri(relativeUri, UriKind.Relative));
            }
        }
    }
}