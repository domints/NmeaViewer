using NmeaViewer;
using Serilog;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;
using Terminal.Gui.Views;

var logName = $"logs{DateTime.Now.ToString("yyyyMMdd")}.log";
if (System.Diagnostics.Debugger.IsAttached)
{
    if (File.Exists(logName))
        File.Delete(logName);
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs.log",  rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14)
    .CreateLogger();

//ConfigurationManager.Enable(ConfigLocations.All);
Log.Information("Starting application");
Application.Run<PortSelectionWindow>().Dispose();
//Application.Run<DataDisplayWindow>().Dispose();

// Before the application exits, reset Terminal.Gui for clean shutdown
Application.Shutdown();