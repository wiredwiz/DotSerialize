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
   public class Pet : IEquatable<Pet>
   {
      [XmlElement(Order = 10)] private readonly Owner _Owner;

      /// <summary>
      ///    Initializes a new instance of the <see cref="Pet" /> class.
      /// </summary>
      /// <param name="owner"></param>
      public Pet(Owner owner)
      {
         _Owner = owner;
         Name = String.Empty;
         Breed = String.Empty;
         BirthDate = DateTime.MinValue;
      }

      [XmlElement(Order = 1)] public virtual string Name { get; set; }
      [XmlElement(Order = 3)] public virtual string Breed { get; set; }
      [XmlElement(Order = 2)] public virtual DateTime BirthDate { get; set; }
      public double Weight { get; set; }
      public double Height { get; set; }

      [XmlIgnore] public virtual double Age
      {
         get { return (DateTime.Now - BirthDate).TotalDays / 365; }
      }

      public virtual Owner Owner
      {
         get { return _Owner; }
      }

      #region IEquatable<Pet> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Pet other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(_Owner, other._Owner) && string.Equals(Name, other.Name) && string.Equals(Breed, other.Breed) &&
                BirthDate.Equals(other.BirthDate);
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
         return Equals((Pet)obj);
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
            var hashCode = (_Owner != null ? _Owner.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Breed != null ? Breed.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
            return hashCode;
         }
      }
   }
}