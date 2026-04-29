using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EcoTrack360.Tests.Support;

public sealed class ApiTestContext
{
    private const string UnavailableMongoConnection = "mongodb://127.0.0.1:27079/?connectTimeoutMS=500&serverSelectionTimeoutMS=500";
    private const string AvailableMongoConnection = "mongodb://localhost:27017";

    private WebApplicationFactory<Program>? _factory;
    private HttpClient? _client;

    public HttpClient Client => _client ??= CreateClient();

    public HttpResponseMessage? LastResponse { get; private set; }

    public JsonDocument? LastJson { get; private set; }

    public ApiTestContext()
    {
        Environment.SetEnvironmentVariable("MONGO_CONN", UnavailableMongoConnection);
        Environment.SetEnvironmentVariable("APP_SEED_ON_STARTUP", "false");
    }

    public void SetMongoUnavailable()
    {
        SetMongoConnection(UnavailableMongoConnection);
    }

    public void SetMongoAvailable()
    {
        var mongoConnection = Environment.GetEnvironmentVariable("MONGO_TEST_CONN") ?? AvailableMongoConnection;
        SetMongoConnection(mongoConnection);
    }

    public async Task GetAsync(string route)
    {
        LastResponse = await Client.GetAsync(route);
        LastJson = await TryReadJsonAsync(LastResponse);
    }

    public async Task PostAsync(string route)
    {
        LastResponse = await Client.PostAsJsonAsync(route, new { });
        LastJson = await TryReadJsonAsync(LastResponse);
    }

    public HttpStatusCode EnsureStatusCode()
    {
        if (LastResponse is null)
        {
            throw new InvalidOperationException("Nenhuma resposta foi recebida.");
        }

        return LastResponse.StatusCode;
    }

    public JsonElement EnsureJsonRoot()
    {
        if (LastJson is null)
        {
            throw new InvalidOperationException("A resposta não contém JSON válido.");
        }

        return LastJson.RootElement;
    }

    private static async Task<JsonDocument?> TryReadJsonAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        try
        {
            return JsonDocument.Parse(content);
        }
        catch
        {
            return null;
        }
    }

    private void SetMongoConnection(string connection)
    {
        Environment.SetEnvironmentVariable("MONGO_CONN", connection);
        _client?.Dispose();
        _factory?.Dispose();
        _client = null;
        _factory = null;
    }

    private HttpClient CreateClient()
    {
        _factory = new WebApplicationFactory<Program>();
        return _factory.CreateClient();
    }
}
