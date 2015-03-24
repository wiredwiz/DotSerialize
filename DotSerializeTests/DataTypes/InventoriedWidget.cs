#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Drawing;
using System.Runtime.Serialization;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [XmlRoot("InventoriedWidget", "acme.org")]
   [DataContract(Name = "Widget", Namespace = "acme.org")]
   public class InventoriedWidget : Widget
   {
      [DataMember(Order = 1)] [XmlAttribute] public override string Name { get; set; }
      [DataMember(Order = 4)] [XmlAttribute] public override double Height { get; set; }
      [DataMember(Order = 5)] [XmlAttribute] public override double Width { get; set; }
      [DataMember(Order = 6)] [XmlAttribute] public override double Length { get; set; }
      [DataMember(Order = 7)] [XmlAttribute] public override double Weight { get; set; }
      [DataMember(Order = 3)] [XmlAttribute] public override Color Color { get; set; }
      [DataMember(Order = 2)] [XmlAttribute] public override Branding Brand { get; set; }
      [XmlIgnore] public override string Miscellaneous { get; set; }
      [XmlIgnore] public override bool BeingConstructed { get; set; }
      [DataMember(Order = 8)] public virtual int QuantityInInventory { get; set; }
   }
}