using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatApp.Models
{
    public partial class ChatContext : DbContext
    {
        public ChatContext()
        {
        }

        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ChatAppDb;Trusted_Connection=True;");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.IdentityId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Message).IsRequired();

                entity.HasOne(d => d.Identity)
                    .WithMany(p => p.Chat)
                    .HasForeignKey(d => d.IdentityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Chat__IdentityId__49C3F6B7");
            });
        }
    }
}
