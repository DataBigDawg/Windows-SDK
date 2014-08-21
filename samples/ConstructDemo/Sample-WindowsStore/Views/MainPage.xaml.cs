using Accela.WindowsStoreSDK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace Accela.WindowsStore.Sample
{
    public sealed partial class MainPage : Page
    {
        //    string[] _scopes = { 
        //                            "get_records", 
        //                            "get_record", 
        //                            "search_records", 
        //                            "create_record" ,
        //                            "get_record_documents",
        //                            "get_document",
        //                            "create_record_document",
        //                            "get_reports_definition",
        //                       };
        //    string _recordId = string.Empty,
        //           _recordDocId = string.Empty,
        //           _recordDocName = string.Empty;

        //    private AccelaSDK _sharedAccelaSDK;

        public MainPage()
        {
            //_sharedAccelaSDK = ((App)App.Current).SharedAccelaSDK;
            //_sharedAccelaSDK.SessionChanged += _sharedAccelaSDK_SessionDelegate;
            this.InitializeComponent();
        }

        //    protected override void OnNavigatedTo(NavigationEventArgs e)
        //    {
        //        EnabledButton(_sharedAccelaSDK.IsSessionValid());
        //    }

        //    private void InitSDK(string id, string secret)
        //    {
        //        _sharedAccelaSDK = new AccelaSDK(id, secret);
        //        _sharedAccelaSDK.SessionChanged += _sharedAccelaSDK_SessionDelegate;
        //        EnabledButton(_sharedAccelaSDK.IsSessionValid());
        //    }

        //    private void EnabledButton(bool isLogon)
        //    {
        //        //btnWebLogin.IsEnabled = !isLogon;

        //        //btnCitizenWebLogin.IsEnabled = !isLogon;

        //        //btnGetRecords.IsEnabled = isLogon;

        //        //btnCreateRecord.IsEnabled = isLogon;

        //        //btnDownloadRecordDoc.IsEnabled = isLogon;

        //        //btnLogout.IsEnabled = isLogon;

        //        //btnUploadRecordDoc.IsEnabled = isLogon;

        //        //btnGetRecordDocs.IsEnabled = isLogon;
        //    }

        //    private async void btnLogin_Click(object sender, RoutedEventArgs e)
        //    {
        //        string _appId = "635079844452681133";   //TODO: Agency appid
        //        string _appSecret = "2a8ac15ea324404eaf8cee7b437b451f"; //TODO: Agency appsecret

        //        InitSDK(_appId, _appSecret);
        //        _sharedAccelaSDK.ApiHost = "apis.dev.accela.com";
        //        _sharedAccelaSDK.OAuthHost = "auth.dev.accela.com";

        //        progressBar.IsIndeterminate = true;
        //        try
        //        {
        //            await _sharedAccelaSDK.Authorize(_scopes);
        //        }
        //        catch (AccelaOAuthException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (AccelaApiException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;

        //    }

        //    private async void btnWebLogin_Click_1(object sender, RoutedEventArgs e)
        //    {
        //        //production
        //        //string _appId = "635042799889843982";   //todo: Citic appid
        //        //string _appSecret = "ec81b6c3901b479cb0f9166ecac7969f"; //todo: Citic appsecret

        //        //developer
        //        //string _appId = "635029121525590692";   //todo: Citic appid
        //        //string _appSecret = "5dddd38a56dd45fcaf260d2cc71b7912"; //todo: Citic appsecret


        //        string[] _scopes = { 
        //                            "get_records", 
        //                            "get_record", 
        //                            "search_records", 
        //                            "create_record" ,
        //                            "get_record_documents",
        //                            "get_document",
        //                            "create_record_document",
        //                            "get_reports_definition"
        //                       };


        //        InitSDK(_appId, _appSecret);

        //        //production
        //        _sharedAccelaSDK.ApiHost = "apis.accela.com";
        //        _sharedAccelaSDK.OAuthHost = "auth.accela.com";
        //        _sharedAccelaSDK.CustomHttpHeaders.Add("x-accela-agency", "islandton");

        //        //developer
        //        //_sharedAccelaSDK.ApiHost = "apis.dev.accela.com";
        //        //_sharedAccelaSDK.OAuthHost = "auth.dev.accela.com";
        //        //_sharedAccelaSDK.CustomHttpHeaders.Add("x-accela-agency", "islandton");

        //        progressBar.IsIndeterminate = true;
        //        try
        //        {
        //            await _sharedAccelaSDK.Authorize(_scopes);
        //        }
        //        catch (AccelaOAuthException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (AccelaApiException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;
        //    }


        //    private async void btnNativeLogin_Click_1(object sender, RoutedEventArgs e)
        //    {
        //        //production
        //        string _appId = "635042799889843982";   //todo: Citic appid
        //        string _appSecret = "ec81b6c3901b479cb0f9166ecac7969f"; //todo: Citic appsecret
        //        string[] _scopes = { 
        //                            "get_records", 
        //                            "get_record", 
        //                            "search_records", 
        //                            "create_record" ,
        //                            "get_record_documents",
        //                            "get_document",
        //                            "create_record_document",
        //                            "get_reports_definition"
        //                       };
        //        InitSDK(_appId, _appSecret);

        //        //production
        //        _sharedAccelaSDK.ApiHost = "apps-apis.dev.accela.com";
        //        _sharedAccelaSDK.OAuthHost = "apps-auth.dev.accela.com";

        //        progressBar.IsIndeterminate = true;
        //        try
        //        {
        //            await _sharedAccelaSDK.Authorize("admin", "admin", "BPTDEV", _scopes, AccelaEnvironment.STAGE);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;
        //    }

        //    private async void btnGetRecords_Click(object sender, RoutedEventArgs e)
        //    {
        //        progressBar.IsIndeterminate = true;

        //        try
        //        {
        //            string path = "/v3/search/records?limit=25";
        //            var result = await _sharedAccelaSDK.PostAsync(path);

        //            if (result != null)
        //            {
        //                var records = result["records"] as JsonArray;
        //                if (records.Count > 0)
        //                {
        //                    var record = records[0] as JsonObject;
        //                    var openDate = record["openDate"] as string;
        //                    IFormatProvider culture = new CultureInfo("en-US");
        //                    var date = DateTime.ParseExact(openDate, "yyyy-MM-dd h:m:s", culture);
        //                }
        //            }

        //            MessageHelper.DisplayMessage(result.ToString(), txtOutput);
        //        }
        //        catch (AccelaOAuthException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (AccelaApiException ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }

        //        progressBar.IsIndeterminate = false;
        //    }

        //    private async void btnCreateRecord_Click(object sender, RoutedEventArgs e)
        //    {
        //        progressBar.IsIndeterminate = true;

        //        var postData = new
        //        {
        //            createRecord = new
        //            {
        //                type = new
        //                {
        //                    id = "Building-Residential-Demolition-NA",

        //                },
        //                description = "Residential Demolition"
        //            }
        //        };

        //        var jsonData = JsonConvert.SerializeObject(postData);

        //        try
        //        {

        //            var result = await _sharedAccelaSDK.PostAsync("/v3/records", jsonData);

        //            MessageHelper.DisplayMessage(result.ToString(), txtOutput);

        //            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(result.ToString());

        //            _recordId = obj.recordId.id;

        //        }
        //        catch (AccelaOAuthException ex)
        //        {
        //            btnLogout_Click(null, null);
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;

        //    }

        //    private Task<IReadOnlyList<StorageFile>> openMultiplePicPicker()
        //    {
        //        FileOpenPicker picker = new FileOpenPicker();

        //        picker.ViewMode = PickerViewMode.Thumbnail;

        //        picker.SuggestedStartLocation = PickerLocationId.Desktop;

        //        picker.FileTypeFilter.Add(".jpg");

        //        picker.FileTypeFilter.Add(".png");

        //        picker.FileTypeFilter.Add(".jpeg");

        //        return picker.PickMultipleFilesAsync().AsTask();
        //    }

        //    private Task<StorageFile> openSinglePicPicker()
        //    {
        //        FileOpenPicker picker = new FileOpenPicker();

        //        picker.ViewMode = PickerViewMode.Thumbnail;

        //        picker.SuggestedStartLocation = PickerLocationId.Desktop;

        //        picker.FileTypeFilter.Add(".jpg");

        //        picker.FileTypeFilter.Add(".png");

        //        picker.FileTypeFilter.Add(".jpeg");

        //        return picker.PickSingleFileAsync().AsTask();
        //    }

        //    private async void btnUploadRecordDoc_Click(object sender, RoutedEventArgs e)
        //    {
        //        if (string.IsNullOrEmpty(_recordId))
        //        {
        //            MessageHelper.ShowMessage("Please creater Record.");
        //            return;
        //        }

        //        progressBar.IsIndeterminate = true;

        //        StorageFile file = await openSinglePicPicker();

        //        if (file == null)
        //            return;

        //        var mediaObject = new AccelaMediaStream()
        //        {
        //            FileName = file.Name,
        //            ContentType = file.ContentType
        //        }.SetValue(await file.OpenStreamForReadAsync());

        //        //var mediaObject = new AccelaSDKMediaObject()
        //        //{
        //        //    FileName = file.Name,
        //        //    ContentType = file.ContentType
        //        //};

        //        //IBuffer buffer = await FileIO.ReadBufferAsync(file);

        //        //byte[] bytes = new byte[buffer.Length];

        //        //using (DataReader reader = DataReader.FromBuffer(buffer))
        //        //{
        //        //    reader.ReadBytes(bytes);
        //        //}

        //        //mediaObject.SetValue(bytes);

        //        //string str = (new StreamReader(mediaObject.GetValue())).ReadToEnd();

        //        try
        //        {
        //            string path = string.Format("v4/records/{0}/documents/", _recordId);

        //            dynamic parameters = new ExpandoObject();
        //            parameters.file1 = mediaObject;
        //            parameters.fileInfo = JsonConvert.SerializeObject(new[] { new { serviceProviderCode = "BPTDEV", fileName = file.Name, type = file.ContentType } });

        //            var result = await _sharedAccelaSDK.UploadAttachmentAsync(path, parameters);

        //            MessageHelper.DisplayMessage(result.ToString(), txtOutput);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;

        //    }

        //    private async void btnGetRecordDocs_Click(object sender, RoutedEventArgs e)
        //    {
        //        if (string.IsNullOrEmpty(_recordId))
        //        {
        //            MessageHelper.ShowMessage("Please creater Record.");
        //            return;
        //        }

        //        progressBar.IsIndeterminate = true;

        //        try
        //        {
        //            string path = string.Format("v4/records/{0}/documents", _recordId);

        //            dynamic parameters = new ExpandoObject();
        //            parameters.limit = 5;

        //            var result = await _sharedAccelaSDK.GetAsync(path, parameters);

        //            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(result.ToString());

        //            _recordDocId = obj.documents[0].id;

        //            _recordDocName = obj.documents[0].fileName;

        //            MessageHelper.DisplayMessage(result.ToString(), txtOutput);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }
        //        progressBar.IsIndeterminate = false;

        //    }

        //    private async void btnDownloadRecordDoc_Click(object sender, RoutedEventArgs e)
        //    {
        //        progressBar.IsIndeterminate = true;

        //        try
        //        {
        //            string path = string.Format("/v3/documents/{0}", _recordDocId);

        //            var bitmapImage = new BitmapImage();
        //            var img = new Image();

        //            StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("files", CreationCollisionOption.OpenIfExists);

        //            StorageFile file = await folder.CreateFileAsync(_recordDocId + ".png", CreationCollisionOption.ReplaceExisting);

        //            await _sharedAccelaSDK.DownloadAttachmentAsync(path, null, file.Path);

        //            using (var ras = await file.OpenAsync(FileAccessMode.Read))
        //            {
        //                await bitmapImage.SetSourceAsync(ras);
        //                img.Source = bitmapImage;
        //            }

        //            //var result = await _am.DownloadAttachment("v3/document/{id}/", parameters);

        //            //using (var ras = new InMemoryRandomAccessStream())
        //            //using (var outputStream = ras.GetOutputStreamAt(0))
        //            //using (var stream = new MemoryStream(result))
        //            //{
        //            //    await RandomAccessStream.CopyAsync(stream.AsInputStream(), outputStream);


        //            //    await bitmapImage.SetSourceAsync(ras);

        //            //    img.Source = bitmapImage;
        //            //}

        //            popupImg.Child = img;

        //            popupImg.IsLightDismissEnabled = true;

        //            if (!popupImg.IsOpen) popupImg.IsOpen = true;


        //        }
        //        catch (Exception ex)
        //        {
        //            MessageHelper.ShowMessage(ex.Message);
        //        }

        //        progressBar.IsIndeterminate = false;

        //    }

        //    private void btnLogout_Click(object sender, RoutedEventArgs e)
        //    {
        //        _sharedAccelaSDK.Logout();
        //    }

        //    void _sharedAccelaSDK_SessionDelegate(object sender, AccelaSessionEventArgs e)
        //    {
        //        switch (e.SessionStatus)
        //        {
        //            case AccelaSessionStatus.LoginSucceeded:
        //                EnabledButton(true);
        //                txtOutput.Text = "Login succeeded.";
        //                break;
        //            case AccelaSessionStatus.InvalidSession:
        //                EnabledButton(false);
        //                txtOutput.Text = "Invalid session.";
        //                break;
        //            case AccelaSessionStatus.LoginCancelled:
        //                txtOutput.Text = "Login cancelled.";
        //                break;
        //            case AccelaSessionStatus.LoginFailed:
        //                txtOutput.Text = "Login failed.";
        //                break;
        //            case AccelaSessionStatus.LogoutSucceeded:
        //                EnabledButton(false);
        //                txtOutput.Text = "Logout succeeded.";
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    private void btnGetReportsDefinition_Click(object sender, RoutedEventArgs e)
        //    {

        //    }

        //}

    }
}
