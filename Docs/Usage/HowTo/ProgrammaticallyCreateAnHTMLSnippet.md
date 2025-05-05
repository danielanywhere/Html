## Programmatically Create an HTML Snippet

Creating a small HTML snippet is as easy as creating a new HtmlNodeItem and populating it. No further configuration is needed.

Once created in memory, it can be merged into other trees, serialized, or processed in any way you like.

Any snippet can be rendered to HTML by reading the **Html** property, the same way you would do with a full HtmlDocument.

### Goal

Create an in-memory HTML node that resembles:

```html
<div class="paragraph-list">
 <p>This is one of the paragraphs.</p>
 <p>This is the second paragraph.</p>
 <p>This is the last paragraph.</p>
</div>
```

<p>&nbsp;</p>

### Step-by-Step Example (C#)

```c#
using Html;

public class HtmlBuilderExample
{
  //*-----------------------------------------------------------------------*
  //* CreatePartialHtmlDocumentProgrammatically                             *
  //*-----------------------------------------------------------------------*
  /// <summary>
  /// Create a partial HTML document programmatically from scratch.
  /// </summary>
  /// <remarks>
  /// In this example, a stand-alone, self-renderable snippet is created.
  /// </remarks>
  private static void CreatePartialHtmlDocumentProgrammatically()
  {
   HtmlNodeItem div = new HtmlNodeItem("div");

   div.Attributes.AddClass("paragraph-list");

   div.Nodes.Add("p", "This is one of the paragraphs.");
   div.Nodes.Add("p", "This is the second paragraph.");
   div.Nodes.Add("p", "This is the last paragraph.");

   Console.WriteLine("The snippet is:");
   Console.WriteLine(div.Html);
   Console.WriteLine("");
  }
  //*-----------------------------------------------------------------------*
}
```

