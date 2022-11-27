using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalSurveyPractice.DTOs.Survey
{
    public class GetSurveyDto
    {
        public int IdSurvey { get; set; }
        public string Name { get; set; } = null!;
        public DateTime RegisterDate { get; set; }
        public bool Status { get; set; }
        public Guid IdCategory { get; set; }
    }
}
