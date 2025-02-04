using CourseManagement.Models;

namespace SchoolManagementSystem.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailTakenAsync(string email);
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task SaveAsync();
    }
}
