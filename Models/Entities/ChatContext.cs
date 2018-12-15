using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatApp.Models.Entities
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
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<PrivateMessage> PrivateMessage { get; set; }


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

            modelBuilder.Entity<Friends>(entity =>
            {
                entity.HasKey(e => new { e.IdentityId, e.FriendId })
                    .HasName("PK__Friends__1A4A8C1D4F247A16");

                entity.HasOne(d => d.Friend)
                    .WithMany(p => p.FriendsFriend)
                    .HasForeignKey(d => d.FriendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friends__FriendI__4D94879B");

                entity.HasOne(d => d.Identity)
                    .WithMany(p => p.FriendsIdentity)
                    .HasForeignKey(d => d.IdentityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Friends__Identit__4CA06362");
            });

            modelBuilder.Entity<PrivateMessage>(entity =>
            {
                entity.Property(e => e.ToUserId).HasMaxLength(450);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.PrivateMessage)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PrivateMe__ChatI__5535A963");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.PrivateMessageToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK__PrivateMe__ToUse__534D60F1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PrivateMessageUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__PrivateMe__UserI__52593CB8");
            });
        }
    }
}
