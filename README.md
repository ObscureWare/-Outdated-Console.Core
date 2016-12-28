# Console.Core
Color control and System.Console base operations wrapper.

*![PayPal](https://github.com/ObscureWare/Console.Commands/blob/master/doc/pp64.png) If you find this library useful please consider [donating](https://www.paypal.me/SebastianGruchacz) to support my work.*

### There are two main purposes of this library:

* Provide more control over console output that .Net implemntation does
* Give the developer ability to freely use regular RGB colors inside application and then let the code try to match one of the currently defined colors.

Two examples of color matching results can be found here:
- Default colors [GitHub](https://github.com/ObscureWare/Console.Core/blob/master/demo/default_setup.html)
- Slightly modified by (SG) for better matching brown colors [GitHub](https://github.com/ObscureWare/Console.Core/blob/master/demo/custom_setup_seba.html)

Code that generates these files is here: [GitHub](https://github.com/ObscureWare/Console.Core/blob/master/src/ObscureWare.Console.Tests/ColoringVisualTests.cs)

#Printing function# 
Just naively renders HTML
```csharp
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
```
#Generating default setup#
```csharp
public void PrintDefaultColorsTest()
{
    string fName = "default_setup.html";
    using (var colorHelper = CloseColorFinder.GetDefault())
    {
        PrintAllNamedColorsToHtml(colorHelper, fName);
    }
}
```
#... and results#
![Default partial demo](https://github.com/ObscureWare/Console.Core/blob/master/demo/Sample_default_1.png)

#Generating my custom alternative#
```csharp
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
```
#... and results#
![Seba's partial demo](https://github.com/ObscureWare/Console.Core/blob/master/demo/Sample_seba_1.png)
