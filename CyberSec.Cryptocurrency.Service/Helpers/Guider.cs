namespace CyberSec.Cryptocurrency.Service.Helpers;

internal class Guider
{
    public static string Generate()
    {
        return Guid.NewGuid().ToString();
    }
}