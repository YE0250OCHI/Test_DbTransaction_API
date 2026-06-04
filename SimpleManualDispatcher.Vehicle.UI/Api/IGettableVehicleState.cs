namespace SimpleManualDispatcher.Vehicle.UI.Api;

public interface IGettableVehicleState
{
    Task<Shared.Domain.Vehicle?> GetVehicleAsync(int vehicleId);

}
