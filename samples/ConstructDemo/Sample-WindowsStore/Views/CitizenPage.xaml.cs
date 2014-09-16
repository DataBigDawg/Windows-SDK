using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Accela.WindowsStoreSDK;


namespace Accela.WindowsStore.Sample
{    /// <summary>
    /// Citizen page
    /// </summary>
    public sealed partial class CitizenPage : Accela.WindowsStore.Sample.Common.LayoutAwarePage
    {
        public CitizenPage()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }


        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }



    }
}
