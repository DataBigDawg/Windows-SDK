/*! \mainpage Accela SDK for Windows
 *
 * \section intro_sec Introduction
 *
 *
 * <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">Accela</a> SDK for Windows allows .NET developers to add <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">Accela</a> Automation functionality to your applications designed for for desktop, phones or tablets running Windows 8 operating system. The SDK includes .NET APIs that leverages functionality provided by Accela Cloud services through the REST interface. The API primarily provides record and inspection components. The record component accesses record information and allows for simple create of records. The inspection component allows access to inspection data with functions to schedule, reschedule, and cancel.
 *
 * The API is available in a single AccelaSDK dll library. Classes and functions is available in the Accela.WindowsStoreSDK namespace. This namespace prevents naming conflicts with classes defined in your applications or other frameworks you use.
 * 
 * The <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">Accela</a> Windows SDK is downloadable from <a href="https://www.nuget.org/packages/AccelaSDK/">NuGet</a> via Visual Studio Library Package Manager. For details, <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">The Accela Windows 8 SDK</a>.
 *
 * \section install_sec Setting Up Your Windows 8 Project
 *
 * When creating a Windows/Windows Phone project, you will need the Windows 8 or Windows 8.1 operating system. Also choose one of the editions of Visual Studio 2012 or Visual Studio 2013.
 * 
 * Make sure you installed the Package Manager Console or NuGet Package Manager. 
 * From the Tools menu, select the Package Manager. After the Package Manager Console windows appears, type the command 'Install-Package AccelaSDK'. If it is installed successfully, 'Accela SDK' will be shown on the references in the Solution Explorer. 
 * If you want to build the SDK using the source code, open the AccelaSDK-WindowsStore or AccelaSDK-WP8 project and build the project. Locate the downloaded AccelaSDK.dll file and add it to the references on the Solution Explorer in your project.
 * 
 * To start coding with <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">Accela</a> SDK, import the namespace:
 *
  \verbatim
  usingAccela.WindowsStoreSDK;
  \endverbatim
 *
 * You also need to declare the APP ID and APP Secret values which you received after you registered the windows store APP on the <a href="https://developer.accela.com/docs/accela_construct_api_developers_guide/windows_8_sdk/the_accela_windows_8_sdk.htm">AccelaDeveloper Portal</a>.
 * 
  \verbatim
  Public AccelaSDK ShareAccelaSDK { get; private set; }
  
  private string _appId = "";
  private string _appSecret = "";
  
  ShareAccelaSDK = new AccelaSDK(_appid, _appSecret);
  \endverbatim
*/