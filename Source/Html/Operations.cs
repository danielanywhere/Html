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
	//*	Operations																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Comparison Operators.
	/// </summary>
	public enum Operations
	{
		/// <summary>
		/// Ignore this Value.
		/// </summary>
		Ignore,         //	00 - Ignore this value.
		/// <summary>
		/// Result is Between A and B.
		/// </summary>
		Between,        //	01 - BETWEEN a AND b
		/// <summary>
		/// Result is Equal to A.
		/// </summary>
		Equal,          //	02 - == a
		/// <summary>
		/// Result is Greater than A.
		/// </summary>
		Greater,        //	03 - > a
		/// <summary>
		/// Result is Greater than or Equal to A.
		/// </summary>
		GreaterEqual,   //	04 - >= a
		/// <summary>
		/// Result is one of the members of the provided set.
		/// </summary>
		In,             //	05 - IN (a,b,c)
		/// <summary>
		/// Result is Less Than A.
		/// </summary>
		Less,           //	06 - < a
		/// <summary>
		/// Result is Less Than or Equal To A.
		/// </summary>
		LessEqual,      //	07 - <= a
		/// <summary>
		/// Result is Not Equal to A.
		/// </summary>
		NotEqual,       //	08 - != a
		/// <summary>
		/// Result is not one of the members of the provided set.
		/// </summary>
		NotIn,          //	09 - NOT IN (a,b,c)
		/// <summary>
		/// Result Begins with A.
		/// </summary>
		BeginsWith,     //	10 - LEFT(LEN(a), a) = a
		/// <summary>
		/// Result Contains A.
		/// </summary>
		Contains,       //	11 - INSTR(a) > 0
		/// <summary>
		/// Result is Like A.
		/// </summary>
		Like,           //	12 - LIKE(a)
		/// <summary>
		/// Result is returned on result of Expression.
		/// </summary>
		Expression,     //	13 - This value is an Expression, and contains the
										//				proper comparison operators.
		/// <summary>
		/// Result is Not Like A.
		/// </summary>
		NotLike,        //	14 - NOT LIKE(a).
		/// <summary>
		/// Filter to Cart Contents.
		/// </summary>
		Cart,           //	15 - Cart Contents.
		/// <summary>
		/// Value Assignment. Used to specify parameters of a stored procedure.
		/// </summary>
		Assign          //	16 - Value Assignment. b = a.
	}
	//*-------------------------------------------------------------------------*
}
