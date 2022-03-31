namespace CyberSec.Cryptocurrency.Service.Interfaces;

public interface IDate
{
    public DateTime Now { get; }
    public DateTime AddDays(double value);
    public DateTime AddMinutes(double value);
}