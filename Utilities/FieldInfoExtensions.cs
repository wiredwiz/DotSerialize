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
   public static class FieldInfoExtensions
   {
      public static PropertyInfo GetEncapsulatingAutoProperty(this FieldInfo info)
      {
         var propertyName = NamingUtils.GetAutoPropertyName(info.Name);
         if (!string.IsNullOrEmpty(propertyName))
            return info.DeclaringType.Property(propertyName);

         return null;
      }

      public static bool IsBackingField(this FieldInfo info)
      {
         var propertyName = NamingUtils.GetAutoPropertyName(info.Name);
         if (string.IsNullOrEmpty(propertyName))
            return false;
         return info.HasAttribute<CompilerGeneratedAttribute>();
      }
   }
}