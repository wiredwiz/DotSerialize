using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class NamingUtils
   {
      public static string GetAutoPropertyName(string backingFieldName)
      {
         var result = Regex.Match(backingFieldName, "<(.+)>k__BackingField", RegexOptions.Compiled);
         if (!result.Success)
            return null;
         return result.Groups[1].Value;
      }
   }
}
