using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Attributes;

namespace DotSerializeTests.DataClasses
{
   public class Person
   {
      [XmlIgnore]
      public virtual string FirstName { get; set; }
      [XmlIgnore]
      public virtual string MiddleInitial { get; set; }
      [XmlIgnore]
      public virtual string LastName { get; set; }
      [XmlIgnore]
      public virtual int Age { get; set; }

      [XmlElement]
      public virtual string FullName
      {
         get { return String.Format("{0} {1} {2}", FirstName, MiddleInitial, LastName); }
         set
         {
            var names = value.Split(' ');
            FirstName = names[0];
            MiddleInitial = names[1];
            LastName = names[2];
         }
      }
      
   }
}
