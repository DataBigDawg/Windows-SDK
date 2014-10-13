using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCoTaskMemStringHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		private bool unicode;
		internal static SafeHGlobalHandle InvalidHandle
		{
			get
			{
				return new SafeHGlobalHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCoTaskMemStringHandle(string str, bool unicode) : base(true)
		{
			this.unicode = unicode;
			System.IntPtr handle = unicode ? System.Runtime.InteropServices.Marshal.StringToCoTaskMemUni(str) : System.Runtime.InteropServices.Marshal.StringToCoTaskMemAnsi(str);
			base.SetHandle(handle);
		}
		protected override bool ReleaseHandle()
		{
			try
			{
				if (this.unicode)
				{
					System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemUnicode(this.handle);
				}
				else
				{
					System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemAnsi(this.handle);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
