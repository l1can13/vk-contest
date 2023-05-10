using Microsoft.EntityFrameworkCore;
using vk_app.Entities;

namespace vk_app.Database
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=vk;Username=postgres;Password=");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany()
                .HasForeignKey(u => u.UserGroupId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserState)
                .WithMany()
                .HasForeignKey(u => u.UserStateId);

            modelBuilder.Entity<UserGroup>()
                .HasData(
                    new UserGroup { Id = 1, Code = "Admin", Description = "Administrator" },
                    new UserGroup { Id = 2, Code = "User", Description = "Regular User" }
                );

            modelBuilder.Entity<UserState>()
                .HasData(
                    new UserState { Id = 1, Code = "Active", Description = "Active User" },
                    new UserState { Id = 2, Code = "Blocked", Description = "Blocked User" }
                );
        }
    }
}
