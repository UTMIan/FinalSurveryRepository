namespace FinalSurveyPractice.DTOs.AuthUser
{
    public class AddUserRoleDto
    {
        public Guid RoleId { get; set; }
        public string UserId { get; set; }
        public bool Status { get; set; } = true;
    }
}
