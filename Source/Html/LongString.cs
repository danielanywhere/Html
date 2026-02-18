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
	//*	LongString																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Special String handling class for text-type data strings.
	/// </summary>
	public class LongString : IConvertible
	{
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new Instance of the LongString Item.
		/// </summary>
		public LongString()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new Instance of the LongString Item.
		/// </summary>
		/// <param name="value">
		/// Initial String value.
		/// </param>
		public LongString(string value)
		{
			mValue = value;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit string = LongString																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the Long String to a String.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator string(LongString value)
		{
			string rs = "";

			if(value != null)
			{
				rs = value.Value;
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//		//*-----------------------------------------------------------------------*
		//		//*	_Implicit LongString = object																					*
		//		//*-----------------------------------------------------------------------*
		//		/// <summary>
		//		/// Cast the Object to a Long String.
		//		/// </summary>
		//		/// <param name="value">
		//		/// The Value to cast.
		//		/// </param>
		//		/// <returns>
		//		/// Converted Value.
		//		/// </returns>
		//		public static implicit operator LongString(object value)
		//		{
		//			LongString ro = new LongString();
		//
		//			if(value != null)
		//			{
		//				ro.Value = value.ToString();
		//			}
		//			return ro;
		//		}
		//		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit LongString = string																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the String to a Long String.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator LongString(string value)
		{
			LongString ro = new LongString();

			if(value != null)
			{
				ro.Value = value;
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTypeCode																														*
		//*-----------------------------------------------------------------------*
		//	The following code is required to implement the IConvertible interface.
		/// <summary>
		/// Return the type code of this object.
		/// </summary>
		/// <returns>
		/// The suitable system type code of this object.
		/// </returns>
		public System.TypeCode GetTypeCode()
		{
			return System.TypeCode.String;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToBoolean																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate boolean representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A boolean representation of this value.
		/// </returns>
		public bool ToBoolean(System.IFormatProvider provider)
		{
			return Conversion.ToBoolean(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToByte																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual byte representation of this
		/// object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A byte representation of this value.
		/// </returns>
		public byte ToByte(System.IFormatProvider provider)
		{
			byte bt = 0;
			char[] ca = this.Value.ToCharArray();

			if(ca.Length != 0)
			{
				bt = (byte)ca[0];
			}
			return bt;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToChar																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual character representation of this
		/// object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A character representation of this value.
		/// </returns>
		public char ToChar(System.IFormatProvider provider)
		{
			char c = '\0';
			char[] ca = this.Value.ToCharArray();

			if(ca.Length != 0)
			{
				c = ca[0];
			}
			return c;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDateTime																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual date and time representation of
		/// this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A date and time representation of this value.
		/// </returns>
		public DateTime ToDateTime(System.IFormatProvider provider)
		{
			return Conversion.ToDateTime(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDecimal																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual decimal representation of this
		/// object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A decimal representation of this value.
		/// </returns>
		public decimal ToDecimal(System.IFormatProvider provider)
		{
			return Conversion.ToDecimal(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDouble																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual double representation of this
		/// object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A double representation of this value.
		/// </returns>
		public double ToDouble(System.IFormatProvider provider)
		{
			return Conversion.ToDouble(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt16																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 16-bit signed integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 16-bit signed integer representation of this value.
		/// </returns>
		public Int16 ToInt16(System.IFormatProvider provider)
		{
			return 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt32																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 32-bit signed integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer representation of this value.
		/// </returns>
		public Int32 ToInt32(System.IFormatProvider provider)
		{
			return Conversion.ToInt32(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt64																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 64-bit signed integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 64-bit signed integer representation of this value.
		/// </returns>
		public Int64 ToInt64(System.IFormatProvider provider)
		{
			return Conversion.ToInt64(this.Value);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToSByte																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 8-bit signed integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// An 8-bit signed integer representation of this value.
		/// </returns>
		public SByte ToSByte(System.IFormatProvider provider)
		{
			return 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToSingle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual single floating point
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A single floating point representation of this value.
		/// </returns>
		public Single ToSingle(System.IFormatProvider provider)
		{
			return Conversion.ToSingle(this.Value, 0);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this instance.
		/// </summary>
		/// <returns>
		/// Value property of this instance.
		/// </returns>
		public override string ToString()
		{
			return mValue;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the string representation of this instance.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// Value property of this instance.
		/// </returns>
		public string ToString(System.IFormatProvider provider)
		{
			return this.Value;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToType																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the system type that best represents the supplied data type.
		/// </summary>
		/// <param name="type">
		/// Reference to the data type to test.
		/// </param>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// Reference to the system data type that best represents the specified
		/// data type.
		/// </returns>
		public object ToType(System.Type type, System.IFormatProvider provider)
		{
			return Conversion.ToDataType(this.Value, type);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToUInt16																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 16-bit unsigned integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 16-bit unsigned integer representation of this value.
		/// </returns>
		public UInt16 ToUInt16(System.IFormatProvider provider)
		{
			return 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToUInt32																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 32-bit unsigned integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 32-bit unsigned integer representation of this value.
		/// </returns>
		public UInt32 ToUInt32(System.IFormatProvider provider)
		{
			return (UInt32)Conversion.ToInt32(this.Value, 0);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToUInt64																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the most appropriate individual 64-bit unsigned integer
		/// representation of this object.
		/// </summary>
		/// <param name="provider">
		/// Format provider.
		/// </param>
		/// <returns>
		/// A 64-bit unsigned integer representation of this value.
		/// </returns>
		public UInt64 ToUInt64(System.IFormatProvider provider)
		{
			return (UInt64)Conversion.ToInt64(this.Value, 0);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the Value of this instance.
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
