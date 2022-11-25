namespace FinalSurveyPractice.DTOs.QuestionAnswer
{
    public class GetQuestionAnswerDto
    {
        public Guid GuidQuestionId { get; set; }
        public string AnswerOption { get; set; } = null!;

        public bool Correct { get; set; }

        public bool Status { get; set; }

        public Guid QuestionId { get; set; }
    }
}
