using System.Collections.ObjectModel;
using MAUIBasics.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MAUIBasics.Models.DB; // Namespace f�r UserContext
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models.DB; // Namespace f�r UserContext
using Microsoft.Maui.Controls; // F�r Shell.Current.DisplayAlert
using Microsoft.EntityFrameworkCore;
using Klassenbiliothek_Article; // F�r EF Core Erweiterungsmethoden wie FirstOrDefaultAsync


namespace MAUIBasics.ViewModels
{
    public partial class ShopPageViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        private const string ApiUrl = "https://localhost:7243/api/shop/articles";

        [ObservableProperty]
        private ObservableCollection<Article> articles;

        // ObservableCollection für den Warenkorb
        [ObservableProperty]
        private ObservableCollection<Article> cart;

        public ShopPageViewModel()
        {
            Articles = new ObservableCollection<Article>();
            Cart = new ObservableCollection<Article>();
            _httpClient = new HttpClient();
            LoadArticlesAsync();
        }

        // Command zum Hinzufügen eines Artikels zum Warenkorb
        [RelayCommand]
        private async Task AddToCartAsync(Article selectedArticle)
        {
            if (selectedArticle == null)
                return;

            // Optional: Überprüfen, ob der Artikel bereits im Warenkorb ist
            if (!Cart.Contains(selectedArticle))
            {
                Cart.Add(selectedArticle);
                await Application.Current.MainPage.DisplayAlert("Erfolg", $"{selectedArticle.Name} wurde zum Warenkorb hinzugefügt.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Information", $"{selectedArticle.Name} befindet sich bereits im Warenkorb.", "OK");
            }
        }

        private async Task LoadArticlesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}?apiKey={ApiKey}");

                if (response.IsSuccessStatusCode)
                {
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