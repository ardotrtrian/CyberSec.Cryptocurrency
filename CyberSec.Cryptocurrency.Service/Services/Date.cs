using CyberSec.Cryptocurrency.Service.Interfaces;

namespace CyberSec.Cryptocurrency.Service.Services;

public class Date : IDate
{
    public DateTime Now => DateTime.UtcNow;

    public DateTime AddDays(double value)
    {
        return Now.AddDays(value);
    }

    public DateTime AddMinutes(double value)
    {
        return Now.AddMinutes(value);
    }
}