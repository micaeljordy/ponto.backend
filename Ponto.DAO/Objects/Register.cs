using System;
using System.Collections.Generic;

namespace Ponto.DAL.Objects;

public partial class Register
{
    public long Id { get; set; }

    public string? EmployeeName { get; set; }

    public string? PunchDate { get; set; }

    public virtual ICollection<Entry> Entries { get; } = new List<Entry>();
}
