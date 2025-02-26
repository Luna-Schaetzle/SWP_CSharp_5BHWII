using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using Microsoft.Maui.Controls;
using MAUIBasics.Services;

namespace MAUIBasics.ViewModels
{
    public partial class ShopPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        private const string ApiUrlArticles = "https://localhost:7243/api/shop/articles";

        [ObservableProperty]
        private ObservableCollection<Article> articles;

        [ObservableProperty]
        private ObservableCollection<CartItem> basket;

        [ObservableProperty]
        private bool isUserLoggedIn;

        public ShopPageViewModel(IUserService userService, ICartService cartService)
        {
            _userService = userService;
            _cartService = cartService;
            _httpClient = new HttpClient();
            Articles = new ObservableCollection<Article>();
            Basket = new ObservableCollection<CartItem>();

            IsUserLoggedIn = _userService.IsLoggedIn;

            if (_userService is UserService service)
            {
                service.UserChanged += async (s, user) =>
                {
                    IsUserLoggedIn = user != null;
                };
            }

            // Parallel: Artikel abrufen & Warenkorb laden
            Task.Run(async () => await Task.WhenAll(LoadArticlesAsync()));
        }

        // Artikel in den lokalen Warenkorb speichern
        [RelayCommand]
        private async Task AddToCartAsync(Article selectedArticle)
        {
            await Shell.Current.DisplayAlert("Info", "Artikel hinzugefügt", "OK");
            
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
                var user = _userService.CurrentUser;
                await _cartService.AddToCartAsync(selectedArticle, user, 1);
                await Shell.Current.DisplayAlert("Erfolg", $"{selectedArticle.Name} wurde zum Warenkorb hinzugefügt.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] AddToCartAsync: {ex.Message}");
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }

  

        // API-Artikel abrufen
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
                Console.WriteLine($"[ERROR] LoadArticlesAsync: {ex.Message}");
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }
    }
}