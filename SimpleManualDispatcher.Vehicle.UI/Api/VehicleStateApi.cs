using Microsoft.Extensions.Options;
using SimpleManualDispatcher.Shared.Domain;
using System.Net.Http.Json;

namespace SimpleManualDispatcher.Vehicle.UI.Api;

public class VehicleStateApi(IOptions<ApiClientOptions> options) : IGettableVehicleState
{
    private readonly string _baseUriString = options.Value.BaseUriString;
    private static readonly HttpClient _client = new();

    public async Task<Shared.Domain.Vehicle?> GetVehicleAsync(int vehicleId)
    {
        var uri = new Uri($"{_baseUriString}/api/vehicle/{vehicleId}");

        using var response = await _client.GetAsync(uri);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        else
        {
            response.EnsureSuccessStatusCode();
        }

        if (response.Content == null || response.Content.Headers.ContentLength == 0)
        {
            return null;
        }

        var vehicle = await response.Content.ReadFromJsonAsync<Shared.Domain.Vehicle>();

        return vehicle;
    }
}
