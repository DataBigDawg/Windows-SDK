using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

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

        //public String Agency
        //{
        //    get { return (string)GetValue(AgencyProperty); }
        //    set { SetValue(AgencyProperty, value); }
        //}

        //public String UserName
        //{
        //    get { return (string)GetValue(UserNameProperty); }
        //    set { SetValue(UserNameProperty, value); }
        //}

        //public String Password
        //{
        //    get { return (string)GetValue(PasswordProperty); }
        //    set { SetValue(PasswordProperty, value); }
        //}

        //public IList<string> EnvList
        //{
        //    get { return (IList<string>)GetValue(EnvListProperty); }
        //    set { SetValue(EnvListProperty, value); }
        //}

        //public String Enviroment
        //{
        //    get { return (string)GetValue(EnviromentProperty); }
        //    set { SetValue(EnviromentProperty, value); }
        //}
        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(AgencyNativeControl), new PropertyMetadata(null));

        //public static readonly DependencyProperty AgencyProperty =
        //    DependencyProperty.Register("Agency", typeof(string), typeof(AgencyNativeControl), new PropertyMetadata(null));

        //public static readonly DependencyProperty UserNameProperty =
        //    DependencyProperty.Register("UserName", typeof(string), typeof(AgencyNativeControl), new PropertyMetadata(null));

        //public static readonly DependencyProperty PasswordProperty =
        //    DependencyProperty.Register("Password", typeof(string), typeof(AgencyNativeControl), new PropertyMetadata(null));

        //public static readonly DependencyProperty EnvListProperty =
        //    DependencyProperty.Register("EnvList", typeof(IList<string>), typeof(AgencyNativeControl), new PropertyMetadata(null));

        //public static readonly DependencyProperty EnviromentProperty =
        //    DependencyProperty.Register("Enviroment", typeof(string), typeof(AgencyNativeControl), new PropertyMetadata(null));

        #endregion
    }
}
