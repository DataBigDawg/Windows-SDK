using Accela.WindowsPhone8.Sample.Resources;

namespace Accela.WindowsPhone8.Sample
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