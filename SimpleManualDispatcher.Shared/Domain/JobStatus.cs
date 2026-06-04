namespace SimpleManualDispatcher.Shared.Domain;

public enum JobStatus
{
    None = 0,           // 初期状態
    NotActive,          // 実行待ち
    Active,             // 実行中
    Completed,          // 正常終了
    Canceled,           // キャンセル
    Aborted             // 中断された
}
