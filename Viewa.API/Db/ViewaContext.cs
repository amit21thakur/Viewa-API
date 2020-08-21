using System;
using Microsoft.EntityFrameworkCore;


namespace Viewa.Db
{
    public partial class ViewaContext : DbContext
    {
        public ViewaContext()
        {
        }

        public ViewaContext(DbContextOptions<ViewaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SampleData> SampleData { get; set; }
        public virtual DbSet<Users> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleData>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AppDeviceType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppUserGender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppUserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CampaignName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventDate).HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
