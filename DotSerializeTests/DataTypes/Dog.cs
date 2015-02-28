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
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class Dog : Pet, IEquatable<Dog>
   {
      #region NoseStyle enum

      public enum NoseStyle
      {
         Normal,
         Smushed,
         Pointy
      }

      #endregion

      /// <summary>
      ///    Initializes a new instance of the <see cref="Dog" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="breed"></param>
      /// <param name="hasCollar"></param>
      /// <param name="owner"></param>
      public Dog(string name, string breed, bool hasCollar, Owner owner)
         : base(owner)
      {
         Name = name;
         Breed = breed;
         HasCollar = hasCollar;
      }

      public virtual bool HasCollar { get; set; }
      public NoseStyle Nose { get; set; }
      public Bone Bone { get; set; }
      [XmlElement(Order = 4)] public DogCollar Collar { get; set; }

      #region IEquatable<Dog> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Dog other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return base.Equals(other) && HasCollar.Equals(other.HasCollar) && Nose == other.Nose && Bone.Equals(other.Bone) &&
                Equals(Collar, other.Collar);
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
         return Equals((Dog)obj);
      }

      /// <summary>
      ///    Serves as a hash function for a particular type.
      /// </summary>
      /// <returns>
      ///    A hash code for the current <see cref="T:System.Object" />.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked
         {
            int hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ HasCollar.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Nose;
            hashCode = (hashCode * 397) ^ Bone.GetHashCode();
            hashCode = (hashCode * 397) ^ (Collar != null ? Collar.GetHashCode() : 0);
            return hashCode;
         }
      }
   }
}