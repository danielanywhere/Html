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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlAttributeCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of HtmlAttributeItem Items.
	/// </summary>
	public class HtmlAttributeCollection : List<HtmlAttributeItem>
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
		/// Create a new instance of the HtmlAttributeCollection Item.
		/// </summary>
		public HtmlAttributeCollection()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the HtmlAttributeCollection Item.
		/// </summary>
		/// <param name="autoCreateItemsOnAccess">
		/// Value indicating whether to automatically create new elements if they
		/// do not exist when referenced.
		/// </param>
		public HtmlAttributeCollection(bool autoCreateItemsOnAccess)
		{
			mAutoCreate = autoCreateItemsOnAccess;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from the Collection by its name.
		/// </summary>
		public HtmlAttributeItem this[string name]
		{
			get
			{
				HtmlAttributeItem ro = null;
				string tl = name.ToLower();

				foreach(HtmlAttributeItem ai in this)
				{
					if(ai.Name.ToLower() == tl)
					{
						ro = ai;
						break;
					}
				}
				if(ro == null)
				{
					ro = new HtmlAttributeItem()
					{
						Name = name
					};
					if(mAutoCreate)
					{
						this.Add(ro);
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
		/// Create a new HtmlAttributeItem, add it to the Collection, and return it
		/// to the caller.
		/// </summary>
		/// <returns>
		/// Newly created and added HtmlAttributeItem.
		/// </returns>
		public HtmlAttributeItem Add()
		{
			HtmlAttributeItem ro = new HtmlAttributeItem();

			this.Add(ro);
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new HtmlAttributeItem, add it to the Collection, and return it
		/// to the caller.
		/// </summary>
		/// <param name="name">
		/// Name of the Attribute to add.
		/// </param>
		/// <returns>
		/// Newly created and added HtmlAttributeItem.
		/// </returns>
		public HtmlAttributeItem Add(string name)
		{
			HtmlAttributeItem ro = this.Add();
			ro.Name = name;
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new HtmlAttributeItem, add it to the Collection, and return it
		/// to the caller.
		/// </summary>
		/// <param name="name">
		/// Name of the Attribute to add.
		/// </param>
		/// <param name="value">
		/// Value of the Attribute to add.
		/// </param>
		/// <returns>
		/// Newly created and added HtmlAttributeItem.
		/// </returns>
		public HtmlAttributeItem Add(string name, string value)
		{
			HtmlAttributeItem ro = this.Add();
			ro.Name = name;
			ro.Value = value;
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddClass																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a class reference to the class attribute, if unique.
		/// </summary>
		/// <param name="className">
		/// Name of the class reference to add.
		/// </param>
		public void AddClass(string className)
		{
			HtmlAttributeItem attribute = null;
			int count = 0;
			int index = 0;
			char[] space = new char[] { ' ' };
			List<string> values = null;

			if(className?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "class");
				if(attribute == null)
				{
					attribute = new HtmlAttributeItem()
					{
						Name = "class"
					};
					this.Add(attribute);
				}
				values = attribute.Value.Split(space).ToList();
				count = values.Count;
				for(index = 0; index < count; index ++)
				{
					if(values[index] == null || values[index].Length == 0)
					{
						values.RemoveAt(index);
						index--;
						count--;
					}
				}
				if(!values.Contains(className))
				{
					values.Add(className);
					attribute.Value = string.Join(" ", values);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddUnique																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an Attribute to the Collection if it has a unique name.
		/// </summary>
		/// <param name="name">
		/// Name of the attribute to add.
		/// </param>
		/// <returns>
		/// Instance of the Attribute found or created.
		/// </returns>
		public HtmlAttributeItem AddUnique(string name)
		{
			HtmlAttributeItem ro = this[name];
			if(ro == null)
			{
				ro = Add(name);
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AutoCreate																														*
		//*-----------------------------------------------------------------------*
		private bool mAutoCreate = false;
		/// <summary>
		/// Get/Set a value indicating whether elements will be created
		/// automatically if they don't exist when accessed by index.
		/// </summary>
		public bool AutoCreate
		{
			get { return mAutoCreate; }
			set { mAutoCreate = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ContainsAny																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this collection contains any of the
		/// specified names.
		/// </summary>
		/// <param name="names">
		/// Array of names to be searched for.
		/// </param>
		/// <returns>
		/// True if any of the items in this collection have at least one of the
		/// specified names. False otherwise.
		/// </returns>
		public bool ContainsAny(string[] names)
		{
			bool rv = false;    //	No matches, by default.

			foreach(string s in names)
			{
				if(this[s] != null)
				{
					rv = true;
					break;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAttributeValue																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified attribute from the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the HTML node in which to search for the specified
		/// attribute.
		/// </param>
		/// <param name="attributeName">
		/// Name of the attribute to find.
		/// </param>
		/// <returns>
		/// Value of the specified attribute, if found. Otherwise, an empty string.
		/// </returns>
		public static string GetAttributeValue(HtmlNodeItem node,
			string attributeName)
		{
			HtmlAttributeItem attrib = null;
			string result = "";

			if(node != null && node.Attributes.Count > 0)
			{
				attrib = node.Attributes.FirstOrDefault(x => x.Name == attributeName);
				if(attrib != null)
				{
					result = attrib.Value;
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
		/// <param name="styleName">
		/// Name of the style property to return.
		/// </param>
		/// <returns>
		/// Value of the specified style property, if found. Otherwise, an
		/// empty string.
		/// </returns>
		public string GetStyle(string styleName)
		{
			HtmlAttributeItem attribute = null;
			string result = "";

			if(this.Count > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					result = HtmlAttributeItem.GetStyle(attribute, styleName);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified attribute from the list.
		/// </summary>
		/// <param name="attributeName">
		/// Name of the attribute to find.
		/// </param>
		/// <returns>
		/// Value of the specified attribute, if found. Otherwise, an empty string.
		/// </returns>
		public string GetValue(string attributeName)
		{
			HtmlAttributeItem attrib = null;
			string result = "";

			attrib = this.FirstOrDefault(x => x.Name == attributeName);
			if(attrib != null)
			{
				result = attrib.Value;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HasAttribute																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified attribute is defined in
		/// this collection.
		/// </summary>
		/// <param name="attributeName">
		/// Name of the attribute to search for.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified attribute is defined in the
		/// collection.
		/// </returns>
		public bool HasAttribute(string attributeName)
		{
			bool result = false;

			if(attributeName?.Length > 0)
			{
				result = this.Exists(x => x.Name == attributeName);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HasClass																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified class is defined in
		/// this collection.
		/// </summary>
		/// <param name="className">
		/// Name of the class to search for.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified class is defined in the class
		/// attribute of this collection.
		/// </returns>
		public bool HasClass(string className)
		{
			HtmlAttributeItem attribute = null;
			bool result = false;
			char[] space = new char[] { ' ' };
			List<string> values = null;

			if(className?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "class");
				if(attribute != null)
				{
					values = attribute.Value.Split(space).ToList();
					result = values.Contains(className);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	InsertClass																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert a class reference in the specified index of the class attribute.
		/// </summary>
		/// <param name="itemIndex">
		/// Index at which to insert the class reference.
		/// </param>
		/// <param name="className">
		/// Name of the class reference to insert.
		/// </param>
		public void InsertClass(int itemIndex, string className)
		{
			HtmlAttributeItem attribute = null;
			int count = 0;
			int index = 0;
			char[] space = new char[] { ' ' };
			List<string> values = null;

			if(className?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "class");
				if(attribute == null)
				{
					attribute = new HtmlAttributeItem()
					{
						Name = "class"
					};
					this.Add(attribute);
				}
				values = attribute.Value.Split(space).ToList();
				count = values.Count;
				for(index = 0; index < count; index++)
				{
					if(values[index] == null || values[index].Length == 0)
					{
						values.RemoveAt(index);
						index--;
						count--;
					}
				}
				if(values.Contains(className))
				{
					values.Remove(className);
				}
				values.Insert(itemIndex, className);
				attribute.Value = string.Join(" ", values);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the named attribute.
		/// </summary>
		/// <param name="name">
		/// Name of the attribute to remove.
		/// </param>
		public void Remove(string name)
		{
			int lc = 0;   //	List Count.
			int lp = 0;   //	List Position.

			lc = this.Count;
			for(lp = 0; lp < lc; lp++)
			{
				if(this[lp].Name == name)
				{
					this.RemoveAt(lp);
					break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RemoveClass																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove a class reference from the class attribute, if present.
		/// </summary>
		/// <param name="className">
		/// Name of the class reference to remove.
		/// </param>
		public void RemoveClass(string className)
		{
			HtmlAttributeItem attribute = null;
			char[] space = new char[] { ' ' };
			List<string> values = null;

			if(className?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "class");
				if(attribute != null)
				{
					values = attribute.Value.Split(space).ToList();
					if(values.Contains(className))
					{
						values.Remove(className);
						attribute.Value = string.Join(" ", values);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetAttribute																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified attribute in the provided node.
		/// </summary>
		/// <param name="attributeName">
		/// Name of the attribute property to return.
		/// </param>
		/// <param name="value">
		/// Value of the attribute.
		/// </param>
		public void SetAttribute(string attributeName,
			string value)
		{
			HtmlAttributeItem attribute = null;

			if(attributeName?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == attributeName);
				if(attribute != null)
				{
					attribute.Value = value;
				}
				else
				{
					//	Attribute didn't yet exist on node.
					this.Add(attributeName, value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified style in the provided node.
		/// </summary>
		/// <param name="styleName">
		/// Name of the style property to return.
		/// </param>
		/// <param name="value">
		/// Value of the style.
		/// </param>
		public void SetStyle(string styleName,
			string value)
		{
			HtmlAttributeItem attribute = null;

			if(styleName?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					//	Style attribute exists.
					HtmlAttributeItem.SetStyle(attribute, styleName, value);
				}
				else
				{
					//	Style didn't yet exist on node.
					this.Add("style", $"{styleName}: {value};");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Unquoted																															*
		//*-----------------------------------------------------------------------*
		private static HtmlAttributeCollection mUnquoted = null;
		/// <summary>
		/// Get a list of unquoted Attributes.
		/// </summary>
		/// <remarks>
		/// This property is activated on the first get.
		/// </remarks>
		public static HtmlAttributeCollection Unquoted
		{
			get
			{
				if(mUnquoted == null)
				{
					mUnquoted = new HtmlAttributeCollection(false);
					//					mUnquoted.Add("class");
					//					mUnquoted.Add("id");
				}
				return mUnquoted;
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	HtmlAttributeItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Definition of an Attribute Name and Value.
	/// </summary>
	public class HtmlAttributeItem
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
		//* GetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified style in the provided attribute.
		/// </summary>
		/// <param name="attribute">
		/// Reference to the attribute to inspect.
		/// </param>
		/// <param name="styleName">
		/// Name of the style property to return.
		/// </param>
		/// <returns>
		/// Value of the specified style property, if found. Otherwise, an
		/// empty string.
		/// </returns>
		public static string GetStyle(HtmlAttributeItem attribute,
			string styleName)
		{
			string[] parts = null;
			string result = "";
			string[] styles = null;

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

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetStyles																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the list of Name: Value styles as separate entries from the
		/// provided style-type attribute.
		/// </summary>
		/// <param name="value">
		/// Attribute containing Style entries.
		/// </param>
		/// <returns>
		/// Enumerated Collection of Styles.
		/// </returns>
		/// <remarks>
		/// The syntax of the Style collection is Name1: Value1; NameN: ValueN[;]
		/// </remarks>
		public static HtmlAttributeCollection GetStyles(HtmlAttributeItem value)
		{
			HtmlAttributeCollection nc = new HtmlAttributeCollection();
			HtmlAttributeItem ni;
			MatchCollection mc = Regex.Matches(value.Value,
				@"(?<n>[^:]+)(:|$)\s*(?<v>[^;]+)*(;\s*|$)");

			foreach(Match m in mc)
			{
				if(m.Groups["n"] != null)
				{
					ni = nc.Add();
					ni.Name = m.Groups["n"].Value;
					if(m.Groups["v"] != null)
					{
						ni.Value = m.Groups["v"].Value;
					}
				}
			}
			return nc;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		private string mName = "";
		/// <summary>
		/// Get/Set the Name of this Item.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Presence																															*
		//*-----------------------------------------------------------------------*
		private bool mPresence = false;
		/// <summary>
		/// Get/Set a value indicating whether this attribute is used for
		/// presence-only, as in the case of 'checked'.
		/// </summary>
		public bool Presence
		{
			get { return mPresence; }
			set { mPresence = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified style in the provided attribute.
		/// </summary>
		/// <param name="attribute">
		/// Reference to the style attribute to inspect.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to select.
		/// </param>
		/// <param name="value">
		/// Value of the style.
		/// </param>
		public static void SetStyle(HtmlAttributeItem attribute, string styleName,
			string value)
		{
			string attributeValue = (value?.Length > 0 ? value : "");
			StringBuilder builder = new StringBuilder();
			int index = 0;
			string[] parts = null;
			NameItem property = null;
			string[] styles = null;
			List<NameItem> values = new List<NameItem>();

			if(attribute != null && styleName?.Length > 0)
			{
				//	Style attribute found.
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
					property.Description = attributeValue;
				}
				else
				{
					values.Add(new NameItem()
					{
						Name = styleName,
						Description = attributeValue
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
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the Value of this Item.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
