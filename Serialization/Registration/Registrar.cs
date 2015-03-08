#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using Ninject.Syntax;
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Registration
{
   /// <summary>
   /// Type serializer registrar.
   /// </summary>
   /// <typeparam name="T">Type of object for which a custom serializer is being registered.</typeparam>
   public class Registrar<T>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="Registrar" /> class.
      /// </summary>
      /// <param name="currentSerializer"></param>
      public Registrar(Serializer currentSerializer)
      {
         CurrentSerializer = currentSerializer;
      }

      protected Serializer CurrentSerializer { get; set; }

      /// <summary>
      /// Registers a custom type serializer to the registrar type.
      /// </summary>
      /// <typeparam name="TImplementation">Type of custom serializer.</typeparam>
      /// <returns></returns>
      public RegistrarBinding<TImplementation> ToTypeSerializer<TImplementation>() where TImplementation : ITypeSerializer
      {
         return new RegistrarBinding<TImplementation>(CurrentSerializer.RegisterTypeSerializer(typeof(ITypeSerializer<T>), typeof(TImplementation)));
      }
   }
}