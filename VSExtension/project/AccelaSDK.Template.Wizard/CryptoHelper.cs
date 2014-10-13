using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
namespace Accela.Mobile.CustomWizard
{
	internal sealed class CryptoHelper : System.IDisposable
	{
		private const uint DefaultKeyLength = 2048u;
		private SafeArrayHandle extensionsArrayHandle = SafeArrayHandle.InvalidHandle;
		private SafeHGlobalHandle encodedData = SafeHGlobalHandle.InvalidHandle;
		private SafeArrayHandle enhancedKeyUsageUsageIdentifierArrayHandle = SafeArrayHandle.InvalidHandle;
		private SafeHGlobalHandle enhancedKeyUsageEncodedDataHandle = SafeHGlobalHandle.InvalidHandle;
		private SafeCoTaskMemStringHandle enhancedKeyUsageOidHandle;
		private SafeCoTaskMemStringHandle basicConstraintOidHandle;
		public string CertificateSubjectDistinguishedName
		{
			get;
			private set;
		}
		public uint KeyLength
		{
			get;
			set;
		}
		private CryptoHelper(string certificateSubjectDistinguishedName)
		{
			this.CertificateSubjectDistinguishedName = certificateSubjectDistinguishedName;
			this.KeyLength = 2048u;
		}
		internal static byte[] CreateX509CertificateBlob(string subjectDistinguishedName, uint keyLength = 2048u, System.Security.SecureString password = null)
		{
			byte[] result;
			using (CryptoHelper cryptoHelper = new CryptoHelper(subjectDistinguishedName)
			{
				KeyLength = keyLength
			})
			{
				result = cryptoHelper.CreateX509CertificateBlob(password);
			}
			return result;
		}
		internal static string EncodeDistinguishedNameComponent(string component)
		{
			if (string.IsNullOrEmpty(component))
			{
				throw (component == null) ? new System.ArgumentNullException("component") : new System.ArgumentException("The DN component cannot be an empty string.", "component");
			}
			char[] anyOf = new char[]
			{
				',',
				'=',
				'\n',
				'+',
				'<',
				'>',
				'#',
				';',
				'"'
			};
			string text = string.Empty;
			if (component.IndexOfAny(anyOf) >= 0 || component.StartsWith(" ", System.StringComparison.Ordinal) || component.EndsWith(" ", System.StringComparison.Ordinal) || component.Contains("  "))
			{
				text = "\"";
			}
			return text + component.Replace("\"", "\"\"") + text;
		}
		internal static string CreateSubjectFromPublisherName(string publisherName)
		{
			string str = CryptoHelper.EncodeDistinguishedNameComponent(publisherName);
			string subjectDistinguishedName = "CN=" + str;
			return CryptoHelper.CanonicalizeSubject(subjectDistinguishedName);
		}
		private static string CanonicalizeSubject(string subjectDistinguishedName)
		{
			SafeCryptMemAllocHandle invalidHandle = SafeCryptMemAllocHandle.InvalidHandle;
			NativeMethods.CRYPT_DATA_BLOB cRYPT_DATA_BLOB = default(NativeMethods.CRYPT_DATA_BLOB);
			string result;
			try
			{
				CryptoHelper.CertStrToName(subjectDistinguishedName, out invalidHandle, out cRYPT_DATA_BLOB);
				result = CryptoHelper.CertNameToStr(ref cRYPT_DATA_BLOB);
			}
			finally
			{
				CryptoHelper.DisposeSafeHandle<SafeCryptMemAllocHandle>(ref invalidHandle);
			}
			return result;
		}
		private static void CertStrToName(string subjectDistinguishedName, out SafeCryptMemAllocHandle cryptMemHandle, out NativeMethods.CRYPT_DATA_BLOB certNameBlob)
		{
			cryptMemHandle = SafeCryptMemAllocHandle.InvalidHandle;
			certNameBlob = default(NativeMethods.CRYPT_DATA_BLOB);
			uint num = 0u;
			if (!NativeMethods.CertStrToName(1u, subjectDistinguishedName, 3u, System.IntPtr.Zero, System.IntPtr.Zero, ref num, System.IntPtr.Zero))
			{
				throw new Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
			}
			cryptMemHandle = NativeMethods.CryptMemAlloc(num);
			certNameBlob.cbData = num;
			certNameBlob.pbData = cryptMemHandle.DangerousGetHandle();
			if (!NativeMethods.CertStrToName(1u, subjectDistinguishedName, 3u, System.IntPtr.Zero, certNameBlob.pbData, ref certNameBlob.cbData, System.IntPtr.Zero))
			{
				throw new Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());
			}
		}
		private static string CertNameToStr(ref NativeMethods.CRYPT_DATA_BLOB certNameBlob)
		{
			uint capacity = NativeMethods.CertNameToStr(1u, ref certNameBlob, 3u, null, 0u);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder((int)capacity);
			capacity = NativeMethods.CertNameToStr(1u, ref certNameBlob, 3u, stringBuilder, (uint)stringBuilder.Capacity);
			return stringBuilder.ToString();
		}
		private static string GetManagedString(System.Security.SecureString str)
		{
			if (str == null)
			{
				return string.Empty;
			}
			System.IntPtr intPtr = System.IntPtr.Zero;
			string result;
			try
			{
				intPtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(str);
				result = System.Runtime.InteropServices.Marshal.PtrToStringUni(intPtr);
			}
			finally
			{
				System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
			}
			return result;
		}
		private static void DisposeSafeHandle<T>(ref T safeHandle) where T : System.Runtime.InteropServices.SafeHandle
		{
			if (safeHandle != null && !safeHandle.IsInvalid)
			{
				safeHandle.Dispose();
				safeHandle = default(T);
			}
		}
		private static void FreeGCHandle(System.Runtime.InteropServices.GCHandle blobHandle)
		{
			if (blobHandle.IsAllocated)
			{
				blobHandle.Free();
			}
		}
		private byte[] CreateX509CertificateBlob(System.Security.SecureString password = null)
		{
			SafeCryptProviderContextHandle invalidHandle = SafeCryptProviderContextHandle.InvalidHandle;
			SafeCryptMemAllocHandle invalidHandle2 = SafeCryptMemAllocHandle.InvalidHandle;
			SafeCryptMemAllocHandle safeCryptMemAllocHandle = SafeCryptMemAllocHandle.InvalidHandle;
			SafeCertContextHandle safeCertContextHandle = SafeCertContextHandle.InvalidHandle;
			SafeCryptKeyHandle invalidHandle3 = SafeCryptKeyHandle.InvalidHandle;
			SafeHGlobalHandle safeHGlobalHandle = SafeHGlobalHandle.InvalidHandle;
			SafeCertStoreHandle safeCertStoreHandle = SafeCertStoreHandle.InvalidHandle;
			SafeHGlobalHandle safeHGlobalHandle2 = SafeHGlobalHandle.InvalidHandle;
			NativeMethods.CRYPT_DATA_BLOB cRYPT_DATA_BLOB = default(NativeMethods.CRYPT_DATA_BLOB);
			NativeMethods.CRYPT_DATA_BLOB cRYPT_DATA_BLOB2 = default(NativeMethods.CRYPT_DATA_BLOB);
			NativeMethods.CRYPT_ALGORITHM_IDENTIFIER cRYPT_ALGORITHM_IDENTIFIER = default(NativeMethods.CRYPT_ALGORITHM_IDENTIFIER);
			System.Runtime.InteropServices.GCHandle blobHandle = default(System.Runtime.InteropServices.GCHandle);
			System.Runtime.InteropServices.GCHandle blobHandle2 = default(System.Runtime.InteropServices.GCHandle);
			byte[] result;
			try
			{
				CryptoHelper.CertStrToName(this.CertificateSubjectDistinguishedName, out invalidHandle2, out cRYPT_DATA_BLOB);
				if (!NativeMethods.CryptAcquireContext(out invalidHandle, System.Guid.NewGuid().ToString(), null, 1u, 8u))
				{
					throw new Win32Exception();
				}
				if (!NativeMethods.CryptGenKey(invalidHandle, 2u, this.KeyLength << 16 | 1u, out invalidHandle3))
				{
					throw new Win32Exception();
				}
				safeHGlobalHandle = new SafeHGlobalHandle(System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi("1.2.840.113549.1.1.11"));
				cRYPT_ALGORITHM_IDENTIFIER.pszObjId = safeHGlobalHandle.DangerousGetHandle();
				cRYPT_ALGORITHM_IDENTIFIER.Parameters.cbData = 0u;
				cRYPT_ALGORITHM_IDENTIFIER.Parameters.pbData = System.IntPtr.Zero;
				blobHandle = System.Runtime.InteropServices.GCHandle.Alloc(cRYPT_DATA_BLOB, System.Runtime.InteropServices.GCHandleType.Pinned);
				blobHandle2 = System.Runtime.InteropServices.GCHandle.Alloc(cRYPT_ALGORITHM_IDENTIFIER, System.Runtime.InteropServices.GCHandleType.Pinned);
				NativeMethods.CERT_EXTENSIONS signingCertificateExtensions = this.GetSigningCertificateExtensions();
				safeCertContextHandle = NativeMethods.CertCreateSelfSignCertificate(invalidHandle, blobHandle.AddrOfPinnedObject(), 0u, System.IntPtr.Zero, blobHandle2.AddrOfPinnedObject(), System.IntPtr.Zero, System.IntPtr.Zero, ref signingCertificateExtensions);
				if (safeCertContextHandle.IsInvalid)
				{
					throw new Win32Exception();
				}
				safeCertStoreHandle = NativeMethods.CertOpenStore(NativeMethods.CERT_STORE_PROV_MEMORY, 0u, System.IntPtr.Zero, 8192u, System.IntPtr.Zero);
				if (safeCertStoreHandle.IsInvalid)
				{
					throw new Win32Exception();
				}
				if (!NativeMethods.CertAddCertificateContextToStore(safeCertStoreHandle.DangerousGetHandle(), safeCertContextHandle.DangerousGetHandle(), 1u, System.IntPtr.Zero))
				{
					throw new Win32Exception();
				}
				safeHGlobalHandle2 = new SafeHGlobalHandle(System.Runtime.InteropServices.Marshal.AllocHGlobal(System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethods.CRYPT_DATA_BLOB))));
				if (safeHGlobalHandle2.IsInvalid)
				{
					throw new Win32Exception();
				}
				System.Runtime.InteropServices.Marshal.StructureToPtr(cRYPT_DATA_BLOB2, safeHGlobalHandle2.DangerousGetHandle(), true);
				if (!NativeMethods.PFXExportCertStoreEx(safeCertStoreHandle.DangerousGetHandle(), safeHGlobalHandle2.DangerousGetHandle(), CryptoHelper.GetManagedString(password), System.IntPtr.Zero, 4u))
				{
					throw new Win32Exception();
				}
				cRYPT_DATA_BLOB2 = (NativeMethods.CRYPT_DATA_BLOB)System.Runtime.InteropServices.Marshal.PtrToStructure(safeHGlobalHandle2.DangerousGetHandle(), typeof(NativeMethods.CRYPT_DATA_BLOB));
				safeCryptMemAllocHandle = NativeMethods.CryptMemAlloc(cRYPT_DATA_BLOB2.cbData);
				if (safeCryptMemAllocHandle.IsInvalid)
				{
					throw new Win32Exception();
				}
				cRYPT_DATA_BLOB2.pbData = safeCryptMemAllocHandle.DangerousGetHandle();
				System.Runtime.InteropServices.Marshal.StructureToPtr(cRYPT_DATA_BLOB2, safeHGlobalHandle2.DangerousGetHandle(), true);
				if (!NativeMethods.PFXExportCertStoreEx(safeCertStoreHandle.DangerousGetHandle(), safeHGlobalHandle2.DangerousGetHandle(), CryptoHelper.GetManagedString(password), System.IntPtr.Zero, 4u))
				{
					throw new Win32Exception();
				}
				byte[] array = new byte[cRYPT_DATA_BLOB2.cbData];
				System.Runtime.InteropServices.Marshal.Copy(cRYPT_DATA_BLOB2.pbData, array, 0, (int)cRYPT_DATA_BLOB2.cbData);
				result = array;
			}
			finally
			{
				CryptoHelper.DisposeSafeHandle<SafeCertStoreHandle>(ref safeCertStoreHandle);
				CryptoHelper.DisposeSafeHandle<SafeCryptMemAllocHandle>(ref safeCryptMemAllocHandle);
				CryptoHelper.DisposeSafeHandle<SafeHGlobalHandle>(ref safeHGlobalHandle);
				CryptoHelper.DisposeSafeHandle<SafeHGlobalHandle>(ref safeHGlobalHandle2);
				CryptoHelper.FreeGCHandle(blobHandle);
				CryptoHelper.FreeGCHandle(blobHandle2);
				CryptoHelper.DisposeSafeHandle<SafeCryptMemAllocHandle>(ref invalidHandle2);
				CryptoHelper.DisposeSafeHandle<SafeCertContextHandle>(ref safeCertContextHandle);
				CryptoHelper.DisposeSafeHandle<SafeCryptKeyHandle>(ref invalidHandle3);
				CryptoHelper.DisposeSafeHandle<SafeCryptProviderContextHandle>(ref invalidHandle);
			}
			return result;
		}
		private NativeMethods.CERT_EXTENSIONS GetSigningCertificateExtensions()
		{
			NativeMethods.CERT_EXTENSION basicConstraintExtension = this.GetBasicConstraintExtension(false, false, 0u);
			NativeMethods.CERT_EXTENSION enhancedKeyUsageExtension = this.GetEnhancedKeyUsageExtension();
			NativeMethods.CERT_EXTENSION[] array = new NativeMethods.CERT_EXTENSION[]
			{
				basicConstraintExtension,
				enhancedKeyUsageExtension
			};
			this.extensionsArrayHandle = SafeArrayHandle.FromArray<NativeMethods.CERT_EXTENSION>(array);
			NativeMethods.CERT_EXTENSIONS result;
			result.cExtension = (uint)array.Length;
			result.rgExtension = this.extensionsArrayHandle.DangerousGetHandle();
			return result;
		}
		private NativeMethods.CERT_EXTENSION GetBasicConstraintExtension(bool ca, bool hasPathLenConstraint, uint pathLenConstraint)
		{
			NativeMethods.CERT_EXTENSION result = default(NativeMethods.CERT_EXTENSION);
			NativeMethods.CERT_BASIC_CONSTRAINTS2_INFO cERT_BASIC_CONSTRAINTS2_INFO = default(NativeMethods.CERT_BASIC_CONSTRAINTS2_INFO);
			cERT_BASIC_CONSTRAINTS2_INFO.fCA = System.Convert.ToInt32(ca);
			cERT_BASIC_CONSTRAINTS2_INFO.fPathLenConstraint = System.Convert.ToInt32(hasPathLenConstraint);
			cERT_BASIC_CONSTRAINTS2_INFO.dwPathLenConstraint = (hasPathLenConstraint ? pathLenConstraint : 0u);
			uint cbData = 0u;
			if (!NativeMethods.CryptEncodeObjectEx(65537u, "2.5.29.19", ref cERT_BASIC_CONSTRAINTS2_INFO, 32768u, System.IntPtr.Zero, out this.encodedData, ref cbData))
			{
				throw new Win32Exception();
			}
			this.basicConstraintOidHandle = new SafeCoTaskMemStringHandle("2.5.29.19", false);
			result.pszObjId = this.basicConstraintOidHandle.DangerousGetHandle();
			result.fCritical = System.Convert.ToInt32(true);
			result.Value.cbData = cbData;
			result.Value.pbData = this.encodedData.DangerousGetHandle();
			return result;
		}
		private NativeMethods.CERT_EXTENSION GetEnhancedKeyUsageExtension()
		{
			NativeMethods.CERT_EXTENSION result = default(NativeMethods.CERT_EXTENSION);
			NativeMethods.CTL_USAGE cTL_USAGE = default(NativeMethods.CTL_USAGE);
			string[] array = new string[]
			{
				"1.3.6.1.5.5.7.3.3"
			};
			this.enhancedKeyUsageUsageIdentifierArrayHandle = SafeArrayHandle.FromArray(array);
			cTL_USAGE.cUsageIdentifier = (uint)array.Length;
			cTL_USAGE.rgpszUsageIdentifier = this.enhancedKeyUsageUsageIdentifierArrayHandle.DangerousGetHandle();
			uint cbData = 0u;
			if (!NativeMethods.CryptEncodeObjectEx(65537u, "2.5.29.37", ref cTL_USAGE, 32768u, System.IntPtr.Zero, out this.enhancedKeyUsageEncodedDataHandle, ref cbData))
			{
				throw new Win32Exception();
			}
			this.enhancedKeyUsageOidHandle = new SafeCoTaskMemStringHandle("2.5.29.37", false);
			result.pszObjId = this.enhancedKeyUsageOidHandle.DangerousGetHandle();
			result.fCritical = System.Convert.ToInt32(true);
			result.Value.cbData = cbData;
			result.Value.pbData = this.enhancedKeyUsageEncodedDataHandle.DangerousGetHandle();
			return result;
		}
		public void Dispose()
		{
			CryptoHelper.DisposeSafeHandle<SafeCoTaskMemStringHandle>(ref this.enhancedKeyUsageOidHandle);
			CryptoHelper.DisposeSafeHandle<SafeArrayHandle>(ref this.extensionsArrayHandle);
			CryptoHelper.DisposeSafeHandle<SafeHGlobalHandle>(ref this.encodedData);
			CryptoHelper.DisposeSafeHandle<SafeHGlobalHandle>(ref this.enhancedKeyUsageEncodedDataHandle);
			CryptoHelper.DisposeSafeHandle<SafeArrayHandle>(ref this.enhancedKeyUsageUsageIdentifierArrayHandle);
		}
	}
}
