using Stockfish.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Source
{
    public class Game
    {
        public Board Board { get; set; }
        public bool IsPlayerTurn { get; set; }
        public string FenString { get; set; }

        public Game()
        {
            Board = new Board();
            IsPlayerTurn = FenString.Split(' ')[1] == "w";
        }

        public void Draw(Graphics Graphics)
        {
            Board.Draw(Graphics);
        }
    }
}
