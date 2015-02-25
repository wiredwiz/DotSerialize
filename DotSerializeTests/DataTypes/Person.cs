using System;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   public class Person
   {
      [XmlIgnore]
      public virtual string FirstName { get; set; }
      [XmlIgnore]
      public virtual string MiddleInitial { get; set; }
      [XmlIgnore]
      public virtual string LastName { get; set; }
      public virtual int Age { get; set; }

      public Person Father { get; set; }
      public Person Mother { get; set; }
      public Person[] Children { get; set; }      

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
