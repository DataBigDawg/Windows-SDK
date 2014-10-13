using Microsoft.Win32.SafeHandles;
using System;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeCryptMemAllocHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		internal static SafeCryptMemAllocHandle InvalidHandle
		{
			get
			{
				return new SafeCryptMemAllocHandle(System.IntPtr.Zero);
			}
		}
		internal SafeCryptMemAllocHandle(System.IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}
		private SafeCryptMemAllocHandle() : base(true)
		{
		}
		protected override bool ReleaseHandle()
		{
			NativeMethods.CryptMemFree(this.handle);
			return true;
		}
	}
}
