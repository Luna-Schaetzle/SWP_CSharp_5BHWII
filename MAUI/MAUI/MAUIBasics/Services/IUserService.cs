using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIBasics.Services
{
    // Services/IUserService.cs
public interface IUserService
{
    event EventHandler<User> UserChanged;
    User CurrentUser { get; set; }
    bool IsLoggedIn { get; }
}

public class UserService : IUserService
{
    public event EventHandler<User> UserChanged;
    
    private User _currentUser;
    
    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            IsLoggedIn = value != null;
            UserChanged?.Invoke(this, value);
        }
    }
    
    public bool IsLoggedIn { get; private set; }
}
}
