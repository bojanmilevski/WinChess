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
            Game.Draw(e.Graphics);
            Invalidate();
        }

        private void WinChess_MouseClick(object sender, MouseEventArgs e)
        {
            Game.Click(e.Location, label1);
            Invalidate();
        }
    }
}