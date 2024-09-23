using Microsoft.EntityFrameworkCore;
using TarpinisAtsiskaitymasAdasDB.Entities;
using TarpinisAtsiskaitymasAdasDB.Services;

namespace TarpinisAtsiskaitymasAdasDB.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        
    }
    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Lecture> Lectures { get; set; }
    public DbSet<Department> Departments { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AtsiskaitymasDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            // Primary key
            entity.HasKey(d => d.DepartmentCode);

            // Properties
            entity.Property(d => d.DepartmentCode)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(d => d.DepartmentName)
                  .IsRequired()
                  .HasMaxLength(100);

            // Relationships
            // One-to-many: Department -> Students
            entity.HasMany(d => d.Students)
                  .WithOne(s => s.Department)
                  .HasForeignKey(s => s.DepartmentCode)
                  .OnDelete(DeleteBehavior.SetNull);

            // Many-to-many: Department -> Lectures
            entity.HasMany(d => d.Lectures)
                  .WithMany(l => l.Departments)
                  .UsingEntity<Dictionary<string, object>>(
                      "DepartmentLectures",
                      j => j.HasOne<Lecture>().WithMany().HasForeignKey("LectureName"),
                      j => j.HasOne<Department>().WithMany().HasForeignKey("DepartmentCode"),
                      j =>
                      {
                          j.HasKey("DepartmentCode", "LectureName"); // Composite primary key
                      }
                  );
        });

        modelBuilder.Entity<Lecture>(entity =>
        {
            // Primary key
            entity.HasKey(l => l.LectureName);

            // Properties
            entity.Property(l => l.LectureName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(l => l.LectureTime)
                  .IsRequired()
                  .HasMaxLength(20);

            // Many-to-many relationships
            // Many-to-many: Lecture -> Departments (already configured in Department)
            // Many-to-many: Student -> Lectures
            entity.HasMany(l => l.Students)
                  .WithMany(s => s.Lectures)
                  .UsingEntity<Dictionary<string, object>>(
                      "StudentLectures",
                      j => j.HasOne<Student>().WithMany().HasForeignKey("StudentNumber"),
                      j => j.HasOne<Lecture>().WithMany().HasForeignKey("LectureName"),
                      j =>
                      {
                          j.HasKey("StudentNumber", "LectureName"); // Composite primary key
                      }
                  );
        });

        modelBuilder.Entity<Student>(entity =>
        {
            // Primary key
            entity.HasKey(s => s.StudentNumber);
            entity.Property(s => s.StudentNumber)
            .IsRequired().
            ValueGeneratedNever();

            // Properties
            entity.Property(s => s.FirstName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(s => s.LastName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(s => s.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            // Relationships
            // One-to-many: Student -> Department (already configured in Department)
            // Many-to-many: Student -> Lectures (already configured in Lecture)
        });

        ModelSeeder.SeedData(modelBuilder);
    }

}
