using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Attributes
{
   [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.ReturnValue)]
   public class XmlRootAttribute : Attribute
   {
      public string Name { get; set; }
      public string Namespace { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlRootAttribute"/> class.
      /// </summary>
      /// <param name="name"></param>
      public XmlRootAttribute(string name)
      {
         Name = name;
         Namespace = String.Empty;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="XmlRootAttribute"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="@namespace"></param>
      public XmlRootAttribute(string name, string @namespace)
      {
         Name = name;
         Namespace = @namespace;
      }
   }
}
