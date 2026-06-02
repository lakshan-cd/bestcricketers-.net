using BestCricketers.Application;
using BestCricketers.Infrastructure.Repositories;
using MediatR;

var builder = WebApplication.CreateBuilder(args);


// ─── Register Services (Dependency Injection) ───────────────────────────────
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(BestCricketers.Application.AssemblyReference).Assembly);
});

// Register the Repository
// "Scoped" means: create one instance per HTTP request, then dispose it.
// This is the standard lifetime for database-related services.
// Java comparison: Like @RequestScope in Spring.
var connectionString = builder.Configuration.GetConnectionString("CricketerDatabase")!;
builder.Services.AddScoped<ICricketerRepository>(
    _ => new CricketerRepository(connectionString)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
