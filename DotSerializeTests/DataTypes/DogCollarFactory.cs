using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSerializeTests.DataTypes
{
   public static class DogCollarFactory
   {
      public static DogCollar GetCollar(int length, bool studded)
      {
         return new DogCollar(length, studded);
      }
   }
}
