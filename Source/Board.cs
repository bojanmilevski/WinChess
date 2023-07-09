using Stockfish.NET;
using System;
using System.Text;

namespace WinChess.Source
{
    public class Board
    {
        public IStockfish ChessEngine { get; set; }
        public HashSet<Tile> ValidMoves { get; set; }
        public List<List<Tile>> Tiles { get; set; }
        public List<string> Moves { get; set; }
        public Tile SelectedTile { get; set; }
        public string FenString { get; set; }
        public const int DELTA = 25;
        public const int FIELD_SIZE = 65;

        public Board()
        {
            FenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            Tiles = new List<List<Tile>>(64);
            Tiles = UpdateBoard(FenString);
            ChessEngine = new Stockfish.NET.Stockfish(@"C:\Users\user\Desktop\stockfish_13_win_x64_avx2.exe");
            ChessEngine.SetFenPosition(FenString);
            Moves = new List<string>();
            SelectedTile = null;
            ValidMoves = new HashSet<Tile>();
        }
        
        private Color GetTileColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.White : Color.DarkGray;
        }

        private List<List<Tile>> UpdateBoard(string FenString)
        {
            List<List<Tile>> tiles = new List<List<Tile>>(64);
            string[] ranks = FenString.Split(' ')[0].Split('/');

            for (int y = 0; y < ranks.Length; y++)
            {
                List<Tile> rankTiles = new List<Tile>();
                string rank = ranks[y];
                int x = 0;

                foreach (char c in rank)
                {
                    Point Position = new Point(x, y);

                    if (char.IsDigit(c))
                    {
                        int numEmptySquares = int.Parse(c.ToString());
                        for (int i = 0; i < numEmptySquares; i++)
                        {
                            rankTiles.Add(new Tile(Position, null, GetTileColor(x, y)));
                            x++;
                        }
                    }

                    else
                    {
                        rankTiles.Add(new Tile(Position, c, GetTileColor(x, y)));
                        x++;
                    }
                }

                tiles.Add(rankTiles);
            }

            return tiles;
        }

        public void Draw(Graphics Graphics)
        {
            int x = DELTA;
            int y = DELTA;
            Size Size = new Size(FIELD_SIZE, FIELD_SIZE);

            foreach (var T in Tiles)
            {
                foreach (var Tile in T)
                {
                    Brush Brush = new SolidBrush(Tile.TileColor);
                    Point Position = new Point(x, y);
                    Rectangle Rectangle = new Rectangle(Position, Size);
                    Graphics.FillRectangle(Brush, Rectangle);

                    if (Tile == SelectedTile)
                    {
                        Graphics.FillRectangle(new SolidBrush(Color.LightGreen), Rectangle);
                    }

                    if (ValidMoves.Contains(Tile))
                    {
                        Graphics.FillRectangle(new SolidBrush(Color.LightYellow), Rectangle);
                    }

                    if (Tile.Piece != null)
                    {
                        Graphics.DrawImage(Tile.Piece.Image, new Point(x, y));
                    }

                    Graphics.DrawRectangle(new Pen(Color.Black), Rectangle);

                    Brush.Dispose();
                    x += FIELD_SIZE;
                }

                x = DELTA;
                y += FIELD_SIZE;
            }
        }

        private Tile GetTile(string Move)
        {
            Point Point = new Point(Move[0] - 'a', 8 - int.Parse(Move[1].ToString()));
            return Tiles[Point.Y][Point.X];
        }

        private void GeneratePieceMoves(string Position)
        {
            for (char File = 'a'; File <= 'h'; File++)
            {
                for (int Rank = 1; Rank <= 8; Rank++)
                {
                    string NewPosition = $"{File}{Rank}";
                    string Move = $"{Position}{NewPosition}";
                    if (ChessEngine.IsMoveCorrect(Move))
                    {
                        ValidMoves.Add(GetTile(NewPosition));
                    }
                }
            }
        }

        public void Click(Point Point)
        {
            foreach (var T in Tiles)
            {
                foreach (var Tile in T)
                {
                    int fieldX = DELTA + (Tile.Position.X * FIELD_SIZE);
                    int fieldY = DELTA + (Tile.Position.Y * FIELD_SIZE);
                    Rectangle TileBounds = new Rectangle(fieldX, fieldY, FIELD_SIZE, FIELD_SIZE);

                    if (TileBounds.Contains(Point))
                    {
                        if (Tile == SelectedTile)
                        {
                            SelectedTile = null;
                            ValidMoves.Clear();
                        }

                        if (Tile != SelectedTile && ValidMoves.Contains(Tile))
                        {
                            Moves.Add($"{SelectedTile.AlgebraicPosition}{Tile.AlgebraicPosition}");
                            ChessEngine.SetPosition(Moves.ToArray());
                            string update = ChessEngine.GetFenPosition();
                            Tiles = UpdateBoard(update);
                            SelectedTile = null;
                            ValidMoves.Clear();
                        }

                        else
                        {
                            SelectedTile = Tile;
                            if (SelectedTile.Piece == null)
                            {
                                return;
                            }

                            ValidMoves.Clear();
                            GeneratePieceMoves(SelectedTile.AlgebraicPosition);
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var Ranks in Tiles)
            {
                foreach (var File in Ranks)
                {
                    sb.Append(File);
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
