using System;
using System.Collections.Generic;

namespace Ponto.DAL.Objects;

public partial class Entry
{
    public long Id { get; set; }

    public string? PunchDateTime { get; set; }

    public int? PunchType { get; set; }

    public long RegisterId { get; set; }

    public virtual Register Register { get; set; } = null!;
}
