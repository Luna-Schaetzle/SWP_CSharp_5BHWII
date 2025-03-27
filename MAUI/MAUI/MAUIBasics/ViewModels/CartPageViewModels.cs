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

        private User user;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private ObservableCollection<CartItem> cart;

        [ObservableProperty]
        private decimal totalPrice;

        [ObservableProperty]
        private int _quantity;

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
            await _cartService.LoadCartAsync(_userService.CurrentUser);
            Cart = _cartService.Cart;
            Quantity = Cart.Count;
            CalculateTotalPrice();
        }

        // Gesamtsumme berechnen
        private void CalculateTotalPrice()
        {
            TotalPrice = Cart.Sum(item => item.Article.Price * item.Quantity);
        }

        // Artikel aus dem Warenkorb entfernen
        [RelayCommand]
        private async Task RemoveFromCart(CartItem cartItem)
        {
            /*var founduser = Cart.FirstOrDefault(c => c.user.Id == _userService.CurrentUser.Id);
            if (founduser == null) return;
            else
            {
                await _cartService.RemoveFromCartAsync(cartItem);
                CalculateTotalPrice();
            }*/
            //await Shell.Current.DisplayAlert("Info", $"Artikel {cartItem.Article.Name} entfernt", "OK");
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

        [RelayCommand]
        private async Task CalculateTotalPriceButton()
        {
            CalculateTotalPrice();
        }

        [RelayCommand]
        private async Task UpdateQuantity(CartItem cartItem){
            await _cartService.UpdateQuantityAsync(cartItem);
            CalculateTotalPrice();
        }

        [RelayCommand]
        private async Task Checkout()
        {
            await Shell.Current.DisplayAlert("Info", "Checkout", "OK");
        }
    }
}
