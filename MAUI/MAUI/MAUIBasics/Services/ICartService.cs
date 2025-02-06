using MAUIBasics.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MAUIBasics.Services
{
    public interface ICartService
    {
        ObservableCollection<CartItem> Cart { get; }

        Task LoadCartAsync();
        Task AddToCartAsync(Article article, User user, int quantity);
        Task RemoveFromCartAsync(CartItem cartItem);
        Task ClearCartAsync();
    }
}
