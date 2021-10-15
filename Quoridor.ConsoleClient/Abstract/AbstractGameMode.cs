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
    protected readonly ITurnService _turnService;
    protected readonly ConsoleView _consoleView;

    protected int _currentPlayerId = 1;
    protected bool _isGameEnded;

    protected AbstractGameMode(int playersCount)
    {
        var gameField = new GameField(playersCount);
        _consoleView = new ConsoleView(gameField);
        _turnService = new TurnService(gameField, new WavePathFinder(), OnPlayerReachedFinish);

        InitializeWithView(_consoleView);
    }

    protected void OnPlayerReachedFinish(int playerId)
    {
        _consoleView.SetFieldChanged();
        _isGameEnded = true;
        _consoleView.Clear();
        _consoleView.WriteLine($"Player with ID {playerId} won!\nPress any key...");
        _consoleView.Redraw();
        Console.ReadKey();
    }

    public abstract void StartMainCycle();

    protected void MakePlayerTurn()
    {
        var turn = RequestTurnMenuOption();
        switch (turn)
        {
            case TurnMenuOptions.Move:
                MakeMove();
                break;
            case TurnMenuOptions.SetWall:
                SetWall();
                break;
        }
    }

    protected void MakeMove()
    {
        var possibleMoves = _turnService.GetPossibleMoves(_currentPlayerId);
        var direction = RequestMoveDirection(possibleMoves);
        _turnService.TryMove(direction, _currentPlayerId);
        _consoleView.SetFieldChanged();
    }

    protected void SetWall()
    {
        var wall = RequestWall();
        _turnService.TrySetWall(wall, _currentPlayerId);
        _consoleView.SetFieldChanged();
    }
}