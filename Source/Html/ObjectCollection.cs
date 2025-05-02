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
	//*	ObjectCollection																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of object Items.
	/// </summary>
	public class ObjectCollection : List<object>
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
		//*	_Implicit int[] = ObjectCollection																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the Object Collection instance to an Integer Array.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator int[](ObjectCollection value)
		{
			int lp = 0;             //	List Position.
			int[] ra = new int[0];

			if(value != null)
			{
				ra = new int[value.Count];
				foreach(object o in value)
				{
					try
					{
						ra[lp] = Convert.ToInt32(o);
					}
					catch
					{
						ra[lp] = 0;
					}
				}
			}
			return ra;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit object[] = ObjectCollection																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the Object Collection instance to an Object Array.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator object[](ObjectCollection value)
		{
			int lp = 0;             //	List Position.
			object[] ra = new object[0];

			if(value != null)
			{
				ra = new object[value.Count];
				foreach(object o in value)
				{
					ra[lp++] = o;
				}
			}
			return ra;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddRange																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a range of objects to the Collection.
		/// </summary>
		/// <param name="value">
		/// Generic Array.
		/// </param>
		public void AddRange(Array value)
		{
			if(value != null)
			{
				foreach(object o in value)
				{
					this.Add(o);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AddUnique																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an object to the Collection if it has a unique value.
		/// </summary>
		/// <param name="value">
		/// The object to add to the Collection if it is unique.
		/// </param>
		/// <returns>
		/// A value indicating whether or not the value was added.
		/// </returns>
		public bool AddUnique(object value)
		{
			bool rv = false;
			foreach(object o in this)
			{
				if((bool)Conversion.CommonEquals(value, o))
				{
					rv = true;
					break;
				}
			}
			if(rv)
			{
				//	If we found the item, then it is not unique. Return false.
				rv = false;
			}
			else
			{
				//	If we didn't find the item, then it is unique. Add it and return
				//	true.
				this.Add(value);
				rv = true;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Exists																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified object exists in the
		/// collection.
		/// </summary>
		/// <param name="value">
		/// Object to compare for.
		/// </param>
		/// <returns>
		/// True if the specified Object exists in the Collection. Otherwise,
		/// false.
		/// </returns>
		public bool Exists(object value)
		{
			bool rv = false;

			foreach(object o in this)
			{
				if(o.Equals(value))
				{
					rv = true;
					break;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
