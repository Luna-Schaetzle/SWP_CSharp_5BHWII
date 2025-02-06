using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
//using Xamarin.Essentials;

namespace MAUIBasics.Models.DB
{
    internal class UserContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Article> Articles { get; set; } // Optional, falls du Artikel lokal speichern möchtest


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "cart.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }


    }
}
