using System;
using Org.Edgerunner.DotSerialize.Mapping;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.Maps
{
   public sealed class CatMap2 : XmlClassMap<Cat>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CatMap2"/> class.
      /// </summary>
      public CatMap2()
      {
         Map(x => x.Name).OrderedAs(0);
         Map(x => x.Breed).OrderedAs(2);
         Map(x => x.Selfish).UsingName("ActsLikeACat").AsAttribute();
      }
   }
}
