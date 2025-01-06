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
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            
            // Seed some data
            modelBuilder.Entity<User>().HasData(
                new User { 
                    Id = 1, 
                    Username = "user1", 
                    Email = "user1@example.com",
                    PasswordHash = "AQAAAAIAAYagAAAAELbHLYHoYyLgK+nqcqLZK5KHAUPvXZr6OxHPCYz8HGSyZvw+WVGQmH/+FyUyX1B/vw==" // Password: Test123!
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    IsAdmin = true
                }
            );

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}