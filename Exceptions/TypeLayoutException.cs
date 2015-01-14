using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Org.Edgerunner.DotSerialize.Exceptions
{
   public class TypeLayoutException : Exception
   {
      public TypeLayoutException()
      {
         
      }
      public TypeLayoutException(string message)
         : base(message)
      {
         
      }
      public TypeLayoutException(string message, Exception innerException)
         : base(message, innerException)
      {
         
      }
      protected TypeLayoutException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
         : base(info, context)
      {
         
      }
   }
}
