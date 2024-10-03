using System.ComponentModel;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Klassenbiliothek_Article;

namespace API_Abfrage
{
    internal class Program
    {

        private static HttpClient _articleClient = new HttpClient();
        private static HttpClient _userclient = new HttpClient();



        static async Task Main(string[] args)
        {
            _userclient.BaseAddress = new Uri("https://localhost:7243/api/user/");
            Console.WriteLine("Wollen sich Regestrieren? j/n");
            char? start = Convert.ToChar(Console.ReadLine());
            string apiKey = "";

            if (start == 'j')
            {
                //Register for API Key
                Console.WriteLine("Regestrieren Sie sich für einen API Key");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Email:");
                string? email = Console.ReadLine();
                Console.WriteLine("Passwort:");
                string? password = Console.ReadLine();

                //API Key wird generiert und zurückgegeben
                //Ruft bei UserController die POST Methode Register auf
                apiKey = RegisterAsync(email, password).Result;
                Console.WriteLine($"API Key: {apiKey}");

            }
            else
            {
                //User Login
                Console.WriteLine("Login");
                Console.WriteLine("--------------------------------------");
                /*Console.WriteLine("Email:");
                string? email = Console.ReadLine();
                Console.WriteLine("Passwort:");
                string? password = Console.ReadLine();
                apiKey = loginAsync(email, password).Result;
                */
                Console.WriteLine("API Key:");
                apiKey = Console.ReadLine();
                Console.WriteLine($"API Key: {apiKey}");
            }
            Console.WriteLine("-------------------------------------------------");




            _articleClient.BaseAddress = new Uri("https://localhost:7243/api/shop/");

            do
            {
                //Menü
                Console.WriteLine("Menü");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("1. Alle Artikel anzeigen");
                Console.WriteLine("2. Artikel anzeigen");
                Console.WriteLine("3. Artikel löschen");
                Console.WriteLine("4. Artikel hinzufügen");
                Console.WriteLine("5. Artikel ändern");
                Console.WriteLine("Any other key to exit");
                Console.WriteLine("-------------------------------------------------");
                int menu = Convert.ToInt32(Console.ReadLine());

                if (menu == 1)
                {
                    List<Article> list = await GetArticlesAsync(apiKey);

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
                }
                else if (menu == 2)
                {
                    Article? a = await GetArticleAsync(1, apiKey);


                    if (a != null)
                    {
                        Console.WriteLine($"Article ID: {a.ArticleId}");
                        Console.WriteLine($"Article Name: {a.Name}");
                        Console.WriteLine($"Article Price: {a.Price}");
                        Console.WriteLine($"Article Release Date: {a.ReleaseDate}");
                    }


                    Console.WriteLine("-------------------------------------------------");
                }
                else if (menu == 3)
                {
                    //eingabe wie welche ID gelöscht werden soll
                    Console.WriteLine("Welche ID soll gelöscht werden?");
                    int id = Convert.ToInt32(Console.ReadLine());

                    bool? del = await DelArticleAsync(id, apiKey);

                    Console.WriteLine($"Artikel mit ID {id} wurde gelöscht: {del}");

                    Console.WriteLine("-------------------------------------------------");
                }
                else if (menu == 4)
                {
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

                    bool? add = await AddArticleAsync(article_2, apiKey);
                    Console.WriteLine($"Artikel wurde hinzugefügt: {add}");

                    Console.WriteLine("-------------------------------------------------");
                }
                else if (menu == 5)
                {
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

                    bool? change = await UpdateArticleAsync(new Article()
                    {
                        ArticleId = id_2,
                        Name = name_2,
                        Price = price_2,
                        ReleaseDate = releaseDate_2
                    }, apiKey);

                    Console.WriteLine($"Artikel wurde geändert: {change}");

                    Console.WriteLine("-------------------------------------------------");
                }
                else
                {
                    break;
                }
            } while (true);

            Console.ReadKey();

        }

        static async Task<string?> RegisterAsync(string email, string password)
        {
            User user = new User()
            {
                UserId = 0,
                Email = email,
                Password = password,
                ApiKey = "null"
            };

            HttpResponseMessage response = await _userclient.PostAsJsonAsync("Register", user);

            //only return the API Key via Response body
            return await response.Content.ReadAsStringAsync();

        }

        static async Task<string?> loginAsync(string email, string password)
        {
            User user = new User()
            {
                UserId = 0,
                Email = email,
                Password = password,
                ApiKey = "null"
            };

            HttpResponseMessage response = await _userclient.PostAsJsonAsync("CheckAPIKey", user);

            //only return the API Key via Response body
            return await response.Content.ReadAsStringAsync();

        }

        static async Task<List<Article>?> GetArticlesAsync(string apiKey)
        {


            List<Article>? articles = await _articleClient.GetFromJsonAsync<List<Article>>("articles?apiKey=" + apiKey);

            return articles;
        }

        static async Task<Article?> GetArticleAsync(int id, string apiKey)
        {
            Article? article = await _articleClient.GetFromJsonAsync<Article>($"article/{id}?apiKey=" + apiKey);

            return article;
        }

        //testet die DELETE Methode

        static async Task<bool?> DelArticleAsync(int id, string apiKey)
        {
            HttpResponseMessage response = await _articleClient.DeleteAsync($"article/{id}?apiKey=" + apiKey);

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static async Task<bool?> AddArticleAsync(Article article, string apiKey)
        {
            HttpResponseMessage response = await _articleClient.PostAsJsonAsync("article?apiKey=" + apiKey, article);

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static async Task<bool?> UpdateArticleAsync(Article article, string apiKey)
        {
            HttpResponseMessage response = await _articleClient.PutAsJsonAsync("article?apiKey=" + apiKey, article);

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
