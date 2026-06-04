using Microsoft.AspNetCore.Mvc;
using SimpleManualDispatcher.Shared.Domain;

namespace SimpleManualDispatcher.Server.API.Controllers;

[Route("api/vehicle")]
[ApiController]
public class ValuesController : ControllerBase
{
    private static readonly List<Vehicle> _tempVehicle = [
        new(1, VehicleState.NotAssigned, 0, "Yamada", "Niihama_Eki"),
        new(2, VehicleState.NotAssigned, 1, "Tanaka", "Niihama_Shiyakusho"),
        new(3, VehicleState.NotAssigned, 0, "Suzuki", "Niihama_Eki"),
        ];

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle?>> GetVehicle(int id)
    {
        /* DBから非同期でデータを取る */
        var vehicle = _tempVehicle.FirstOrDefault(x => x.Id == id);
        return vehicle;
    }
}
