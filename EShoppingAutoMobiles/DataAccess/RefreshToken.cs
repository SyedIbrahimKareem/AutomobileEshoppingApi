using EShoppingBusinessLibrary.Token;
using EShoppingBusinessLibrary.UserModels;
using Microsoft.EntityFrameworkCore;

namespace EShoppingAPI.DataAccess
{
    public class RefreshTokenDBContext
        : DbContext
    {
        protected readonly IConfiguration Configuration;

        public RefreshTokenDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration["SqlConnection"]);
        }

        public DbSet<ReFreshToken> reFreshTokens { get; set; }

        protected virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReFreshToken>().ToTable("ReFreshToken").HasKey(x=>x.RefreshtokenId);
            base.OnModelCreating(modelBuilder);
        }

    }
}
