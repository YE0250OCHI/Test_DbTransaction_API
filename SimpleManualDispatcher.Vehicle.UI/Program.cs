using Microsoft.Extensions.DependencyInjection;
using SimpleManualDispatcher.Vehicle.UI;
using SimpleManualDispatcher.Vehicle.UI.Api;
using SimpleManualDispatcher.Vehicle.UI.View;

var services = new ServiceCollection();

services.Configure<ApiClientOptions>(options =>
{
    options.BaseUriString = "https://localhost:7276";
});

services.AddTransient<IGettableVehicleState, VehicleStateApi>();
services.AddTransient<IConsoleWriter,ConsoleWriter>();

services.AddSingleton<VehicleManager>();

var builder = services.BuildServiceProvider();

var app = builder.GetRequiredService<VehicleManager>();
var writer = builder.GetRequiredService<IConsoleWriter>();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    Console.CursorVisible = false;
    await app.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    writer.Refresh();
    writer.WriteLine("Cancel Key Pressed...",OutputColor.Red);
}
finally
{
    Console.CursorVisible = true;
}
