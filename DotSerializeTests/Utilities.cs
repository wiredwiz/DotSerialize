#region Apache License 2.0

// Copyright 2015 Thaddeus Ryker
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Org.Edgerunner.DotSerialize.Tests
{
   public static class Utilities
   {
      public static void DeleteFile(string fileName)
      {
         if (File.Exists(fileName))
            File.Delete(fileName);
      }

      public static void ExtractEmbeddedFile(string fileName)
      {
         var assembly = Assembly.GetExecutingAssembly();
         using (var resourceStream = assembly.GetManifestResourceStream(fileName))
         {
            if (resourceStream == null)
               throw new MissingManifestResourceException(string.Format("Embedded resource file {0} could not be accessed",
                                                                        fileName));

            using (var sr = new StreamReader(resourceStream))
               using (var sw = File.CreateText(fileName))
               {
                  sw.Write(sr.ReadToEnd());
                  sw.Flush();
               }
         }
      }
   }
}