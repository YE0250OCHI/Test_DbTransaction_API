namespace SimpleManualDispatcher.Vehicle.UI.View;

public interface IConsoleWriter
{
    // カーソル位置
    (int currentLeft, int currentTop) CursorPosition { get; }

    // バッファ削除
    void Clear();

    // バッファに文字列追加（改行つき）
    void AddLine(string text);

    // バッファを表示
    void FlushWithoutClear(int? left, int? top);

    // 即時表示（改行あり）
    void WriteLine(string text);

    // 即時表示（改行あり、色付き）
    void WriteLine(string text, OutputColor color);

    // 全消し
    void Refresh();
}
