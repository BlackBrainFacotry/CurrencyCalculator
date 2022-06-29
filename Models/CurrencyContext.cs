using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CurrencyCalculator.Models
{
    public partial class CurrencyContext : DbContext
    {
        public CurrencyContext()
        {
        }

        public CurrencyContext(DbContextOptions<CurrencyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RatesDb> RatesDbs { get; set; }
        public virtual DbSet<RootDb> RootDbs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-2A7VNHC\\SQLEXPRESS;Database=currencyCalculator;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<RatesDb>(entity =>
            {
                entity.HasKey(e => new { e.Code, e.RootId });

                entity.ToTable("RatesDb");

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .IsFixedLength(true);

                entity.Property(e => e.Currency)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Root)
                    .WithMany(p => p.RatesDbs)
                    .HasForeignKey(d => d.RootId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RatesDb_RootDb");
            });

            modelBuilder.Entity<RootDb>(entity =>
            {
                entity.ToTable("RootDb");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EffectiveDate).HasColumnType("date");

                entity.Property(e => e.No)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Table)
                    .HasMaxLength(1)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
