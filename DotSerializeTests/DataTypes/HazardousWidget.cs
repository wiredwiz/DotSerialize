using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class HazardousWidget : Widget
   {
      [XmlIgnore]
      public override Color Color { get; set; }
   }
}
