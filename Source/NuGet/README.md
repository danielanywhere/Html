# Dan's HTML Library

An easy-to-use .NET function library for parsing and directly handling HTML and similar markup languages that use angle-bracketed tags. It supports structures where opening tags may contain name="value" property assignments, such as XML.

Basic Example:

```cs
using System;
using Html;

// Create a new HtmlDocument and add a paragraph node
// with class 'fancy'.
namespace MyProject
{
 public class Program
 {
  public static void Main(string[] args)
  {
   HtmlDocument doc = new HtmlDocument();
   HtmlNodeItem node = new HtmlNodeItem() { NodeType = "p" };

   node.Attributes.AddClass("fancy");
   doc.Nodes.Add(node);
  }
 }
}

```

## Updates

| Version | Description |
|---------|-------------|
| 25.2502.4550 | Initial public release. |


## More Information

For more information, please see the GitHub project:
[danielanywhere/Html](https://github.com/danielanywhere/Html)

Full API documentation is available at this library's [GitHub User Page](https://danielanywhere.github.io/Geometry).

