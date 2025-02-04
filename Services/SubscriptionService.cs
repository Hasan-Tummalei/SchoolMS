using CourseManagement.Models;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IEnumerable<StudentCourse>> GetSubscriptionsByStudentAsync(int studentId)
        {
            return await _subscriptionRepository.GetSubscriptionsByStudentAsync(studentId);
        }

        public async Task<bool> SubscribeAsync(int studentId, int courseId)
        {
            if (await _subscriptionRepository.IsSubscribedAsync(studentId, courseId))
            {
                return false; // Already subscribed
            }

            var subscription = new StudentCourse
            {
                StudentId = studentId,
                CourseId = courseId
            };

            await _subscriptionRepository.AddSubscriptionAsync(subscription);
            return true;
        }

        public async Task<bool> UnsubscribeAsync(int studentId, int courseId)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsByStudentAsync(studentId);
            var subscription = subscriptions.FirstOrDefault(sc => sc.CourseId == courseId);

            if (subscription == null)
            {
                return false; // Subscription not found
            }

            await _subscriptionRepository.RemoveSubscriptionAsync(subscription);
            return true;
        }

        public async Task<bool> IsSubscribedAsync(int studentId, int courseId)
        {
            return await _subscriptionRepository.IsSubscribedAsync(studentId, courseId);
        }
    }
}