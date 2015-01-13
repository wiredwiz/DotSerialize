using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class TypeSerializationInfo
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializationInfo"/> class.
      /// </summary>
      /// <param name="typeName"></param>
      /// <param name="dataType"></param>
      public TypeSerializationInfo(string typeName, Type dataType)
      {
         Name = typeName;
         EntityName = typeName;
         DataType = dataType;
         MemberInfoByName = new Dictionary<string, TypeMemberSerializationInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberSerializationInfo>();
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      public TypeSerializationInfo(string name, Type dataType, string entityName)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = String.Empty;
         MemberInfoByName = new Dictionary<string, TypeMemberSerializationInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberSerializationInfo>();
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      /// <param name="@namespace"></param>
      public TypeSerializationInfo(string name, Type dataType, string entityName, string @namespace)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = @namespace;
         MemberInfoByName = new Dictionary<string, TypeMemberSerializationInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberSerializationInfo>();
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializationInfo"/> class.
      /// </summary>
      /// <param name="typeName"></param>
      /// <param name="dataType"></param>
      /// <param name="members"></param>
      public TypeSerializationInfo(string typeName, Type dataType, IList<TypeMemberSerializationInfo> members)
      {
         Name = typeName;
         EntityName = typeName;
         DataType = dataType;
         MemberInfoByName = new Dictionary<string, TypeMemberSerializationInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberSerializationInfo>();
         foreach (TypeMemberSerializationInfo field in members)
         {
            MemberInfoByName.Add(field.Name, field);
            MemberInfoByEntityName.Add(field.EntityName, field);
         }
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="TypeSerializationInfo"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="dataType"></param>
      /// <param name="entityName"></param>
      /// <param name="@namespace"></param>
      /// <param name="members"></param>
      public TypeSerializationInfo(string name, Type dataType, string entityName, string @namespace, IList<TypeMemberSerializationInfo> members)
      {
         Name = name;
         DataType = dataType;
         EntityName = entityName;
         Namespace = @namespace;
         MemberInfoByName = new Dictionary<string, TypeMemberSerializationInfo>();
         MemberInfoByEntityName = new Dictionary<string, TypeMemberSerializationInfo>();
         foreach (TypeMemberSerializationInfo field in members)
         {
            MemberInfoByName.Add(field.Name, field);
            MemberInfoByEntityName.Add(field.EntityName, field);
         }
      }

      public string Name { get; set; }
      public Type DataType { get; set; }
      public string EntityName { get; set; }
      public string Namespace { get; set; }
      public IDictionary<string, TypeMemberSerializationInfo> MemberInfoByName { get; set; }
      public IDictionary<string, TypeMemberSerializationInfo> MemberInfoByEntityName { get; set; }
   }
}
