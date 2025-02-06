using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using Microsoft.Maui.Controls;
using MAUIBasics.Services;

namespace MAUIBasics.ViewModels
{
    public partial class CartPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;


        [ObservableProperty]
        private string _username;

        //private int _userID;

        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        private const string ApiUrlBasket = "https://localhost:7243/api/shop/basket";

        [ObservableProperty]
        private ObservableCollection<Basket> cart;

        [ObservableProperty]
        private decimal totalPrice;

        public CartPageViewModel(IUserService userService)
        {
            //Username = string.Empty;

            Cart = new ObservableCollection<Basket>();

            _httpClient = new HttpClient();
            _userService = userService;

            new Task(async() => await LoadCartAsync()).Start();

            new Task(async () => await InitializeUser()).Start();



            if (_userService is UserService service)
            {
                service.UserChanged += OnUserChanged;
            }



        }

        /*
        private async void InitializeAsync()
        {
            await LoadCartAsync();
        }
        */

        private async void OnUserChanged(object? sender, User? e)
        {
            InitializeUser();
            await LoadCartAsync();
        }
        // Warenkorb laden
        private async Task LoadCartAsync()
        {
            try
            {
                //var userId =  "1";//await SecureStorage.GetAsync("userId");
                var userId = _userService.CurrentUser.Id;
                await Shell.Current.DisplayAlert("Benutzer ID", $"Ihre Benutzer ID ist: {userId}", "OK");
                if (string.IsNullOrEmpty(userId.ToString()))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Kein Benutzer angemeldet. Bitte zuerst einloggen.", "OK");
                    return;
                }

                var response = await _httpClient.GetAsync($"{ApiUrlBasket}/{userId}?apiKey={ApiKey}");
                if (response.IsSuccessStatusCode)
                {
                    var basketItems = await response.Content.ReadFromJsonAsync<List<Basket>>();
                    if (basketItems != null)
                    {
                        Cart = new ObservableCollection<Basket>(basketItems);
                        CalculateTotalPrice();
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Laden des Warenkorbs.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }

        private async Task InitializeUser()
        {
            if (_userService.IsLoggedIn && _userService.CurrentUser != null)
            {
                Console.WriteLine($"Initializing user with name: {_userService.CurrentUser.Name}");
                Username = _userService.CurrentUser.Name;
                //_userID = _userService.CurrentUser.Id;
            }
            else
            {
                Console.WriteLine("No user logged in, setting to Guest");
                Username = "Guest";
            }
        }

        // Gesamtsumme berechnen
        private void CalculateTotalPrice()
        {
            TotalPrice = Cart.Sum(item => item.article.Price);
        }

        // Artikel aus dem Warenkorb entfernen
        [RelayCommand]
        private async Task RemoveFromCartAsync(Basket basketItem)
        {
            try
            {
                var userId = "1";//await SecureStorage.GetAsync("userId");
                if (string.IsNullOrEmpty(userId))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Kein Benutzer angemeldet. Bitte zuerst einloggen.", "OK");
                    return;
                }

                var response = await _httpClient.DeleteAsync($"{ApiUrlBasket}/{userId}/{basketItem.article.ArticleId}?apiKey={ApiKey}");
                if (response.IsSuccessStatusCode)
                {
                    Cart.Remove(basketItem);
                    CalculateTotalPrice();
                    await Shell.Current.DisplayAlert("Erfolg", "Artikel wurde aus dem Warenkorb entfernt.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Entfernen des Artikels.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }

        // Warenkorb leeren
        [RelayCommand]
        private async Task ClearCartAsync()
        {
            try
            {
                var userId = await SecureStorage.GetAsync("userId");
                if (string.IsNullOrEmpty(userId))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Kein Benutzer angemeldet. Bitte zuerst einloggen.", "OK");
                    return;
                }

                var response = await _httpClient.PostAsync($"{ApiUrlBasket}/buy/{userId}?apiKey={ApiKey}", null);
                if (response.IsSuccessStatusCode)
                {
                    Cart.Clear();
                    CalculateTotalPrice();
                    await Shell.Current.DisplayAlert("Erfolg", "Warenkorb wurde geleert und Artikel gekauft.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Leeren des Warenkorbs.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }
    }
}
