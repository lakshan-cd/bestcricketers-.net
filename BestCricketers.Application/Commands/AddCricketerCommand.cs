using BestCricketers.Domain.Entities;
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
    /// COMMAND: A request to add a new cricketer.
    /// Commands carry the data needed to perform an action.
    /// IRequest<int> means this command returns an int (the new cricketer's ID).
    /// </summary>
    public record AddCricketerCommand(
        string Name,
        int ODI,
        int Tests,
        int ODIRuns,
        int TestRuns
        ) : IRequest<int>;

    /// <summary>
    /// HANDLER: Processes the AddCricketerCommand.
    /// This is where business logic lives. For example, you could check:
    /// - Is the name already taken?
    /// - Are the run counts valid?
    /// Then it uses the repository to persist the data.
    /// </summary>
    public class AddCricketerCommandHandler : IRequestHandler<AddCricketerCommand, int>
    {
        private readonly ICricketerRepository _repository;

        public AddCricketerCommandHandler(ICricketerRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(AddCricketerCommand request, CancellationToken cancellationToken)
        {
            // Create the domain entity from the command data
            var cricketer = new CricketerProfile
            {
                Name = request.Name,
                ODI = request.ODI,
                Tests = request.Tests,
                ODIRuns = request.ODIRuns,
                TestRuns = request.TestRuns
            };

            // Persist to database and return the new ID
            return await _repository.AddAsync(cricketer);
        }
    }
}
