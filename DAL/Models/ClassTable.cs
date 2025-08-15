using System;
using System.Collections.Generic;

namespace CRUD.DAL.Models;

public partial class ClassTable
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public virtual ICollection<CandidateTable> CandidateTables { get; set; } = new List<CandidateTable>();
}
