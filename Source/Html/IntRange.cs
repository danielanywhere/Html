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
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	IntRange																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Definition of a contiguous range of Int values.
	/// </summary>
	public class IntRange
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
		/// Create a new Instance of the IntRange Item.
		/// </summary>
		public IntRange()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new Instance of the IntRange Item.
		/// </summary>
		/// <param name="startValue">
		/// Starting Value of the Range.
		/// </param>
		/// <param name="endValue">
		/// Ending Value of the Range.
		/// </param>
		public IntRange(int startValue, int endValue)
		{
			mStartValue = startValue;
			mEndValue = endValue;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	And																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the logical AND result of the two specified Int Ranges.
		/// </summary>
		/// <param name="range1">
		/// First operand.
		/// </param>
		/// <param name="range2">
		/// Second operand.
		/// </param>
		/// <returns>
		/// If the two Ranges had at least some value in common, then the
		/// mutually specified values between them, specified as a new IntRange
		/// object. Otherwise, null.
		/// </returns>
		public static IntRange And(IntRange range1, IntRange range2)
		{
			IntRange ro = null;

			if(range1.StartValue <= range2.StartValue &&
				range1.EndValue > range2.EndValue)
			{
				//	If here, then value 1 starts on or before value 2, and ends
				//	after value 2 starts. Value 2 is the starting reference, and the
				//	least of the end values of the ending reference.
				if(range1.EndValue > range2.EndValue)
				{
					//	If value 1 ends after value 2, then value 2 end.
					ro = new IntRange(range2.StartValue, range2.EndValue);
				}
				else
				{
					//	Otherwise, value 1 end.
					ro = new IntRange(range2.StartValue, range1.EndValue);
				}
			}
			else if(range2.StartValue <= range1.StartValue &&
				range2.EndValue > range1.StartValue)
			{
				//	If here, then value 2 starts on or before value 1, and ends
				//	after value 1 starts. Value 1 is the starting reference, and the
				//	least of the end values is the ending reference.
				if(range2.EndValue > range1.EndValue)
				{
					//	If value 2 ends after value 1, then value 1 end.
					ro = new IntRange(range1.StartValue, range1.EndValue);
				}
				else
				{
					//	Otherwise, value 2 end.
					ro = new IntRange(range1.StartValue, range2.EndValue);
				}
			}
			if(ro != null)
			{
				//	If a return value exists, then attempt to associate tags.
				if(range1.Tag != null)
				{
					//	If the first value has a tag, then use it.
					ro.Tag = range1.Tag;
				}
				else if(range2.Tag != null)
				{
					//	Otherwise, if the second value has a tag, then use it.
					ro.Tag = range2.Tag;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AndFit																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating the type of fit the two specified Ranges
		/// will yield if logically ANDed together.
		/// </summary>
		/// <param name="range1">
		/// First operand.
		/// </param>
		/// <param name="range2">
		/// Second operand.
		/// </param>
		/// <returns>
		/// If the two Ranges had at least some value in common, then
		/// RangePart.All or RangePart.Part. Otherwise, RangePart.None.
		/// </returns>
		public static RangePart AndFit(IntRange range1, IntRange range2)
		{
			RangePart rv = RangePart.None;

			if(range1.StartValue == range2.StartValue &&
				range1.EndValue == range2.EndValue)
			{
				rv = RangePart.All;
			}
			else
			{
				if(range1.StartValue <= range2.StartValue &&
					range1.EndValue > range2.StartValue)
				{
					//	If here, then value 1 starts on or before value 2, and ends
					//	after value 2 starts. Value 2 is the starting reference, and the
					//	least of the end value is the ending reference.
					rv = RangePart.Part;
				}
				else if(range2.StartValue <= range1.StartValue &&
					range2.EndValue > range1.StartValue)
				{
					//	If here, then value 2 starts on or before value 1, and ends
					//	after value 1 starts. Value 1 is the starting reference, and the
					//	least of the end values is the ending reference.
					rv = RangePart.Part;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Contains																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified value is implicitly
		/// contained within the range.
		/// </summary>
		/// <param name="item">
		/// Range to inspect.
		/// </param>
		/// <param name="value">
		/// Value to compare.
		/// </param>
		/// <returns>
		/// True if the specified value is within the range. False otherwise.
		/// </returns>
		public static bool Contains(IntRange item, int value)
		{
			bool rv = false;

			if(item != null)
			{
				rv = (value >= item.StartValue && value <= item.EndValue);
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndValue																															*
		//*-----------------------------------------------------------------------*
		private int mEndValue = 0;
		/// <summary>
		/// Get/Set the Inclusive End Value of this Item.
		/// </summary>
		public int EndValue
		{
			get { return mEndValue; }
			set { mEndValue = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FitsInside																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether this Range will fit inside the
		/// specified Int Range.
		/// </summary>
		/// <param name="value">
		/// The value to test for fit.
		/// </param>
		/// <returns>
		/// Value indicating whether this Range will fit completely inside the
		/// specified Int Range.
		/// </returns>
		public bool FitsInside(IntRange value)
		{
			bool rv = false;

			if(value != null)
			{
				if(this.StartValue >= value.StartValue &&
					this.EndValue <= value.EndValue)
				{
					//	If this range fits, then return success to the caller.
					rv = true;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetHighBound																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the closest value greater than or equal to the specified value
		/// to find.
		/// </summary>
		/// <param name="values">
		/// Array of integers to search.
		/// </param>
		/// <param name="find">
		/// Value to find.
		/// </param>
		/// <returns>
		/// The closest value greater than or equal to the specified find value,
		/// if found. Otherwise, -1.
		/// </returns>
		/// <remarks>
		/// This method assumes that all members of the values array are sequential
		/// in value, but not necessarily incremental. For example, 1, 3, 5, but
		/// not 1, 5, 3.
		/// </remarks>
		public static int GetHighBound(int[] values, int find)
		{
			int rv = -1;

			if(values != null)
			{
				foreach(int i in values)
				{
					if(i >= find)
					{
						rv = i;
						break;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetLowBound																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the closest value lower than or equal to the specified value to
		/// find.
		/// </summary>
		/// <param name="values">
		/// Array of integers to search.
		/// </param>
		/// <param name="find">
		/// Value to find.
		/// </param>
		/// <returns>
		/// The closest value lower than or equal to the specified find value, if
		/// found. Otherwise, -1.
		/// </returns>
		/// <remarks>
		/// This method assumes that all members of the values array are sequential
		/// in value, but not necessarily incremental. For example, 1, 3, 5, but
		/// not 1, 5, 3.
		/// </remarks>
		public static int GetLowBound(int[] values, int find)
		{
			int rv = -1;

			if(values != null)
			{
				foreach(int i in values)
				{
					if(i <= find)
					{
						rv = i;
					}
					else
					{
						break;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Length																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the implied length of this range.
		/// </summary>
		public int Length
		{
			get { return (mEndValue - mStartValue) + 1; }
			set { mEndValue = (mStartValue + value) - 1; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartValue																														*
		//*-----------------------------------------------------------------------*
		private int mStartValue = 0;
		/// <summary>
		/// Get/Set the Inclusive Start Value of this Item.
		/// </summary>
		public int StartValue
		{
			get { return mStartValue; }
			set { mStartValue = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Tag																																		*
		//*-----------------------------------------------------------------------*
		private object mTag = null;
		/// <summary>
		/// Get/Set an object to associate with this Range.
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
		/// Return the string representation of this instance.
		/// </summary>
		/// <returns>
		/// String representation of the range, from the start value to the end
		/// value.
		/// </returns>
		public override string ToString()
		{
			return mStartValue.ToString() + " - " + mEndValue.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	IntRangeCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of IntRange Items.
	/// </summary>
	public class IntRangeCollection : List<IntRange>
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
		//*	Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new IntRange, add it to the Collection, and return it to
		/// the caller.
		/// </summary>
		/// <returns>
		/// Newly created and added IntRange.
		/// </returns>
		public IntRange Add()
		{
			IntRange ro = new IntRange();

			this.Add(ro);
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Add a new item to the collection by Start and End Values.
		/// </summary>
		/// <param name="startValue">
		/// The inclusive start value of the range.
		/// </param>
		/// <param name="endValue">
		/// The inclusive end value of the range.
		/// </param>
		/// <returns>
		/// Newly created and added Int Range Item.
		/// </returns>
		public IntRange Add(int startValue, int endValue)
		{
			IntRange ro = this.Add();

			ro.StartValue = startValue;
			ro.EndValue = endValue;
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Contains																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified value is implicitly
		/// contained within any of the ranges in this collection.
		/// </summary>
		/// <param name="value">
		/// Value to compare.
		/// </param>
		/// <returns>
		/// True if the specified value is within any of the ranges in the
		/// collection. False otherwise.
		/// </returns>
		public bool Contains(int value)
		{
			bool rv = false;

			foreach(IntRange ri in this)
			{
				if(IntRange.Contains(ri, value))
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
