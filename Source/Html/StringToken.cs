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
	//*	StringTokenCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of StringTokenItem Items.
	/// </summary>
	public class StringTokenCollection : List<StringTokenItem>
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
		//*	Original																															*
		//*-----------------------------------------------------------------------*
		private string mOriginal = "";
		/// <summary>
		/// Get/Set the original string from which these tokens have been parsed.
		/// </summary>
		public string Original
		{
			get { return mOriginal; }
			set { mOriginal = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	StringTokenItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual tokenized string.
	/// </summary>
	public class StringTokenItem
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
		//*	Length																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the length of the token.
		/// </summary>
		public int Length
		{
			get { return (mValue?.Length > 0 ? mValue.Length : 0); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartIndex																														*
		//*-----------------------------------------------------------------------*
		private int mStartIndex = 0;
		/// <summary>
		/// Get/Set the staring index of this item within the source content.
		/// </summary>
		public int StartIndex
		{
			get { return mStartIndex; }
			set { mStartIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the token value.
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
