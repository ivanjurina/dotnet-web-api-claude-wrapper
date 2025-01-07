using Microsoft.EntityFrameworkCore;
using dotnet_webapi_claude_wrapper.DataModel.Entities;
using BCrypt.Net;

namespace dotnet_webapi_claude_wrapper.DataModel
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Configure Chat
            modelBuilder.Entity<Chat>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            // Configure Message
            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);

            // Seed admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "admin",
                IsAdmin = true
            });
        }
    }
}