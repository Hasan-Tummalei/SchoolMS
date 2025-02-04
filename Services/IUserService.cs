using CourseManagement.Models;

namespace SchoolManagementSystem.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string name, string email, string password, UserRole role);
        Task<User> LoginAsync(string email, string password);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}