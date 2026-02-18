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

using Html.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	RuleCollection																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of RuleItem Items.
	/// </summary>
	public class RuleCollection : List<RuleItem>
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
		//*	GetStyle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified style on the provided node.
		/// </summary>
		/// <param name="node">
		/// Reference to the node for which the style will be retrieved.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property entry to retrieve.
		/// </param>
		/// <returns>
		/// Value of the specified style.
		/// </returns>
		public string GetStyle(HtmlNodeItem node, string propertyName)
		{
			int count = 0;
			int index = 0;
			string result = "";
			char[] split = new char[] { ',' };
			RuleItem rule = null;
			string[] targets = null;

			if(node != null)
			{
				count = this.Count;
				for(index = count - 1; index > -1; index --)
				{
					rule = this[index];
					targets = rule.Name.Split(split);
					foreach(string targetItem in targets)
					{
						//	Check each available option.

					}
					if(rule.Name == propertyName)
					{
						//	The 
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	RuleItem																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual CSS rule definition.
	/// </summary>
	public class RuleItem
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
		//*	Styles																																*
		//*-----------------------------------------------------------------------*
		private NameValueCollection mEntries = new NameValueCollection();
		/// <summary>
		/// Get a reference to the collection of entries defined for this style.
		/// </summary>
		public NameValueCollection Styles
		{
			get { return mEntries; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		private string mName = "";
		/// <summary>
		/// Get/Set the name of this style.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
