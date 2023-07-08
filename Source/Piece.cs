using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinChess.Source
{
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

        private Image SetImage(PieceType Type, PieceColor Color)
        {
            switch (Type)
            {
                case PieceType.Pawn:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_pawn;
                    }
                    return Properties.Resources.b_pawn;
                case PieceType.Rook:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_rook;
                    }
                    return Properties.Resources.b_rook;
                case PieceType.Knight:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_knight;
                    }
                    return Properties.Resources.b_knight;
                case PieceType.Bishop:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_bishop;
                    }
                    return Properties.Resources.b_bishop;
                case PieceType.Queen:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_queen;
                    }
                    return Properties.Resources.b_queen;
                case PieceType.King:
                    if (Color == PieceColor.White)
                    {
                        return Properties.Resources.w_king;
                    }
                    return Properties.Resources.b_king;
                default:
                    return Properties.Resources.w_pawn;
            }
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

        public override string ToString()
        {
            char c = ' ';

            switch(Type)
            {
                case PieceType.Pawn: c = 'p'; break;
                case PieceType.Rook: c = 'r'; break;
                case PieceType.Knight: c = 'n'; break;
                case PieceType.Bishop: c = 'b'; break;
                case PieceType.Queen: c = 'q'; break;
                case PieceType.King: c = 'k'; break;
                default: c = ' '; break;
            }

            if (Color == PieceColor.White)
            {
                return char.ToUpper(c).ToString();
            }

            return char.ToLower(c).ToString();
        }
    }
}
