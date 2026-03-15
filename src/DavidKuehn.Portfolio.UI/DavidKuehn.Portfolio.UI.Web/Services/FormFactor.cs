using DavidKuehn.Portfolio.UI.Shared.Services;

namespace DavidKuehn.Portfolio.UI.Web.Services;

public class FormFactor : IFormFactor
{
    public string GetFormFactor()
    {
        return "Web";
    }

    public string GetPlatform()
    {
        return Environment.OSVersion.ToString();
    }
}
