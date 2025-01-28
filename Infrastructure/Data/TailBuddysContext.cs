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
        public DbSet<Message> Messeges { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public TailBuddysContext(DbContextOptions dbOptions) : base(dbOptions)
        {

        }
    }
}
