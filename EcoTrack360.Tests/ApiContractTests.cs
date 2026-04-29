using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NJsonSchema;
using Xunit;

public class ApiContractTests
{
    private const string UnavailableMongoConnection = "mongodb://127.0.0.1:27079/?connectTimeoutMS=500&serverSelectionTimeoutMS=500";
    private const string AvailableMongoConnection = "mongodb://localhost:27017";

    [Fact]
    public async Task Get_Ready_Should_Return_200_With_Valid_Contract_When_Mongo_Online()
    {
        var mongoConnection = Environment.GetEnvironmentVariable("MONGO_TEST_CONN") ?? AvailableMongoConnection;
        using var client = CreateClient(mongoConnection);
        var response = await client.GetAsync("/health/ready");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        var schema = await JsonSchema.FromJsonAsync("""
        {
          "type": "object",
          "required": ["status", "database", "timestamp"],
          "properties": {
            "status": { "type": "string" },
            "database": { "type": "string" },
            "timestamp": { "type": "string" }
          },
          "additionalProperties": true
        }
        """);

        var errors = schema.Validate(body);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Post_Seed_Should_Return_200_With_Valid_Contract_When_Mongo_Online()
    {
        var mongoConnection = Environment.GetEnvironmentVariable("MONGO_TEST_CONN") ?? AvailableMongoConnection;
        using var client = CreateClient(mongoConnection);
        var response = await client.PostAsync("/seed", content: null);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        var schema = await JsonSchema.FromJsonAsync("""
        {
          "type": "object",
          "required": ["seeded", "database"],
          "properties": {
            "seeded": { "type": "boolean" },
            "database": { "type": "string" }
          },
          "additionalProperties": true
        }
        """);

        var errors = schema.Validate(body);
        errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("/health", HttpStatusCode.OK, """
    {
      "type": "object",
      "required": ["status", "service", "timestamp"],
      "properties": {
        "status": { "type": "string" },
        "service": { "type": "string" },
        "timestamp": { "type": "string" }
      },
      "additionalProperties": true
    }
    """)]
    [InlineData("/health/live", HttpStatusCode.OK, """
    {
      "type": "object",
      "required": ["status", "service"],
      "properties": {
        "status": { "type": "string" },
        "service": { "type": "string" }
      },
      "additionalProperties": true
    }
    """)]
    public async Task Get_Endpoints_Should_Respect_Contract(string route, HttpStatusCode expectedStatus, string schemaJson)
    {
        using var client = CreateClient(UnavailableMongoConnection);
        var response = await client.GetAsync(route);
        response.StatusCode.Should().Be(expectedStatus);

        var body = await response.Content.ReadAsStringAsync();
        var schema = await JsonSchema.FromJsonAsync(schemaJson);
        var errors = schema.Validate(body);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_Root_Should_Return_200_Or_Graceful_Response_When_Mongo_Offline()
    {
        using var client = CreateClient(UnavailableMongoConnection);
        var response = await client.GetAsync("/");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(body).RootElement;
        json.TryGetProperty("service", out _).Should().BeTrue();
        json.TryGetProperty("database", out _).Should().BeTrue();
        json.TryGetProperty("connected", out _).Should().BeTrue();
    }

    [Fact]
    public async Task Get_Ready_Should_Return_503_With_ProblemDetails_When_Mongo_Offline()
    {
        using var client = CreateClient(UnavailableMongoConnection);
        var response = await client.GetAsync("/health/ready");
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var body = await response.Content.ReadAsStringAsync();
        var schema = await JsonSchema.FromJsonAsync("""
        {
          "type": "object",
          "required": ["type", "title", "status"],
          "properties": {
            "type": { "type": "string" },
            "title": { "type": "string" },
            "status": { "type": "integer" }
          },
          "additionalProperties": true
        }
        """);

        var errors = schema.Validate(body);
        errors.Should().BeEmpty();
    }

    [Fact]
    public async Task Post_Seed_Should_Return_503_With_ProblemDetails_When_Mongo_Offline()
    {
        using var client = CreateClient(UnavailableMongoConnection);
        var response = await client.PostAsync("/seed", content: null);
        response.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        var body = await response.Content.ReadAsStringAsync();
        var schema = await JsonSchema.FromJsonAsync("""
        {
          "type": "object",
          "required": ["type", "title", "status"],
          "properties": {
            "type": { "type": "string" },
            "title": { "type": "string" },
            "status": { "type": "integer" }
          },
          "additionalProperties": true
        }
        """);

        var errors = schema.Validate(body);
        errors.Should().BeEmpty();
    }

    private static HttpClient CreateClient(string mongoConnection)
    {
        Environment.SetEnvironmentVariable("MONGO_CONN", mongoConnection);
        Environment.SetEnvironmentVariable("APP_SEED_ON_STARTUP", "false");
        var factory = new WebApplicationFactory<Program>();
        return factory.CreateClient();
    }
}
