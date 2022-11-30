namespace FinalSurveyPractice.DTOs.UserAnswer
{
    public class AddUserAnswerDto
    {
        public string UserAns { get; set; } = null!;
        public Guid IdUser { get; set; }
        public Guid IdQuestion { get; set; }
    }
}

