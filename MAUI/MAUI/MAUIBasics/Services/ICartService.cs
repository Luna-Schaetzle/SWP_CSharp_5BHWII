using MAUIBasics.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MAUIBasics.Services
{
    public interface ICartService
    {
        ObservableCollection<CartItem> Cart { get;  }

        Task LoadCartAsync(User user);

        Task UpdateQuantityAsync(CartItem cartItem);
        Task AddToCartAsync(Article article, User user, int quantity);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync();
    }
}
