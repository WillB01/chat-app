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
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<FriendRequest> FriendRequest { get; set; }
        public virtual DbSet<Friends> Friends { get; set; }
        public virtual DbSet<GroupChat> GroupChat { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<ProfileImage> ProfileImage { get; set; }


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

            modelBuilder.Entity<Conversation>(entity =>
            {
                entity.Property(e => e.ToUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.Conversation)
                    .HasForeignKey(d => d.MessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Conversat__Messa__1AD3FDA4");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ConversationToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Conversat__ToUse__19DFD96B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ConversationUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Conversat__UserI__18EBB532");
            });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.Property(e => e.FromUser).HasMaxLength(450);

                entity.Property(e => e.FromUserName).HasMaxLength(256);

                entity.Property(e => e.ToUser).HasMaxLength(450);

                entity.Property(e => e.ToUserName).HasMaxLength(256);
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

            modelBuilder.Entity<GroupChat>(entity =>
            {
                entity.Property(e => e.GroupAdminId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.GroupMemberId).HasMaxLength(450);

                entity.Property(e => e.GroupName).IsRequired();

                entity.HasOne(d => d.GroupAdmin)
                    .WithMany(p => p.GroupChatGroupAdmin)
                    .HasForeignKey(d => d.GroupAdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupChat__Group__625A9A57");

                entity.HasOne(d => d.GroupMember)
                    .WithMany(p => p.GroupChatGroupMember)
                    .HasForeignKey(d => d.GroupMemberId)
                    .HasConstraintName("FK__GroupChat__Group__73852659");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.IdentityId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Message1)
                    .IsRequired()
                    .HasColumnName("Message");

                entity.HasOne(d => d.Identity)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.IdentityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__Identit__160F4887");
            });

            modelBuilder.Entity<ProfileImage>(entity =>
            {
                entity.Property(e => e.ProfileImage1)
                    .IsRequired()
                    .HasColumnName("ProfileImage");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProfileImage)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProfileIm__UserI__503BEA1C");
            });
        }
    }
}
