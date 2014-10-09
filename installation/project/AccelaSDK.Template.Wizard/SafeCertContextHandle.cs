using Microsoft.Win32.SafeHandles;
using System;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCertContextHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCertContextHandle InvalidHandle
		{
			get
			{
				return new SafeCertContextHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCertContextHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeCertContextHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CertFreeCertificateContext(this.handle);
		}
	}
}
