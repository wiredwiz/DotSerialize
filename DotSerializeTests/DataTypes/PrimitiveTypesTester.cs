using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class PrimitiveTypesTester
   {
      public string StringDat { get; set; }
      public char CharDat { get; set; }
      public bool BooleanDat { get; set; }
      public Byte ByteDat { get; set; }
      public SByte SbyteDat { get; set; }
      public Int16 Int16Dat { get; set; }
      public Int32 Int32Dat { get; set; }
      public Int64 Int64Dat { get; set; }
      public UInt16 UInt16Dat { get; set; }
      public UInt32 UInt32Dat { get; set; }
      public UInt64 UInt64Dat { get; set; }
      public Decimal DecimalDat { get; set; }
      public Single SingleDat { get; set; }
      public Double DoubleDat { get; set; }
      public DateTime DateTimeDat { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="PrimitiveTypesTester"/> class.
      /// </summary>
      public PrimitiveTypesTester()
      {
      }
   }
}
