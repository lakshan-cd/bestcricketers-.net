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
    /// QUERY: Get a single cricketer by their ID.
    /// Notice this query has a property (CricketerId) — this is the input data
    /// the handler needs to do its job.
    /// </summary>
    /// 
    public record GetCricketerByIdQuery(int CrickerterId) : IRequest<CricketerDto>;

    public class GetCricketerByIdQueryHandler : IRequestHandler<GetCricketerByIdQuery, CricketerDto?>
    {
        private readonly ICricketerRepository _repository;

        public GetCricketerByIdQueryHandler(ICricketerRepository repository)
        {
            _repository = repository;
        }

        public async Task<CricketerDto?> Handle(GetCricketerByIdQuery request, CancellationToken cancellationToken)
        {
            var cricketer = await _repository.GetByIdAsync(request.CrickerterId);

            // If not found, return null. The controller will handle this as a 404.
            if (cricketer is null) return null;

            return new CricketerDto
            {
                Id = cricketer.Id,
                Name = cricketer.Name,
                ODI = cricketer.ODI,
                Tests = cricketer.Tests,
                ODIRuns = cricketer.ODIRuns,
                TestRuns = cricketer.TestRuns
            };
        }
    }
}
