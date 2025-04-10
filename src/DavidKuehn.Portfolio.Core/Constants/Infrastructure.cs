namespace DavidKuehn.Portfolio.Core.Constants
{
    /// <summary>
    /// Constants for infrastructure.
    /// </summary>
    public static class Infrastructure
    {
        /// <summary>
        /// Name of the environment variable that contains the connection string to the security database.
        /// </summary>
        public static string SecurityDatabaseConnectionStringName = "SECURITY_DATABASE_CONNECTION_STRING";
        
        /// <summary>
        /// Name of the environment variable that contains the connection string to the career database.
        /// </summary>
        public static string CareerDatabaseConnectionStringName = "CAREER_DATABASE_CONNECTION_STRING";
        
        /// <summary>
        /// Name of the environment variable that contains the number of minutes a JWT is alive for.
        /// </summary>
        public static string JwtExpiryInMinutesName = "PORTFOLIO_JWT_EXPIRY_MINUTES";
        
        /// <summary>
        /// Name of the environment variable that contains the secret key for JWT signing.
        /// </summary>
        public static string JwtSecretKey = "PORTFOLIO_JWT_SECRET_KEY";
    }
}
