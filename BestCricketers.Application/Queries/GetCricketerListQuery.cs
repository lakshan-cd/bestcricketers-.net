using BestCricketers.Application.DTOs;
using BestCricketers.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Application.Queries
{
    /// <summary>
    /// QUERY: A request to get all cricketers.
    /// IRequest<T> tells MediatR: "this query returns a list of CricketerDto".
    ///
    /// In MediatR, a Query is just a plain C# class (a record in modern C#).
    /// Records are perfect for this because they are immutable — a query should
    /// not change its own data.
    /// </summary>
    /// 
    public record GetCricketerListQuery : IRequest<IEnumerable<CricketerDto>>;

    /// <summary>
    /// HANDLER: Processes the GetCricketerListQuery.
    /// IRequestHandler<TRequest, TResponse> means:
    ///   - TRequest = GetCricketerListQuery (the query this handles)
    ///   - TResponse = IEnumerable<CricketerDto> (what it returns)
    ///
    /// MediatR automatically finds this handler and calls Handle() when the query is sent.
    /// </summary>
    public class GetCricketerListQueryHandler : IRequestHandler<GetCricketerListQuery, IEnumerable<CricketerDto>>
    {
        private readonly ICricketerRepository _repository;

        // Dependency Injection — the repository is injected by the DI container.
        // Java comparison: This is like @Autowired in Spring.
        public GetCricketerListQueryHandler(ICricketerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CricketerDto>> Handle(GetCricketerListQuery request, CancellationToken cancellationToken)
        {
            // 1. Get all cricketers from the database via repository
            var cricketers = await _repository.GetAllAsync();


            return cricketers.Select(c => new CricketerDto
            {
                Id = c.Id,
                Name = c.Name,
                ODI = c.ODI,
                Tests = c.Tests,
                ODIRuns = c.ODIRuns,
                TestRuns = c.TestRuns
            });
        }
    }
}
