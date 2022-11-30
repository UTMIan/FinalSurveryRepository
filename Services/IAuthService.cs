using FinalSurveyPractice.DTOs.AuthUser;
using FinalSurveyPractice.Models;

namespace FinalSurveyPractice.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, Guid id);
        Task<bool> UserExist(string username);
        Task<bool> UserIdExist(Guid id);
    }
}
