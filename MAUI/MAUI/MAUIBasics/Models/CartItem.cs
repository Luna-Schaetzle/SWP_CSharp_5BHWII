// Models/CartItem.cs
using MAUIBasics.Models; // Namespace für Article

namespace MAUIBasics.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; } // Primärschlüssel
        public int ArticleId { get; set; }   // Fremdschlüssel zum Artikel
        public int Quantity { get; set; }    // Anzahl des Artikels im Warenkorb

        // Navigationseigenschaft
        public Article Article { get; set; }
    }
}
