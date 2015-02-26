using System;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class DogCollar : IEquatable<DogCollar>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Collar"/> class.
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

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(DogCollar other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Studded.Equals(other.Studded) && Length == other.Length;
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
         return Equals((DogCollar)obj);
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current <see cref="T:System.Object"/>.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked { return (Studded.GetHashCode() * 397) ^ Length; }
      }
   }
}
