using Microsoft.Win32.SafeHandles;
using System;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCryptProviderContextHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCryptProviderContextHandle InvalidHandle
		{
			get
			{
				return new SafeCryptProviderContextHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCryptProviderContextHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeCryptProviderContextHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CryptReleaseContext(this.handle, 0u);
		}
	}
}
