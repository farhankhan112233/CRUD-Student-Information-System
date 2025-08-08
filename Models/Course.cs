using System;
using System.Collections.Generic;

namespace CRUD.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Name { get; set; }

    public int? CandidateId { get; set; }

    public virtual Candidate? Candidate { get; set; }
}
