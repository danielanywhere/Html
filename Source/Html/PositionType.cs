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
	//*	PositionType																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Positioning Modes.
	/// </summary>
	public enum PositionType
	{
		/// <summary>
		/// No Position Type specified.
		/// </summary>
		None,
		/// <summary>
		/// Default positioning mode. Follows Html Layout rules.
		/// </summary>
		Static,
		/// <summary>
		/// Positioned relative to Document, using Left and Top values.
		/// </summary>
		Absolute,
		/// <summary>
		/// Positioned relative to normal flow, then offset by Left and Top values.
		/// </summary>
		Relative
	}
	//*-------------------------------------------------------------------------*
}
