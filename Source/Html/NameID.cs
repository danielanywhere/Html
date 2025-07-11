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
	//*	NameIDCollection																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of NameIDItem Items.
	/// </summary>
	public class NameIDCollection : List<NameIDItem>, IDisposable
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
		//*	_Destructor																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Instance is being finalized without call to dispose.
		/// </summary>
		~NameIDCollection()
		{
			Dispose(false);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get an Item from the Collection by its Name.
		/// </summary>
		public NameIDItem this[string name]
		{
			get
			{
				NameIDItem ro = null;

				foreach(NameIDItem ii in this)
				{
					if(ii.Name == name)
					{
						ro = ii;
						break;
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
		/// Create a new NameIDItem, add it to the Collection, and return it to the
		/// caller.
		/// </summary>
		/// <returns>
		/// Newly created and added NameIDItem.
		/// </returns>
		public NameIDItem Add()
		{
			NameIDItem ro = new NameIDItem();

			this.Add(ro);
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new Item to the Collection by Name and ID.
		/// </summary>
		/// <param name="name">
		/// The Name of the Item to Add.
		/// </param>
		/// <param name="id">
		/// The ID of the Item to Add.
		/// </param>
		/// <returns>
		/// Newly created and added Item.
		/// </returns>
		public NameIDItem Add(string name, int id)
		{
			NameIDItem ro = new NameIDItem()
			{
				Name = name,
				ID = id
			};
			this.Add(ro);

			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new Item to the Collection by member values.
		/// </summary>
		/// <param name="name">
		/// The Name of the Item to Add.
		/// </param>
		/// <param name="id">
		/// The ID of the Item to Add.
		/// </param>
		/// <param name="tag">
		/// Object referenced from this item.
		/// </param>
		/// <returns>
		/// Newly created and added Item.
		/// </returns>
		public NameIDItem Add(string name, int id, object tag)
		{
			NameIDItem ro = new NameIDItem()
			{
				Name = name,
				ID = id,
				Tag = tag
			};

			this.Add(ro);

			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Contains																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this collection contains the
		/// specified ID.
		/// </summary>
		/// <param name="ID">
		/// ID to search for.
		/// </param>
		/// <returns>
		/// True if the ID is found within the collection. Otherwise, false.
		/// </returns>
		public bool Contains(int ID)
		{
			bool rv = false;

			foreach(NameIDItem ni in this)
			{
				if(ni.ID == ID)
				{
					rv = true;
					break;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CountOf																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Count of occurrences of the specified Name.
		/// </summary>
		/// <param name="name">
		/// Name to search for.
		/// </param>
		/// <returns>
		/// Number of instances found.
		/// </returns>
		public int CountOf(string name)
		{
			int rv = 0;

			foreach(NameIDItem ni in this)
			{
				if(ni.Name == name)
				{
					rv++;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Dispose																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Value indicating whether the instance has already been disposed.
		/// </summary>
		private bool mbDisposed = false;
		/// <summary>
		/// Dispose the item and its resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			//	Remove us from the finalization queue to prevent subsequent
			//	finalizations.
			GC.SuppressFinalize(this);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Dispose the collection and its resources.
		/// </summary>
		/// <param name="disposing">
		/// Value indicating whether Dispose has been called from User Code (true),
		/// or from the internal finalizer (false).
		/// </param>
		public virtual void Dispose(bool disposing)
		{
			if(!mbDisposed)
			{
				//	If the item hasn't already been disposed, then do so now.
				mbDisposed = true;
				if(disposing)
				{
					//	Application call.
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetLast																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the last instance of the specified Name.
		/// </summary>
		/// <param name="name">
		/// Name to search for.
		/// </param>
		/// <returns>
		/// NameIDItem representing the last of the specified Name in the
		/// Collection, if found. Otherwise, null.
		/// </returns>
		public NameIDItem GetLast(string name)
		{
			int lp = 0;             //	List Position.
			NameIDItem ni;
			NameIDItem ro = null;

			for(lp = this.Count - 1; lp >= 0; lp--)
			{
				ni = (NameIDItem)this[lp];
				if(ni.Name == name)
				{
					//	If the name matches, then return it.
					ro = ni;
					break;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNames																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of values matching the specified Name.
		/// </summary>
		/// <param name="name">
		/// Name to search for.
		/// </param>
		/// <returns>
		/// Array of NameIDItem values having the specified Name, if the name was
		/// found. Otherwise, a zero length array.
		/// </returns>
		public NameIDItem[] GetNames(string name)
		{
			int nc = 0;     //	Name Count.
			int np = 0;     //	Name Position.
			NameIDItem[] nia = new NameIDItem[0];

			foreach(NameIDItem ni in this)
			{
				if(ni.Name == name)
				{
					nc++;
				}
			}
			if(nc > 0)
			{
				nia = new NameIDItem[nc];
				np = 0;
				foreach(NameIDItem ni in this)
				{
					if(ni.Name == name)
					{
						nia[np++] = ni;
					}
				}
			}
			return nia;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ID																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Item having the specified ID from the Collection.
		/// </summary>
		/// <param name="id">
		/// ID of the Item to search for.
		/// </param>
		/// <returns>
		/// NameIDItem having the specified ID, if found. Otherwise, null.
		/// </returns>
		public NameIDItem ID(int id)
		{
			NameIDItem ro = null;

			foreach(NameIDItem nid in this)
			{
				if(nid.ID == id)
				{
					ro = nid;
					break;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this Collection.
		/// </summary>
		/// <returns>
		/// String representation of Collection contents.
		/// </returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach(NameIDItem ni in this)
			{
				if(sb.Length != 0)
				{
					sb.Append("\r\n");
				}
				sb.Append("ID: " + ni.ID.ToString().PadLeft(3, '0'));
				sb.Append(" ");
				sb.Append("Name: " + ni.Name);
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	NameIDItem																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A generic Item containing a Name and an ID.
	/// </summary>
	public class NameIDItem
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
		//*	ID																																		*
		//*-----------------------------------------------------------------------*
		private int mID = 0;
		/// <summary>
		/// Get/Set the Unique ID of this Item.
		/// </summary>
		public int ID
		{
			get { return mID; }
			set { mID = value; }
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
		//*	Tag																																		*
		//*-----------------------------------------------------------------------*
		private object mTag = null;
		/// <summary>
		/// Get/Set a reference to an object used for tracking or managing this
		/// Item.
		/// </summary>
		public object Tag
		{
			get { return mTag; }
			set { mTag = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the String equivalent of this Item.
		/// </summary>
		/// <returns>
		/// String representation of this item.
		/// </returns>
		public override string ToString()
		{
			return mName;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
