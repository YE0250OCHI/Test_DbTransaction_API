namespace SimpleManualDispatcher.Server.API.Repository;

public interface IEditableRepository
{
    // JOBの登録・更新
    Task UpsertJobAsync();

    // 車両の登録・更新
    Task UpsertVehicleAsync();

    // JOB状態と車両状態の更新
    Task ChangeStateAsync();

    // JOBの削除
    Task DeleteJobAsync(ushort JobId);

    // 車両の削除
    Task DeleteVehicleAsync(ushort VehicleId);
}
