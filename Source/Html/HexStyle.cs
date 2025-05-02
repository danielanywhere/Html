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
	//*	HexStyle																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Hexadecimal Layout Styles.
	/// </summary>
	[Flags]
	public enum HexStyle
	{
		/// <summary>
		/// Use the Default Hex Layout.
		/// </summary>
		Default = 0x0000,
		/// <summary>
		/// Return the value as a single Hexadecimal Key.
		/// </summary>
		/// <remarks>
		/// This value can be used in conjunction with GUID and Prefix modifiers.
		/// </remarks>
		SingleKey = 0x0001,
		/// <summary>
		/// Return the value as delimited bytes.
		/// </summary>
		/// <remarks>
		/// This value can be used in conjunction with Prefix, Space, and Comma
		/// modifiers.
		/// </remarks>
		Delimited = 0x0002,
		/// <summary>
		/// Return the Single Key value formatted as a GUID
		/// as in <i>{5d99525d-4e27-4a48-b097-b7f587418a3e}</i>.
		/// </summary>
		GUID = 0x0004,
		/// <summary>
		/// Return the value with the Hex prefix (<i>0x</i>).
		/// </summary>
		Prefix = 0x0008,
		/// <summary>
		/// Separate Delimited Values with a space.
		/// </summary>
		Space = 0x0010,
		/// <summary>
		/// Seperate Delimited Values with a comma.
		/// </summary>
		Comma = 0x0020,
		/// <summary>
		/// Input is in numeric form.
		/// </summary>
		Numeric = 0x0040,
		/// <summary>
		/// Values are expressed as single bytes.
		/// </summary>
		Byte = 0x0080,
		/// <summary>
		/// Values are expressed as double-byte words.
		/// </summary>
		Word = 0x0100,
		/// <summary>
		/// Values are expressed as double-words.
		/// </summary>
		DoubleWord = 0x0200,
		/// <summary>
		/// Fill the return string with the full size of the input value.
		/// </summary>
		/// <remarks>
		/// If the Input Value is Int32, then setting the Fill flag on the style
		/// will cause a 4 byte value to be filled. In conjunction with the Byte,
		/// Word, and Double word settings, either 4, 2, or 1 values will be
		/// present, respectively.
		/// </remarks>
		Fill = 0x0400
	}
	//*-------------------------------------------------------------------------*
}
