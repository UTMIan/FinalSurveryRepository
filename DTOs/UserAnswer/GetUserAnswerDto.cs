using FinalSurveyPractice.DTOs.AuthUser;
using FinalSurveyPractice.DTOs.Question;
using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.DTOs.UserAnswer
{
    public class GetUserAnswerDto
    {
        public Guid IdUserAnswer { get; set; }
        public string UserAns { get; set; } = null!;
        public GetUserDto? IdUser { get; set; }
        public GetQuestionDto? IdQuestion { get; set; }
    }
}
