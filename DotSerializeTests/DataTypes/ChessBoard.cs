using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class ChessBoard
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ChessBoard"/> class.
      /// </summary>
      public ChessBoard()
      {
         _Position = new ChessPiece[8, 8];
         _Position[0, 0] = new ChessPiece(ChessPiece.PieceType.Rook);
         _Position[0, 1] = new ChessPiece(ChessPiece.PieceType.Knight);
         _Position[0, 2] = new ChessPiece(ChessPiece.PieceType.Bishop);
         _Position[0, 3] = new ChessPiece(ChessPiece.PieceType.Queen);
         _Position[0, 4] = new ChessPiece(ChessPiece.PieceType.King);
         _Position[0, 5] = new ChessPiece(ChessPiece.PieceType.Bishop);
         _Position[0, 6] = new ChessPiece(ChessPiece.PieceType.Knight);
         _Position[0, 7] = new ChessPiece(ChessPiece.PieceType.Rook);

         _Position[1, 0] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 1] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 2] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 3] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 4] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 5] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 6] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[1, 7] = new ChessPiece(ChessPiece.PieceType.Pawn);

         _Position[6, 0] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 1] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 2] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 3] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 4] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 5] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 6] = new ChessPiece(ChessPiece.PieceType.Pawn);
         _Position[6, 7] = new ChessPiece(ChessPiece.PieceType.Pawn);

         _Position[7, 0] = new ChessPiece(ChessPiece.PieceType.Rook);
         _Position[7, 1] = new ChessPiece(ChessPiece.PieceType.Knight);
         _Position[7, 2] = new ChessPiece(ChessPiece.PieceType.Bishop);
         _Position[7, 3] = new ChessPiece(ChessPiece.PieceType.Queen);
         _Position[7, 4] = new ChessPiece(ChessPiece.PieceType.King);
         _Position[7, 5] = new ChessPiece(ChessPiece.PieceType.Bishop);
         _Position[7, 6] = new ChessPiece(ChessPiece.PieceType.Knight);
         _Position[7, 7] = new ChessPiece(ChessPiece.PieceType.Rook);
      }
      // Fields...
      private ChessPiece[,] _Position;

      public ChessPiece[,] Position
      {
         get { return _Position; }
         set
         {
            _Position = value;
         }
      }

      public void Move(Point start, Point end)
      {
         if ((start.X < 0) || (start.X > 7))
            throw new Exception("Invalid coordinates");
         if ((end.X < 0) || (end.X > 7))
            throw new Exception("Invalid coordinates");
         if (_Position[start.X, start.Y] == null)
            throw new Exception(string.Format("No piece exists at position {0},{1}", start.X, start.Y));

         _Position[end.X, end.Y] = _Position[start.X, start.Y];
         _Position[start.X, start.Y] = null;
      }
       
   }
}
