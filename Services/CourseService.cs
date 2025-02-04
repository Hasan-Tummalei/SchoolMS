using CourseManagement.Models;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateCourseAsync(string title, string description, int teacherId)
        {
            var course = new Course
            {
                Title = title,
                Description = description,
                TeacherId = teacherId
            };

            await _courseRepository.AddAsync(course);
            return true;
        }

        public async Task<bool> UpdateCourseAsync(int id, string title, string description, int teacherId)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null || !await _courseRepository.IsTeacherAuthorizedAsync(id, teacherId))
            {
                return false;
            }

            course.Title = title;
            course.Description = description;

            await _courseRepository.UpdateAsync(course);
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id, int teacherId)
        {
            if (!await _courseRepository.IsTeacherAuthorizedAsync(id, teacherId))
            {
                return false;
            }

            await _courseRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> IsTeacherAuthorizedAsync(int courseId, int teacherId)
        {
            return await _courseRepository.IsTeacherAuthorizedAsync(courseId, teacherId);
        }
    }
}