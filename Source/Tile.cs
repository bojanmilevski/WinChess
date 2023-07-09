using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Source
{
    public class Tile
    {
        public Color TileColor { get; set; }
        public Piece? Piece { get; set; }
        public Point Position { get; set; }
        public string AlgebraicPosition { get; set; }

        public Tile(Point Position, char? Piece, Color TileColor)
        {
            this.Position = Position;
            this.TileColor = TileColor;
            AlgebraicPosition = GetAlgebraicPosition(Position);

            if (Piece.HasValue)
            {
                this.Piece = new Piece(Piece.Value);
            }

            else
            {
                this.Piece = null;
            }
        }

        private string GetAlgebraicPosition(Point Position)
        {
            int Rank = 8 - Position.Y;
            char File = (char)('a' + Position.X);
            return $"{File}{Rank}";
        }

        public override string ToString()
        {
            if (Piece == null)
            {
                return " ";
            }

            return $"{Piece}";
        }
    }
}
