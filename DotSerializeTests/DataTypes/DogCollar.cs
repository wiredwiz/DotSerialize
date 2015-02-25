using System;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class DogCollar
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Collar"/> class.
      /// </summary>
      /// <param name="length"></param>
      /// <param name="studded"></param>
      internal DogCollar(int length, bool studded)
      {
         Length = length;
         Studded = studded;
      }
      public int Length { get; set; }
      public bool Studded { get; set; }
   }
}
