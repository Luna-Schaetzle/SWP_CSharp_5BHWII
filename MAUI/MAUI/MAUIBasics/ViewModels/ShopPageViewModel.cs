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
    public class ShopPageViewModel : BindableObject
    {
        private ObservableCollection<Article> _articles;
        public ObservableCollection<Article> Articles
        {
            get => _articles;
            set
            {
                _articles = value;
                OnPropertyChanged();
            }
        }

        public ShopPageViewModel()
        {
            Articles = new ObservableCollection<Article>();
            LoadArticlesAsync();
        }

        private async Task LoadArticlesAsync()
        {
            string apiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
            string apiUrl = $"https://localhost:7243/api/shop/articles?apiKey={apiKey}";

            using HttpClient client = new HttpClient();
            var articles = await client.GetFromJsonAsync<List<Article>>(apiUrl);
            if (articles != null)
            {
            foreach (var article in articles)
            {
                Articles.Add(article);
            }
            }
        }

        private async void LoadArticles()
        {
            string apiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
            string apiUrl = $"https://localhost:7243/api/shop/articles?apiKey={apiKey}";

            using HttpClient client = new HttpClient();
            var articles = await client.GetFromJsonAsync<List<Article>>(apiUrl);
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    Articles.Add(article);
                }
            }
        }
    }
}