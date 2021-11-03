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
        builder.Append((int)move.MoveDirection is >= 1 and <= 4 ? "move " : "jump ");

        var code = ConvertPosition(move.Row, move.Column, 'a');
        builder.Append(code);
        Console.WriteLine(builder.ToString());
    }

    private static void WriteWallCommand(Move move)
    {
        var builder = new StringBuilder();
        builder.Append("wall ");
        var code = ConvertPosition(move.Row, move.Column, 's');
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
                var direction = ParseMoveDirection(words[1], player);
                return new Move(player, direction);
            case "wall":
                var wall = ParseWall(words[1]);
                return new Move(player, wall);
            default:
                throw new ArgumentException("Invalid input", nameof(input));
        }
    }

    private static MoveDirection ParseMoveDirection(string position, Player player)
    {
        var convertedPosition = ParsePosition(position, 'a');

        return convertedPosition switch
        {
            (int, int) t when t.Item1 == player.Row && t.Item2 > player.Column => MoveDirection.Right,
            (int, int) t when t.Item1 == player.Row && t.Item2 < player.Column => MoveDirection.Left,
            (int, int) t when t.Item1 > player.Row && t.Item2 == player.Column => MoveDirection.Down,
            (int, int) t when t.Item1 < player.Row && t.Item2 == player.Column => MoveDirection.Up,
            (int, int) t when t.Item1 < player.Row && t.Item2 < player.Column => MoveDirection.UpLeft,
            (int, int) t when t.Item1 < player.Row && t.Item2 > player.Column => MoveDirection.UpRight,
            (int, int) t when t.Item1 > player.Row && t.Item2 < player.Column => MoveDirection.DownLeft,
            (int, int) t when t.Item1 > player.Row && t.Item2 > player.Column => MoveDirection.DownRight,
        };

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