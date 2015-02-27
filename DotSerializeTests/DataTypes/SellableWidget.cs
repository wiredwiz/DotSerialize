using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [XmlRoot("SaleWidget", "acme.net")]
   public class SellableWidget : Widget
   {
      public double Cost { get; set; }
   }
}
