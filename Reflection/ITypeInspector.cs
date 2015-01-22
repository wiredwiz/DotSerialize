using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Fasterflect;
using Org.Edgerunner.DotSerialize.Attributes;
using Org.Edgerunner.DotSerialize.Exceptions;
using Org.Edgerunner.DotSerialize.Reflection.Caching;

namespace Org.Edgerunner.DotSerialize.Reflection
{
   public interface ITypeInspector
   {
      TypeInfo GetInfo(string fullyQualifiedTypeName);
      TypeInfo GetInfo(Type type);
   }
}
