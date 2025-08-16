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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlNodeCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of HtmlNodeItem Items.
	/// </summary>
	public class HtmlNodeCollection : List<HtmlNodeItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from anywhere in the structure by its ID.
		/// </summary>
		public HtmlNodeItem this[string id]
		{
			get
			{
				HtmlNodeItem ro = null;

				foreach(HtmlNodeItem ni in this)
				{
					if(ni.Id == id)
					{
						ro = ni;
						break;
					}
					else
					{
						ro = ni.Nodes[id];
						if(ro != null)
						{
							//	If we found the item somewhere inside, then exit now.
							break;
						}
					}
				}
				return ro;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new HtmlNodeItem, add it to the Collection, and return it to
		/// the caller.
		/// </summary>
		/// <returns>
		/// Newly created and added HtmlNodeItem.
		/// </returns>
		public HtmlNodeItem Add()
		{
			HtmlNodeItem ro = new HtmlNodeItem()
			{
				Index = this.Count,
				Parent = this
			};
			if(this.ParentNode != null)
			{
				this.ParentNode.SelfClosing = false;
			}

			base.Add(ro);
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add an existing HtmlNodeItem to the Collection.
		/// </summary>
		/// <param name="value">
		/// The HtmlNodeItem Item to add to the Collection.
		/// </param>
		public new void Add(HtmlNodeItem value)
		{
			if(value != null)
			{
				if(value.Parent == null)
				{
					value.Index = this.Count;
					value.Parent = this;
				}
				if(this.ParentNode != null)
				{
					this.ParentNode.SelfClosing = false;
				}

				base.Add(value);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new HtmlNodeItem to the collection by its node type and text.
		/// </summary>
		/// <param name="nodeType">
		/// The node type to add.
		/// </param>
		/// <param name="text">
		/// The node text to add.
		/// </param>
		/// <returns>
		/// Reference to a newly created and added HtmlNodeItem, if a valid
		/// nodeType was provided. Otherwise, reference to a newly created div
		/// element.
		/// </returns>
		public HtmlNodeItem Add(string nodeType, string text)
		{
			HtmlNodeItem node = null;

			if(nodeType?.Length > 0)
			{
				node = new HtmlNodeItem(nodeType, text);
			}
			if(node == null)
			{
				node = new HtmlNodeItem("div");
			}
			if(this.ParentNode != null)
			{
				this.ParentNode.SelfClosing = false;
			}

			this.Add(node);
			return node;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new HtmlNodeItem to the collection by its content.
		/// </summary>
		/// <param name="value">
		/// Element formatted content.
		/// </param>
		/// <returns>
		/// Reference to the newly created and added node.
		/// </returns>
		public HtmlNodeItem Add(string value)
		{
			//			MatchCollection mc;											//	Matches.
			//			//	Case insensitive:
			//			//	id
			//			//	followed by zero or more spaces
			//			//	followed by =
			//			//	followed by zero or more spaces
			//			//	capture result for one word
			//			string mpID = "(?i:id\\s*?=\\s*?(?<result>\\w*))";					//	ID Match.
			//			//	Case insensitive:
			//			//	name
			//			//	followed by zero or more spaces
			//			//	followed by =
			//			//	followed by zero or more spaces
			//			//	followed by "
			//			//	capture result for one word
			//			//	followed by "
			//			string mpName = "(?i:name\\s*?=\\s*?\"(?<result>\\w*)\")";	//	Name Match.
			HtmlNodeItem ro = this.Add();
			string working = "";

			//ro.Index = this.Count;

			if(value?.Length > 0)
			{
				if(value.IndexOfAny(new char[] { '<', '>' }) == -1)
				{
					//	The caller is adding a generic item by its tag name only.
					if(Conversion.IsSelfClosingTag(value))
					{
						working = $"<{value} />";
					}
					else
					{
						working = $"<{value}>";
					}
					ro.Original = working;
					ro.NodeType = value;
				}
				else
				{
					//	The caller is adding an HTML element.
					working = value;
					ro.Original = value;
				}
			}

			if(working.Length != 0)
			{
				ro.NodeType = HtmlDocument.GetElementType(working);
				if(ro.NodeType != "!--")
				{
					HtmlDocument.AssignAttributes(working, ro.Attributes,
						GetPreserveSpace(this));
				}
			}
			if(this.ParentNode != null)
			{
				this.ParentNode.SelfClosing = false;
			}

			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new HtmlNodeItem to the Collection by its content.
		/// </summary>
		/// <param name="value">
		/// Element formatted content.
		/// </param>
		/// <param name="parse">
		/// Value indicating whether or not to parse multiple nodes in value.
		/// </param>
		/// <returns>
		/// Reference to the newly created and added node.
		/// </returns>
		public HtmlNodeItem Add(string value, bool parse)
		{
			//			MatchCollection mc;											//	Matches.
			//			//	Case insensitive:
			//			//	id
			//			//	followed by zero or more spaces
			//			//	followed by =
			//			//	followed by zero or more spaces
			//			//	capture result for one word
			//			string mpID = "(?i:id\\s*?=\\s*?(?<result>\\w*))";					//	ID Match.
			//			//	Case insensitive:
			//			//	name
			//			//	followed by zero or more spaces
			//			//	followed by =
			//			//	followed by zero or more spaces
			//			//	followed by "
			//			//	capture result for one word
			//			//	followed by "
			//			string mpName = "(?i:name\\s*?=\\s*?\"(?<result>\\w*)\")";	//	Name Match.
			HtmlNodeItem ro = this.Add();

			ro.Index = this.Count;

			if(value.Length != 0)
			{
				if(parse)
				{
					ro.Nodes.Html = value;
				}
				else
				{
					ro.Original = value;
					ro.NodeType = HtmlDocument.GetElementType(value);
					HtmlDocument.AssignAttributes(value, ro.Attributes,
						GetPreserveSpace(this));
				}
			}
			if(this.ParentNode != null)
			{
				this.ParentNode.SelfClosing = false;
			}


			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a series of HTML nodes to this list.
		/// </summary>
		/// <param name="collection">
		/// Reference to the collection of items to add.
		/// </param>
		public new void AddRange(IEnumerable<HtmlNodeItem> collection)
		{
			if(collection?.Count() > 0)
			{
				foreach(HtmlNodeItem nodeItem in collection)
				{
					this.Add(nodeItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddText																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add basic text as a node.
		/// </summary>
		/// <param name="value">
		/// Node text to add.
		/// </param>
		/// <returns>
		/// Reference to the node containing inner text.
		/// </returns>
		public HtmlNodeItem AddText(string value)
		{
			HtmlNodeItem ro = this.Add();
			string working = "";

			//	The caller is adding an HTML element.
			working = value;
			ro.Original = value;

			if(working.Length != 0)
			{
				ro.NodeType = "";
				ro.Text = value;
			}
			if(this.ParentNode != null)
			{
				this.ParentNode.SelfClosing = false;
			}


			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AppendMatches																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Append items matching the specified pattern to the list of items.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the colleciton of nodes being searched.
		/// </param>
		/// <param name="targetItems">
		/// Reference to the collection of target items.
		/// </param>
		/// <param name="match">
		/// Reference to the function pattern to match.
		/// </param>
		public static void AppendMatches(
			List<HtmlNodeItem> nodes,
			List<HtmlAttributeItem> targetItems,
			Func<HtmlAttributeItem, bool> match)
		{
			if(nodes?.Count > 0 && targetItems != null && match != null)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					foreach(HtmlAttributeItem attributeItem in nodeItem.Attributes)
					{
						if(match.Invoke(attributeItem) &&
							!targetItems.Contains(attributeItem))
						{
							targetItems.Add(attributeItem);
						}
					}
					AppendMatches(nodeItem.Nodes, targetItems, match);
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Append items matching the specified pattern to the list of items.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of source nodes to search.
		/// </param>
		/// <param name="targetItems">
		/// Reference to the collection of target items.
		/// </param>
		/// <param name="match">
		/// Reference to the function pattern to match.
		/// </param>
		public static void AppendMatches(
			List<HtmlNodeItem> nodes,
			List<HtmlNodeItem> targetItems,
			Func<HtmlNodeItem, bool> match)
		{
			if(nodes?.Count > 0 && targetItems != null && match != null)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					if(match.Invoke(nodeItem) &&
						!targetItems.Contains(nodeItem))
					{
						targetItems.Add(nodeItem);
					}
					AppendMatches(nodeItem.Nodes, targetItems, match);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ClearAll																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear all Nodes in all Levels of this Collection.
		/// </summary>
		public void ClearAll()
		{
			foreach(HtmlNodeItem ni in this)
			{
				ni.Nodes.ClearAll();
			}
			this.Clear();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ContentEmpty																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether there is any printable content within
		/// the Node or its children.
		/// </summary>
		/// <param name="value">
		/// Value to inspect.
		/// </param>
		/// <returns>
		/// True if no printable content exists in the node.
		/// </returns>
		public static bool ContentEmpty(HtmlNodeCollection value)
		{
			bool rv = true;

			if(value != null)
			{
				//	Value exists.
				foreach(HtmlNodeItem ni in value)
				{
					if(!HtmlNodeItem.ContentEmpty(ni))
					{
						rv = false;
						break;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Document																															*
		//*-----------------------------------------------------------------------*
		private HtmlDocument mDocument = null;
		/// <summary>
		/// Get/Set a reference to the HTML document controlling this collection.
		/// </summary>
		public HtmlDocument Document
		{
			get { return mDocument; }
			set { mDocument = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindMatch																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first item matching the specified pattern.
		/// </summary>
		/// <param name="match">
		/// Reference to the function pattern to match.
		/// </param>
		/// <param name="recurse">
		/// Value indicating whether to return match from any level.
		/// </param>
		/// <returns>
		/// Reference to the specified item, if found. Otherwise, null.
		/// </returns>
		public HtmlNodeItem FindMatch(Func<HtmlNodeItem, bool> match,
			bool recurse = true)
		{
			HtmlNodeItem result = null;

			if(match != null)
			{
				foreach(HtmlNodeItem nodeItem in this)
				{
					if(match.Invoke(nodeItem))
					{
						result = nodeItem;
					}
					else if(recurse)
					{
						result = nodeItem.Nodes.FindMatch(match, recurse);
					}
					if(result != null)
					{
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindMatches																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list of items matching the specified pattern.
		/// </summary>
		/// <param name="match">
		/// Reference to the function pattern to match.
		/// </param>
		/// <param name="recurse">
		/// Value indicating whether to return matches from all levels.
		/// </param>
		/// <returns>
		/// Reference to a list of matching items.
		/// </returns>
		public List<HtmlNodeItem> FindMatches(Func<HtmlNodeItem, bool> match,
			bool recurse = true)
		{
			List<HtmlNodeItem> result = new List<HtmlNodeItem>();

			if(match != null)
			{
				foreach(HtmlNodeItem nodeItem in this)
				{
					if(match.Invoke(nodeItem))
					{
						result.Add(nodeItem);
					}
					AppendMatches(nodeItem.Nodes, result, match);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FindMatchingAttributes																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list of attributes from the nodes in the caller's collection
		/// that match the specified value.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes to be searched.
		/// </param>
		/// <param name="match">
		/// Reference to the matching predicate.
		/// </param>
		/// <param name="recurse">
		/// Optional value indicating whether to recurse in levels. Default =
		/// true.
		/// </param>
		/// <returns>
		/// Reference to a list of attributes, if any matches were found.
		/// Otherwise, an empty list.
		/// </returns>
		public static List<HtmlAttributeItem> FindMatchingAttributes(
			HtmlNodeCollection nodes,
			Func<HtmlAttributeItem, bool> match, bool recurse = true)
		{
			List<HtmlAttributeItem> attributes = new List<HtmlAttributeItem>();

			if(nodes?.Count > 0)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					foreach(HtmlAttributeItem attributeItem in nodeItem.Attributes)
					{
						if(match.Invoke(attributeItem))
						{
							attributes.Add(attributeItem);
						}
					}
					if(recurse)
					{
						AppendMatches(nodeItem.Nodes, attributes, match);
					}
				}
			}
			return attributes;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDocument																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the HTML document to which this collection is
		/// associated.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes to test.
		/// </param>
		/// <returns>
		/// Reference to the root document to which this collection is associated.
		/// </returns>
		public static HtmlDocument GetDocument(HtmlNodeCollection nodes)
		{
			HtmlDocument result = null;

			if(nodes != null)
			{
				if(nodes.mParentNode != null)
				{
					if(nodes.mParentNode is HtmlDocument document)
					{
						result = document;
					}
					else
					{
						result = GetDocument(nodes.mParentNode.Parent);
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetIncludeComments																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether comments are enabled for this
		/// document.
		/// </summary>
		/// <param name="nodes">
		/// Reference to a collection of nodes to test.
		/// </param>
		/// <returns>
		/// Value indicating whether comments are enabled on this document.
		/// </returns>
		public static bool GetIncludeComments(HtmlNodeCollection nodes)
		{
			HtmlDocument document = null;
			bool result = true;

			if(nodes != null)
			{
				document = GetDocument(nodes);
				if(document != null)
				{
					result = document.IncludeComments;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetLastNodeOfType																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the last node in the hierarchy of the specified type.
		/// </summary>
		/// <param name="nodes">
		/// Collection of nodes to inspect.
		/// </param>
		/// <param name="nodeType">
		/// Type of node to search for.
		/// </param>
		/// <returns>
		/// The last node in the heirarchy of the specified type, if found.
		/// Otherwise, null.
		/// </returns>
		public static HtmlNodeItem GetLastNodeOfType(HtmlNodeCollection nodes,
			string nodeType)
		{
			HtmlNodeItem ro = null;
			HtmlNodeItem wn;

			foreach(HtmlNodeItem ni in nodes)
			{
				if(ni.NodeType == nodeType)
				{
					//	We found a node of that type at this level.
					ro = ni;
				}
				wn = GetLastNodeOfType(ni.Nodes, nodeType);
				if(wn != null)
				{
					ro = wn;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetLineFeed																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether HTML elements are terminated with
		/// line feeds.
		/// </summary>
		/// <param name="nodes">
		/// Reference to a collection of nodes to test.
		/// </param>
		/// <returns>
		/// Value indicating whether elements are separated by line on this
		/// document.
		/// </returns>
		public static bool GetLineFeed(HtmlNodeCollection nodes)
		{
			HtmlDocument document = null;
			bool result = true;

			if(nodes != null)
			{
				document = GetDocument(nodes);
				if(document != null)
				{
					result = document.LineFeed;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNode																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first node found of a given node type.
		/// </summary>
		/// <param name="nodes">
		/// Collection of nodes to inspect.
		/// </param>
		/// <param name="nodeType">
		/// Node Type to find.
		/// </param>
		/// <returns>
		/// First node of the specified type, if found. Null otherwise.
		/// </returns>
		public static HtmlNodeItem GetNode(HtmlNodeCollection nodes,
			string nodeType)
		{
			string nt = nodeType;
			HtmlNodeItem ro = null;

			if(nodes != null)
			{
				foreach(HtmlNodeItem ni in nodes)
				{
					if(ni.NodeType == nt)
					{
						ro = ni;
						break;
					}
					ro = GetNode(ni.Nodes, nt);
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
		/// <param name="nodes">
		/// Collection of nodes to inspect.
		/// </param>
		/// <param name="nodeType">
		/// Node type to find.
		/// </param>
		/// <returns>
		/// Collection of nodes of the specified type, if found.
		/// Zero Length Array otherwise.
		/// </returns>
		public static HtmlNodeItem[] GetNodes(HtmlNodeCollection nodes,
			string nodeType)
		{
			int lp = 0;
			string nt = nodeType;
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem[] ro = new HtmlNodeItem[0];

			if(nodes != null)
			{
				foreach(HtmlNodeItem ni in nodes)
				{
					if(ni.NodeType == nt)
					{
						oc.Add(ni);
					}
					oc.AddRange(GetNodes(ni.Nodes, nt));
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
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the all nodes found of a given node type.
		/// </summary>
		/// <param name="nodes">
		/// Collection of nodes to inspect.
		/// </param>
		/// <param name="nodeTypes">
		/// Array of node types to find.
		/// </param>
		/// <returns>
		/// Collection of nodes of the specified type, if found.
		/// Zero Length Array otherwise.
		/// </returns>
		public static HtmlNodeItem[] GetNodes(HtmlNodeCollection nodes,
			string[] nodeTypes)
		{
			int lp = 0;
			string nt = "";
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem[] ro = new HtmlNodeItem[0];

			if(nodes != null && nodeTypes?.Length > 0)
			{
				foreach(HtmlNodeItem ni in nodes)
				{
					foreach(string nodeType in nodeTypes)
					{
						nt = nodeType;
						if(ni.NodeType == nt)
						{
							oc.Add(ni);
						}
						oc.AddRange(GetNodes(ni.Nodes, nt));
					}
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
		/// <param name="nodes">
		/// Collection of Nodes in which to search.
		/// </param>
		/// <param name="attributeName">
		/// Name of the attribute to search for.
		/// </param>
		/// <returns>
		/// Array of Nodes containing the specified Attribute, if found. Otherwise,
		/// a zero length array.
		/// </returns>
		public static HtmlNodeItem[] GetNodesWithAttribute(
			HtmlNodeCollection nodes, string attributeName)
		{
			int lp = 0;
			HtmlNodeItem[] na = new HtmlNodeItem[0];
			ObjectCollection oc = new ObjectCollection();

			foreach(HtmlNodeItem ni in nodes)
			{
				if(ni.Attributes[attributeName] != null)
				{
					oc.Add(ni);
				}
				oc.AddRange(GetNodesWithAttribute(ni.Nodes, attributeName));
			}
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
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return an array of nodes having the specified Attribute.
		/// </summary>
		/// <param name="node">
		/// Node in which search will begin.
		/// </param>
		/// <param name="attributeName">
		/// Name of the attribute to search for.
		/// </param>
		/// <returns>
		/// Array of Nodes containing the specified Attribute, if found. Otherwise,
		/// a zero length array.
		/// </returns>
		public static HtmlNodeItem[] GetNodesWithAttribute(
			HtmlNodeItem node, string attributeName)
		{
			int lp = 0;
			HtmlNodeItem[] na = new HtmlNodeItem[0];
			ObjectCollection oc = new ObjectCollection();

			if(node != null)
			{
				if(node.Attributes[attributeName] != null)
				{
					oc.Add(node);
				}
				oc.AddRange(GetNodesWithAttribute(node.Nodes, attributeName));
				if(oc.Count != 0)
				{
					na = new HtmlNodeItem[oc.Count];
					foreach(HtmlNodeItem ni in oc)
					{
						na[lp++] = ni;
					}
				}
			}
			return na;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPreserveSpace																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether whitespace is preserved everywhere in
		/// the document.
		/// </summary>
		/// <param name="nodes">
		/// Reference to a collection of nodes to test.
		/// </param>
		/// <returns>
		/// Value indicating whether elements are separated by line on this
		/// document.
		/// </returns>
		public static bool GetPreserveSpace(HtmlNodeCollection nodes)
		{
			HtmlDocument document = null;
			bool result = true;

			if(nodes != null)
			{
				document = GetDocument(nodes);
				if(document != null)
				{
					result = document.PreserveSpace;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRoot																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the root-most node collection of the specified collection.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection for which a root reference will be found.
		/// </param>
		/// <returns>
		/// Reference to the root-most collection available in the heirarchy, if
		/// found. Otherwise, null;
		/// </returns>
		public static HtmlNodeCollection GetRoot(HtmlNodeCollection nodes)
		{
			HtmlNodeCollection result = null;

			if(nodes != null)
			{
				if(nodes.ParentNode != null && nodes.ParentNode.Parent != null)
				{
					result = GetRoot(nodes.ParentNode.Parent);
					if(result == null)
					{
						result = nodes.ParentNode.Parent;
					}
				}
				else
				{
					result = nodes;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Html																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the HTML Content of this Nodes Collection.
		/// </summary>
		public string Html
		{
			get
			{
				bool bFeed = GetLineFeed(this);
				//bool bQuoted = true;
				int index = 0;
				bool lineFeedSeparation = bFeed;
				bool preserveSpace = GetPreserveSpace(this);
				StringBuilder builder = new StringBuilder();

				foreach(HtmlNodeItem nodeItem in this)
				{
					bFeed = lineFeedSeparation;
					if(bFeed)
					{
						if(index + 1 < this.Count)
						{
							if(this[index + 1].NodeType.Length == 0 &&
								this[index + 1].Text.Trim().Length > 0)
							{
								bFeed = false;
							}
						}
					}
					HtmlUtil.AppendNodeHtml(nodeItem, builder, preserveSpace, bFeed);
					index++;
				}
				return builder.ToString();
			}
			set { HtmlDocument.Parse(this, value,
				GetIncludeComments(this), GetPreserveSpace(this)); }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	IncludeComments																												*
		////*-----------------------------------------------------------------------*
		//private bool mIncludeComments = true;
		///// <summary>
		///// Get/Set a value indicating whether to include comments.
		///// </summary>
		//public bool IncludeComments
		//{
		//	get { return mIncludeComments; }
		//	set { mIncludeComments = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Insert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new node prior to the specified Node in this collection.
		/// </summary>
		/// <param name="beforeNode">
		/// The Node prior to which a new node will be inserted.
		/// </param>
		/// <returns>
		/// Newly created and added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public HtmlNodeItem Insert(HtmlNodeItem beforeNode)
		{
			int lp = 0;                   //	List Position.
			HtmlNodeItem nn = null;       //	New Node.

			if(beforeNode != null)
			{
				//	Check first to make sure the insertion point exists.
				lp = 0;
				foreach(HtmlNodeItem ni in this)
				{
					if(ni.Equals(beforeNode))
					{
						nn = new HtmlNodeItem()
						{
							Parent = this
						};
						this.Insert(lp, nn);
						break;
					}
					else
					{
						nn = ni.Nodes.Insert(beforeNode);
						if(nn != null)
						{
							break;
						}
					}
					lp++;
				}
			}
			return nn;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert a new node prior to the specified Node in this collection.
		/// </summary>
		/// <param name="beforeNode">
		/// The Node prior to which a new node will be inserted.
		/// </param>
		/// <param name="html">
		/// Html to insert.
		/// </param>
		/// <returns>
		/// Newly created and added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public HtmlNodeItem Insert(HtmlNodeItem beforeNode, string html)
		{
			HtmlNodeCollection fc = null; //	Found Collection.
			int lp = 0;                   //	List Position.
			HtmlNodeItem nn = null;       //	New Node.

			if(beforeNode != null)
			{
				//	Check first to make sure the insertion point exists.
				lp = 0;
				foreach(HtmlNodeItem ni in this)
				{
					if(ni.Equals(beforeNode))
					{
						fc = ni.Parent;
						nn = fc.Insert(ni);
						HtmlDocument.Parse(nn.Nodes, html,
							GetIncludeComments(this), GetPreserveSpace(this));
						break;
					}
					else
					{
						nn = ni.Nodes.Insert(beforeNode);
						if(nn != null)
						{
							break;
						}
					}
					lp++;
				}
			}
			return nn;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert a new node prior to the specified Node in this collection.
		/// </summary>
		/// <param name="beforeIndex">
		/// The index of the node prior to which a new node will be inserted.
		/// </param>
		/// <param name="html">
		/// Html to insert.
		/// </param>
		/// <param name="parse">
		/// Value indicating whether to parse the text (true), or to just insert
		/// it as-is (false).
		/// </param>
		/// <returns>
		/// Newly created and added Node.
		/// </returns>
		public HtmlNodeItem Insert(int beforeIndex, string html, bool parse = true)
		{
			int count = this.Count;
			int index = 0;
			HtmlNodeItem ro = new HtmlNodeItem()
			{
				Parent = this
			};

			//	Make the position safe.
			if(beforeIndex < 0)
			{
				index = 0;
			}
			else if(beforeIndex >= count)
			{
				index = count;
			}
			this.Insert(index, ro);
			count = this.Count;

			if(html?.Length > 0)
			{
				if(parse)
				{
					ro.Nodes.Html = html;
				}
				else
				{
					ro.Original = html;
					ro.NodeType = HtmlDocument.GetElementType(html);
					HtmlDocument.AssignAttributes(html, ro.Attributes,
						GetPreserveSpace(this));
				}
			}
			//	Refresh item index.
			for(index ++; index < count; index ++)
			{
				this[index].Index = index;
			}

			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert the provided HTML before the specified Node in the appropriate
		/// level.
		/// </summary>
		/// <param name="beforeID">
		/// The Unique ID to find within the document.
		/// </param>
		/// <param name="html">
		/// HTML Content to Insert.
		/// </param>
		/// <returns>
		/// Html Node Object constructed from caller's HTML.
		/// </returns>
		public HtmlNodeItem Insert(string beforeID, string html)
		{
			HtmlNodeCollection fc = null; //	Found Collection.
			HtmlNodeItem fn = null;       //	Found Node.
			HtmlNodeItem nn = null;       //	New Node.

			if(beforeID.Length != 0 & html.Length != 0)
			{
				fn = this[beforeID];
				if(fn != null)
				{
					//	If we found the node, then continue.
					fc = fn.Parent;
					nn = fc.Insert(fn);
					HtmlDocument.Parse(nn.Nodes, html,
						GetIncludeComments(this), GetPreserveSpace(this));
				}
				else
				{
					foreach(HtmlNodeItem ni in this)
					{
						nn = ni.Nodes.Insert(beforeID, html);
						if(nn != null)
						{
							break;
						}
					}
				}
			}
			return nn;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InsertAfter																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a new node after the specified Node in this collection.
		/// </summary>
		/// <param name="afterNode">
		/// The Node after which a new node will be inserted.
		/// </param>
		/// <returns>
		/// Newly created and added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public HtmlNodeItem InsertAfter(HtmlNodeItem afterNode)
		{
			int lp = 0;                   //	List Position.
			HtmlNodeItem nn = null;       //	New Node.

			if(afterNode != null)
			{
				//	Check first to make sure the insertion point exists.
				lp = 0;
				foreach(HtmlNodeItem ni in this)
				{
					if(ni.Equals(afterNode))
					{
						nn = new HtmlNodeItem()
						{
							Parent = this
						};
						this.Insert(lp + 1, nn);
						break;
					}
					else
					{
						nn = ni.Nodes.InsertAfter(afterNode);
						if(nn != null)
						{
							break;
						}
					}
					lp++;
				}
			}
			return nn;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert a new node after the specified Node in this collection.
		/// </summary>
		/// <param name="afterNode">
		/// The Node after which a new node will be inserted.
		/// </param>
		/// <param name="value">
		/// Node to insert.
		/// </param>
		/// <returns>
		/// Newly added Node, if the insertion point was found. Null
		/// otherwise.
		/// </returns>
		public HtmlNodeItem InsertAfter(HtmlNodeItem afterNode, HtmlNodeItem value)
		{
			int lp = 0;                   //	List Position.
			HtmlNodeItem nn = null;       //	New Node.

			if(afterNode != null)
			{
				//	Check first to make sure the insertion point exists.
				lp = 0;
				foreach(HtmlNodeItem ni in this)
				{
					if(ni.Equals(afterNode))
					{
						this.Insert(lp + 1, value);
						nn = value;
						break;
					}
					else
					{
						nn = ni.Nodes.InsertAfter(afterNode, value);
						if(nn != null)
						{
							break;
						}
					}
					lp++;
				}
			}
			return nn;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ParentNode																														*
		//*-----------------------------------------------------------------------*
		private HtmlNodeItem mParentNode = null;
		/// <summary>
		/// Get/Set the Parent Node hosting this Collection.
		/// </summary>
		public HtmlNodeItem ParentNode
		{
			get { return mParentNode; }
			set { mParentNode = value; }
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
		public new int RemoveAll(Predicate<HtmlNodeItem> match)
		{
			int result = 0;

			if(match != null)
			{
				result = base.RemoveAll(match);
				foreach(HtmlNodeItem nodeItem in this)
				{
					result += nodeItem.Nodes.RemoveAll(match);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResetParent																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Reset the parent property on the child nodes and their descendants.
		/// </summary>
		/// <param name="nodes">
		/// Nodes at which to being reset.
		/// </param>
		public static void ResetParent(HtmlNodeCollection nodes)
		{
			int index = 0;
			if(nodes?.Count > 0)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					nodeItem.Index = index++;
					nodeItem.Parent = nodes;
					ResetParent(nodeItem.Nodes);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Trim																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Trim blank text from elements in the specified collection and its
		/// descendants.
		/// </summary>
		/// <param name="nodes">
		/// Collection of nodes whose text will be trimmed.
		/// </param>
		public static void Trim(HtmlNodeCollection nodes)
		{
			if(nodes?.Count > 0)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					if(nodeItem.Text.Length > 0)
					{
						nodeItem.Text = nodeItem.Text.Trim();
					}
					Trim(nodeItem.Nodes);
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	HtmlNodeItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A Single HTML Node.
	/// </summary>
	public class HtmlNodeItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
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
		/// Create a new instance of the HtmlNodeItem item.
		/// </summary>
		public HtmlNodeItem()
		{
			mNodes.ParentNode = this;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the HtmlNodeItem item.
		/// </summary>
		/// <param name="nodeType">
		/// The type of node to create.
		/// </param>
		public HtmlNodeItem(string nodeType) : this()
		{
			mNodeType = nodeType;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the HtmlNodeItem item.
		/// </summary>
		/// <param name="nodeType">
		/// The type of node to create.
		/// </param>
		/// <param name="text">
		/// Text to assign to the node.
		/// </param>
		public HtmlNodeItem(string nodeType, string text) : this()
		{
			mNodeType = nodeType;
			if(text?.Length > 0)
			{
				mText = text;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from the Attributes collection by its Name.
		/// </summary>
		public HtmlAttributeItem this[string value]
		{
			get { return mAttributes[value]; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AbsoluteIndex																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the absolute index of this item in the hierarchy.
		/// </summary>
		public int AbsoluteIndex
		{
			get
			{
				int rv = Index;

				if(ParentNode != null)
				{
					rv++;       //	If there is a parent, then this is at least one
											//	further down the list from there.
					rv += ParentNode.AbsoluteIndex;
				}
				return rv;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Attributes																														*
		//*-----------------------------------------------------------------------*
		private HtmlAttributeCollection mAttributes =
			new HtmlAttributeCollection();
		/// <summary>
		/// Get a reference to the Collection of Attributes on this Node.
		/// </summary>
		public HtmlAttributeCollection Attributes
		{
			get { return mAttributes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ClosingTag																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the Closing Tag of this Element.
		/// </summary>
		public string ClosingTag
		{
			get
			{
				string rs = "";

				if(NodeType.Length != 0 &&
					!HtmlUtil.Singles.Exists(x => x.ToLower() == NodeType.ToLower()))
				{
					//	If this is an element, and not a single, then get the
					//	closing tag.
					rs = "</" + NodeType + ">";
				}
				return rs;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ContentEmpty																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether there is any printable content within
		/// the Node or its children.
		/// </summary>
		/// <param name="value">
		/// Value to inspect.
		/// </param>
		/// <returns>
		/// True if no printable content exists in the node.
		/// </returns>
		public static bool ContentEmpty(HtmlNodeItem value)
		{
			bool rv = true;

			if(value != null)
			{
				//	Value exists.
				if(value.Text.Replace("\r", "").Replace("\n", "").Length != 0)
				{
					rv = false;
				}
				if(rv)
				{
					rv &= HtmlNodeCollection.ContentEmpty(value.Nodes);
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Copy																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create an exact copy of the specified Node and its hierarchy.
		/// </summary>
		/// <param name="value">
		/// Node to copy.
		/// </param>
		/// <returns>
		/// Newly created Node.
		/// </returns>
		public static HtmlNodeItem Copy(HtmlNodeItem value)
		{
			HtmlNodeItem ro = new HtmlNodeItem();

			if(value != null)
			{
				foreach(HtmlAttributeItem ai in value.Attributes)
				{
					ro.Attributes.Add(ai.Name, ai.Value);
				}
				foreach(NameItem ni in value.Comments)
				{
					ro.Comments.Add(ni.Name, ni.Description);
				}
				ro.NodeType = value.NodeType;
				ro.Original = value.Original;
				ro.Text = value.Text;
				//	NOTE: This was added to maintain original node Parent Collection...
				//	In collection iteration below, the Parent Property will be
				//	overwritten with the appropriate value.
				ro.Parent = value.Parent;
				foreach(HtmlNodeItem ni in value.Nodes)
				{
					ro.Nodes.Add(Copy(ni));
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Comments																															*
		//*-----------------------------------------------------------------------*
		private NameCollection mComments = new NameCollection();
		/// <summary>
		/// Get a reference to the Comments collection for this Node.
		/// </summary>
		public NameCollection Comments
		{
			get { return mComments; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CopyContent																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Copy the content from the source node to the target node.
		/// </summary>
		/// <param name="source">
		/// Node containing content to be copied.
		/// </param>
		/// <param name="target">
		/// Instance of a Node prepared to receive content.
		/// </param>
		/// <remarks>
		/// The previous content of the Target node will be overwritten.
		/// </remarks>
		public static void CopyContent(HtmlNodeItem source, HtmlNodeItem target)
		{
			if(source != null && target != null)
			{
				target.Nodes.Clear();
				target.Attributes.Clear();
				target.Text = source.Text;
				target.NodeType = source.NodeType;
				target.Original = source.Original;

				foreach(HtmlAttributeItem ha in source.Attributes)
				{
					target.Attributes.Add(ha.Name, ha.Value);
				}
				target.Text = source.Text;
				foreach(HtmlNodeItem ni in source.Nodes)
				{

					CopyContent(ni, target.Nodes.Add());
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FindAttribute																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return Nodes by Name, ID, or Value content.
		/// </summary>
		/// <param name="baseNode">
		/// Base node at which to begin the search.
		/// </param>
		/// <param name="expression">
		/// Regular expression used to find the text.
		/// </param>
		/// <returns>
		/// Array of Nodes matching the Expression, if found. Otherwise, zero
		/// length array.
		/// </returns>
		public static HtmlNodeItem[] FindAttribute(HtmlNodeItem baseNode,
			string expression)
		{
			return FindAttribute(baseNode, expression,
				new string[] { "name", "id", "value" });
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return Nodes by specified Attributes.
		/// </summary>
		/// <param name="baseNode">
		/// Base node at which to begin the search.
		/// </param>
		/// <param name="expression">
		/// Regular expression used to find the text.
		/// </param>
		/// <param name="attributeNames">
		/// Array of Attribute Names to search for.
		/// </param>
		/// <returns>
		/// Array of Nodes matching the Expression, if found. Otherwise, zero
		/// length array.
		/// </returns>
		public static HtmlNodeItem[] FindAttribute(HtmlNodeItem baseNode,
			string expression, string[] attributeNames)
		{
			bool bf;
			int lp = 0;
			Match m;
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem[] ro = new HtmlNodeItem[0];
			string tl;

			if(baseNode != null)
			{
				foreach(HtmlAttributeItem ai in baseNode.Attributes)
				{
					tl = ai.Name;
					bf = false;
					foreach(string s in attributeNames)
					{
						if(s == tl)
						{
							bf = true;
							break;
						}
					}
					if(bf)
					{
						m = Regex.Match(ai.Value, expression);
						if(m.Success)
						{
							oc.Add(baseNode);
							break;
						}
					}
				}
				foreach(HtmlNodeItem ni in baseNode.Nodes)
				{
					FindAttribute(ni, expression, attributeNames, oc);
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
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return Nodes by specified Attributes.
		/// </summary>
		/// <param name="baseNode">
		/// Base node at which to begin the search.
		/// </param>
		/// <param name="expression">
		/// Regular expression used to find the text.
		/// </param>
		/// <param name="attributeNames">
		/// Array of Attribute Names to search for.
		/// </param>
		/// <param name="foundList">
		/// Collection of items already found.
		/// </param>
		public static void FindAttribute(HtmlNodeItem baseNode,
			string expression, string[] attributeNames, ObjectCollection foundList)
		{
			bool bf;        //	Match Found.
			Match m;
			string tl;      //	Lower Case.

			if(baseNode != null)
			{
				foreach(HtmlAttributeItem ai in baseNode.Attributes)
				{
					tl = ai.Name;
					bf = false;
					foreach(string s in attributeNames)
					{
						if(s == tl)
						{
							bf = true;
							break;
						}
					}
					if(bf)
					{
						m = Regex.Match(ai.Value, expression);
						if(m.Success)
						{
							foundList.Add(baseNode);
							break;
						}
					}
				}
				foreach(HtmlNodeItem ni in baseNode.Nodes)
				{
					FindAttribute(ni, expression, attributeNames, foundList);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FindNodeText																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a node by text content.
		/// </summary>
		/// <param name="baseNode">
		/// Base node at which to begin the search. The base node text is assumed
		/// to be an eclosing element, so is not searched.
		/// </param>
		/// <param name="expression">
		/// Regular expression used to find the text.
		/// </param>
		/// <returns>
		/// Array of Nodes matching the Expression, if found. Otherwise, zero
		/// length array.
		/// </returns>
		public static HtmlNodeItem[] FindNodeText(HtmlNodeItem baseNode,
			string expression)
		{
			int lp = 0;
			Match m;
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem[] ro = new HtmlNodeItem[0];

			if(baseNode != null)
			{
				foreach(HtmlNodeItem ni in baseNode.Nodes)
				{
					m = Regex.Match(ni.Text, expression);
					if(m.Success)
					{
						//	If we found a match, then return the node to the caller.
						oc.Add(ni);
					}
					FindNodeText(ni, expression, oc);
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
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a node by text content.
		/// </summary>
		/// <param name="baseNode">
		/// Base node at which to begin the search. The base node text is assumed
		/// to be an eclosing element, so is not searched.
		/// </param>
		/// <param name="expression">
		/// Regular expression used to find the text.
		/// </param>
		/// <param name="foundList">
		/// Collection of items already found.
		/// </param>
		public static void FindNodeText(HtmlNodeItem baseNode,
			string expression, ObjectCollection foundList)
		{
			Match m;

			if(baseNode != null)
			{
				foreach(HtmlNodeItem ni in baseNode.Nodes)
				{
					m = Regex.Match(ni.Text, expression);
					if(m.Success)
					{
						//	If we found a match, then return the node to the caller.
						foundList.Add(ni);
					}
					FindNodeText(ni, expression, foundList);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetId																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Safely return the ID attribute of the provided node, if it exists.
		/// </summary>
		/// <param name="node">
		/// Reference to the HTML node to inspect.
		/// </param>
		/// <returns>
		/// The ID attribute if the node, if found. Otherwise, an empty string.
		/// </returns>
		public static string GetId(HtmlNodeItem node)
		{
			string result = "";

			if(node != null && node.Attributes.Exists(x => x.Name == "id"))
			{
				result = node.Attributes["id"].Value;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetOutermostNodeOfType																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the specified node type encapsulating the provided node at the
		/// outermost level.
		/// </summary>
		/// <param name="node">
		/// Instance of the node which is either the specified type, or contained
		/// by the specified type.
		/// </param>
		/// <param name="nodeType">
		/// The type of Node to find in the provided or parent instances.
		/// </param>
		/// <returns>
		/// If the specified type is found, then the instance of that type.
		/// Otherwise, null.
		/// </returns>
		public static HtmlNodeItem GetOutermostNodeOfType(HtmlNodeItem node,
			string nodeType)
		{
			HtmlNodeItem wn = null;     //	Working Node.
			HtmlNodeItem wnr = null;    //	Reference Node.

			wnr = wn = GetOuterNodeOfType(node, nodeType);
			while(wn != null)
			{
				wn = GetOuterNodeOfType(wn.ParentNode, nodeType);
				if(wn != null)
				{
					wnr = wn;
				}
			}
			return wnr;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetOuterNodeOfType																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the specified node type encapsulating the provided node.
		/// </summary>
		/// <param name="node">
		/// Instance of the node which is either the specified type, or contained
		/// by the specified type.
		/// </param>
		/// <param name="nodeType">
		/// The type of Node to find in the provided or parent instances.
		/// </param>
		/// <returns>
		/// If the specified type is found, then the instance of that type.
		/// Otherwise, null.
		/// </returns>
		public static HtmlNodeItem GetOuterNodeOfType(HtmlNodeItem node,
			string nodeType)
		{
			while(node != null && node.NodeType != nodeType)
			{
				node = node.ParentNode;
			}
			return node;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the specified node type encapsulating the provided node.
		/// </summary>
		/// <param name="node">
		/// Instance of the node which is either the specified type, or contained
		/// by the specified type.
		/// </param>
		/// <param name="nodeTypes">
		/// List of types of Node to find in the provided or parent instances.
		/// </param>
		/// <returns>
		/// If one of the specified types is found, then the instance of that type.
		/// Otherwise, null.
		/// </returns>
		public static HtmlNodeItem GetOuterNodeOfType(HtmlNodeItem node,
			string[] nodeTypes)
		{
			int hv = -1;                //	Current High Value.
			int hi = -1;                //	High Value Index.
			int i = 0;                  //	Working Indexer.
			int ic = 0;                 //	Collection Count.
			IntCollection icl = new IntCollection();  //	Level Index.
			HtmlNodeItem ni;            //	Working Node.
			ObjectCollection oc = new ObjectCollection();
			HtmlNodeItem ro = null;     //	Return Value.

			foreach(string s in nodeTypes)
			{
				ni = GetOuterNodeOfType(node, s);
				if(ni != null)
				{
					icl.Add(ni.Level);
					oc.Add(ni);
				}
			}
			if(icl.Count != 0)
			{
				if(icl.Count == 1)
				{
					//	If only one item was found, then return that node.
					ro = (HtmlNodeItem)oc[0];
				}
				else
				{
					ic = icl.Count;
					for(i = 0; i < ic; i++)
					{
						if(hv == -1 || icl[i] > hv)
						{
							//	If we found a new high level, then record its index.
							hi = i;
						}
					}
					if(hi != -1)
					{
						//	If we found a high index, then return that value.
						ro = (HtmlNodeItem)oc[hi];
					}
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRoot																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the root-most node of the specified item.
		/// </summary>
		/// <param name="node">
		/// Reference to the node for which a root reference will be found.
		/// </param>
		/// <returns>
		/// Reference to the root-most node available in the heirarchy, if found.
		/// Otherwise, null;
		/// </returns>
		public static HtmlNodeItem GetRoot(HtmlNodeItem node)
		{
			HtmlNodeItem result = null;

			if(node != null)
			{
				if(node.ParentNode != null)
				{
					result = GetRoot(node.ParentNode);
					if(result == null)
					{
						result = node.ParentNode;
					}
				}
				else
				{
					result = node;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified style in the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the node to inspect.
		/// </param>
		/// <param name="styleName">
		/// Name of the style property to return.
		/// </param>
		/// <returns>
		/// Value of the specified style property, if found. Otherwise, an
		/// empty string.
		/// </returns>
		public static string GetStyle(HtmlNodeItem node, string styleName)
		{
			HtmlAttributeItem attribute = null;
			string[] parts = null;
			string result = "";
			string[] styles = null;

			if(node?.Attributes.Count > 0)
			{
				attribute = node.Attributes.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					//	Style attribute found.
					styles = attribute.Value.Split(';').Select(p => p.Trim()).ToArray();
					foreach(string styleItem in styles)
					{
						if(styleItem.IndexOf(':') > -1)
						{
							parts =
								styleItem.Split(':').Select(p => p.Trim()).ToArray();
							if(parts[0] == styleName)
							{
								result = parts[1];
								break;
							}
						}
						else if(styleItem == styleName)
						{
							result = styleItem;
							break;
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HasSiblingsAfter																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified Node has siblings
		/// following.
		/// </summary>
		/// <param name="value">
		/// Value to inspect.
		/// </param>
		/// <returns>
		/// True if siblings follow the specified Item. Otherwise, false.
		/// </returns>
		public static bool HasSiblingsAfter(HtmlNodeItem value)
		{
			bool bf = false;
			bool rv = false;

			if(value != null && value.Parent != null)
			{
				foreach(HtmlNodeItem ni in value.Parent)
				{
					if(bf)
					{
						rv = true;
						break;
					}
					if(ni.Equals(value))
					{
						bf = true;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Html																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the HTML Content of this Node Item.
		/// </summary>
		public string Html
		{
			get
			{
				//bool bQuoted = true;
				StringBuilder builder = new StringBuilder();
				bool lineFeedSeparation = true;
				bool preserveSpace = false;

				if(Parent != null)
				{
					lineFeedSeparation = HtmlNodeCollection.GetLineFeed(Parent);
					preserveSpace = HtmlNodeCollection.GetPreserveSpace(Parent);
				}
				HtmlUtil.AppendNodeHtml(this, builder, preserveSpace,
					lineFeedSeparation);

				return builder.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Index																																	*
		//*-----------------------------------------------------------------------*
		private int mIndex = 0;
		/// <summary>
		/// Get/Set the Index of this Item within the local collection.
		/// </summary>
		public int Index
		{
			get { return mIndex; }
			set { mIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InnerHtml																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the HTML Content Inside this entity.
		/// </summary>
		public string InnerHtml
		{
			get { return $"{mText}{Nodes.Html}"; }
			set { Nodes.Html = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InnerText																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the Inner Text on this Item.
		/// </summary>
		public string InnerText
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				sb.Append(mText);
				if(Nodes.Count > 0)
				{
					foreach(HtmlNodeItem ni in Nodes)
					{
						//sb.Append(ni.Text);
						sb.Append(ni.InnerText);
					}
				}
				//else
				//{
				//	sb.Append(mText);
				//}
				return sb.ToString();
			}
			set
			{
				HtmlNodeItem n = null;  //	Working node.

				if(Nodes.Count > 0)
				{
					foreach(HtmlNodeItem ni in Nodes)
					{
						if(ni.Text.Length != 0)
						{
							n = ni;
							ni.Text = value;
							break;
						}
					}
					foreach(HtmlNodeItem ni in Nodes)
					{
						if(ni.Text.Length != 0 && !ni.Equals(n))
						{
							ni.Text = "";
						}
					}
				}
				else
				{
					//	This was an empty node.
					//	Load the section without parsing.
					this.mText = value;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Id																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the ID of this Node.
		/// </summary>
		public string Id
		{
			get
			{
				HtmlAttributeItem ai = Attributes["id"];
				string rs = "";

				if(ai != null)
				{
					//	If the ID attribute exists, then return its value.
					rs = ai.Value;
				}
				return rs;
			}
			set
			{
				HtmlAttributeItem ai = Attributes.FirstOrDefault(x => x.Name == "id");

				if(ai != null)
				{
					//	If the id attribute already existed, then simply change the
					//	value.
					ai.Value = value;
				}
				else
				{
					//	Otherwise, add it to the structure.
					Attributes.Add("id", value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Level																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the level of this item within the hierarchy.
		/// </summary>
		public int Level
		{
			get
			{
				int rv = 0;

				if(ParentNode != null)
				{
					rv = ParentNode.Level + 1;
				}
				return rv;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the Name of this Node.
		/// </summary>
		public string Name
		{
			get
			{
				HtmlAttributeItem ai = Attributes["name"];
				string rs = "";

				if(ai != null)
				{
					//	If the ID attribute exists, then return its value.
					rs = ai.Value;
				}
				return rs;
			}
			set
			{
				HtmlAttributeItem ai =
					Attributes.FirstOrDefault(x => x.Name == "name");
				if(ai != null)
				{
					//	If the id attribute already existed, then simply change the
					//	value.
					ai.Value = value;
				}
				else
				{
					//	Otherwise, add it to the structure.
					Attributes.Add("name", value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Nodes																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Internal member for <see cref="Nodes">Nodes</see>.
		/// </summary>
		internal HtmlNodeCollection mNodes = new HtmlNodeCollection();
		/// <summary>
		/// Get a reference to the Nodes collection for this Item.
		/// </summary>
		public HtmlNodeCollection Nodes
		{
			get { return mNodes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	NodeType																															*
		//*-----------------------------------------------------------------------*
		private string mNodeType = "";
		/// <summary>
		/// Get/Set the Node Type of this Node.
		/// </summary>
		public string NodeType
		{
			get { return mNodeType; }
			set { mNodeType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OpeningTag																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the Opening Tag of this Element Node.
		/// </summary>
		public string OpeningTag
		{
			get
			{
				bool qt = false;      //	Quote Flag.
				StringBuilder sb = new StringBuilder();

				if(NodeType.Length != 0)
				{
					//	This is an element.
					sb.Append("<" + NodeType);
					//	If this item has attributes, then attach them.
					foreach(HtmlAttributeItem ai in Attributes)
					{
						//	Each attribute has at least a name.
						sb.Append(" " + ai.Name);
						if(ai.Value.Length != 0 || !ai.Presence)
						{
							//	In this version, all attributes have quoted values unless
							//	they are marked as presence-only.
							//	If the attribute has a value, then place it.
							qt = (HtmlAttributeCollection.Unquoted[ai.Name] == null);
							sb.Append('=');
							if(qt)
							{
								sb.Append('\"');
							}
							sb.Append(ai.Value);
							if(qt)
							{
								sb.Append('\"');
							}
						}
					}
					sb.Append(">");
				}
				return sb.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Original																															*
		//*-----------------------------------------------------------------------*
		private string mOriginal = "";
		/// <summary>
		/// Get/Set the Original Value of this Node.
		/// </summary>
		public string Original
		{
			get { return mOriginal; }
			set { mOriginal = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Parent																																*
		//*-----------------------------------------------------------------------*
		private HtmlNodeCollection mParent = null;
		/// <summary>
		/// Get/Set the Node Collection to which this Node Belongs.
		/// </summary>
		public HtmlNodeCollection Parent
		{
			get { return mParent; }
			set { mParent = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ParentNode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the Parent Node of this Item.
		/// </summary>
		public HtmlNodeItem ParentNode
		{
			get
			{
				HtmlNodeItem ro = null;     //	Return value.

				if(mParent != null)
				{
					//	If there is a collection, then return that collection's parent.
					ro = mParent.ParentNode;
				}
				return ro;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PreviousNodeType																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the node type of the previous sibling node in the current level.
		/// </summary>
		/// <param name="node">
		/// Base node at which to begin the search.
		/// </param>
		/// <returns>
		/// Previous non-blank node type, if found. Otherwise, an empty string.
		/// </returns>
		public static string PreviousNodeType(HtmlNodeItem node)
		{
			bool bf = false;    //	Flag - Found.
			int lp = 0;         //	List Position.
			HtmlNodeCollection nc;  //	Parent Collection.
			string rs = "";     //	Return String.

			if(node != null && node.Parent != null)
			{
				//	If the node was specified, and is a member of a collection, then
				//	search the collection for this item.
				nc = node.Parent;
				for(lp = nc.Count - 1; lp >= 0; lp--)
				{
					if(!bf)
					{
						//	If we haven't found the reference node yet, then keep searching
						if(nc[lp] == node)
						{
							//	Here, we found the reference. Any previous item with a type
							//	will match.
							bf = true;
						}
					}
					else
					{
						//	If we have already found our reference node, then match if this
						//	entry has a type.
						if(nc[lp].NodeType.Length != 0)
						{
							rs = nc[lp].NodeType;
							break;
						}
					}
				}
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RemoveAttribute																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all attributes of a specified name within the Node and children.
		/// </summary>
		/// <param name="node">
		/// Instance of the Node to inspect.
		/// </param>
		/// <param name="name">
		/// Name of the Attribute to Remove.
		/// </param>
		public static void RemoveAttribute(HtmlNodeItem node, string name)
		{
			HtmlAttributeItem ai;   //	Working Attribute.
			int lc = 0;     //	List Count.
			int lp = 0;     //	List Position.
			string tl = name; //	Name to search for.

			if(node != null)
			{
				lc = node.Attributes.Count;
				for(lp = 0; lp < lc; lp++)
				{
					ai = node.Attributes[lp];
					if(ai.Name == tl)
					{
						node.Attributes.RemoveAt(lp);
						lp--;   //	Deindex.
						lc--;   //	Decount.
					}
				}
				foreach(HtmlNodeItem ni in node.Nodes)
				{
					RemoveAttribute(ni, name);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResetParent																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Reset the parent property on the node and its descendants.
		/// </summary>
		/// <param name="node">
		/// Starting node at which to being reset.
		/// </param>
		public static void ResetParent(HtmlNodeItem node)
		{
			if(node != null)
			{
				HtmlNodeCollection.ResetParent(node.Nodes);
				node.mParent = null;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelfClosing																														*
		//*-----------------------------------------------------------------------*
		private bool mSelfClosing = false;
		/// <summary>
		/// Get/Set a value indicating whether this node is self-closing.
		/// </summary>
		public bool SelfClosing
		{
			get { return mSelfClosing; }
			set { mSelfClosing = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified style in the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the node to inspect.
		/// </param>
		/// <param name="styleName">
		/// Name of the style property to return.
		/// </param>
		/// <param name="value">
		/// Value of the style.
		/// </param>
		public static void SetStyle(HtmlNodeItem node, string styleName,
			string value)
		{
			HtmlAttributeItem attribute = null;
			//string attributeValue = "";
			StringBuilder builder = new StringBuilder();
			int index = 0;
			string[] parts = null;
			NameItem property = null;
			string[] styles = null;
			List<NameItem> values = new List<NameItem>();

			if(node != null)
			{
				attribute = node.Attributes.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					//	Style attribute found.
					//attributeValue = attribute.Value.Trim();
					//if(attributeValue.EndsWith(";"))
					//{
					//	attributeValue =
					//		attributeValue.Substring(0, attributeValue.Length - 1);
					//}
					styles = attribute.Value.Split(';').Select(p => p.Trim()).ToArray();
					foreach(string styleItem in styles)
					{
						if(styleItem.IndexOf(':') > -1)
						{
							parts =
								styleItem.Split(':').Select(p => p.Trim()).ToArray();
							values.Add(new NameItem()
							{
								Name = parts[0],
								Description = parts[1]
							});
						}
						else if(styleItem.Length > 0)
						{
							values.Add(new NameItem()
							{
								Name = styleItem
							});
						}
					}
					property = values.FirstOrDefault(x => x.Name == styleName);
					if(property != null)
					{
						property.Description = value;
					}
					else
					{
						values.Add(new NameItem()
						{
							Name = styleName,
							Description = value
						});
					}
					index = 0;
					foreach(NameItem item in values)
					{
						if(index > 0)
						{
							builder.Append(' ');
						}
						if(item.Description.Length > 0)
						{
							builder.Append($"{item.Name}: {item.Description};");
						}
						else
						{
							builder.Append($"{item.Name};");
						}
						index++;
					}
					attribute.Value = builder.ToString();
				}
				else
				{
					//	Style didn't yet exist on node.
					node.Attributes.Add("style", $"{styleName}: {value};");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Text																																	*
		//*-----------------------------------------------------------------------*
		private string mText = "";
		/// <summary>
		/// Get/Set the Non-Elemental Text of this Node.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TrailingText																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TrailingText">TrailingText</see>.
		/// </summary>
		private string mTrailingText = "";
		/// <summary>
		/// Get/Set the text trailing the end of this node.
		/// </summary>
		public string TrailingText
		{
			get { return mTrailingText; }
			set { mTrailingText = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this item.
		/// </summary>
		/// <returns>
		/// The string representation of this item's NodeType, if set. Otherwise,
		/// '(Blank)'.
		/// </returns>
		public override string ToString()
		{
			string result = "(Blank)";
			if(this.mNodeType?.Length > 0)
			{
				result = this.mNodeType;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
