namespace SimpleManualDispatcher.Shared.Domain;

public enum VehicleState
{
    None = 0,       // 初期状態

    NotAssigned,    // 待機中、JOBが割当られていない

    Enroute,        // 移動中
    Parked,         // 停車中
    Acquiring,      // 乗車中
    Depositing,     // 降車中

    Removed,        // オフライン状態
}
