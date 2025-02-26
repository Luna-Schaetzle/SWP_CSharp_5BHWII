using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using MAUIBasics.Services;
using Microsoft.Maui.Controls;

namespace MAUIBasics.ViewModels
{
    public partial class CartPageViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly ICartService _cartService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private ObservableCollection<CartItem> cart;

        [ObservableProperty]
        private decimal totalPrice;

        public CartPageViewModel(IUserService userService, ICartService cartService)
        {
            _userService = userService;
            _cartService = cartService;


            Cart = new ObservableCollection<CartItem>();
            LoadCart();
            InitializeUser();

            if (_userService is UserService service)
            {
                service.UserChanged += OnUserChanged;
            }
        }

        private async void OnUserChanged(object? sender, User? e)
        {
            await InitializeUser();
            LoadCart();
        }

        private async Task InitializeUser()
        {
            if (_userService.IsLoggedIn && _userService.CurrentUser != null)
            {
                Username = _userService.CurrentUser.Name;
            }
            else
            {
                Username = "Guest";
            }
        }

        // Warenkorb laden
        private async void LoadCart()
        {
            await _cartService.LoadCartAsync();
            Cart = _cartService.Cart;
            CalculateTotalPrice();
        }

        // Gesamtsumme berechnen
        private void CalculateTotalPrice()
        {
            TotalPrice = Cart.Sum(item => item.Article.Price * item.Quantity);
        }

        // Artikel aus dem Warenkorb entfernen
        [RelayCommand]
        private async Task RemoveFromCartAsync(CartItem cartItem)
        {
            await _cartService.RemoveFromCartAsync(cartItem);
            CalculateTotalPrice();
        }

        // Warenkorb leeren
        [RelayCommand]
        private async Task ClearCartAsync()
        {
            await _cartService.ClearCartAsync();
            CalculateTotalPrice();
        }
    }
}
