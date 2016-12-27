// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseColorFinder.cs" company="Obscureware Solutions">
// MIT License
//
// Copyright(c) 2015-2016 Sebastian Gruchacz
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </copyright>
// <summary>
//   Defines the CloseColorFinder class responsible for color matching routines.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Class responsible for finding closest color index from given console colors array.
    /// </summary>
    public class CloseColorFinder : IDisposable
    {
        private readonly ConcurrentDictionary<Color, ConsoleColor> _knownMappings = new ConcurrentDictionary<Color, ConsoleColor>();

        private readonly KeyValuePair<ConsoleColor, Color>[] _colorBuffer;

        private readonly ColorBalancer _colorBalancer;

        public CloseColorFinder(KeyValuePair<ConsoleColor, Color>[] colorBuffer, ColorBalancer colorBalancer = null)
        {
            this._colorBuffer = colorBuffer;
            this._colorBalancer = colorBalancer ?? ColorBalancer.Default;
        }

        /// <summary>
        /// Tries to find the closest match for given RGB color among current set of colors used by System.Console
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <remarks>Influenced by http://stackoverflow.com/questions/1720528/what-is-the-best-algorithm-for-finding-the-closest-color-in-an-array-to-another</remarks>
        public ConsoleColor FindClosestColor(Color color)
        {
            ConsoleColor cc;
            if (this._knownMappings.TryGetValue(color, out cc))
            {
                return cc;
            }

            cc = this._colorBuffer.OrderBy(kp => this._colorBalancer.ColorMatching(color, kp.Value)).First().Key;
            this._knownMappings.TryAdd(color, cc);
            return cc;
        }

        /// <summary>
        /// Returns actual ARGB color stored at console enumerated colors.
        /// </summary>
        /// <param name="cc">Enumeration-index in console colors</param>
        /// <returns>ARGB color.</returns>
        public Color GetCurrentConsoleColor(ConsoleColor cc)
        {
            return this._colorBuffer.Single(pair => pair.Key == cc).Value;
        }

        public static CloseColorFinder GetDefault()
        {
            return new CloseColorFinder(GetDefaultDefinitions().ToArray());
        }

        public static CloseColorFinder CustomizedDefault(params Tuple<ConsoleColor, Color>[] overwrites)
        {
            var dict = GetDefaultDefinitions().ToDictionary(p => p.Key, p => p.Value);
            foreach (var overwrite in overwrites)
            {
                dict[overwrite.Item1] = overwrite.Item2;
            }

            return new CloseColorFinder(dict.ToArray());
        }

        /// <summary>
        /// Returns default color-set
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<ConsoleColor, Color>> GetDefaultDefinitions()
        {
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Black, Color.FromArgb(0, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.FromArgb(0, 0, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGreen, Color.FromArgb(0, 128, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.FromArgb(0, 128, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkRed, Color.FromArgb(128, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkMagenta, Color.FromArgb(128, 0, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkYellow, Color.FromArgb(128, 128, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Gray, Color.FromArgb(192, 192, 192));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.DarkGray, Color.FromArgb(128, 128, 128));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Blue, Color.FromArgb(0, 0, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Green, Color.FromArgb(0, 255, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Cyan, Color.FromArgb(0, 255, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Red, Color.FromArgb(255, 0, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Magenta, Color.FromArgb(255, 0, 255));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.Yellow, Color.FromArgb(255, 255, 0));
            yield return new KeyValuePair<ConsoleColor, Color>(ConsoleColor.White, Color.FromArgb(255, 255, 255));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // TODO: just do invalidation when disposed...
        }
    }
}