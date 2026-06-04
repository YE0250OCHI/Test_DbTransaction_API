namespace SimpleManualDispatcher.Server.API.Domain;

public enum JobStatus
{
    NotAssigned = 0,    // 未割当
    NotActive,          // 割当済み、実行待ち
    Active,             // 実行中
    Completed,          // 正常終了
    Canceled,           // キャンセル
    Aborted             // 中断された
}
