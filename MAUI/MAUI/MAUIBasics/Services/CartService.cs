
using MAUIBasics.Models.DB;
using MAUIBasics.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MAUIBasics.Services
{
    public class CartService 
    {
        private readonly UserContext _dbContext;

        public ObservableCollection<CartItem> Cart { get; }

        /*public CartService(UserContext dbContext)
        {
            _dbContext = dbContext;
            Cart = new ObservableCollection<CartItem>();
            LoadCartAsync().Wait();
        }*/

        // Laden des Warenkorbs aus der Datenbank
        public async Task LoadCartAsync()
        {
            var cartItems = await _dbContext.CartItems.Include(c => c.Article).ToListAsync();
            Cart.Clear();
            foreach (var item in cartItems)
            {
                Cart.Add(item);
            }
        }

        // Hinzufügen eines Artikels zum Warenkorb
        public async Task AddToCartAsync(Article article)
        {
            var existingItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.ArticleId == article.ArticleId);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                _dbContext.CartItems.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    ArticleId = article.ArticleId,
                    Quantity = 1,
                    Article = article // Optional, wenn Navigationseigenschaften genutzt werden
                };
                await _dbContext.CartItems.AddAsync(cartItem);
            }
            await _dbContext.SaveChangesAsync();
            await LoadCartAsync();
        }

        // Entfernen eines Artikels aus dem Warenkorb
        public async Task RemoveFromCartAsync(CartItem cartItem)
        {
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity -= 1;
                _dbContext.CartItems.Update(cartItem);
            }
            else
            {
                _dbContext.CartItems.Remove(cartItem);
            }
            await _dbContext.SaveChangesAsync();
            await LoadCartAsync();
        }

        // Löschen des gesamten Warenkorbs
        public async Task ClearCartAsync()
        {
            _dbContext.CartItems.RemoveRange(_dbContext.CartItems);
            await _dbContext.SaveChangesAsync();
            Cart.Clear();
        }
    }
}
