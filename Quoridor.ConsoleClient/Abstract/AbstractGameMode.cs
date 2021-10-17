using System;
using HavocAndCry.Quoridor.ConsoleClient.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Model.Services;
using static HavocAndCry.Quoridor.ConsoleClient.Menu;

namespace HavocAndCry.Quoridor.ConsoleClient.Abstract;

public abstract class AbstractGameMode : IGameMode
{
    protected readonly ITurnService TurnService;
    protected readonly ConsoleView ConsoleView;
    protected readonly IGameField GameField;

    protected int CurrentPlayerId = 1;
    protected bool IsGameEnded;

    protected AbstractGameMode(int playersCount)
    {
        GameField = new GameField(playersCount);
        ConsoleView = new ConsoleView(GameField);
        TurnService = new TurnService(GameField, new WavePathFinder(), OnPlayerReachedFinish);

        InitializeWithView(ConsoleView);
    }

    private void OnPlayerReachedFinish(int playerId)
    {
        ConsoleView.SetFieldChanged();
        IsGameEnded = true;
        ConsoleView.Clear();
        ConsoleView.WriteLine($"Player with ID {playerId} won!\nPress any key...");
        ConsoleView.Redraw();
        Console.ReadKey();
    }

    public abstract void StartMainCycle();

    protected void MakePlayerTurn()
    {
        var turn = RequestTurnMenuOption();
        switch (turn)
        {
            case TurnType.Move:
                MakeMove();
                break;
            case TurnType.SetWall:
                SetWall();
                break;
        }
    }

    protected void MakeMove()
    {
        var possibleMoves = TurnService.GetPossibleMoves(CurrentPlayerId);
        var direction = RequestMoveDirection(possibleMoves);
        TurnService.TryMove(direction, CurrentPlayerId);
        ConsoleView.SetFieldChanged();
    }

    protected void SetWall()
    {
        var wall = RequestWall();
        TurnService.TrySetWall(wall, CurrentPlayerId);
        ConsoleView.SetFieldChanged();
    }
}