namespace SimpleManualDispatcher.Server.API.Domain;

public enum JobState
{
    None = 0,       // 初期状態

    Queued,         // 待機中
    Waiting,        // 実行開始（FROMまで移動中、回送含む）
    Canceling,      // JOBをキャンセルして終了

    Transferring,   // 処理中：乗降～移動～乗降
    Paused,         // 処理中：一時中断
    Aborting,       // 処理を中断して終了
}
