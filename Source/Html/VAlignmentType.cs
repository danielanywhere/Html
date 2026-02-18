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
	//*	VAlignmentType																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Vertical Alignment Types.
	/// </summary>
	public enum VAlignmentType
	{
		/// <summary>
		/// No alignment specified.
		/// </summary>
		None,
		/// <summary>
		/// Center Text Vertically.
		/// </summary>
		Middle,
		/// <summary>
		/// Align base of current object with base of adjacent objects.
		/// </summary>
		Baseline,
		/// <summary>
		/// Items are aligned at the bottom of the cell.
		/// </summary>
		Bottom,
		/// <summary>
		/// Items are aligned at the top of the cell.
		/// </summary>
		Top
	}
	//*-------------------------------------------------------------------------*
}
