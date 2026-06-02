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
    /// COMMAND: A request to delete a cricketer by their ID.
    /// This is a simple command — it only needs the Id.
    /// IRequest<bool> returns true if deleted, false if not found.
    /// </summary>
    public record DeleteCricketerCommand(int Id) : IRequest<bool>;

    public class DeleteCricketerCommandHandler : IRequestHandler<DeleteCricketerCommand, bool>
    {
        private readonly ICricketerRepository _repository;

        public DeleteCricketerCommandHandler(ICricketerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCricketerCommand request, CancellationToken cancellationToken)
        {
            // Check if cricketer exists before deleting
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null) return false;

            var rowsAffected = await _repository.DeleteAsync(request.Id);
            return rowsAffected > 0;
        }
    }
}
