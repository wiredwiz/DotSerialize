using System;
using Org.Edgerunner.DotSerialize.Attributes;

namespace Org.Edgerunner.DotSerialize.Tests.DataTypes
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

      public enum NoseStyle
      {
         Normal,
         Smushed,
         Pointy
      }

      [XmlElement(Order = 1)]
      public virtual string Name { get; set; }
      [XmlElement(Order = 2)]
      public virtual string Breed { get; set; }
      public virtual bool HasCollar { get; set; }
      public Dog.NoseStyle Nose { get; set; }
      public Bone Bone { get; set; }
      [XmlElement(Order = 3)]
      public DateTime BirthDate { get; set; }
      public DogCollar Collar { get; set; }
      // Fields...
      [XmlElement(Order = 4)]
      private int _Age;

      public virtual int Age
      {
         get { return _Age; }
         set
         {
            _Age = value;
         }
      }


      public Owner Owner
      {
         get { return _Owner; }
      }
      [XmlElement(Order = 5)]
      private readonly Owner _Owner;
   }
}
