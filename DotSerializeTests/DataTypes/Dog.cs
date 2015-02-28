using System;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class Dog : Pet, IEquatable<Dog>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Dog"/> class.
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

      public enum NoseStyle
      {
         Normal,
         Smushed,
         Pointy
      }

      public virtual bool HasCollar { get; set; }
      public NoseStyle Nose { get; set; }
      public Bone Bone { get; set; }
      [XmlElement(Order = 4)]
      public DogCollar Collar { get; set; }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Dog other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return base.Equals(other) && HasCollar.Equals(other.HasCollar) && Nose == other.Nose && Bone.Equals(other.Bone) && Equals(Collar, other.Collar);
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
         return Equals((Dog)obj);
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current <see cref="T:System.Object"/>.
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
