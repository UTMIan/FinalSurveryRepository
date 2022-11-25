using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalSurveyPractice.Models;

namespace FinalSurveyPractice.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<FinalSurveyPractice.Models.User> User { get; set; } = default!;

        public DbSet<FinalSurveyPractice.Models.Category> Category { get; set; }

        public DbSet<FinalSurveyPractice.Models.Question> Question { get; set; }

        public DbSet<FinalSurveyPractice.Models.QuestionAnswer> QuestionAnswer { get; set; }

        public DbSet<FinalSurveyPractice.Models.Role> Role { get; set; }

        public DbSet<FinalSurveyPractice.Models.Survey> Survey { get; set; }

        public DbSet<FinalSurveyPractice.Models.UserAnswer> UserAnswer { get; set; }
    }
}
