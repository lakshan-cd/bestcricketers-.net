using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Domain.Entities
{
    /// <summary>
    /// Represents a cricketer in our domain.
    /// This is the core entity — it maps to the CricketerProfile table in the database.
    /// In CQRS, this is what our Command Handlers load and work with.
    /// </summary>
    public class CricketerProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ODI { get; set; }       // Number of One Day Internationals played
        public int Tests { get; set; }     // Number of Test matches played
        public int ODIRuns { get; set; }   // Runs scored in ODIs
        public int TestRuns { get; set; }  // Runs scored in Tests
    }
}
 