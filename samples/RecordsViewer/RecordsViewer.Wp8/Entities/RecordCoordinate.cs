using Microsoft.Phone.Maps.Controls;
using RecordsViewer.Portable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Entities
{    /// <summary>
    /// A module class for record's baseic infomation and coordinate
    /// </summary>
    public class RecordCoordinate : NotifyPropertyBase
    {
        private GeoCoordinate _coordinate;
        private string _recordName;
        private string _recoidId;


        [TypeConverter(typeof(GeoCoordinateConverter))]
        public GeoCoordinate Coordinate
        {
            get { return _coordinate; }
            set { SetProperty<GeoCoordinate>(ref _coordinate, value); }
        }

        public string RecordId
        {
            get { return _recoidId; }
            set { SetProperty<string>(ref _recoidId, value); }
        }

        public override string ToString()
        {
            return _recordName;
        }
    }
}
