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
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class MappingException : Exception
   {
      public MappingException()
      {
      }

      public MappingException(string message)
         : base(message)
      {
      }

      public MappingException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      protected MappingException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}