// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConsole.cs" company="Obscureware Solutions">
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
//   Defines the core IConsole interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Encapsulates some typical console operations with interface. Expected usages - simple system console, with low resolution and limited colors or graphical console.
    /// </summary>
    /// <remarks>Deliberately removed all formatting overloads that System.Console has. All of this can be done before call.</remarks>
    public interface IConsole
    {
        /// <summary>
        /// Writes specific text at given position, and using given colors
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="text"></param>
        /// <param name="foreColor"></param>
        /// <param name="bgColor"></param>
        void WriteText(int x, int y, string text, Color foreColor, Color bgColor);

        /// <summary>
        /// Clears entire visible console area (window)
        /// </summary>
        void Clear();

        /// <summary>
        /// Writes given text at current cursor position using given colors
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="text"></param>
        void WriteText(ConsoleFontColor colors, string text);

        /// <summary>
        /// Writes given text at current cursor position using most recent colors
        /// </summary>
        /// <param name="text"></param>
        void WriteText(string text);

        /// <summary>
        /// Writes given line of text at current cursor position using given colors
        /// </summary>
        /// <param name="colors"></param>
        /// <param name="text"></param>
        void WriteLine(ConsoleFontColor colors, string text);

        /// <summary>
        /// Writes given text at current cursor position using most recent colors
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text);

        /// <summary>
        /// Sets pair of colors to be used by following "WriteText***" and "WriteLine***" calls
        /// </summary>
        /// <param name="foreColor"></param>
        /// <param name="bgColor"></param>
        void SetColors(Color foreColor, Color bgColor);

        /// <summary>
        /// Positions cursor at specific position ON THE SCREEN, not in the buffer.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetCursorPosition(int x, int y);

        /// <summary>
        /// Writes single character at current cursor position using most recent colors
        /// </summary>
        /// <param name="character"></param>
        void WriteText(char character);

        // TODO: read operations

        // TODO: asynchronous read-write operations? Probably not, but asynchronous writes -  for sure... Maybe move to operations?

        /// <summary>
        /// Gets vertical size of console Window
        /// </summary>
        int WindowHeight { get; }

        /// <summary>
        /// Gets horizontal size of console Window
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// Reads one text line synchronously.
        /// </summary>
        /// <returns></returns>
        string ReadLine();

        /// <summary>
        /// Reads one key or function key pressed in console without actually printing it on the screen.
        /// </summary>
        /// <returns></returns>
        ConsoleKeyInfo ReadKey();

        void WriteLine();

        void SetColors(ConsoleFontColor style);
    }
}
