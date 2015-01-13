using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class ClassSerializationInfo
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ClassSerializationInfo"/> class.
      /// </summary>
      /// <param name="className"></param>
      /// <param name="fullyQualifiedTypeName"></param>
      public ClassSerializationInfo(string className, string fullyQualifiedTypeName)
      {
         ClassName = className;
         EntityName = className;
         FullyQualifiedTypeName = fullyQualifiedTypeName;
         FieldInfoByName = new Dictionary<string, FieldSerializationInfo>();
         FieldInfoByEntityName = new Dictionary<string, FieldSerializationInfo>();
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="ClassSerializationInfo"/> class.
      /// </summary>
      /// <param name="className"></param>
      /// <param name="fullyQualifiedTypeName"></param>
      public ClassSerializationInfo(string className, string fullyQualifiedTypeName, IList<FieldSerializationInfo> fields)
      {
         ClassName = className;
         EntityName = className;
         FullyQualifiedTypeName = fullyQualifiedTypeName;
         FieldInfoByName = new Dictionary<string, FieldSerializationInfo>();
         FieldInfoByEntityName = new Dictionary<string, FieldSerializationInfo>();
         foreach (FieldSerializationInfo field in fields)
         {
            FieldInfoByName.Add(field.FieldName, field);
            FieldInfoByEntityName.Add(field.EntityName, field);
         }
      }

      public string ClassName { get; set; }
      public string FullyQualifiedTypeName { get; set; }
      public string EntityName { get; set; }
      public IDictionary<string, FieldSerializationInfo> FieldInfoByName { get; set; }
      public IDictionary<string, FieldSerializationInfo> FieldInfoByEntityName { get; set; }
   }
}
