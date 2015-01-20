using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Edgerunner.DotSerialize.Attributes;

namespace DotSerializeTests.DataTypes
{
   public class Dog
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Dog"/> class.
      /// </summary>
      /// <param name="name"></param>
      /// <param name="breed"></param>
      /// <param name="hasCollar"></param>
      /// <param name="owner"></param>
      public Dog(string name, string breed, bool hasCollar, Owner owner)
      {
         Name = name;
         Breed = breed;
         HasCollar = hasCollar;
         _Owner = owner;
         _Age = 0;
      }

      public virtual string Name { get; set; }
      public virtual string Breed { get; set; }
      public virtual bool HasCollar { get; set; }
      // Fields...
      private int _Age;

      public virtual int Age
      {
         get { return _Age; }
         set
         {
            _Age = value;
         }
      }

      private readonly Owner _Owner;
   }
}
