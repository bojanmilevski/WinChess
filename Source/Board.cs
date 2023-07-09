using Stockfish.NET;
using System.Text;

namespace WinChess.Source
{
    public class Board
    {
        public IStockfish ChessEngine { get; set; }
        public HashSet<Tile> ValidMoves { get; set; }
        public List<List<Tile>> Tiles { get; set; }
        public List<string> Moves { get; set; }
        public Tile? SelectedTile { get; set; }
        public string FenString { get; set; }
        public const int DELTA = 25;
        public const int FIELD_SIZE = 65;
        public bool IsPlayerWhiteTurn { get; set; }

        public Board()
        {
            FenString = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            ChessEngine = new Stockfish.NET.Stockfish(@"C:\Users\user\Desktop\stockfish_13_win_x64_avx2.exe");
            ChessEngine.SetFenPosition(FenString);
            Tiles = UpdateBoard(FenString);
            Moves = new List<string>();
            ValidMoves = new HashSet<Tile>();
            SelectedTile = null;
            IsPlayerWhiteTurn = FenString.Split(' ')[1] == "w";
        }

        private Color GetTileColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.White : Color.DarkGray;
        }

        private List<List<Tile>> UpdateBoard(string FenString)
        {
            List<List<Tile>> tiles = new List<List<Tile>>(64);
            string[] ranks = FenString.Split(' ')[0].Split('/');

            for (int x = 0; x < ranks.Length; x++)
            {
                List<Tile> files = new List<Tile>();
                string rank = ranks[x];
                int y = 0;

                foreach (char c in rank)
                {
                    if (char.IsDigit(c))
                    {
                        int empty = int.Parse(c.ToString());
                        for (int i = 0; i < empty; i++)
                        {
                            files.Add(new Tile(new Point(x, y), null, GetTileColor(x, y)));
                            y++;
                        }
                    }

                    else
                    {
                        files.Add(new Tile(new Point(x, y), c, GetTileColor(x, y)));
                        y++;
                    }
                }

                tiles.Add(files);
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

        private void GeneratePieceMoves(Tile Tile)
        {
            for (char File = 'a'; File <= 'h'; File++)
            {
                for (int Rank = 1; Rank <= 8; Rank++)
                {
                    string Move = $"{Tile.AlgebraicPosition}{File}{Rank}";
                    if (ChessEngine.IsMoveCorrect(Move))
                    {
                        ValidMoves.Add(Tiles[8 - Rank][File - 'a']);
                    }
                }
            }
        }

        public void Click(Point Point, Label Label)
        {
            foreach (var T in Tiles)
            {
                foreach (var Tile in T)
                {
                    int x = DELTA + (Tile.Position.X * FIELD_SIZE);
                    int y = DELTA + (Tile.Position.Y * FIELD_SIZE);
                    Rectangle TileBounds = new Rectangle(y, x, FIELD_SIZE, FIELD_SIZE);

                    if (TileBounds.Contains(Point))
                    {
                        if (SelectedTile == Tile)
                        {
                            SelectedTile = Tile;
                            ValidMoves.Clear();
                        }

                        else if (SelectedTile == null)
                        {
                            SelectedTile = Tile;
                            GeneratePieceMoves(SelectedTile);
                        }

                        if (SelectedTile != null && Tile != SelectedTile && ValidMoves.Contains(Tile))
                        {
                            Moves.Add($"{SelectedTile.AlgebraicPosition}{Tile.AlgebraicPosition}");
                            ChessEngine.SetPosition(Moves.ToArray());
                            Tiles = UpdateBoard(ChessEngine.GetFenPosition());
                            IsPlayerWhiteTurn = FenString.Split(' ')[1] == "w";
                            SelectedTile = null;
                            ValidMoves.Clear();
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
