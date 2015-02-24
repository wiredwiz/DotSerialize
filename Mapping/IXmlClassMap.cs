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
   public interface IXmlClassMap<T>
   {
      XmlClassMap<T> Named(string name);
      XmlClassMap<T> WithNamespace(string @namespace);
   }
}
