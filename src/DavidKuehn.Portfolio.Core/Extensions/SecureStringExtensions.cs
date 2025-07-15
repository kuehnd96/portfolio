using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DavidKuehn.Portfolio.Core.Extensions
{
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Converts a secure string to an unsecure string.
        /// </summary>
        /// <param name="secureString">The <see cref="SecureString">secure string</see> to convert. Cannot be null.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if the secureString parameter is null.</exception>
        public static string ConvertToUnsecureString(this SecureString secureString)
        {
            if (secureString == null)
            {
                throw new ArgumentNullException(nameof(secureString));
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                // Convert SecureString to an unmanaged string
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString) ?? string.Empty;
            }
            finally
            {
                // Free the unmanaged memory
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
