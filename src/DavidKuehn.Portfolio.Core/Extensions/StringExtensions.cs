using System;
using System.Security;

namespace DavidKuehn.Portfolio.Core.Extensions
{
    /// <summary>
    /// All things to do to strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to a readonly secure string. Github Copilot generated this code.
        /// </summary>
        /// <param name="source">The string to convert to a secure string. Cannot be null or empty.</param>
        /// <returns>A readonly <see cref="SecureString"/> containing the source string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source parameter is null or empty.</exception>
        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            var secureString = new SecureString();
            foreach (var c in source)
            {
                secureString.AppendChar(c);
            }

            secureString.MakeReadOnly();
            return secureString;
        }
    }
}
