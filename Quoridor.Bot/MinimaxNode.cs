using HavocAndCry.Quoridor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HavocAndCry.Quoridor.Core.Models;

namespace Quoridor.Bot
{
    public class MinimaxNode
    {
        private readonly List<MinimaxNode> _children;

        public MinimaxNode(MinimaxNode parent, Move move)
        {
            Parent = parent;
            Move = move;
            _children = new List<MinimaxNode>();
        }

        public MinimaxNode Parent { get; }
        public IReadOnlyList<MinimaxNode> Children => _children;
        public Move Move { get; }
        public int Score { get; set; }

        public void AddChildrenNodes(List<Move> moves)
        {
            var childNodes = moves.Select(move => new MinimaxNode(this, move));
            _children.AddRange(childNodes);
        }
    }
}
