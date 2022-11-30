namespace FinalSurveyPractice.DTOs.UserAnswer
{
    public class UpdateUserAnswerDto
    {
        public string UserAns { get; set; } = null!;
        public Guid IdUser { get; set; }
        public Guid IdQuestion { get; set; }
    }
}
