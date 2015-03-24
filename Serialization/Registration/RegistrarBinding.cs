using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Syntax;
using Org.Edgerunner.DotSerialize.Serialization.Factories;

namespace Org.Edgerunner.DotSerialize.Serialization.Registration
{
   /// <summary>
   /// Type serializer registration binding.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class RegistrarBinding<T> where T : ITypeSerializer
   {
      protected readonly IBindingWhenInNamedWithOrOnSyntax<object> _Binding;

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrarBinding"/> class.
      /// </summary>
      /// <param name="binding"></param>
      public RegistrarBinding(IBindingWhenInNamedWithOrOnSyntax<object> binding)
      {
         _Binding = binding;
      }

      /// <summary>
      /// Adds constructor argument to binding so new instances will be created with supplied constructor args.
      /// </summary>
      /// <param name="parameterName">The constructor parameter name to provide a value for.</param>
      /// <param name="parameterValue">The value to use for the constructor parameter.</param>
      /// <returns></returns>
      public RegistrarBinding<T> WithConstructorArgument(string parameterName, object parameterValue)
      {
         _Binding.WithConstructorArgument(parameterName, parameterValue);
         return this;
      }

      /// <summary>
      /// Adds constructor argument to binding so new instances will be created with supplied constructor args.
      /// </summary>
      /// <param name="parameterType">The constructor parameter type to provide a value for.</param>
      /// <param name="parameterValue">The value to use for the constructor parameter.</param>
      /// <returns></returns>
      public RegistrarBinding<T> WithConstructorArgument(Type parameterType, object parameterValue)
      {
         _Binding.WithConstructorArgument(parameterType, parameterValue);
         return this;
      }
   }
}
