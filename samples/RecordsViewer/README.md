The RecordsViewer sample is a Windows app that shows how to view records using Accela Windows SDK. This app also plots record location on a map using the Bing Maps SDK.


## Built with

* Accela Windows SDK
* [Bing Maps SDK for Windows Store apps](http://visualstudiogallery.msdn.microsoft.com/ebc98390-5320-4088-a2b5-8f276e4530f9)
  * After you download Bing Maps SDK, add it to the References in the RecordsViewer solution. Make sure you have the correct Bing Maps SDK version that is compatible to your Windows 8 version. Refer to [Getting Started with Bing Maps](http://msdn.microsoft.com/en-us/library/hh846498.aspx) for version compatibility requirements between Bing Maps, Visual Studio, and Windows 8.
* [Accela Construct](https://developer.accela.com/) API

## Highlights
* <code>RecordsViewer/RecordsViewer.Wp8/ViewModels/LoginService.cs</code> - shows the authentication logic
* <code>RecordsViewer/RecordsViewer.Portable/RecordsViewModel.cs</code> - shows the Construct API calls

