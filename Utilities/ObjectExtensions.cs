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
using Fasterflect;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class ObjectExtensions
   {
      public static void SetArrayFieldValue(this object obj, string name, object value, int[] indices)
      {
         var type = obj.GetType();
         var fieldInfo = type.Field(name);
         if (fieldInfo == null)
            throw new Exception(string.Format("Cannot set non-existant field \"{0}\"", name));
         if (!fieldInfo.Type().IsArray)
            throw new TargetException("Field \"{0}\" is not an array.");
         Array array = fieldInfo.GetValue(obj) as Array;
         if (array == null)
            throw new TargetException("The value of field \"{0}\" is null.");
         array.SetValue(value, indices);
      }

      public static void SetArrayPropertyValue(this object obj, string name, object value, int[] indices)
      {
         var type = obj.GetType();
         var propInfo = type.Property(name);
         if (propInfo == null)
            throw new Exception(string.Format("Cannot set non-existant property \"{0}\"", name));
         if (!propInfo.Type().IsArray)
            throw new TargetException("Property \"{0}\" is not an array.");
         Array array = propInfo.GetValue(obj) as Array;
         if (array == null)
            throw new TargetException("The value of property \"{0}\" is null.");
         array.SetValue(value, indices);
      }
   }
}