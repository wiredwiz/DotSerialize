using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fasterflect;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Utilities;

namespace Org.Edgerunner.DotSerialize.Mapping
{
   public abstract class XmlClassMap<T> : IXmlClassMap<T>
   {
      protected readonly List<XmlNodeMap> _Mappings;
      private string _RootNodeName;
      private string _Namespace;

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlClassMap"/> class.
      /// </summary>
      protected XmlClassMap()
      {
         _Mappings = new List<XmlNodeMap>();
         _RootNodeName = TypeInspector.CleanNodeName(typeof(T).Name());
         _Namespace = string.Empty;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlClassMap"/> class.
      /// </summary>
      /// <param name="rootNodeName"></param>
      protected XmlClassMap(string rootNodeName)
      {
         _RootNodeName = rootNodeName;
         _Mappings = new List<XmlNodeMap>();
         _Namespace = String.Empty;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlClassMap"/> class.
      /// </summary>
      /// <param name="rootNodeName"></param>
      /// <param name="@namespace"></param>
      protected XmlClassMap(string rootNodeName, string @namespace)
      {
         _RootNodeName = rootNodeName;
         _Namespace = @namespace;
         _Mappings = new List<XmlNodeMap>();
      }

      public XmlClassMap<T> Named(string name)
      {
         _RootNodeName = name;
         return this;
      }

      public XmlClassMap<T> WithNamespace(string @namespace)
      {
         _Namespace = @namespace;
         return this;
      }

      /// <summary>
      /// Maps a property to a CSV field.
      /// </summary>
      /// <param name="expression">The property to map.</param>
      /// <returns>The property mapping.</returns>
      protected virtual XmlNodeMap Map(Expression<Func<T, object>> expression)
      {
         var member = TypeHelper.GetMemberExpression(expression).Member;
         XmlNodeMap nodeMap;
         var property = member as PropertyInfo;
         if (property != null)
         {
            nodeMap = new XmlNodeMap(property.Name, Reflection.TypeMemberInfo.MemberType.Property, property.PropertyType);
            _Mappings.Add(nodeMap);
            return nodeMap;
         }
         var field = member as FieldInfo;
         if (field != null)
         {
            nodeMap = new XmlNodeMap(field.Name, Reflection.TypeMemberInfo.MemberType.Field, field.FieldType);
            _Mappings.Add(nodeMap);
            return nodeMap;
         }

         throw new MappingException(string.Format("'{0}' is not a property or field reference.", member.Name));
      }

      internal Reflection.TypeInfo GetTypeInfo()
      {
         var type = typeof(T);
         var memberInfo = new List<TypeMemberInfo>(_Mappings.Count);
         memberInfo.AddRange(_Mappings.Select(mapping => mapping.Info));
         var info = new Reflection.TypeInfo(type.Name, type, _RootNodeName, _Namespace, memberInfo);
         return info;
      }
   }
}
