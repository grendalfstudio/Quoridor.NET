using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Bot
{
    public class MinimaxNode
    {
        private readonly List<MinimaxNode> _children;

        public MinimaxNode(MinimaxNode parent, string moveRepresentation)
        {
            Parent = parent;
            MoveRepresentation = moveRepresentation;
            _children = new List<MinimaxNode>();
        }

        public MinimaxNode Parent { get; }
        public IReadOnlyList<MinimaxNode> Children => _children;
        public string MoveRepresentation { get; }

        public void AddChildrenNodes(List<string> moves)
        {
            var childNodes = moves.Select(move => new MinimaxNode(this, move));
            _children.AddRange(childNodes);
        }

        public int EvaluateScore()
        {
            return _children.Count == 0
                ? 1 
                : _children.Sum(child => child.EvaluateScore());
        }
    }
}
