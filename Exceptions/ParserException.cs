#region Apapche License 2.0

// <copyright file="ParserException.cs" company="Edgerunner.org">
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
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class ParserException : Exception
   {
      public ParserException()
      {
      }

      public ParserException(string message)
         : base(message)
      {
      }

      public ParserException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected ParserException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}