using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;

namespace TrainingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingProgram>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(p => p.Type)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(p => p.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(p => p.UserId)
                    .HasDatabaseName("IX_TrainingPrograms_UserId");
            });

            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(e => e.UserId)
                    .HasDatabaseName("IX_Exercises_UserId");

                entity.HasOne(e => e.Program)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(e => e.ProgramId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Notes)
                    .HasMaxLength(200);
                entity.Property(a => a.UserId)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.HasIndex(a => a.UserId)
                    .HasDatabaseName("IX_Activities_UserId");

                entity.HasOne(a => a.Exercise)
                    .WithMany(e => e.Activities)
                    .HasForeignKey(a => a.ExerciseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}