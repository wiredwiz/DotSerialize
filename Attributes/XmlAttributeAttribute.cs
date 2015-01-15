using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Attributes
{
   [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
   public class XmlAttributeAttribute : Attribute
   {
      public string Name { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlAttributeAttribute"/> class.
      /// </summary>
      /// <param name="name"></param>
      public XmlAttributeAttribute(string name = "")
      {
         Name = name;
      }
   }
}
