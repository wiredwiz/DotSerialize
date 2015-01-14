using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Edgerunner.DotSerialize.Attributes
{
   [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field)]
   public class XmlElementAttribute : Attribute
   {
      public string Name { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="XmlElementAttribute"/> class.
      /// </summary>
      /// <param name="name"></param>
      public XmlElementAttribute(string name = null)
      {
         Name = name;
      }
   }
}
