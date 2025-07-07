using Coursedee.Application.Models;

namespace Coursedee.Application.Services;

public interface IUserContextAccessor
{
    IUserContext Get();
    void Set(IUserContext userContext);
}

public class DefaultUserContextAccessor : IUserContextAccessor
{
    private readonly AsyncLocal<IUserContext> _currentUser = new();

    public DefaultUserContextAccessor()
    {
    }

    public IUserContext Get()
    {
        return _currentUser.Value;
    }

    public void Set(IUserContext userContext)
    {
        _currentUser.Value = userContext;
    }
}