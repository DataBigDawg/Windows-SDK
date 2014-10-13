using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeHGlobalHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeHGlobalHandle InvalidHandle
		{
			get
			{
				return new SafeHGlobalHandle(System.IntPtr.Zero);
			}
		}
		internal SafeHGlobalHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeHGlobalHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			System.Runtime.InteropServices.Marshal.FreeHGlobal(this.handle);
			return true;
		}
	}
}
