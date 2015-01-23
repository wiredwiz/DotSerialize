using System;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public static class DogCollarFactory
   {
      public static DogCollar GetCollar(int length, bool studded)
      {
         return new DogCollar(length, studded);
      }
   }
}
