namespace SimpleManualDispatcher.Server.API.Domain;

public record Vehicle(
    int Id,
    VehicleState Status,
    int CurrentJobId,
    string DriverName,  // フレーバー
    string Location     // フレーバー
);
