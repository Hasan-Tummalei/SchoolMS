using CourseManagement.Models;

namespace SchoolManagementSystem.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int id);
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<bool> IsTeacherAuthorizedAsync(int courseId, int teacherId);
    }
}
