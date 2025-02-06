using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using Microsoft.Maui.Controls;
using MAUIBasics.Services; // Add this line if IUserService is in the MAUIBasics.Services namespace

namespace MAUIBasics.ViewModels
{
    public partial class ShopPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        private const string ApiUrlArticles = "https://localhost:7243/api/shop/articles";
        private const string ApiUrlBasket = "https://localhost:7243/api/shop/basket";

        [ObservableProperty]
        private ObservableCollection<Article> articles;

        [ObservableProperty]
        private ObservableCollection<Basket> basket;

        [ObservableProperty]
        private bool isUserLoggedIn;

        public ShopPageViewModel(IUserService userService)
        {
            _userService = userService;
            Articles = new ObservableCollection<Article>();
            Basket = new ObservableCollection<Basket>();
            _httpClient = new HttpClient();
            
            IsUserLoggedIn = _userService.IsLoggedIn;
            
            // Auf Änderungen des Login-Status reagieren
            if (_userService is UserService service)
            {
                service.UserChanged += (s, user) =>
                {
                    IsUserLoggedIn = user != null;
                    LoadBasketAsync().ConfigureAwait(false);
                };
            }
            
            LoadArticlesAsync();
        }

        [RelayCommand]
        private async Task AddToCartAsync(Article selectedArticle)
        {
            if (!_userService.IsLoggedIn)
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte melden Sie sich zuerst an.", "OK");
                return;
            }

            if (selectedArticle == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Kein Artikel ausgewählt.", "OK");
                return;
            }

            try
            {
                var basketItem = new Basket
                {
                    BasketId = 0,
                    user = _userService.CurrentUser, // Aktueller User aus dem UserService
                    article = selectedArticle
                };

                var response = await _httpClient.PostAsJsonAsync($"{ApiUrlBasket}?apiKey={ApiKey}", basketItem);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Erfolg", $"{selectedArticle.Name} wurde zum Warenkorb hinzugefügt.", "OK");
                    await LoadBasketAsync(); // Warenkorb neu laden
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Hinzufügen des Artikels zum Warenkorb.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task LoadBasketAsync()
        {
            if (!_userService.IsLoggedIn)
            {
                Basket.Clear();
                return;
            }

            try
            {
                var userId = _userService.CurrentUser.Id;
                var response = await _httpClient.GetAsync($"{ApiUrlBasket}/{userId}?apiKey={ApiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var basketList = await response.Content.ReadFromJsonAsync<List<Basket>>();
                    if (basketList != null)
                    {
                        Basket = new ObservableCollection<Basket>(basketList);
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

        [RelayCommand]
        private async Task BuyBasketAsync()
        {
            if (!_userService.IsLoggedIn)
            {
                await Shell.Current.DisplayAlert("Fehler", "Bitte melden Sie sich zuerst an.", "OK");
                return;
            }

            try
            {
                var userId = _userService.CurrentUser.Id;
                var response = await _httpClient.PostAsync($"{ApiUrlBasket}/buy/{userId}?apiKey={ApiKey}", null);

                if (response.IsSuccessStatusCode)
                {
                    Basket.Clear();
                    await Shell.Current.DisplayAlert("Erfolg", "Warenkorb erfolgreich gekauft.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Kaufen des Warenkorbs.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }

        private async Task LoadArticlesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrlArticles}?apiKey={ApiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var articlesList = await response.Content.ReadFromJsonAsync<List<Article>>();
                    if (articlesList != null)
                    {
                        Articles = new ObservableCollection<Article>(articlesList);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Laden der Artikel.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }
    }
}