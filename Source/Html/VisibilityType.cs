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
	//*	VisibilityType																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Visibilitiy Values.
	/// </summary>
	public enum VisibilityType
	{
		/// <summary>
		/// No Visibility specified.
		/// </summary>
		None,
		/// <summary>
		/// Visibility is inherited from Parent.
		/// </summary>
		Inherit,
		/// <summary>
		/// Item is Visible.
		/// </summary>
		Visible,
		/// <summary>
		/// Item is Hidden.
		/// </summary>
		Hidden
	}
	//*-------------------------------------------------------------------------*
}
