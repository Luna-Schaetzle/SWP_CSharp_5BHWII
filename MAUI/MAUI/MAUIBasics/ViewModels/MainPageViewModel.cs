using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Services;
using System.ComponentModel;

namespace MAUIBasics.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IUserService _userService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private int _count;

        [ObservableProperty]
        private string _btnText = "Click me";

        public MainPageViewModel(IUserService userService)
        {
            _userService = userService;

            // Debug-Ausgabe
            Console.WriteLine($"MainPageViewModel initialized. IsLoggedIn: {_userService.IsLoggedIn}");

            InitializeUser();

            if (_userService is UserService service)
            {
                service.UserChanged += OnUserChanged;
            }
        }

        private void OnUserChanged(object sender, User user)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Console.WriteLine($"User changed event received. New username: {user?.Name ?? "null"}");
                Username = user?.Name ?? "Guest";
            });
        }

        private void InitializeUser()
        {
            if (_userService.IsLoggedIn && _userService.CurrentUser != null)
            {
                Console.WriteLine($"Initializing user with name: {_userService.CurrentUser.Name}");
                Username = _userService.CurrentUser.Name;
                //UserID = _userService.CurrentUser.Id;
            }
            else
            {
                Console.WriteLine("No user logged in, setting to Guest");
                Username = "Guest";
            }
        }
    }
}