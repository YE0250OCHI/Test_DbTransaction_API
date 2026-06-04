using System.Text;
using System.Text.RegularExpressions;

namespace SimpleManualDispatcher.Vehicle.UI.View;

public partial class ConsoleWriter: IConsoleWriter
{
    private const int StringWidth = 64;

    private readonly StringBuilder _viewBuffer = new();

    // カーソル位置
    public (int currentLeft, int currentTop) CursorPosition =>
        Console.GetCursorPosition();

    // バッファ削除
    public void Clear()
    {
        _viewBuffer.Clear();
    }

    // バッファに文字列追加（改行つき）
    public void AddLine(string text)
    {
        _viewBuffer.AppendLine(FormattedString(text)); // 1行追加
    }

    // バッファを表示
    public void FlushWithoutClear(int? left = null, int? top = null )
    {
        var viewBuffer = _viewBuffer.ToString();

        Console.SetCursorPosition(0, 0);
        Console.Write(viewBuffer);

        (int affectedLeft, int affectedTop) = Console.GetCursorPosition();

        int nextLeft = left ?? affectedLeft;
        int nextTop = top ?? affectedTop;

        if (nextLeft >= 0 && nextLeft < Console.BufferWidth &&
            nextTop >= 0 && nextTop < Console.BufferHeight) 
        {
            Console.SetCursorPosition(nextLeft, nextTop);
        }
    }

    // 即時表示（改行付き）
    public void WriteLine(string text)
    {
        Console.WriteLine(FormattedString(text));
    }

    // 即時表示（改行付き）
    public void WriteLine(string text, OutputColor color)
    {
        var consoleColor = color switch
        {
            OutputColor.Red => ConsoleColor.Red,
            OutputColor.Green => ConsoleColor.Green,
            _ => ConsoleColor.White
        };

        Console.ForegroundColor = consoleColor;
        Console.WriteLine(FormattedString(text));
        Console.ForegroundColor = ConsoleColor.White;
    }

    // 全消し
    public void Refresh()
    {
        Console.Clear();
    }


    // === private ===

    // 文字幅の取得
    private static string FormattedString(string target)
    {
        int width = 0;
        foreach (var c in target)
        {

            if (char.IsControl(c)) continue;

            // Unicodeの範囲から「全角（幅2）」となる主な領域を判定
            if ((c >= 0x3000 && c <= 0x30FF) || // ひらがな・カタカナ・句読点
                (c >= 0x4E00 && c <= 0x9FFF) || // CJK統合漢字
                (c >= 0xFF00 && c <= 0xFFEF))   // 全角英数・全角記号 (ただし半角カナ領域を除く)
            {
                // ※0xFF61～0xFF9Fの「半角カタカナ」は幅1にする
                if (c >= 0xFF61 && c <= 0xFF9F)
                {
                    width += 1;
                }
                else
                {
                    width += 2;
                }
            }
            else
            {
                width += 1;
            }
        }

        var spaces = new string('_', Math.Max(StringWidth - width, 0));

        return $"{target}{spaces}";
    }

}
