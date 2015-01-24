using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize
{
   public class Settings
   {
      public List<Attribute> AttributesToIgnore { get; set; }
      public CultureInfo Culture { get; set; }
      public bool OmitTypeWhenPossible { get; set; }
      public bool IncludeAssemblyVersionWithType { get; set; }
      public bool IncludeAssemblyCultureWithType { get; set; }
      public bool IncludeAssemblyKeyWithType { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="Settings"/> class.
      /// </summary>
      public Settings()
      {
         AttributesToIgnore = new List<Attribute>() { new XmlIgnoreAttribute() };
         Culture = CultureInfo.InvariantCulture;
      }


      private static Settings _Default;

      public static Settings Default
      {
         get { return _Default ?? (_Default = new Settings()); }
      }
      
   }
}
