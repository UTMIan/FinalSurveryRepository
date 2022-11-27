using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.DTOs.QuestionAnswer
{
    public class AddQuestionAnswerDto
    {
        public string AnswerOption { get; set; } = null!;

        public bool Correct { get; set; }

        public bool Status { get; set; }

        public Guid IdQuestion { get; set; }
    }
}
