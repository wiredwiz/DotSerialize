﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Attributes;

namespace DotSerializeTests.DataClasses
{
   public class Owner : Person
   {
      [XmlElement]
      public override int Age
      {
         get { return base.Age; }
         set { base.Age = value; }
      }
   }
}
