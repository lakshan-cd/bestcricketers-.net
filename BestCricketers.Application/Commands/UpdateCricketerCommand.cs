using BestCricketers.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestCricketers.Application.Commands
{
    /// <summary>
    /// COMMAND: A request to update an existing cricketer.
    /// Notice we include the Id — we need it to know WHICH cricketer to update.
    /// IRequest<bool> means this returns true/false (did the update succeed?).
    /// </summary>
    public record UpdateCricketerCommand(
        int Id,
        string Name,
        int ODI,
        int Tests,
        int ODIRuns,
        int TestRuns
    ) : IRequest<bool>;

    internal class UpdateCricketerCommandHandler : IRequestHandler<UpdateCricketerCommand, bool>
    {
        private readonly ICricketerRepository _repository;

        public UpdateCricketerCommandHandler(ICricketerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateCricketerCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null) return false;

            // Update the entity with new values from the command
            existing.Name = request.Name;
            existing.ODI = request.ODI;
            existing.Tests = request.Tests;
            existing.ODIRuns = request.ODIRuns;
            existing.TestRuns = request.TestRuns; 
            // Persist the changes
            var rowsAffected = await _repository.UpdateAsync(existing);
            return rowsAffected > 0;
        }
    }
}
