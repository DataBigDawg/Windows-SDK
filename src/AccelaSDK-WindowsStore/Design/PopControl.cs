/** 
  * Copyright 2014 Accela, Inc. 
  * 
  * You are hereby granted a non-exclusive, worldwide, royalty-free license to 
  * use, copy, modify, and distribute this software in source code or binary 
  * form for use in connection with the web services and APIs provided by 
  * Accela. 
  * 
  * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
  * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
  * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
  * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
  * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
  * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
  * DEALINGS IN THE SOFTWARE. 
  * 
  */
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Accela.WindowsStoreSDK
{
    internal sealed class PopControl : ContentControl  
    {
        Popup m_pop = null;

        public PopControl()
        {
            this.DefaultStyleKey = typeof(PopControl);

            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            this.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.m_pop = new Popup();
            this.m_pop.Child = this;

        }

        public TransitionCollection PopTransitions
        {
            get
            {
                if (this.m_pop.ChildTransitions == null)
                {
                    this.m_pop.ChildTransitions = new TransitionCollection();
                }
                return this.m_pop.ChildTransitions;
            }
        }

        public void ShowPop()
        {
            if (this.m_pop != null)
                this.m_pop.IsOpen = true;
        }

        public void HidePop()
        {
            if (this.m_pop != null)
                this.m_pop.IsOpen = false;
        }

    }
}
