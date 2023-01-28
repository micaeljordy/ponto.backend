using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponto.DTO
{
    public class EntryGetDTO
    {
        public string? employeeName { get; set; }
        public string? punchDate { get; set; }
        public List<EntryDTO>? Entries { get; set; }
        public string amountOfHoursWorked { get; set; }
    }

    public class EntryDTO
    {
        public string? punchDateTime { get; set; }
        public int punchType { get; set; }
    }
}
