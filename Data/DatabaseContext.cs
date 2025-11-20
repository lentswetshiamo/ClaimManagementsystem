using Microsoft.EntityFrameworkCore;
using ClaimManagementsystem.Models;

namespace ClaimManagementsystem.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Claim>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne<Lecturer>()
                .WithMany()
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed initial data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Lecturer",
                    Age = "35",
                    Email = "lecturer@example.com",
                    Role = "Lecturer",
                    Password = "password123"
                },
                new User
                {
                    Id = 2,
                    Name = "Jane",
                    Surname = "Coordinator",
                    Age = "40",
                    Email = "coordinator@example.com",
                    Role = "Programme Coordinator",
                    Password = "password123"
                },
                new User
                {
                    Id = 3,
                    Name = "Bob",
                    Surname = "Manager",
                    Age = "45",
                    Email = "manager@example.com",
                    Role = "Academic Manager",
                    Password = "password123"
                },
                new User
                {
                    Id = 4,
                    Name = "Alice",
                    Surname = "HR",
                    Age = "38",
                    Email = "hr@example.com",
                    Role = "HR",
                    Password = "password123"
                }
            );
        }
    }
}
