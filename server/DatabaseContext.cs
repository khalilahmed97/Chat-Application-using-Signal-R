using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server
{
    sealed public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
