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

namespace Org.Edgerunner.DotSerialize.Utilities
{
   public static class TypeExtensions
   {
      #region Static Methods

      public static object GetDefaultValue(this Type t)
      {
         if (t.IsValueType)
            return Activator.CreateInstance(t);

         return null;
      }

      public static bool IsImplementationOf<T>(this Type t)
      {
         var type = typeof(T);
         if (t == null) throw new ArgumentNullException("t");
         if (type == null) throw new NotSupportedException("Type of T must not be null");
         if (!type.IsInterface) throw new NotSupportedException("Type of T must an interface");

         return t.GetInterface(type.FullName) != null;
      }

      public static bool IsImplementationOf(this Type t, Type interfaceToTest)
      {         
         if (t == null) throw new ArgumentNullException("t");
         if (interfaceToTest == null) throw new ArgumentNullException("interfaceToTest");
         if (!interfaceToTest.IsInterface) throw new ArgumentNullException("Must an interface", "interfaceToTest");

         return t.GetInterface(interfaceToTest.FullName) != null;
      }

      #endregion
   }
}