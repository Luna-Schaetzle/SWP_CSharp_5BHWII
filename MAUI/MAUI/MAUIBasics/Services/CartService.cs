using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using MAUIBasics.Models;

namespace MAUIBasics.Services
{
    public class CartService : ICartService
    {
        private const string CartFileName = "cart.json"; // JSON-Datei für lokale Speicherung
        private readonly string CartFilePath = Path.Combine(FileSystem.AppDataDirectory, CartFileName);
        public ObservableCollection<CartItem> Cart { get; }

        public CartService()
        {
            Cart = new ObservableCollection<CartItem>();
            LoadCartAsync();
        }

        // 📌 Warenkorb aus JSON-Datei oder Preferences laden
        public async Task LoadCartAsync()
        {
            try
            {
                if (File.Exists(CartFilePath))
                {
                    string json = await File.ReadAllTextAsync(CartFilePath);
                    var cartItems = JsonSerializer.Deserialize<List<CartItem>>(json);

                    if (cartItems != null)
                    {
                        Cart.Clear();
                        foreach (var item in cartItems)
                        {
                            Cart.Add(item);
                        }
                    }
                }
                else
                {
                    // Falls keine Datei existiert, alten Stand aus Preferences laden
                    string json = Preferences.Get("CartData", string.Empty);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var cartItems = JsonSerializer.Deserialize<List<CartItem>>(json);
                        if (cartItems != null)
                        {
                            Cart.Clear();
                            foreach (var item in cartItems)
                            {
                                Cart.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fehlerhafte Daten oder Datei nicht gefunden -> Warenkorb zurücksetzen
                Cart.Clear();
            }
        }

        // 🛒 Artikel zum Warenkorb hinzufügen
        public async Task AddToCartAsync(Article article, User user, int quantity)
        {
            var existingItem = Cart.FirstOrDefault(c => c.Article.ArticleId == article.ArticleId && c.user.Id == user.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Cart.Add(new CartItem
                {
                    user = user,
                    Quantity = quantity,
                    Article = article
                });
            }

            await SaveCartAsync();
        }

        // ❌ Artikel aus dem Warenkorb entfernen
        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            var item = Cart.FirstOrDefault(c => c.CartItemId == cartItem.CartItemId);
            if (item == null) return;

            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
            }
            else
            {
                Cart.Remove(item);
            }

            await SaveCartAsync();
        }

        // 🗑️ Warenkorb leeren
        public async Task ClearCartAsync()
        {
            Cart.Clear();
            await SaveCartAsync();
        }

        // 💾 Warenkorb in JSON-Datei und Preferences speichern
        private async Task SaveCartAsync()
        {
            try
            {
                string json = JsonSerializer.Serialize(Cart.ToList());

                // JSON-Datei speichern
                await File.WriteAllTextAsync(CartFilePath, json);

                // Auch in Preferences speichern (Fallback)
                Preferences.Set("CartData", json);
            }
            catch
            {
                // Fehler beim Speichern
            }
        }
    }
}