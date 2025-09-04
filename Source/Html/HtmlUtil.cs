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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlUtil																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Features and functionality for the HTML library.
	/// </summary>
	public class HtmlUtil
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private static char[] mLineEnd = new char[] { '\r', '\n' };
		private static char[] mWhiteSpace = new char[] { ' ', '\t', '\r', '\n' };

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* AppendNodeHtml																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Append the rendered HTML text of the provided node to the supplied
		/// string builder.
		/// </summary>
		/// <param name="node">
		/// Reference to the node to be rendered.
		/// </param>
		/// <param name="builder">
		/// Reference to the string builder receiving the content.
		/// </param>
		/// <param name="preserveSpace">
		/// Value indicating whether space will be preserved throughout the
		/// document.
		/// </param>
		/// <param name="lineFeedSeparation">
		/// Value indicating whether nodes are terminated with line feeds.
		/// </param>
		public static void AppendNodeHtml(HtmlNodeItem node,
			StringBuilder builder,
			bool preserveSpace, bool lineFeedSeparation)
		{
			bool bQuoted = true;
			string nameTrim = "";

			if(node != null && builder != null)
			{
				if(node.NodeType.Length > 0 && node.NodeType != "!--" &&
					node.NodeType != "?")
				{
					//	This is an element.
					builder.Append("<" + node.NodeType);
					//	If this item has attributes, then attach them.
					if(preserveSpace)
					{
						//	Preserved space output.
						foreach(HtmlAttributeItem attributeItem in node.Attributes)
						{
							nameTrim = attributeItem.Name.Trim();
							if(attributeItem.PreSpace.Length > 0)
							{
								builder.Append(attributeItem.PreSpace);
							}
							else if(nameTrim.Length > 0)
							{
								builder.Append(' ');
							}
							if(nameTrim.Length > 0)
							{
								builder.Append(attributeItem.Name);
								if(attributeItem.AssignmentSpace.Length > 0)
								{
									builder.Append(attributeItem.AssignmentSpace);
								}
								else if(!attributeItem.Presence)
								{
									builder.Append('=');
								}
								if(!attributeItem.Presence)
								{
									//	If the attribute has a value, then place it.
									//	In this version, the quoted value is only omitted if
									//	the attribute is marked as a presence-only attribute.
									//	If deciding to unquote the following line, the unquoted
									//	property will need to be fixed.
									//qt = (HtmlAttributeCollection.Unquoted[ai.Name] == null);
									//	TODO: Use alternate quotes than contained in value.
									builder.Append('\"');
									builder.Append(attributeItem.Value);
									builder.Append('\"');
								}
							}
						}
					}
					else
					{
						//	Traditional output.
						foreach(HtmlAttributeItem attributeItem in node.Attributes)
						{
							//	Each attribute has at least a name.
							builder.Append(" " + attributeItem.Name);
							if(attributeItem.Value.Length > 0 || !attributeItem.Presence)
							{
								//	If the attribute has a value, then place it.
								//	In this version, the quoted value is only omitted if
								//	the attribute is marked as a presence-only attribute.
								//	If deciding to unquote the following line, the unquoted
								//	property will need to be fixed.
								//qt = (HtmlAttributeCollection.Unquoted[ai.Name] == null);
								builder.Append('=');
								if(bQuoted)
								{
									builder.Append('\"');
								}
								builder.Append(attributeItem.Value);
								if(bQuoted)
								{
									builder.Append('\"');
								}
							}
						}
					}
					if(node.SelfClosing && node.Nodes.Count == 0)
					{
						//	Self-closing.
						if(!preserveSpace)
						{
							builder.Append(' ');
						}
						builder.Append("/>");
						//	On a self-closing node, the text follows the tag.
						builder.Append(node.Text);
						//if(!ni.Text.EndsWith("\r") &&
						//	!ni.Text.EndsWith("\n") &&
						//	GetLineFeed(ni.Nodes))
						//{
						//	sb.AppendLine("");
						//}
					}
					else
					{
						//	Separate closing tag.
						builder.Append(">");
						//	Get all of the inner stuff.
						builder.Append(node.Text);
						builder.Append(node.Nodes.Html);
						//	If this item has a closing tag, then close it when done.
						if(!HtmlUtil.Singles.Exists(x =>
							x.ToLower() == node.NodeType.ToLower()))
						{
							builder.Append("</" + node.NodeType + ">");
							if(lineFeedSeparation && node.NodeType != "tspan")
							{
								builder.AppendLine("");
							}
						}
						else
						{
							//	Single item.
							if(lineFeedSeparation &&
								node.NodeType != "tspan" &&
								node.Text.IndexOfAny(new char[] { '\r', '\n' }) == -1)
							{
								builder.AppendLine("");
							}
						}
						builder.Append(node.TrailingText);
					}
				}
				else if(node.NodeType == "!--" || node.NodeType == "?")
				{
					builder.Append(node.Original);
					if(preserveSpace)
					{
						builder.Append(node.TrailingText);
					}
					if(lineFeedSeparation &&
						!node.Original.EndsWith("\r") &&
						!node.Original.EndsWith("\n"))
					{
						builder.AppendLine("");
					}
				}
				else
				{
					//	(Blanks)
					//	This text occurs after the close of the inside elements, as
					//	in the example <b>Some Stuff Inside</b>This text.
					//	Theoretically, this should not ever be called, after the
					//	introduction of the TrailingText version.
					//	TODO: Remove this section if proven to be unused.
					builder.Append(node.Text);
					builder.Append(node.Nodes.Html);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the specified string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the builder to be cleared.
		/// </param>
		public static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DecodeHtmlText																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Decode the text contents of the provided node collection.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes whose contents will be decoded.
		/// </param>
		public static void DecodeHtmlText(HtmlNodeCollection nodes)
		{
			if(nodes?.Count > 0)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					DecodeHtmlText(nodeItem);
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Decode the text contents of the provided HTML node.
		/// </summary>
		/// <param name="node">
		/// Reference to the node whose text contents will be decoded.
		/// </param>
		public static void DecodeHtmlText(HtmlNodeItem node)
		{
			string text = "";

			if(node != null)
			{
				if(node.Text?.Length > 0)
				{
					text = Regex.Replace(node.Text, @"[\r\n]+", " ");
					text = Regex.Replace(text, @"\s{2,}", " ");
					if(text.Trim().Length == 0)
					{
						//	If the item was space-only at this point, then clear it.
						text = "";
					}
					if(node.NodeType == "br" && node.Text.Length > 0)
					{
						node.Text = node.Text.TrimStart();
					}
					text = Regex.Replace(text,
						@"\&tab;", "\t", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&newline;", "\r\n", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&nbsp;", " ", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&quot;", "\"", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&apos;", "'", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&lt;", "<", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&gt;", ">", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&copy;", "©", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&reg;", "®", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&deg;", "°", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&spades;", "♠", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&clubs;", "♣", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&hearts;", "♥", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&diams;", "♦", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&bull;", "•", RegexOptions.IgnoreCase);
					text = Regex.Replace(text,
						@"\&amp;", "&", RegexOptions.IgnoreCase);
					node.Text = text;
				}
				DecodeHtmlText(node.Nodes);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FixAttributeQuotes																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fix all attribute quotes in the provided node collection and its
		/// descendants.
		/// </summary>
		/// <param name="nodes">
		/// Reference to the collection of nodes to repair.
		/// </param>
		public static void FixAttributeQuotes(HtmlNodeCollection nodes)
		{
			if(nodes?.Count > 0)
			{
				foreach(HtmlNodeItem nodeItem in nodes)
				{
					foreach(HtmlAttributeItem attributeItem in nodeItem.Attributes)
					{
						if(attributeItem.Value.IndexOf('"') > -1)
						{
							attributeItem.Value = attributeItem.Value.Replace("\"", "'");
						}
					}
					FixAttributeQuotes(nodeItem.Nodes);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetHtmlAttributes																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse and return a collection of attribute names and values from the
		/// provided element text.
		/// </summary>
		/// <param name="element">
		/// HTML element content with opening and closing braces.
		/// </param>
		/// <param name="preserveSpace">
		/// Value indicating whether to preserve all whitespace in the document.
		/// </param>
		/// <returns>
		/// Collection of attribute names and values for the specified element.
		/// </returns>
		public static HtmlAttributeCollection GetHtmlAttributes(string element,
			bool preserveSpace)
		{
			bool bAssignment = false;
			bool bContinue = true;
			bool bName = false;
			StringBuilder builder = new StringBuilder();
			char[] chars = null;
			char charVal = char.MinValue;
			int count = 0;
			int index = 0;
			char inEscape = char.MinValue;
			char inQuote = char.MinValue;
			HtmlAttributeItem item = null;
			HtmlAttributeCollection results = new HtmlAttributeCollection();
			int state = 0;
			// State:
			// 0 - Brace;
			// 1 - Tag name;
			// 2 - Pre-space;
			// 3 - Name;
			// 4 - Assignment space;
			// 5 - Value

			if(element?.Length > 0)
			{
				chars = element.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index++)
				{
					charVal = chars[index];
					switch(state)
					{
						case 0:
							//	Bracket.
							bName = false;
							if(charVal == '<')
							{
								//	Brace was present.
								Clear(builder);
								state = 1;
							}
							else
							{
								//	Jump out when no brace was provided.
								index = count;
							}
							break;
						case 1:
							//	Entry Type.
							bName = false;
							if(mWhiteSpace.Contains(charVal))
							{
								if(builder.Length > 0)
								{
									//	Some node type was specified.
									Clear(builder);
									if(preserveSpace)
									{
										state = 2;
										index--;	//	Re-read character.
									}
									else
									{
										state = 3;
									}
								}
							}
							else
							{
								builder.Append(charVal);
							}
							break;
						case 2:
							//	Pre-space.
							bName = false;
							if(mWhiteSpace.Contains(charVal))
							{
								builder.Append(charVal);
							}
							else
							{
								if(builder.Length > 0)
								{
									//	This item will also cover spaces after the end of the
									//	last attribute.
									item = new HtmlAttributeItem()
									{
										PreSpace = builder.ToString()
									};
									results.Add(item);
									Clear(builder);
									state = 3;
									index--;	//	Re-read the current character.
								}
							}
							break;
						case 3:
							//	Name.
							bAssignment = false;
							if(mWhiteSpace.Contains(charVal) ||
								charVal == '=' || charVal == '/' ||
								charVal == '>' || charVal == '?')
							{
								if(builder.Length > 0)
								{
									//	Name was specified.
									if(item == null)
									{
										item = new HtmlAttributeItem();
										results.Add(item);
									}
									item.Name = builder.ToString();
									Clear(builder);
									bName = true;
								}
								if(charVal == '/' || charVal == '>' || charVal == '?')
								{
									//	End of process.
									index = count;
								}
								else if(bName)
								{
									//	Equal or whitespace.
									//	This might be a presence-only attribute or
									//	the assignment could be separated by spaces.
									//	In either case, we will let the assignment
									//	phase decide what is going to happen next.
									state = 4;
									index--;  //	Re-read this character.
								}
							}
							else
							{
								//	Build the name.
								builder.Append(charVal);
							}
							break;
						case 4:
							//	Assignment.
							bName = false;
							if(mWhiteSpace.Contains(charVal) ||
								charVal == '=')
							{
								builder.Append(charVal);
								if(charVal == '=')
								{
									bAssignment = true;
								}
							}
							else
							{
								if(item != null)
								{
									if(builder.Length > 0)
									{
										if(bAssignment)
										{
											if(preserveSpace)
											{
												item.AssignmentSpace = builder.ToString();
											}
											state = 5;
										}
										else
										{
											item.Presence = true;
											item = new HtmlAttributeItem()
											{
												PreSpace = builder.ToString()
											};
											results.Add(item);
											state = 3;
										}
										Clear(builder);
										index--;	//	Re-read this character.
									}
								}
							}
							break;
						case 5:
							//	Value.
							bName = false;
							if(item != null)
							{
								if(inEscape != char.MinValue)
								{
									//	This character is escapted.
									builder.Append(charVal);
									inEscape = char.MinValue;
								}
								else if(inQuote != char.MinValue)
								{
									//	Working in quote.
									builder.Append(charVal);
									if(charVal == '\\')
									{
										//	Start of escape.
										inEscape = charVal;
									}
									else if(inQuote == charVal)
									{
										//	End of quote.
										inQuote = char.MinValue;
									}
								}
								else if(charVal == '\'' || charVal == '"' || charVal == '`')
								{
									//	Start of quote.
									builder.Append(charVal);
									inQuote = charVal;
								}
								else if(mWhiteSpace.Contains(charVal) ||
									charVal == '/' || charVal == '?' || charVal == '>')
								{
									//	Space after value.
									if(builder.Length > 0)
									{
										item.Value = RemoveOuterQuotes(builder.ToString());
										Clear(builder);
										item = null;
									}
									if(charVal == '/' || charVal == '>')
									{
										//	End of element.
										bContinue = false;
									}
									else
									{
										//	Whitespace.
										if(preserveSpace)
										{
											state = 2;
											index--;		//	Re-read this character.
										}
										else
										{
											state = 3;
										}
									}
								}
								else
								{
									//	Some non-space value.
									//	This should typically be a number.
									builder.Append(charVal);
								}
							}
							break;
					}
					if(!bContinue)
					{
						break;
					}
				}
			}
			return results;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetHtmlCommentText																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the text of the provided HTML comment-formatted string.
		/// </summary>
		/// <param name="htmlComment">
		/// HTML comment-formatted string with the syntax &lt;!-- Content --&gt;
		/// </param>
		/// <returns>
		/// The text contents of the provided comment string.
		/// </returns>
		public static string GetHtmlCommentText(string htmlComment)
		{
			string result = "";

			if(htmlComment?.Length > 0)
			{
				result = Regex.Replace(htmlComment,
					ResourceMain.rxHtmlCommentText, "${content}");
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetHtmlElementTextOnly																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return only the text of provided HTML rlement.
		/// </summary>
		/// <param name="node">
		/// Reference to the node for which the node will be rendered.
		/// </param>
		/// <returns>
		/// The HTML content of just the opening element.
		/// </returns>
		public static string GetHtmlElementTextOnly(HtmlNodeItem node)
		{
			StringBuilder builder = new StringBuilder();
			int index = 0;

			if(node != null)
			{
				builder.Append("<");
				builder.Append(node.NodeType);
				builder.Append(" ");
				foreach(HtmlAttributeItem attributeItem in node.Attributes)
				{
					if(index > 0)
					{
						builder.Append(" ");
					}
					builder.Append(attributeItem.Name);
					if(!attributeItem.Presence)
					{
						if(attributeItem.Value.IndexOf('"') > -1)
						{
							//	This item's value contains a quote.
							if(attributeItem.Value.IndexOf("'") > -1)
							{
								//	This item's value contains an apostrophe.
								//	Use backtick.
								builder.Append("=`");
								builder.Append(attributeItem.Value);
								builder.Append("`");
							}
							else
							{
								//	Apostrophe okay.
								builder.Append("='");
								builder.Append(attributeItem.Value);
								builder.Append("'");
							}
						}
						else
						{
							//	Quote okay.
							builder.Append("=\"");
							builder.Append(attributeItem.Value);
							builder.Append("\"");
						}
					}
					index++;
				}
				if(node.SelfClosing)
				{
					builder.Append(" /");
				}
				builder.Append(">");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetHtmlTags																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of HTML tags present in the source content.
		/// </summary>
		/// <param name="content">
		/// Content to inspect.
		/// </param>
		/// <returns>
		/// Reference to a sequential collection of HTML tags present in the
		/// content.
		/// </returns>
		public static StringTokenCollection GetHtmlElements(string content)
		{
			bool bInComment = false;
			bool bInElement = false;
			StringBuilder builder = new StringBuilder();
			char charVal = char.MinValue;
			char[] chars = null;
			int count = 0;
			char inEscape = char.MinValue;
			int index = 0;
			char inQuote = char.MinValue;
			StringTokenCollection result = new StringTokenCollection();
			int startIndex = 0;
			StringTokenItem token = null;

			if(content?.Length > 0)
			{
				//	Content was supplied.
				result.Original = content;
				chars = content.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index++)
				{
					//if(index == 1012)
					//{
					//	Console.WriteLine("Break here...");
					//}
					//if(builder.ToString() == "<style type=\"text/css\" id=\"appStyle\"")
					//{
					//	Console.WriteLine("GetHtmlElements. Break here...");
					//}
					//if(index == 592)
					//{
					//	Console.WriteLine("GetHtmlElements. Break here...");
					//}
					charVal = chars[index];
					if(!bInElement)
					{
						//	Not working in element.
						if(inEscape != char.MinValue)
						{
							//	Working in an escape.
							inEscape = char.MinValue;
						}
						else if(inQuote != char.MinValue)
						{
							if(inQuote == charVal)
							{
								//	Closing quote.
								inQuote = char.MinValue;
							}
							else if(charVal == '\\')
							{
								inEscape = '\\';
							}
						}
						//	NOTE: Quoted text is not observed in the inner HTML area.
						//else if(charVal == '\'' || charVal == '"' || charVal == '`')
						//{
						//	//	Starting a quote.
						//	inQuote = charVal;
						//}
						else if(charVal == '<')
						{
							//	Starting a comment or element.
							startIndex = index;
							builder.Append(charVal);
							bInElement = true;
							if(index + 3 < count &&
								chars[index + 1] == '!' &&
								chars[index + 2] == '-' &&
								chars[index + 3] == '-')
							{
								//	Comment.
								builder.Append(chars[index + 1]);
								builder.Append(chars[index + 2]);
								builder.Append(chars[index + 3]);
								index += 3;
								bInComment = true;
							}
						}
					}
					else
					{
						//	Working in element.
						if(bInComment)
						{
							//	Working in comment.
							builder.Append(charVal);
							if(charVal == '-' &&
								index + 2 < count &&
								chars[index + 1] == '-' &&
								chars[index + 2] == '>')
							{
								builder.Append(chars[index + 1]);
								builder.Append(chars[index + 2]);
								token = new StringTokenItem()
								{
									StartIndex = startIndex,
									Value = builder.ToString()
								};
								result.Add(token);
								Clear(builder);
								index += 2;
								bInComment = false;
								bInElement = false;
							}
						}
						else if(inEscape != char.MinValue)
						{
							//	Working in an escape.
							builder.Append(charVal);
							inEscape = char.MinValue;
						}
						else if(inQuote != char.MinValue)
						{
							//	Working in a quote.
							builder.Append(charVal);
							if(inQuote == charVal)
							{
								//	Closing quote.
								inQuote = char.MinValue;
							}
							else if(charVal == '\\')
							{
								inEscape = '\\';
							}
						}
						else if(charVal == '\'' || charVal == '"' || charVal == '`')
						{
							//	Starting a quote.
							builder.Append(charVal);
							inQuote = charVal;
						}
						else if(charVal == '>')
						{
							//	Closing symbol found.
							builder.Append(charVal);
							token = new StringTokenItem()
							{
								StartIndex = startIndex,
								Value = builder.ToString()
							};
							result.Add(token);
							Clear(builder);
							bInElement = false;
						}
						else
						{
							//	Building the element.
							builder.Append(charVal);
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetText																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return only the text content of the provided HTML document.
		/// </summary>
		/// <param name="document">
		/// Reference to the HTML document to evaluate.
		/// </param>
		/// <returns>
		/// The text-only content of the document.
		/// </returns>
		public static string GetText(HtmlDocument document)
		{
			bool bEndSpace = true;
			StringBuilder builder = new StringBuilder();
			string text = "";

			if(document != null)
			{
				foreach(HtmlNodeItem nodeItem in document.Nodes)
				{
					text = GetText(nodeItem);
					if(text.Length > 0)
					{
						if(builder.Length > 0 && !bEndSpace)
						{
							builder.Append(' ');
						}
						builder.Append(text);
						bEndSpace = text.EndsWith(' ') || text.EndsWith('\n');
					}
				}
			}
			return builder.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return only the text content of the provided HTML node.
		/// </summary>
		/// <param name="node">
		/// Reference to the HTML node to evaluate.
		/// </param>
		/// <returns>
		/// The text-only content of the node.
		/// </returns>
		public static string GetText(HtmlNodeItem node)
		{
			bool bEndSpace = true;
			StringBuilder builder = new StringBuilder();
			string text = "";

			if(node != null)
			{
				if(node.NodeType == "br")
				{
					builder.AppendLine("");
				}
				if(node.Text?.Length > 0)
				{
					if(builder.Length > 0 && !bEndSpace)
					{
						builder.Append(' ');
					}
					builder.Append(node.Text);
				}
				foreach(HtmlNodeItem nodeItem in node.Nodes)
				{
					text = GetText(nodeItem);
					if(text.Length > 0)
					{
						if(builder.Length > 0 && !bEndSpace)
						{
							builder.Append(' ');
						}
						builder.Append(text);
						bEndSpace = text.EndsWith(' ') || text.EndsWith('\n');
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HtmlNodeTypes																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="HtmlNodeTypes">HtmlNodeTypes</see>.
		/// </summary>
		private static List<string> mHtmlNodeTypes = new List<string>
		{
			"!--", "!doctype", "a", "abbr", "acronym", "address", "applet", "area",
			"article", "aside", "audio", "b", "base", "basefont", "bdi", "bdo",
			"big", "blockquote", "body", "br", "button", "canvas", "caption",
			"center", "cite", "code", "col", "colgroup", "data", "datalist", "dd",
			"del", "details", "dfn", "dialog", "dir", "div", "dl", "dt", "em",
			"embed", "fieldset", "figcaption", "figure", "font", "footer", "form",
			"frame", "frameset", "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8",
			"h9", "head", "header", "hgroup", "hr", "html", "i", "iframe", "img",
			"input", "ins", "kbd", "label", "legend", "li", "link", "main", "map",
			"mark", "menu", "meta", "meter", "nav", "noframes", "noscript", "object",
			"ol", "optgroup", "option", "output", "p", "param", "picture", "pre",
			"progress", "q", "rp", "rt", "ruby", "s", "samp", "script", "search",
			"section", "select", "small", "source", "span", "strike", "strong",
			"style", "sub", "summary", "sup", "svg", "table", "tbody", "td",
			"template", "textarea", "tfoot", "th", "thead", "time", "title", "tr",
			"track", "tt", "u", "ul", "var", "video", "wbr"
		};
		/// <summary>
		/// Get a reference to the collection of known HTML node types.
		/// </summary>
		public static List<string> HtmlNodeTypes
		{
			get { return mHtmlNodeTypes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveOuterQuotes																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the outer quotes from the caller's string and return the new
		/// value.
		/// </summary>
		/// <param name="value">
		/// Original value that might be surrounded by outer quotes.
		/// </param>
		/// <returns>
		/// Caller's value without matching outer quotes.
		/// </returns>
		public static string RemoveOuterQuotes(string value)
		{
			char charVal = char.MinValue;
			string result = "";

			if(value?.Length > 0)
			{
				charVal = value[0];
				if(charVal == '\'' || charVal == '"' || charVal == '`' &&
					value.EndsWith(charVal))
				{
					//	String is quoted.
					result = value.Substring(1, value.Length - 2);
				}
				else
				{
					result = value;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Singles																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Singles">Singles</see>.
		/// </summary>
		private static List<string> mSingles =
			new List<string>
			{
				"?", "!doctype", "base", "basefont", "bgsound", "br", "col",
				"embed", "frame", "hr", "img", "input", "link", "meta", "param",
				"wbr"
			};
		/// <summary>
		/// Get a reference to a list of Single Elements not requiring a Closing
		/// Tag.
		/// </summary>
		public static List<string> Singles
		{
			get { return mSingles; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
