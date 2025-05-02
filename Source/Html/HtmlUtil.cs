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
		/// <returns>
		/// Collection of attribute names and values for the specified element.
		/// </returns>
		public static NameValueCollection GetHtmlAttributes(string element)
		{
			StringBuilder builder = new StringBuilder();
			char[] chars = null;
			char charVal = char.MinValue;
			int count = 0;
			int index = 0;
			char inEscape = char.MinValue;
			char inQuote = char.MinValue;
			NameValueItem item = null;
			NameValueCollection results = new NameValueCollection();
			int state = 0;		// 0 - Brace; 1 - Element name; 2 - Name; 3 - Value

			if(element?.Length > 0)
			{
				chars = element.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index ++)
				{
					charVal = chars[index];
					switch(state)
					{
						case 0:
							//	Bracket.
							if(charVal == '<')
							{
								//	Brace was present.
								Clear(builder);
								state++;
							}
							else
							{
								//	Jump out when no brace was provided.
								index = count;
							}
							break;
						case 1:
							//	Entry Type.
							if(mWhiteSpace.Contains(charVal))
							{
								if(builder.Length > 0)
								{
									//	Some node type was specified.
									Clear(builder);
									state++;
								}
							}
							else
							{
								builder.Append(charVal);
							}
							break;
						case 2:
							//	Name.
							if(mWhiteSpace.Contains(charVal) ||
								charVal == '=' || charVal == '/' ||
								charVal == '>' || charVal == '?')
							{
								if(builder.Length > 0)
								{
									//	Name was specified.
									item = new NameValueItem()
									{
										Name = builder.ToString()
									};
									results.Add(item);
									Clear(builder);
								}
								if(charVal == '=')
								{
									//	Starting on the value.
									state++;
								}
								if(charVal == '/' || charVal == '>' || charVal == '?')
								{
									//	End of process.
									index = count;
								}
							}
							else
							{
								builder.Append(charVal);
							}
							break;
						case 3:
							//	Value.
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
										state--;
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
				for(index = 0; index < count; index ++)
				{
					//if(index == 1507)
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

	}
	//*-------------------------------------------------------------------------*

}
