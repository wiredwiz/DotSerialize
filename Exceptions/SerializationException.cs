using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class SerializationException : Exception
   {
      public SerializationException()
      {
         
      }
      public SerializationException(string message)
         : base(message)
      {
         
      }
      public SerializationException(string message, Exception innerException)
         : base(message, innerException)
      {
         
      }
      protected SerializationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
         : base(info, context)
      {
         
      }
   }
}
