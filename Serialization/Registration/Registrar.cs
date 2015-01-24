using System;

namespace Org.Edgerunner.DotSerialize.Serialization.Registration
{
   public class Registrar<T>
   {
      protected Serializer CurrentSerializer {get; set;}

      /// <summary>
      /// Initializes a new instance of the <see cref="Registrar"/> class.
      /// </summary>
      /// <param name="currentSerializer"></param>
      public Registrar(Serializer currentSerializer)
      {
         CurrentSerializer = currentSerializer;
      }

      public void To<TImplementation>()
      {
         CurrentSerializer.RegisterTypeSerializer(typeof(T), typeof(TImplementation));
      }
   }
}
