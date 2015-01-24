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
using System.Collections.Generic;
using System.Globalization;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize
{
   public class Settings
   {
      private static Settings _Default;
      public List<Attribute> AttributesToIgnore { get; set; }
      public CultureInfo Culture { get; set; }
      public bool OmitTypeWhenPossible { get; set; }
      public bool IncludeAssemblyVersionWithType { get; set; }
      public bool IncludeAssemblyCultureWithType { get; set; }
      public bool IncludeAssemblyKeyWithType { get; set; }

      public static Settings Default
      {
         get { return _Default ?? (_Default = new Settings()); }
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="Settings" /> class.
      /// </summary>
      public Settings()
      {
         AttributesToIgnore = new List<Attribute> { new XmlIgnoreAttribute() };
         Culture = CultureInfo.InvariantCulture;
      }
   }
}