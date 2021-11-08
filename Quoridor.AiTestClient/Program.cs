using HavocAndCry.Quoridor.AiTestClient;

var game = new GameController();
try
{
    game.StartGame();
}
catch (Exception e)
{
    File.AppendAllText(
        @"./log.jsonc", 
        $"\n\n//[{DateTime.Now}] Some unhandled exception happened: {e.Message}, {e?.InnerException?.Message}\n{e?.StackTrace}\n\n");
    
    throw;
}
