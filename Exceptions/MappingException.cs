using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
      protected MappingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
         : base(info, context)
      {
         
      }
   }
}
