using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [XmlRoot("InventoriedWidget", "acme.org")]
   [DataContract(Name="Widget", Namespace="acme.org")]
   public class InventoriedWidget : Widget
   {
      [DataMember(Order = 1)]
      [XmlAttribute]
      public override string Name { get; set; }
      [DataMember(Order = 4)]
      [XmlAttribute]
      public override double Height { get; set; }
      [DataMember(Order = 5)]
      [XmlAttribute]
      public override double Width { get; set; }
      [DataMember(Order = 6)]
      [XmlAttribute]
      public override double Length { get; set; }
      [DataMember(Order = 7)]
      [XmlAttribute]
      public override double Weight { get; set; }
      [DataMember(Order = 3)]
      [XmlAttribute]
      public override Color Color { get; set; }
      [DataMember(Order = 2)]
      [XmlAttribute]
      public override Branding Brand { get; set; }
      [XmlIgnore]
      public override string Miscellaneous { get; set; }
      [XmlIgnore]
      public override bool BeingConstructed { get; set; }
      [DataMember(Order=8)]
      [XmlElement]
      public virtual int QuantityInInventory { get; set; }
   }
}
