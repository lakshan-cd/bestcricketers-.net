using BestCricketers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Infrastructure.Repositories
{
    /// <summary>
    /// This interface defines WHAT operations are available for Cricketer data.
    /// The Application layer depends on this interface, not on the concrete implementation.
    /// This is the Dependency Inversion Principle — high-level code depends on abstractions.
    ///
    /// Java comparison: Think of this like a Spring @Repository interface.
    /// </summary>
    public interface ICricketerRepository
    {
        Task<IEnumerable<CricketerProfile>> GetAllAsync();
        Task<CricketerProfile?> GetByIdAsync(int id);
        Task<int> AddAsync(CricketerProfile cricketer);
        Task<int> UpdateAsync(CricketerProfile cricketer);
        Task<int> DeleteAsync(int id);
    }
}
