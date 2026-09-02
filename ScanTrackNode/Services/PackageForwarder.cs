using System.Text;
using System.Text.Json;
using ScanTrackNode.Models;

namespace ScanTrackNode.Services;

public class PackageForwarder
{
    private readonly NodeRegistry _registry;
    private readonly IHttpClientFactory _factory;
    private readonly ILogger<PackageForwarder> _logger;

    public PackageForwarder(NodeRegistry registry, IHttpClientFactory factory, ILogger<PackageForwarder> logger)
    {
        _registry = registry;
        _factory = factory;
        _logger = logger;
    }

    public async Task<bool> ForwardAsync(Package package, string nextCity)
    {
        // 1. Hämta nodlistan (stad → url) från registret
        var nodes = await _registry.GetNodesAsync();

        // 2. Slå upp URL:en för nästa stad
        if (!nodes.TryGetValue(nextCity, out var url))
        {
            _logger.LogError("Kunde inte vidarebefordra {PackageId}: staden {City} finns inte i nodlistan",
                package.PackageId, nextCity);
            return false;
        }

        // 3. Serialisera paketet till JSON
        var json = JsonSerializer.Serialize(package);

        // 4. Skapa HTTP-body
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // 5. Skapa en HttpClient via factory
        var http = _factory.CreateClient();

        // 6. Skicka POST-anrop till nästa nods /paket-endpoint
        var response = await http.PostAsync($"{url}/paket", content);

        // 7. Logga vad som skickades
        _logger.LogInformation("Skickade paket {PackageId} till {City} ({Url})",
            package.PackageId, nextCity, url);

        // 8. Returnera om det lyckades
        return response.IsSuccessStatusCode;
    }
}
