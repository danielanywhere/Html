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
	//*	CssPartEnum																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of known CSS parts.
	/// </summary>
	public enum CssPartEnum
	{
		/// <summary>
		/// No part specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Selector area.
		/// </summary>
		Selector,
		/// <summary>
		/// Internal entry area, before, between, and after attributes.
		/// </summary>
		Entry,
		/// <summary>
		/// Individual attribute name.
		/// </summary>
		AttributeName,
		/// <summary>
		/// Space between attribute name and value.
		/// </summary>
		AttributeNameValue,
		/// <summary>
		/// Individual attribute value.
		/// </summary>
		AttributeValue
	}
	//*-------------------------------------------------------------------------*

}
