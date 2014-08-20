﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordsViewer.Portable.Entities;

namespace RecordsViewer.Portable
{
    public class RecordsViewModel : NotifyPropertyBase
    {
        private IRecordService _recordService;

        public ObservableCollection<WSRecord> Records { get; private set; }

        public RecordsViewModel(IRecordService recordService)
        {
            this._recordService = recordService;
            this.Records = new ObservableCollection<WSRecord>();
        }

        public Task LoadRecordsAsync()
        {
            return Task.Run(() =>
            {
                var apiPath = "/v4/records?expand=address";
                try
                {
                    var records = _recordService.GetRecordsAsync(apiPath, null).Result;
                    if (records == null)
                        return;
                    Records = new ObservableCollection<WSRecord>(records.OrderByDescending(c => c.OpenedDate));
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                        throw ex.InnerException;
                }
            });
        }
    }
}
