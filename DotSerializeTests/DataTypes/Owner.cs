using System;
using System.Linq;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [XmlRoot("PetOwner")]
   public class Owner : Person, IEquatable<Owner>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Owner"/> class.
      /// </summary>
      public Owner()
      {
         
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="Owner"/> class.
      /// </summary>
      /// <param name="firstName"></param>
      /// <param name="middleInitial"></param>
      /// <param name="lastName"></param>
      public Owner(string firstName, string middleInitial, string lastName)
         : base(firstName, middleInitial, lastName)
      {
         
      }
      public Pet[] Pets { get; set; }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Owner other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return base.Equals(other) && Pets.SequenceEqual(other.Pets);
      }

      /// <summary>
      /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
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
         return Equals((Owner)obj);
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current <see cref="T:System.Object"/>.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked { return (base.GetHashCode() * 397) ^ (Pets != null ? Pets.GetHashCode() : 0); }
      }
   }
}
