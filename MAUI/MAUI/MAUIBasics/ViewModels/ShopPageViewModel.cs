// ViewModels/ShopPageViewModel.cs
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using MAUIBasics.Services;
using Microsoft.Maui.Controls;

namespace MAUIBasics.ViewModels
{
    public partial class ShopPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly CartService _cartService;
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        private const string ApiUrl = "https://localhost:7243/api/shop/articles";
        private const string ApiUrlBasket = "https://localhost:7243/api/shop/";

        [ObservableProperty]
        private ObservableCollection<Article> articles;

        public ShopPageViewModel()
        {
            Articles = new ObservableCollection<Article>();
            //_cartService = cartService;
            _httpClient = new HttpClient();
            new Task(async () => await LoadArticlesAsync()).Start();
            //TODO: Task machen
        }

        // Command zum Hinzufügen eines Artikels zum Warenkorb
        [RelayCommand]
        private async Task AddToCartAsync(Article selectedArticle)
        {
            //einen Basket eintrag erstellen 
            Basket basket = new Basket();
            basket.Articles.Add(selectedArticle);
            //basket.user; //TODO: User setzen der Angemeldet ist


            //Mittels der API den Artikel zum Warenkorb hinzufügen
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{ApiUrlBasket}/basket?apiKey={ApiKey}", basket);
                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Erfolg", "Artikel wurde zum Warenkorb hinzugefügt.", "OK");
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

        // Command zum Navigieren zur CartPage
        [RelayCommand]
        private async Task NavigateToCartAsync()
        {
            //await Shell.Current.GoToAsync(nameof(CartPage));
        }

        // Laden der Artikel von der API
        private async Task LoadArticlesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}?apiKey={ApiKey}");

                 if (response.IsSuccessStatusCode){
                    var articlesList = await response.Content.ReadFromJsonAsync<List<Article>>();
                    if (articlesList != null)
                    {
                        foreach (var article in articlesList)
                        {
                            Articles.Add(article);
                        }
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
