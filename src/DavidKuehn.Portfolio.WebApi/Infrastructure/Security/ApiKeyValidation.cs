namespace DavidKuehn.Portfolio.WebApi.Infrastructure.Security
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        public bool IsApiKeyValid(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;

            string? apiKey = Environment.GetEnvironmentVariable(Constants.ApiKeyEnivironmentVariableName);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException($"Environment variable '{Constants.ApiKeyEnivironmentVariableName}' is not set.");
            }

            if (userApiKey.Equals(apiKey, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
