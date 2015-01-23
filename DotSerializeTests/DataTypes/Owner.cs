using System;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
{
   [XmlRoot("DogOwner")]
   public class Owner : Person
   {
      [XmlElement]
      public override int Age
      {
         get { return base.Age; }
         set { base.Age = value; }
      }

      public Dog[] Dogs { get; set; }
   }
}
