#region Apapche License 2.0

// <copyright file="MemberReference.cs" company="Edgerunner.org">
// Copyright 2015 Thaddeus Ryker
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

using System;
using System.Reflection;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Serialization.Reference
{
   public sealed class MemberReference : IEquatable<MemberReference>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="MemberReference" /> class.
      /// </summary>
      /// <param name="source"></param>
      /// <param name="type"></param>
      /// <param name="name"></param>
      public MemberReference(object source, MemberTypes type, string name)
      {
         Source = source;
         Type = type;
         Name = name;
         IsIndexReference = false;
         ArrayIndices = null;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="MemberReference" /> class.
      /// </summary>
      /// <param name="source"></param>
      /// <param name="type"></param>
      /// <param name="name"></param>
      /// <param name="arrayIndices"></param>
      public MemberReference(object source, MemberTypes type, string name, int[] arrayIndices)
      {
         Source = source;
         Type = type;
         Name = name;
         IsIndexReference = true;
         ArrayIndices = arrayIndices;
      }

      public object Source { get; set; }
      public MemberTypes Type { get; set; }
      public string Name { get; set; }
      public bool IsIndexReference { get; set; }
      public int[] ArrayIndices { get; set; }

      #region IEquatable<MemberReference> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(MemberReference other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Equals(Source, other.Source) && string.Equals(Name, other.Name) && Type == other.Type;
      }

      #endregion

      /// <summary>
      ///    Determines whether the specified object is equal to the current object.
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
         return Equals((MemberReference)obj);
      }

      /// <summary>
      ///    Serves as a hash function for a particular type.
      /// </summary>
      /// <returns>
      ///    A hash code for the current object.
      /// </returns>
      public override int GetHashCode()
      {
         unchecked
         {
            var hashCode = Source != null ? Source.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)Type;
            return hashCode;
         }
      }

      public void UpdateValue(object newValue)
      {
         if (Type == MemberTypes.Property)
            if (IsIndexReference)
               Source.SetArrayPropertyValue(Name, newValue, ArrayIndices);
            else
               Source.SetPropertyValue(Name, newValue);
         else if (Type == MemberTypes.Field)
            if (IsIndexReference)
               Source.SetArrayFieldValue(Name, newValue, ArrayIndices);
            else
               Source.SetFieldValue(Name, newValue);
         else
            throw new ReferenceException("Reference manager cannot update types other than Property or Field");
      }
   }
}