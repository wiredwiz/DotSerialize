using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public class FieldSerializationInfo
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="fieldName"></param>
      /// <param name="fullyQualifiedTypeName"></param>
      public FieldSerializationInfo(string fieldName, string fullyQualifiedTypeName)
      {
         FieldName = fieldName;
         FullyQualifiedTypeName = fullyQualifiedTypeName;
         EntityName = fieldName;
         IsElement = true;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="fieldName"></param>
      /// <param name="entityName"></param>
      /// <param name="fullyQualifiedTypeName"></param>
      public FieldSerializationInfo(string fieldName, string entityName, string fullyQualifiedTypeName)
      {
         FieldName = fieldName;
         EntityName = entityName;
         FullyQualifiedTypeName = fullyQualifiedTypeName;
         IsElement = true;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="FieldSerializationInfo"/> class.
      /// </summary>
      /// <param name="fieldName"></param>
      /// <param name="entityName"></param>
      /// <param name="fullyQualifiedTypeName"></param>
      /// <param name="isElement"></param>
      public FieldSerializationInfo(string fieldName, string entityName, string fullyQualifiedTypeName, bool isElement)
      {
         FieldName = fieldName;
         EntityName = entityName;
         FullyQualifiedTypeName = fullyQualifiedTypeName;
         IsElement = isElement;
      }
      public string FieldName { get; set; }
      public string EntityName { get; set; }
      public string FullyQualifiedTypeName { get; set; }
      public bool IsElement { get; set; }
   }
}
