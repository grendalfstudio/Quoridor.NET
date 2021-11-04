using System;
using System.Linq;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using HavocAndCry.Quoridor.Core.Pathfinding;
using HavocAndCry.Quoridor.Core.Services;
using HavocAndCry.Quoridor.TUI.Models;
using static HavocAndCry.Quoridor.TUI.Menu;

namespace HavocAndCry.Quoridor.TUI.Abstract;

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
        if (GameField.Players.First(p => p.PlayerId == CurrentPlayerId).WallsCount > 0)
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
        else
        {
            MakeMove();
        }
    }

    protected void MakeMove()
    {
        var success = false;
        while(!success){
            var possibleMoves = TurnService.GetPossibleMoves(CurrentPlayerId);
            var direction = RequestMovePosition(possibleMoves);
            if (!TurnService.TryMove(direction, CurrentPlayerId))
            {
                ConsoleView.WriteLine("Can't make this move, select another");
                ConsoleView.Redraw();
            }
            else
            {
                success = true;
                ConsoleView.SetFieldChanged();
            }
        }
    }

    protected void SetWall()
    {
        var success = false;
        while(!success){
            var wall = RequestWall();
            if (!TurnService.TrySetWall(wall, CurrentPlayerId))
            {
                ConsoleView.WriteLine("Can't place wall here, select another place\nPress any key...");
                ConsoleView.Redraw();
                Console.ReadKey();
            }
            else
            {
                success = true;
                ConsoleView.SetFieldChanged();
            }
        }
    }
}