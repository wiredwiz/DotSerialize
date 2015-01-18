using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class ReferenceException : Exception
   {
      public ReferenceException()
      {
         
      }
      public ReferenceException(string message)
         : base(message)
      {
         
      }
      public ReferenceException(string message, Exception innerException)
         : base(message, innerException)
      {
         
      }
      protected ReferenceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
         : base(info, context)
      {
         
      }
   }
}
