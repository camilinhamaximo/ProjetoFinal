using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ProjetoFinal.API.Contracts;
using ProjetoFinal.API.Data;
using ProjetoFinal.API.Middlewares;
using ProjetoFinal.API.Repositories;
using ProjetoFinal.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value!.Errors
                    .Select(_ => "O valor informado é inválido.")
                    .ToArray());

        return new BadRequestObjectResult(
            new ApiErrorResponse(400, "VALIDATION_ERROR", "Dados informados são inválidos.", errors));
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "A connection string 'DefaultConnection' deve ser configurada por variável de ambiente ou arquivo local ignorado pelo Git.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IChamadoService, ChamadoService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
