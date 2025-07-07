namespace Coursedee.Application.Models;

public interface IUserContext
{
    long UserId { get; }
    string AccessToken { get; }
}

public class UserContext : IUserContext
{
    public long UserId { get; set; }
    public string AccessToken { get; set; }
}