using CourseManagement.Data;
using CourseManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolManagementSystem.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentCourse>> GetSubscriptionsByStudentAsync(int studentId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Teacher)
                .ToListAsync();
        }

        public async Task<bool> IsSubscribedAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task AddSubscriptionAsync(StudentCourse subscription)
        {
            _context.StudentCourses.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSubscriptionAsync(StudentCourse subscription)
        {
            _context.StudentCourses.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
