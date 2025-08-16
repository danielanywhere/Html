/*
 * Copyright (c). 2000 - 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */

using Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlTests
{
	//*-------------------------------------------------------------------------*
	//*	Program																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Main instance of the HTML Tests application.
	/// </summary>
	public class Program
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* CreateHtmlDocumentFromContent																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create an HTML document from string content.
		/// </summary>
		private static void CreateHtmlDocumentFromContent()
		{
			StringBuilder builder = new StringBuilder();

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

			//	Trim the text of all of the nodes.
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

		//*-----------------------------------------------------------------------*
		//* CreateHtmlDocumentFromImageBank																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create an HTML document from image bank content.
		/// </summary>
		private static void CreateHtmlDocumentFromImageBank()
		{
			HtmlDocument doc = new HtmlDocument(ResourceMain.ImageMapHTML);
			Console.WriteLine(doc.ToString());
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreateHtmlDocumentProgrammatically																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create an HTML document programmatically from scratch.
		/// </summary>
		private static void CreateHtmlDocumentProgrammatically()
		{
			HtmlDocument doc = new HtmlDocument();
			HtmlNodeItem docType = new HtmlNodeItem("!DOCTYPE");
			HtmlNodeItem html = new HtmlNodeItem("html");

			//	Set the document type to HTML.
			docType.Attributes.Add(new HtmlAttributeItem()
			{
				Name = "html",
				Presence = true
			});

			//	Create the head node with a title.
			HtmlNodeItem head = new HtmlNodeItem("head");
			HtmlNodeItem title = new HtmlNodeItem("title", "Hello World!");
			head.Nodes.Add(title);
			html.Nodes.Add(head);

			//	Create the body with some basic content.
			HtmlNodeItem body = new HtmlNodeItem("body");
			body.Nodes.Add("h1", "Welcome!");
			body.Nodes.Add("<p>" +
			 "This document was created using <i>Dans.Html.Library</i>.</p>",
			 true);
			html.Nodes.Add(body);

			//	Assemble the document.
			doc.Nodes.Add(docType);
			doc.Nodes.Add(html);

			Console.WriteLine("The programmatic document is:");
			Console.WriteLine(doc.Html);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreatePartialHtmlDocumentProgrammatically															*
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

		//*-----------------------------------------------------------------------*
		//* RemoveAllLineFeeds																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Render the document without line feeds.
		/// </summary>
		/// <remarks>
		/// By default, elements will render with a minimal number of line-feeds
		/// to help separate content, but it is possible to force all elements
		/// to render back to back, if you place the node into a document then
		/// set that document's LineFeed property to false.
		/// </remarks>
		private static void RemoveAllLineFeeds()
		{
			HtmlDocument doc = new HtmlDocument() { LineFeed = false };
			HtmlNodeItem ul = new HtmlNodeItem("ul");


			ul.Attributes.AddClass("general-list");

			ul.Nodes.Add(
				"<li><b>First item</b>. This is the first item.</li>", true);
			ul.Nodes.Add(
				"<li><b>Second item</b>. A second item.</li>", true);
			ul.Nodes.Add(
				"<li><b>Third</b>. This is probably the third item.</li>", true);

			doc.Nodes.Add(ul);

			Console.WriteLine("The list HTML is:");
			Console.WriteLine(ul.Html);
			Console.WriteLine("");
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Main																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Configure and run the application.
		/// </summary>
		public static void Main(string[] args)
		{
			bool bShowHelp = false; //	Flag - Explicit Show Help.
			string key = "";        //	Current Parameter Key.
			string lowerArg = "";   //	Current Lowercase Argument.
			string message = "";    //	Message to display in Console.
			Program prg = new Program();  //	Initialized instance.


			Console.WriteLine("HtmlTests.exe");
			foreach(string arg in args)
			{
				lowerArg = arg.ToLower();
				key = "/?";
				if(lowerArg == key)
				{
					bShowHelp = true;
					continue;
				}
				//key = "/exampleparameter:";
				//if(lowerArg.StartsWith(key))
				//{
				//	prg.exampleparameter = arg.Substring(key.Length);
				//	continue;
				//}
				key = "/wait";
				if(lowerArg.StartsWith(key))
				{
					prg.mWaitAfterEnd = true;
					continue;
				}
			}
			if(bShowHelp)
			{
				//	Display Syntax.
				Console.WriteLine(message.ToString() + "\r\n" + ResourceMain.Syntax);
			}
			else
			{
				//	Run the configured application.
				prg.Run();
			}
			if(prg.mWaitAfterEnd)
			{
				Console.WriteLine("Press [Enter] to exit...");
				Console.ReadLine();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Run																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Run the configured application.
		/// </summary>
		public void Run()
		{
			CreateHtmlDocumentFromContent();
			CreateHtmlDocumentProgrammatically();
			CreatePartialHtmlDocumentProgrammatically();
			RemoveAllLineFeeds();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaitAfterEnd																													*
		//*-----------------------------------------------------------------------*
		private bool mWaitAfterEnd = true;
		/// <summary>
		/// Get/Set a value indicating whether to wait for user keypress after
		/// processing has completed.
		/// </summary>
		public bool WaitAfterEnd
		{
			get { return mWaitAfterEnd; }
			set { mWaitAfterEnd = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
