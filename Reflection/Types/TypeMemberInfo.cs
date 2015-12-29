#region Apapche License 2.0

// <copyright file="TypeMemberInfo.cs" company="Edgerunner.org">
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
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Reflection.Types
{
   public class TypeMemberInfo : IEquatable<TypeMemberInfo>
   {
      #region MemberType enum

      public enum MemberType
      {
         Field, 
         Property
      }

      #endregion

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeMemberInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="dataType"></param>
      public TypeMemberInfo(string name, MemberType type, Type dataType)
      {
         Name = name;
         Type = type;
         DataType = dataType;
         EntityName = name;
         IsAttribute = false;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeMemberInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="entityName"></param>
      /// <param name="dataType"></param>
      public TypeMemberInfo(string name, MemberType type, string entityName, Type dataType)
      {
         Name = name;
         Type = type;
         EntityName = entityName;
         DataType = dataType;
         IsAttribute = false;
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeMemberInfo" /> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="entityName"></param>
      /// <param name="dataType"></param>
      /// <param name="isAttribute"></param>
      public TypeMemberInfo(string name, MemberType type, string entityName, Type dataType, bool isAttribute)
      {
         Name = name;
         Type = type;
         EntityName = entityName;
         DataType = dataType;
         IsAttribute = isAttribute;
      }

      public string Name { get; set; }
      public MemberType Type { get; set; }
      public string EntityName { get; set; }
      public Type DataType { get; set; }
      public bool IsAttribute { get; set; }
      public int Order { get; set; }

      public string ConstructorFriendlyName
      {
         get
         {
            string friendlyName = NamingUtils.GetAutoPropertyName(Name);
            return string.IsNullOrEmpty(friendlyName) ? Name : friendlyName;
         }
      }

      #region IEquatable<TypeMemberInfo> Members

      /// <summary>
      ///    Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      ///    true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(TypeMemberInfo other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return string.Equals(Name, other.Name) && Type == other.Type && string.Equals(EntityName, other.EntityName) &&
                Equals(DataType, other.DataType) && IsAttribute.Equals(other.IsAttribute);
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
         return Equals((TypeMemberInfo)obj);
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
            var hashCode = Name != null ? Name.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ (int)Type;
            hashCode = (hashCode * 397) ^ (EntityName != null ? EntityName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsAttribute.GetHashCode();
            return hashCode;
         }
      }
   }
}