using ManitApp.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManitApp.API.Infrastructure
{
    public class ManitAppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderVector> OrderVector { get; set; }

        public ManitAppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ManitAppDbContext");

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.UseNpgsql(connectionString, o => o.UseVector());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            #region Order
            modelBuilder.Entity<Order>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Order>()
                .Property(p => p.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Order>()
                .Property(p => p.UserId)
                .IsRequired()
                .HasColumnName("userid");

            modelBuilder.Entity<Order>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");

            modelBuilder.Entity<Order>()
                .Property(p => p.Gender)
                .IsRequired()
                .HasColumnName("gender");

            modelBuilder.Entity<Order>()
                .Property(p => p.Origin)
                .IsRequired()
                .HasColumnName("origin");

            modelBuilder.Entity<Order>()
                .Property(p => p.Destination)
                .IsRequired()
                .HasColumnName("destination");

            modelBuilder.Entity<Order>()
                .Property(p => p.Time)
                .IsRequired()
                .HasColumnName("time");

            modelBuilder.Entity<Order>().ToTable("orders");
            #endregion

            #region OrderVector
            modelBuilder.Entity<OrderVector>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<OrderVector>()
              .Property(p => p.Id)
              .HasColumnName("id");

            modelBuilder.Entity<OrderVector>()
                .Property(p => p.OrderId)
                .IsRequired()
                .HasColumnName("orderid");

            modelBuilder.Entity<OrderVector>()
                .Property(p => p.Vector)
                .HasColumnName("vector")
                .HasColumnType("vector(3)");

            modelBuilder.Entity<OrderVector>().ToTable("ordervectors");
            #endregion
        }
    }
}
