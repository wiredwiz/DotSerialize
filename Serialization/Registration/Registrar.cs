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
using Org.Edgerunner.DotSerialize.Serialization.Generic;

namespace Org.Edgerunner.DotSerialize.Serialization.Registration
{
   public class Registrar<T>
   {
      protected Serializer CurrentSerializer { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="Registrar" /> class.
      /// </summary>
      /// <param name="currentSerializer"></param>
      public Registrar(Serializer currentSerializer)
      {
         CurrentSerializer = currentSerializer;
      }

      public void To<TImplementation>()
      {
         CurrentSerializer.RegisterTypeSerializer(typeof(ITypeSerializer<T>), typeof(TImplementation));
      }
   }
}