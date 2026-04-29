using System.Net;
using FluentAssertions;
using NJsonSchema;
using Reqnroll;
using EcoTrack360.Tests.Support;

namespace EcoTrack360.Tests.StepDefinitions;

[Binding]
public sealed class ApiStepDefinitions
{
    private readonly ApiTestContext _context = new();

    [Given("que a API do EcoTrack360 está em execução")]
    public void GivenApiIsRunning()
    {
    }

    [Given("que o MongoDB está indisponível para a aplicação")]
    public void GivenMongoIsUnavailableForApplication()
    {
        _context.SetMongoUnavailable();
    }

    [Given("que o MongoDB está disponível para a aplicação")]
    public void GivenMongoIsAvailableForApplication()
    {
        _context.SetMongoAvailable();
    }

    [When("eu faço uma requisição GET para \"(.*)\"")]
    public async Task WhenIDoAGetTo(string route)
    {
        await _context.GetAsync(route);
    }

    [When("eu faço uma requisição POST para \"(.*)\"")]
    public async Task WhenIDoAPostTo(string route)
    {
        await _context.PostAsync(route);
    }

    [Then("o status da resposta deve ser (.*)")]
    public void ThenStatusCodeMustBe(int statusCode)
    {
        ((int)_context.EnsureStatusCode()).Should().Be(statusCode);
    }

    [Then("o JSON de resposta deve conter o campo \"(.*)\"")]
    public void ThenJsonMustContainField(string fieldName)
    {
        var root = _context.EnsureJsonRoot();
        root.TryGetProperty(fieldName, out _).Should().BeTrue();
    }

    [Then("o valor do campo \"(.*)\" deve ser \"(.*)\"")]
    public void ThenFieldValueMustBe(string fieldName, string expectedValue)
    {
        var root = _context.EnsureJsonRoot();
        root.TryGetProperty(fieldName, out var prop).Should().BeTrue();
        prop.GetString().Should().Be(expectedValue);
    }

    [Then("o contrato JSON da resposta deve ser válido para \"(.*)\"")]
    public async Task ThenResponseSchemaMustBeValidFor(string schemaName)
    {
        var root = _context.EnsureJsonRoot();
        var schemaJson = schemaName switch
        {
            "summary" => """
            {
              "type": "object",
              "required": ["service", "database", "connected"],
              "properties": {
                "service": { "type": "string" },
                "database": { "type": "string" },
                "connected": { "type": "boolean" }
              },
              "additionalProperties": true
            }
            """,
            "healthLive" => """
            {
              "type": "object",
              "required": ["status", "service"],
              "properties": {
                "status": { "type": "string" },
                "service": { "type": "string" }
              },
              "additionalProperties": true
            }
            """,
            "ready" => """
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
            """,
            "seed" => """
            {
              "type": "object",
              "required": ["seeded", "database"],
              "properties": {
                "seeded": { "type": "boolean" },
                "database": { "type": "string" }
              },
              "additionalProperties": true
            }
            """,
            "problem" => """
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
            """,
            _ => throw new InvalidOperationException($"Schema desconhecido: {schemaName}")
        };

        var schema = await JsonSchema.FromJsonAsync(schemaJson);
        var errors = schema.Validate(root.GetRawText());
        errors.Should().BeEmpty();
    }

    [Then("a resposta deve estar no formato problem details")]
    public async Task ThenResponseShouldBeProblemDetails()
    {
        var statusCode = _context.EnsureStatusCode();
        (statusCode == HttpStatusCode.ServiceUnavailable || statusCode == HttpStatusCode.BadRequest || statusCode == HttpStatusCode.InternalServerError)
            .Should().BeTrue();

        await ThenResponseSchemaMustBeValidFor("problem");
    }
}
