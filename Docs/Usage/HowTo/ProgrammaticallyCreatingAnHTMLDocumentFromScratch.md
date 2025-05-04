# Programmatically Creating an HTML Document from Scratch

This how-to shows you how to build a basic HTML document using the `HtmlDocument` and `HtmlNodeItem` classes. This is useful when you want to programmatically generate HTML content, such as for templating, automation, or internal tools.

## Goal

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

<p>&nbsp;</p>

## Step-by-Step Example (C#)

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

