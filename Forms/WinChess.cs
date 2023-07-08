using WinChess.Source;

namespace winchess
{
    public partial class WinChess : Form
    {
        public Game Game { get; set; }

        public WinChess()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Game = new Game();
        }

        private void WinChess_Paint(object sender, PaintEventArgs e)
        {
            // label.Text = Game.Board.ToString();
            Game.Draw(e.Graphics);
            Invalidate();
        }
    }
}