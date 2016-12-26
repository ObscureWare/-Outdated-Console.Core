// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemConsole.cs" company="Obscureware Solutions">
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
//   Defines the core SystemConsole wrapper on SystemConsole that implements IConsole interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ObscureWare.Console
{
    using System;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Wraps System.Console with IConsole interface methods
    /// </summary>
    public class SystemConsole : IConsole
    {
        private readonly ConsoleController _controller;

        /// <summary>
        /// In characters...
        /// </summary>
        public Point WindowSize { get; }

        public SystemConsole(ConsoleController controller, bool isFullScreen)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            this._controller = controller;

            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            if (isFullScreen)
            {
                this.SetConsoleWindowToFullScreen();

                // now can calculate how large could be full-screen buffer

                // SG: (-2) On Win8 the only working way to keep borders on the screen :(
                // (-1) required on Win10 though :(
                this.WindowSize = new Point(Console.LargestWindowWidth - 2, Console.LargestWindowHeight - 1);

                // setting full-screen
                Console.BufferWidth = this.WindowSize.X;
                Console.WindowWidth = this.WindowSize.X;
                Console.BufferHeight = this.WindowSize.Y;
                Console.WindowHeight = this.WindowSize.Y;
                Console.SetWindowPosition(0, 0);
            }
            else
            {
                // set console (buffer) little bigger by default
                // TODO: use more constructors / methods to control window / buffer / size / position. Perhaps expose dedicated control interface?
                Console.BufferWidth = 120;
                Console.BufferHeight = 500;
                Console.WindowWidth = 120;
                Console.WindowHeight = 500;
            }

            this.WindowWidth = Console.WindowWidth;
            this.WindowHeight = Console.WindowHeight;
        }

        private void SetConsoleWindowToFullScreen()
        {
            // http://www.codeproject.com/Articles/4426/Console-Enhancements
            this.SetWindowPosition(
                0,
                0,
                Screen.PrimaryScreen.WorkingArea.Width - (2 * 16),
                Screen.PrimaryScreen.WorkingArea.Height - (2 * 16) - SystemInformation.CaptionHeight);
        }

        // TODO: introduce atomic operations for asynchronous writes...

        public void WriteText(int x, int y, string text, Color foreColor, Color bgColor)
        {
            this.SetCursorPosition(x, y);
            this.SetColors(foreColor, bgColor);
            this.WriteText(text);
        }

        private void WriteText(string text)
        {
            Console.Write(text);
        }

        void IConsole.WriteText(string text)
        {
            this.WriteText(text);
        }

        public void WriteLine(ConsoleFontColor colors, string text)
        {
            this.SetColors(colors.ForeColor, colors.BgColor);
            Console.WriteLine(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void SetColors(Color foreColor, Color bgColor)
        {
            Console.ForegroundColor = this._controller.CloseColorFinder.FindClosestColor(foreColor);
            Console.BackgroundColor = this._controller.CloseColorFinder.FindClosestColor(bgColor);
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void WriteText(ConsoleFontColor colors, string text)
        {
            this.SetColors(colors.ForeColor, colors.BgColor);
            Console.Write(text);
        }

        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }

        public void WriteText(char character)
        {
            Console.Write(character);
        }

        public int WindowHeight { get; }

        public int WindowWidth { get; }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void SetColors(ConsoleFontColor style)
        {
            this.SetColors(style.ForeColor, style.BgColor);
        }

        public void ReplaceConsoleColor(ConsoleColor color, Color rgbColor)
        {
            this._controller.ReplaceConsoleColor(color, rgbColor);
        }

        /// <summary>
        /// Sets the console window location and size in pixels
        /// </summary>
        public void SetWindowPosition(int x, int y, int width, int height)
        {
            IntPtr hwnd = NativeMethods.GetConsoleWindow();
            NativeMethods.SetWindowPos(hwnd, IntPtr.Zero, x, y, width, height, NativeMethods.SWP_NOZORDER | NativeMethods.SWP_NOACTIVATE);
            // no release handle?
        }
    }
}