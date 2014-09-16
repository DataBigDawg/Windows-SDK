using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace Accela.WindowsStoreSDK
{
    internal sealed class AgencyNativeControl : Control
    {
        public AgencyNativeControl()
        {
            this.DefaultStyleKey = typeof(AgencyNativeControl);
            this.Width = Window.Current.Bounds.Width;
        }

        #region Property
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(AgencyNativeControl), new PropertyMetadata(null));
        #endregion
    }
}
