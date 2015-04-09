using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class ChessPiece
   {
      private readonly PieceType _Type;

      /// <summary>
      /// Initializes a new instance of the <see cref="ChessPiece"/> class.
      /// </summary>
      /// <param name="type"></param>
      public ChessPiece(PieceType type)
      {
         _Type = type;
      }

      public enum PieceType
      {
         Pawn,
         Bishop,
         Rook,
         Knight,
         Queen,
         King
      }

      public PieceType Type
      {
         get { return _Type; }
      }
   }
}
