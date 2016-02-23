#region Apapche License 2.0
// <copyright file="SimplePerson.cs" company="Edgerunner.org">
// Copyright 2016 
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
// </copyright>
#endregion
namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   using System;

   using Org.Edgerunner.DotSerialize.Attributes;

   public class SimplePerson : IEquatable<SimplePerson>
   {
      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(SimplePerson other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return string.Equals(this.Name, other.Name);
      }

      /// <summary>
      /// Determines whether the specified object is equal to the current object.
      /// </summary>
      /// <returns>
      /// true if the specified object  is equal to the current object; otherwise, false.
      /// </returns>
      /// <param name="obj">The object to compare with the current object. </param>
      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != this.GetType()) return false;
         return Equals((SimplePerson)obj);
      }

      /// <summary>
      /// Serves as the default hash function. 
      /// </summary>
      /// <returns>
      /// A hash code for the current object.
      /// </returns>
      public override int GetHashCode()
      {
         return (this.Name != null ? this.Name.GetHashCode() : 0);
      }

      public static bool operator ==(SimplePerson left, SimplePerson right)
      {
         return Equals(left, right);
      }

      public static bool operator !=(SimplePerson left, SimplePerson right)
      {
         return !Equals(left, right);
      }

      [XmlAttribute]
      public string Name { get; set; }
   }
}