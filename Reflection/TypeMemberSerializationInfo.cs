using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeMemberSerializationInfo : IEquatable<TypeMemberSerializationInfo>
   {
      public enum MemberType
      {
         Field,
         Property
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="dataType"></param>
      public TypeMemberSerializationInfo(string name, MemberType type, Type dataType)
      {
         Name = name;
         Type = type;
         DataType = dataType;
         EntityName = name;
         IsAttribute = false;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="entityName"></param>
      /// <param name="dataType"></param>
      public TypeMemberSerializationInfo(string name, MemberType type, string entityName, Type dataType)
      {
         Name = name;
         Type = type;
         EntityName = entityName;
         DataType = dataType;
         IsAttribute = false;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="type"></param>
      /// <param name="entityName"></param>
      /// <param name="dataType"></param>
      /// <param name="isAttribute"></param>
      public TypeMemberSerializationInfo(string name, MemberType type, string entityName, Type dataType, bool isAttribute)
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
      public string ConstructorFriendlyName
      {
         get
         {
            string friendlyName = Utilities.NamingUtils.GetAutoPropertyName(Name);
            return string.IsNullOrEmpty(friendlyName) ? Name : friendlyName;
         }
      }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
      /// </returns>
      /// <param name="other">An object to compare with this object.</param>
      public bool Equals(TypeMemberSerializationInfo other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return string.Equals(Name, other.Name) && Type == other.Type && string.Equals(EntityName, other.EntityName) && Equals(DataType, other.DataType) && IsAttribute.Equals(other.IsAttribute);
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
         return Equals((TypeMemberSerializationInfo)obj);
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
            var hashCode = (Name != null ? Name.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)Type;
            hashCode = (hashCode * 397) ^ (EntityName != null ? EntityName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (DataType != null ? DataType.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsAttribute.GetHashCode();
            return hashCode;
         }
      }
   }
}
