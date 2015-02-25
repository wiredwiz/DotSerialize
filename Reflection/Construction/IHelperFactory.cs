using System;

namespace Org.Edgerunner.DotSerialize.Reflection.Construction
{
   /// <summary>Defines a method to create an instance of some type.</summary>
   public interface IHelperFactory
   {
      /// <summary>Creates a new instance of some type.</summary>
      /// <returns>A new instance of some Type.</returns>
      object CreateInstance();
   }
}
