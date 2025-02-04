using CourseManagement.Models;

namespace SchoolManagementSystem.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<StudentCourse>> GetSubscriptionsByStudentAsync(int studentId);
        Task<bool> SubscribeAsync(int studentId, int courseId);
        Task<bool> UnsubscribeAsync(int studentId, int courseId);
        Task<bool> IsSubscribedAsync(int studentId, int courseId);
    }
}