using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.DTOs.UserAnswer
{
    public class GetUserAnswerDto
    {
        public Guid IdUserAnswer { get; set; }
        public string UserAns { get; set; } = null!;
        public int UserId { get; set; }
        public Guid QuestionId { get; set; }
    }
}
