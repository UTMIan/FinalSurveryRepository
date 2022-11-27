using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.DTOs.UserAnswer
{
    public class GetUserAnswerDto
    {
        public Guid IdUserAnswer { get; set; }
        public string UserAns { get; set; } = null!;
        public int IdUser { get; set; }
        public Guid IdQuestion { get; set; }
    }
}
