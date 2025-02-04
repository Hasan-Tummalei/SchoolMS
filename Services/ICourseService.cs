using CourseManagement.Models;

namespace SchoolManagementSystem.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int id);
        Task<bool> CreateCourseAsync(string title, string description, int teacherId);
        Task<bool> UpdateCourseAsync(int id, string title, string description, int teacherId);
        Task<bool> DeleteCourseAsync(int id, int teacherId);
        Task<bool> IsTeacherAuthorizedAsync(int courseId, int teacherId);
    }
}