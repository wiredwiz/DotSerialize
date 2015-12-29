#region Apapche License 2.0

// <copyright file="ArrayExtensions.cs" company="Edgerunner.org">
// Copyright 2015 Thaddeus Ryker
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

using System;

namespace Org.Edgerunner.DotSerialize.Utilities
{
   /// <summary>
   ///    Class of array extension methods.
   /// </summary>
   public static class ArrayExtensions
   {
      /// <summary>
      ///    Creates and returns an array that maps an absolute index to the individual indexes in a multi-dimensional array.
      /// </summary>
      /// <param name="arrayObject">The array to create a map for.</param>
      /// <returns>The array of array indices that is the map.</returns>
      public static int[][] CreateAbsoluteIndexArrayMap(this Array arrayObject)
      {
         // ReSharper disable once ExceptionNotDocumented
         var max = arrayObject.Length;
         var dimensions = arrayObject.Rank;
         var storage = new int[max][];
         var position = new int[dimensions];
         var maxDimension = dimensions - 1;

         for (var absoluteIndex = 0; absoluteIndex < max; absoluteIndex++)
         {
            var curPosition = new int[dimensions];

            // ReSharper disable ExceptionNotDocumented
            position.CopyTo(curPosition, 0);

            // ReSharper restore ExceptionNotDocumented
            storage[absoluteIndex] = curPosition;

            var workingDimension = maxDimension;

            while ((workingDimension >= 0) && (position[workingDimension] == (arrayObject.GetLength(workingDimension) - 1)))
               workingDimension--;

            if (workingDimension == -1)
               break;

            position[workingDimension]++;
            for (var i = workingDimension + 1; i < dimensions; i++)
               position[i] = 0;
         }

         return storage;
      }
   }
}