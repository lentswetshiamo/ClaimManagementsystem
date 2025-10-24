using ClaimManagementsystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ClaimManagementsystem.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // User Management
        public DbSet<User> Users { get; set; }

        // Claim Management
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClaimItem> ClaimItems { get; set; }
        public DbSet<Programme> Programmes { get; set; }

        // Workflow
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<WorkflowState> WorkflowStates { get; set; }
        public DbSet<BusinessRule> BusinessRules { get; set; }

        // Documents
        public DbSet<Document> Documents { get; set; }

        // Audit & Notifications
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<SecurityEvent> SecurityEvents { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Claim Configuration
            modelBuilder.Entity<Claim>(entity =>
            {
                entity.HasKey(e => e.ClaimId);
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            // Claim Items Configuration
            modelBuilder.Entity<ClaimItem>(entity =>
            {
                entity.HasKey(e => e.ClaimItemId);
                entity.HasOne<Claim>()
                      .WithMany()
                      .HasForeignKey(ci => ci.ClaimId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }

    // Programme model if not defined elsewhere (kept minimal)
    public class Programme
    {
        public int ProgrammeId { get; set; }
        public string ProgrammeName { get; set; }
        public string ProgrammeCode { get; set; }
        public decimal BudgetLimit { get; set; }
        public int CoordinatorId { get; set; }
    }
}
