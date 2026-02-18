/*
 * Copyright (c). 2000 - 2026 Daniel Patterson, MCSD (danielanywhere).
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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	CssEntryBuilder																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Entry building helper for css entries.
	/// </summary>
	public class CssEntryBuilder
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private int mCharCount = 0;
		private int mCharIndex = 0;
		private CssPartEnum mCurrentFocus = CssPartEnum.Selector;
		private char mInEscape = char.MinValue;
		private char mInQuote = char.MinValue;
		private bool mLastWasSpace = false;
		private char[] mLineEnd = new char[] { '\r', '\n' };
		private char[] mWhiteSpace = new char[] { ' ', '\t', '\r', '\n' };

		//*-----------------------------------------------------------------------*
		//* PostAttributeName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Post the attribute name to the current entry.
		/// </summary>
		private void PostAttributeName()
		{
			if(mEntries != null && mEntry != null)
			{
				if(mBuilder.Length > 0)
				{
					mAttribute = new CssAttributeItem()
					{
						Name = mBuilder.ToString()
					};
					mEntry.Attributes.Add(mAttribute);
					CssUtil.Clear(mBuilder);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PostAttributeValue																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Post the outstanding value of the attribute to the current entry.
		/// </summary>
		private void PostAttributeValue()
		{
			if(mAttribute != null && mBuilder.Length > 0)
			{
				mAttribute.Value = mBuilder.ToString().Trim();
				mAttribute = null;
				CssUtil.Clear(mBuilder);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PostEntry																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Post the current entry.
		/// </summary>
		private void PostEntry()
		{
			if(mEntries != null)
			{
				if(mEntry != null)
				{
					mEntry = null;
					mAttribute = null;
					CssUtil.Clear(mBuilder);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PostSelector																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Post a selector to the current entry, creating an entry if necessary,
		/// then clearing the string builder afterward.
		/// </summary>
		private void PostSelector()
		{
			if(mEntries != null)
			{
				if(mEntry == null)
				{
					mEntry = new CssEntryItem();
					mEntries.Add(mEntry);
				}
				if(mBuilder.Length > 0)
				{
					mEntry.Selectors.Add(mBuilder.ToString());
					CssUtil.Clear(mBuilder);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ProcessCharacter																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Process the provided character.
		/// </summary>
		/// <param name="value">
		/// The character to process.
		/// </param>
		private void ProcessCharacter(char value)
		{
			if(mInQuote != char.MinValue)
			{
				//	Working in quote.
				if(mInEscape != char.MinValue)
				{
					//	Any value is legitimate in escape.
					mInEscape = char.MinValue;
				}
				else if(value == mInQuote)
				{
					//	End quote.
					mInQuote = char.MinValue;
				}
				else if(value == '\\')
				{
					//	Estape.
					mInEscape = value;
				}
				//	Any character contributes to the string.
				Builder.Append(value);
			}
			else
			{
				//	Not in quote.
				if(value == '\'' || value == '"' || value == '`')
				{
					//	New quote starting.
					mInQuote = value;
					mInEscape = char.MinValue;
					Builder.Append(value);
				}
				else
				{
					//	This item is not a quote.
					switch(mCurrentFocus)
					{
						case CssPartEnum.Selector:
							//	Selector names.
							if(value == ',')
							{
								//	If we encountered a comma, create a new selector.
								PostSelector();
							}
							else if(value == '{')
							{
								//	End of selectors, start of entry.
								PostSelector();
								mCurrentFocus = CssPartEnum.Entry;
							}
							else
							{
								//	Non-comma. Building selector.
								if(mWhiteSpace.Contains(value))
								{
									//	This item is a whitespace.
									if(mBuilder.Length > 0)
									{
										//	The selector has already been started.
										if(!mLineEnd.Contains(value) && !mLastWasSpace)
										{
											//	A normal space indicates the item has inheritance
											//	specifications.
											mBuilder.Append(' ');
										}
									}
								}
								else
								{
									//	Item is not a whitespace.
									mBuilder.Append(value);
								}
							}
							break;
						case CssPartEnum.Entry:
							//	Entry area.
							if(value == '}')
							{
								//	End of entry.
								PostEntry();
								mCurrentFocus = CssPartEnum.Selector;
							}
							else if(!mWhiteSpace.Contains(value))
							{
								//	Start an attribute name.
								CssUtil.Clear(mBuilder);
								mBuilder.Append(value);
								mCurrentFocus = CssPartEnum.AttributeName;
							}
							break;
						case CssPartEnum.AttributeName:
							//	Attribute name.
							if(value == '}')
							{
								//	End of name-only attribute.
								PostEntry();
							}
							else if(mWhiteSpace.Contains(value))
							{
								//	No spaces allowed in name. This item will either
								//	move on to value after whitespace or will end
								//	the entry.
								PostAttributeName();
								mCurrentFocus = CssPartEnum.AttributeNameValue;
							}
							else if(value == ':')
							{
								//	Name:Value separator.
								PostAttributeName();
								mCurrentFocus = CssPartEnum.AttributeValue;
							}
							else
							{
								//	Accepted character.
								mBuilder.Append(value);
							}
							break;
						case CssPartEnum.AttributeNameValue:
							//	Space after name.
							if(value == ':')
							{
								mCurrentFocus = CssPartEnum.AttributeValue;
							}
							else if(value == '}')
							{
								PostEntry();
								mCurrentFocus = CssPartEnum.Selector;
							}
							break;
						case CssPartEnum.AttributeValue:
							//	Attribute value.
							if(value == ';')
							{
								//	End of value.
								PostAttributeValue();
								mCurrentFocus = CssPartEnum.Entry;
							}
							else if(value == '}')
							{
								//	End of entry.
								PostAttributeValue();
								mCurrentFocus = CssPartEnum.Selector;
							}
							else
							{
								//	Some portion of the value.
								mBuilder.Append(value);
							}
							break;
					}
				}
				mLastWasSpace = mWhiteSpace.Contains(value);
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	Attribute																															*
		//*-----------------------------------------------------------------------*
		private CssAttributeItem mAttribute = null;
		/// <summary>
		/// Get/Set a reference to the active CSS attribute.
		/// </summary>
		public CssAttributeItem Attribute
		{
			get { return mAttribute; }
			set { mAttribute = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Builder																																*
		//*-----------------------------------------------------------------------*
		private StringBuilder mBuilder = new StringBuilder();
		/// <summary>
		/// Get a reference to the local string builder.
		/// </summary>
		public StringBuilder Builder
		{
			get { return mBuilder; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Entries																																*
		//*-----------------------------------------------------------------------*
		private CssEntryCollection mEntries = null;
		/// <summary>
		/// Get/Set a reference to the collection being built.
		/// </summary>
		public CssEntryCollection Entries
		{
			get { return mEntries; }
			set { mEntries = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Entry																																	*
		//*-----------------------------------------------------------------------*
		private CssEntryItem mEntry = null;
		/// <summary>
		/// Get/Set a reference to the entry item currently in progress.
		/// </summary>
		public CssEntryItem Entry
		{
			get { return mEntry; }
			set { mEntry = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Process																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Process the provided CSS string.
		/// </summary>
		/// <param name="value">
		/// String value to parse.
		/// </param>
		public void Process(string value)
		{
			char[] chars = null;

			if(value?.Length > 0)
			{
				mCurrentFocus = CssPartEnum.Selector;
				chars = value.ToCharArray();
				mCharCount = chars.Length;
				mCharIndex = 0;
				for(mCharIndex = 0; mCharIndex < mCharCount; mCharIndex ++)
				{
					ProcessCharacter(chars[mCharIndex]);
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CssEntryCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CssEntryItem Items.
	/// </summary>
	public class CssEntryCollection : List<CssEntryItem>
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
		//* Parse																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the styles found in the caller's CSS string to entries and their
		/// corresponding attributes.
		/// </summary>
		/// <param name="styles">
		/// Style information to be parsed into working CSS.
		/// </param>
		/// <returns>
		/// CSS object model containing zero or more CSS entries and their
		/// child attributes.
		/// </returns>
		public static CssEntryCollection Parse(string styles)
		{
			CssEntryCollection result = new CssEntryCollection();
			CssEntryBuilder tracker = new CssEntryBuilder();

			if(styles?.Length > 0)
			{
				tracker.Entries = result;
				tracker.Process(styles);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this collection.
		/// </summary>
		/// <returns>
		/// String representation of this collection.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			string line = "";

			foreach(CssEntryItem entryItem in this)
			{
				line = entryItem.ToString();
				if(line.EndsWith('\r') || line.EndsWith('\n'))
				{
					builder.Append(line);
				}
				else
				{
					builder.AppendLine(line);
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CssEntryItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual CSS entry.
	/// </summary>
	public class CssEntryItem
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
		//*	Attributes																														*
		//*-----------------------------------------------------------------------*
		private CssAttributeCollection mAttributes = new CssAttributeCollection();
		/// <summary>
		/// Get a reference to the CSS attributes defined within this item.
		/// </summary>
		public CssAttributeCollection Attributes
		{
			get { return mAttributes; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Selectors																															*
		//*-----------------------------------------------------------------------*
		private List<string> mSelectors = new List<string>();
		/// <summary>
		/// Get a reference to the list of selectors for this entry.
		/// </summary>
		public List<string> Selectors
		{
			get { return mSelectors; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this item.
		/// </summary>
		/// <returns>
		/// String representation of this CSS entry.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if(mSelectors.Count > 0)
			{
				builder.AppendLine(string.Join(",\r\n", mSelectors));
				builder.AppendLine("{");
				foreach(CssAttributeItem attributeItem in this.mAttributes)
				{
					builder.Append(attributeItem.Name);
					builder.Append(": ");
					builder.Append(attributeItem.Value);
					builder.AppendLine(";");
				}
				builder.AppendLine("}\r\n");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
