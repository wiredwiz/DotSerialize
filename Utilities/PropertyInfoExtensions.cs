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
using System.Reflection;
using System.Runtime.CompilerServices;
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class PropertyInfoExtensions
   {
      public static FieldInfo GetBackingField(this PropertyInfo info)
      {
         var field = info.DeclaringType.Field(string.Format("<{0}>k__BackingField", info.Name));
         if (field == null)
            return null;
         return field.HasAttribute<CompilerGeneratedAttribute>() ? field : null;
      }

      public static bool IsAutoProperty(this PropertyInfo info)
      {
         var field = info.DeclaringType.Field(string.Format("<{0}>k__BackingField", info.Name));
         if (field == null)
            return false;
         return field.HasAttribute<CompilerGeneratedAttribute>();
      }
   }
}