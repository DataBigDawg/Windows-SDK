using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class SafeArrayHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
	{
		private int length;
		private System.Type elementType;
		internal static SafeArrayHandle InvalidHandle
		{
			get
			{
				return new SafeArrayHandle(System.IntPtr.Zero, 0, null);
			}
		}
		private SafeArrayHandle(System.IntPtr handle, int length, System.Type elementType) : base(true)
		{
			this.length = length;
			this.elementType = elementType;
			base.SetHandle(handle);
		}
		internal static SafeArrayHandle FromArray<T>(T[] arr) where T : struct
		{
			if (arr == null)
			{
				return SafeArrayHandle.InvalidHandle;
			}
			int num = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
			System.IntPtr intPtr = System.IntPtr.Zero;
			int i = 0;
			try
			{
				intPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(num * arr.Length);
				for (i = 0; i < arr.Length; i++)
				{
					System.Runtime.InteropServices.Marshal.StructureToPtr(arr[i], System.IntPtr.Add(intPtr, num * i), false);
				}
			}
			catch (System.Exception)
			{
				SafeArrayHandle.CleanupStructArray(ref intPtr, i, typeof(T));
				throw;
			}
			return new SafeArrayHandle(intPtr, arr.Length, typeof(T));
		}
		internal static SafeArrayHandle FromArray(string[] arr)
		{
			if (arr == null)
			{
				return SafeArrayHandle.InvalidHandle;
			}
			int num = System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr));
			System.IntPtr intPtr = System.IntPtr.Zero;
			int i = 0;
			try
			{
				intPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(num * arr.Length);
				for (i = 0; i < arr.Length; i++)
				{
					System.Runtime.InteropServices.Marshal.WriteIntPtr(intPtr, num * i, System.Runtime.InteropServices.Marshal.StringToCoTaskMemAnsi(arr[i]));
				}
			}
			catch (System.Exception)
			{
				SafeArrayHandle.CleanupStringArray(ref intPtr, i);
				throw;
			}
			return new SafeArrayHandle(intPtr, arr.Length, typeof(string));
		}
		private static void CleanupStructArray(ref System.IntPtr parr, int len, System.Type elementType)
		{
			if (parr == System.IntPtr.Zero)
			{
				return;
			}
			int num = System.Runtime.InteropServices.Marshal.SizeOf(elementType);
			for (int i = 0; i < len; i++)
			{
				System.Runtime.InteropServices.Marshal.DestroyStructure(System.IntPtr.Add(parr, num * i), elementType);
			}
			System.Runtime.InteropServices.Marshal.FreeCoTaskMem(parr);
			parr = System.IntPtr.Zero;
		}
		private static void CleanupStringArray(ref System.IntPtr parr, int len)
		{
			if (parr == System.IntPtr.Zero)
			{
				return;
			}
			int num = System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr));
			for (int i = 0; i < len; i++)
			{
				System.IntPtr s = System.Runtime.InteropServices.Marshal.ReadIntPtr(parr, num * i);
				System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemAnsi(s);
			}
			System.Runtime.InteropServices.Marshal.FreeCoTaskMem(parr);
			parr = System.IntPtr.Zero;
		}
		protected override bool ReleaseHandle()
		{
			if (this.handle == System.IntPtr.Zero)
			{
				return true;
			}
			try
			{
				if (this.elementType == typeof(string))
				{
					SafeArrayHandle.CleanupStringArray(ref this.handle, this.length);
				}
				else
				{
					SafeArrayHandle.CleanupStructArray(ref this.handle, this.length, this.elementType);
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
