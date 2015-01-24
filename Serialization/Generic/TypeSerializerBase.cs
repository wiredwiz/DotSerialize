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
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Reference;

namespace Org.Edgerunner.DotSerialize.Serialization.Generic
{
   public abstract class TypeSerializerBase<T> : ITypeSerializer<T>
   {
      protected ITypeSerializerFactory Factory { get; set; }
      protected ITypeInspector Inspector { get; set; }
      protected Settings Settings { get; set; }
      protected IReferenceManager RefManager { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeSerializerBase" /> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      protected TypeSerializerBase(Settings settings,
                                   ITypeSerializerFactory factory,
                                   ITypeInspector inspector,
                                   IReferenceManager refManager)
      {
         Factory = factory;
         Inspector = inspector;
         Settings = settings;
         RefManager = refManager;
      }

      #region ITypeSerializer<T> Members

      public virtual object Deserialize(XmlReader reader, Type type)
      {
         throw new NotImplementedException();
      }

      public virtual void Serialize(XmlWriter writer, Type type, object obj)
      {
         throw new NotImplementedException();
      }

      public virtual T Deserialize(XmlReader reader)
      {
         return (T)Deserialize(reader, typeof(T));
      }

      public virtual void Serialize(XmlWriter writer, T obj)
      {
         Serialize(writer, typeof(T), obj);
      }

      #endregion
   }
}