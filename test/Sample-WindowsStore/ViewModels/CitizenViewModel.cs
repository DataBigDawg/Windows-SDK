#if WINDOWS_PHONE
using Accela.WindowsPhone8.Sample.Models;
using Accela.WindowsPhone8.Sample.Services;
using Accela.WindowsPhone8.Sample.Extensions;
#else
using Accela.WindowsStore.Sample.Models;
using Accela.WindowsStore.Sample.Services;
using Accela.WindowsStore.Sample.Extensions;
#endif
using Accela.WindowsStoreSDK;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

#if WINDOWS_PHONE
namespace Accela.WindowsPhone8.Sample.ViewModels
#else
namespace Accela.WindowsStore.Sample.ViewModels
#endif
{
    public class CitizenViewModel : ViewModelBase
    {
        private AccelaSDK _shareSdk = null;
        private IDataService _dataService;
        private IDisplayMessageService _displayMessageService;

        private string _recordTypeId = "";


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

        public CitizenViewModel(IDataService dataService,
                                IDisplayMessageService displayMessageService)
        {
            _dataService = dataService;
            _displayMessageService = displayMessageService;

            var appId = "635330682731176056";
            var appSecret = "318feed831d54d3bac493010617090f5";
            var authHost = "apps-auth.dev.accela.com";
            var apiHost = "apps-apis.dev.accela.com";
            _shareSdk = new AccelaSDK(appId, appSecret);
            _shareSdk.ApiHost = apiHost;
            _shareSdk.OAuthHost = authHost;

            Messenger.Default.Register<CustomMessage>(this, (msg) => ShowResult(msg));
        }

        private void ShowResult(CustomMessage msg)
        {
            if (msg != null)
            {
#if WINDOWS_PHONE
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


        #region LoginCommand
        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                    (_loginCommand = new RelayCommand(() => RunningAction(ExecuteLoginCommand)));
            }
        }

        private async void ExecuteLoginCommand()
        {
            var scopes = new string[] { "records", "a311citizen_create_record" };
            try
            {
                await _shareSdk.Authorize(scopes, "BPTDEV");
                Messenger.Default.Send(new CustomMessage("Login Succeeded"));

            }
            catch (FileNotFoundException)
            {

            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
                //Messenger.Default.Send(new CustomMessage("Error", ex.GetUnderExceptionMessage()));
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
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));

            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
                //Messenger.Default.Send(new CustomMessage("Error", ex.GetUnderExceptionMessage()));
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
            if (string.IsNullOrWhiteSpace(_recordTypeId))
            {
                _displayMessageService.Show("Required", "Please get the Record Type by Get Records first.");
            }
            try
            {
                var jsonObj = await _dataService.CreateRecord(_shareSdk, _recordTypeId);
                Messenger.Default.Send(new CustomMessage(jsonObj.ToString()));
            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
                Messenger.Default.Send(new CustomMessage("Error", ex.GetUnderExceptionMessage()));
            }
        }

        #endregion

        #region Create record with attachments

        private RelayCommand _createRecordWithAttachmentsCommand;
        public RelayCommand CreateRecordWidthAttachmentsCommand
        {
            get
            {
                return _createRecordWithAttachmentsCommand ??
                    (_createRecordWithAttachmentsCommand = new RelayCommand(() => RunningAction(ExcuteCreateRecordWithAttachmentsCommand)));
            }
        }

        private async void ExcuteCreateRecordWithAttachmentsCommand()
        {
            if (string.IsNullOrWhiteSpace(_recordTypeId))
            {
                _displayMessageService.Show("Required", "Please get the Record Type by Get Records first.");
                //Messenger.Default.Send(new CustomMessage("Required", "Please get the Record Type by Get Records first."));

            }
            try
            {

            }
            catch (Exception ex)
            {
                _displayMessageService.Show("Error", ex.GetUnderExceptionMessage());
                //Messenger.Default.Send(new CustomMessage("Error", ex.Message));
            }
        }
        #endregion

        #region Search Record

        private RelayCommand _searchRecordsCommand;
        public RelayCommand SearchRecordsCommand
        {
            get
            {
                return _searchRecordsCommand ??
                    (_searchRecordsCommand = new RelayCommand(() => RunningAction(ExcuteSearchRecords)));
            }
        }

        private async void ExcuteSearchRecords()
        {
            if (!_shareSdk.IsSessionValid())
            {
                _displayMessageService.Show("Info", "Not logged in yet.");
                return;
            }
            try
            {
                var jsonObj = await _dataService.SearchRecords(_shareSdk);
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
                //Messenger.Default.Send(new CustomMessage("Error", ex.GetUnderExceptionMessage()));
            }
        }


        #endregion
    }
}
