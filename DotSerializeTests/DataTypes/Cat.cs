﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class Cat : Pet, IEquatable<Cat>
   {
      public bool Selfish { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="Cat"/> class.
      /// </summary>
      public Cat(Owner owner)
         : base(owner)
      {
         Selfish = false;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Cat"/> class.
      /// </summary>
      public Cat()
         : base(null)
      {
         Selfish = false;
      }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(Cat other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return base.Equals(other) && Selfish.Equals(other.Selfish);
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
         return Equals((Cat)obj);
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current <see cref="T:System.Object"/>.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked { return (base.GetHashCode() * 397) ^ Selfish.GetHashCode(); }
      }
   }
}
