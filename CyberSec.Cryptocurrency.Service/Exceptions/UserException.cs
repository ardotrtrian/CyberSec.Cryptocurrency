namespace CyberSec.Cryptocurrency.Service.Exceptions;

public class UserException : Exception
{
    public UserException() : base() { }
    
    public UserException(string message) : base(message) { }
}