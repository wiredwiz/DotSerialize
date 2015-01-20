using System;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public sealed class MemberReference : IEquatable<MemberReference>
   {
      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(MemberReference other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(Source, other.Source) && string.Equals(Name, other.Name) && Type == other.Type;
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
         return Equals((MemberReference)obj);
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current object.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = (Source != null ? Source.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)Type;
            return hashCode;
         }
      }

      public object Source { get; set; }
      public System.Reflection.MemberTypes Type { get; set; }
      public string Name { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="MemberReference"/> class.
      /// </summary>
      /// <param name="source"></param>
      /// <param name="type"></param>
      /// <param name="name"></param>
      public MemberReference(object source, System.Reflection.MemberTypes type, string name)
      {
         Source = source;
         Type = type;
         Name = name;
      }

      public void UpdateValue(object newValue)
      {
         if (Type == System.Reflection.MemberTypes.Property)
            Source.SetPropertyValue(Name, newValue);
         else if (Type == System.Reflection.MemberTypes.Field)
            Source.SetFieldValue(Name, newValue);
         else
            throw new ReferenceException("Reference manager cannot update types other than Property or Field");
      }
   }
}
