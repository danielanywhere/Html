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
	//*	CssAttributeCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CssAttributeItem Items.
	/// </summary>
	public class CssAttributeCollection : List<CssAttributeItem>
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


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CssAttributeItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual CSS attribute specification.
	/// </summary>
	public class CssAttributeItem
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
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		private string mName = "";
		/// <summary>
		/// Get/Set the name of the attribute.
		/// </summary>
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of the attribute.
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
