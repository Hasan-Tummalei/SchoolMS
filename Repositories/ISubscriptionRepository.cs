using CourseManagement.Models;

namespace SchoolManagementSystem.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<StudentCourse>> GetSubscriptionsByStudentAsync(int studentId);
        Task<bool> IsSubscribedAsync(int studentId, int courseId);
        Task AddSubscriptionAsync(StudentCourse subscription);
        Task RemoveSubscriptionAsync(StudentCourse subscription);
    }
}
