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
    }
}
