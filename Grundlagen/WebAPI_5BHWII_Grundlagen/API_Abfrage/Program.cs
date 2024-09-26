using System.ComponentModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Klassenbiliothek_Article;

namespace API_Abfrage
{
    internal class Program
    {

        private static HttpClient _client = new HttpClient();



        static void Main(string[] args)
        {

            _client.BaseAddress = new Uri("https://localhost:7243/api/shop/");

    	    
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

            Console.WriteLine("-------------------------------------------------");

    	    
            Article? a = GetArticleAsync(1).Result;
          

            if (a != null)
            {
                Console.WriteLine($"Article ID: {a.ArticleId}");
                Console.WriteLine($"Article Name: {a.Name}");
                Console.WriteLine($"Article Price: {a.Price}");
                Console.WriteLine($"Article Release Date: {a.ReleaseDate}");
            }
            
            
            Console.WriteLine("-------------------------------------------------");

            //eingabe wie welche ID gelöscht werden soll
            Console.WriteLine("Welche ID soll gelöscht werden?");
            int id = Convert.ToInt32(Console.ReadLine());

            bool? del = DelArticleAsync(id).Result;

            Console.WriteLine($"Artikel mit ID {id} wurde gelöscht: {del}");

            Console.WriteLine("-------------------------------------------------");
            
            //neuen Artikel hinzufügen
            //Den nutzer nach den Daten fragen
            Console.WriteLine("Neuen Artikel hinzufügen");
            Console.WriteLine("Name:");
            string name = Console.ReadLine();
            Console.WriteLine("Preis:");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Release Date:");
            DateTime releaseDate = Convert.ToDateTime(Console.ReadLine());

            Article article_2 = new Article()
            {
                Name = name,
                Price = price,
                ReleaseDate = releaseDate
            };

            bool? add = AddArticleAsync(article_2).Result;
            Console.WriteLine($"Artikel wurde hinzugefügt: {add}");

            Console.WriteLine("-------------------------------------------------");

            //Artikel ändern
            //Den nutzer nach den Daten fragen
            Console.WriteLine("Artikel ändern");
            Console.WriteLine("ID des zu ändernden Artikels:");
            int id_2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("neuer Name:");
            string name_2 = Console.ReadLine();
            Console.WriteLine("neuer Preis:");
            decimal price_2 = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("neues Release Date:");
            DateTime releaseDate_2 = Convert.ToDateTime(Console.ReadLine());

            bool? change = UpdateArticleAsync(new Article()
            {
                ArticleId = id_2,
                Name = name_2,
                Price = price_2,
                ReleaseDate = releaseDate_2
            }).Result;

            Console.WriteLine($"Artikel wurde geändert: {change}");
            
            

                    
            Console.ReadKey();

        }
        static async Task<List<Article>?> GetArticlesAsync()
        {
            

            List<Article>? articles = await _client.GetFromJsonAsync<List<Article>>("articles");

            return articles;
        }

        static async Task<Article?> GetArticleAsync(int id)
        {
            Article? article = await _client.GetFromJsonAsync<Article>($"article/{id}");

            return article;
        }

        //testet die DELETE Methode

        static async Task<bool?> DelArticleAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"article/{id}");

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static async Task<bool?> AddArticleAsync(Article article)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("article", article);

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static async Task<bool?> UpdateArticleAsync(Article article)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync("article", article);

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
  
    }
}
