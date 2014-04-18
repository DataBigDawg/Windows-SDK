using RecordsViewer.Portable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{
    public class RecordViewModel : NotifyPropertyBase
    {
        private IRecordService _recordService;
        private string _id;
        private string _name;
        private string _typeName;
        private string _discription;
        private string _openDate;
        private string _statusName;

        public string Id
        {
            get { return _id; }
            set { SetProperty<string>(ref _id, value); }
        }
        public string Name
        {
            get { return _name; }
            set { SetProperty<string>(ref _name, value); }
        }
        public string TypeName
        {
            get { return _typeName; }
            set { SetProperty<string>(ref _typeName, value); }
        }
        public string Discription
        {
            get { return _discription; }
            set { SetProperty<string>(ref _discription, value); }
        }
        public string OpenDate
        {
            get { return _openDate; }
            set { SetProperty<string>(ref _openDate, value); }
        }
        public string StatusName
        {
            get { return _statusName; }
            set { SetProperty<string>(ref _statusName, value); }
        }

        public RecordViewModel(IRecordService recordService, string recordId)
        {
            if (recordService == null)
                throw new ArgumentNullException("recordService");

            this._recordService = recordService;
            this.LoadRecord(recordId);
        }

        private void LoadRecord(string recordId)
        {
                var record = _recordService.GetRecordAsync(recordId).Result;
                this.FromWSEntity(record);
        }

        private void FromWSEntity(WSRecord record)
        {
            if (record != null)
            {
                Id = record.Id;
                Name = record.Display;
                TypeName = record.Type != null ? record.Type.Text : string.Empty;
                Discription = record.Description;
                OpenDate = record.OpenedDate;
                StatusName = record.Status != null ? record.Status.Text : string.Empty;
            }
        }
    }
}
