using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize
{
   public class Settings
   {
      public List<Attribute> AttributesToIgnore { get; set; }
      public CultureInfo Culture { get; set; }

      public static Settings Default()
      {
         return new Settings();
      }
   }
}
