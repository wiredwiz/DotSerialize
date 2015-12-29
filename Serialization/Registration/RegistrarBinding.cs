#region Apapche License 2.0

// <copyright file="RegistrarBinding.cs" company="Edgerunner.org">
// Copyright 2015 Thaddeus Ryker
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

using System;
using Ninject.Syntax;

namespace Org.Edgerunner.DotSerialize.Serialization.Registration
{
   /// <summary>
   ///    Type serializer registration binding.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class RegistrarBinding<T>
      where T : ITypeSerializer
   {
      protected readonly IBindingWhenInNamedWithOrOnSyntax<object> _Binding;

      /// <summary>
      /// Initializes a new instance of the <see cref="RegistrarBinding{T}"/> class. 
      ///    Initializes a new instance of the <see cref="RegistrarBinding"/> class.
      /// </summary>
      /// <param name="binding">
      /// </param>
      public RegistrarBinding(IBindingWhenInNamedWithOrOnSyntax<object> binding)
      {
         _Binding = binding;
      }

      /// <summary>
      ///    Adds constructor argument to binding so new instances will be created with supplied constructor args.
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
      ///    Adds constructor argument to binding so new instances will be created with supplied constructor args.
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