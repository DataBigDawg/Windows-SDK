using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Accela.WindowsPhone8.Sample.Resources;
using Accela.WindowsStoreSDK;
using System.Dynamic;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Accela.WindowsPhone8.Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private ProgressIndicator _progressIndicator;
        // ????
        public MainPage()
        {
            InitializeComponent();
            //isPinch = false;
            //isDrag = false;
            //isGestureOnTarget = false;

            //App.ShareSDK.SessionChanged += SDK_SessionChanged;
        }

        //public void SDK_SessionChanged(object sender, WindowsStoreSDK.AccelaSessionEventArgs e)
        //{
        //    switch (e.SessionStatus)
        //    {
        //        case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginSucceeded:
        //            MessageBox.Show("Login Succeeded");
        //            break;
        //        case Accela.WindowsStoreSDK.AccelaSessionStatus.InvalidSession:
        //            MessageBox.Show("Invalid Session");
        //            break;
        //        case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginCancelled:
        //            MessageBox.Show("Login Cancelled");
        //            break;
        //        case Accela.WindowsStoreSDK.AccelaSessionStatus.LoginFailed:
        //            MessageBox.Show("Login Failed");
        //            break;
        //        case Accela.WindowsStoreSDK.AccelaSessionStatus.LogoutSucceeded:
        //            MessageBox.Show("Logout Succeeded");
        //            break;
        //        default:
        //            MessageBox.Show("error.");
        //            break;
        //    }
        //}

        //private void ShowMessage(string msg)
        //{
        //    MessageBox.Show(msg);
        //    Debug.WriteLine(msg);
        //}

        //private static AutoResetEvent choosePhoneFinishedEvent = new AutoResetEvent(false);

        //private Task<AccelaMediaStream> ChoosePhotoSource()
        //{
        //    Task<AccelaMediaStream> task = Task<AccelaMediaStream>.Factory.StartNew(() =>
        //    {

        //        AccelaMediaStream stream = null;

        //        Dispatcher.BeginInvoke(() =>
        //        {
        //            CustomMessageBox messageBox = new CustomMessageBox
        //            {
        //                Caption = "Upload Image",
        //                Message = "Choose how to upload image.",
        //                LeftButtonContent = "Take Photo",
        //                RightButtonContent = "Choose Existing"
        //            };
        //            messageBox.Dismissed += (s, e) =>
        //            {
        //                switch (e.Result)
        //                {
        //                    case CustomMessageBoxResult.LeftButton:
        //                        CameraCaptureTask cct = new CameraCaptureTask();
        //                        cct.Completed += (s1, e1) =>
        //                        {
        //                            stream = new AccelaMediaStream();
        //                            BuildMediaStream(stream, e1);
        //                        };
        //                        cct.Show();
        //                        break;
        //                    case CustomMessageBoxResult.RightButton:
        //                        PhotoChooserTask pct = new PhotoChooserTask();
        //                        pct.Completed += (s1, e1) =>
        //                        {
        //                            stream = new AccelaMediaStream();
        //                            BuildMediaStream(stream, e1);
        //                        };
        //                        pct.Show();
        //                        break;
        //                    case CustomMessageBoxResult.None:
        //                        break;
        //                    default:
        //                        throw new InvalidOperationException();
        //                }
        //            };
        //            messageBox.Show();
        //        });
        //        choosePhoneFinishedEvent.WaitOne();
        //        return stream;
        //    });

        //    return task;
        //}

        //private static async void BuildMediaStream(AccelaMediaStream stream, PhotoResult e1)
        //{

        //    if (e1.TaskResult == TaskResult.OK)
        //    {
        //        var fileName = Path.GetFileName(e1.OriginalFileName);

        //        using (var fileStream = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication().CreateFile(fileName))
        //        {
        //            byte[] bytes = new byte[e1.ChosenPhoto.Length];
        //            await e1.ChosenPhoto.ReadAsync(bytes, 0, bytes.Length);
        //            e1.ChosenPhoto.Seek(0, SeekOrigin.Begin);

        //            await fileStream.WriteAsync(bytes, 0, bytes.Length);
        //        }


        //        stream.FileName = fileName;
        //        //stream.SetValue(e1.ChosenPhoto);

        //        var filePath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, fileName);
        //        Windows.Storage.StorageFile file = await Windows.Storage.StorageFile.GetFileFromPathAsync(filePath);
        //        stream.ContentType = "image/*";
        //        stream.SetValue(await file.OpenStreamForReadAsync());
        //    }
        //    choosePhoneFinishedEvent.Set();
        //}

        //private void ProgressRun()
        //{
        //    if (_progressIndicator == null)
        //    {
        //        _progressIndicator = new ProgressIndicator();
        //        _progressIndicator.IsIndeterminate = true;
        //        _progressIndicator.Text = "Loading...";
        //        SystemTray.ProgressIndicator = _progressIndicator;
        //    }
        //    _progressIndicator.IsVisible = true;
        //}

        //private void ProgressStop()
        //{
        //    if (_progressIndicator != null)
        //        _progressIndicator.IsVisible = false;
        //}



        //#region Agency Code

        //string _recordId = "13CAP-00000-000T7",
        //       _recordDocId = "13CAP-00000-000T7-1856549";
        //private void Agency_CheckSession(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();
        //    MessageBox.Show(App.ShareSDK.IsSessionValid().ToString());
        //    ProgressStop();
        //}

        //private async void Login_Click(object sender, RoutedEventArgs e)
        //{
        //    String[] appV4Scopes = { "addresses", 
        //    "conditions", "contacts", "documents", "inspections", "owners",
        //    "parcels", "professionals", "records", "reports", "settings"};


        //    try
        //    {
        //        ProgressRun();
        //        await App.ShareSDK.Authorize(appV4Scopes);
        //    }
        //    catch (AccelaOAuthException ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    catch (AccelaApiException ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }

        //}
        //private async void btnLoginNative_Click(object sender, RoutedEventArgs e)
        //{
        //    String[] appV4Scopes = { "addresses", 
        //    "conditions", "contacts", "documents", "inspections", "owners",
        //    "parcels", "professionals", "records", "reports", "settings"};

        //    try
        //    {
        //        ProgressRun();
        //        await App.ShareSDK.Authorize("admin", "admin", "BPTDEV", appV4Scopes, AccelaEnvironment.DEV);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }
        //}
        

        //private async void Agency_GetRecords(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ProgressRun();
        //        //string path = "/v3/search/records?limit=25";
        //        //var result = await App.ShareSDK.PostAsync(path);
        //        string path = "/v4/records?openDateFrom={openDateFrom}&openDateTo={openDateTo}";
        //        var @params = new { openDateFrom = "2014-03-01", openDateTo = "2014-03-30" };
        //        var result = await App.ShareSDK.GetAsync(path, @params);
        //        MessageBox.Show(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }
        //}

        //private async void Agency_CreateRecord(object sender, RoutedEventArgs e)
        //{
        //    var postData = new
        //    {
        //        type = new
        //        {
        //            id = "ServiceRequest-Trees.cand.cWeeds-Tall.cGrass.cand.cWeeds-NA",
        //            value = "ServiceRequest/Trees and Weeds/Tall Grass and Weeds/NA"
        //        }
        //    };
        //    var jsonData = App.ShareSDK.SerializeJson(postData);
        //    try
        //    {
        //        ProgressRun();


        //        var result = await App.ShareSDK.PostAsync("/v4/records", jsonData);

        //        MessageBox.Show(result.ToString());

        //        _recordId = ((JsonObject)result["result"])["id"].ToString();

        //    }
        //    catch (AccelaOAuthException ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }

        //}

        //private async void Agency_UpdateRecord(object sender, RoutedEventArgs e)
        //{
        //    var postData = new
        //    {
        //        id = _recordId,
        //        priority = new
        //        {
        //            value = 2
        //        }
        //    };
        //    var jsonData = App.ShareSDK.SerializeJson(postData);
        //    try
        //    {
        //        ProgressRun();

        //        var result = await App.ShareSDK.PutAsync("/v4/records/" + _recordId, jsonData);

        //        ShowMessage(result.ToString());
        //    }
        //    catch (AccelaOAuthException ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }

        //}

        //private async void Agency_UploadDocuments(object sender, RoutedEventArgs e)
        //{
        //    var mediaObject = await ChoosePhotoSource();
        //    if (mediaObject == null || mediaObject.GetValue() == null)
        //        return;
        //    try
        //    {
        //        ProgressRun();
        //        string path = "/v4/records/{recordId}/documents";
        //        dynamic parameters = new ExpandoObject();
        //        parameters.file1 = mediaObject;
        //        parameters.recordId = _recordId;
        //        parameters.fileInfo = JsonConvert.SerializeObject(new[] { new { serviceProviderCode = "BPTDEV", fileName = mediaObject.FileName, type = mediaObject.ContentType } });

        //        var result = await App.ShareSDK.UploadAttachmentAsync(path, parameters, CancellationToken.None);

        //        ShowMessage(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }

        //}

        //private async void Agency_GetDocuments(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ProgressRun();

        //        string path = "/v4/records/{recordId}/documents";

        //        dynamic parameters = new ExpandoObject();
        //        parameters.limit = 5;
        //        parameters.recordId = _recordId;

        //        JsonObject result = await App.ShareSDK.GetAsync(path, parameters);

        //        var array = (JsonArray)result["result"];
        //        if (array != null)
        //        {

        //            var recordDoc = (JsonObject)array[0];

        //            _recordDocId = recordDoc["id"].ToString();

        //            ShowMessage(result.ToString());
        //        }
        //        else
        //        {
        //            ShowMessage("no documents");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }

        //}

        //private async void Agency_DownloadDocument(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ProgressRun();

        //        string path = "/v4/documents/{documentId}/download";

        //        var @params = new { documentId = _recordDocId };

        //        var result = await App.ShareSDK.DownloadAttachmentAsync(path, null);

        //        var bitmapImage = new BitmapImage();

        //        bitmapImage.SetSource(new MemoryStream(result));

        //        //imgPhoto.Source = bitmapImage;

        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }


        //}

        //private void Agency_Logout(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();

        //    App.ShareSDK.Logout();

        //    ProgressStop();

        //}
        //#endregion


        //#region PreviewPhoto

        ////SolidColorBrush greenBrush = new SolidColorBrush(Colors.Green);
        ////SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
        ////SolidColorBrush normalBrush;

        //bool isPinch;
        //bool isDrag;
        //double initialAngle;
        //bool isGestureOnTarget;

        //#region UIElement touch event handlers

        //// UIElement.ManipulationStarted indicates the beginning of a touch interaction. It tells us
        //// that we went from having no fingers on the screen to having at least one finger on the screen.
        //// It doesn't tell us what gesture this is going to become, but it can be useful for 
        //// initializing your gesture handling code.
        //private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        //{
        //}

        //// UIElement.Tap is used in place of GestureListener.Tap.
        //private void OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    transform.TranslateX = transform.TranslateY = 0;
        //}

        //// UIElement.DoubleTap is used in place of GestureListener.DoubleTap.
        //private void OnDoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    transform.ScaleX = transform.ScaleY = 1;
        //}

        //// UIElement.Hold is used in place of GestureListener.Hold.
        //private void OnHold(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    transform.TranslateX = transform.TranslateY = 0;
        //    transform.ScaleX = transform.ScaleY = 1;
        //    transform.Rotation = 0;
        //}

        //// UIElement.ManipulationDelta represents either a drag or a pinch.
        //// If PinchManipulation == null, then we have a drag, corresponding to GestureListener.DragStarted, 
        //// GestureListener.DragDelta, or GestureListener.DragCompleted.
        //// If PinchManipulation != null, then we have a pinch, corresponding to GestureListener.PinchStarted, 
        //// GestureListener.PinchDelta, or GestureListener.PinchCompleted.
        //// 
        //// In this sample we track drag and pinch state to illustrate how to manage transitions between 
        //// pinching and dragging, but commonly only the pinch or drag deltas will be of interest, in which 
        //// case determining when pinches and drags begin and end is not necessary.
        ////
        //// Note that the exact APIs for the event args are not quite the same as the ones in GestureListener.
        //// Comments inside methods called from here will note where they diverge.
        //private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        //{
        //    bool oldIsPinch = isPinch;
        //    bool oldIsDrag = isDrag;
        //    isPinch = e.PinchManipulation != null;

        //    // The origin of the first manipulation after a pinch is completed always corresponds to the
        //    // primary touch point from the pinch, even if the secondary touch point is the one that 
        //    // remains active. In this sample we only want a drag to affect the rectangle if the finger
        //    // on the screen falls inside the rectangle's bounds, so if we've just finished a pinch,
        //    // we have to defer until the next ManipulationDelta to determine whether or not a new 
        //    // drag has started.
        //    isDrag = e.PinchManipulation == null && !oldIsPinch;

        //    // check for ending gestures
        //    if (oldIsDrag && !isDrag)
        //    {
        //        this.OnDragCompleted();
        //    }
        //    if (oldIsPinch && !isPinch)
        //    {
        //        this.OnPinchCompleted();
        //    }

        //    // check for continuing gestures
        //    if (oldIsDrag && isDrag)
        //    {
        //        this.OnDragDelta(sender, e);
        //    }
        //    if (oldIsPinch && isPinch)
        //    {
        //        this.OnPinchDelta(sender, e);
        //    }

        //    // check for starting gestures
        //    if (!oldIsDrag && isDrag)
        //    {
        //        // Once a manipulation has started on the UIElement, that element will continue to fire ManipulationDelta
        //        // events until all fingers have left the screen and we get a ManipulationCompleted. In this sample
        //        // however, we treat each transition between pinch and drag as a new gesture, and we only want to 
        //        // apply effects to our border control if the the gesture begins within the bounds of the border.
        //        isGestureOnTarget = e.ManipulationContainer == imgPhoto &&
        //                new Rect(0, 0, imgPhoto.ActualWidth, imgPhoto.ActualHeight).Contains(e.ManipulationOrigin);
        //        this.OnDragStarted();
        //    }
        //    if (!oldIsPinch && isPinch)
        //    {
        //        isGestureOnTarget = e.ManipulationContainer == imgPhoto &&
        //                new Rect(0, 0, imgPhoto.ActualWidth, imgPhoto.ActualHeight).Contains(e.PinchManipulation.Original.PrimaryContact);
        //        this.OnPinchStarted(sender, e);
        //    }
        //}

        //// UIElement.ManipulationCompleted indicates the end of a touch interaction. It tells us that
        //// we went from having at least one finger on the screen to having no fingers on the screen.
        //// If e.IsInertial is true, then it's also the same thing as GestureListener.Flick,
        //// although the event args API for the flick case are different, as will be noted inside that method.
        //private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        //{
        //    if (isDrag)
        //    {
        //        isDrag = false;
        //        this.OnDragCompleted();

        //        if (e.IsInertial)
        //        {
        //            this.OnFlick(sender, e);
        //        }
        //    }

        //    if (isPinch)
        //    {
        //        isPinch = false;
        //        this.OnPinchCompleted();
        //    }
        //}

        //#endregion

        //#region Gesture events inferred from UIElement.Manipulation* touch events

        //private void OnDragStarted()
        //{
        //    if (isGestureOnTarget)
        //    {
        //        //imgPhoto.Background = greenBrush;
        //    }
        //}

        //private void OnDragDelta(object sender, ManipulationDeltaEventArgs e)
        //{
        //    if (isGestureOnTarget)
        //    {
        //        // HorizontalChange and VerticalChange from DragDeltaGestureEventArgs are now
        //        // DeltaManipulation.Translation.X and DeltaManipulation.Translation.Y.

        //        // The translation is given in the coordinate space of e.ManipulationContainer, which in
        //        // this case is the border control that we're applying transforms to. We need to apply 
        //        // the the current rotation and scale transforms to the deltas to get back to screen coordinates.
        //        // Note that if other ancestors of the border control had transforms applied as well, we would
        //        // need to use UIElement.TransformToVisual to get the aggregate transform between
        //        // the border control and Application.Current.RootVisual. See GestureListenerStatic.cs in the 
        //        // WP8 toolkit source for a detailed look at how this can be done.
        //        Point transformedTranslation = GetTransformNoTranslation(transform).Transform(e.DeltaManipulation.Translation);

        //        transform.TranslateX += transformedTranslation.X;
        //        transform.TranslateY += transformedTranslation.Y;
        //    }
        //}

        //private void OnDragCompleted()
        //{
        //    if (isGestureOnTarget)
        //    {
        //        //imgPhoto.Background = normalBrush;
        //    }
        //}

        //private void OnPinchStarted(object sender, ManipulationDeltaEventArgs e)
        //{
        //    if (isGestureOnTarget)
        //    {
        //        //imgPhoto.Background = redBrush;
        //        initialAngle = transform.Rotation;
        //    }
        //}

        //private void OnPinchDelta(object sender, ManipulationDeltaEventArgs e)
        //{
        //    if (isGestureOnTarget)
        //    {
        //        // Rather than providing the rotation, the event args now just provide
        //        // the raw points of contact for the pinch manipulation.
        //        // However, calculating the rotation from these two points is fairly trivial;
        //        // the utility method used here illustrates how that's done.
        //        // Note that we don't have to apply a transform because the angle delta is the
        //        // same in any non-skewed reference frame.
        //        double angleDelta = this.GetAngle(e.PinchManipulation.Current) - this.GetAngle(e.PinchManipulation.Original);

        //        transform.Rotation = initialAngle + angleDelta;

        //        // DistanceRatio from PinchGestureEventArgs is now replaced by
        //        // PinchManipulation.DeltaScale and PinchManipulation.CumulativeScale,
        //        // which expose the scale from the pinch directly.
        //        // Note that we don't have to apply a transform because the distance ratio is the
        //        // same in any reference frame.
        //        transform.ScaleX *= e.PinchManipulation.DeltaScale;
        //        transform.ScaleY *= e.PinchManipulation.DeltaScale;
        //    }
        //}

        //private void OnPinchCompleted()
        //{
        //    if (isGestureOnTarget)
        //    {
        //        //border.Background = normalBrush;
        //    }
        //}

        //private void OnFlick(object sender, ManipulationCompletedEventArgs e)
        //{
        //    if (isGestureOnTarget)
        //    {
        //        // All of the properties on FlickGestureEventArgs have been replaced by the single property
        //        // FinalVelocities.LinearVelocity.  This method shows how to retrieve from FinalVelocities.LinearVelocity
        //        // the properties that used to be in FlickGestureEventArgs. Also, note that while the GestureListener
        //        // provided fairly precise directional information, small linear velocities here are rounded
        //        // to 0, resulting in flick vectors that are often snapped to one axis.

        //        Point transformedVelocity = GetTransformNoTranslation(transform).Transform(e.FinalVelocities.LinearVelocity);

        //        double horizontalVelocity = transformedVelocity.X;
        //        double verticalVelocity = transformedVelocity.Y;
        //    }
        //}
        //#endregion

        //#region Helpers
        //private GeneralTransform GetTransformNoTranslation(CompositeTransform transform)
        //{
        //    CompositeTransform newTransform = new CompositeTransform();
        //    newTransform.Rotation = transform.Rotation;
        //    newTransform.ScaleX = transform.ScaleX;
        //    newTransform.ScaleY = transform.ScaleY;
        //    newTransform.CenterX = transform.CenterX;
        //    newTransform.CenterY = transform.CenterY;
        //    newTransform.TranslateX = 0;
        //    newTransform.TranslateY = 0;

        //    return newTransform;
        //}

        //private double GetAngle(PinchContactPoints points)
        //{
        //    Point directionVector = new Point(points.SecondaryContact.X - points.PrimaryContact.X, points.SecondaryContact.Y - points.PrimaryContact.Y);
        //    return GetAngle(directionVector.X, directionVector.Y);
        //}

        //private Orientation GetDirection(double x, double y)
        //{
        //    return Math.Abs(x) >= Math.Abs(y) ? System.Windows.Controls.Orientation.Horizontal : System.Windows.Controls.Orientation.Vertical;
        //}

        //private double GetAngle(double x, double y)
        //{
        //    // Note that this function works in xaml coordinates, where positive y is down, and the
        //    // angle is computed clockwise from the x-axis. 
        //    double angle = Math.Atan2(y, x);

        //    // Atan2() returns values between pi and -pi.  We want a value between
        //    // 0 and 2 pi.  In order to compensate for this, we'll add 2 pi to the angle
        //    // if it's less than 0, and then multiply by 180 / pi to get the angle
        //    // in degrees rather than radians, which are the expected units in XAML.
        //    if (angle < 0)
        //    {
        //        angle += 2 * Math.PI;
        //    }

        //    return angle * 180 / Math.PI;
        //}
        //#endregion






        //#endregion

        //#region inspection
        //private string strInspectionId = "";
        //private string strInspectionDocId = "";
        //private void btn_CheckSession_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    Agency_CheckSession(sender, e);
        //}
        //private void btn_Login_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    btnLoginNative_Click(sender, e);
        //}

        //private async void btnGet_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();
        //    try
        //    {
        //        string path = "/v4/search/inspections";
        //        var postData = new { scheduleDate = DateTime.Today.AddDays(-5d).ToString("yyyy-MM-dd"), scheduleDateEnd = DateTime.Today.AddDays(5d).ToString("yyyy-MM-dd") };

        //        var result = await App.ShareSDK.PostAsync(path, JsonConvert.SerializeObject(postData));

        //        ShowMessage(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    ProgressStop();
        //}

        //private async void btnCreate_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();

        //    try
        //    {
        //        string path = "/v4/inspections/schedule";
        //        var postData = new { recordId = new { id = "BPTDEV-14CAP-00000-0002T" }, type = new { id = "362" } };
        //        var result = await App.ShareSDK.PostAsync(path, JsonConvert.SerializeObject(postData));
        //        if (result.ContainsKey("result"))
        //        {
        //            strInspectionId = ((JsonObject)result["result"])["id"].ToString();
        //        }
        //        ShowMessage(result.ToString());

        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    ProgressStop();

        //}

        //private async void btnUpdate_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();

        //    try
        //    {
        //        string path = "/v4/inspections/" + strInspectionId;
        //        var postData = new { requestComment = "this is comment" };
        //        JsonObject result = await App.ShareSDK.PutAsync(path, JsonConvert.SerializeObject(postData));
        //        ShowMessage(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    ProgressStop();

        //}

        //private async void btnUploadDocuments_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    var mediaObject = await ChoosePhotoSource();
        //    if (mediaObject == null || mediaObject.GetValue() == null)
        //        return;
        //    try
        //    {
        //        ProgressRun();

        //        AccelaMediaStream mediaObject2 = new AccelaMediaStream();
        //        mediaObject2.ContentType = mediaObject.ContentType;
        //        mediaObject2.FileName = mediaObject.FileName;
        //        var filePath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, mediaObject.FileName);
        //        Windows.Storage.StorageFile file = await Windows.Storage.StorageFile.GetFileFromPathAsync(filePath);
        //        mediaObject2.SetValue(await file.OpenStreamForReadAsync());

        //        string path = "/v4/inspections/{inspectionId}/documents";
        //        dynamic parameters = new ExpandoObject();
        //        parameters.file1 = mediaObject;
        //        parameters.file2 = mediaObject2;
        //        parameters.inspectionId = strInspectionId;
        //        parameters.fileInfo = JsonConvert.SerializeObject(
        //            new[] { 
        //                new { 
        //                    serviceProviderCode = "BPTDEV", 
        //                    fileName = mediaObject.FileName, 
        //                    type = mediaObject.ContentType 
        //                },
        //                new { 
        //                    serviceProviderCode = "BPTDEV", 
        //                    fileName = mediaObject.FileName, 
        //                    type = mediaObject.ContentType 
        //                }
        //            });
        //        var result = await App.ShareSDK.UploadAttachmentAsync(path, parameters, CancellationToken.None);
        
        //        ShowMessage(result.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }
        //}


        //private async void btnGetDocuments_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    ProgressRun();
        //    try
        //    {
        //        string path = "/v4/inspections/{inspectionId}/documents";
        //        var urlParams = new { inspectionId = strInspectionId };

        //        var result = await App.ShareSDK.GetAsync(path, urlParams);
        //        if (result.ContainsKey("result"))
        //        {
        //            strInspectionDocId = ((JsonObject)((JsonArray)result["result"])[0])["id"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    ProgressStop();
        //}

        //private async void btnDownloadDocument_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ProgressRun();

        //        string path = "/v4/documents/{documentId}/download";

        //        var @params = new { documentId = strInspectionDocId };

        //        var result = await App.ShareSDK.DownloadAttachmentAsync(path, @params);

        //        var bitmapImage = new BitmapImage();

        //        bitmapImage.SetSource(new MemoryStream(result));

        //        imgPhoto.Source = bitmapImage;

        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessage(ex.Message);
        //    }
        //    finally
        //    {
        //        ProgressStop();
        //    }
        //}

        //private void btnLogout_Insp_Click(object sender, RoutedEventArgs e)
        //{
        //    Agency_Logout(sender, e);
        //}
        //#endregion
    }
}