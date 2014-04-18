using RecordsViewer.Entities;
using RecordsViewer.Portable;
using RecordsViewer.Portable.Entities;
using RecordsViewer.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecordsViewer.ViewModels
{
    public class RecordsMapViewModel : NotifyPropertyBase
    {
        public RecordsMapViewModel(IDictionary<string, WSRecordLocation> recordLocations)
        {
            this.InitViewModel(recordLocations);
        }

        private ObservableCollection<RecordCoordinate> _coordinates;

        public ObservableCollection<RecordCoordinate> Coordinates
        {
            get { return _coordinates; }
            set { SetProperty<ObservableCollection<RecordCoordinate>>(ref _coordinates, value); }
        }

        public void InitViewModel(IDictionary<string, WSRecordLocation> recordLocations)
        {
            _coordinates = new ObservableCollection<RecordCoordinate>();

            if (recordLocations != null && recordLocations.Any())
            {
                foreach (var r in recordLocations)
                {
                    var recordId = r.Key;
                    var location = r.Value;

                    if (location != null && location.GeometryPoint != null)
                    {
                        var address = location.GeometryPoint;
                        if (string.IsNullOrEmpty(address.X) ||
                            string.IsNullOrEmpty(address.Y))
                            continue;
                        var latitude = Double.Parse(address.Y);
                        var longitude = Double.Parse(address.X);
                        if (-180.0 > longitude || longitude > 180.0)
                            continue;
                        if (longitude > 90.0)
                            continue;
                        var coordinate = new RecordCoordinate
                        {
                            Coordinate = new GeoCoordinate(latitude, longitude),
                            RecordId = recordId
                        };

                        _coordinates.Add(coordinate);
                    }
                }
            }
        }


    }
}
