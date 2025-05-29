using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Booking.Models;

namespace Booking.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Smjestaj> Smjestaj { get; set; }
        public DbSet<Rezervacija> Rezervacija { get; set; }
        public DbSet<Recenzija> Recenzija { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Smjestaj>().ToTable("Smjestaj");
            modelBuilder.Entity<Rezervacija>().ToTable("Rezervacija");
            modelBuilder.Entity<Recenzija>().ToTable("Recenzija");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.HasIndex(e => e.NormalizedUserName)
                      .HasDatabaseName("UserNameIndex")
                      .IsUnique();

                entity.HasIndex(e => e.NormalizedEmail)
                      .HasDatabaseName("EmailIndex");
            });
        }
    }
}
