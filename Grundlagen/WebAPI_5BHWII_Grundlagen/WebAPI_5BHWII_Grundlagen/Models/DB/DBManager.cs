using Microsoft.EntityFrameworkCore;
using Klassenbiliothek_Article;


namespace WebAPI_5BHWII_Grundlagen.Models.DB
{
    public class DBManager : DbContext
    {
        public DbSet<Article> Articles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;database=shop_webapi;user=root;password=zwiebel55";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

    }
}
