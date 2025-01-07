// ViewModels/CartPageViewModel.cs
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
        private readonly CartService _cartService;

        [ObservableProperty]
        private ObservableCollection<CartItem> cart;

        [ObservableProperty]
        private decimal totalPrice;

        public CartPageViewModel(CartService cartService)
        {
            _cartService = cartService;
            Cart = _cartService.Cart;
            CalculateTotalPrice();
        }

        // Berechnung der Gesamtsumme
        private void CalculateTotalPrice()
        {
            TotalPrice = Cart.Sum(item => item.Article.Price * item.Quantity);
        }

        // Aktualisiere die Gesamtsumme, wenn sich der Warenkorb ändert
        partial void OnCartChanged(ObservableCollection<CartItem> value)
        {
            CalculateTotalPrice();
        }

        // Command zum Entfernen eines Artikels aus dem Warenkorb
        [RelayCommand]
        private async Task RemoveFromCartAsync(CartItem cartItem)
        {
            if (cartItem == null)
                return;

            await _cartService.RemoveFromCartAsync(cartItem);
            CalculateTotalPrice();
            await Application.Current.MainPage.DisplayAlert("Erfolg", $"{cartItem.Article.Name} wurde aus dem Warenkorb entfernt.", "OK");
        }

        // Command zum Leeren des Warenkorbs
        [RelayCommand]
        private async Task ClearCartAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Bestätigung", "Möchtest du den gesamten Warenkorb leeren?", "Ja", "Nein");
            if (confirm)
            {
                await _cartService.ClearCartAsync();
                CalculateTotalPrice();
                await Application.Current.MainPage.DisplayAlert("Erfolg", "Der Warenkorb wurde geleert.", "OK");
            }
        }
    }
}
