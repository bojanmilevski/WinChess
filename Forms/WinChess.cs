using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using WinChess.Source;

namespace winchess
{
    public partial class WinChess : Form
    {
        private bool IsGame { get; set; }
        private Board Board { get; set; }
        private int PlayerSeconds { get; set; }
        private int OpponentSeconds { get; set; }

        public WinChess()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Board = new Board();
            PlayerSeconds = 0;
            OpponentSeconds = 0;
            lIsGame.Text = "Game Status:\nNOT STARTED";
            playerTimer.Interval = 1000;
            opponentTimer.Interval = 1000;
            btnUndo.Enabled = false;
            btnRedo.Enabled = false;
        }

        private bool CheckCheckMate()
        {
            try
            {
                var message = Board.ChessEngine.GetEvaluation().Type;
            }

            catch (Exception ex)
            {
                IsGame = false;
                lIsGame.Text = "Game Status:\nOVER";
                return true;
            }

            return false;
        }

        private void UpdateMoveHistory()
        {
            if (Board.Moves.Count > 0 && !lbMoveHistory.Items.Contains(Board.Moves.ToArray().LastOrDefault()))
            {
                lbMoveHistory.Items.Add(Board.Moves.ToArray().LastOrDefault());
            }
        }

        private void WinChess_Paint(object sender, PaintEventArgs e)
        {
            Board.Draw(e.Graphics);
            Invalidate();
        }

        private void WinChess_MouseClick(object sender, MouseEventArgs e)
        {
            //IsGame = CheckCheckMate();

            if (!IsGame)
            {
                return;
            }

            if (Board.IsPlayerTurn)
            {
                opponentTimer.Stop();
                playerTimer.Start();
                btnUndo.Enabled = true;
                btnRedo.Enabled = true;
                Board.Click(e.Location);
            }

            UpdateMoveHistory();
            Invalidate();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            IsGame = true;
            lIsGame.Text = "Game Status:\nONGOING";
            btnStart.Enabled = false;
            playerTimer.Start();
        }

        private void playerTimer_Tick(object sender, EventArgs e)
        {
            if (Board.IsPlayerTurn)
            {
                lPlayerTurn.Text = $"Player Turn:\nPlayer";
            }

            PlayerSeconds++;
            tbPlayerTime.Text = $"{PlayerSeconds / 60}:{PlayerSeconds % 60}";
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Board.UndoMove();
            Invalidate();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            Board.RedoMove();
            Invalidate();
        }

        private void opponentTimer_Tick(object sender, EventArgs e)
        {
            if (Board.IsPlayerTurn)
            {
                lPlayerTurn.Text = $"Opponent Turn:\nOpponent";
            }

            OpponentSeconds++;
            tbOpponentTime.Text = $"{PlayerSeconds / 60}:{PlayerSeconds % 60}";
        }

        private void WinChess_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsGame)
            {
                return;
            }

            if (!Board.IsPlayerTurn)
            {
                playerTimer.Stop();
                opponentTimer.Start();
                btnUndo.Enabled = false;
                btnRedo.Enabled = false;
                Board.AIMove();
                UpdateMoveHistory();
                Invalidate();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Board.UndoMove();
            Invalidate();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Board.RedoMove();
            Invalidate();
        }
    }
}