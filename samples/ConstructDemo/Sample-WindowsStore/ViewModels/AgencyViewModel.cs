#if WINDOWS_PHONE || WINDOWS_PHONE_APP
using Accela.WindowsPhone.Sample.Extensions;
using Accela.WindowsPhone.Sample.Services;
using Accela.WindowsPhone.Sample.Models;
#else
using Accela.WindowsStore.Sample.Extensions;
using Accela.WindowsStore.Sample.Services;
using Accela.WindowsStore.Sample.Models;
#endif
using Accela.WindowsStoreSDK;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP

namespace Accela.WindowsPhone.Sample.ViewModels
#else
namespace Accela.WindowsStore.Sample.ViewModels
#endif
{
    /// <summary>
    /// Agency view model with action command for Agency page
    /// </summary>
    public class AgencyViewModel : ViewModelBase
    {
        private AccelaSDK _shareSdk;
        private string[] _scopes = new string[] { "records" };

        private IDataService _dataService;
        private IDisplayMessageService _displayMessageService;

        private string _recordId;
        private string _recordTypeValue;
        private string _recordDocumentId;

        private bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
            set { Set(ref _isRunning, value); }
        }

        private string _resultMsg;
        public string ResultMsg
        {
            get { return _resultMsg; }
            set { Set(ref _resultMsg, value); }
        }
        /// <summary>
        /// Agency View Model constructor
        /// </summary>
        public AgencyViewModel(IDataService dataService,
                               IDisplayMessageService displayMessageService)
        {
            _dataService = dataService;
            _displayMessageService = displayMessageService;

            var appId = "635442545792218073";
            var appSecret = "28c6edc56e714078a23a50a4193f348f";
            _shareSdk = new AccelaSDK(appId, appSecret);
            Messenger.Default.Register<CustomMessage>(this, (msg) => ShowResult(msg));
        }
        /// <summary>
        /// Display the mesage through message service
        /// </summary>
        private void ShowResult(CustomMessage msg)
        {
            if (msg != null)
            {
#if WINDOWS_PHONE || WINDOWS_PHONE_APP
                _displayMessageService.Show(msg.Title, msg.Message);
#else
                ResultMsg = msg.Message;
#endif
            }
        }

        private void RunningAction(Action command)
        {
            IsRunning = true;

            command.Invoke();

            IsRunning = false;
        }

        #region NativeLoginCommand
        private RelayCommand _nativeLoginCommand;
        public RelayCommand NativeLoginCommand
        {
            get
            {
                return _nativeLoginCommand ??
                    (_nativeLoginCommand = new RelayCommand(() => RunningAction(ExecuteNativeLoginCommand)));
            }
        }

        private async void ExecuteNativeLoginCommand()
        {

            try
            {
                if (await _shareSdk.Authorize("developer", "accela", "ISLANDTON", _scopes, AccelaEnvironment.PROD))
                {
                    Messenger.Default.Send(new CustomMessage("Login Succeeded"));
                }
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region WebLoginCommand
        private RelayCommand _webLoginCommand;
        public RelayCommand WebLoginCommand
        {
            get
            {
                return _webLoginCommand ??
                    (_webLoginCommand = new RelayCommand(() => RunningAction(ExecuteWebLoginCommand)));
            }
        }

        private async void ExecuteWebLoginCommand()
        {
            try
            {
                if (await _shareSdk.Authorize(_scopes)) 
                { 
                    Messenger.Default.Send(new CustomMessage("Login Succeeded"));
                }
            }
            catch (FileNotFoundException) { }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region GetRecords

        private RelayCommand _getRecordsCommand;
        public RelayCommand GetRecordsCommand
        {
            get
            {
                return _getRecordsCommand ??
                    (_getRecordsCommand = new RelayCommand(() => RunningAction(ExcuteGetRecordsCommand)));
            }
        }

        private async void ExcuteGetRecordsCommand()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            try
            {
                JsonObject jsonObj = await _dataService.GetRecords(_shareSdk);
                if (jsonObj != null && jsonObj.ContainsKey("result"))
                {
                    var results = jsonObj["result"] as JsonArray;
                    if (results != null && results.Count > 0)
                    {
                        var result = results[0] as JsonObject;
                        _recordId = result["id"] as string;
                        var type = result["type"] as JsonObject;
                        _recordTypeValue = type["value"] as string;
                    }
                }
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region Get A Specific Record

        private RelayCommand _getASpecificRecordCommand;
        public RelayCommand GetASpecificRecordCommand
        {
            get
            {
                return _getASpecificRecordCommand ??
                    (_getASpecificRecordCommand = new RelayCommand(() => RunningAction(ExcuteGetASpecificRecordCommand)));
            }
        }

        private async void ExcuteGetASpecificRecordCommand()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            if (string.IsNullOrWhiteSpace(_recordId))
            {
                _displayMessageService.Show("Required", "Please get the Record Id by Get Records first.");
                return;
            }
            try
            {
                JsonObject jsonObj = await _dataService.GetRecord(_shareSdk, new[] { _recordId });
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (AccelaApiException ex)
            {
                _displayMessageService.Show("Status", "Http Status:" + ex.ApiError.status + " Message:" + ex.ApiError.message + " ErrorCode:" + ex.ApiError.code + " TraceId:" + ex.ApiError.traceId);
            }
        }

        #endregion

        #region Create Record

        private RelayCommand _createRecordCommand;
        public RelayCommand CreateRecordCommand
        {
            get
            {
                return _createRecordCommand ??
                    (_createRecordCommand = new RelayCommand(() => RunningAction(ExcuteCreateRecordCommand)));
            }
        }

        private async void ExcuteCreateRecordCommand()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            if (string.IsNullOrWhiteSpace(_recordTypeValue) ||
                string.IsNullOrWhiteSpace(_recordId))
            {
                _displayMessageService.Show("Required", "Please get the Record Type by Get Records first.");
                return;
            }
            try
            {
                var jsonObj = await _dataService.CreateRecord(_shareSdk, _recordTypeValue);
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region Download Attachment List

        private RelayCommand _downloadAttachmentListCommand;
        public RelayCommand DownloadAttachmentListCommand
        {
            get
            {
                return _downloadAttachmentListCommand ??
                    (_downloadAttachmentListCommand = new RelayCommand(() => RunningAction(ExcuteDownloadAttachmentListCommand)));
            }
        }

        private async void ExcuteDownloadAttachmentListCommand()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }

            try
            {
                var jsonObj = await _dataService.GetRecordDocumentList(_shareSdk, _recordId);
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region Download Attachment
        private RelayCommand _downloadAttachmentCommand;
        public RelayCommand DownloadAttachmentCommand
        {
            get
            {
                return _downloadAttachmentCommand ??
                    (_downloadAttachmentCommand = new RelayCommand(() => RunningAction(ExcuteDownloadAttachmentCommand)));
            }
        }

        private async void ExcuteDownloadAttachmentCommand()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            if (string.IsNullOrWhiteSpace(_recordDocumentId))
            {
                _displayMessageService.Show("Required", "Please get record document list first.");
                return;
            }
            try
            {
                var bytes = await _dataService.DownloadDocument(_shareSdk, _recordDocumentId);

            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }
        #endregion

        #region Upload Attachment

        private RelayCommand _uploadAttachment;
        public RelayCommand UploadAttachment
        {
            get
            {
                return _uploadAttachment ??
                    (_uploadAttachment = new RelayCommand(() => RunningAction(ExcuteUploadAttachment)));
            }
        }

        private async void ExcuteUploadAttachment()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            try
            {
                JsonObject jsonObj = await _dataService.CreateRecordDocument(_shareSdk, _recordId, null);
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region Get AppSettings

        private RelayCommand _getAppSettingsCommand;
        public RelayCommand GetAppSettingsCommand
        {
            get
            {
                return _getAppSettingsCommand ??
                    (_getAppSettingsCommand = new RelayCommand(() => RunningAction(ExcuteGetAppSettingsCommand)));
            }
        }

        /// <summary>
        /// App can get app settings w/o access token
        /// </summary>
        private async void ExcuteGetAppSettingsCommand()
        {
            try
            {
                JsonObject jsonObj = await _dataService.GetAppSettings(_shareSdk);
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }

        #endregion

        #region Logout

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                return _logoutCommand ??
                    (_logoutCommand = new RelayCommand(() => RunningAction(ExcuteLogtouCommand)));
            }
        }

        private void ExcuteLogtouCommand()
        {
            try
            {
                _shareSdk.Logout();
                Messenger.Default.Send(new CustomMessage("Logout Succeeded"));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
            }
        }


        #endregion
    }
}
