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

        public UserContext()
        {
            SQLitePCL.Batteries_V2.Init();

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "users.db3");

            optionsBuilder
                .UseSqlite($"Filename={dbPath}");
        }


    }
}
