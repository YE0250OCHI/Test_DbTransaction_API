using SimpleManualDispatcher.Shared.Domain;
using SimpleManualDispatcher.Vehicle.UI.Api;
using SimpleManualDispatcher.Vehicle.UI.View;
using System.Text;

namespace SimpleManualDispatcher.Vehicle.UI;

public class VehicleManager(IConsoleWriter consoleWriter, IGettableVehicleState gettableVehicle)
{
    private const string CliReset = "\u001b[0m";
    private const string CliRed = "\u001b[91m";
    private const string CliGreen = "\u001b[92m";


    private SimpleManualDispatcher.Shared.Domain.Vehicle? _vehicle;

    //private string _command = "";



    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        StringBuilder lineBuffer = new();
        StringBuilder inputBuilder = new();
        (int? currentLeft, int? currentTop) cursorPos = (null, null);
        bool isOnBusiness = false;
        bool isExiting = false;

        while (!cancellationToken.IsCancellationRequested)
        {
            // === バッファ初期化 ===
            lineBuffer.Clear();
            consoleWriter.Clear();

            // === 終了処理 ===
            if (isExiting)
            {
                consoleWriter.Refresh();
                consoleWriter.WriteLine("Application exit.");
                break;
            }

            // === 初期表示 ===
            if (!isOnBusiness)
            {
                // ログイン表示
                lineBuffer.Append("Enter Vehicle ID   : ");
            }
            else
            {
                // ID
                consoleWriter.AddLine($"Current Vehicle ID : {_vehicle?.Id}");
                // 車両状態
                var state = (_vehicle?.Status ?? VehicleState.None).ToString();
                consoleWriter.AddLine($"Vehicle Status     : {state}");
                // 現在のJOB
                var jobId = _vehicle?.CurrentJobId ?? 0;
                consoleWriter.AddLine($"Assigned Job ID    : {(jobId > 0 ? jobId.ToString() : "N/A")}");
                // 担当者
                consoleWriter.AddLine($"Driver Name        : {_vehicle?.DriverName}");
                // 現在地
                consoleWriter.AddLine($"Current Location   : {_vehicle?.Location}");
            }

            // === 入力計算 ===
            if (Console.KeyAvailable)
            {
                var input = Console.ReadKey(intercept: true);

                if (input.Key == ConsoleKey.Enter)
                {
                    /* エンター：確定 */
                    if (!isOnBusiness)
                    {
                        /* 初期状態 */
                        string inputRow = inputBuilder.ToString();
                        inputBuilder.Clear();

                        var result = await TryLoginAsync(inputRow);
                        if (result)
                        {
                            cursorPos = (null, null);
                            isOnBusiness = true;
                        }
                        else
                        {
                            consoleWriter.WriteLine("ID is invalid or not found.", OutputColor.Red);
                            cursorPos = (null, null);
                        }

                        // 入力クリア
                        inputBuilder.Clear();
                    }
                    else
                    {
                        /* 営業状態 */
                        string command = inputBuilder.ToString();
                        inputBuilder.Clear();

                        if (!string.IsNullOrWhiteSpace(command))
                        {
                            if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                isExiting = true;
                                continue;
                            }
                            else if (command.Equals("logout", StringComparison.OrdinalIgnoreCase))
                            {
                                _vehicle = null;
                                isOnBusiness = false;
                                consoleWriter.Refresh();
                                continue;
                            }
                            else
                            {
                                var result = await TryExecuteCommandAsync(command);
                                if (result)
                                {
                                    inputBuilder.Clear();
                                }
                            }
                        }

                    }
                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    /* BS：1文字消す */
                    if(inputBuilder.Length > 0)
                    {
                        inputBuilder.Length--; // 1文字消す
                    }
                }
                else if (input.KeyChar != '\u0000' && !char.IsControl(input.KeyChar))
                {
                    /* その他：入力 */
                    inputBuilder.Append(input.KeyChar);
                }
            }

            lineBuffer.Append(inputBuilder.ToString());
            consoleWriter.AddLine(lineBuffer.ToString());

            // === 表示 ===
            consoleWriter.FlushWithoutClear(cursorPos.currentLeft, cursorPos.currentTop); // 表示
            cursorPos = consoleWriter.CursorPosition;

            await Task.Delay(42, cancellationToken); // 秒間 24回更新
        }
    }


    // ====================
    //   ヘルパー
    // ====================

    // ログイン処理
    private async Task<bool> TryLoginAsync(string id)
    {
        if (!int.TryParse(id, out var input))
        {
            return false;
        }

        // 認証
        var vehicle = await gettableVehicle.GetVehicleAsync(input);
        if (vehicle is null)
        {
            return false;
        }

        _vehicle = vehicle;
        return true;
    }

    // コマンド処理
    private async Task<bool> TryExecuteCommandAsync(string command)
    {
        return true;
    }

}
