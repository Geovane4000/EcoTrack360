using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://0.0.0.0:8080");

builder.Services.AddSingleton(MongoSettings.FromEnvironment());
builder.Services.AddSingleton<MongoContext>();

var app = builder.Build();

app.MapGet("/", async (MongoContext mongoContext, CancellationToken cancellationToken) =>
{
	var summary = await mongoContext.GetSummaryAsync(cancellationToken);
	return Results.Ok(summary);
});

app.MapGet("/health", () => Results.Ok(new
{
	status = "ok",
	service = "EcoTrack360",
	timestamp = DateTimeOffset.UtcNow
}));

app.MapGet("/health/live", () => Results.Ok(new
{
	status = "live",
	service = "EcoTrack360"
}));

app.MapGet("/health/ready", async (MongoContext mongoContext, CancellationToken cancellationToken) =>
{
	if (!await mongoContext.CanConnectAsync(cancellationToken))
	{
		return Results.Problem("MongoDB indisponível.", statusCode: StatusCodes.Status503ServiceUnavailable);
	}

	return Results.Ok(new
	{
		status = "ready",
		database = mongoContext.DatabaseName,
		timestamp = DateTimeOffset.UtcNow
	});
});

app.MapPost("/seed", async (MongoContext mongoContext, CancellationToken cancellationToken) =>
{
	var seeded = await mongoContext.SeedIfEmptyAsync(cancellationToken);
	return Results.Ok(new
	{
		seeded,
		database = mongoContext.DatabaseName
	});
});

await app.Services.GetRequiredService<MongoContext>().TrySeedOnStartupAsync(app.Logger, app.Lifetime.ApplicationStopping);
app.Run();

[BsonIgnoreExtraElements]
public class EnvironmentalImpact
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public double CarbonFootprint { get; set; }
	public double WaterUsage { get; set; }
	public double WasteGenerated { get; set; }
}

public class SocialAction
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public int EmployeesInvolved { get; set; }
	public int CommunityProjects { get; set; }
}

public class GovernanceIndicator
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public double AuditScore { get; set; }
	public bool Compliance { get; set; }
}

public class SustainableProject
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; } = string.Empty;
	public int NumberOfProjects { get; set; }
	public double InvestmentAmount { get; set; }
}

public class Employee
{
	public ObjectId Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string CompanyName { get; set; } = string.Empty;
	public int VolunteerHours { get; set; }
}

public sealed class MongoSettings
{
	public string ConnectionString { get; init; } = "mongodb://localhost:27017";
	public string DatabaseName { get; init; } = "EcoTrack360";
	public bool SeedOnStartup { get; init; } = true;

	public static MongoSettings FromEnvironment()
	{
		var connectionString = Environment.GetEnvironmentVariable("MONGO_CONN")
			?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION")
			?? Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
			?? "mongodb://localhost:27017";

		var databaseName = Environment.GetEnvironmentVariable("MONGO_DB_NAME")
			?? Environment.GetEnvironmentVariable("MONGODB_DATABASE")
			?? "EcoTrack360";

		var seedOnStartup = !bool.TryParse(Environment.GetEnvironmentVariable("APP_SEED_ON_STARTUP"), out var parsed)
			|| parsed;

		return new MongoSettings
		{
			ConnectionString = connectionString,
			DatabaseName = databaseName,
			SeedOnStartup = seedOnStartup
		};
	}
}

public sealed class MongoContext
{
	private readonly MongoSettings _settings;
	private readonly MongoClient _client;
	private readonly IMongoCollection<EnvironmentalImpact> _environmentalImpacts;
	private readonly IMongoCollection<SocialAction> _socialActions;
	private readonly IMongoCollection<GovernanceIndicator> _governanceIndicators;
	private readonly IMongoCollection<SustainableProject> _sustainableProjects;
	private readonly IMongoCollection<Employee> _employees;
	private readonly IMongoCollection<BsonDocument> _environmentalFlexible;

	public MongoContext(MongoSettings settings)
	{
		_settings = settings;
		_client = new MongoClient(settings.ConnectionString);
		var database = _client.GetDatabase(settings.DatabaseName);
		_environmentalImpacts = database.GetCollection<EnvironmentalImpact>("EnvironmentalImpacts");
		_socialActions = database.GetCollection<SocialAction>("SocialActions");
		_governanceIndicators = database.GetCollection<GovernanceIndicator>("GovernanceIndicators");
		_sustainableProjects = database.GetCollection<SustainableProject>("SustainableProjects");
		_employees = database.GetCollection<Employee>("Employees");
		_environmentalFlexible = database.GetCollection<BsonDocument>("EnvironmentalImpacts");
	}

	public string DatabaseName => _settings.DatabaseName;

	public async Task<bool> CanConnectAsync(CancellationToken cancellationToken)
	{
		try
		{
			await _client.GetDatabase("admin")
				.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1), cancellationToken: cancellationToken);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public async Task<object> GetSummaryAsync(CancellationToken cancellationToken)
	{
      try
		{
          var canConnect = await CanConnectAsync(cancellationToken);
			if (!canConnect)
			{
				return new
				{
					service = "EcoTrack360",
					database = _settings.DatabaseName,
					connected = false,
					message = "Aguardando conexão com o MongoDB.",
					timestamp = DateTimeOffset.UtcNow
				};
			}

			var environmentalCount = await _environmentalImpacts.CountDocumentsAsync(FilterDefinition<EnvironmentalImpact>.Empty, cancellationToken: cancellationToken);
			var socialCount = await _socialActions.CountDocumentsAsync(FilterDefinition<SocialAction>.Empty, cancellationToken: cancellationToken);
			var governanceCount = await _governanceIndicators.CountDocumentsAsync(FilterDefinition<GovernanceIndicator>.Empty, cancellationToken: cancellationToken);
			var sustainableCount = await _sustainableProjects.CountDocumentsAsync(FilterDefinition<SustainableProject>.Empty, cancellationToken: cancellationToken);
			var employeeCount = await _employees.CountDocumentsAsync(FilterDefinition<Employee>.Empty, cancellationToken: cancellationToken);

			return new
			{
				service = "EcoTrack360",
				database = _settings.DatabaseName,
				connected = true,
				collections = new
				{
					environmentalImpacts = environmentalCount,
					socialActions = socialCount,
					governanceIndicators = governanceCount,
					sustainableProjects = sustainableCount,
					employees = employeeCount
				},
				timestamp = DateTimeOffset.UtcNow
			};
		}
		catch (Exception ex)
		{
			return new
			{
				service = "EcoTrack360",
				database = _settings.DatabaseName,
				connected = false,
				message = ex.Message,
				timestamp = DateTimeOffset.UtcNow
			};
		}
	}

	public async Task TrySeedOnStartupAsync(ILogger logger, CancellationToken cancellationToken)
	{
		if (!_settings.SeedOnStartup)
		{
			logger.LogInformation("Seed inicial desabilitado.");
			return;
		}

		for (var attempt = 1; attempt <= 10; attempt++)
		{
           try
			{
             if (await CanConnectAsync(cancellationToken))
				{
					var seeded = await SeedIfEmptyAsync(cancellationToken);
					logger.LogInformation(seeded ? "Base populada com dados ESG." : "Base já populada. Nenhum seed adicional necessário.");
					return;
				}
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "MongoDB ainda não está pronto para seed na tentativa {Attempt}/10.", attempt);
			}

			logger.LogWarning("MongoDB indisponível na tentativa {Attempt}/10. Nova tentativa em 3 segundos.", attempt);
			await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
		}

		logger.LogWarning("Aplicação iniciada sem conexão ativa com o MongoDB.");
	}

	public async Task<bool> SeedIfEmptyAsync(CancellationToken cancellationToken)
	{
		var existingCount = await _environmentalImpacts.CountDocumentsAsync(FilterDefinition<EnvironmentalImpact>.Empty, cancellationToken: cancellationToken);
		if (existingCount > 0)
		{
			return false;
		}

		var environmentalDocuments = Enumerable.Range(1, 10)
			.Select(i => new EnvironmentalImpact
			{
				CompanyName = $"Empresa {i}",
				CarbonFootprint = i * 100,
				WaterUsage = i == 1 ? 999 : i * 50,
				WasteGenerated = i * 20
			})
			.ToList();

		var socialDocuments = Enumerable.Range(1, 10)
			.Where(i => i != 2)
			.Select(i => new SocialAction
			{
				CompanyName = $"Empresa {i}",
				EmployeesInvolved = i * 5,
				CommunityProjects = i
			})
			.ToList();

		var governanceDocuments = Enumerable.Range(1, 10)
			.Select(i => new GovernanceIndicator
			{
				CompanyName = $"Empresa {i}",
				AuditScore = 7 + (i * 0.1),
				Compliance = true
			})
			.ToList();

		var sustainableDocuments = Enumerable.Range(1, 10)
			.Select(i => new SustainableProject
			{
				CompanyName = $"Empresa {i}",
				NumberOfProjects = i,
				InvestmentAmount = i * 10000
			})
			.ToList();

		var employeeDocuments = Enumerable.Range(1, 10)
			.Select(i => new Employee
			{
				Name = $"Funcionario {i}",
				CompanyName = $"Empresa {i}",
				VolunteerHours = i * 3
			})
			.ToList();

		await _environmentalImpacts.InsertManyAsync(environmentalDocuments, cancellationToken: cancellationToken);
		await _socialActions.InsertManyAsync(socialDocuments, cancellationToken: cancellationToken);
		await _governanceIndicators.InsertManyAsync(governanceDocuments, cancellationToken: cancellationToken);
		await _sustainableProjects.InsertManyAsync(sustainableDocuments, cancellationToken: cancellationToken);
		await _employees.InsertManyAsync(employeeDocuments, cancellationToken: cancellationToken);

		await _environmentalFlexible.InsertOneAsync(new BsonDocument
		{
			{ "CompanyName", "Empresa Verde" },
			{ "TreesPlanted", 500 },
			{ "EnergyType", "Solar" }
		}, cancellationToken: cancellationToken);

		return true;
	}
}
