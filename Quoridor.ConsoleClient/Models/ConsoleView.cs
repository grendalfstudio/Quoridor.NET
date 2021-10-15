using System;
using System.Text;
using HavocAndCry.Quoridor.Core.Abstract;

namespace HavocAndCry.Quoridor.ConsoleClient.Models;

public class ConsoleView
{
    private readonly GameFieldViewModel _fieldViewModel;
    private readonly IGameField _gameField;
    private string _upperPart;
    private StringBuilder _lowerPart;

    public ConsoleView(IGameField gameField)
    {
        _gameField = gameField;
        _fieldViewModel = new GameFieldViewModel(gameField);
        _lowerPart = new StringBuilder();
    }

    public int CurrentPlayerId { get; set; }

    public void SetFieldChanged() => _fieldViewModel.IsChanged = true;

    public void Redraw()
    {
        Console.Clear();

        BuildUpperPart();
        Console.WriteLine(_upperPart);

        Console.WriteLine(_lowerPart.ToString());
    }

    private void BuildUpperPart()
    {
        if (!_fieldViewModel.IsChanged)
            return;

        var builder = new StringBuilder();
        builder.Append("\tAvailable walls:\n\t\t");
        foreach (var player in _gameField.Players)
        {
            builder.AppendFormat("Player {0}: {1}\t", player.PlayerId, player.WallsCount);
        }

        _fieldViewModel.UpdateFieldView();
        builder.AppendLine(_fieldViewModel.PrintField());
        builder.Append($"\n\tCurrent player's ID is {CurrentPlayerId}\n");
        _fieldViewModel.IsChanged = false;

        _upperPart = builder.ToString();
    }

    public void Clear() => _lowerPart.Clear();

    public void WriteLine(string text) => _lowerPart.AppendLine(text);
}