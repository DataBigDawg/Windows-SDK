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
{   /// <summary>
    /// RecordsMapViewModel set/get records Coordinate, initialize coordinates from records.
    /// </summary>
    public class RecordsMapViewModel : NotifyPropertyBase
    {   /// <summary>
        /// Construct RecordsMapViewModel initialize records' coordinates
        /// </summary>
        /// <param name="records">List of records.</param>
        /// <returns></returns>
        public RecordsMapViewModel(IList<WSRecord> records)
        {
            this.InitViewModel(records);
        }

        private ObservableCollection<RecordCoordinate> _coordinates;

        public ObservableCollection<RecordCoordinate> Coordinates
        {
            get { return _coordinates; }
            set { SetProperty<ObservableCollection<RecordCoordinate>>(ref _coordinates, value); }
        }
        /// <summary>
        /// InitViewModel initialize records' coordinates
        /// </summary>
        /// <param name="records">List of records.</param>
        /// <returns></returns>
        public void InitViewModel(IList<WSRecord> records)
        {
            _coordinates = new ObservableCollection<RecordCoordinate>();

            if (records != null && records.Any())
            {
                foreach (var r in records)
                {
                    var recordId = r.Id;
                    var address = r.Addresses != null ? r.Addresses.FirstOrDefault() : null;

                    if (address != null && address != null)
                    {
                        if (string.IsNullOrEmpty(address.XCoordinate) ||
                            string.IsNullOrEmpty(address.YCoordinate))
                            continue;
                        var latitude = Double.Parse(address.YCoordinate);
                        var longitude = Double.Parse(address.XCoordinate);
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
