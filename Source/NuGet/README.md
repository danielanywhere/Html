# Dan's HTML Library

🆕 NEW! - If you find this library useful, please consider sponsoring me on [GitHub](https://github.com/sponsors/danielanywhere).

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

   HtmlDocument doc = HtmlDocument.Parse(builder.ToString(), true, false);

   // Trim the text of all of the nodes.
   List<HtmlNodeItem> flatNodesList =
    doc.Nodes.FindMatches(x => x.Text?.Length >= 0);
   foreach(HtmlNodeItem nodeItem in flatNodesList)
   {
    nodeItem.Text = nodeItem.Text.Trim();
    nodeItem.TrailingText = nodeItem.TrailingText.Trim();
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
| 25.2822.3845 | The static **Clone** method has been added to the following classes: **HtmlNodeCollection**, **HtmlNodeItem**, **HtmlAttributeCollection**, **HtmlAttributeItem**, and **NameItem**; The **HtmlAttributeCollection** class now offers the static methods **StyleExists(attributes,styleName)** and **RemoveStyle(attributes,styleName)**; The static function **HtmlNodeItem.HasAncestorNodeType(node, tagName)** has been added to check to see if the node's parent or any other ancestors have the specified tag name; The static **HtmlNodeItem.GetDocument(node)** function has been added; The **HtmlDocument.UniqueIds** collection has been added to help keep track of all unique id values in the document. The collection is not populated by default. If you want to initialize the collection, call **HtmlDocument.FillUniqueIds(document)**. When used in an application where ids are being tracked, the adding of new nodes, deletion of nodes, or renaming of nodes should be accompanied by associated changes to the UniqueIds list; The **HtmlNodeItem.GetIds(node)** static function has been added to retrieve the ids of the specified node and all of its descendants; The static **HtmlAttributeCollection.GetAttributes(node, params string[] names)** function has been added to retrieve all attributes in the collection matching the parameter names; The static method **HtmlAttributeCollection.SetAttributeValue(node, attributeName, attributeValue)** has been added; A static overload of the **HtmlAttributeCollection.GetStyle(node, styleName)** has been added; Add the static overload method **HtmlAttributeCollection.SetStyle(node, styleName, styleValue)**. |
| 25.2819.4048 | After parsing an element with an additional space at the end of the attributes in **PreserveSpace** document mode, a re-render of that element's attributes was producing a trailing no-name, empty-value attribute assignment in the form of '=""'. This condition has been corrected. |
| 25.2819.3939 | Bubble-up event handling is now available on **HtmlNodeCollection**, **HtmlNodeItem**, **HtmlAttributeCollection**, and **HtmlNodeItem**. Please see the API documentation for more information; In **HtmlAttributeItem**, if either **PreSpace** or **AssignmentSpace** values are blank when the document is in **PreserveSpace** mode, they will now be rendered with default values. In the case of **PreSpace**, a single leading space is used and in the case of **AssignmentSpace**, an equal sign with no buffering spaces is used if **Presence == false**. |
| 25.2816.3809 | A **PreserveSpace** option has been added to the **HtmlDocument** that preserves all whitespace during parsing and rendering when set; Each **HtmlNodeItem** now has a **TrailingText** property that contains the information between the end of this node and the beginning of the next. |
| 25.2806.4417 | An element name is now allowed to contain hyphens, underscores, and digits after the first character, which must be a letter; **HtmlNodeItem.InnerHtml** now returns the combined content of the node's **Text** and **Nodes.Html** properties. |
| 25.2711.4233 | Any inner text following a comment block is now parsed into the object model as a blank sibling to that comment with its Text property set; an issue where the trailing text at the end of a child node was being placed on a new line has been resolved; instance-level **GetValue(string attributeName)** function has been added to the **HtmlAttributeCollection** class, to match the **GetStyle(string styleName)** function. |
| 25.2515.4053 | A bug was fixed that had been introduced in version 25.2515.3752. The error was that no HTML nodes had closing tags. |
| 25.2515.3752 | The static **Singles** property has been moved from **HtmlDocument** to **HtmlUtil**, and is now initialized at startup; **HtmlDocument.Singles** has been depreciated; static read-only **HtmlUtil.HtmlNodeTypes** property has been added to return all of the currently known HTML node types. |
| 25.2513.3949 | When child nodes are added to an **HtmlNodeItem**, the parent node's **SelfClosing** property is now reset automatically. |
| 25.2504.3941 | Change in assembly name; single element node tags have been made case-insensitive; **HtmlNodeItem(string nodeType, string text)** constructor overload has been added to accept node type and text; **HtmlNodeItem.ToString()** now returns the item's node type to aid in debugging. |
| 25.2502.4550 | Initial public release. |


## More Information

For more information, please see the GitHub project:
[danielanywhere/Html](https://github.com/danielanywhere/Html)

Full API documentation is available at this library's [GitHub User Page](https://danielanywhere.github.io/Html).

