﻿#region Apapche License 2.0

// <copyright file="IHelperFactory.cs" company="Edgerunner.org">
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