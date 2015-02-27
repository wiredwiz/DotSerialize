using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [DataContract(Name="Widget", Namespace="acme.org")]
   public class Widget
   {
      public enum Branding
      {
         Acme,
         Other
      }
      [DataMember(Order = 1)]
      public virtual string Name { get; set; }
      [DataMember(Order = 4)]
      public virtual double Height { get; set; }
      [DataMember(Order = 5)]
      public virtual double Width { get; set; }
      [DataMember(Order = 6)]
      public virtual double Length { get; set; }
      [DataMember(Order = 7)]
      public virtual double Weight { get; set; }
      [DataMember(Order = 3)]
      public virtual Color Color { get; set; }
      [DataMember(Order=2)]
      public virtual Branding Brand { get; set; }
      public virtual string Miscellaneous { get; set; }
      public virtual bool BeingConstructed { get; set; }
   }
}
