using Microsoft.Win32.SafeHandles;
using System;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCryptKeyHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCryptKeyHandle InvalidHandle
		{
			get
			{
				return new SafeCryptKeyHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCryptKeyHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeCryptKeyHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CryptDestroyKey(this.handle);
		}
	}
}
