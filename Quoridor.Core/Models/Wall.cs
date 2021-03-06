namespace HavocAndCry.Quoridor.Core.Models
{
    public struct Wall
    {
        public Wall(WallType type, WallCenter wallCenter)
        {
            Type = type;
            WallCenter = wallCenter;
        }
        
        public WallType Type { get; }
        public WallCenter WallCenter { get; }

        public override string ToString()
        {
            return $"[Center = {WallCenter}; Type = {Type.ToString()}]";
        }
    }
}