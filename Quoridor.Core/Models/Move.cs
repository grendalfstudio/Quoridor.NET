using HavocAndCry.Quoridor.Core.Abstract;
using HavocAndCry.Quoridor.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavocAndCry.Quoridor.Model
{
    public class Move
    {
        public Move(Player player, MoveDirection moveDirection)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.Move;
            MoveDirection = moveDirection;
            PlayerRow = player.Row;
            PlayerColumn = player.Column;
        }

        public Move(Player player, Wall wall)
        {
            PlayerId = player.PlayerId;
            TurnType = TurnType.SetWall;
            Wall = wall;
        }

        public int PlayerId { get; }
        public TurnType TurnType { get; }
        public MoveDirection MoveDirection { get; }
        public Wall Wall { get; }
        public int PlayerRow { get; }
        public int PlayerColumn { get; }
    }
}
