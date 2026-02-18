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
using System.Data;
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	IntCollection																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of int Items.
	/// </summary>
	public class IntCollection : List<int>
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
		//*	_Implicit int[] = IntCollection																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the int Collection to an int Array.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator int[](IntCollection value)
		{
			int i = 0;
			int[] ia = new int[value.Count];

			foreach(int ii in value)
			{
				ia[i] = ii;
				i++;
			}
			return ia;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit IntCollection = int[]																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the int Array to an int Collection.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator IntCollection(int[] value)
		{
			IntCollection ic = new IntCollection();

			foreach(int i in value)
			{
				ic.Add(i);
			}
			return ic;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Abs																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the absolute value of items in the collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to inspect.
		/// </param>
		/// <returns>
		/// Absolute value on all items.
		/// </returns>
		public static IntCollection Abs(IntCollection items)
		{
			IntCollection rv = new IntCollection();     //	Return Value.

			if(items != null)
			{
				foreach(int iv in items)
				{
					rv.Add(Math.Abs(iv));
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a Range of Cell Values from Table Rows to the Collection.
		/// </summary>
		/// <param name="rows">
		/// The Array of Rows Cell Values to add.
		/// </param>
		/// <param name="columnName">
		/// The name of the Column from which to extract the Cell Value.
		/// </param>
		public void AddRange(DataRow[] rows, string columnName)
		{
			if(rows != null && rows.Length != 0 && columnName.Length != 0)
			{
				//	If the table existed and had rows, then continue.
				DataColumn dc = rows[0].Table.Columns[columnName];
				int i = 0;        //	Working Int.

				if(dc != null)
				{
					//	We have a column from which to extract the data.
					foreach(DataRow dr in rows)
					{
						try
						{
							i = Convert.ToInt32(dr[dc.Ordinal]);
							this.Add(i);    //	Add the Item to the list if we're still in.
						}
						catch { }
					}
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a Range of Cell Values from Table Rows to the Collection.
		/// </summary>
		/// <param name="rows">
		/// The Rows Collection with Cell Values to add.
		/// </param>
		/// <param name="columnName">
		/// The name of the Column from which to extract the Cell Value.
		/// </param>
		public void AddRange(DataRowCollection rows, string columnName)
		{
			if(rows != null && rows.Count != 0 && columnName.Length != 0)
			{
				//	If the table existed and had rows, then continue.
				DataColumn dc = rows[0].Table.Columns[columnName];
				int i = 0;        //	Working Int.

				if(dc != null)
				{
					//	We have a column from which to extract the data.
					foreach(DataRow dr in rows)
					{
						try
						{
							i = Convert.ToInt32(dr[dc.Ordinal]);
							this.Add(i);    //	Add the Item to the list if we're still in.
						}
						catch { }
					}
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a Range of Cell Values from Table Rows to the Collection.
		/// </summary>
		/// <param name="table">
		/// The Table containing rows with Cell Values to add.
		/// </param>
		/// <param name="columnName">
		/// The name of the Column from which to extract the Cell Value.
		/// </param>
		public void AddRange(DataTable table, string columnName)
		{
			if(table != null && table.Rows.Count != 0 && columnName.Length != 0)
			{
				//	If the table existed and had rows, then continue.
				DataColumn dc = table.Columns[columnName];
				int i = 0;        //	Working Int.

				if(dc != null)
				{
					//	We have a column from which to extract the data.
					foreach(DataRow dr in table.Rows)
					{
						try
						{
							i = Convert.ToInt32(dr[dc.Ordinal]);
							this.Add(i);    //	Add the Item to the list if we're still in.
						}
						catch { }
					}
				}
			}
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Add a Range of Cell Values from a Collection of Object Arrays to the
		///// Collection.
		///// </summary>
		///// <param name="objects">
		///// Collection of Object Arrays, each containing a Cell Value to Add.
		///// </param>
		///// <param name="columnIndex">
		///// The Index of the Cell within each Object Array to add.
		///// </param>
		//public void AddRange(ObjectArrayCollection objects, int columnIndex)
		//{
		//	if(objects != null && objects.Count != 0 &&
		//		columnIndex >= 0 && columnIndex < objects[0].Length)
		//	{
		//		int i = 0;      //	Working Int.

		//		foreach(object[] oa in objects)
		//		{
		//			try
		//			{
		//				i = Convert.ToInt32(oa[columnIndex]);
		//				this.Add(i);    //	Add the Item to the list if we're still in.
		//			}
		//			catch { }
		//		}
		//	}
		//}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddRangeUnique																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a range of unique values to the current collection.
		/// </summary>
		/// <param name="value">
		/// Collection of integers containing potentially unique items to add to
		/// the local collection.
		/// </param>
		public void AddRangeUnique(IntCollection value)
		{
			foreach(int i in value)
			{
				AddUnique(i);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddUnique																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an int Value to the Collection, but only if it is unique.
		/// </summary>
		/// <param name="value">
		/// The Item to Add to the Collection, on the condition that it is unique.
		/// </param>
		public void AddUnique(int value)
		{
			bool bUnique = true;    //	By default, this value is unique.
			foreach(int i in this)
			{
				if(i == value)
				{
					bUnique = false;
					break;
				}
			}
			if(bUnique)
			{
				//	If the value is unique, then add it now.
				this.Add(value);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add an array of int Values to the Collection, but only where each is
		/// unique.
		/// </summary>
		/// <param name="values">
		/// The array of values to Add to the Collection, on the condition that
		/// each value is unique.
		/// </param>
		public void AddUnique(int[] values)
		{
			if(values != null)
			{
				foreach(int i in values)
				{
					AddUnique(i);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Average																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the average value of items in the collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to inspect.
		/// </param>
		/// <returns>
		/// Average value found in the collection.
		/// </returns>
		public static int Average(IntCollection items)
		{
			int rv = 0;     //	Return Value.

			if(items != null && items.Count > 0)
			{
				rv = Sum(items) / items.Count;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Contains																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified Integer is contained
		/// by the provided Int Collection.
		/// </summary>
		/// <param name="items">
		/// Instance of the IntCollection to search.
		/// </param>
		/// <param name="value">
		/// Value to search for.
		/// </param>
		/// <returns>
		/// Value indicating whether the specified Integer was found in the
		/// collection.
		/// </returns>
		public static bool Contains(IntCollection items, int value)
		{
			bool rv = false;

			foreach(int i in items)
			{
				if(i == value)
				{
					//	The value was found.
					rv = true;
					break;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetHighValue																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the High Value of Items in the specified Collection.
		/// </summary>
		/// <param name="value">
		/// Reference to the collection to search.
		/// </param>
		/// <returns>
		/// The high value found in the collection.
		/// </returns>
		public static int GetHighValue(IntCollection value)
		{
			int rv = 0;

			foreach(int i in value)
			{
				if(i > rv)
				{
					rv = i;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetLowValue																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Low Value of Items in the specified Collection.
		/// </summary>
		/// <param name="value">
		/// Reference to the collection to search.
		/// </param>
		/// <returns>
		/// The low value found in the collection.
		/// </returns>
		public static int GetLowValue(IntCollection value)
		{
			bool br = true;   //	Reset.
			int rv = 0;

			foreach(int i in value)
			{
				if(rv > i || br)
				{
					rv = i;
					br = false;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNextHigherValue																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the next higher value in the collection from that one specified
		/// by the caller.
		/// </summary>
		/// <param name="items">
		/// IntCollection to search.
		/// </param>
		/// <param name="value">
		/// The reference value for which the next higher value is requested.
		/// </param>
		/// <returns>
		/// If a higher value was found than the caller's value, then that higher
		/// value is returned. Otherwise, -1.
		/// </returns>
		public static int GetNextHigherValue(IntCollection items, int value)
		{
			bool br = true;   //	Reset.
			int rv = -1;

			if(value < GetHighValue(items))
			{
				//	If there is a next higher value, then continue.
				foreach(int i in items)
				{
					if(i > value && (i < rv || br))
					{
						rv = i;
						br = false;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Insert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Insert an Item into a sorted position within the Collection, moving
		/// all other values downward.
		/// </summary>
		/// <param name="value">
		/// The int value to Insert.
		/// </param>
		public void Insert(int value)
		{
			//			if(value != null)
			//			{
			int lc = this.Count;
			int lp = 0;

			for(lp = lc - 1; lp >= 0; lp--)
			{
				if(value < (int)this[lp] && (lp == 0 ||
					(lp > 0 && value >= (int)this[lp - 1])))
				{
					//	If this value is lower than the current item, and greater than
					//	or equal to the previous item, or less than the the item at
					//	the zero position, then let's insert it.
					Insert(value, lp);
				}
			}
			if(lp < 0)
			{
				//	If we went all the way up not finding something we were less
				//	than, then append.
				this.Add(value);
			}
			//			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Insert an Item into the specified Index of the Collection, moving all
		/// other values downward.
		/// </summary>
		/// <param name="value">
		/// The int value to Insert.
		/// </param>
		/// <param name="index">
		/// The Ordinal Index at which to insert the Item.
		/// </param>
		/// <remarks>
		/// If an illegal index is specified, the Item is simply appended to the
		/// end of the Collection.
		/// </remarks>
		public new void Insert(int value, int index)
		{
			//			if(value != null)
			//			{
			int lc = this.Count;
			int lp = 0;

			if(index != -1 && index < lc)
			{
				//	We have a legal index value. Move all other Items down.
				this.Add((int)-1);            //	Add a blank item at end.
				for(lp = lc; lp > index; lp--)
				{
					//	Work our way up the chain.
					this[lp] = this[lp - 1];    //	Move the item instance.
				}
				this[lp] = value;             //	Insert the Item.
			}
			else
			{
				//	If an illegal index was specified, then append the object.
				this.Add(value);
			}
			//			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ItemArray																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the array of Items in the Collection.
		/// </summary>
		public int[] ItemArray
		{
			get
			{
				int[] ia = new int[this.Count]; //	Default Return value is Zero
																				//	length array.
				int ip = 0;                         //	Item Position.
				foreach(int i in this)
				{
					ia[ip] = i;   //	Place the Value in the Cell.
					ip++;       //	Next Position.
				}
				return ia;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ItemExists																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether or not the specified value exists
		/// within the Collection.
		/// </summary>
		/// <param name="value">
		/// The Value to Search For.
		/// </param>
		/// <returns>
		/// A value indicating whether or not the specified value was found in the
		/// Collection.
		/// </returns>
		public bool ItemExists(int value)
		{
			bool rv = false;    //	By default, the item was not found.

			foreach(int i in this)
			{
				if(i == value)
				{
					rv = true;
					break;
				}
			}
			return rv;          //	Return the value to the caller.
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MemberCompare																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Compare the members of two Collections, and return a new Collection
		/// with the differences of each relative member.
		/// </summary>
		/// <param name="items1">
		/// Collection 1.
		/// </param>
		/// <param name="items2">
		/// Collection 2.
		/// </param>
		/// <returns>
		/// New Collection containing the differences of each member in the two
		/// Collections.
		/// </returns>
		public static IntCollection MemberCompare(IntCollection items1,
			IntCollection items2)
		{
			int lc = 0;       //	List Count.
			int lp = 0;       //	List Position.
			IntCollection rv = new IntCollection();
			if(items1 != null && items2 != null)
			{
				lc = Math.Max(items1.Count, items2.Count);
				for(lp = 0; lp < lc; lp++)
				{
					if(items1.Count > lp && items2.Count > lp)
					{
						rv.Add(items2[lp] - items1[lp]);
					}
					else if(items1.Count > lp)
					{
						rv.Add(0 - items1[lp]);
					}
					else
					{
						rv.Add(items2[lp]);
					}
				}
			}
			else if(items1 != null)
			{
				lc = items1.Count;
				for(lp = 0; lp < lc; lp++)
				{
					rv.Add(0 - items1[lp]);
				}
			}
			else if(items2 != null)
			{
				lc = items2.Count;
				for(lp = 0; lp < lc; lp++)
				{
					rv.Add(items2[lp]);
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Remove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove an Item from the Collection by its value.
		/// </summary>
		/// <param name="value">
		/// Value of the Item to remove.
		/// </param>
		/// <remarks>
		/// This method removes the first occurrence of the specified value.
		/// </remarks>
		public new void Remove(int value)
		{
			int lc = this.Count;
			int lp = 0;

			for(lp = 0; lp < lc; lp++)
			{
				if((int)this[lp] == value)
				{
					this.RemoveAt(lp);
					break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Sort																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Sort the contents of the Collection.
		/// </summary>
		/// <param name="value">
		/// Reference to the IntCollection to sort.
		/// </param>
		public static void Sort(IntCollection value)
		{
			int a = 0;    //	Left Working Value.
			int b = 0;    //	Right Working Value.
			int l = 0;    //	Lower Element Value.
			int h = 0;    //	Higher Element Value.

			for(a = 0; a < value.Count; a++)
			{
				for(b = 1; b < value.Count; b++)
				{
					l = (int)value[b - 1];
					h = (int)value[b];
					if(h < l)
					{
						value[b - 1] = h;
						value[b] = l;
					}
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Sort the contents of the Array.
		/// </summary>
		/// <param name="value">
		/// Reference to the int array to sort.
		/// </param>
		public static void Sort(int[] value)
		{
			int a = 0;    //	Left Working Value.
			int b = 0;    //	Right Working Value.
			int l = 0;    //	Lower Element Value.
			int h = 0;    //	Higher Element Value.

			for(a = 0; a < value.Length; a++)
			{
				for(b = 1; b < value.Length; b++)
				{
					l = value[b - 1];
					h = value[b];
					if(h < l)
					{
						value[b - 1] = h;
						value[b] = l;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Sum																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the sum of the caller's Int Collection.
		/// </summary>
		/// <param name="value">
		/// Reference to the collection to be summed.
		/// </param>
		/// <returns>
		/// The sum of all entries in the collection.
		/// </returns>
		public static int Sum(IntCollection value)
		{
			int rv = 0;

			if(value != null)
			{
				foreach(int ii in value)
				{
					rv += ii;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the String representation of the contents of this Collection.
		/// </summary>
		/// <returns>
		/// String representation of this Collection's contents.
		/// </returns>
		public override string ToString()
		{
			int i = 0;    //	Working List Item.
			int lc = 0;   //	List Count.
			int lp = 0;   //	List Position.
			StringBuilder sb = new StringBuilder();

			lc = this.Count;
			for(lp = 0; lp < lc; lp++)
			{
				i = (int)this[lp];
				if(lp != 0)
				{
					sb.Append(",");
				}
				sb.Append(i.ToString());
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
