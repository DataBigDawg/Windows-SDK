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


namespace Accela.WindowsStore.Sample.Views
{   /// <summary>
    /// Page providing basic features
    /// </summary>
    public sealed partial class ImagePage : Accela.WindowsStore.Sample.Common.LayoutAwarePage
    {
        public ImagePage()
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
