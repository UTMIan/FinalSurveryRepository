using FinalSurveyPractice.DTOs.AuthUser;
using FinalSurveyPractice.Models;

namespace FinalSurveyPractice.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> UpdateUser(User user, string password, int id);
        Task<bool> UserExist(string username);
        Task<bool> UserIdExist(int id);
    }
}
