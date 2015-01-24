using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class SerializerException : Exception
   {
      public SerializerException()
      {
         
      }
      public SerializerException(string message)
         : base(message)
      {
         
      }
      public SerializerException(string message, Exception innerException)
         : base(message, innerException)
      {
         
      }
      protected SerializerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
         : base(info, context)
      {
         
      }
   }
}
