using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Mapping;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.Maps
{
   public sealed class CatMap : XmlClassMap<Cat>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CatMap"/> class.
      /// </summary>
      public CatMap()
      {
         Map(x => x.Name).OrderedAs(0);
         Map(x => x.Breed).OrderedAs(2);
         Map(x => x.Age).OrderedAs(1);
         Map(x => x.Selfish).UsingName("IsACat").AsAttribute();
      }
   }
}
