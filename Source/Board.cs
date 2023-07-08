using Stockfish.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Source
{
    public class Board
    {
        public List<List<Tile>> Tiles { get; set; }
        public IStockfish ChessEngine { get; set; }
        public string FenString { get; set; }

        public Board()
        {
            FenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            Tiles = UpdateBoard(FenString);
            ChessEngine = new Stockfish.NET.Stockfish(@"C:\Users\user\Desktop\stockfish_13_win_x64_avx2.exe");
            ChessEngine.SetFenPosition(FenString);
        }

        private List<List<Tile>> UpdateBoard(string FenString)
        {
            List<List<Tile>> Tiles = new List<List<Tile>>();
            string[] rows = FenString.Split(' ')[0].Split('/');
            int rowLength = rows.Length;
            int colLength = rows[0].Length;
            bool isWhiteTile = true;

            for (int x = 0; x < rowLength; x++)
            {
                List<Tile> tiles = new List<Tile>();

                for (int y = 0; y < colLength; y++)
                {
                    char c = rows[x][y];

                    if (char.IsDigit(c))
                    {
                        int emptySpaces = int.Parse(c.ToString());

                        for (int i = 0; i < emptySpaces; i++)
                        {
                            tiles.Add(new Tile(new Point(y++, x), null, GetTileColor(isWhiteTile)));
                            isWhiteTile = !isWhiteTile;
                        }

                        y += emptySpaces - 1;
                    }

                    else
                    {
                        tiles.Add(new Tile(new Point(y, x), c, GetTileColor(isWhiteTile)));
                        isWhiteTile = !isWhiteTile;
                    }
                }

                Tiles.Add(tiles);
                isWhiteTile = !isWhiteTile;
            }

            return this.Tiles;
        }

        private Color GetTileColor(bool IsWhiteTile)
        {
            return IsWhiteTile ? Color.White : Color.Gray;
        }

        public void Draw(Graphics Graphics)
        {
            const int delta = 25;
            const int size = 65;
            int x = delta;
            int y = delta;

            foreach (var Tile in Tiles)
            {
                foreach (var Field in Tile)
                {
                    Brush Brush = new SolidBrush(Field.TileColor);
                    Point Position = new Point(x, y);
                    Size Size = new Size(size, size);
                    Rectangle Rectangle = new Rectangle(Position, Size);
                    Graphics.FillRectangle(Brush, Rectangle);
                    if (Field.Piece != null)
                    {
                        Graphics.DrawImage(Field.Piece.Image, new Point(x, y));
                    }
                    x += size;
                }

                x = delta;
                y += size;
            }
        }

        private HashSet<string>? GeneratePieceMoves(string Position)
        {
            HashSet<string> Moves = new HashSet<string>();

            for (char File = 'a'; File <= 'h'; File++)
            {
                for (int Rank = 1; Rank <= 8; Rank++)
                {
                    string Move = $"{Position}{File}{Rank}";
                    if (ChessEngine.IsMoveCorrect(Move))
                    {
                        Moves.Add(Move);
                    }
                }
            }

            return Moves;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var Ranks in Tiles)
            {
                foreach(var File in Ranks)
                {
                    sb.Append(File);
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
