using System;
using System.Collections.Generic;

namespace CRUD.Models;

public partial class CourseTable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CandidateId { get; set; }

    public virtual CandidateTable? Candidate { get; set; }
}
