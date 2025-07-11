# Dan's HTML Library

An easy-to-use .NET function library for parsing and directly handling HTML and similar markup languages that use angle-bracketed tags. It supports structures where opening tags may contain name="value" property assignments, such as XML.

Following are some basic examples.



## Programmatically Creating an HTML Document from Scratch

This how-to shows you how to build a basic HTML document using the `HtmlDocument` and `HtmlNodeItem` classes. This is useful when you want to programmatically generate HTML content, such as for templating, automation, or internal tools.



### Goal

Create an in-memory HTML document structure that resembles:

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Hello World</title>
  </head>
  <body>
    <h1>Welcome!</h1>
    <p>This document was created using <i>Dans.Html.Library</i>.</p>
  </body>
</html>
```



### Step-by-Step Example (C#)

```c#
using Html;

public class HtmlBuilderExample
{
  //*-----------------------------------------------------------------------*
  //* CreateHtmlDocumentProgrammatically                                    *
  //*-----------------------------------------------------------------------*
  /// <summary>
  /// Create an HTML document programmatically from scratch.
  /// </summary>
  private static void CreateHtmlDocumentProgrammatically()
  {
   HtmlDocument doc = new HtmlDocument();
   HtmlNodeItem docType = new HtmlNodeItem("!DOCTYPE");
   HtmlNodeItem html = new HtmlNodeItem("html");

   // Set the document type to HTML.
   docType.Attributes.Add(new HtmlAttributeItem()
   {
    Name = "html",
    Presence = true
   });

   // Create the head node with a title.
   HtmlNodeItem head = new HtmlNodeItem("head");
   HtmlNodeItem title = new HtmlNodeItem("title", "Hello World!");
   head.Nodes.Add(title);
   html.Nodes.Add(head);

   // Create the body with some basic content.
   HtmlNodeItem body = new HtmlNodeItem("body");
   body.Nodes.Add("h1", "Welcome!");
   body.Nodes.Add("<p>" +
    "This document was created using <i>Dans.Html.Library</i>.</p>",
    true);
   html.Nodes.Add(body);

   // Assemble the document.
   doc.Nodes.Add(docType);
   doc.Nodes.Add(html);

   Console.WriteLine("The programmatic document is:");
   Console.WriteLine(doc.Html);
  }
  //*-----------------------------------------------------------------------*
}

```



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



# Parse HTML Text

To parse an HTML document, you will generally be using the **HtmlDocument.Parse** method, where there are overloads for various conditions.

A usual case follows in the example.



## Step-by-Step Example (C#)

```c#
using Html;

public class HtmlBuilderExample
{
  //*-----------------------------------------------------------------------*
  //* CreateHtmlDocumentFromContent                                         *
  //*-----------------------------------------------------------------------*
  /// <summary>
  /// Create an HTML document from string content.
  /// </summary>
  private static void CreateHtmlDocumentFromContent()
  {
   StringBuilder builder = new StringBuilder();
   Match match = null;

   builder.Append(@"<!DOCTYPE html>
    <html>
     <head>
      <title>Hello World</title>
     </head>
     <body>
      <h1>Welcome!</h1>
      <p>This document was created using the Data.Html library.</p>
     </body>
    </html>");

   // Notice that in the following document, there will be multiple
   // nodes under the html, head, body, and body nodes that have a blank
   // NodeType property. This is done to preserve line feeds in the
   // original content. To remove all line-feeds from the content
   // prior to using the data for processing, you can filter it all
   // out using something like:
   //   doc.Nodes.RemoveAll(x => x.NodeType == "" &&
   //   Regex.IsMatch(x.Text, $"[\r\n\t ]{{{x.Text.Length}}}"));
   // ... which will remove all blank nodes in the document that only
   // define whitespace.
   HtmlDocument doc = HtmlDocument.Parse(builder.ToString(), true);

   // Remove all inter-node whitespace preservation nodes, which are blank.
   // The following code is a more manual alternative to the single line
   // in the previous description.
   List<HtmlNodeItem> matchingNodes =
    doc.Nodes.FindMatches(x => x.NodeType == "");
   foreach(HtmlNodeItem matchingNodeItem in matchingNodes)
   {
    match = Regex.Match(matchingNodeItem.Text,
     $"[\r\n\t ]{{{matchingNodeItem.Text.Length}}}");
    if(match.Success)
    {
     matchingNodeItem.Parent.Remove(matchingNodeItem);
    }
   }

   // Trim the text of all of the nodes.
   List<HtmlNodeItem> flatNodesList =
    doc.Nodes.FindMatches(x => x.Text?.Length >= 0);
   foreach(HtmlNodeItem nodeItem in flatNodesList)
   {
    nodeItem.Text = nodeItem.Text.Trim();
   }

   Console.WriteLine("The document from content is:");
   Console.WriteLine(doc.Html);

  }
  //*-----------------------------------------------------------------------*

}
```



## Updates

| Version | Description |
|---------|-------------|
| 25.2711.4233 | Any inner text following a comment block is now parsed into the object model as a blank sibling to that comment with its Text property set; an issue where the trailing text at the end of a child node was being placed on a new line has been resolved; instance-level **GetValue(string attributeName)** function has been added to the **HtmlAttributeCollection** class, to match the **GetStyle(string styleName)** function. |
| 25.2515.4053 | A bug was fixed that had been introduced in version 25.2515.3752. The error was that no HTML nodes had closing tags. |
| 25.2515.3752 | The static **Singles** property has been moved from **HtmlDocument** to **HtmlUtil**, and is now initialized at startup; **HtmlDocument.Singles** has been depreciated; static read-only **HtmlUtil.HtmlNodeTypes** property has been added to return all of the currently known HTML node types. |
| 25.2513.3949 | When child nodes are added to an **HtmlNodeItem**, the parent node's **SelfClosing** property is now reset automatically. |
| 25.2504.3941 | Change in assembly name; single element node tags have been made case-insensitive; **HtmlNodeItem(string nodeType, string text)** constructor overload has been added to accept node type and text; **HtmlNodeItem.ToString()** now returns the item's node type to aid in debugging. |
| 25.2502.4550 | Initial public release. |


## More Information

For more information, please see the GitHub project:
[danielanywhere/Html](https://github.com/danielanywhere/Html)

