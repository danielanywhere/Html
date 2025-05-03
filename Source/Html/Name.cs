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
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	NameCollection																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of NameItem Items.
	/// </summary>
	public class NameCollection : List<NameItem>
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
		//*	_Implicit string[] = NameCollection																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the Name Collection to a String Array.
		/// </summary>
		/// <param name="value">
		/// The Name Collection to convert to a String Array.
		/// </param>
		/// <returns>
		/// String Array containing Name Collection names.
		/// </returns>
		public static implicit operator string[](NameCollection value)
		{
			int lc = 0;
			int lp = 0;
			NameItem ni;
			string[] ro = null;

			if(value != null)
			{
				ro = new string[value.Count];
				lc = value.Count;
				for(lp = 0; lp < lc; lp++)
				{
					ni = value[lp];
					ro[lp] = ni.Name;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from the Collection by its Name.
		/// </summary>
		public NameItem this[string name]
		{
			get
			{
				NameItem ro = null;
				string tl = "";

				if(name?.Length > 0)
				{
					tl = name.ToLower();
					foreach(NameItem ni in this)
					{
						if(ni.Name.ToLower() == tl)
						{
							ro = ni;
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
		/// Create a new NameItem, add it to the Collection, and return it to the
		/// caller.
		/// </summary>
		/// <returns>
		/// Newly created and added NameItem.
		/// </returns>
		public NameItem Add()
		{
			NameItem ro = new NameItem();

			this.Add(ro);
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a collection of names and descriptions to the collection.
		/// </summary>
		/// <param name="value">
		/// Collection of Names and Descriptions to add.
		/// </param>
		public void Add(NameCollection value)
		{
			if(value != null)
			{
				foreach(NameItem ni in value)
				{
					Add(ni);
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create and add a new Name Item to the Collection, then return it to the
		/// caller.
		/// </summary>
		/// <param name="value">
		/// The Name for this Item.
		/// </param>
		/// <returns>
		/// Newly created and added NameItem.
		/// </returns>
		public NameItem Add(string value)
		{
			NameItem ro = Add();

			ro.Name = value;
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create and add a new Name Item to the Collection by member values.
		/// </summary>
		/// <param name="name">
		/// The Name for this Item.
		/// </param>
		/// <param name="description">
		/// Description of the Item.
		/// </param>
		/// <returns>
		/// Newly created and added NameItem.
		/// </returns>
		public NameItem Add(string name, string description)
		{
			NameItem ro = Add();

			ro.Name = name;
			ro.Description = description;
			return ro;
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
		//*	Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove the Item from the collection by Name.
		/// </summary>
		/// <param name="name">
		/// Name of the Item to remove.
		/// </param>
		public void Remove(string name)
		{
			NameItem ni = this[name];

			if(ni != null)
			{
				//	If the item exists, then remove it.
				this.Remove(ni);
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	NameItem																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Simple Item containing a Name and optional Description.
	/// </summary>
	public class NameItem
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
		//*	Description																														*
		//*-----------------------------------------------------------------------*
		private string mDescription = "";
		/// <summary>
		/// Get/Set the Description of this Name.
		/// </summary>
		public string Description
		{
			get { return mDescription; }
			set { mDescription = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		private string mName = "";
		/// <summary>
		/// Get/Set the Name of the Item.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the String equivalent of this Item.
		/// </summary>
		public override string ToString()
		{
			return mName;
		}
		//*-----------------------------------------------------------------------*
	}
	//*-------------------------------------------------------------------------*
}
