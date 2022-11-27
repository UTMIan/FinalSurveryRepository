namespace FinalSurveyPractice.DTOs.Survey
{
    public class AddSurveyDto
    {
        public string Name { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public bool Status { get; set; }
        public Guid IdCategory { get; set; }
    }
}
