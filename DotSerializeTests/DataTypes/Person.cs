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
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class Person : IEquatable<Person>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="Person" /> class.
      /// </summary>
      public Person()
      {
         FirstName = String.Empty;
         MiddleInitial = String.Empty;
         LastName = String.Empty;
         BirthDate = DateTime.MinValue;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="Person" /> class.
      /// </summary>
      /// <param name="firstName"></param>
      /// <param name="middleInitial"></param>
      /// <param name="lastName"></param>
      public Person(string firstName, string middleInitial, string lastName)
      {
         FirstName = firstName;
         MiddleInitial = middleInitial;
         LastName = lastName;
         BirthDate = DateTime.MinValue;
      }

      [XmlIgnore] public virtual string FirstName { get; set; }
      [XmlIgnore] public virtual string MiddleInitial { get; set; }
      [XmlIgnore] public virtual string LastName { get; set; }
      public DateTime BirthDate { get; set; }
      public Person Father { get; set; }
      public Person Mother { get; set; }
      public List<Person> Children { get; set; }

      [XmlElement] public virtual string FullName
      {
         get { return String.Format("{0} {1} {2}", FirstName, MiddleInitial, LastName); }
         set
         {
            var names = value.Split(' ');
            FirstName = names[0];
            MiddleInitial = names[1];
            LastName = names[2];
         }
      }

      #region IEquatable<Person> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Person other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return string.Equals(FirstName, other.FirstName) && string.Equals(MiddleInitial, other.MiddleInitial) &&
                string.Equals(LastName, other.LastName) && BirthDate.Equals(other.BirthDate) && Equals(Father, other.Father) &&
                Equals(Mother, other.Mother) && Equals(Children, other.Children);
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
         return Equals((Person)obj);
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
            var hashCode = (FirstName != null ? FirstName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (MiddleInitial != null ? MiddleInitial.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ BirthDate.GetHashCode();
            hashCode = (hashCode * 397) ^ (Father != null ? Father.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Mother != null ? Mother.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Children != null ? Children.GetHashCode() : 0);
            return hashCode;
         }
      }
   }
}