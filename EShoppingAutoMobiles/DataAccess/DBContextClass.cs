using EShoppingBusinessLibrary.UserModels;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using EShoppingBusinessLibrary.Token;

namespace EShoppingAPI.DataAccess
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration["SqlConnection"]);
        }

        public DbSet<UserRegisteration> UserRegisteration { get; set; }

        protected virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRegisteration>().ToTable("UserRegisteration").HasKey(x => x.UserId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
