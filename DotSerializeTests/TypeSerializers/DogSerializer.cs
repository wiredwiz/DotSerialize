using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Org.Edgerunner.DotSerialize.Reflection;
using Org.Edgerunner.DotSerialize.Serialization.Factories;
using Org.Edgerunner.DotSerialize.Serialization.Generic;
using Org.Edgerunner.DotSerialize.Serialization.Reference;
using Org.Edgerunner.DotSerialize.Tests.DataTypes;

namespace Org.Edgerunner.DotSerialize.Tests.TypeSerializers
{
   public class DogSerializer : TypeSerializerBase<Dog>
   {
      public string Title { get; set; }

      /// <summary>
      ///    Initializes a new instance of the <see cref="TypeSerializerBase{T}" /> class.
      /// </summary>
      /// <param name="settings"></param>
      /// <param name="factory"></param>
      /// <param name="inspector"></param>
      /// <param name="refManager"></param>
      /// <param name="title"></param>
      public DogSerializer(Settings settings, ITypeSerializerFactory factory, ITypeInspector inspector, IReferenceManager refManager, string title = "")
         : base(settings, factory, inspector, refManager)
      {
         Title = title;
      }

      protected override object GetEntityValue(object entity, TypeMemberInfo memberInfo)
      {
         var result = base.GetEntityValue(entity, memberInfo);
         if (memberInfo.Name == "<Name>k__BackingField")
            result = string.Format("{0} {1}", Title, result);
         return result;
      }

      /// <summary>
      /// Retrieves the value from the current xml node that the reader is on.
      /// </summary>
      /// <param name="reader">The reader to read from.</param>
      /// <returns>A string containing the contents of the node</returns>
      protected override string GetNodeContents(XmlReader reader)
      {
         var result = base.GetNodeContents(reader);
         if ((reader.Name == "Name") && (result.Substring(0, Title.Length) == Title))
            result = result.Substring(Title.Length);
         return result;
      }
   }
}
