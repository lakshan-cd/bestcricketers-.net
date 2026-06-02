using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Application.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) for Cricketer data.
    /// This is what we return to the API client.
    /// It can be different from the domain entity — for example, we could
    /// add computed fields or remove sensitive fields here.
    /// </summary>
    public class CricketerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ODI { get; set; }
        public int Tests { get; set; }
        public int ODIRuns { get; set; }
        public int TestRuns { get; set; }
    }
}
