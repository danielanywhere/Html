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

using static Html.HtmlUtil;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlAttributeCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of HtmlAttributeItem Items.
	/// </summary>
	public class HtmlAttributeCollection :
		ChangeObjectCollection<HtmlAttributeItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Font-relative CSS measurement units.
		/// </summary>
		private static string[] mFontRelativeCssMeasurements = new string[]
		{
				"ch", "em", "ex", "rem"
		};

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
				HtmlAttributeItem result = null;
				string nameLower = "";

				if(name?.Length > 0)
				{
					nameLower = name.ToLower();
					foreach(HtmlAttributeItem ai in this)
					{
						if(ai.Name.ToLower() == nameLower)
						{
							result = ai;
							break;
						}
					}
					if(result == null)
					{
						result = new HtmlAttributeItem()
						{
							Name = name
						};
						if(mAutoCreate)
						{
							this.Add(result);
						}
					}
				}
				return result;
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
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep copy of the caller's attribute collection.
		/// </summary>
		/// <param name="attributes">
		/// Reference to the collection of attributes to copy.
		/// </param>
		/// <returns>
		/// Reference to the newly create attributes clone, if a legitimate
		/// value was supplied. Otherwise, null.
		/// </returns>
		public HtmlAttributeCollection Clone(HtmlAttributeCollection attributes)
		{
			HtmlAttributeCollection result = null;

			if(attributes != null)
			{
				result = new HtmlAttributeCollection();
				foreach(HtmlAttributeItem attributeItem in attributes)
				{
					result.Add(HtmlAttributeItem.Clone(attributeItem));
				}
			}
			return result;
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
		//* GetActiveStyle																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the active value of the specified style in the current node or
		/// its ancestors.
		/// </summary>
		/// <param name="node">
		/// Reference to the current node to be tested.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to check for.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return in case no active value was found.
		/// </param>
		/// <returns>
		/// The active value for the specified style on the provided node or its
		/// ancestors, if found. Otherwise, the default value, if not null.
		/// Otherwise, an empty string.
		/// </returns>
		public static string GetActiveStyle(HtmlNodeItem node, string styleName,
			string defaultValue)
		{
			string defaultMeasure = "";
			float defaultNumber = 0f;
			Match match = null;
			string measure = "";
			string number = "";
			string result = "";
			string style = "";

			if(node != null && styleName?.Length > 0)
			{
				style = HtmlAttributeCollection.GetStyle(node, styleName);
				if(style.Length > 0)
				{
					match = Regex.Match(style, ResourceMain.rxCssNumberWithMeasure);
					if(match.Success)
					{
						number = HtmlUtil.GetValue(match, "number");
						measure = HtmlUtil.GetValue(match, "measure");
						if(measure.Length > 0)
						{
							if(mFontRelativeCssMeasurements.Contains(measure))
							{
								//	This is a font-relative measurement.
								if(node.ParentNode != null)
								{
									result = GetActiveStyle(node.ParentNode, "font-size", style);
									if(result.Length == 0 && defaultValue?.Length > 0)
									{
										result = defaultValue;
									}
									else
									{
										result = style;
									}
								}
								else
								{
									result = style;
								}
							}
							else
							{
								//	The style has been resolved locally.
								if(defaultValue?.Length > 2 &&
									mFontRelativeCssMeasurements.Contains(
										measure.Substring(measure.Length - 2)) &&
									styleName == "font-size")
								{
									//	Currently matching on a supplied relative font size.
									match = Regex.Match(defaultValue,
										ResourceMain.rxCssNumberWithMeasure);
									if(match.Success)
									{
										defaultNumber =
											ToFloat(HtmlUtil.GetValue(match, "number"));
										defaultMeasure = HtmlUtil.GetValue(match, "measure");
										switch(defaultMeasure)
										{
											case "ch":
												defaultNumber *= 0.5f;
												result = $"{ToFloat(number) * defaultNumber}{measure}";
												break;
											case "em":
											case "ex":
												result = $"{ToFloat(number) * defaultNumber}{measure}";
												break;
											case "rem":
												if(HasAncestorStyle(node, styleName))
												{
													result = GetActiveStyle(node.ParentNode, "font-size",
														defaultValue);
												}
												else
												{
													result = defaultValue;
												}
												break;
										}
									}
								}
								else
								{
									result = style;
								}
							}
						}
						else
						{
							result = number;
						}
					}
				}
				else if(node.ParentNode != null)
				{
					result = GetActiveStyle(node.ParentNode, styleName, defaultValue);
				}
				else if(defaultValue?.Length > 0)
				{
					result = defaultValue;
				}
				else
				{
					result = "";
				}
			}
			else if(defaultValue?.Length > 0)
			{
				result = defaultValue;
			}
			else
			{
				result = "";
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAttributes																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of attributes matching the specified names in
		/// the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the HTML node in which to search for the specified
		/// attributes.
		/// </param>
		/// <param name="attributeNames">
		/// Array of attribute names to find.
		/// </param>
		/// <returns>
		/// Reference to a list of attributes found in the provided node, if
		/// found. Otherwise, an empty list.
		/// </returns>
		public static List<HtmlAttributeItem> GetAttributes(HtmlNodeItem node,
			params string[] attributeNames)
		{
			List<HtmlAttributeItem> attribs = new List<HtmlAttributeItem>();
			HtmlAttributeItem attribute = null;

			if(node != null && node.Attributes.Count > 0 &&
				attributeNames?.Length > 0)
			{
				foreach(string attributeNameItem in attributeNames)
				{
					attribute = node.Attributes[attributeNameItem];
					if(attribute != null)
					{
						attribs.Add(attribute);
					}
				}
			}
			return attribs;
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
			string lowerName = "";
			string result = "";

			if(node != null && node.Attributes.Count > 0)
			{
				lowerName = attributeName.ToLower();
				attrib = node.Attributes.FirstOrDefault(x =>
					x.Name.ToLower() == lowerName);
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
		/// <param name="node">
		/// Reference to the node within which to find a style attribute.
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
			string result = "";

			if(node?.Attributes.Count > 0)
			{
				attribute = node.Attributes.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					result = HtmlAttributeItem.GetStyle(attribute, styleName);
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
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
			string lowerName = "";
			string result = "";

			if(attributeName?.Length > 0)
			{
				lowerName = attributeName.ToLower();
				attrib = this.FirstOrDefault(x => x.Name.ToLower() == lowerName);
				if(attrib != null)
				{
					result = attrib.Value;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasAncestorStyle																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this node's parent or one of its
		/// ancestors contain the specified style.
		/// </summary>
		/// <param name="node">
		/// Reference to the node whose ancestors will be inspected.
		/// </param>
		/// <param name="styleName">
		/// The name of the style to test for.
		/// </param>
		/// <returns>
		/// True if the node's parent or other ancestors contain the specified
		/// style. Otherwise, false.
		/// </returns>
		public static bool HasAncestorStyle(HtmlNodeItem node, string styleName)
		{
			bool result = false;

			if(node != null && styleName?.Length > 0 && node.ParentNode != null)
			{
				result = HtmlAttributeCollection.StyleExists(
					node.ParentNode.Attributes, styleName);
				if(!result)
				{
					result = HasAncestorStyle(node.ParentNode, styleName);
				}
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
			string lowerName = "";
			bool result = false;

			if(attributeName?.Length > 0)
			{
				lowerName = attributeName.ToLower();
				result = this.Exists(x => x.Name.ToLower() == lowerName);
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
		/// <param name="attributeName">
		/// Name of the attribute to remove.
		/// </param>
		public void Remove(string attributeName)
		{
			string lowerName = "";

			if(attributeName?.Length > 0)
			{
				lowerName = attributeName.ToLower();
				this.RemoveAll(x => x.Name.ToLower() == lowerName);
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
		//* RemoveStyle																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the specified style from the style attribute within the
		/// caller's collection.
		/// </summary>
		/// <param name="attributes">
		/// Reference to the collection of attributes within which to find the
		/// style item.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to remove.
		/// </param>
		public static void RemoveStyle(HtmlAttributeCollection attributes,
			string styleName)
		{
			StringBuilder builder = new StringBuilder();
			HtmlAttributeItem entry = null;
			HtmlAttributeItem style = null;
			HtmlAttributeCollection styles = null;

			if(attributes?.Count > 0 && styleName?.Length > 0)
			{
				style = attributes.FirstOrDefault(x => x.Name.ToLower() == "style");
				if(style != null)
				{
					styles = HtmlAttributeItem.GetStyles(style);
					entry = styles.FirstOrDefault(x =>
						x.Name.ToLower() == styleName.ToLower());
					if(entry != null)
					{
						styles.Remove(entry);
						if(styles.Count > 0)
						{
							//	Other styles still remain.
							foreach(HtmlAttributeItem attributeItem in styles)
							{
								if(attributeItem.Name.Length > 0)
								{
									if(builder.Length > 0)
									{
										builder.Append(';');
									}
									builder.Append(attributeItem.Name);
									if(attributeItem.Value.Length > 0)
									{
										builder.Append(':');
										builder.Append(attributeItem.Value);
									}
								}
							}
							style.Value = builder.ToString();
						}
						else
						{
							style.Value = "";
						}
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
				attribute = this[attributeName];
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
		//* SetAttributeValue																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified attribute from the provided node,
		/// creating it if it didn't already exist.
		/// </summary>
		/// <param name="node">
		/// Reference to the HTML node in which to search for the specified
		/// attribute.
		/// </param>
		/// <param name="attributeName">
		/// Name of the attribute to find.
		/// </param>
		/// <param name="attributeValue">
		/// The value to place in the attribute.
		/// </param>
		public static void SetAttributeValue(HtmlNodeItem node,
			string attributeName, string attributeValue)
		{
			HtmlAttributeItem attrib = null;

			if(node != null && node.Attributes.Count > 0)
			{
				attrib = node[attributeName];
				if(attrib == null)
				{
					attrib = new HtmlAttributeItem()
					{
						Name = attributeName
					};
					node.Attributes.Add(attrib);
				}
				attrib.Value = attributeValue;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value of the specified style in the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the node upon which the style will be set.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to set.
		/// </param>
		/// <param name="styleValue">
		/// Value of the style.
		/// </param>
		public static void SetStyle(HtmlNodeItem node, string styleName,
			string styleValue)
		{
			HtmlAttributeItem attribute = null;
			string value = "";

			if(node != null && styleName?.Length > 0)
			{
				attribute = node.Attributes.FirstOrDefault(x => x.Name == "style");
				value = (styleValue?.Length > 0 ? styleValue : "");
				if(attribute != null)
				{
					//	Style attribute exists.
					HtmlAttributeItem.SetStyle(attribute, styleName, value);
				}
				else
				{
					//	Style wasn't present on the node.
					node.Attributes.Add("style", $"{styleName}: {value};");
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Set the value of the specified style.
		/// </summary>
		/// <param name="styleName">
		/// Name of the style to set.
		/// </param>
		/// <param name="styleValue">
		/// Value of the style.
		/// </param>
		public void SetStyle(string styleName,
			string styleValue)
		{
			HtmlAttributeItem attribute = null;
			string value = "";

			if(styleName?.Length > 0)
			{
				attribute = this.FirstOrDefault(x => x.Name == "style");
				value = (styleValue?.Length > 0 ? styleValue : "");
				if(attribute != null)
				{
					//	Style attribute exists.
					HtmlAttributeItem.SetStyle(attribute, styleName, value);
				}
				else
				{
					//	Style wasn't present on the node.
					this.Add("style", $"{styleName}: {value};");
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* StyleExists																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified style exists in the
		/// supplied attributes collection.
		/// </summary>
		/// <param name="attributes">
		/// Reference to the collection of attributes to search.
		/// </param>
		/// <param name="styleName">
		/// Name of the style to search for.
		/// </param>
		/// <returns>
		/// True if the specified style is found within the given attributes
		/// collection. Otherwise, false.
		/// </returns>
		public static bool StyleExists(HtmlAttributeCollection attributes,
			string styleName)
		{
			HtmlAttributeItem attribute = null;
			bool result = false;
			HtmlAttributeCollection styles = null;

			if(attributes?.Count > 0 && styleName?.Length > 0)
			{
				attribute = attributes.FirstOrDefault(x => x.Name == "style");
				if(attribute != null)
				{
					styles = HtmlAttributeItem.GetStyles(attribute);
					result =
						styles.Exists(x => x.Name.ToLower() == styleName.ToLower());
				}
			}
			return result;
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
	public class HtmlAttributeItem : ChangeObjectItem
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
		//*	AssignmentSpace																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AssignmentSpace">AssignmentSpace</see>.
		/// </summary>
		private string mAssignmentSpace = "";
		/// <summary>
		/// Get/Set the space that was found with the assignment.
		/// </summary>
		public string AssignmentSpace
		{
			get { return mAssignmentSpace; }
			set
			{
				string original = mAssignmentSpace;

				mAssignmentSpace = value;
				if(mAssignmentSpace != original)
				{
					OnPropertyChanged("AssignmentSpace", original, value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep copy of the specified HTML attribute.
		/// </summary>
		/// <param name="attribute">
		/// Reference to the attribute to be copied.
		/// </param>
		/// <returns>
		/// Reference to a new clone of the caller's attribute.
		/// </returns>
		public static HtmlAttributeItem Clone(HtmlAttributeItem attribute)
		{
			HtmlAttributeItem result = null;

			if(attribute != null)
			{
				result = new HtmlAttributeItem()
				{
					mAssignmentSpace = attribute.mAssignmentSpace,
					mName = attribute.mName,
					mPresence = attribute.mPresence,
					mPreSpace = attribute.mPreSpace,
					mValue = attribute.mValue
				};
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

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
			HtmlAttributeCollection attributes = new HtmlAttributeCollection();
			HtmlAttributeItem attribute;
			MatchCollection matches = Regex.Matches(value.Value,
				@"(?<name>[^:]+)(:|$)\s*(?<value>[^;]+)*(;\s*|$)");
			string name = "";

			foreach(Match matchItem in matches)
			{
				name = GetValue(matchItem, "name");
				if(name.Length > 0)
				{
					attribute = attributes.Add();
					attribute.Name = name;
					attribute.Value = GetValue(matchItem, "value");
				}
			}
			return attributes;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Name">Name</see>.
		/// </summary>
		private string mName = "";
		/// <summary>
		/// Get/Set the Name of this Item.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set
			{
				string original = mName;

				mName = value;
				if(mName != original)
				{
					OnPropertyChanged("Name", original, value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Presence																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Presence">Presence</see>.
		/// </summary>
		private bool mPresence = false;
		/// <summary>
		/// Get/Set a value indicating whether this attribute is used for
		/// presence-only, as in the case of 'checked'.
		/// </summary>
		public bool Presence
		{
			get { return mPresence; }
			set
			{
				bool original = mPresence;

				mPresence = value;
				if(mPresence != original)
				{
					OnPropertyChanged("Presence", original, value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PreSpace																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PreSpace">PreSpace</see>.
		/// </summary>
		private string mPreSpace = "";
		/// <summary>
		/// Get/Set the space preserved prior to the beginning of this item.
		/// </summary>
		public string PreSpace
		{
			get { return mPreSpace; }
			set
			{
				string original = mPreSpace;

				mPreSpace = value;
				if(mPreSpace != original)
				{
					OnPropertyChanged("PreSpace", original, value);
				}
			}
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
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a string representation of this item.
		/// </summary>
		/// <returns>
		/// String representation of this attribute.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if(mName.Length > 0)
			{
				builder.Append(mName);
				builder.Append(':');
				if(mValue.Length > 0)
				{
					builder.Append(mValue);
				}
				else if(mPresence)
				{
					builder.Append("(presence)");
				}
				else
				{
					builder.Append("(empty)");
				}
			}
			else if(mValue.Length > 0)
			{
				builder.Append($"(no name):{mValue}");
			}
			else
			{
				builder.Append("(blank)");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Value">Value</see>.
		/// </summary>
		private string mValue = "";
		/// <summary>
		/// Get/Set the Value of this Item.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set
			{
				string original = mValue;

				mValue = value;
				if(mValue != original)
				{
					OnPropertyChanged("Value", original, value);
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
