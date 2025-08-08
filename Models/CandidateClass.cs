using System;
using System.Collections.Generic;

namespace CRUD.Models;

public partial class CandidateClass
{
    public int ClassId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
}
