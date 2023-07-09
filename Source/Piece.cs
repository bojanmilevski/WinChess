using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Source
{
    [Serializable]
    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public enum PieceColor
    {
        White,
        Black,
    }

    public class Piece
    {
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }
        public Image Image { get; set; }

        public Piece(char Piece)
        {
            Type = SetType(Piece);
            Color = SetColor(Piece);
            Image = SetImage(Type, Color);
        }

        private PieceType SetType(char Piece)
        {
            switch(char.ToLower(Piece))
            {
                case 'p': return PieceType.Pawn;
                case 'r': return PieceType.Rook;
                case 'n': return PieceType.Knight;
                case 'b': return PieceType.Bishop;
                case 'q': return PieceType.Queen;
                case 'k': return PieceType.King;
                default: return PieceType.Pawn;
            }
        }

        private PieceColor SetColor(char Piece)
        {
            if (char.IsUpper(Piece))
            {
                return PieceColor.White;
            }

            return PieceColor.Black;
        }

        private Image SetImage(PieceType Type, PieceColor Color)
        {
            switch (Type)
            {
                case PieceType.Pawn:
                    return Color == PieceColor.White ? Properties.Resources.w_pawn : Properties.Resources.b_pawn;
                case PieceType.Rook:
                    return Color == PieceColor.White ? Properties.Resources.w_rook : Properties.Resources.b_rook;
                case PieceType.Knight:
                    return Color == PieceColor.White ? Properties.Resources.w_knight : Properties.Resources.b_knight;
                case PieceType.Bishop:
                    return Color == PieceColor.White ? Properties.Resources.w_bishop : Properties.Resources.b_bishop;
                case PieceType.Queen:
                    return Color == PieceColor.White ? Properties.Resources.w_queen : Properties.Resources.b_queen;
                case PieceType.King:
                    return Color == PieceColor.White ? Properties.Resources.w_king : Properties.Resources.b_king;
                default:
                    return null;
            }
        }
    }
}
