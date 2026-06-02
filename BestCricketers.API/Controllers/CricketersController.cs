using BestCricketers.Application.Commands;
using BestCricketers.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BestCricketers.API.Controllers
{
    /// <summary>
    /// The API Controller is the ENTRY POINT for all HTTP requests related to Cricketers.
    ///
    /// Key rules for this controller:
    /// 1. It does NOT contain business logic. It only:
    ///    - Receives HTTP requests
    ///    - Creates a Query or Command
    ///    - Sends it via MediatR
    ///    - Returns the result as HTTP response
    ///
    /// [ApiController] — enables automatic model validation and other API-specific behaviors
    /// [Route("api/[controller]")] — the URL will be /api/cricketers (controller name without "Controller")
    ///
    /// Java comparison: This is like a @RestController in Spring Boot.
    /// The ISender (MediatR) here plays the role of a @Service injection.
    /// </summary>
    /// 

    [ApiController]
    [Route("api/[controller]")]
    public class CricketersController : ControllerBase
    {
        // ISender is the MediatR interface for sending queries/commands
        private readonly ISender _mediator;

        public CricketersController(ISender mediator)
        {
            _mediator = mediator;
        }

        // ═══════════════════════════════════════════════════════════
        // GET METHODS (Read Side — Queries)
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// GET: api/cricketers
        /// Returns a list of all cricketers.
        ///
        /// Flow:
        /// 1. Client sends GET request to /api/cricketers
        /// 2. We create a GetCricketerListQuery
        /// 3. MediatR routes it to GetCricketerListQueryHandler
        /// 4. Handler fetches data via repository (Dapper → SQL Server)
        /// 5. Returns list of CricketerDto as JSON
        ///
        /// HTTP Status Codes returned:
        /// - 200 OK: success, with data in body
        /// - 500 Internal Server Error: unexpected error
        /// </summary>
        /// 

        [HttpGet]
        public async Task<IActionResult> getCricketerList()
        {
            var query = new GetCricketerListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// GET: api/cricketers/{id}
        /// Returns a single cricketer by their ID.
        ///
        /// [FromRoute] — the id comes from the URL path: /api/cricketers/1
        ///
        /// HTTP Status Codes:
        /// - 200 OK: cricketer found
        /// - 404 Not Found: no cricketer with this ID
        /// </summary>
        /// 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCrickerterById([FromRoute] int id)
        {
            var query = new GetCricketerByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result is null)
                return NotFound(new { Message = $"Cricketer with ID {id} was not found." });

            return Ok(result); // 200 OK
        }

        /// <summary>
        /// POST: api/cricketers
        /// Creates a new cricketer.
        ///
        /// [FromBody] — the data comes from the HTTP request body (JSON).
        /// The client sends a JSON object, and .NET automatically deserializes it
        /// into our AddCricketerRequest object.
        ///
        /// HTTP Status Codes:
        /// - 201 Created: cricketer was added, with location header pointing to the new resource
        /// - 400 Bad Request: invalid data (handled automatically by [ApiController])
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddCricketer([FromBody] AddCricketerRequest request)
        {
            // Map the request to a Command and send via MediatR
            var command = new AddCricketerCommand(
                request.Name,
                request.ODI,
                request.Tests,
                request.ODIRuns,
                request.TestRuns
            );

            var newId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCrickerterById), new { id = newId }, new { Id = newId });
        }

        // ═══════════════════════════════════════════════════════════
        // PUT METHOD (Write Side — Command)
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// PUT: api/cricketers/{id}
        /// Updates an existing cricketer.
        ///
        /// Note: The {id} comes from the URL, the rest of the data from the body.
        /// We combine them into the UpdateCricketerCommand.
        ///
        /// HTTP Status Codes:
        /// - 200 OK: update successful
        /// - 404 Not Found: no cricketer with this ID
        /// </summary>
        /// 
        [HttpPut("id")]
        public async Task<IActionResult> UpdateCricketer([FromRoute] int id, [FromBody] UpdateCricketerRequest request)
        {
            var command = new UpdateCricketerCommand(
                        id,
                        request.Name,
                        request.ODI,
                        request.Tests,
                        request.ODIRuns,
                        request.TestRuns
                    );
            var success = await _mediator.Send(command);

            if (!success)
                return NotFound(new { Message = $"Cricketer with ID {id} was not found." }); // 404

            return Ok(new { Message = "Cricketer updated successfully." }); // 200 OK
        }

        // ═══════════════════════════════════════════════════════════
        // DELETE METHOD (Write Side — Command)
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// DELETE: api/cricketers/{id}
        /// Deletes a cricketer by their ID.
        ///
        /// HTTP Status Codes:
        /// - 204 No Content: deleted successfully (no body returned — standard for DELETE)
        /// - 404 Not Found: no cricketer with this ID
        /// </summary>
        /// 
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCrickter([FromRoute] int id) {
            var command = new DeleteCricketerCommand(id);
            var response = await _mediator.Send(command);

            if (!response)
                return NotFound(new { Message = $"Cricketer with ID {id} was not found." }); // 404

            return NoContent(); // 204 No Content
        }

        // ═══════════════════════════════════════════════════════════════════════════════
        // REQUEST MODELS
        // These are the shapes of the JSON body the client sends for POST and PUT.
        // Separate from DTOs — these are INPUT models, DTOs are OUTPUT models.
        // ═══════════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// The JSON body shape for POST (create) requests.
        /// </summary>
        public class AddCricketerRequest
        {
            public string Name { get; set; } = string.Empty;
            public int ODI { get; set; }
            public int Tests { get; set; }
            public int ODIRuns { get; set; }
            public int TestRuns { get; set; }
        }

        /// <summary>
        /// The JSON body shape for PUT (update) requests.
        /// Same fields as Add — we require all fields to be provided on update.
        /// </summary>
        public class UpdateCricketerRequest
        {
            public string Name { get; set; } = string.Empty;
            public int ODI { get; set; }
            public int Tests { get; set; }
            public int ODIRuns { get; set; }
            public int TestRuns { get; set; }
        }
    }
}
