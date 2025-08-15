using System;
using System.Collections.Generic;

namespace CRUD.Models;

public partial class CandidateTable
{
    public int CandidateId { get; set; }

    public string Name { get; set; } = null!;

    public int? ClassId { get; set; }

    public virtual ClassTable? Class { get; set; }

    public virtual ICollection<CourseTable> CourseTables { get; set; } = new List<CourseTable>();
}
