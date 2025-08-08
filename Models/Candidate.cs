using System;
using System.Collections.Generic;

namespace CRUD.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string Name { get; set; } = null!;

    public int? ClassId { get; set; }

    public virtual CandidateClass? Class { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
