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
using System.Xml;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Reflection.Types;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Generic;
using Org.Edgerunner.DotSerialize.Serialization.Reference;

namespace Org.Edgerunner.DotSerialize.Serialization
{
   public class DefaultTypeSerializer : TypeSerializerBase<object>
   {
      /// <summary>
      ///    Initializes a new instance of the <see cref="DefaultTypeSerializer" /> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      public DefaultTypeSerializer(Settings settings,
                                   ITypeSerializerFactory factory,
                                   ITypeInspector inspector,
                                   IReferenceManager refManager)
         : base(settings, factory, inspector, refManager)
      {
      }

      public virtual T Deserialize<T>(XmlReader reader)
      {
         if (reader == null) throw new ArgumentNullException("reader");
         return (T)Deserialize(reader, typeof(T));
      }

      public virtual void Serialize<T>(XmlWriter writer, T obj)
      {
         if (writer == null) throw new ArgumentNullException("writer");
         Serialize(writer, typeof(T), obj);
      }
   }
}