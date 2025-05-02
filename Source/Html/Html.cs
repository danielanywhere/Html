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

using Html.Internal;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlDocument																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Structured HTML Document.
	/// </summary>
	/// <remarks>
	/// 20230715.1116 - Converted to case-sensitive. At this time, it is the
	/// responsibility of the HTML author to write all primary HTML in lower
	/// case.
	/// </remarks>
	public class HtmlDocument : HtmlNodeItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	FormatWhitespace																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Format the Whitespace found in the caller's text.
		/// </summary>
		/// <param name="value">
		/// Text Value to be whitespace formatted.
		/// </param>
		/// <param name="comments">
		/// Value indicating whether to process the text as comments.
		/// </param>
		/// <returns>
		/// Single line of text, having well-formatted whitespace characteristics.
		/// </returns>
		private static string FormatWhitespace(string value, bool comments)
		{
			string rs = value;

			//			if(value.IndexOf("known lead-based paint") >= 0)
			//			{
			//				Debug.WriteLine("Break Here", "HtmlDocument.FormatWhitespace");
			//			}
			if(!comments)
			{
				if(rs != "\r\n")
				{
					rs = Regex.Replace(rs, @"\r", "");
					rs = Regex.Replace(rs,
						@"(?i:(?<f1>[., a-z0-9])\n(?<f2>[., a-z0-9]))",
						"${f1} ${f2}");
					rs = Regex.Replace(rs, @"\n", "");
					rs = Regex.Replace(rs, @"\s+", " ");
				}
				else
				{
					rs = "";
				}
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new Instance of the HtmlDocument Item.
		/// </summary>
		public HtmlDocument()
		{
			mNodes = new HtmlNodeCollection()
			{
				ParentNode = this
			};
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new Instance of the HtmlDocument Item.
		/// </summary>
		/// <param name="html">
		/// Html content to preload the Document with while creating.
		/// </param>
		/// <param name="includeComments">
		/// Value indicating whether to include comments in the content.
		/// </param>
		public HtmlDocument(string html, bool includeComments = true) : this()
		{
			mIncludeComments = includeComments;
			Parse(this, html, mIncludeComments);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit String = HtmlDocument																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the Html Document to a String.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator string(HtmlDocument value)
		{
			string rs = "";

			if(value != null)
			{
				rs = value.ToString();
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from anywhere in the structure by its ID.
		/// </summary>
		public new HtmlNodeItem this[string id]
		{
			get { return Nodes[id]; }
		}
		//*-----------------------------------------------------------------------*

		//	TODO: Set Widths of 99% with no quotes.
		//	TODO: Some non-quoted name=value assignments not appearing.
		//*-----------------------------------------------------------------------*
		//*	AssignAttributes																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Assign attributes found in the HTML Element to Name / Value pairs in
		/// the supplied Attribute Collection.
		/// </summary>
		/// <param name="value">
		/// An HTML Element with opening and closing braces.
		/// </param>
		/// <param name="attributes">
		/// Collection designated to receive all attribute values found.
		/// </param>
		public static void AssignAttributes(string value,
			HtmlAttributeCollection attributes)
		{
			//Group nameGroup = null;								//	Working Name Group.
			//IntRangeCollection irc = new IntRangeCollection();
			//MatchCollection matches;      //	Working Match Collection.
			NameValueCollection nameValues = null;
			//Group valueGroup = null;               //	Working Value Group.
			//string workingName = "";				//	Working Name.
			//string workingValue = "";			//	Working Value.

			//	TODO: Place attributes in a desired order of some sort.
			if(value.Length != 0 && attributes != null)
			{
				nameValues = HtmlUtil.GetHtmlAttributes(value);
				foreach(NameValueItem item in nameValues)
				{
					attributes.Add(item.Name, item.Value);
				}

				////	Parse Items with and without values, in or out of quotes.
				//matches = Regex.Matches(value, ResourceMain.rxHtmlAttributes);
				//foreach(Match matchItem in matches)
				//{
				//	nameGroup = matchItem.Groups["name"];
				//	valueGroup = matchItem.Groups["value"];
				//	workingName = "";
				//	workingValue = "";
				//	if(nameGroup != null && nameGroup.Value != null)
				//	{
				//		workingName = nameGroup.Value;
				//	}
				//	if(valueGroup != null && valueGroup.Value != null)
				//	{
				//		workingValue = valueGroup.Value.Trim();
				//	}
				//	attributes.Add(workingName, workingValue);
				//}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the HTML Document.
		/// </summary>
		public void Clear()
		{
			Nodes.Clear();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Dump																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Dump the contents of the HTML Document.
		/// </summary>
		/// <param name="document">
		/// Instance of an HTML Document to output.
		/// </param>
		public static void Dump(HtmlDocument document)
		{
			if(document != null)
			{
				Debug.WriteLine("*** Entity Tree ***");
				foreach(HtmlNodeItem ni in document.Nodes)
				{
					if(ni.NodeType.Length != 0)
					{
						Debug.WriteLine(ni.NodeType +
							(ni.Id.Length != 0 ? " id:" + ni.Id : "") +
							(ni.Name.Length != 0 ? " name:" + ni.Name : ""));
					}
					if(ni.Text.Length != 0)
					{
						Debug.WriteLine("-text:" + ni.Text);
					}
					Dump(ni.Nodes, 1);
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Dump the contents of an HTML Nodes Collection.
		/// </summary>
		/// <param name="nodes">
		/// Instance of an HTML Nodes Collection to output.
		/// </param>
		/// <param name="level">
		/// Indent Level.
		/// </param>
		public static void Dump(HtmlNodeCollection nodes, int level)
		{
			if(nodes != null)
			{
				foreach(HtmlNodeItem ni in nodes)
				{
					if(ni.NodeType.Length != 0)
					{
						Debug.WriteLine(("").PadRight(level, ' ') + ni.NodeType +
							(ni.Id.Length != 0 ? " id:" + ni.Id : "") +
							(ni.Name.Length != 0 ? " name:" + ni.Name : ""));
					}
					if(ni.Text.Length != 0)
					{
						Debug.WriteLine(("").PadRight(level, ' ') + "-text:" + ni.Text);
					}
					Dump(ni.Nodes, level + 1);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetElementType																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Element Type of the caller-supplied element.
		/// </summary>
		/// <param name="value">
		/// An Html Element with opening and closing braces.
		/// </param>
		/// <returns>
		/// The basic element type, in lower case.
		/// </returns>
		public static string GetElementType(string value)
		{
			MatchCollection mc;                       //	Matches.
			string rs = "";                           //	Return String.

			//	Element Type.
			if(value?.Length > 1)
			{
				mc = Regex.Matches(value, ResourceMain.rxHtmlElementType);
				foreach(Match m in mc)
				{
					if(m.Groups["result"].Value != null)
					{
						rs = m.Groups["result"].Value;
						break;
					}
				}
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNode																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first node found of a given node type.
		/// </summary>
		/// <param name="document">
		/// Document to inspect.
		/// </param>
		/// <param name="nodeType">
		/// Node Type to find.
		/// </param>
		/// <returns>
		/// First node of the specified type, if found. Null otherwise.
		/// </returns>
		public static HtmlNodeItem GetNode(HtmlDocument document, string nodeType)
		{
			string nt = nodeType;
			HtmlNodeItem ro = null;

			if(document != null)
			{
				foreach(HtmlNodeItem ni in document.Nodes)
				{
					if(ni.NodeType == nt)
					{
						ro = ni;
						break;
					}
					ro = HtmlNodeCollection.GetNode(ni.Nodes, nodeType);
					if(ro != null)
					{
						break;
					}
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNodes																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the all nodes found of a given node type.
		/// </summary>
		/// <param name="document">
		/// Document to inspect.
		/// </param>
		/// <param name="nodeType">
		/// Node Type to find.
		/// </param>
		/// <returns>
		/// Collection of nodes of the specified type, if found.
		/// Zero Length Array otherwise.
		/// </returns>
		public static HtmlNodeItem[] GetNodes(HtmlDocument document,
			string nodeType)
		{
			int lp = 0;
			string nt = nodeType;
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem[] ro = new HtmlNodeItem[0];

			if(document != null)
			{
				foreach(HtmlNodeItem ni in document.Nodes)
				{
					if(ni.NodeType == nt)
					{
						oc.Add(ni);
					}
					oc.AddRange(HtmlNodeCollection.GetNodes(ni.Nodes, nt));
				}
			}
			if(oc.Count != 0)
			{
				ro = new HtmlNodeItem[oc.Count];
				foreach(HtmlNodeItem ni in oc)
				{
					ro[lp++] = ni;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNodesWithAttribute																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of nodes having the specified Attribute.
		/// </summary>
		/// <param name="document">
		/// Instance of the Html Document from which the Nodes will be retrieved.
		/// </param>
		/// <param name="attributeName">
		/// Name of the attribute to search for.
		/// </param>
		/// <returns>
		/// Array of Nodes containing the specified Attribute, if found. Otherwise,
		/// a zero length array.
		/// </returns>
		public static HtmlNodeItem[] GetNodesWithAttribute(HtmlDocument document,
			string attributeName)
		{
			int lp = 0;
			HtmlNodeItem[] na = new HtmlNodeItem[0];
			ObjectCollection oc = new ObjectCollection();

			oc.AddRange(
				HtmlNodeCollection.GetNodesWithAttribute(
				document.Nodes, attributeName));
			if(oc.Count != 0)
			{
				na = new HtmlNodeItem[oc.Count];
				foreach(HtmlNodeItem ni in oc)
				{
					na[lp++] = ni;
				}
			}
			return na;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Html																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the HTML Content of this Instance.
		/// </summary>
		public new string Html
		{
			get { return Nodes.Html; }
			set { Parse(this, value, mIncludeComments); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IncludeComments																												*
		//*-----------------------------------------------------------------------*
		private bool mIncludeComments = true;
		/// <summary>
		/// Get/Set a value indicating whether to include comments.
		/// </summary>
		public bool IncludeComments
		{
			get { return mIncludeComments; }
			set { mIncludeComments = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Insert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new node prior to the specified Node in this collection.
		/// </summary>
		/// <param name="document">
		/// Instance of an Html Document Object into which the new Node will be
		/// inserted.
		/// </param>
		/// <param name="beforeNode">
		/// The Node prior to which a new node will be inserted.
		/// </param>
		/// <returns>
		/// Newly created and added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public static HtmlNodeItem Insert(HtmlDocument document,
			HtmlNodeItem beforeNode)
		{
			return document.Nodes.Insert(beforeNode);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert the provided HTML before the specified Node in the appropriate
		/// level.
		/// </summary>
		/// <param name="document">
		/// Instance of an Html Document Object into which the new Node will be
		/// inserted.
		/// </param>
		/// <param name="beforeID">
		/// The Unique ID to find within the document.
		/// </param>
		/// <param name="html">
		/// HTML Content to Insert.
		/// </param>
		/// <returns>
		/// Html Node Item constructed from the caller's HTML.
		/// </returns>
		public static HtmlNodeItem Insert(HtmlDocument document, string beforeID,
			string html)
		{
			return document.Nodes.Insert(beforeID, html);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InsertAfter																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new node after the specified Node in this collection.
		/// </summary>
		/// <param name="document">
		/// Instance of an Html Document Object into which the new Node will be
		/// inserted.
		/// </param>
		/// <param name="afterNode">
		/// The Node after which a new node will be inserted.
		/// </param>
		/// <returns>
		/// Newly created and added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public static HtmlNodeItem InsertAfter(HtmlDocument document,
			HtmlNodeItem afterNode)
		{
			return document.Nodes.InsertAfter(afterNode);
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Insert the provided Html after the specified Node in the appropriate
		///// level.
		///// </summary>
		///// <param name="document">
		///// Instance of an Html Document Object into which the new Node will be
		///// inserted.
		///// </param>
		///// <param name="afterID">
		///// The Unique ID to find within the document.
		///// </param>
		///// <param name="html">
		///// Html Content to Insert.
		///// </param>
		///// <returns>
		///// Html Node Item constructed from the caller's HTML.
		///// </returns>
		//public static HtmlNodeItem InsertAfter(HtmlDocument document,
		//	string afterID, string html)
		//{
		//	return document.Nodes.InsertAfter(afterID, html);
		//}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	LineFeed																															*
		//*-----------------------------------------------------------------------*
		private bool mLineFeed = true;
		/// <summary>
		/// Get/Set a value indicating whether to add line feeds at the ends of
		/// nodes.
		/// </summary>
		public bool LineFeed
		{
			get { return mLineFeed; }
			set { mLineFeed = value; }
		}
		//*-----------------------------------------------------------------------*

		//	TODO: Fix an inheritance problem between HtmlNodeItem.Nodes and
		//	HtmlDocument.Nodes where this Nodes hides the base property.
		////*-----------------------------------------------------------------------*
		////*	Nodes																																	*
		////*-----------------------------------------------------------------------*
		////private HtmlNodeCollection mNodes = new HtmlNodeCollection();
		//private HtmlNodeCollection mNodes = null;
		///// <summary>
		///// Get a reference to the main Nodes collection for this Document.
		///// </summary>
		//public new HtmlNodeCollection Nodes
		//{
		//	get { return mNodes; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Parse																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parses the supplied HTML String to construct a basic Document.
		/// </summary>
		/// <param name="document">
		/// Instance of an HTML Document to Parse.
		/// </param>
		/// <param name="html">
		/// String containing HTML formatted information.
		/// </param>
		/// <param name="comments">
		/// Value indicating whether to include comments.
		/// </param>
		/// <remarks>
		/// This basic method does not apply most basic rules except the
		/// expectation that the string be well-structured.
		/// </remarks>
		public static void Parse(HtmlDocument document, string html, bool comments)
		{
			document.IncludeComments = comments;
			Parse(document.Nodes, html, comments);
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Iterates the supplied HTML String Collection to construct a basic
		///// Document.
		///// </summary>
		///// <param name="nodes">
		///// Collection of nodes from which the search is starting.
		///// </param>
		///// <param name="elements">
		///// String Collection containing HTML and Text Elements.
		///// </param>
		///// <param name="comments">
		///// Value indicating whether comments will be included.
		///// </param>
		///// <remarks>
		///// This basic method does not apply rules.
		///// </remarks>
		//public static void Parse(HtmlNodeCollection nodes,
		//	StringCollection elements, bool comments)
		//{
		//	string et;          //	Element Type.
		//	bool ic = false;    //	Flag - In Comment.
		//	int lc = 0;         //	List Count.
		//	int lp = 0;         //	List Position.
		//	HtmlNodeItem nn = null; //	New Node.
		//	string s;           //	Working String.
		//	bool sc = false;    //	Singles Closing.
		//	int si = 0;         //	String Index.
		//	int sl = 0;         //	String Length.
		//	string ws = "";     //	Working String.

		//	if(elements != null && nodes != null)
		//	{
		//		//	If here, we have string elements and a nodes collection to start
		//		//	from.
		//		lc = elements.Count;
		//		for(lp = 0; lp < lc; lp++)
		//		{
		//			s = elements[lp].Trim();
		//			if(s.IndexOf("<break") >= 0)
		//			{
		//				Debug.WriteLine("Break Here", "HtmlDocument.Parse(nodes,elements)");
		//			}
		//			if(s.Length > 2 && s.Substring(0, 2) == "<?")
		//			{
		//				//	XML opening node.
		//				nodes.Add(s);
		//			}
		//			else if(s.Length > 3 && s.Substring(0, 3) == "<!-")
		//			{
		//				//	Comment.
		//				nn = nodes.Add(s);
		//				nn.Text = FormatWhitespace(s, comments);
		//			}
		//			else if(s?.Length > 0 && s.Substring(0, 1) == "<")
		//			{
		//				//						//	If this is an opening character, then at this level, we are
		//				//						//	only allowed to go inward.
		//				//						//	Place this item at this level, and the next in that node's
		//				//						//	collection.
		//				//						et = GetElementType(s);
		//				//						nn = nodes.Add(s);
		//				//						if(s.Length > 2 && s.Substring(s.Length - 2, 1) != "/" &&
		//				//							s.Substring(1, 1) != "!" && Singles[et] == null)
		//				//						{
		//				//							//	If this is not a self-closing element, then continue.
		//				//							lp = Parse(nn.Nodes, elements, lp + 1);
		//				//						}
		//				//	We have an opening brace...
		//				//							if(s.Length > 9 && s.Substring(0, 9) == "<input id")
		//				//							{
		//				//								Debug.WriteLine("Break Here");
		//				//							}
		//				et = GetElementType(s);
		//				sc = false; //	By default, this is not a singles closing tag.
		//				if(s.Length > 2 && s.Substring(1, 1) == "/")
		//				{
		//					//	...and a closing flag, then we need to check to see if this
		//					//	item's parent is the parent of this tag.
		//					if(Singles[et] != null)
		//					{
		//						sc = true;
		//					}
		//					else
		//					{
		//						break;
		//					}
		//				}
		//				else if(s.Length >= 4 && s.Substring(0, 4) == "<!--")
		//				{
		//					ws = "-->";
		//					if(s.IndexOf(ws) > 0)
		//					{
		//						//	If the closing side is on this element, then check to
		//						//	see if we have an internal remainder.
		//						si = s.IndexOf(ws);
		//						sl = s.Length - (si + 3);
		//						if(sl > 0)
		//						{
		//							s = s.Substring(si, sl);
		//						}
		//						else
		//						{
		//							s = "";
		//							sc = true;
		//						}
		//					}
		//					else
		//					{
		//						ic = true;
		//					}
		//				}
		//				if(!sc && !ic)
		//				{
		//					//	If not a closing flag, then keep going.
		//					nn = nodes.Add(s);
		//					if(s.Length > 2 && s.Substring(s.Length - 2, 1) != "/" &&
		//						s.Substring(1, 1) != "!" && Singles[et] == null)
		//					{
		//						//	If this is not a self-closing element, then continue
		//						//	inward.
		//						lp = Parse(nn.Nodes, elements, lp + 1, comments);
		//					}
		//				}
		//			}
		//			else
		//			{
		//				// If this is text, then add text on the current item.
		//				nn = nodes.Add("");
		//				nn.Text = FormatWhitespace(s, comments);
		//			}
		//		}
		//	}
		//}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Parse the HTML elements token list into individual nodes.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes to which each item will be added.
		/// </param>
		/// <param name="tokens">
		/// Reference to the collection of string tokens containing the valid HTML
		/// elements.
		/// </param>
		/// <param name="comments">
		/// Value indicating whether to include comments in the output/
		/// </param>
		public static void Parse(HtmlNodeCollection nodes,
			StringTokenCollection tokens, bool comments)
		{
			int count = 0;      //	List Count.
			string element = "";
			string eType;				//	Element Type.
			int index = 0;
			HtmlNodeItem node = null;	//	New Node.
			bool selfClosing = false;	//	Singles/Self Closing.
			string text = "";   //	Working String.
			StringTokenItem token = null;
			int tokenEnd = 0;
			int tokenNextStart = 0;

			if(nodes != null && tokens?.Count > 0)
			{
				//	If here, we have string elements and a nodes collection to start
				//	from.
				count = tokens.Count;
				for(index = 0; index < count; index++)
				{
					token = tokens[index];
					element = token.Value;
					if(element.IndexOf("<break") >= 0)
					{
						Debug.WriteLine("Break Here", "HtmlDocument.Parse(nodes,tokens)");
					}
					if(element.Length > 2 && element.Substring(0, 2) == "<?")
					{
						//	XML opening node.
						nodes.Add(element);
					}
					else if(element.Length > 3 && element.Substring(0, 3) == "<!-")
					{
						//	Comment.
						if(comments)
						{
							node = nodes.Add(element);
							node.Text = FormatWhitespace(element, comments);
						}
					}
					else if(element.Length > 2 && element.Substring(0, 2) == "</")
					{
						//	End-cap for current node.
						//	If this item begins text to the next node, then add it as
						//	a non-node sibling to the parent node.
						if(index + 1 < count)
						{
							tokenEnd = token.StartIndex + token.Length;
							tokenNextStart = tokens[index + 1].StartIndex;
							if(tokenNextStart > tokenEnd)
							{
								text = tokens.Original.Substring(tokenEnd,
									tokenNextStart - tokenEnd);
								if(nodes.ParentNode != null && nodes.ParentNode.Parent != null)
								{
									node = new HtmlNodeItem()
									{
										Text = text
									};
									nodes.ParentNode.Parent.Add(node);
								}
							}
						}
						break;
					}
					else
					{
						//	Normal-looking element.
						eType = GetElementType(element);
						selfClosing = false;
						if(element.Trim().EndsWith("/>") || Singles[eType] != null)
						{
							selfClosing = true;
						}
						node = nodes.Add(element);
						if(element.Trim().EndsWith("/>"))
						{
							//	This node has a self-closing bracket.
							node.SelfClosing = true;
						}
						if(count > index + 1)
						{
							//	Apply any additional text within this node.
							tokenEnd = token.StartIndex + token.Length;
							tokenNextStart = tokens[index + 1].StartIndex;
							if(tokenNextStart > tokenEnd)
							{
								text = tokens.Original.Substring(tokenEnd,
									tokenNextStart - tokenEnd);
								node.Text = text;
							}
						}
						if(!selfClosing)
						{
							//	The node is not self-closing. Create children.
							index = Parse(node.Nodes, tokens, index + 1, comments);
						}
					}
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Parse the HTML elements token list into individual nodes.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes to which each item will be added.
		/// </param>
		/// <param name="tokens">
		/// Reference to the collection of string tokens containing the valid HTML
		/// elements.
		/// </param>
		/// <param name="tokenIndex">
		/// Index of the current token to be parsed in the sequential list.
		/// </param>
		/// <param name="comments">
		/// Value indicating whether to include comments in the output/
		/// </param>
		/// <returns>
		/// Index of the last captured token.
		/// </returns>
		public static int Parse(HtmlNodeCollection nodes,
			StringTokenCollection tokens, int tokenIndex, bool comments)
		{
			int count = 0;      //	List Count.
			string element = "";
			string eType;       //	Element Type.
			int index = 0;
			HtmlNodeItem node = null; //	New Node.
			bool selfClosing = false; //	Singles/Self Closing.
			string text = "";   //	Working String.
			StringTokenItem token = null;
			int tokenEnd = 0;
			int tokenNextStart = 0;

			if(nodes != null && tokens?.Count > 0 &&
				tokenIndex > -1 && tokenIndex < tokens.Count)
			{
				//	If here, we have string elements and a nodes collection to start
				//	from.
				index = tokenIndex;
				count = tokens.Count;
				for(; index < count; index++)
				{
					token = tokens[index];
					element = token.Value;
					if(element.Length > 2 && element.Substring(0, 2) == "<?")
					{
						//	XML opening node.
						nodes.Add(element);
					}
					else if(element.Length > 3 && element.Substring(0, 3) == "<!-")
					{
						//	Comment.
						if(comments)
						{
							node = nodes.Add(element);
							node.Text = FormatWhitespace(element, comments);
						}
					}
					else if(element.Length > 2 && element.Substring(0, 2) == "</")
					{
						//	End-cap for current node.
						//	If this item begins text to the next node, then add it as
						//	a non-node sibling to the parent node.
						if(index + 1 < count)
						{
							tokenEnd = token.StartIndex + token.Length;
							tokenNextStart = tokens[index + 1].StartIndex;
							if(tokenNextStart > tokenEnd)
							{
								text = tokens.Original.Substring(tokenEnd,
									tokenNextStart - tokenEnd);
								if(nodes.ParentNode != null && nodes.ParentNode.Parent != null)
								{
									node = new HtmlNodeItem()
									{
										Text = text
									};
									nodes.ParentNode.Parent.Add(node);
								}
							}
						}
						break;
					}
					else
					{
						//	Normal-looking element.
						eType = GetElementType(element);
						selfClosing = false;
						if(element.Trim().EndsWith("/>") || Singles[eType] != null)
						{
							selfClosing = true;
						}
						node = nodes.Add(element);
						if(element.Trim().EndsWith("/>"))
						{
							//	This node has a self-closing bracket.
							node.SelfClosing = true;
						}
						if(count > index + 1)
						{
							//	Apply any additional text within this node.
							tokenEnd = token.StartIndex + token.Length;
							tokenNextStart = tokens[index + 1].StartIndex;
							if(tokenNextStart > tokenEnd)
							{
								text = tokens.Original.Substring(tokenEnd,
									tokenNextStart - tokenEnd);
								node.Text = text;
							}
						}
						if(!selfClosing)
						{
							//	The node is not self-closing. Create children.
							index = Parse(node.Nodes, tokens, index + 1, comments);
						}
						//	Otherwise, add siblings.
					}
				}
			}
			return index;
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Iterates the supplied HTML String Collection to construct a basic
		///// Document.
		///// </summary>
		///// <param name="nodes">
		///// Collection of nodes from which the search is starting.
		///// </param>
		///// <param name="elements">
		///// String Collection containing HTML and Text Elements.
		///// </param>
		///// <param name="index">
		///// The List Index at which the string parsing will begin.
		///// </param>
		///// <param name="comments">
		///// Value indicating whether comments will be included.
		///// </param>
		///// <returns>
		///// The last Index processed by passes in this level.
		///// </returns>
		///// <remarks>
		///// This basic method does not apply rules.
		///// </remarks>
		//public static int Parse(HtmlNodeCollection nodes,
		//	StringCollection elements, int index, bool comments)
		//{
		//	string et = "";     //	Element Type.
		//	bool ic = false;    //	Flag - In Comment.
		//	int lc;             //	List Count.
		//	int lp = index;     //	List Position.
		//	HtmlNodeItem nn = null;   //	New Node.
		//	string s;           //	Working String.
		//	bool sc = false;    //	Singles Closing.
		//	int si = 0;         //	String Index.
		//	int sl = 0;         //	String Length.
		//	string st = "";			//	Trimmed string.
		//	string ws = "";     //	Working String.

		//	if(elements != null && nodes != null)
		//	{
		//		//	If here, we have string elements and a nodes collection to start
		//		//	from.
		//		lc = elements.Count;
		//		for(lp = index; lp < lc; lp++)
		//		{
		//			//s = elements[lp].Trim();
		//			s = elements[lp];
		//			st = s.Trim();
		//			if(s.IndexOf("<break") >= 0)
		//			{
		//				Debug.WriteLine("Break Here", "HtmlDocument.Parse(nodes,elements,index)");
		//			}
		//			if(s.Length != 0)
		//			{
		//				if(!ic)
		//				{
		//					if(s.Substring(0, 1) == "<")
		//					{
		//						//	We have an opening brace...
		//						//							if(s.Length > 9 && s.Substring(0, 9) == "<input id")
		//						//							{
		//						//								Debug.WriteLine("Break Here");
		//						//							}
		//						et = GetElementType(s);
		//						sc = false; //	By default, this is not a singles closing tag.
		//						if(s.Length > 2 && s.Substring(1, 1) == "/")
		//						{
		//							//	...and a closing flag, then we need to check to see if this
		//							//	item's parent is the parent of this tag.
		//							if(Singles[et] != null)
		//							{
		//								sc = true;
		//							}
		//							else
		//							{
		//								break;
		//							}
		//						}
		//						else if(s.Length >= 4 && s.Substring(0, 4) == "<!--")
		//						{
		//							ws = "-->";
		//							if(s.IndexOf(ws) > 0)
		//							{
		//								//	If the closing side is on this element, then check to
		//								//	see if we have an internal remainder.
		//								si = s.IndexOf(ws);
		//								sl = s.Length - (si + 3);
		//								if(sl > 0)
		//								{
		//									s = s.Substring(si, sl);
		//								}
		//								else
		//								{
		//									if(comments)
		//									{
		//										nn = nodes.Add(s);
		//									}
		//									s = "";
		//									sc = true;
		//								}
		//							}
		//							else
		//							{
		//								ic = true;
		//							}
		//						}
		//						if(!sc && !ic)
		//						{
		//							//	If not a closing flag, then keep going.
		//							nn = nodes.Add(s);
		//							if(st.Length > 2 &&
		//								st.Substring(st.Length - 2, 1) != "/" &&
		//								st.Substring(1, 1) != "!" && Singles[et] == null)
		//							{
		//								//	If this is not a self-closing element, then continue
		//								//	inward.
		//								lp = Parse(nn.Nodes, elements, lp + 1, comments);
		//							}
		//						}
		//					}
		//					else
		//					{
		//						// If this is text, then add text on the current item.
		//						nn = nodes.AddText(s);
		//						nn.Text = FormatWhitespace(s, comments);
		//					}
		//				}
		//				else
		//				{
		//					//	TODO: This block might never be reached.
		//					//	If working in a comment, then we need the closing comment
		//					//	block.
		//					ws = "-->";
		//					if(s.IndexOf(ws) > 0)
		//					{
		//						//	If the closing side is on this element, then check to
		//						//	see if we have an internal remainder.
		//						si = s.IndexOf(ws);
		//						sl = s.Length - (si + 3);
		//						if(si >= 0)
		//						{
		//							ic = false;
		//						}
		//						if(sl > 0)
		//						{
		//							s = s.Substring(si + 3, sl);
		//						}
		//						else
		//						{
		//							s = "";
		//						}
		//						if(!ic && s.Length > 0)
		//						{
		//							//	If we closed the comment with other items, then
		//							//	process again.
		//							nn = nodes.Add(s);
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	return lp;
		//}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Parses the supplied HTML String to construct a basic Document.
		/// </summary>
		/// <param name="nodes">
		/// Collection of nodes from which the search is starting.
		/// </param>
		/// <param name="html">
		/// String containing HTML formatted information.
		/// </param>
		/// <param name="comments">
		/// Value indicating whether comments will be included.
		/// </param>
		/// <remarks>
		/// This basic method does not apply rules.
		/// </remarks>
		public static void Parse(HtmlNodeCollection nodes, string html,
			bool comments)
		{
			//int count = 0;
			//int index = 0;
			StringTokenCollection mc;
			//int nIndex = 0;				//	Next Expected Index.
			//StringCollection sc = new StringCollection(); //	Flat Finds List.
			//string value = "";
			//string ws = "";       //	Working String.

			nodes.ClearAll();
			//ws = Regex.Replace(html, @"([\r\n]+\s*)+", "&crlfs;");
			//			//	Convert to one line.
			//			ws = Regex.Replace(html, @"(?<f>\r\n)", " ");
			//			//	Remove extra spaces.
			//			ws = Regex.Replace(html, @"\s+", " ");
			if(html?.Length != 0 && nodes != null)
			{
				//	Get the list of all closed tags.
				mc = HtmlUtil.GetHtmlElements(html);
				Parse(nodes, mc, comments);
				//foreach(StringTokenItem m in mc)
				//{
				//	if(nIndex < m.StartIndex)
				//	{
				//		//	If the next expected index is less than the actual current
				//		//	index, then this is a text entry.
				//		//	Debug.WriteLine("Text: " + html.Substring(nIndex, m.Index - nIndex), "HtmlDocument.Parse");
				//		value = ws.Substring(nIndex, m.StartIndex - nIndex);
				//		if(value != "&crlfs;")
				//		{
				//			//if(comments)
				//			//{
				//			//	sc.Add(value.Replace("&crlfs;", "\r\n"));
				//			//}
				//			//else
				//			//{
				//			//	sc.Add(value);
				//			//}
				//			sc.Add(value);
				//		}
				//	}
				//	//	Debug.WriteLine("Node: " + m.Value, "HtmlDocument.Parse");
				//	sc.Add(m.Value);
				//	nIndex = m.StartIndex + m.Length;
				//}
				//if(nIndex < ws.Length)
				//{
				//	//	If we have trailing text, then add it beside the last tag.
				//	//	Debug.WriteLine("Tail: " + html.Substring(nIndex, html.Length - nIndex), "HtmlDocument.Parse");
				//	value = ws.Substring(nIndex, ws.Length - nIndex);
				//	if(value != "&crlfs;")
				//	{
				//		sc.Add(value);
				//	}
				//}
				//count = sc.Count;
				//for(index = 0; index < count; index++)
				//{
				//	if(sc[index].IndexOf("&crlfs;") > -1)
				//	{
				//		//sc[index] = sc[index].Replace("&crlfs;", " ");
				//		if(comments)
				//		{
				//			sc[index] = sc[index].Replace("&crlfs;", "\r\n") + "\r\n";
				//		}
				//		else
				//		{
				//			sc[index] = sc[index].Replace("&crlfs;", " ");
				//		}
				//	}
				//}
				//Parse(nodes, sc, comments);
			}
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Parses the supplied HTML String to fill an existing node and its
		///// children.
		///// </summary>
		///// <param name="node">
		///// Instance of a Node from which the search is starting.
		///// </param>
		///// <param name="html">
		///// String containing HTML formatted information.
		///// </param>
		///// <param name="comments">
		///// Value indicating whether comments will be included.
		///// </param>
		///// <remarks>
		///// This basic method does not apply rules.
		///// </remarks>
		//public static void Parse(HtmlNodeItem node, string html, bool comments)
		//{
		//	int count = 0;
		//	string et = ""; //	Element Type.
		//	int index = 0;
		//	MatchCollection mc;	//	Matches.
		//	int nIndex = 0;                         //	Next Expected Index.
		//	string s = "";                          //	Working String.
		//	StringCollection sc = new StringCollection(); //	Flat Finds List.
		//	string value = "";
		//	string ws;                              //	Working String.

		//	node.Attributes.Clear();
		//	node.Nodes.ClearAll();
		//	node.NodeType = "";
		//	node.Text = "";

		//	ws = Regex.Replace(html, @"([\r\n]+\s*)+", "&crlfs;");
		//	if(ws.Length != 0 && node != null)
		//	{
		//		//	Get the list of all closed tags.
		//		mc = Regex.Matches(ws, ResourceMain.rxHtmlTags);
		//		foreach(Match m in mc)
		//		{
		//			if(nIndex < m.Index)
		//			{
		//				//	If the next expected index is less than the actual current
		//				//	index, then this is a text entry.
		//				value = ws.Substring(nIndex, m.Index - nIndex);
		//				if(value != "&crlfs;")
		//				{
		//					sc.Add(value);
		//				}
		//			}
		//			sc.Add(m.Value);
		//			nIndex = m.Index + m.Length;
		//		}
		//		if(nIndex < html.Length)
		//		{
		//			//	If we have trailing text, then add it beside the last tag.
		//			value = ws.Substring(nIndex, ws.Length - nIndex);
		//			if(value != "&crlfs;")
		//			{
		//				sc.Add(value);
		//			}
		//		}
		//		if(sc.Count != 0)
		//		{
		//			count = sc.Count;
		//			for(index = 0; index < count; index++)
		//			{
		//				//sc[index] = sc[index].Replace("&crlfs;", " ");
		//				if(comments)
		//				{
		//					sc[index] = sc[index].Replace("&crlfs;", "\r\n");
		//				}
		//				else
		//				{
		//					sc[index] = sc[index].Replace("&crlfs;", " ");
		//				}
		//			}
		//			//	If we have at least one string, then the first string is destined
		//			//	for the active node.
		//			s = sc[0];
		//			if(s.Substring(0, 1) == "<")
		//			{
		//				//	If this is an opening character, then at this level, we are
		//				//	only allowed to go inward.
		//				//	Place this item at this level, and the next in that node's
		//				//	collection.
		//				et = GetElementType(s);
		//				node.NodeType = et;
		//				AssignAttributes(s, node.Attributes);
		//				if(s.Length > 2 && s.Substring(s.Length - 2, 1) != "/" &&
		//					s.Substring(1, 1) != "!" && Singles[et] == null)
		//				{
		//					//	If this is not a self-closing element, then continue.
		//					sc.RemoveAt(0);
		//				}
		//			}
		//			else
		//			{
		//				// If this is text, then add text on the current item.
		//				sc.RemoveAt(0);
		//				node.Text = FormatWhitespace(s, comments);
		//			}
		//			if(sc.Count != 0)
		//			{
		//				Parse(node.Nodes, sc, comments);
		//			}
		//		}
		//	}
		//}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Parses the supplied HTML String to construct a basic Document.
		/// </summary>
		/// <param name="html">
		/// String containing HTML formatted information.
		/// </param>
		/// <returns>
		/// Populated HTML Document.
		/// </returns>
		/// <param name="comments">
		/// Value indicating whether comments will be included.
		/// </param>
		/// <remarks>
		/// This basic method does not apply most basic rules except the
		/// expectation that the string be well-structured.
		/// </remarks>
		public static HtmlDocument Parse(string html, bool comments)
		{
			HtmlDocument document = new HtmlDocument();
			Parse(document.Nodes, html, comments);
			return document;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Find and remove the node with the specified unique ID.
		/// </summary>
		/// <param name="nodeID">
		/// The unique ID of the node within this document.
		/// </param>
		public void Remove(string nodeID)
		{
			HtmlNodeItem ni = Nodes[nodeID];

			if(ni != null)
			{
				//	If we found the node, then remove it.
				ni.Parent.Remove(ni);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveAll																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Recursively remove all items matching the specified pattern.
		/// </summary>
		/// <param name="match">
		/// Pattern predicate to match.
		/// </param>
		/// <returns>
		/// Count of items to match.
		/// </returns>
		public int RemoveAll(Predicate<HtmlNodeItem> match)
		{
			int result = 0;

			if(match != null)
			{
				result = mNodes.RemoveAll(match);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RemoveAttribute																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all attributes of a specified name within the Document.
		/// </summary>
		/// <param name="document">
		/// Instance of the Document to inspect.
		/// </param>
		/// <param name="name">
		/// Name of the Attribute to Remove.
		/// </param>
		public static void RemoveAttribute(HtmlDocument document, string name)
		{
			if(document != null)
			{
				foreach(HtmlNodeItem ni in document.Nodes)
				{
					HtmlNodeItem.RemoveAttribute(ni, name);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	SelfClosing																														*
		////*-----------------------------------------------------------------------*
		//private bool mSelfClosing = true;
		///// <summary>
		///// Get/Set a value indicating whether the HTML elements are self-closing
		///// if no child elements or text exist on them.
		///// </summary>
		//public bool SelfClosing
		//{
		//	get { return mSelfClosing; }
		//	set { mSelfClosing = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Singles																																*
		//*-----------------------------------------------------------------------*
		private static NameCollection mSingles =
			new NameCollection();
		/// <summary>
		/// Get a reference to a list of Single Elements not requiring a Closing
		/// Tag.
		/// </summary>
		public static NameCollection Singles
		{
			get
			{
				if(mSingles.Count == 0)
				{
					//	If the collection has not yet been initialized, then load it now.
					mSingles.Add("?");
					mSingles.Add("!doctype");
					mSingles.Add("base");
					mSingles.Add("basefont");
					mSingles.Add("bgsound");
					mSingles.Add("br");
					mSingles.Add("col");
					mSingles.Add("embed");
					mSingles.Add("frame");
					mSingles.Add("hr");
					mSingles.Add("img");
					mSingles.Add("input");
					mSingles.Add("link");
					mSingles.Add("meta");
					mSingles.Add("param");
					mSingles.Add("wbr");
				}
				return mSingles;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this Document Object Model.
		/// </summary>
		/// <returns>
		/// The HTML formatted contents of this instance.
		/// </returns>
		public override string ToString()
		{
			return Html;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
