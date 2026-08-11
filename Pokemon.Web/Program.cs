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
using Pokemon.Persistence.Sql.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

static string ResolveConnectionString(IConfiguration configuration, IWebHostEnvironment environment)
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var isPlaceholder = string.IsNullOrWhiteSpace(connectionString)
        || connectionString.Contains("{{", StringComparison.Ordinal);

    var looksLikeSqlServer = !string.IsNullOrWhiteSpace(connectionString)
        && (connectionString.Contains("MultipleActiveResultSets", StringComparison.OrdinalIgnoreCase)
            || connectionString.Contains("Initial Catalog", StringComparison.OrdinalIgnoreCase)
            || connectionString.Contains("Trusted_Connection", StringComparison.OrdinalIgnoreCase));

    const string localPostgres =
        "Host=localhost;Port=5432;Database=pokemon;Username=postgres;Password=123qwe";

    if (environment.IsDevelopment() && (isPlaceholder || looksLikeSqlServer))
    {
        return localPostgres;
    }

    if (isPlaceholder)
    {
        throw new InvalidOperationException(
            "Connection string not configured. Set ConnectionStrings__DefaultConnection or update appsettings.json.");
    }

    return connectionString!;
}

var connectionString = ResolveConnectionString(builder.Configuration, builder.Environment);

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
    options.UseNpgsql(connectionString));
builder.Services.AddDbContext<PokemonDbContext>(option =>
    option.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IPokeApiService, PokeApiService>(client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    client.Timeout = TimeSpan.FromMinutes(3);
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

// ─── Auto-migrate + Seed desde PokéAPI ───────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PokemonDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Aplicando migraciones...");
        await db.Database.MigrateAsync();

        bool dbEmpty = !await db.Pokemonss.AnyAsync();
        if (dbEmpty)
        {
            logger.LogInformation("DB vacía — iniciando seed desde PokéAPI...");

            var pokeApiService = scope.ServiceProvider.GetRequiredService<IPokeApiService>();
            var pokemons = await pokeApiService.GetPokemonsFromApiAsync();

            foreach (var dto in pokemons)
            {
                // Upsert de tipos
                var typeNames = dto.Types ?? new List<string>();
                var types = new List<PokemonType>();

                foreach (var typeName in typeNames)
                {
                    var existing = await db.PokemonTypes.FirstOrDefaultAsync(t => t.Name == typeName);
                    if (existing == null)
                    {
                        existing = new PokemonType { Name = typeName };
                        db.PokemonTypes.Add(existing);
                        await db.SaveChangesAsync();
                    }
                    types.Add(existing);
                }

                var pokemon = new Pokemones
                {
                    Id = dto.Id,
                    Name = dto.Name ?? "",
                    Image = dto.Image,
                    Hp = dto.Hp,
                    Attack = dto.Attack,
                    Defense = dto.Defense,
                    Speed = dto.Speed,
                    Height = dto.Height,
                    Weight = dto.Weight,
                    Custom = false,
                    PokemonTypes = types
                };

                db.Pokemonss.Add(pokemon);
            }

            await db.SaveChangesAsync();
            logger.LogInformation("Seed completado: {Count} pokémon guardados.", pokemons.Count());
        }
        else
        {
            logger.LogInformation("DB ya tiene datos. Se omite el seed.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error durante migración/seed. La app continúa.");
    }
}
// ─────────────────────────────────────────────────────────────────────────────

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
