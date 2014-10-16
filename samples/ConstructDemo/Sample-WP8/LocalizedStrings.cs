using Accela.WindowsPhone.Sample.Resources;

namespace Accela.WindowsPhone.Sample
{
    /// <summary>
    /// Supply access to the resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}