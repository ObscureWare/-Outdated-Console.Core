namespace ObscureWare.Console.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class ColoringVisualTests
    {
        private const string TEST_ROOT = @"C:\\TestResults\\ConsoleColors\\";

        // This is actually not possible to programmatically verify how "close" were colors matched - it's all manual testing...
        // Therefore - help page will be rendered

        [Fact]
        public void PrintDefaultColorsTest()
        {
            string fName = "default_setup.html";
            using (var colorHelper = CloseColorFinder.GetDefault())
            {
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        [Fact]
        public void PrintCustomizedColorsBySeba()
        {
            string fName = "custom_setup_seba.html";
            using (var colorHelper = CloseColorFinder.CustomizedDefault(
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
                    new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue)))
            { 
                PrintAllNamedColorsToHtml(colorHelper, fName);
            }
        }

        // TODO: find exact colors for demo purposes
        //[Fact]
        //public void PrintCustomizedColorsByDnv()
        //{
        //    string fName = "custom_setup_dnv.html";
        //    using (var colorHelper = CloseColorFinder.CustomizedDefault(
        //            new Tuple<ConsoleColor, Color>(ConsoleColor.DarkCyan, Color.Chocolate),
        //            new Tuple<ConsoleColor, Color>(ConsoleColor.Blue, Color.DodgerBlue),
        //            new Tuple<ConsoleColor, Color>(ConsoleColor.Yellow, Color.Gold),
        //            new Tuple<ConsoleColor, Color>(ConsoleColor.DarkBlue, Color.MidnightBlue)))
        //    {
        //        PrintAllNamedColorsToHTML(colorHelper, fName);
        //    }
        //}

        private static void PrintAllNamedColorsToHtml(CloseColorFinder helper, string fName)
        {
            var props = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .Where(p => p.PropertyType == typeof(Color));

            var colorsVersionAtt = typeof(CloseColorFinder).Assembly.GetCustomAttributes().FirstOrDefault(att => att is AssemblyFileVersionAttribute) as AssemblyFileVersionAttribute;
            string colorsVersion = colorsVersionAtt?.Version ?? "unknown";

            var dir = $"{TEST_ROOT}{colorsVersion}\\";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var path = Path.Combine(dir, fName);

            using (var tw = new StreamWriter(path))
            {
                // TODO: print table with console colors

                tw.WriteLine("<html><body><table>");
                tw.WriteLine("<tr><th>ColorName</th><th>Sys Color</th><th>Console Color</th></tr>");

                foreach (var propertyInfo in props)
                {
                    tw.WriteLine("<tr>");

                    Color c = (Color)propertyInfo.GetValue(null);
                    ConsoleColor cc = helper.FindClosestColor(c);

                    Color cCol = helper.GetCurrentConsoleColor(cc);
                    var ccName = Enum.GetName(typeof(ConsoleColor), cc);

                    tw.WriteLine($"<td>{propertyInfo.Name}</td><td bgcolor=\"{c.ToRgbHex()}\">{c.Name}</td><td bgcolor=\"{cCol.ToRgbHex()}\">{ccName}</td>");
                    tw.WriteLine("</tr>");
                }

                tw.WriteLine("</table></body></html>");
                tw.Close();
            }
        }
    }
}

