using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Models;
namespace TailBuddys.Infrastructure.Data
{
    public class TailBuddysContext : DbContext
    {
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<MatchNotification> MatchNotification { get; set; }
        public DbSet<ChatNotification> ChatNotifications { get; set; } // להגדיר אנטיטיז

        public TailBuddysContext(DbContextOptions dbOptions) : base(dbOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Dog (One-to-Many)
            modelBuilder.Entity<Dog>()
                .HasOne(d => d.User)
                .WithMany(u => u.Dogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Dog)
                .WithMany(d => d.Images)
                .HasForeignKey(i => i.DogId)
                .HasPrincipalKey(d => d.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Park)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ParkId)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Chat - Dog Relationship (Self-Referencing Many-to-One)
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.SenderDog)
                .WithMany(d => d.ChatsAsSender)  // ✅ Keep track of chats where Dog is the sender
                .HasForeignKey(c => c.SenderDogId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.ReceiverDog)
                .WithMany(d => d.ChatsAsReceiver)  // ✅ Keep track of chats where Dog is the receiver
                .HasForeignKey(c => c.ReceiverDogId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Ensure only ONE chat exists between two dogs (avoid duplicate chat records)
            modelBuilder.Entity<Chat>()
                .HasIndex(c => new { c.SenderDogId, c.ReceiverDogId })
                .IsUnique();

            // Message - Chat (One-to-Many)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatID)
                .OnDelete(DeleteBehavior.Cascade);

            // Match - Dog Relationship (Self-Referencing)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.SenderDog)
                .WithMany(d => d.MatchesAsSender) // ✅ FromDog will now have a list of matches
                .HasForeignKey(m => m.SenderDogId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.ReceiverDog)
                .WithMany(d => d.MatchesAsReceiver) // ✅ ToDog will now have a list of matches
                .HasForeignKey(m => m.ReceiverDogId)
                .OnDelete(DeleteBehavior.Restrict);

            // Park - Dog (Many-to-Many)
            modelBuilder.Entity<Park>()
                .HasMany(p => p.DogLikes)
                .WithMany(d => d.FavParks)
                .UsingEntity<Dictionary<string, object>>(
                    "DogParks",
                    j => j.HasOne<Dog>().WithMany().HasForeignKey("DogLikesId"),
                    j => j.HasOne<Park>().WithMany().HasForeignKey("FavParksId"),
                    j =>
                    {
                        j.HasKey("DogLikesId", "FavParksId");
                        j.ToTable("DogParks");
                    });

            // Notification - Dog (One-to-Many)
            //modelBuilder.Entity<MatchNotification>()
            //    .HasOne(n => n.Dog)
            //    .WithMany(d => d.MatchNotification)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchNotification>()
                .HasKey(mn => mn.Id); // Set the primary key

            modelBuilder.Entity<MatchNotification>()
                .HasOne(mn => mn.Dog)
                .WithMany(d => d.MatchNotification) // Dog has a collection of MatchNotifications
                .HasForeignKey(mn => mn.DogId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior

            modelBuilder.Entity<MatchNotification>()
                .HasOne(mn => mn.Match)
                .WithMany(m => m.MatchNotification) // Match has a collection of MatchNotifications
                .HasForeignKey(mn => mn.MatchId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete behavior



            modelBuilder.Entity<ChatNotification>()
                .HasOne(n => n.Dog)
                .WithMany(d => d.UnreadChatNotification)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed(); // 👈 Add this

        }
    }
}
