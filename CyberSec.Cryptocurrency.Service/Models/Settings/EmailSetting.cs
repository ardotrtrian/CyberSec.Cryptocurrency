namespace CyberSec.Cryptocurrency.Service.Models.Settings;

public class EmailSetting
{
    public string EmailFrom { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = string.Empty;
    public string SmtpPass { get; set; } = string.Empty;

}