## Parse HTML Text

To parse an HTML document, you will generally be using the **HtmlDocument.Parse** method, where there are overloads for various conditions.

A usual type of case follows in the example.

<p>&nbsp;</p>

### Step-by-Step Example (C#)

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
