using EShoppingAutoMobilesBusinessLibrary.UserModels;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using EShoppingAutoMobilesBusinessLibrary.Token;

namespace EShoppingAutoMobiles.DataAccess
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

        public DbSet<UserMaster> UserMaster { get; set; }

        protected virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMaster>().ToTable("UserMaster").HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
