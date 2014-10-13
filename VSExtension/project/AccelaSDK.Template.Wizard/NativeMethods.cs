using System;
using System.Runtime.InteropServices;
using System.Text;
namespace Accela.Mobile.CustomWizard
{
	internal static class NativeMethods
	{
		public struct CRYPT_DATA_BLOB
		{
			public uint cbData;
			public System.IntPtr pbData;
		}
		public struct CRYPT_ALGORITHM_IDENTIFIER
		{
			public System.IntPtr pszObjId;
			public NativeMethods.CRYPT_DATA_BLOB Parameters;
		}
		public struct CERT_EXTENSION
		{
			public System.IntPtr pszObjId;
			public int fCritical;
			public NativeMethods.CRYPT_DATA_BLOB Value;
		}
		public struct CERT_EXTENSIONS
		{
			public uint cExtension;
			public System.IntPtr rgExtension;
		}
		public struct CERT_BASIC_CONSTRAINTS2_INFO
		{
			public int fCA;
			public int fPathLenConstraint;
			public uint dwPathLenConstraint;
		}
		public struct CTL_USAGE
		{
			public uint cUsageIdentifier;
			public System.IntPtr rgpszUsageIdentifier;
		}
		public const uint AT_SIGNATURE = 2u;
		public const uint CRYPT_EXPORTABLE = 1u;
		public const uint CRYPT_NEWKEYSET = 8u;
		public const uint CRYPT_DELETEKEYSET = 16u;
		public const uint PROV_RSA_FULL = 1u;
		public const int X509_ASN_ENCODING = 1;
		public const int PKCS_7_ASN_ENCODING = 65536;
		public const int CERT_X500_NAME_STR = 3;
		public const uint EXPORT_PRIVATE_KEYS = 4u;
		public const uint PKCS12_INCLUDE_EXTENDED_PROPERTIES = 16u;
		public const uint CRYPT_ENCODE_ALLOC_FLAG = 32768u;
		public const string szOID_RSA_SHA256RSA = "1.2.840.113549.1.1.11";
		public const string IdKpCodeSigning = "1.3.6.1.5.5.7.3.3";
		public const string szOID_BASIC_CONSTRAINTS2 = "2.5.29.19";
		public const string szOID_ENHANCED_KEY_USAGE = "2.5.29.37";
		public const uint CERT_STORE_CREATE_NEW_FLAG = 8192u;
		public const uint CERT_CLOSE_STORE_FORCE_FLAG = 1u;
		public const uint CERT_STORE_ADD_NEW = 1u;
		public static System.IntPtr CERT_STORE_PROV_MEMORY = new System.IntPtr(2);
		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptAcquireContext(out SafeCryptProviderContextHandle hProv, [System.Runtime.InteropServices.In] string pszContainer, [System.Runtime.InteropServices.In] string pszProvider, [System.Runtime.InteropServices.In] uint dwProvType, [System.Runtime.InteropServices.In] uint dwFlags);
		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptReleaseContext([System.Runtime.InteropServices.In] System.IntPtr hProv, [System.Runtime.InteropServices.In] uint dwFlags);
		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptGenKey(SafeCryptProviderContextHandle hProv, uint Algid, uint dwFlags, out SafeCryptKeyHandle phKey);
		[System.Runtime.InteropServices.DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptDestroyKey(System.IntPtr hKey);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeCryptMemAllocHandle CryptMemAlloc(uint cbSize);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern void CryptMemFree(System.IntPtr pv);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CertStrToName([System.Runtime.InteropServices.In] uint dwCertEncodingType, [System.Runtime.InteropServices.In] string pszX500, [System.Runtime.InteropServices.In] uint dwStrType, [System.Runtime.InteropServices.In] System.IntPtr pvReserved , [System.Runtime.InteropServices.Out] System.IntPtr pbEncoded, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] ref uint pcbEncoded, [System.Runtime.InteropServices.Out] System.IntPtr ppszError = default(System.IntPtr));
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode)]
		public static extern uint CertNameToStr([System.Runtime.InteropServices.In] uint dwCertEncodingType, [System.Runtime.InteropServices.In] ref NativeMethods.CRYPT_DATA_BLOB pName, [System.Runtime.InteropServices.In] uint dwStrType, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] [System.Runtime.InteropServices.Out] System.Text.StringBuilder psz, [System.Runtime.InteropServices.In] uint csz);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern SafeCertContextHandle CertCreateSelfSignCertificate([System.Runtime.InteropServices.In] SafeCryptProviderContextHandle hProv, [System.Runtime.InteropServices.In] System.IntPtr pSubjectIssuerBlob, [System.Runtime.InteropServices.In] uint dwFlags, [System.Runtime.InteropServices.In] System.IntPtr pKeyProvInfo, [System.Runtime.InteropServices.In] System.IntPtr pSignatureAlgorithm, [System.Runtime.InteropServices.In] System.IntPtr pStartTime, [System.Runtime.InteropServices.In] System.IntPtr pEndTime, [System.Runtime.InteropServices.In] ref NativeMethods.CERT_EXTENSIONS pExtensions);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", BestFitMapping = false, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptEncodeObjectEx([System.Runtime.InteropServices.In] uint dwCertEncodingType, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)] [System.Runtime.InteropServices.In] string lpszStructType, [System.Runtime.InteropServices.In] ref NativeMethods.CERT_BASIC_CONSTRAINTS2_INFO pvStructInfo, [System.Runtime.InteropServices.In] uint dwFlags, [System.Runtime.InteropServices.In] System.IntPtr pEncodePara, out SafeHGlobalHandle pvEncoded, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] ref uint pcbEncoded);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", BestFitMapping = false, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CryptEncodeObjectEx([System.Runtime.InteropServices.In] uint dwCertEncodingType, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPStr)] [System.Runtime.InteropServices.In] string lpszStructType, [System.Runtime.InteropServices.In] ref NativeMethods.CTL_USAGE pvStructInfo, [System.Runtime.InteropServices.In] uint dwFlags, [System.Runtime.InteropServices.In] System.IntPtr pEncodePara, out SafeHGlobalHandle pvEncoded, [System.Runtime.InteropServices.In] [System.Runtime.InteropServices.Out] ref uint pcbEncoded);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CertFreeCertificateContext(System.IntPtr pCertContext);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", SetLastError = true)]
		public static extern SafeCertStoreHandle CertOpenStore(System.IntPtr lpszStoreProvider, uint dwMsgAndCertEncodingType, System.IntPtr hCryptProv, uint dwFlags, System.IntPtr pvPara);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CertCloseStore(System.IntPtr hCertStore, uint dwFlags);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool CertAddCertificateContextToStore(System.IntPtr hCertStore, System.IntPtr pCertContext, uint dwAddDisposition, System.IntPtr ppStoreContext);
		[System.Runtime.InteropServices.DllImport("crypt32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool PFXExportCertStoreEx(System.IntPtr hStore, System.IntPtr pPFX, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string password, System.IntPtr pvReserved, uint dwFlags);
	}
}
