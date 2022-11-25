using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    public Guid IdCategory { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Survey> Surveys { get; } = new List<Survey>();
}
