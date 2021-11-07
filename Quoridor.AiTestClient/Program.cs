using HavocAndCry.Quoridor.AiTestClient;
using Serilog;
using Serilog.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log-.txt").CreateLogger();

var game = new GameController();
try
{
    game.StartGame();
}
catch (Exception e)
{
    Log.Fatal(e, "Some unhandled exception happened");
    File.AppendAllText(@"./log.jsonc", $"\n\n//[{DateTime.Now}] Some unhandled exception happened: {e.Message}, {e?.InnerException?.Message}\n{e.StackTrace}\n\n");
    throw;
}
