﻿#region Apache License 2.0

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
using System.Collections.Generic;
using System.Globalization;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize
{
   /// <summary>The Settings class is used to pass various settings to a <see cref="Serializer"/> instance. These options allow for customized behavior of the <see cref="Serializer"/>.</summary>
   /// <example>
   /// 	<innovasys:widget type="Colorized Example Code Section" layout="block" xmlns:innovasys="http://www.innovasys.com/widgets">
   /// 		<innovasys:widgetproperty layout="inline" name="LanguageName">CS</innovasys:widgetproperty>
   /// 		<innovasys:widgetproperty layout="block" name="Content">Serializer.Instance.Settings.OmitTypeWhenPossible = true;</innovasys:widgetproperty>
   /// 	</innovasys:widget>
   /// 	<code title="" description="" lang="neutral"></code>
   /// </example>
   public class Settings
   {
      /// <summary>A list of all Attributes that will be used to mark type members to be ignored for serialization.</summary>
      /// <remarks>If you have a custom attribute used to decorate any of your classes and you wish all members decorated with that attribute to be ignored by the serializer, you
      /// can simply add the attribute type to this list in the <see cref="Settings"/> instance for your <see cref="Serializer"/>.</remarks>
      /// <value>A list of <see cref="Attribute"/> types.</value>
      public List<Attribute> AttributesToIgnore { get; set; }
      /// <summary>Culture to use when serializing and deserializing data.</summary>
      /// <remarks>The default value is CultureInfo.InvariantCulture.</remarks>
      public CultureInfo Culture { get; set; }
      public bool OmitTypeWhenPossible { get; set; }
      /// <summary>Specifies whether the Version of assembly that the type resides within should be written out in the type attribute of the serialized data.</summary>
      /// <remarks>The default value is false.</remarks>
      public bool IncludeAssemblyVersionWithType { get; set; }
      /// <summary>Specifies whether the <see cref="CultureInfo"/> of assembly that the type resides within should be written out in the type attribute of the serialized data.</summary>
      /// <remarks>The default value is false.</remarks>
      public bool IncludeAssemblyCultureWithType { get; set; }
      /// <summary>Specifies whether the public key of assembly that the type resides within should be written out in the type attribute of the serialized data.</summary>
      /// <remarks>The default value is false.</remarks>
      public bool IncludeAssemblyKeyWithType { get; set; }

      /// <summary>Default settings instance.</summary>
      public static Settings Default
      {
         get { return new Settings(); }
      }

      /// <summary>
      ///    Initializes a new instance of the <see cref="Settings" /> class.
      /// </summary>
      public Settings()
      {
         AttributesToIgnore = new List<Attribute> { new XmlIgnoreAttribute() };
         Culture = CultureInfo.InvariantCulture;
      }
   }
}