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

		//	The following code is required to implement the IConvertible interface.
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public System.TypeCode GetTypeCode()
		{
			return System.TypeCode.String;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public bool ToBoolean(System.IFormatProvider provider)
		{
			return Conversion.ToBoolean(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public DateTime ToDateTime(System.IFormatProvider provider)
		{
			return Conversion.ToDateTime(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public decimal ToDecimal(System.IFormatProvider provider)
		{
			return Conversion.ToDecimal(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public double ToDouble(System.IFormatProvider provider)
		{
			return Conversion.ToDouble(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public Int16 ToInt16(System.IFormatProvider provider)
		{
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public Int32 ToInt32(System.IFormatProvider provider)
		{
			return Conversion.ToInt32(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public Int64 ToInt64(System.IFormatProvider provider)
		{
			return Conversion.ToInt64(this.Value);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public SByte ToSByte(System.IFormatProvider provider)
		{
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public Single ToSingle(System.IFormatProvider provider)
		{
			return Conversion.ToSingle(this.Value, 0);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public string ToString(System.IFormatProvider provider)
		{
			return this.Value;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="provider"></param>
		/// <returns></returns>
		public object ToType(System.Type type, System.IFormatProvider provider)
		{
			return Conversion.ToDataType(this.Value, type);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public UInt16 ToUInt16(System.IFormatProvider provider)
		{
			return 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public UInt32 ToUInt32(System.IFormatProvider provider)
		{
			return (UInt32)Conversion.ToInt32(this.Value, 0);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public UInt64 ToUInt64(System.IFormatProvider provider)
		{
			return (UInt64)Conversion.ToInt64(this.Value, 0);
		}

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

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string value of this instance.
		/// </summary>
		/// <returns>
		/// Value property of this instance.
		/// </returns>
		public override string ToString()
		{
			return mValue;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
