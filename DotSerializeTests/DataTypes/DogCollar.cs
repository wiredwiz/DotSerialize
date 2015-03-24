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

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class DogCollar : IEquatable<DogCollar>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="Collar" /> class.
      /// </summary>
      /// <param name="length"></param>
      /// <param name="studded"></param>
      internal DogCollar(int length, bool studded)
      {
         Length = length;
         Studded = studded;
      }

      public int Length { get; set; }
      public bool Studded { get; set; }

      #region IEquatable<DogCollar> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(DogCollar other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Studded.Equals(other.Studded) && Length == other.Length;
      }

      #endregion

      /// <summary>
      ///    Determines whether the specified <see cref="T:System.Object" /> is equal to the current
      ///    <see cref="T:System.Object" />.
      /// </summary>
      /// <returns>
      ///    true if the specified object  is equal to the current object; otherwise, false.
      /// </returns>
      /// <param name="obj">The object to compare with the current object. </param>
      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != GetType()) return false;
         return Equals((DogCollar)obj);
      }

      /// <summary>
      ///    Serves as a hash function for a particular type.
      /// </summary>
      /// <returns>
      ///    A hash code for the current <see cref="T:System.Object" />.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked { return (Studded.GetHashCode() * 397) ^ Length; }
      }
   }
}