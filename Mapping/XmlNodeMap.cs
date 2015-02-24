using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Reflection;

namespace Org.Edgerunner.DotSerialize.Mapping
{
   public class XmlNodeMap
   {
      internal TypeMemberInfo Info { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlNodeMap"/> class.
      /// </summary>
      internal XmlNodeMap(string name, TypeMemberInfo.MemberType memberType, Type dataType)
      {
         Info = new TypeMemberInfo(name, memberType, dataType);
      }

      public XmlNodeMap UsingName(string name)
      {
         Info.EntityName = name;
         return this;
      }

      public XmlNodeMap OrderedAs(int index)
      {
         Info.Order = index;
         return this;
      }

      public XmlNodeMap AsAttribute()
      {
         Info.IsAttribute = true;
         return this;
      }
   }
}
