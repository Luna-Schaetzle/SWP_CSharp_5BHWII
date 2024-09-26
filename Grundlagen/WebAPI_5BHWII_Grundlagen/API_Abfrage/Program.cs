using System.Linq.Expressions;
using System.Net.Http.Json;
using Klassenbiliothek_Article;

namespace API_Abfrage
{
    internal class Program
    {

        private static HttpClient _client = new HttpClient();
        static void Main(string[] args)
        {
            List<Article> list = GetArticlesAsync().Result;

            if (list != null)
            {
                foreach (var article in list)
                {
                    Console.WriteLine($"Article ID: {article.ArticleId}");
                    Console.WriteLine($"Article Name: {article.Name}");
                    Console.WriteLine($"Article Price: {article.Price}");
                    Console.WriteLine($"Article Release Date: {article.ReleaseDate}");
                }
            }
            
            Console.ReadKey();

        }
        static async Task<List<Article>?> GetArticlesAsync()
        {
            _client.BaseAddress = new Uri("https://localhost:7243/api/shop/");

            List<Article>? articles = await _client.GetFromJsonAsync<List<Article>>("articles");

            return articles;
        }
    }
}
