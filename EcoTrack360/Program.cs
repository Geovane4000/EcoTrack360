using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

[BsonIgnoreExtraElements]
public class EnvironmentalImpact
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; }
	public double CarbonFootprint { get; set; }
	public double WaterUsage { get; set; }
	public double WasteGenerated { get; set; }
}

public class SocialAction
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; }
	public int EmployeesInvolved { get; set; }
	public int CommunityProjects { get; set; }
}

public class GovernanceIndicator
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; }
	public double AuditScore { get; set; }
	public bool Compliance { get; set; }
}

public class SustainableProject
{
	public ObjectId Id { get; set; }
	public string CompanyName { get; set; }
	public int NumberOfProjects { get; set; }
	public double InvestmentAmount { get; set; }
}

public class Employee
{
	public ObjectId Id { get; set; }
	public string Name { get; set; }
	public string CompanyName { get; set; }
	public int VolunteerHours { get; set; }
}

class Program
{
        static async Task Main()
		{
			Console.WriteLine("Iniciando EcoTrack360...");
			Console.WriteLine("Conectando ao MongoDB...");

			// DevOps-friendly: usar variável de ambiente MONGO_CONN
			var connectionString = Environment.GetEnvironmentVariable("MONGO_CONN")
				?? "mongodb://localhost:27017";

			var cliente = new MongoClient(connectionString);
			var banco = cliente.GetDatabase("EcoTrack360");

		var ambiental = banco.GetCollection<EnvironmentalImpact>("EnvironmentalImpacts");
		var social = banco.GetCollection<SocialAction>("SocialActions");
		var governanca = banco.GetCollection<GovernanceIndicator>("GovernanceIndicators");
		var sustentavel = banco.GetCollection<SustainableProject>("SustainableProjects");
		var funcionarios = banco.GetCollection<Employee>("Employees");

		// Collection flexível (schema livre)
		var ambientalFlexivel = banco.GetCollection<BsonDocument>("EnvironmentalImpacts");

        Console.WriteLine("Conectado ao MongoDB");

		// CREATE — 10 documentos por collection
		for (int i = 1; i <= 10; i++)
		{
			await ambiental.InsertOneAsync(new EnvironmentalImpact
			{
				CompanyName = $"Empresa {i}",
				CarbonFootprint = i * 100,
				WaterUsage = i * 50,
				WasteGenerated = i * 20
			});

			await social.InsertOneAsync(new SocialAction
			{
				CompanyName = $"Empresa {i}",
				EmployeesInvolved = i * 5,
				CommunityProjects = i
			});

			await governanca.InsertOneAsync(new GovernanceIndicator
			{
				CompanyName = $"Empresa {i}",
				AuditScore = 7 + i * 0.1,
				Compliance = true
			});

			await sustentavel.InsertOneAsync(new SustainableProject
			{
				CompanyName = $"Empresa {i}",
				NumberOfProjects = i,
				InvestmentAmount = i * 10000
			});

			await funcionarios.InsertOneAsync(new Employee
			{
				Name = $"Funcionario {i}",
				CompanyName = $"Empresa {i}",
				VolunteerHours = i * 3
			});
		}

		// SCHEMA FLEXÍVEL — documento com estrutura diferente
		await ambientalFlexivel.InsertOneAsync(new BsonDocument
		{
			{ "CompanyName", "Empresa Verde" },
			{ "TreesPlanted", 500 },
			{ "EnergyType", "Solar" }
		});

		Console.WriteLine("Dados inseridos com sucesso!");

		// READ
		var total = await ambiental.Find(_ => true).ToListAsync();
		Console.WriteLine($"Total de registros ambientais: {total.Count}");

		// UPDATE
		var atualizacao = Builders<EnvironmentalImpact>.Update.Set(x => x.WaterUsage, 999);
		await ambiental.UpdateOneAsync(x => x.CompanyName == "Empresa 1", atualizacao);

		Console.WriteLine("Registro atualizado com sucesso!");

		// DELETE
		await social.DeleteOneAsync(x => x.CompanyName == "Empresa 2");

		Console.WriteLine("Registro deletado com sucesso!");

        Console.WriteLine("CRUD completo executado.");
		Console.WriteLine("Encerrando em 5 segundos...");
		await Task.Delay(TimeSpan.FromSeconds(5));
		Console.WriteLine("Aplicação finalizada com sucesso.");
	}
}
