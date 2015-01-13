using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeMemberSerializationInfo
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
   }
}
