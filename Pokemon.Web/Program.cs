using Pokemon.Domain.QueryModel.Queries.Pokemons;
using Pokemon.Infrastructure.ExternalServices;
using Pokemon.Persistence.Repositories;
using Pokemon.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Pokemon.Persistence.Contexts;
using Pokemon.Domain.Interfaces;
using Pokemon.Persistence.Sql;
using Pokemon.Web.Middleware;
using Pokemon.Infrastructure;
using Pokemon.Web.Filters;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerResponseFilter>();
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(PokemonsListQuery).Assembly);
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<PokemonDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IPokeApiService, PokeApiService>(client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon API V1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowFrontend");

app.UseAuthorization();
app.MapControllers();
app.Run();