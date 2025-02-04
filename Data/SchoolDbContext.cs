using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using SchoolManagementSystem.Models;

namespace CourseManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Course -> Teacher relationship
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher) // Course has one Teacher
                .WithMany(u => u.Courses)// Teacher has many Courses
                .HasForeignKey(c => c.TeacherId) // Foreign key in Course
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure StudentCourse -> Student relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student) // StudentCourse has one Student
                .WithMany(u => u.StudentCourses) // Student has many StudentCourses
                .HasForeignKey(sc => sc.StudentId) // Foreign key in StudentCourse
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Student is deleted

            // Configure StudentCourse -> Course relationship
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course) // StudentCourse has one Course
                .WithMany(c => c.StudentCourses) // Course has many StudentCourses
                .HasForeignKey(sc => sc.CourseId) // Foreign key in StudentCourse
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if Course is deleted

            // Configure composite primary key for StudentCourse
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId }); // Composite key

            // Optional: Add constraints for Title and Email
            modelBuilder.Entity<Course>()
                .Property(c => c.Title)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
