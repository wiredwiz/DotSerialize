using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Serialization.Generic;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Reference;
namespace Org.Edgerunner.DotSerialize.Serialization
{
   class CollectionTypeSerializer : TypeSerializerBase<ICollection>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CollectionTypeSerializer"/> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      protected CollectionTypeSerializer(Settings settings, ITypeSerializerFactory factory, ITypeInspector inspector, IReferenceManager refManager)
         : base(settings, factory, inspector, refManager)
      {
         
      }
   }
}
