using Microsoft.Win32.SafeHandles;
using System;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCertStoreHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCertStoreHandle InvalidHandle
		{
			get
			{
				return new SafeCertStoreHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCertStoreHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeCertStoreHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CertCloseStore(this.handle, 1u);
		}
	}
}
