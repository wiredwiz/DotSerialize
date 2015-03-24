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

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [DataContract(Name = "Widget", Namespace = "acme.org")]
   public class Widget
   {
      #region Branding enum

      public enum Branding
      {
         Acme,
         Other
      }

      #endregion

      [DataMember(Order = 1)] public virtual string Name { get; set; }
      [DataMember(Order = 4)] public virtual double Height { get; set; }
      [DataMember(Order = 5)] public virtual double Width { get; set; }
      [DataMember(Order = 6)] public virtual double Length { get; set; }
      [DataMember(Order = 7)] public virtual double Weight { get; set; }
      [DataMember(Order = 3)] public virtual Color Color { get; set; }
      [DataMember(Order = 2)] public virtual Branding Brand { get; set; }
      public virtual string Miscellaneous { get; set; }
      public virtual bool BeingConstructed { get; set; }
   }
}