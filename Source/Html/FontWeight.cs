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
	//*	FontWeight																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Font Weight Values.
	/// </summary>
	public enum FontWeight
	{
		/// <summary>
		/// No Font Weight specified.
		/// </summary>
		None,
		/// <summary>
		/// Normal.
		/// </summary>
		Normal,
		/// <summary>
		/// Bold.
		/// </summary>
		Bold,
		/// <summary>
		/// Heavier than Bold.
		/// </summary>
		Bolder,
		/// <summary>
		/// Lighter than Normal.
		/// </summary>
		Lighter,
		/// <summary>
		/// At least as light as 200 Weight.
		/// </summary>
		w100,
		/// <summary>
		/// At least as light as 300 Weight. At least as heavy as 100 Weight.
		/// </summary>
		w200,
		/// <summary>
		/// At least as light as 400 Weight. At least as heavy as 200 Weight.
		/// </summary>
		w300,
		/// <summary>
		/// Normal.
		/// </summary>
		w400,
		/// <summary>
		/// At least as light as 600 Weight. At least as heavy as 400 Weight.
		/// </summary>
		w500,
		/// <summary>
		/// At least as light as 700 Weight. At least as heavy as 500 Weight.
		/// </summary>
		w600,
		/// <summary>
		/// Bold.
		/// </summary>
		w700,
		/// <summary>
		/// At least as light as 900 Weight. At least as heavy as 700 Weight.
		/// </summary>
		w800,
		/// <summary>
		/// At least as heavy as 800 Weight.
		/// </summary>
		w900
	}
	//*-------------------------------------------------------------------------*
}
