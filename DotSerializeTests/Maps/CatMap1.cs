using System;
using Org.Edgerunner.DotSerialize.Mapping;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.Maps
{
   public sealed class CatMap1 : XmlClassMap<Cat>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CatMap1"/> class.
      /// </summary>
      public CatMap1()
      {
         Map(x => x.Name).OrderedAs(0);
         Map(x => x.Breed).OrderedAs(2);
         Map(x => x.Age).OrderedAs(1);
         Map(x => x.Selfish).UsingName("ActsLikeACat").AsAttribute();
      }
   }
}
