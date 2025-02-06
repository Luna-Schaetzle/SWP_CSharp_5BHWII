using MAUIBasics.Models.DB;
using MAUIBasics.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MAUIBasics.Services
{
    public class CartService : ICartService
    {
        private readonly UserContext _dbContext;

        public ObservableCollection<CartItem> Cart { get; }

        public CartService(UserContext dbContext)
        {
            _dbContext = dbContext;
            Cart = new ObservableCollection<CartItem>();
            InitializeDatabase();
        }

        // Initialisiert die SQLite-Datenbank
        private async void InitializeDatabase()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            await LoadCartAsync();
        }

        // Laden des Warenkorbs aus SQLite
        public async Task LoadCartAsync()
        {
            var cartItems = await _dbContext.CartItems.Include(c => c.Article).ToListAsync();
            Cart.Clear();
            foreach (var item in cartItems)
            {
                Cart.Add(item);
            }
        }

        // Artikel zum Warenkorb hinzufügen
        public async Task AddToCartAsync(Article article, User user, int quantity)
        {
            var existingItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.Article.ArticleId == article.ArticleId && c.user.UserId == user.UserId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _dbContext.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    user = user,
                    Quantity = quantity,
                    Article = article
                };
                await _dbContext.CartItems.AddAsync(cartItem);
            }

            await _dbContext.SaveChangesAsync();
            await LoadCartAsync();
        }

        // Artikel aus dem Warenkorb entfernen
        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            var item = await _dbContext.CartItems.FindAsync(cartItem.CartItemId);
            if (item == null) return;

            if (item.Quantity > 1)
            {
                item.Quantity -= 1;
                _dbContext.CartItems.Update(item);
            }
            else
            {
                _dbContext.CartItems.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            await LoadCartAsync();
        }

        // Warenkorb leeren
        public async Task ClearCartAsync()
        {
            _dbContext.CartItems.RemoveRange(_dbContext.CartItems);
            await _dbContext.SaveChangesAsync();
            Cart.Clear();
        }
    }
}
