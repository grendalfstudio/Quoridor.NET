using System.Text;
using HavocAndCry.Quoridor.AiTestClient.Models;
using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;

namespace HavocAndCry.Quoridor.AiTestClient;

public static class ConsoleHelper
{
    public static PlayerColor GetColor()
    {
        var input = Console.ReadLine();

        var selectedColor = input switch
        {
            "black" => PlayerColor.Black,
            "white" => PlayerColor.White
        };

        return selectedColor;
    }

    public static Move GetTurn(PlayerColor color, IGameField gameField)
    {
        var input = Console.ReadLine();
        return ParseInput(input!, color, gameField);
    }

    public static void WriteTurn(Move move)
    {
        switch (move.TurnType)
        {
            case TurnType.Move:
                WriteMoveCommand(move);
                break;
            case TurnType.SetWall:
                WriteWallCommand(move);
                break;
        }
    }

    private static void WriteMoveCommand(Move move)
    {
        var builder = new StringBuilder();
        builder.Append(GetMoveType(move)).Append(' ');

        var code = ConvertPosition(move.Position.Row, move.Position.Column, 'a');
        builder.Append(code);
        Console.WriteLine(builder.ToString());
    }

    private static string GetMoveType(Move move)
    {
        var (row, column) = move.Position;
        var (playerRow, playerCol) = new Position(move.PlayerRow, move.PlayerColumn);
        if (row == playerRow && column != playerCol)
        {
            return Math.Abs(column - playerCol) > 1 ? "jump" : "move";
        }

        if (column == playerCol && row != playerRow)
        {
            return Math.Abs(row - playerRow) > 1 ? "jump" : "move";
        }

        if (row != playerRow && column != playerCol)
        {
            return "jump";
        }

        return "move";
    }

    private static void WriteWallCommand(Move move)
    {
        var builder = new StringBuilder();
        builder.Append("wall ");
        var code = ConvertPosition(move.Wall.WallCenter.NorthRow, move.Wall.WallCenter.WestColumn, 's');
        builder.Append(code);
        switch (move.Wall.Type)
        {
            case WallType.Horizontal:
                builder.Append('h');
                break;
            case WallType.Vertical:
                builder.Append('v');
                break;
        }

        Console.WriteLine(builder.ToString());
    }

    private static Move ParseInput(string input, PlayerColor color, IGameField gameField)
    {
        var words = input.Split(' ');
        var player = gameField.Players[(int)color - 1];
        switch (words[0])
        {
            case "move" or "jump":
                var desiredPosition = ParseMoveDirection(words[1], player);
                return new Move(player, desiredPosition);
            case "wall":
                var wall = ParseWall(words[1]);
                return new Move(player, wall);
            default:
                throw new ArgumentException("Invalid input", nameof(input));
        }
    }

    private static Position ParseMoveDirection(string position, Player player)
    {
        var (row, column) = ParsePosition(position, 'a');

        return new Position(row, column);
    }

    private static (int, int) ParsePosition(string pos, char startLetter)
    {
        pos = pos.ToLower();
        
        var letter = pos[0];
        var number = pos[1];

        var column = letter - startLetter;
        var row = number - '1';

        return (row, column);
    }

    private static string ConvertPosition(int row, int col, char startLetter)
    {
        var rowChar = (char)(row + '1');
        var colChar = (char)(col + startLetter);

        return $"{colChar}{rowChar}";
    }

    private static Wall ParseWall(string position)
    {
        position = position.ToLower();
        var orientationChar = position[^1];
        var orientation = orientationChar switch
        {
            'h' => WallType.Horizontal,
            'v' => WallType.Vertical
        };

        var convertedPosition = ParsePosition(position, 's');

        return new Wall(orientation, new WallCenter(convertedPosition.Item1, convertedPosition.Item2));
    }
}