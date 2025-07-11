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
#define NoWindowsOnly

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;

#if WindowsOnly
//	Make sure to include System.Common.Drawing 6.0 (Windows ONLY!).
#endif

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	Conversion																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Handles common forms of data conversion.
	/// </summary>
	public class Conversion
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private static string[] mSelfClosingTags = new string[]
		{
			"area",
			"base",
			"br",
			"col",
			"command",
			"embed",
			"hr",
			"img",
			"input",
			"keygen",
			"link",
			"menuitem",
			"meta",
			"param",
			"source",
			"track",
			"wbr"
		};
		private static SysType[,] mSysTypeConversion = new SysType[,]
		{
			//	Unknown.
			{SysType.Unknown, SysType.Unknown, SysType.Boolean,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.Unknown,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.Image, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Type,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Null.
			{SysType.Null, SysType.Null, SysType.Boolean,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.Null,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.Image, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Type,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Boolean.
			{SysType.Boolean, SysType.Boolean, SysType.Boolean,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.Boolean,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Boolean,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Byte.
			{SysType.Byte, SysType.Byte, SysType.Byte,
			SysType.Byte, SysType.ByteArray, SysType.Byte,
			SysType.CharArray, SysType.DateTime, SysType.Byte,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.Byte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Byte,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	ByteArray.
			{SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray},
			//	Char.
			{SysType.Char, SysType.Char, SysType.Char,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.Char,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Char,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	CharArray.
			{SysType.CharArray, SysType.CharArray, SysType.CharArray,
			SysType.CharArray, SysType.ByteArray, SysType.CharArray,
			SysType.CharArray, SysType.CharArray, SysType.CharArray,
			SysType.CharArray, SysType.CharArray, SysType.CharArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.LongString,
			SysType.CharArray, SysType.ByteArray, SysType.String,
			SysType.StringArray, SysType.ByteArray, SysType.CharArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray},
			//	DateTime.
			{SysType.DateTime, SysType.DateTime, SysType.DateTime,
			SysType.DateTime, SysType.ByteArray, SysType.DateTime,
			SysType.CharArray, SysType.DateTime, SysType.DateTime,
			SysType.DateTime, SysType.Double, SysType.DateTime,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.DateTime, SysType.DateTime, SysType.String,
			SysType.StringArray, SysType.DateTime, SysType.DateTime,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	DBNull.
			{SysType.DBNull, SysType.DBNull, SysType.Boolean,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.DBNull,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.Image, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Type,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Decimal.
			{SysType.Decimal, SysType.Decimal, SysType.Decimal,
			SysType.Decimal, SysType.ByteArray, SysType.Decimal,
			SysType.CharArray, SysType.DateTime, SysType.Decimal,
			SysType.Decimal, SysType.Double, SysType.Decimal,
			SysType.Guid, SysType.ByteArray, SysType.Decimal,
			SysType.Decimal, SysType.Decimal, SysType.LongString,
			SysType.Decimal, SysType.Decimal, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Decimal,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Double.
			{SysType.Double, SysType.Double, SysType.Double,
			SysType.Double, SysType.ByteArray, SysType.Double,
			SysType.CharArray, SysType.Double, SysType.Double,
			SysType.Double, SysType.Double, SysType.Double,
			SysType.Guid, SysType.ByteArray, SysType.Double,
			SysType.Double, SysType.Double, SysType.LongString,
			SysType.Double, SysType.Double, SysType.String,
			SysType.StringArray, SysType.Double, SysType.Double,
			SysType.Double, SysType.Double, SysType.Double},
			//	Enum.
			{SysType.Enum, SysType.Enum, SysType.Enum,
			SysType.Enum, SysType.ByteArray, SysType.Enum,
			SysType.CharArray, SysType.DateTime, SysType.Enum,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Enum,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Guid.
			{SysType.Guid, SysType.Guid, SysType.Guid,
			SysType.Guid, SysType.ByteArray, SysType.Guid,
			SysType.ByteArray, SysType.Guid, SysType.Guid,
			SysType.Guid, SysType.Guid, SysType.Guid,
			SysType.Guid, SysType.ByteArray, SysType.Guid,
			SysType.Guid, SysType.Guid, SysType.LongString,
			SysType.Guid, SysType.Guid, SysType.String,
			SysType.StringArray, SysType.ByteArray, SysType.Guid,
			SysType.Guid, SysType.Guid, SysType.Guid},
			//	Image.
			{SysType.Image, SysType.Image, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.Image,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.Image, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray,
			SysType.ByteArray, SysType.ByteArray, SysType.ByteArray},
			//	Int16.
			{SysType.Int16, SysType.Int16, SysType.Int16,
			SysType.Int16, SysType.ByteArray, SysType.Int16,
			SysType.ByteArray, SysType.Int16, SysType.Int16,
			SysType.Int16, SysType.Double, SysType.Int16,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.Int16, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Int16,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Int32.
			{SysType.Int32, SysType.Int32, SysType.Int32,
			SysType.Int32, SysType.ByteArray, SysType.Int32,
			SysType.ByteArray, SysType.Int32, SysType.Int32,
			SysType.Decimal, SysType.Double, SysType.Int32,
			SysType.Guid, SysType.ByteArray, SysType.Int32,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.Int32, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Int32,
			SysType.Int32, SysType.UInt32, SysType.UInt64},
			//	Int64.
			{SysType.Int64, SysType.Int64, SysType.Int64,
			SysType.Int64, SysType.ByteArray, SysType.Int64,
			SysType.ByteArray, SysType.Int64, SysType.Int64,
			SysType.Int64, SysType.Double, SysType.Int64,
			SysType.Guid, SysType.ByteArray, SysType.Int64,
			SysType.Int64, SysType.Int64, SysType.LongString,
			SysType.Int64, SysType.Int64, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Int64,
			SysType.Int64, SysType.Int64, SysType.UInt64},
			//	LongString.
			{SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.ByteArray, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString,
			SysType.LongString, SysType.LongString, SysType.LongString},
			//	SByte.
			{SysType.SByte, SysType.SByte, SysType.SByte,
			SysType.Byte, SysType.ByteArray, SysType.SByte,
			SysType.CharArray, SysType.DateTime, SysType.SByte,
			SysType.Decimal, SysType.Double, SysType.SByte,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.SByte,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	Single.
			{SysType.Single, SysType.Single, SysType.Single,
			SysType.Single, SysType.ByteArray, SysType.Single,
			SysType.ByteArray, SysType.DateTime, SysType.Single,
			SysType.Decimal, SysType.Double, SysType.Single,
			SysType.Guid, SysType.ByteArray, SysType.Single,
			SysType.Single, SysType.Int64, SysType.LongString,
			SysType.Single, SysType.Single, SysType.String,
			SysType.StringArray, SysType.Single, SysType.Single,
			SysType.Single, SysType.Single, SysType.UInt64},
			//	String.
			{SysType.String, SysType.String, SysType.String,
			SysType.String, SysType.ByteArray, SysType.String,
			SysType.String, SysType.String, SysType.String,
			SysType.String, SysType.String, SysType.String,
			SysType.String, SysType.ByteArray, SysType.String,
			SysType.String, SysType.String, SysType.LongString,
			SysType.String, SysType.String, SysType.String,
			SysType.StringArray, SysType.String, SysType.String,
			SysType.String, SysType.String, SysType.String},
			//	StringArray.
			{SysType.StringArray, SysType.StringArray, SysType.StringArray,
			SysType.StringArray, SysType.ByteArray, SysType.StringArray,
			SysType.StringArray, SysType.StringArray, SysType.StringArray,
			SysType.StringArray, SysType.StringArray, SysType.StringArray,
			SysType.StringArray, SysType.ByteArray, SysType.StringArray,
			SysType.StringArray, SysType.StringArray, SysType.LongString,
			SysType.StringArray, SysType.StringArray, SysType.StringArray,
			SysType.StringArray, SysType.StringArray, SysType.StringArray,
			SysType.StringArray, SysType.StringArray, SysType.StringArray},
			//	TimeSpan.
			{SysType.TimeSpan, SysType.TimeSpan, SysType.TimeSpan,
			SysType.TimeSpan, SysType.ByteArray, SysType.TimeSpan,
			SysType.ByteArray, SysType.DateTime, SysType.TimeSpan,
			SysType.TimeSpan, SysType.Double, SysType.TimeSpan,
			SysType.ByteArray, SysType.ByteArray, SysType.TimeSpan,
			SysType.TimeSpan, SysType.TimeSpan, SysType.LongString,
			SysType.TimeSpan, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.TimeSpan,
			SysType.TimeSpan, SysType.TimeSpan, SysType.UInt64},
			//	Type.
			{SysType.Type, SysType.Type, SysType.Boolean,
			SysType.Byte, SysType.ByteArray, SysType.Char,
			SysType.CharArray, SysType.DateTime, SysType.Type,
			SysType.Decimal, SysType.Double, SysType.Enum,
			SysType.Guid, SysType.ByteArray, SysType.Int16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.SByte, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.Type,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	UInt16.
			{SysType.UInt16, SysType.UInt16, SysType.UInt16,
			SysType.UInt16, SysType.ByteArray, SysType.UInt16,
			SysType.ByteArray, SysType.UInt16, SysType.UInt16,
			SysType.UInt16, SysType.Double, SysType.UInt16,
			SysType.Guid, SysType.ByteArray, SysType.UInt16,
			SysType.Int32, SysType.Int64, SysType.LongString,
			SysType.UInt16, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.UInt16,
			SysType.UInt16, SysType.UInt32, SysType.UInt64},
			//	UInt32.
			{SysType.UInt32, SysType.UInt32, SysType.UInt32,
			SysType.UInt32, SysType.ByteArray, SysType.UInt32,
			SysType.ByteArray, SysType.UInt32, SysType.UInt32,
			SysType.UInt32, SysType.Double, SysType.UInt32,
			SysType.Guid, SysType.ByteArray, SysType.UInt32,
			SysType.UInt32, SysType.Int64, SysType.LongString,
			SysType.UInt32, SysType.Single, SysType.String,
			SysType.StringArray, SysType.TimeSpan, SysType.UInt32,
			SysType.UInt32, SysType.UInt32, SysType.UInt64},
			//	UInt64.
			{SysType.UInt64, SysType.UInt64, SysType.UInt64,
			SysType.UInt64, SysType.ByteArray, SysType.UInt64,
			SysType.ByteArray, SysType.UInt64, SysType.UInt64,
			SysType.UInt64, SysType.Double, SysType.UInt64,
			SysType.Guid, SysType.ByteArray, SysType.UInt64,
			SysType.UInt64, SysType.UInt64, SysType.LongString,
			SysType.UInt64, SysType.UInt64, SysType.String,
			SysType.StringArray, SysType.UInt64, SysType.UInt64,
			SysType.UInt64, SysType.UInt64, SysType.UInt64},
		};
		/// <summary>
		/// Verbose Ones between zero and nineteen.
		/// </summary>
		private static string[] mvOnespot = new string[]
		{
			"Zero", "One",
			"Two", "Three",
			"Four", "Five",
			"Six", "Seven",
			"Eight", "Nine",
			"Ten", "Eleven",
			"Twelve", "Thirteen",
			"Fourteen", "Fifteen",
			"Sixteen", "Seventeen",
			"Eighteen", "Nineteen"
		};
		/// <summary>
		/// Verbose Tens between zero and ninety.
		/// </summary>
		private static string[] mvTenspot = new string[]
		{
			"", "_",
			"Twenty", "Thirty",
			"Fourty", "Fifty",
			"Sixty", "Seventy",
			"Eighty", "Ninety"
		};
		/// <summary>
		/// Verbose Hundreds up to the Million context.
		/// </summary>
		private static string[] mvHundredspot = new string[]
		{ "", "", "Hundred",
			"Thousand", "10", "100",
			"Million", "10", "100",
			"Billion", "10", "100",
			"Trillion", "10", "100"
		};

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	CanConvertToBoolean																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the value can be converted to a
		/// Boolean Type.
		/// </summary>
		/// <param name="value">
		/// The Value to Inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the Conversion is possible.
		/// </returns>
		public static bool CanConvertToBoolean(object value)
		{
			bool rv = false;
			try
			{
				bool c = Convert.ToBoolean(value);
				//	If we reach this point, then the conversion was a success.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanConvertToDateTime																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the value can be converted to a
		/// DateTime Type.
		/// </summary>
		/// <param name="value">
		/// The Value to Inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the Conversion is possible.
		/// </returns>
		public static bool CanConvertToDateTime(object value)
		{
			bool rv = false;
			try
			{
				DateTime c = Convert.ToDateTime(value);
				//	If we reach this point, then the conversion was a success.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanConvertToDouble																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the value can be converted to a
		/// Double.
		/// </summary>
		/// <param name="value">
		/// The Value to Inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the Conversion is possible.
		/// </returns>
		public static bool CanConvertToDouble(object value)
		{
			bool rv = false;
			try
			{
				Double c = Convert.ToDouble(value);
				//	If we reach this point, then the conversion was a success.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CanConvertToInt64																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the value can be converted to an
		/// Int64 Type.
		/// </summary>
		/// <param name="value">
		/// The Value to Inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the Conversion is possible.
		/// </returns>
		public static bool CanConvertToInt64(object value)
		{
			bool rv = false;
			try
			{
				Int64 c = Convert.ToInt64(value);
				//	If we reach this point, then the conversion was a success.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonAdd																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of addition for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 + value2.
		/// </returns>
		public static object CommonAdd(object value1, object value2)
		{
			//	Rules:
			//	If both items are strings while entering, then the items
			//	are processed using string behavior. However, if one or both
			//	items are not strings, and both items can be converted to
			//	non-string objects, then processing is performed on the non-string
			//	entities, and are returned as a string.
			//	Examples:
			//	DateTime(string) + Timespan.
			//		ro = (DateTime(DateTime) + Timespan).ToString()
			//	Int32(string) + Int64.
			//		ro = (Int32(Int64) + Int64).ToString()
			object ro = null;
			SysType t1 = GetSysType(value1);
			bool t1s = false;   //	Flag - T1 is string.
			SysType t2 = GetSysType(value2);
			bool t2s = false;   //	Flag - T2 is string.
			SysType ta;         //	Type to Add with.
			SysType to;         //	Output Type.
			bool ts = false;    //	Flag - Output as String.
			object v1;
			object v2;

			to = GetCommonType(t1, t2);
			switch(t1)
			{
				case SysType.LongString:
				case SysType.String:
				case SysType.StringArray:
					t1s = true;
					break;
			}
			switch(t2)
			{
				case SysType.LongString:
				case SysType.String:
				case SysType.StringArray:
					t2s = true;
					break;
			}
			if(t1s && !t2s)
			{
				//	If t1 is a string and t2 is not, then process using t2's
				//	domain.
				ta = t2;
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
				ts = true;
			}
			else if(!t1s && t2s)
			{
				//	If t1 is not a string, but t2 is, then process using t1's
				//	domain.
				ta = t1;
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
				ts = true;
			}
			else if(t1 == SysType.DateTime && t2 == SysType.TimeSpan)
			{
				//	If we have a Date Time and a Time Span, then keep both.
				ta = SysType.DateTime;
				v1 = GetCommonValue(value1, t1);
				v2 = GetCommonValue(value2, t2);
			}
			else
			{
				//	Otherwise, use the common domain for processing.
				ta = to;
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
			}

			if(ta != SysType.DBNull && ta != SysType.Null && ta != SysType.Unknown &&
				v1 != null && v2 != null)
			{
				switch(ta)
				{
					case SysType.Boolean:
						ro = ((Boolean)v1) | ((Boolean)v2);
						break;
					case SysType.Byte:
						ro = ((Byte)v1) + ((Byte)v2);
						break;
					case SysType.ByteArray:
						ro = Concat((byte[])v1, (byte[])v2);
						break;
					case SysType.Char:
						ro = ((Char)v1) + ((Char)v2);
						break;
					case SysType.CharArray:
						ro = Concat((char[])v1, (char[])v2);
						break;
					case SysType.DateTime:
						ro = ((DateTime)v1).Add((TimeSpan)v2);
						break;
					case SysType.Decimal:
						ro = ((Decimal)v1) + ((Decimal)v2);
						break;
					case SysType.Double:
						ro = ((double)v1) + ((double)v2);
						break;
					case SysType.Enum:
						ro = ((int)v1) + ((int)v2);
						break;
					case SysType.Guid:
						ro = ToValueOfType(
							CommonOr(
							ToValueOfType(v1, SysType.ByteArray, false),
							ToValueOfType(v2, SysType.ByteArray, false)),
							SysType.Guid, false);
						break;
					case SysType.Image:
						ro = null;
						break;
					case SysType.Int16:
						ro = ((Int16)v1) + ((Int16)v2);
						break;
					case SysType.Int32:
						ro = ((Int32)v1) + ((Int32)v2);
						break;
					case SysType.Int64:
						ro = ((Int64)v1) + ((Int64)v2);
						break;
					case SysType.LongString:
						ro = (LongString)(v1.ToString() + v2.ToString());
						break;
					case SysType.SByte:
						ro = ((SByte)v1) + ((SByte)v2);
						break;
					case SysType.Single:
						ro = ((Single)v1) + ((Single)v2);
						break;
					case SysType.String:
						ro = v1.ToString() + v2.ToString();
						break;
					case SysType.StringArray:
						ro = Concat((string[])v1, (string[])v2);
						break;
					case SysType.TimeSpan:
						ro = ((TimeSpan)v1) + ((TimeSpan)v2);
						break;
					case SysType.Type:
						ro = null;
						break;
					case SysType.UInt16:
						ro = ((UInt16)v1) + ((UInt16)v2);
						break;
					case SysType.UInt32:
						ro = ((UInt32)v1) + ((UInt32)v2);
						break;
					case SysType.UInt64:
						ro = ((UInt64)v1) + ((UInt64)v2);
						break;
				}
			}
			else if(v2 != null)
			{
				ro = v2;
			}
			else
			{
				ro = v1;
			}
			if(ts && ro != null)
			{
				//	If we have a value that has been processed in another type, then
				//	return to the caller as a string.
				ro = ro.ToString();
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonAnd																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of AND for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonAnd(byte[] value1, byte[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			byte[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (byte)(value1[lpt] & value2[lpt]);
					}
					else
					{
						ro[lpt] = 0;
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = 0;
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = 0;
				}
			}
			else
			{
				ro = new byte[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of AND for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonAnd(char[] value1, char[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			char[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (char)(value1[lpt] & value2[lpt]);
					}
					else
					{
						ro[lpt] = (char)0;
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (char)0;
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (char)0;
				}
			}
			else
			{
				ro = new char[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of AND for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonAnd(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			UInt64 u1;
			UInt64 u2;
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(t1 == SysType.DateTime && t2 == SysType.TimeSpan)
			{
				ta = SysType.DateTime;
				v1 = GetCommonValue(value1, t1);
				v2 = GetCommonValue(value2, t2);
			}
			else
			{
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
			}

			if(ta != SysType.DBNull && ta != SysType.Null && ta != SysType.Unknown &&
				v1 != null && v2 != null)
			{
				switch(ta)
				{
					case SysType.Boolean:
						ro = ((Boolean)v1) & ((Boolean)v2);
						break;
					case SysType.Byte:
						ro = ((Byte)v1) & ((Byte)v2);
						break;
					case SysType.ByteArray:
						ro = CommonAnd((byte[])v1, (byte[])v2);
						break;
					case SysType.Char:
						ro = ((Char)v1) & ((Char)v2);
						break;
					case SysType.CharArray:
						ro = CommonAnd((char[])v1, (char[])v2);
						break;
					case SysType.DateTime:
						ro = null;
						break;
					case SysType.Decimal:
						u1 = Convert.ToUInt64(v1);
						u2 = Convert.ToUInt64(v2);
						u1 &= u2;
						ro = Convert.ToDecimal(u1);
						//						ro = (Decimal)(((UInt64)v1) & ((UInt64)v2));
						break;
					case SysType.Double:
						ro = (Double)(((UInt64)v1) & ((UInt64)v2));
						break;
					case SysType.Enum:
						ro = ((int)v1) & ((int)v2);
						break;
					case SysType.Guid:
						ro = ToValueOfType(
							CommonAnd(
							ToValueOfType(v1, SysType.ByteArray, false),
							ToValueOfType(v2, SysType.ByteArray, false)),
							SysType.Guid, false);
						break;
					case SysType.Image:
						ro = null;
						break;
					case SysType.Int16:
						ro = ((Int16)v1) & ((Int16)v2);
						break;
					case SysType.Int32:
						ro = ((Int32)v1) & ((Int32)v2);
						break;
					case SysType.Int64:
						ro = ((Int64)v1) & ((Int64)v2);
						break;
					case SysType.LongString:
						ro = (LongString)CommonAnd(
							v1.ToString().ToCharArray(), v2.ToString().ToCharArray());
						break;
					case SysType.SByte:
						ro = ((SByte)v1) & ((SByte)v2);
						break;
					case SysType.Single:
						ro = (Single)(((UInt32)v1) & ((UInt32)v2));
						break;
					case SysType.String:
						ro = CommonAnd(
							v1.ToString().ToCharArray(),
							v2.ToString().ToCharArray());
						break;
					case SysType.StringArray:
						ro = null;
						break;
					case SysType.TimeSpan:
						ro = null;
						break;
					case SysType.Type:
						ro = null;
						break;
					case SysType.UInt16:
						ro = ((UInt16)v1) & ((UInt16)v2);
						break;
					case SysType.UInt32:
						ro = ((UInt32)v1) & ((UInt32)v2);
						break;
					case SysType.UInt64:
						ro = ((UInt64)v1) & ((UInt64)v2);
						break;
				}
			}
			else
			{
				if(IsNumericType(ta))
				{
					ro = ToValueOfType(0, ta, false);
				}
				else
				{
					ro = null;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonAssign																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return assignment of value2 to the caller.
		/// </summary>
		/// <param name="value1">
		/// Existing left side value (target).
		/// </param>
		/// <param name="value2">
		/// Value to be assigned to target.
		/// </param>
		/// <returns>
		/// Right side of assignment.
		/// </returns>
		/// <remarks>
		/// Even though this method only handles value2 in reality, value1 is
		/// required as the target of the expression to maintain the consistency
		/// of the CommonX methods within delegations.
		/// </remarks>
		public static object CommonAssign(object value1, object value2)
		{
			return value2;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonCompare																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of comparison for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Operations member, typed as an object, indicating whether
		/// value1 is greater than, less than, or equal to value2.
		/// </returns>
		/// <remarks>
		/// The calling application can determine whether the comparison matches
		/// more specific ranges such as >=, etc.
		/// </remarks>
		public static object CommonCompare(object value1, object value2)
		{
			Operations ro = Operations.Ignore;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			int wv = 0;     //	Working Value.

			ta = GetCommonType(t1, t2);

			try
			{
				if(value1 != null || value2 != null)
				{
					//	If both of these types are value types, then let's find the
					//	common value between them.
					ta = GetCommonType(t1, t2);   //	Get the common type.
					switch(ta)
					{
						case SysType.Boolean:
							if((Boolean)value1 == (Boolean)value2)
							{
								ro = Operations.Equal;
							}
							else
							{
								ro = Operations.NotEqual;
							}
							break;
						case SysType.Byte:
							if((Byte)value1 > (Byte)value2)
							{
								ro = Operations.Greater;
							}
							else if((Byte)value1 == (Byte)value2)
							{
								ro = Operations.Equal;
							}
							else if((Byte)value1 < (Byte)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Char:
							if((Char)value1 > (Char)value2)
							{
								ro = Operations.Greater;
							}
							else if((Char)value1 == (Char)value2)
							{
								ro = Operations.Equal;
							}
							else if((Char)value1 < (Char)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.DateTime:
							if(DateTime.Compare((DateTime)value1, (DateTime)value2) > 0)
							{
								ro = Operations.Greater;
							}
							else if(DateTime.Compare((DateTime)value1, (DateTime)value2) == 0)
							{
								ro = Operations.Equal;
							}
							else
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Decimal:
							if(ToDecimal(value1) > ToDecimal(value2))
							{
								ro = Operations.Greater;
							}
							else if(ToDecimal(value1) == ToDecimal(value2))
							{
								ro = Operations.Equal;
							}
							else if(ToDecimal(value1) < ToDecimal(value2))
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Double:
							if((Double)value1 > (Double)value2)
							{
								ro = Operations.Greater;
							}
							else if((Double)value1 == (Double)value2)
							{
								ro = Operations.Equal;
							}
							else if((Double)value1 < (Double)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Int16:
							if((Int16)value1 > (Int16)value2)
							{
								ro = Operations.Greater;
							}
							else if((Int16)value1 == (Int16)value2)
							{
								ro = Operations.Equal;
							}
							else if((Int16)value1 < (Int16)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Int32:
							if((Int32)value1 > (Int32)value2)
							{
								ro = Operations.Greater;
							}
							else if((Int32)value1 == (Int32)value2)
							{
								ro = Operations.Equal;
							}
							else if((Int32)value1 < (Int32)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Int64:
							if((Int64)value1 > (Int64)value2)
							{
								ro = Operations.Greater;
							}
							else if((Int64)value1 == (Int64)value2)
							{
								ro = Operations.Equal;
							}
							else if((Int64)value1 < (Int64)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.SByte:
							if((SByte)value1 > (SByte)value2)
							{
								ro = Operations.Greater;
							}
							else if((SByte)value1 == (SByte)value2)
							{
								ro = Operations.Equal;
							}
							else if((SByte)value1 < (SByte)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Single:
							if((Single)value1 > (Single)value2)
							{
								ro = Operations.Greater;
							}
							else if((Single)value1 == (Single)value2)
							{
								ro = Operations.Equal;
							}
							else if((Single)value1 < (Single)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.TimeSpan:
							if((TimeSpan)value1 > (TimeSpan)value2)
							{
								ro = Operations.Greater;
							}
							else if((TimeSpan)value1 == (TimeSpan)value2)
							{
								ro = Operations.Equal;
							}
							else if((TimeSpan)value1 < (TimeSpan)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.UInt16:
							if((UInt16)value1 > (UInt16)value2)
							{
								ro = Operations.Greater;
							}
							else if((UInt16)value1 == (UInt16)value2)
							{
								ro = Operations.Equal;
							}
							else if((UInt16)value1 < (UInt16)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.UInt32:
							if((UInt32)value1 > (UInt32)value2)
							{
								ro = Operations.Greater;
							}
							else if((UInt32)value1 == (UInt32)value2)
							{
								ro = Operations.Equal;
							}
							else if((UInt32)value1 < (UInt32)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.UInt64:
							if((UInt64)value1 > (UInt64)value2)
							{
								ro = Operations.Greater;
							}
							else if((UInt64)value1 == (UInt64)value2)
							{
								ro = Operations.Equal;
							}
							else if((UInt64)value1 < (UInt64)value2)
							{
								ro = Operations.Less;
							}
							break;
						case SysType.Guid:
							if(ToGuid(value1, Guid.Empty) == ToGuid(value2, Guid.Empty))
							{
								ro = Operations.Equal;
							}
							else
							{
								wv = String.Compare(ToString(value1), ToString(value2), true);
								if(wv > 0)
								{
									ro = Operations.Greater;
								}
								else if(wv < 0)
								{
									ro = Operations.Less;
								}
								else
								{
									ro = Operations.Equal;
								}
							}
							break;
						case SysType.LongString:
						case SysType.String:
						default:
							wv = String.Compare(ToString(value1), ToString(value2), true);
							if(wv > 0)
							{
								ro = Operations.Greater;
							}
							else if(wv < 0)
							{
								ro = Operations.Less;
							}
							else
							{
								ro = Operations.Equal;
							}
							break;
					}
				}
			}
			catch { }
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonDivide																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of division for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Dividend.
		/// </param>
		/// <param name="value2">
		/// Divisor.
		/// </param>
		/// <returns>
		/// Common typed result of value1 / value2.
		/// </returns>
		/// <remarks>
		/// This method requires that both values be numeric, or null will be
		/// returned.
		/// </remarks>
		public static object CommonDivide(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(value1 != null && value2 != null && IsNumericType(ta))
			{
				//	If both values are numeric, then continue.
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);

				if((double)ToValueOfType(v2, SysType.Double, false) != 0)
				{
					switch(ta)
					{
						case SysType.Byte:
							ro = ((Byte)v1) / ((Byte)v2);
							break;
						case SysType.Char:
							ro = ((Char)v1) / ((Char)v2);
							break;
						case SysType.Decimal:
							ro = ((Decimal)v1) / ((Decimal)v2);
							break;
						case SysType.Double:
							ro = ((Double)v1) / ((Double)v2);
							break;
						case SysType.Enum:
							ro = ((int)v1) / ((int)v2);
							break;
						case SysType.Int16:
							ro = ((Int16)v1) / ((Int16)v2);
							break;
						case SysType.Int32:
							ro = ((Int32)v1) / ((Int32)v2);
							break;
						case SysType.Int64:
							ro = ((Int64)v1) / ((Int64)v2);
							break;
						case SysType.SByte:
							ro = ((SByte)v1) / ((SByte)v2);
							break;
						case SysType.Single:
							ro = ((Single)v1) / ((Single)v2);
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) / ((UInt16)v2);
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) / ((UInt32)v2);
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) / ((UInt64)v2);
							break;
					}
				}
				else
				{
					//	If trying to divide by zero, then just return zero.
					ro = ToValueOfType(0, ta, false);
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonEquals																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Compare two values and attempt to determine their Relative Equality.
		/// </summary>
		/// <param name="value1">
		/// Left-side Value in comparison.
		/// </param>
		/// <param name="value2">
		/// Right-side Value in comparison.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the values share a common equality.
		/// </returns>
		public static object CommonEquals(object value1, object value2)
		{
			return (Compare(value1, value2) == Operations.Equal);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonGreater																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the logical result of value1 &gt; value2.
		/// </summary>
		/// <param name="value1">
		/// Value expected to be greater.
		/// </param>
		/// <param name="value2">
		/// Value expected to be less.
		/// </param>
		/// <returns>
		/// True if value1 evaluates to greater than value2. Otherwise, false.
		/// </returns>
		public static object CommonGreater(object value1, object value2)
		{
			return (Compare(value1, value2) == Operations.Greater);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonGreaterEqual																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the logical result of value1 &gt;= value2.
		/// </summary>
		/// <param name="value1">
		/// Value expected to be greater or equal.
		/// </param>
		/// <param name="value2">
		/// Value expected to be less or equal.
		/// </param>
		/// <returns>
		/// True if value1 evaluates to greater than or equal to value2. Otherwise,
		/// false.
		/// </returns>
		public static object CommonGreaterEqual(object value1, object value2)
		{
			Operations op = Compare(value1, value2);
			return (op == Operations.Greater || op == Operations.Equal);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonLess																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the logical result of value1 &lt; value2.
		/// </summary>
		/// <param name="value1">
		/// Value expected to be less.
		/// </param>
		/// <param name="value2">
		/// Value expected to be greater.
		/// </param>
		/// <returns>
		/// True if value1 evaluates to less than value2. Otherwise, false.
		/// </returns>
		public static object CommonLess(object value1, object value2)
		{
			return (Compare(value1, value2) == Operations.Less);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonLessEqual																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the logical result of value1 &lt;= value2.
		/// </summary>
		/// <param name="value1">
		/// Value expected to be less or equal.
		/// </param>
		/// <param name="value2">
		/// Value expected to be greater or equal.
		/// </param>
		/// <returns>
		/// True if value1 evaluates to less than or equal to value2. Otherwise,
		/// false.
		/// </returns>
		public static object CommonLessEqual(object value1, object value2)
		{
			Operations op = Compare(value1, value2);
			return (op == Operations.Less || op == Operations.Equal);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonModulus																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of modulus for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Dividend.
		/// </param>
		/// <param name="value2">
		/// Divisor.
		/// </param>
		/// <returns>
		/// Common typed result of value1 % value2.
		/// </returns>
		/// <remarks>
		/// This method requires that both values be numeric, or null will be
		/// returned.
		/// </remarks>
		public static object CommonModulus(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(value1 != null && value2 != null && IsNumericType(ta))
			{
				//	If both values are numeric, then continue.
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);

				if((double)ToValueOfType(v2, SysType.Double, false) != 0)
				{
					switch(ta)
					{
						case SysType.Byte:
							ro = ((Byte)v1) % ((Byte)v2);
							break;
						case SysType.Char:
							ro = ((Char)v1) % ((Char)v2);
							break;
						case SysType.Decimal:
							ro = ((Decimal)v1) % ((Decimal)v2);
							break;
						case SysType.Double:
							ro = ((Double)v1) % ((Double)v2);
							break;
						case SysType.Enum:
							ro = ((int)v1) % ((int)v2);
							break;
						case SysType.Int16:
							ro = ((Int16)v1) % ((Int16)v2);
							break;
						case SysType.Int32:
							ro = ((Int32)v1) % ((Int32)v2);
							break;
						case SysType.Int64:
							ro = ((Int64)v1) % ((Int64)v2);
							break;
						case SysType.SByte:
							ro = ((SByte)v1) % ((SByte)v2);
							break;
						case SysType.Single:
							ro = ((Single)v1) % ((Single)v2);
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) % ((UInt16)v2);
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) % ((UInt32)v2);
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) % ((UInt64)v2);
							break;
					}
				}
				else
				{
					//	If trying to divide by zero, then just return zero.
					ro = ToValueOfType(0, ta, false);
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonMultiply																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of multiplication for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Multiplicand.
		/// </param>
		/// <param name="value2">
		/// Multiplier.
		/// </param>
		/// <returns>
		/// The result of the multiplication.
		/// </returns>
		/// <remarks>
		/// This method requires that both values be numeric, or null will be
		/// returned.
		/// </remarks>
		public static object CommonMultiply(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(value1 != null && value2 != null && IsNumericType(ta))
			{
				//	If both values are numeric, then continue.
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);

				if((double)ToValueOfType(v2, SysType.Double, false) != 0)
				{
					switch(ta)
					{
						case SysType.Byte:
							ro = ((Byte)v1) * ((Byte)v2);
							break;
						case SysType.Char:
							ro = ((Char)v1) * ((Char)v2);
							break;
						case SysType.Decimal:
							ro = ((Decimal)v1) * ((Decimal)v2);
							break;
						case SysType.Double:
							ro = ((Double)v1) * ((Double)v2);
							break;
						case SysType.Enum:
							ro = ((int)v1) * ((int)v2);
							break;
						case SysType.Int16:
							ro = ((Int16)v1) * ((Int16)v2);
							break;
						case SysType.Int32:
							ro = ((Int32)v1) * ((Int32)v2);
							break;
						case SysType.Int64:
							ro = ((Int64)v1) * ((Int64)v2);
							break;
						case SysType.SByte:
							ro = ((SByte)v1) * ((SByte)v2);
							break;
						case SysType.Single:
							ro = ((Single)v1) * ((Single)v2);
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) * ((UInt16)v2);
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) * ((UInt32)v2);
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) * ((UInt64)v2);
							break;
					}
				}
				else
				{
					//	If trying to divide by zero, then just return zero.
					ro = ToValueOfType(0, ta, false);
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonNot																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of AND NOT Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 ! value2.
		/// </returns>
		/// <remarks>
		/// <p>
		/// If either of the values are null upon entry, the return value is
		/// the binary compliment of the non-null value is returned.
		/// </p>
		/// <p>
		/// If the common type can not be expressed as a binary value, then null is
		/// returned.
		/// </p>
		/// </remarks>
		public static object CommonNot(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1 = null;
			object v2 = null;

			ta = GetCommonType(t1, t2);

			if(value1 != null && value2 == null)
			{
				v1 = GetCommonValue(value1, t1);
				ta = t1;
			}
			else if(value1 == null && value2 != null)
			{
				v1 = GetCommonValue(value2, t2);
				ta = t2;
			}
			else if(value1 != null && value2 != null)
			{
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
			}

			if(IsBinaryType(ta))
			{
				//	If we have a binary type, then continue.
				if(v2 != null)
				{
					//	Two operands.
					switch(ta)
					{
						case SysType.Boolean:
							ro = ((Boolean)v1) & !((Boolean)v2);
							break;
						case SysType.Byte:
							ro = (Byte)Not((UInt16)v1, (UInt16)v2);
							break;
						case SysType.ByteArray:
							ro = CommonXor((byte[])CommonAnd((byte[])v1, (byte[])v2), 0xff);
							break;
						case SysType.Char:
							ro = (Char)Not((UInt16)v1, (UInt16)v2);
							break;
						case SysType.CharArray:
							ro = CommonXor((char[])CommonAnd((char[])v1, (char[])v2), 0xff);
							break;
						case SysType.DateTime:
							ro = null;
							break;
						case SysType.Decimal:
							ro = (Decimal)Not((UInt64)v1, (UInt64)v2);
							break;
						case SysType.Double:
							ro = (Double)Not((UInt64)v1, (UInt64)v2);
							break;
						case SysType.Enum:
							ro = (Int32)Not((UInt32)v1, (UInt32)v2);
							break;
						case SysType.Guid:
							ro = ToValueOfType(
								CommonXor(
								(byte[])ToValueOfType(v1, SysType.ByteArray, false),
								(byte[])ToValueOfType(v2, SysType.ByteArray, false)),
								SysType.Guid, false);
							break;
						case SysType.Image:
							ro = null;
							break;
						case SysType.Int16:
							ro = (Int16)Not((UInt16)v1, (UInt16)v2);
							break;
						case SysType.Int32:
							ro = (Int32)Not((UInt32)v1, (UInt32)v2);
							break;
						case SysType.Int64:
							ro = (Int64)Not((UInt64)v1, (UInt64)v2);
							break;
						case SysType.LongString:
							ro = (LongString)CommonXor(CommonAnd(
								v1.ToString().ToCharArray(), v2.ToString().ToCharArray()),
								0xff);
							break;
						case SysType.SByte:
							ro = (SByte)Not((UInt16)v1, (UInt16)v2);
							break;
						case SysType.Single:
							ro = (Single)Not((UInt32)v1, (UInt32)v2);
							break;
						case SysType.String:
							ro = CommonXor(CommonAnd(
								v1.ToString().ToCharArray(),
								v2.ToString().ToCharArray()), 0xff);
							break;
						case SysType.StringArray:
							ro = null;
							break;
						case SysType.TimeSpan:
							ro = null;
							break;
						case SysType.Type:
							ro = null;
							break;
						case SysType.UInt16:
							ro = Not((UInt16)v1, (UInt16)v2);
							break;
						case SysType.UInt32:
							ro = Not((UInt32)v1, (UInt32)v2);
							break;
						case SysType.UInt64:
							ro = Not((UInt64)v1, (UInt64)v2);
							break;
					}
				}
				else
				{
					//	Only one operand.
					switch(ta)
					{
						case SysType.Boolean:
							ro = !((Boolean)v1);
							break;
						case SysType.Byte:
							ro = (Byte)Not((UInt16)v1);
							break;
						case SysType.ByteArray:
							ro = CommonXor((byte[])v1, 0xff);
							break;
						case SysType.Char:
							ro = (Char)Not((UInt16)v1);
							break;
						case SysType.CharArray:
							ro = CommonXor((char[])v1, 0xff);
							break;
						case SysType.DateTime:
							ro = null;
							break;
						case SysType.Decimal:
							ro = (Decimal)Not((UInt64)v1);
							break;
						case SysType.Double:
							ro = (Double)Not((UInt64)v1);
							break;
						case SysType.Enum:
							ro = (Int32)Not((UInt32)v1);
							break;
						case SysType.Guid:
							ro = null;
							break;
						case SysType.Image:
							ro = null;
							break;
						case SysType.Int16:
							ro = (Int16)Not((UInt16)v1);
							break;
						case SysType.Int32:
							ro = (Int32)Not((UInt32)v1);
							break;
						case SysType.Int64:
							ro = (Int64)Not((UInt64)v1);
							break;
						case SysType.LongString:
							ro = null;
							break;
						case SysType.SByte:
							ro = (SByte)Not((UInt16)v1);
							break;
						case SysType.Single:
							ro = (Single)Not((UInt32)v1);
							break;
						case SysType.String:
							ro = null;
							break;
						case SysType.StringArray:
							ro = null;
							break;
						case SysType.TimeSpan:
							ro = null;
							break;
						case SysType.Type:
							ro = null;
							break;
						case SysType.UInt16:
							ro = Not((UInt16)v1);
							break;
						case SysType.UInt32:
							ro = Not((UInt32)v1);
							break;
						case SysType.UInt64:
							ro = Not((UInt64)v1);
							break;
					}
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonNotEqual																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether values 1 and 2 are not equal.
		/// </summary>
		/// <param name="value1">
		/// Left-side Value in comparison.
		/// </param>
		/// <param name="value2">
		/// Right-side Value in comparison.
		/// </param>
		/// <returns>
		/// Value indicating whether values 1 and 2 are not equal.
		/// </returns>
		public static object CommonNotEqual(object value1, object value2)
		{
			return (Compare(value1, value2) != Operations.Equal);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonOr																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of OR Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 | value2.
		/// </returns>
		public static object CommonOr(byte[] value1, byte[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			byte[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (byte)Or((UInt16)value1[lpt], (UInt16)value2[lpt]);
					}
					else if(lpt < lc1)
					{
						ro[lpt] = value1[lpt];
					}
					else if(lpt < lc2)
					{
						ro[lpt] = value2[lpt];
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = value1[lpt];
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = value2[lpt];
				}
			}
			else
			{
				ro = new byte[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of OR Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 | value2.
		/// </returns>
		public static object CommonOr(char[] value1, char[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			char[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (char)Or((UInt16)value1[lpt], (UInt16)value2[lpt]);
					}
					else if(lpt < lc1)
					{
						ro[lpt] = value1[lpt];
					}
					else if(lpt < lc2)
					{
						ro[lpt] = value2[lpt];
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = value1[lpt];
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = value2[lpt];
				}
			}
			else
			{
				ro = new char[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of OR Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 | value2.
		/// </returns>
		public static object CommonOr(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(t1 == SysType.DateTime && t2 == SysType.TimeSpan)
			{
				ta = SysType.DateTime;
				v1 = GetCommonValue(value1, t1);
				v2 = GetCommonValue(value2, t2);
			}
			else
			{
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
			}

			if(ta != SysType.DBNull && ta != SysType.Null && ta != SysType.Unknown &&
				v1 != null && v2 != null)
			{
				switch(ta)
				{
					case SysType.Boolean:
						ro = ((Boolean)v1) | ((Boolean)v2);
						break;
					case SysType.Byte:
						ro = ((Byte)v1) | ((Byte)v2);
						break;
					case SysType.ByteArray:
						ro = CommonOr((byte[])v1, (byte[])v2);
						break;
					case SysType.Char:
						ro = ((Char)v1) | ((Char)v2);
						break;
					case SysType.CharArray:
						ro = CommonOr((char[])v1, (char[])v2);
						break;
					case SysType.DateTime:
						ro = null;
						break;
					case SysType.Decimal:
						ro = (Decimal)Or((UInt64)v1, (UInt64)v2);
						break;
					case SysType.Double:
						ro = (Double)Or((UInt64)v1, (UInt64)v2);
						break;
					case SysType.Enum:
						ro = ((int)v1) | ((int)v2);
						break;
					case SysType.Guid:
						ro = ToValueOfType(
							CommonOr(
							ToValueOfType(v1, SysType.ByteArray, false),
							ToValueOfType(v2, SysType.ByteArray, false)),
							SysType.Guid, false);
						break;
					case SysType.Image:
						ro = null;
						break;
					case SysType.Int16:
						ro = (Int16)Or((UInt16)v1, (UInt16)v2);
						break;
					case SysType.Int32:
						ro = (Int32)Or((UInt32)v1, (UInt32)v2);
						break;
					case SysType.Int64:
						ro = (Int64)Or((UInt64)v1, (UInt64)v2);
						break;
					case SysType.LongString:
						ro = (LongString)CommonOr(
							v1.ToString().ToCharArray(), v2.ToString().ToCharArray()).
							ToString();
						break;
					case SysType.SByte:
						ro = (SByte)Or((UInt16)v1, (UInt16)v2);
						break;
					case SysType.Single:
						ro = (Single)Or((UInt32)v1, (UInt32)v2);
						break;
					case SysType.String:
						ro = CommonOr(
							v1.ToString().ToCharArray(),
							v2.ToString().ToCharArray()).ToString();
						break;
					case SysType.StringArray:
						ro = null;
						break;
					case SysType.TimeSpan:
						ro = null;
						break;
					case SysType.Type:
						ro = null;
						break;
					case SysType.UInt16:
						ro = ((UInt16)v1) | ((UInt16)v2);
						break;
					case SysType.UInt32:
						ro = ((UInt32)v1) | ((UInt32)v2);
						break;
					case SysType.UInt64:
						ro = ((UInt64)v1) | ((UInt64)v2);
						break;
				}
			}
			else if(v2 != null)
			{
				ro = v2;
			}
			else
			{
				ro = v1;
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonShiftLeft																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of Shift Left Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Operand.
		/// </param>
		/// <param name="value2">
		/// Shift Count.
		/// </param>
		/// <returns>
		/// Result of value1 shifted left value2 bits.
		/// </returns>
		public static object CommonShiftLeft(object value1, object value2)
		{
			object ro = null;
			Int32 sc = 0;       //	Shift Count.
			SysType ta;
			//			object v1;
			//			object v2;

			if(value2 != null)
			{
				if(!IsNumeric(value2))
				{
					//	If value2 is a string, then get the number, and convert it to
					//	a value.
					sc = (Int32)ToValueOfType(
						GetNumber(value2.ToString()), SysType.Int32, false);
				}
				else
				{
					sc = (Int32)value2;
				}
			}
			if(sc != 0 && IsNumeric(value1))
			{
				ta = GetSysType(value1);
				switch(ta)
				{
					case SysType.Byte:
						ro = ((Byte)value1) << sc;
						break;
					case SysType.Int16:
						ro = ((Int16)value1) << sc;
						break;
					case SysType.Int32:
						ro = ((Int32)value1) << sc;
						break;
					case SysType.Int64:
						ro = ((Int64)value1) << sc;
						break;
					case SysType.UInt16:
						ro = ((UInt16)value1) << sc;
						break;
					case SysType.UInt32:
						ro = ((UInt32)value1) << sc;
						break;
					case SysType.UInt64:
						ro = ((UInt64)value1) << sc;
						break;
					case SysType.SByte:
						ro = ((SByte)value1) << sc;
						break;
					case SysType.Single:
						ro = (Single)(((UInt32)value1) << sc);
						break;
					case SysType.Double:
						ro = (Double)(((UInt64)value1) << sc);
						break;
					case SysType.Decimal:
						ro = (Decimal)(((UInt64)value1) << sc);
						break;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonShiftRight																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of Shift Right Operation for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Operand.
		/// </param>
		/// <param name="value2">
		/// Shift Count.
		/// </param>
		/// <returns>
		/// Result of value1 shifted right value2 bits.
		/// </returns>
		public static object CommonShiftRight(object value1, object value2)
		{
			object ro = null;
			Int32 sc = 0;       //	Shift Count.
			SysType ta;
			//			object v1;
			//			object v2;

			if(value2 != null)
			{
				if(!IsNumeric(value2))
				{
					//	If value2 is a string, then get the number, and convert it to
					//	a value.
					sc = (Int32)ToValueOfType(
						GetNumber(value2.ToString()), SysType.Int32, false);
				}
				else
				{
					sc = (Int32)value2;
				}
			}
			if(sc != 0 && IsNumeric(value1))
			{
				ta = GetSysType(value1);
				switch(ta)
				{
					case SysType.Byte:
						ro = ((Byte)value1) >> sc;
						break;
					case SysType.Int16:
						ro = ((Int16)value1) >> sc;
						break;
					case SysType.Int32:
						ro = ((Int32)value1) >> sc;
						break;
					case SysType.Int64:
						ro = ((Int64)value1) >> sc;
						break;
					case SysType.UInt16:
						ro = ((UInt16)value1) >> sc;
						break;
					case SysType.UInt32:
						ro = ((UInt32)value1) >> sc;
						break;
					case SysType.UInt64:
						ro = ((UInt64)value1) >> sc;
						break;
					case SysType.SByte:
						ro = ((SByte)value1) >> sc;
						break;
					case SysType.Single:
						ro = (Single)(((UInt32)value1) >> sc);
						break;
					case SysType.Double:
						ro = (Double)(((UInt64)value1) >> sc);
						break;
					case SysType.Decimal:
						ro = (Decimal)(((UInt64)value1) >> sc);
						break;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonSubtract																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of subtraction for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 - value2.
		/// </returns>
		/// <remarks>
		/// This method requires that both values be numeric, or null will be
		/// returned.
		/// </remarks>
		public static object CommonSubtract(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1;
			object v2;

			ta = GetCommonType(t1, t2);
			if(value1 != null && value2 != null && IsNumericType(ta))
			{
				//	If both values are numeric, then continue.
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);

				if((double)ToValueOfType(v2, SysType.Double, false) != 0)
				{
					switch(ta)
					{
						case SysType.Byte:
							ro = ((Byte)v1) - ((Byte)v2);
							break;
						case SysType.Char:
							ro = ((Char)v1) - ((Char)v2);
							break;
						case SysType.Decimal:
							ro = ((Decimal)v1) - ((Decimal)v2);
							break;
						case SysType.Double:
							ro = ((Double)v1) - ((Double)v2);
							break;
						case SysType.Enum:
							ro = ((int)v1) - ((int)v2);
							break;
						case SysType.Int16:
							ro = ((Int16)v1) - ((Int16)v2);
							break;
						case SysType.Int32:
							ro = ((Int32)v1) - ((Int32)v2);
							break;
						case SysType.Int64:
							ro = ((Int64)v1) - ((Int64)v2);
							break;
						case SysType.SByte:
							ro = ((SByte)v1) - ((SByte)v2);
							break;
						case SysType.Single:
							ro = ((Single)v1) - ((Single)v2);
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) - ((UInt16)v2);
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) - ((UInt32)v2);
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) - ((UInt64)v2);
							break;
					}
				}
				else
				{
					//	If trying to subtract zero, then just return first value.
					ro = ToValueOfType(v1, ta, false);
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CommonXor																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of XOR for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="xor">
		/// Xor Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonXor(byte[] value1, int xor)
		{
			int lc1 = 0;        //	List Count.
													//			int lc2 = 0;				//	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			byte[] ro = null;   //	Return Object.

			if(value1 != null)
			{
				lc1 = value1.Length;
				lct = lc1;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (byte)((int)value1[lpt] ^ xor);
				}
			}
			else
			{
				ro = new byte[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of XOR for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonXor(byte[] value1, byte[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			byte[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (byte)((int)value1[lpt] ^ (int)value2[lpt]);
					}
					else if(lpt < lc1)
					{
						ro[lpt] = value1[lpt];
					}
					else if(lpt < lc2)
					{
						ro[lpt] = value2[lpt];
					}
					else
					{
						ro[lpt] = 0;
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (byte)((int)value1[lpt] ^ 0xff);
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new byte[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (byte)((int)value2[lpt] ^ 0xff);
				}
			}
			else
			{
				ro = new byte[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of XOR for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="xor">
		/// Xor Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonXor(char[] value1, int xor)
		{
			int lc1 = 0;        //	List Count.
													//			int lc2 = 0;				//	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			char[] ro = null;   //	Return Object.

			if(value1 != null)
			{
				lc1 = value1.Length;
				lct = lc1;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (char)((int)value1[lpt] ^ xor);
				}
			}
			else
			{
				ro = new char[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of XOR for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 AND value2.
		/// </returns>
		public static object CommonXor(char[] value1, char[] value2)
		{
			int lc1 = 0;        //	List Count.
			int lc2 = 0;        //	List Count.
			int lct = 0;        //	List Count.
			int lpt = 0;        //	List Position.
			char[] ro = null;   //	Return Object.

			if(value1 != null && value2 != null)
			{
				lc1 = value1.Length;
				lc2 = value2.Length;
				lct = lc1;
				if(lc2 > lct)
				{
					lct = lc2;
				}
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					if(lpt < lc1 && lpt < lc2)
					{
						ro[lpt] = (char)((int)value1[lpt] ^ (int)value2[lpt]);
					}
					else if(lpt < lc1)
					{
						ro[lpt] = value1[lpt];
					}
					else if(lpt < lc2)
					{
						ro[lpt] = value2[lpt];
					}
					else
					{
						ro[lpt] = (char)0;
					}
				}
			}
			else if(value1 != null)
			{
				lct = value1.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (char)((int)value1[lpt] ^ 0xff);
				}
			}
			else if(value2 != null)
			{
				lct = value2.Length;
				ro = new char[lct];
				for(lpt = 0; lpt < lct; lpt++)
				{
					ro[lpt] = (char)((int)value2[lpt] ^ 0xff);
				}
			}
			else
			{
				ro = new char[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the result of Exclusive OR Operator for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Common typed result of value1 ^ value2.
		/// </returns>
		public static object CommonXor(object value1, object value2)
		{
			object ro = null;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			object v1 = null;
			object v2 = null;

			ta = GetCommonType(t1, t2);

			if(value1 != null && value2 == null)
			{
				v1 = GetCommonValue(value1, t1);
				ta = t1;
			}
			else if(value1 == null && value2 != null)
			{
				v1 = GetCommonValue(value2, t2);
				ta = t2;
			}
			else if(value1 != null && value2 != null)
			{
				v1 = GetCommonValue(value1, ta);
				v2 = GetCommonValue(value2, ta);
			}

			if(IsBinaryType(ta))
			{
				//	If we have a binary type, then continue.
				if(v2 != null)
				{
					//	Two operands.
					switch(ta)
					{
						case SysType.Boolean:
							ro = ((Boolean)v1) ^ ((Boolean)v2);
							break;
						case SysType.Byte:
							ro = ((Byte)v1) ^ ((Byte)v2);
							break;
						case SysType.ByteArray:
							ro = CommonXor((byte[])v1, (byte[])v2);
							break;
						case SysType.Char:
							ro = ((Char)v1) ^ ((Char)v2);
							break;
						case SysType.CharArray:
							ro = CommonXor((char[])v1, (char[])v2);
							break;
						case SysType.DateTime:
							ro = null;
							break;
						case SysType.Decimal:
							ro = (Decimal)Xor((UInt64)v1, (UInt64)v2);
							break;
						case SysType.Double:
							ro = (Double)Xor((UInt64)v1, (UInt64)v2);
							break;
						case SysType.Enum:
							ro = (Int32)Xor((UInt32)v1, (UInt32)v2);
							break;
						case SysType.Guid:
							ro = ToValueOfType(
								CommonXor(
								(byte[])ToValueOfType(v1, SysType.ByteArray, false),
								(byte[])ToValueOfType(v2, SysType.ByteArray, false)),
								SysType.Guid, false);
							break;
						case SysType.Image:
							ro = null;
							break;
						case SysType.Int16:
							ro = ((Int16)v1) ^ ((Int16)v2);
							break;
						case SysType.Int32:
							ro = ((Int32)v1) ^ ((Int32)v2);
							break;
						case SysType.Int64:
							ro = ((Int64)v1) ^ ((Int64)v2);
							break;
						case SysType.LongString:
							ro = null;
							break;
						case SysType.SByte:
							ro = ((SByte)v1) ^ ((SByte)v2);
							break;
						case SysType.Single:
							ro = (Single)Xor((UInt32)v1, (UInt32)v2);
							break;
						case SysType.String:
							ro = null;
							break;
						case SysType.StringArray:
							ro = null;
							break;
						case SysType.TimeSpan:
							ro = null;
							break;
						case SysType.Type:
							ro = null;
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) ^ ((UInt16)v2);
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) ^ ((UInt32)v2);
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) ^ ((UInt64)v2);
							break;
					}
				}
				else
				{
					//	Only one operand.
					switch(ta)
					{
						case SysType.Boolean:
							ro = !((Boolean)v1);
							break;
						case SysType.Byte:
							ro = ((Byte)v1) ^ 0xff;
							break;
						case SysType.ByteArray:
							ro = CommonXor((byte[])v1, 0xff);
							break;
						case SysType.Char:
							ro = ((Char)v1) ^ 0xff;
							break;
						case SysType.CharArray:
							ro = CommonXor((char[])v1, 0xff);
							break;
						case SysType.DateTime:
							ro = null;
							break;
						case SysType.Decimal:
							ro = (Decimal)Not((UInt64)v1);
							break;
						case SysType.Double:
							ro = (Double)Not((UInt64)v1);
							break;
						case SysType.Enum:
							ro = ((Int32)v1) ^ 0xffffffff;
							break;
						case SysType.Guid:
							ro = null;
							break;
						case SysType.Image:
							ro = null;
							break;
						case SysType.Int16:
							ro = ((Int16)v1) ^ 0xffff;
							break;
						case SysType.Int32:
							ro = ((Int32)v1) ^ 0xffffffff;
							break;
						case SysType.Int64:
							ro = (Int64)Not((UInt64)v1);
							break;
						case SysType.LongString:
							ro = null;
							break;
						case SysType.SByte:
							ro = ((SByte)v1) ^ 0xff;
							break;
						case SysType.Single:
							ro = (Single)Not((UInt32)v1);
							break;
						case SysType.String:
							ro = null;
							break;
						case SysType.StringArray:
							ro = null;
							break;
						case SysType.TimeSpan:
							ro = null;
							break;
						case SysType.Type:
							ro = null;
							break;
						case SysType.UInt16:
							ro = ((UInt16)v1) ^ 0xffff;
							break;
						case SysType.UInt32:
							ro = ((UInt32)v1) ^ 0xffffffff;
							break;
						case SysType.UInt64:
							ro = ((UInt64)v1) ^ 0xffffffffffffffff;
							break;
					}
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Compare																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the result of comparison for values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left Operand.
		/// </param>
		/// <param name="value2">
		/// Right Operand.
		/// </param>
		/// <returns>
		/// Operations member, typed as an object, indicating whether
		/// value1 is greater than, less than, or equal to value2.
		/// </returns>
		/// <remarks>
		/// The calling application can determine whether the comparison matches
		/// more specific ranges such as >=, etc.
		/// </remarks>
		public static Operations Compare(object value1, object value2)
		{
			Operations ro = Operations.Ignore;
			SysType t1 = GetSysType(value1);
			SysType t2 = GetSysType(value2);
			SysType ta;     //	Type to Add with.
			int wv = 0;     //	Working Value.

			ta = GetCommonType(t1, t2);

			if((value1 == null && value2 != null) ||
				(value1 != null && value2 == null))
			{
				ro = Operations.NotEqual;
			}
			else if(value1 == null && value2 == null)
			{
				ro = Operations.Equal;
			}
			else
			{
				//	If both of these types are value types, then let's find the
				//	common value between them.
				ta = GetCommonType(t1, t2);   //	Get the common type.
				switch(ta)
				{
					case SysType.Boolean:
						if((Boolean)value1 == (Boolean)value2)
						{
							ro = Operations.Equal;
						}
						else
						{
							ro = Operations.NotEqual;
						}
						break;
					case SysType.Byte:
						if((Byte)value1 > (Byte)value2)
						{
							ro = Operations.Greater;
						}
						else if((Byte)value1 == (Byte)value2)
						{
							ro = Operations.Equal;
						}
						else if((Byte)value1 < (Byte)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Char:
						if((Char)value1 > (Char)value2)
						{
							ro = Operations.Greater;
						}
						else if((Char)value1 == (Char)value2)
						{
							ro = Operations.Equal;
						}
						else if((Char)value1 < (Char)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.DateTime:
						if(DateTime.Compare((DateTime)value1, (DateTime)value2) > 0)
						{
							ro = Operations.Greater;
						}
						else if(DateTime.Compare((DateTime)value1, (DateTime)value2) == 0)
						{
							ro = Operations.Equal;
						}
						else
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Decimal:
						value1 = ToDecimal(value1);
						value2 = ToDecimal(value2);
						if((Decimal)value1 > (Decimal)value2)
						{
							ro = Operations.Greater;
						}
						else if((Decimal)value1 == (Decimal)value2)
						{
							ro = Operations.Equal;
						}
						else if((Decimal)value1 < (Decimal)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Double:
						if((Double)value1 > (Double)value2)
						{
							ro = Operations.Greater;
						}
						else if((Double)value1 == (Double)value2)
						{
							ro = Operations.Equal;
						}
						else if((Double)value1 < (Double)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Int16:
						if((Int16)value1 > (Int16)value2)
						{
							ro = Operations.Greater;
						}
						else if((Int16)value1 == (Int16)value2)
						{
							ro = Operations.Equal;
						}
						else if((Int16)value1 < (Int16)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Int32:
						if((Int32)value1 > (Int32)value2)
						{
							ro = Operations.Greater;
						}
						else if((Int32)value1 == (Int32)value2)
						{
							ro = Operations.Equal;
						}
						else if((Int32)value1 < (Int32)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Int64:
						if((Int64)value1 > (Int64)value2)
						{
							ro = Operations.Greater;
						}
						else if((Int64)value1 == (Int64)value2)
						{
							ro = Operations.Equal;
						}
						else if((Int64)value1 < (Int64)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.SByte:
						if((SByte)value1 > (SByte)value2)
						{
							ro = Operations.Greater;
						}
						else if((SByte)value1 == (SByte)value2)
						{
							ro = Operations.Equal;
						}
						else if((SByte)value1 < (SByte)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Single:
						if((Single)value1 > (Single)value2)
						{
							ro = Operations.Greater;
						}
						else if((Single)value1 == (Single)value2)
						{
							ro = Operations.Equal;
						}
						else if((Single)value1 < (Single)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.TimeSpan:
						if((TimeSpan)value1 > (TimeSpan)value2)
						{
							ro = Operations.Greater;
						}
						else if((TimeSpan)value1 == (TimeSpan)value2)
						{
							ro = Operations.Equal;
						}
						else if((TimeSpan)value1 < (TimeSpan)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.UInt16:
						if((UInt16)value1 > (UInt16)value2)
						{
							ro = Operations.Greater;
						}
						else if((UInt16)value1 == (UInt16)value2)
						{
							ro = Operations.Equal;
						}
						else if((UInt16)value1 < (UInt16)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.UInt32:
						if((UInt32)value1 > (UInt32)value2)
						{
							ro = Operations.Greater;
						}
						else if((UInt32)value1 == (UInt32)value2)
						{
							ro = Operations.Equal;
						}
						else if((UInt32)value1 < (UInt32)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.UInt64:
						if((UInt64)value1 > (UInt64)value2)
						{
							ro = Operations.Greater;
						}
						else if((UInt64)value1 == (UInt64)value2)
						{
							ro = Operations.Equal;
						}
						else if((UInt64)value1 < (UInt64)value2)
						{
							ro = Operations.Less;
						}
						break;
					case SysType.Guid:
						if(((t1 == SysType.DBNull && (Guid)value2 == Guid.Empty)) ||
							(t2 == SysType.DBNull && (Guid)value1 == Guid.Empty))
						{
							ro = Operations.Equal;
						}
						else
						{
							wv = String.Compare(value1.ToString(), value2.ToString(), true);
							if(wv > 0)
							{
								ro = Operations.Greater;
							}
							else if(wv < 0)
							{
								ro = Operations.Less;
							}
							else
							{
								ro = Operations.Equal;
							}
						}
						break;
					case SysType.LongString:
					case SysType.String:
					default:
						if(t1 == SysType.Boolean || t2 == SysType.Boolean)
						{
							if(t1 == SysType.Boolean)
							{
								//	First Item is Boolean.
								if(ToBoolean(value2.ToString()) == (bool)value1)
								{
									wv = 0;
								}
								else if((bool)value1 == true)
								{
									wv = 1;
								}
								else
								{
									wv = -1;
								}
							}
							else
							{
								//	Second Item is Boolean.
								if(ToBoolean(value1.ToString()) == (bool)value2)
								{
									wv = 0;
								}
								else if((bool)value2 == true)
								{
									wv = -1;
								}
								else
								{
									wv = 1;
								}
							}
						}
						else
						{
							wv = String.Compare(value1.ToString(), value2.ToString(), true);
						}
						if(wv > 0)
						{
							ro = Operations.Greater;
						}
						else if(wv < 0)
						{
							ro = Operations.Less;
						}
						else
						{
							ro = Operations.Equal;
						}
						break;
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Concat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a concatenated version of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// The first value to concatenate.
		/// </param>
		/// <param name="value2">
		/// The second value to append.
		/// </param>
		/// <returns>
		/// Value1 with Value2 appended to the end.
		/// </returns>
		public static byte[] Concat(byte[] value1, byte[] value2)
		{
			byte[] bas;     //	Byte Array - Source.
			byte[] bat;     //	Byte Array - Target.
			int lcs = 0;    //	List Count - Source.
			int lct = 0;    //	List Count - Target.
			int lps = 0;    //	List Position - Source.
			int lpt = 0;    //	List Position - Target.
			byte[] ro = null;

			if(value1 != null && value2 != null)
			{
				bas = value1;
				bat = value2;
				bat = new byte[bas.Length + bat.Length];
				lct = bat.Length;
				lpt = 0;
				lcs = bas.Length;
				lps = 0;
				for(; lpt < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
				bas = value2;
				lcs = bas.Length;
				lps = 0;
				for(; lps < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
			}
			else if(value1 != null)
			{
				ro = value1;
			}
			else if(value2 != null)
			{
				ro = value2;
			}
			else
			{
				ro = new byte[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a concatenated version of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// The first value to concatenate.
		/// </param>
		/// <param name="value2">
		/// The second value to append.
		/// </param>
		/// <returns>
		/// Value1 with Value2 appended to the end.
		/// </returns>
		public static char[] Concat(char[] value1, char[] value2)
		{
			char[] bas;     //	Char Array - Source.
			char[] bat = null;      //	Char Array - Target.
			int lcs = 0;    //	List Count - Source.
			int lct = 0;    //	List Count - Target.
			int lps = 0;    //	List Position - Source.
			int lpt = 0;    //	List Position - Target.
			char[] ro = null;

			if(value1 != null && value2 != null)
			{
				bas = value1;
				bat = value2;
				bat = new char[bas.Length + bat.Length];
				lct = bat.Length;
				lpt = 0;
				lcs = bas.Length;
				lps = 0;
				for(; lpt < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
				bas = value2;
				lcs = bas.Length;
				lps = 0;
				for(; lps < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
				ro = bat;
			}
			else if(value1 != null)
			{
				ro = value1;
			}
			else if(value2 != null)
			{
				ro = value2;
			}
			else
			{
				ro = new char[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a concatenated version of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// The first value to concatenate.
		/// </param>
		/// <param name="value2">
		/// The second value to append.
		/// </param>
		/// <returns>
		/// Value1 with Value2 appended to the end.
		/// </returns>
		public static string[] Concat(string[] value1, string value2)
		{
			string[] ro = null;

			if(value1 != null)
			{
				if(value2 != null)
				{
					ro = new string[value1.Length + 1];
					ro[ro.Length - 1] = value2;
				}
				else
				{
					ro = value1;
				}
			}
			else
			{
				ro = new string[0];
			}
			return ro;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a concatenated version of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// The first value to concatenate.
		/// </param>
		/// <param name="value2">
		/// The second value to append.
		/// </param>
		/// <returns>
		/// Value1 with Value2 appended to the end.
		/// </returns>
		public static string[] Concat(string[] value1, string[] value2)
		{
			string[] bas;         //	String Array - Source.
			string[] bat = null;  //	String Array - Target.
			int lcs = 0;    //	List Count - Source.
			int lct = 0;    //	List Count - Target.
			int lps = 0;    //	List Position - Source.
			int lpt = 0;    //	List Position - Target.
			string[] ro = null;

			if(value1 != null && value2 != null)
			{
				bas = value1;
				bat = value2;
				bat = new string[bas.Length + bat.Length];
				lct = bat.Length;
				lpt = 0;
				lcs = bas.Length;
				lps = 0;
				for(; lpt < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
				bas = value2;
				lcs = bas.Length;
				lps = 0;
				for(; lps < lct && lps < lcs; lpt++, lps++)
				{
					bat[lpt] = bas[lps];
				}
				ro = bat;
			}
			else if(value1 != null)
			{
				ro = value1;
			}
			else if(value2 != null)
			{
				ro = value2;
			}
			else
			{
				ro = new string[0];
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Copy																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(DateTime source, byte[] target, UInt32 offset)
		{
			Copy(source, target, offset, 8);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		/// <param name="length">
		/// Number of Bytes to Copy.
		/// </param>
		public static void Copy(DateTime source, byte[] target, UInt32 offset,
			UInt32 length)
		{
			UInt32 udts = 0;    //	Short Date Time.
			UInt64 udtl = 0;    //	Long Date Time.

			if(length == 4)
			{
				try
				{
					udts = Convert.ToUInt32(source);
				}
				catch { }
				Copy(udts, target, offset);
			}
			else
			{
				try
				{
					udtl = Convert.ToUInt64(source);
				}
				catch { }
				Copy(udtl, target, offset);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(Double source, byte[] target, UInt32 offset)
		{
			Copy(BitConverter.GetBytes(source), target, offset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(Guid source, byte[] target, UInt32 offset)
		{
			Copy(source.ToByteArray(), target, offset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(Single source, byte[] target, UInt32 offset)
		{
			Copy(BitConverter.GetBytes(source), target, offset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(UInt16 source, byte[] target, UInt32 offset)
		{
			int co;

			if(target != null)
			{
				co = (int)offset;
				target[co++] = (byte)((source & 0x00ff));
				target[co++] = (byte)((source & 0xff00) / 0x0100);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(UInt32 source, byte[] target, UInt32 offset)
		{
			int co;

			if(target != null)
			{
				co = (int)offset;
				target[co++] = (byte)((source & 0x000000ff));
				target[co++] = (byte)((source & 0x0000ff00) / 0x00000100);
				target[co++] = (byte)((source & 0x00ff0000) / 0x00010000);
				target[co++] = (byte)((source & 0xff000000) / 0x01000000);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(UInt64 source, byte[] target, UInt32 offset)
		{
			int co;

			if(target != null)
			{
				co = (int)offset;
				target[co++] =
					(byte)((source & 0x00000000000000ff));
				target[co++] =
					(byte)((source & 0x000000000000ff00) / 0x0000000000000100);
				target[co++] =
					(byte)((source & 0x0000000000ff0000) / 0x0000000000010000);
				target[co++] =
					(byte)((source & 0x00000000ff000000) / 0x0000000001000000);
				target[co++] =
					(byte)((source & 0x000000ff00000000) / 0x0000000100000000);
				target[co++] =
					(byte)((source & 0x0000ff0000000000) / 0x0000010000000000);
				target[co++] =
					(byte)((source & 0x00ff000000000000) / 0x0001000000000000);
				target[co++] =
					(byte)((source & 0xff00000000000000) / 0x0100000000000000);
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="sourceOffset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <param name="targetOffset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		/// <param name="length">
		/// Number of Bytes to Copy.
		/// </param>
		public static void Copy(byte[] source, byte[] target, UInt32 sourceOffset,
			UInt32 targetOffset, UInt32 length)
		{
			UInt32 bc = 0;                //	Bytes Copied.
			UInt32 sc = (UInt32)source.Length;    //	Source Count.
			UInt32 sp = 0;                //	Source Position.
			UInt32 tc = (UInt32)target.Length;    //	Target Count.
			UInt32 tp = 0;                //	Target Position.

			for(sp = sourceOffset, tp = targetOffset;
				sp < sourceOffset + sc && bc < length; sp++, tp++, bc++)
			{
				target[tp] = source[sp];
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="targetOffset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(byte[] source, byte[] target, UInt32 targetOffset)
		{
			UInt32 sc = (UInt32)source.Length;    //	Source Count.
			UInt32 sp = 0;                //	Source Position.
			UInt32 tp = 0;                //	Target Position.

			for(sp = 0, tp = targetOffset; sp < sc; sp++, tp++)
			{
				target[tp] = source[sp];
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="targetOffset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		/// <param name="count">
		/// Value indicating whether a count value will be placed at the first byte
		/// of the offset.
		/// </param>
		public static void Copy(byte[] source, byte[] target, UInt32 targetOffset,
			bool count)
		{
			UInt32 sc = (UInt32)source.Length;    //	Source Count.
			UInt32 sp = 0;                //	Source Position.
			UInt32 tp = targetOffset;   //	Target Position.

			if(count)
			{
				target[tp] = (byte)sc;
				//				sc ++;										//	Increment the Count of bytes to write.
				//				sp ++;										//	Increment the Source position.
				tp++;                   //	Increment the Target position.
			}
			for(; sp < sc; sp++, tp++)
			{
				target[tp] = source[sp];
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="targetOffset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		/// <param name="length">
		/// Number of Bytes to Copy.
		/// </param>
		public static void Copy(byte[] source, byte[] target, UInt32 targetOffset,
			int length)
		{
			UInt32 bc = 0;                //	Bytes Copied.
			UInt32 sc = (UInt32)source.Length;    //	Source Count.
			UInt32 sp = 0;                //	Source Position.
			UInt32 tc = (UInt32)target.Length;    //	Target Count.
			UInt32 tp = 0;                //	Target Position.

			for(sp = 0, tp = targetOffset; sp < sc && bc < length; sp++, tp++)
			{
				target[tp] = source[sp];
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		public static void Copy(string source, byte[] target, UInt32 offset)
		{
			byte[] ba;
			int lc = 0;   //	List Count.
			int lo = 0;   //	List Offset.
			int lp = 0;   //	List Position.

			if(source.Length != 0 && target != null)
			{
				ba = ASCIIEncoding.ASCII.GetBytes(source);
				if(ba != null)
				{
					lc = ba.Length;
					lo = (int)offset;
					for(lp = 0; lp < lc; lp++, lo++)
					{
						target[lo] = ba[lp];
					}
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Copy a value to a byte array.
		/// </summary>
		/// <param name="source">
		/// Value to be copied.
		/// </param>
		/// <param name="target">
		/// Byte Array to receive the converted value.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be placed.
		/// </param>
		/// <param name="count">
		/// Value indicating whether a count value will be placed at the first byte
		/// of the offset.
		/// </param>
		public static void Copy(string source, byte[] target, UInt32 offset,
			bool count)
		{
			Copy(ASCIIEncoding.ASCII.GetBytes(source), target, offset, count);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DefaultStringForType																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Default Value, represented as a string, for the specified
		/// Data Type.
		/// </summary>
		/// <param name="value">
		/// The Type to Analyze.
		/// </param>
		/// <param name="nullValue">
		/// The Value to use for Null Defaults.
		/// </param>
		/// <returns>
		/// Default Value for given Type, represented as a string.
		/// </returns>
		public static string DefaultStringForType(Type value, string nullValue)
		{
			string rs = "\"\"";

			if(value == typeof(Boolean))
			{
				rs = "false";
			}
			else if(value == typeof(DateTime))
			{
				rs = "DateTime.Now";
			}
			else if(value == typeof(Double))
			{
				rs = "0";
			}
#if WindowsOnly
			else if(value == typeof(Image))
			{
				rs = nullValue;
			}
#endif
			else if(value == typeof(Int16))
			{
				rs = "0";
			}
			else if(value == typeof(Int32))
			{
				rs = "0";
			}
			else if(value == typeof(Int64))
			{
				rs = "0";
			}
			else if(value == typeof(Single))
			{
				rs = "0";
			}
			else if(value == typeof(String))
			{
				rs = "\"\"";
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DefaultToForType																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Default To(x) conversion name, represented as a string, for
		/// the specified Data Type.
		/// </summary>
		/// <param name="value">
		/// The Type to Analyze.
		/// </param>
		/// <returns>
		/// Default To Conversion for given Type, represented as a string.
		/// </returns>
		public static string DefaultToForType(Type value)
		{
			string rs = "ToString";

			if(value == typeof(Boolean))
			{
				rs = "ToBoolean";
			}
			else if(value == typeof(DateTime))
			{
				rs = "ToDateTime";
			}
			else if(value == typeof(Double))
			{
				rs = "ToDouble";
			}
#if WindowsOnly
			else if(value == typeof(Image))
			{
				rs = "ToImage";
			}
#endif
			else if(value == typeof(Int16))
			{
				rs = "ToInt16";
			}
			else if(value == typeof(Int32))
			{
				rs = "ToInt32";
			}
			else if(value == typeof(Int64))
			{
				rs = "ToInt64";
			}
			else if(value == typeof(Single))
			{
				rs = "ToSingle";
			}
			else if(value == typeof(String))
			{
				rs = "ToString";
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DefaultValueForType																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Default Value, represented in its natural form, for the
		/// specified Data Type.
		/// </summary>
		/// <param name="value">
		/// The Type to Analyze.
		/// </param>
		/// <returns>
		/// Default Value for given Type, represented as a string.
		/// </returns>
		public static object DefaultValueForType(Type value)
		{
			object ro = null;

			if(value == typeof(Boolean))
			{
				ro = false;
			}
			else if(value == typeof(DateTime))
			{
				ro = DateTime.MinValue;
			}
			else if(value == typeof(Double))
			{
				ro = (double)0;
			}
#if WindowsOnly
			else if(value == typeof(Image))
			{
				ro = null;
			}
#endif
			else if(value == typeof(Int16))
			{
				ro = (Int16)0;
			}
			else if(value == typeof(Int32))
			{
				ro = (Int32)0;
			}
			else if(value == typeof(Int64))
			{
				ro = (Int64)0;
			}
			else if(value == typeof(Single))
			{
				ro = (Single)0;
			}
			else if(value == typeof(String))
			{
				ro = "";
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Equals																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Compare two values and attempt to determine their Relative Equality.
		/// </summary>
		/// <param name="value1">
		/// Left-side Value in comparison.
		/// </param>
		/// <param name="value2">
		/// Right-side Value in comparison.
		/// </param>
		/// <returns>
		/// Value indicating whether or not the values share a common equality.
		/// </returns>
		public new static bool Equals(object value1, object value2)
		{
			return (Compare(value1, value2) == Operations.Equal);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetCommonType																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the common type between the two specified items.
		/// </summary>
		/// <param name="type1">
		/// Left Type.
		/// </param>
		/// <param name="type2">
		/// Right Type.
		/// </param>
		/// <returns>
		/// The common type between the caller's two specified types.
		/// </returns>
		public static SysType GetCommonType(SysType type1, SysType type2)
		{
			return mSysTypeConversion[(int)type1, (int)type2];
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetCommonValue																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the caller's value, converted to the Target Type.
		/// </summary>
		/// <param name="value">
		/// Value to be inspected.
		/// </param>
		/// <param name="targetType">
		/// Type to which the value should be converted, if necessary.
		/// </param>
		/// <returns>
		/// Caller's value, converted to specified type, if necessary.
		/// </returns>
		public static object GetCommonValue(object value, SysType targetType)
		{
			object ro = value;

			if(value != null && GetSysType(value) != targetType)
			{
				ro = ToValueOfType(value, targetType, true);
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetGuid																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the guid portion of the caller's string.
		/// </summary>
		/// <param name="value">
		/// Raw value having formatted guid content.
		/// </param>
		/// <returns>
		/// String value containing guid.
		/// </returns>
		public static string GetGuid(string value)
		{
			Group g;
			MatchCollection mc = Regex.Matches(value,
				@"(?i:(?<val>" +
				"[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}))");
			StringBuilder sb = new StringBuilder();

			if(mc != null && mc.Count != 0)
			{
				foreach(Match m in mc)
				{
					g = m.Groups["val"];
					if(g != null)
					{
						sb.Append(g.Value);
					}
				}
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNumber																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the number portion of the caller's string.
		/// </summary>
		/// <param name="value">
		/// Raw value having letters or numbers.
		/// </param>
		/// <returns>
		/// String value containing numbers only.
		/// </returns>
		public static string GetNumber(string value)
		{
			Group g;
			MatchCollection mc = Regex.Matches(value, @"(?<val>-*\d*\.*\d+)");
			StringBuilder sb = new StringBuilder();

			if(mc != null && mc.Count != 0)
			{
				foreach(Match m in mc)
				{
					g = m.Groups["val"];
					if(g != null)
					{
						sb.Append(g.Value);
					}
				}
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetSysType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a SysType Enumeration member corresponding with the system type
		/// specified by the caller.
		/// </summary>
		/// <param name="type">
		/// System Type to represent.
		/// </param>
		/// <returns>
		/// SysType enumeration member corresponding to type. If no match can
		/// be found, the return value is SysType.Unknown.
		/// </returns>
		public static SysType GetSysType(Type type)
		{
			SysType rv = SysType.Unknown;

			if(type == null)
			{
				rv = SysType.Null;
			}
			else
			{
				if(type == typeof(byte[]))
				{
					rv = SysType.ByteArray;
				}
				else if(type == typeof(char[]))
				{
					rv = SysType.CharArray;
				}
				else if(type == typeof(string[]))
				{
					rv = SysType.StringArray;
				}
				else if(type == typeof(Enum))
				{
					rv = SysType.Enum;
				}
				else
				{
					switch(type.FullName)
					{
						case "Stellar.LongString":
							rv = SysType.LongString;
							break;
						case "System.Drawing.Image":
						case "System.Drawing.Bitmap":
							rv = SysType.Image;
							break;
						case "System.Boolean":
							rv = SysType.Boolean;
							break;
						case "System.Byte":
							rv = SysType.Byte;
							break;
						case "System.Char":
							rv = SysType.Char;
							break;
						case "System.DateTime":
							rv = SysType.DateTime;
							break;
						case "System.DBNull":
							rv = SysType.DBNull;
							break;
						case "System.Decimal":
							rv = SysType.Decimal;
							break;
						case "System.Double":
							rv = SysType.Double;
							break;
						case "System.Guid":
							rv = SysType.Guid;
							break;
						case "System.Int16":
							rv = SysType.Int16;
							break;
						case "System.Int32":
							rv = SysType.Int32;
							break;
						case "System.Int64":
							rv = SysType.Int64;
							break;
						case "System.SByte":
							rv = SysType.SByte;
							break;
						case "System.Single":
							rv = SysType.Single;
							break;
						case "System.String":
							rv = SysType.String;
							break;
						case "System.TimeSpan":
							rv = SysType.TimeSpan;
							break;
						case "System.Type":
							rv = SysType.Type;
							break;
						case "System.UInt16":
							rv = SysType.UInt16;
							break;
						case "System.UInt32":
							rv = SysType.UInt32;
							break;
						case "System.UInt64":
							rv = SysType.UInt64;
							break;
					}
				}
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a SysType Enumeration member corresponding with the system type
		/// closest matching the caller-specified value.
		/// </summary>
		/// <param name="value">
		/// Value to analyze.
		/// </param>
		/// <returns>
		/// SysType enumeration member corresponding to value. If no match can
		/// be found, the return value is SysType.Unknown.
		/// </returns>
		public static SysType GetSysType(object value)
		{
			SysType rv = SysType.Unknown;

			if(value == null)
			{
				rv = SysType.Null;
			}
			else
			{
				if(value is byte[])
				{
					rv = SysType.ByteArray;
				}
				else if(value is char[])
				{
					rv = SysType.CharArray;
				}
				else if(value is string[])
				{
					rv = SysType.StringArray;
				}
				else if(value is Enum)
				{
					rv = SysType.Enum;
				}
				else
				{
					switch(value.GetType().FullName)
					{
						case "Stellar.LongString":
							rv = SysType.LongString;
							break;
						case "System.Drawing.Image":
						case "System.Drawing.Bitmap":
							rv = SysType.Image;
							break;
						case "System.Boolean":
							rv = SysType.Boolean;
							break;
						case "System.Byte":
							rv = SysType.Byte;
							break;
						case "System.Char":
							rv = SysType.Char;
							break;
						case "System.DateTime":
							rv = SysType.DateTime;
							break;
						case "System.DBNull":
							rv = SysType.DBNull;
							break;
						case "System.Decimal":
							rv = SysType.Decimal;
							break;
						case "System.Double":
							rv = SysType.Double;
							break;
						case "System.Guid":
							rv = SysType.Guid;
							break;
						case "System.Int16":
							rv = SysType.Int16;
							break;
						case "System.Int32":
							rv = SysType.Int32;
							break;
						case "System.Int64":
							rv = SysType.Int64;
							break;
						case "System.SByte":
							rv = SysType.SByte;
							break;
						case "System.Single":
							rv = SysType.Single;
							break;
						case "System.String":
							rv = SysType.String;
							break;
						case "System.TimeSpan":
							rv = SysType.TimeSpan;
							break;
						case "System.Type":
							rv = SysType.Type;
							break;
						case "System.UInt16":
							rv = SysType.UInt16;
							break;
						case "System.UInt32":
							rv = SysType.UInt32;
							break;
						case "System.UInt64":
							rv = SysType.UInt64;
							break;
					}
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HasBaseType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified Top Type has the
		/// specified Base Type.
		/// </summary>
		/// <param name="topType">
		/// The Top Type to regress.
		/// </param>
		/// <param name="baseType">
		/// The Base Type to search for.
		/// </param>
		/// <returns>
		/// True if the Top Type is inherited from the Base Type. False otherwise.
		/// </returns>
		public static bool HasBaseType(Type topType, Type baseType)
		{
			bool rv = false;
			Type t = topType;

			if(baseType != null)
			{
				while(t != null && t != typeof(object) && t != baseType)
				{
					if(t.Equals(baseType))
					{
						//	If we found our match, then we have an inherited item.
						rv = true;
						break;
					}
					t = t.BaseType;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsBinaryType																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Determine whether or not the caller supplied Data Type has a binary
		/// value.
		/// </summary>
		/// <param name="type">
		/// The Data Type to check for binary value.
		/// </param>
		/// <returns>
		/// True if the Data Type has a binary value. False otherwise.
		/// </returns>
		public static bool IsBinaryType(SysType type)
		{
			bool rv = false;  //	Return Value.
			switch(type)
			{
				case SysType.Byte:
				case SysType.Int16:
				case SysType.Int32:
				case SysType.Int64:
				case SysType.UInt16:
				case SysType.UInt32:
				case SysType.UInt64:
				case SysType.SByte:
				case SysType.Single:
				case SysType.Double:
				case SysType.Decimal:
				case SysType.ByteArray:
				case SysType.CharArray:
					rv = true;
					break;
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Determine whether or not the caller supplied Data Type has a binary
		/// value.
		/// </summary>
		/// <param name="type">
		/// The Data Type to check for binary value.
		/// </param>
		/// <returns>
		/// True if the Data Type has a binary value. False otherwise.
		/// </returns>
		public static bool IsBinaryType(Type type)
		{
			return IsBinaryType(GetSysType(type));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsDate																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Determine whether or not the caller supplied value has a date
		/// constitution.
		/// </summary>
		/// <param name="value">
		/// The value to convert to a date value.
		/// </param>
		/// <returns>
		/// True if the value can be converted to a date value. False otherwise.
		/// </returns>
		public static bool IsDate(object value)
		{
			bool rv = false;  //	Return Value.
			DateTime wv;      //	Working Value.
			try
			{
				//	Attempt to convert to double. This is the widest value, so no other
				//	form will need to be compressed.
				wv = DateTime.Parse(value.ToString());
				//	If there is no error at this point, then we've made it.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsNumeric																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Determine whether or not the caller supplied value has a numeric value.
		/// </summary>
		/// <param name="value">
		/// The value to convert to a numeric value.
		/// </param>
		/// <returns>
		/// True if the value can be converted to a numeric value. False otherwise.
		/// </returns>
		public static bool IsNumeric(object value)
		{
			bool rv = false;  //	Return Value.
			double wv = 0;    //	Working Value.
			try
			{
				//	Attempt to convert to double. This is the widest value, so no other
				//	form will need to be compressed.
				wv = Convert.ToDouble(value.ToString());
				//	If there is no error at this point, then we've made it.
				rv = true;
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsNumericType																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Determine whether or not the caller supplied Data Type has a numeric
		/// value.
		/// </summary>
		/// <param name="type">
		/// The Data Type to check for numeric value.
		/// </param>
		/// <returns>
		/// True if the Data Type has a numeric value. False otherwise.
		/// </returns>
		public static bool IsNumericType(SysType type)
		{
			bool rv = false;  //	Return Value.
			switch(type)
			{
				case SysType.Byte:
				case SysType.Int16:
				case SysType.Int32:
				case SysType.Int64:
				case SysType.UInt16:
				case SysType.UInt32:
				case SysType.UInt64:
				case SysType.SByte:
				case SysType.Single:
				case SysType.Double:
				case SysType.Decimal:
					rv = true;
					break;
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Determine whether or not the caller supplied Data Type has a numeric
		/// value.
		/// </summary>
		/// <param name="type">
		/// The Data Type to check for numeric value.
		/// </param>
		/// <returns>
		/// True if the Data Type has a numeric value. False otherwise.
		/// </returns>
		public static bool IsNumericType(Type type)
		{
			return IsNumericType(GetSysType(type));
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a value indicating whether the specified value can be converted
		/// purely to a number without any loss of information.
		/// </summary>
		/// <param name="value">
		/// String value to inspect for numerical purity.
		/// </param>
		/// <returns>
		/// Value indicating whether the string value can be converted to a number
		/// without any loss of information.
		/// </returns>
		public static bool IsNumericType(string value)
		{
			bool rv = false;

			MatchCollection m = Regex.Matches(value, @"(?<n>[-.0-9])");
			if(m.Count == value.Length)
			{
				//	If the number of matches is the number of characters, then
				//	this can be converted as a pure number.
				rv = true;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsSelfClosingTag																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified tag name is
		/// self-closing.
		/// </summary>
		/// <param name="tagName">
		/// Name of the tag to inspect.
		/// </param>
		/// <returns>
		/// True of the provided tag is self-closing. Otherwise, false.
		/// </returns>
		public static bool IsSelfClosingTag(string tagName)
		{
			bool result = false;
			if(tagName?.Length > 0)
			{
				if(mSelfClosingTags.Contains(tagName))
				{
					result = true;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsTextType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified type is naturally a
		/// text type.
		/// </summary>
		/// <param name="type">
		/// Type to inspect.
		/// </param>
		/// <returns>
		/// True if this value is normally used to convey text. False otherwise.
		/// </returns>
		public static bool IsTextType(Type type)
		{
			bool rv = false;

			switch(type.FullName)
			{
				case "System.Char":
				case "System.Char[]":
				case "System.String":
					rv = true;
					break;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsXml																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified string is Xml.
		/// </summary>
		/// <param name="value">
		/// String to inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether the caller's string is Xml.
		/// </returns>
		public static bool IsXml(string value)
		{
			bool rv = false;

			if(value != null &&
				value.IndexOf("<") >= 0 &&
				(value.IndexOf("/>") > 0 || value.IndexOf("</") > 0))
			{
				rv = true;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsXmlEmbedded																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified string is embedded Xml.
		/// </summary>
		/// <param name="value">
		/// String to inspect.
		/// </param>
		/// <returns>
		/// Value indicating whether the caller's string is embedded Xml.
		/// </returns>
		public static bool IsXmlEmbedded(string value)
		{
			bool rv = false;

			if(value != null &&
				value.IndexOf("&lt;") >= 0 &&
				(value.IndexOf("/&gt;") > 0 || value.IndexOf("&lt;/") > 0))
			{
				rv = true;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Not																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the binary compliment of the specified value.
		/// </summary>
		/// <param name="value1">
		/// Value to compliment.
		/// </param>
		/// <returns>
		/// Complimented value.
		/// </returns>
		public static UInt16 Not(UInt16 value1)
		{
			return (UInt16)((int)value1 ^ (int)0xffff);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the NAND result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// NANDed value.
		/// </returns>
		public static UInt16 Not(UInt16 value1, UInt16 value2)
		{
			return (UInt16)((int)value1 & ((int)value2 ^ 0xffff));
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the binary compliment of the specified value.
		/// </summary>
		/// <param name="value1">
		/// Value to compliment.
		/// </param>
		/// <returns>
		/// Complimented value.
		/// </returns>
		public static UInt32 Not(UInt32 value1)
		{
			return value1 ^ 0xffffffff;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the NAND result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// NANDed value.
		/// </returns>
		public static UInt32 Not(UInt32 value1, UInt32 value2)
		{
			return value1 & (value2 ^ 0xffffffff);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the binary compliment of the specified value.
		/// </summary>
		/// <param name="value1">
		/// Value to compliment.
		/// </param>
		/// <returns>
		/// Complimented value.
		/// </returns>
		public static UInt64 Not(UInt64 value1)
		{
			return value1 ^ 0xffffffffffffffff;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the NAND result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// NANDed value.
		/// </returns>
		public static UInt64 Not(UInt64 value1, UInt64 value2)
		{
			return value1 & (value2 ^ 0xffffffffffffffff);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	NumberSuffix																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the specified number, formatted with the appropriate suffix.
		/// </summary>
		/// <param name="value">
		/// Numeric value to format.
		/// </param>
		/// <returns>
		/// Specified number, formatted with appropriate st, nd, rd, th suffix.
		/// </returns>
		public static string NumberSuffix(int value)
		{
			int m10 = value % 10;       //	Capture lower order values.
			int m100 = value % 100;     //	Capture teen based values.
			StringBuilder sb = new StringBuilder();

			sb.Append(value.ToString());

			if(m10 == 1)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("st");
				}
				else
				{
					sb.Append("th");
				}
			}
			else if(m10 == 2)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("nd");
				}
				else
				{
					sb.Append("th");
				}
			}
			else if(m10 == 3)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("rd");
				}
				else
				{
					sb.Append("th");
				}
			}
			else
			{
				sb.Append("th");
			}
			return sb.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the specified number, formatted with the appropriate suffix.
		/// </summary>
		/// <param name="value">
		/// Numeric value to format.
		/// </param>
		/// <returns>
		/// Specified number, formatted with appropriate st, nd, rd, th suffix.
		/// </returns>
		public static string NumberSuffix(long value)
		{
			long m10 = value % 10;        //	Capture lower order values.
			long m100 = value % 100;      //	Capture teen based values.
			StringBuilder sb = new StringBuilder();

			sb.Append(value.ToString());

			if(m10 == 1)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("st");
				}
				else
				{
					sb.Append("th");
				}
			}
			else if(m10 == 2)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("nd");
				}
				else
				{
					sb.Append("th");
				}
			}
			else if(m10 == 3)
			{
				if(m100 < 11 || m100 > 19)
				{
					sb.Append("rd");
				}
				else
				{
					sb.Append("th");
				}
			}
			else
			{
				sb.Append("th");
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Or																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the OR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// ORed value.
		/// </returns>
		public static UInt16 Or(UInt16 value1, UInt16 value2)
		{
			return (UInt16)((int)value1 | (int)value2);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the OR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// ORed value.
		/// </returns>
		public static UInt32 Or(UInt32 value1, UInt32 value2)
		{
			return value1 | value2;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the OR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// ORed value.
		/// </returns>
		public static UInt64 Or(UInt64 value1, UInt64 value2)
		{
			return value1 | value2;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RequestAreaCode																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the local Telephone Area Code is needed.
		/// </summary>
		public static event StringEventHandler RequestAreaCode;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RoundDown																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an integer value, rounded down from a floating point value.
		/// </summary>
		/// <param name="value">
		/// Value to Round.
		/// </param>
		/// <returns>
		/// Caller's value, rounded down to next integer value.
		/// </returns>
		public static long RoundDown(decimal value)
		{
			long rv = (long)value;

			if((decimal)rv > value)
			{
				rv--;
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return an integer value, rounded down from a floating point value.
		/// </summary>
		/// <param name="value">
		/// Value to Round.
		/// </param>
		/// <returns>
		/// Caller's value, rounded down to next integer value.
		/// </returns>
		public static long RoundDown(double value)
		{
			long rv = (long)value;

			if((float)rv > value)
			{
				rv--;
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return an integer value, rounded down from a floating point value.
		/// </summary>
		/// <param name="value">
		/// Value to Round.
		/// </param>
		/// <returns>
		/// Caller's value, rounded down to next integer value.
		/// </returns>
		public static int RoundDown(float value)
		{
			int rv = (int)value;

			if((float)rv > value)
			{
				rv--;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RoundUp																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an integer value, rounded up from a floating point value.
		/// </summary>
		/// <param name="value">
		/// Value to Round.
		/// </param>
		/// <returns>
		/// Caller's value, rounded up to next integer value.
		/// </returns>
		public static long RoundUp(double value)
		{
			return (long)Math.Ceiling(value);
			//			long rv = (long)value;
			//
			//			if((double)rv < value)
			//			{
			//				rv ++;
			//			}
			//			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return an integer value, rounded up from a floating point value.
		/// </summary>
		/// <param name="value">
		/// Value to Round.
		/// </param>
		/// <returns>
		/// Caller's value, rounded up to next integer value.
		/// </returns>
		public static int RoundUp(float value)
		{
			return (int)Math.Ceiling(value);
			//			int rv = (int)value;
			//
			//			if((float)rv < value)
			//			{
			//				rv ++;
			//			}
			//			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TextNumber																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the binary value of the number expressed in text.
		/// </summary>
		/// <param name="value">
		/// The Text number to convert to a value.
		/// </param>
		/// <returns>
		/// Binary value of the number represented in text. 
		/// </returns>
		public static int TextNumber(string value)
		{
			//	TODO: Support Decimal fractions using 'and'.
			int ac = 0;             //	Accumulator.
			Group g;                //	Current Group.
			int hd = 0;             //	Hundred Value.
			bool hn = false;        //	Flag - Hundred Active.
			int lc = 0;             //	List Count.
			int lp = 0;             //	List Position.
			Match m;                //	Current Match.
			MatchCollection mc;     //	Matches.
			NameIDCollection ml = new NameIDCollection(); //	Multipliers.
			NameIDItem mli;
			NameIDCollection mn = new NameIDCollection(); //	Numbers.
			NameIDItem mni;
			string mp = "(?i:(?<val>(" +
				"and|" +
				"eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|" +
				"nineteen|" +
				"twenty|thirty|fourty|fifty|sixty|seventy|eighty|ninety|" +
				"million|hundred|thousand|" +
				"one|two|three|four|five|six|seven|eight|nine|ten)))";
			int rv = 0;
			string v = value.ToLower();
			string ws;

			ml.Add("hundred", 100);
			ml.Add("thousand", 1000);
			ml.Add("million", 1000000);

			mn.Add("one", 1);
			mn.Add("two", 2);
			mn.Add("three", 3);
			mn.Add("four", 4);
			mn.Add("five", 5);
			mn.Add("six", 6);
			mn.Add("seven", 7);
			mn.Add("eight", 8);
			mn.Add("nine", 9);
			mn.Add("ten", 10);
			mn.Add("eleven", 11);
			mn.Add("twelve", 12);
			mn.Add("thirteen", 13);
			mn.Add("fourteen", 14);
			mn.Add("fifteen", 15);
			mn.Add("sixteen", 16);
			mn.Add("seventeen", 17);
			mn.Add("eighteen", 18);
			mn.Add("nineteen", 19);
			mn.Add("twenty", 20);
			mn.Add("thirty", 30);
			mn.Add("fourty", 40);
			mn.Add("fifty", 50);
			mn.Add("sixty", 60);
			mn.Add("seventy", 70);
			mn.Add("eighty", 80);
			mn.Add("ninety", 90);

			//	*** EXAMPLES ***
			//	---
			//	[Value]
			//	three hundred four

			//	[Matches]
			//	three
			//	hundred
			//	four

			//	[Forward Logic]
			//	three hundred 300
			//	four 4

			//	[Result]
			//	304
			//	---

			//	---
			//	[Value]
			//	three hundred twenty four

			//	[Matches]
			//	three
			//	hundred
			//	twenty
			//	four

			//	[Forward Logic]
			//	three hundred 300
			//	twenty four 24

			//	[Result]
			//	324
			//	---

			//	---
			//	[Value]
			//	seventy hundred???!!!

			//	[Matches]
			//	seventy
			//	hundred

			//	[Forward Logic]
			//	seventy hundred 7000

			//	[Result]
			//	7000 :-)
			//	---

			//	---
			//	[Value]
			//	one hundred million
			//	five hundred ninety two thousand six hundred ninety four.
			//	[Matches]
			//	one
			//	hundred
			//	million
			//	five
			//	hundred
			//	ninety
			//	two
			//	thousand
			//	six
			//	hundred
			//	ninety
			//	four

			//	[Forward Logic]
			//	one hundred 100
			//	million (100 * 1000000) + (1 * 1000000)
			//	five hundred 500
			//	ninety two thousand (500 * 1000) + (92 * 1000)
			//	six hundred 600
			//	ninety four 94

			//	[Result]
			//	100,592,694
			//	---

			mc = Regex.Matches(v, mp);
			if(mc != null)
			{
				//	If there were matches in the caller's statement, then continue.
				lc = mc.Count;
				for(lp = 0; lp < lc; lp++)
				{
					//	Each Matching Item.
					m = mc[lp];
					g = m.Groups["val"];
					if(g != null)
					{
						//	If this match has a value, then continue.
						ws = g.Value;   //	Get the current multiplier or number.
						mli = ml[ws];   //	Get the possible multiplier.
						mni = mn[ws];   //	Get the possible number.

						if(mli != null)
						{
							//	If we are on a multiplier, then break the current
							//	accumulator chain.
							if(ac == 0)
							{
								//	Don't multiply by zero! :-)
								ac = 1;
							}
							if(hn)
							{
								//	If we are in an active hundred multiplier, then
								//	multiply that value by the current multiplier.
								rv += (hd * mli.ID) + (ac * mli.ID);
								hn = false;   //	Clear the hundred multiplier.
							}
							else
							{
								//	If not in an active hundred, then check to see if
								//	we are entering a hundred scope.
								if(mli.ID == 100)
								{
									//	Here, we are entering a hundred scope. Don't
									//	store in result until we know that it is not
									//	multiplied.
									hd = ac * mli.ID;
									hn = true;
								}
								else
								{
									//	Otherwise, just add the multiplied value.
									rv += ac * mli.ID;
								}
							}
							ac = 0;     //	Clear the accumulator.
						}
						else if(mni != null)
						{
							//	If this is a number, then add it to accumulator.
							ac += mni.ID;
						}

					}
				}
				if(hn)
				{
					//	If we have an active hundred outstanding, then resolve it.
					ac += hd;
				}
				if(ac != 0)
				{
					//	If there is a remainder in the accumulator, then post it.
					rv += ac;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TextNumberInline																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string value with the verbose number converted to numerical
		/// digits.
		/// </summary>
		/// <param name="value">
		/// The Text number to convert to a value.
		/// </param>
		/// <returns>
		/// String value of the text with the verbose number coverted to digits.
		/// </returns>
		/// <remarks>
		/// In this version, the verbose number must be contiguous within the
		/// string, and only whole numbers are allowed.
		/// </remarks>
		public static string TextNumberInline(string value)
		{
			//	TODO: Support Decimal fractions using 'and'.
			int ac = 0;             //	Accumulator.
			int cs = 0;             //	Start Character Index.
			int ce = 0;             //	End Character Index.
			int cl = 0;             //	Total Capture Length.
			Group g;                //	Current Group.
			int hd = 0;             //	Hundred Value.
			bool hn = false;        //	Flag - Hundred Active.
			int lc = 0;             //	List Count.
			int lp = 0;             //	List Position.
			Match m;                //	Current Match.
			MatchCollection mc;     //	Matches.
			NameIDCollection ml = new NameIDCollection(); //	Multipliers.
			NameIDItem mli;
			NameIDCollection mn = new NameIDCollection(); //	Numbers.
			NameIDItem mni;
			string mp = "(?i:(?<val>(" +
				//				"and|" +
				"eleven|twelve|thirteen|fourteen|fifteen|sixteen|seventeen|eighteen|" +
				"nineteen|" +
				"twenty|thirty|fourty|fifty|sixty|seventy|eighty|ninety|" +
				"million|hundred|thousand|" +
				"one|two|three|four|five|six|seven|eight|nine|ten)))";
			//	Return string = 'left of match ' + matchdigits + ' right of match.'
			string rs = "";
			int rv = 0;
			StringBuilder sb;       //	Return String Builder.
			string v = value.ToLower();
			string ws;

			ml.Add("hundred", 100);
			ml.Add("thousand", 1000);
			ml.Add("million", 1000000);

			mn.Add("one", 1);
			mn.Add("two", 2);
			mn.Add("three", 3);
			mn.Add("four", 4);
			mn.Add("five", 5);
			mn.Add("six", 6);
			mn.Add("seven", 7);
			mn.Add("eight", 8);
			mn.Add("nine", 9);
			mn.Add("ten", 10);
			mn.Add("eleven", 11);
			mn.Add("twelve", 12);
			mn.Add("thirteen", 13);
			mn.Add("fourteen", 14);
			mn.Add("fifteen", 15);
			mn.Add("sixteen", 16);
			mn.Add("seventeen", 17);
			mn.Add("eighteen", 18);
			mn.Add("nineteen", 19);
			mn.Add("twenty", 20);
			mn.Add("thirty", 30);
			mn.Add("fourty", 40);
			mn.Add("fifty", 50);
			mn.Add("sixty", 60);
			mn.Add("seventy", 70);
			mn.Add("eighty", 80);
			mn.Add("ninety", 90);

			//	*** EXAMPLES ***
			//	---
			//	[Value]
			//	three hundred four

			//	[Matches]
			//	three
			//	hundred
			//	four

			//	[Forward Logic]
			//	three hundred 300
			//	four 4

			//	[Result]
			//	304
			//	---

			//	---
			//	[Value]
			//	three hundred twenty four

			//	[Matches]
			//	three
			//	hundred
			//	twenty
			//	four

			//	[Forward Logic]
			//	three hundred 300
			//	twenty four 24

			//	[Result]
			//	324
			//	---

			//	---
			//	[Value]
			//	seventy hundred???!!!

			//	[Matches]
			//	seventy
			//	hundred

			//	[Forward Logic]
			//	seventy hundred 7000

			//	[Result]
			//	7000 :-)
			//	---

			//	---
			//	[Value]
			//	one hundred million
			//	five hundred ninety two thousand six hundred ninety four.
			//	[Matches]
			//	one
			//	hundred
			//	million
			//	five
			//	hundred
			//	ninety
			//	two
			//	thousand
			//	six
			//	hundred
			//	ninety
			//	four

			//	[Forward Logic]
			//	one hundred 100
			//	million (100 * 1000000) + (1 * 1000000)
			//	five hundred 500
			//	ninety two thousand (500 * 1000) + (92 * 1000)
			//	six hundred 600
			//	ninety four 94

			//	[Result]
			//	100,592,694
			//	---

			mc = Regex.Matches(v, mp);
			if(mc != null && mc.Count != 0)
			{
				//	If there were matches in the caller's statement, then continue.
				lc = mc.Count;
				//	If List Items present, then get the locations of the first
				//	and last matches.
				cs = mc[0].Index;
				ce = (mc[lc - 1].Index + mc[lc - 1].Length) - 1;
				cl = (ce - cs) + 1;
				for(lp = 0; lp < lc; lp++)
				{
					//	Each Matching Item.
					m = mc[lp];
					g = m.Groups["val"];
					if(g != null)
					{
						//	If this match has a value, then continue.
						ws = g.Value;   //	Get the current multiplier or number.
						mli = ml[ws];   //	Get the possible multiplier.
						mni = mn[ws];   //	Get the possible number.

						if(mli != null)
						{
							//	If we are on a multiplier, then break the current
							//	accumulator chain.
							if(ac == 0)
							{
								//	Don't multiply by zero! :-)
								ac = 1;
							}
							if(hn)
							{
								//	If we are in an active hundred multiplier, then
								//	multiply that value by the current multiplier.
								rv += (hd * mli.ID) + (ac * mli.ID);
								hn = false;   //	Clear the hundred multiplier.
							}
							else
							{
								//	If not in an active hundred, then check to see if
								//	we are entering a hundred scope.
								if(mli.ID == 100)
								{
									//	Here, we are entering a hundred scope. Don't
									//	store in result until we know that it is not
									//	multiplied.
									hd = ac * mli.ID;
									hn = true;
								}
								else
								{
									//	Otherwise, just add the multiplied value.
									rv += ac * mli.ID;
								}
							}
							ac = 0;     //	Clear the accumulator.
						}
						else if(mni != null)
						{
							//	If this is a number, then add it to accumulator.
							ac += mni.ID;
						}

					}
				}
				if(hn)
				{
					//	If we have an active hundred outstanding, then resolve it.
					ac += hd;
				}
				if(ac != 0)
				{
					//	If there is a remainder in the accumulator, then post it.
					rv += ac;
				}
				//	Calculate the inline return value.
				sb = new StringBuilder();
				if(cs > 0)
				{
					//	Post the left side of the source.
					sb.Append(value.Substring(0, cs));
				}
				sb.Append(rv.ToString());
				if(ce < value.Length - 1)
				{
					//	Post the right side of the source.
					sb.Append(value.Substring(ce + 1, value.Length - (ce + 1)));
				}
				rs = sb.ToString();
			}
			else
			{
				rs = value;
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

#if WindowsOnly
		//*-----------------------------------------------------------------------*
		//*	ToBitmap																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert Image Data to a Bitmap Object.
		/// </summary>
		/// <param name="imageData">
		/// The Raw Image Data to Convert.
		/// </param>
		/// <param name="multiPage">
		/// Value indicating whether a multi-page image should be constructed. If
		/// false, a single image will be constructed according to MS Knowledge
		/// Base Article 814675.
		/// </param>
		/// <returns>
		/// A Bitmap Containing Image Data.
		/// </returns>
		public static Bitmap ToBitmap(byte[] imageData, bool multiPage)
		{
			Bitmap bm = null;
			Bitmap bm2 = null;
			Graphics gr;
			MemoryStream ms;
			Bitmap ro = null;

			if(imageData.Length > 0)
			{
				//				string fn;
				//				System.IO.FileStream fs;
				//				Image img;
				ms = new MemoryStream();

				//	Convert to pseudo-bitmap so we can get the file format.
				ms.Write(imageData, 0, (int)imageData.Length);
				ms.Flush();

				bm = (Bitmap)Bitmap.FromStream(ms);

				if(!multiPage)
				{
					//	The following code adheres to the Indexed Method found in MS
					//	Knowledge base article 814675 regarding the locking of streams
					//	and files on open bitmaps.
					bm2 = new Bitmap(bm.Width, bm.Height,
						System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					bm2.SetResolution(bm.HorizontalResolution, bm.VerticalResolution);
					gr = Graphics.FromImage(bm2);
					gr.DrawImage(bm, new PointF(0, 0));
					gr.Dispose();

					bm.Dispose();
					ms.Close();
					ro = bm2;
				}
				else
				{
					ro = bm;
				}
			}
			//			return bm2;
			//			return bm;
			return ro;
		}
		//*-----------------------------------------------------------------------*
#endif

		//*-----------------------------------------------------------------------*
		//* ToBoolean																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Boolean representation of a value.
		/// </summary>
		/// <param name="value">
		/// Value to convert to Boolean.
		/// </param>
		/// <returns>
		/// Boolean representation of the specified value.
		/// </returns>
		public static bool ToBoolean(object value)
		{
			bool rv = false;
			string ws;

			if(value != null)
			{
				ws = value.ToString();
				if(ws.Length != 0)
				{
					rv = ToBoolean(ws);
				}
			}
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the Boolean representation of a value.
		/// </summary>
		/// <param name="value">
		/// Value to convert to Boolean.
		/// </param>
		/// <returns>
		/// Boolean representation of the specified value.
		/// </returns>
		public static bool ToBoolean(string value)
		{
			bool rv = false;
			string vl;          //	Lower Value.

			if(value != null && value.Length != 0)
			{
				vl = value.Substring(0, 1).ToLower();
				if(vl == "t" || vl == "y" || vl == "1" || vl == "u" ||
					value == "-1" || value.ToLower() == "on")
				{
					rv = true;
				}
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToByte																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the byte found at the specified offset of the buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer containing the Byte to retrieve.
		/// </param>
		/// <param name="offset">
		/// Location at which to retrieve the Byte.
		/// </param>
		/// <returns>
		/// Byte at specified offset index of buffer.
		/// </returns>
		public static Byte ToByte(byte[] source, UInt32 offset)
		{
			return source[offset];
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToByteArray																														*
		//*-----------------------------------------------------------------------*
#if WindowsOnly
		/// <summary>
		/// Convert an Image to a byte array, and return the byte array.
		/// </summary>
		/// <param name="image">
		/// The Image to Convert.
		/// </param>
		/// <param name="format">
		/// The Output Image Format.
		/// </param>
		/// <returns>
		/// A Byte Array Containing the Image Data.
		/// </returns>
		public static byte[] ToByteArray(Image image,
			System.Drawing.Imaging.ImageFormat format)
		{
			byte[] ba = null;
			MemoryStream bs = new MemoryStream();

			try
			{
				image.Save(bs, format);
				ba = new byte[bs.Length];
				bs.Position = 0;
				bs.Read(ba, 0, (int)bs.Length);
				bs.Flush();
				bs.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
			}
			return ba;
		}
#endif
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a byte array, converted from an object.
		/// </summary>
		/// <param name="value">
		/// Object to convert.
		/// </param>
		/// <returns>
		/// Byte array representing information of specified object.
		/// </returns>
		public static byte[] ToByteArray(object value)
		{
			byte[] ba = new byte[0];    //	Output Bytes.
			byte[] bw;                  //	Working Array of Bytes.
			int cp = 0;                 //	Character Position.
			char[] cw;                  //	Working Array of Characters.
			SysType st = GetSysType(value);

			switch(st)
			{
				case SysType.Boolean:
					ba = new byte[1];
					ba[0] = (byte)(((bool)value) ? 0x01 : 0x00);
					break;
				case SysType.Byte:
					ba = new byte[1];
					ba[0] = (byte)value;
					break;
				case SysType.ByteArray:
					bw = (byte[])value;
					ba = new byte[bw.Length];
					bw.CopyTo(ba, 0);
					break;
				case SysType.Char:
					ba = new byte[1];
					ba[0] = (byte)value;
					break;
				case SysType.CharArray:
					cw = (char[])value;
					ba = new byte[cw.Length];
					cp = 0;
					foreach(char c in cw)
					{
						ba[cp++] = (byte)c;
					}
					break;
				case SysType.DateTime:
					ba = new byte[8];
					Copy((DateTime)value, ba, 0, 8);
					break;
				case SysType.Decimal:
					break;
				case SysType.Double:
					ba = new byte[8];
					Copy((Double)value, ba, 0);
					break;
				case SysType.Enum:
					ba = new byte[4];
					Copy((UInt32)value, ba, 0);
					break;
				case SysType.Guid:
					ba = new byte[16];
					Copy((Guid)value, ba, 0);
					break;
				case SysType.Image:
#if WindowsOnly
					ba = ToByteArray((Image)value, ((Image)value).RawFormat);
#else
					ba = new byte[0];
#endif
					break;
				case SysType.Int16:
					ba = new byte[2];
					Copy((UInt16)value, ba, 0);
					break;
				case SysType.Int32:
					ba = new byte[4];
					Copy((UInt32)value, ba, 0);
					break;
				case SysType.Int64:
					ba = new byte[8];
					Copy((UInt64)value, ba, 0);
					break;
				case SysType.LongString:
					ba = ASCIIEncoding.ASCII.GetBytes((string)value);
					break;
				case SysType.SByte:
					ba = new byte[1];
					ba[0] = (byte)value;
					break;
				case SysType.Single:
					ba = new byte[4];
					Copy((Single)value, ba, 0);
					break;
				case SysType.String:
					ba = ASCIIEncoding.ASCII.GetBytes((string)value);
					break;
				case SysType.StringArray:
					ba = ASCIIEncoding.ASCII.GetBytes(string.Join(";", (string[])value));
					break;
				case SysType.TimeSpan:
					break;
				case SysType.Type:
					ba = ASCIIEncoding.ASCII.GetBytes(((Type)value).FullName);
					break;
				case SysType.UInt16:
					ba = new byte[2];
					Copy((UInt16)value, ba, 0);
					break;
				case SysType.UInt32:
					ba = new byte[4];
					Copy((UInt16)value, ba, 0);
					break;
				case SysType.UInt64:
					ba = new byte[8];
					Copy((UInt16)value, ba, 0);
					break;
				case SysType.DBNull:
				case SysType.Null:
				case SysType.Unknown:
					break;
			}
			return ba;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a byte array, converted from an object.
		/// </summary>
		/// <param name="value">
		/// Object to convert.
		/// </param>
		/// <param name="length">
		/// Absolute length of return array.
		/// </param>
		/// <returns>
		/// Byte array representing information of specified object.
		/// </returns>
		public static byte[] ToByteArray(object value, UInt32 length)
		{
			byte[] ba = new byte[length];   //	Output Bytes.
			byte[] bw;                  //	Working Array of Bytes.
			int cp = 0;                 //	Character Position.
			char[] cw;                  //	Working Array of Characters.
			SysType st = GetSysType(value);

			if(length != 0)
			{
				switch(st)
				{
					case SysType.Boolean:
						ba[0] = (byte)(((bool)value) ? 0x01 : 0x00);
						break;
					case SysType.Byte:
						ba[0] = (byte)value;
						break;
					case SysType.ByteArray:
						bw = (byte[])value;
						Copy(bw, ba, 0, 0, length);
						break;
					case SysType.Char:
						ba[0] = (byte)value;
						break;
					case SysType.CharArray:
						cw = (char[])value;
						cp = 0;
						foreach(char c in cw)
						{
							ba[cp++] = (byte)c;
							if(cp >= length)
							{
								break;
							}
						}
						break;
					case SysType.DateTime:
						if(length >= 8)
						{
							Copy((DateTime)value, ba, 0, 8);
						}
						else if(length >= 4)
						{
							Copy((DateTime)value, ba, 0, 4);
						}
						break;
					case SysType.Decimal:
						break;
					case SysType.Double:
						if(length >= 8)
						{
							Copy((Double)value, ba, 0);
						}
						break;
					case SysType.Enum:
						if(length >= 4)
						{
							Copy((UInt32)value, ba, 0);
						}
						break;
					case SysType.Guid:
						if(length >= 16)
						{
							Copy((Guid)value, ba, 0);
						}
						break;
					case SysType.Image:
#if WindowsOnly
						bw = ToByteArray((Image)value, ((Image)value).RawFormat);
						if(length >= bw.Length)
						{
							bw.CopyTo(ba, 0);
						}
#else
						ba = new byte[0];
#endif
						break;
					case SysType.Int16:
						if(length >= 2)
						{
							Copy(Convert.ToUInt16(value), ba, 0);
						}
						break;
					case SysType.Int32:
						if(length >= 4)
						{
							Copy(Convert.ToUInt32(value), ba, 0);
						}
						break;
					case SysType.Int64:
						if(length >= 8)
						{
							Copy(Convert.ToUInt64(value), ba, 0);
						}
						break;
					case SysType.LongString:
						bw = ASCIIEncoding.ASCII.GetBytes((string)value);
						Copy(bw, ba, 0, 0, length);
						break;
					case SysType.SByte:
						ba[0] = (byte)value;
						break;
					case SysType.Single:
						if(length >= 4)
						{
							Copy((Single)value, ba, 0);
						}
						break;
					case SysType.String:
						bw = ASCIIEncoding.ASCII.GetBytes((string)value);
						Copy(bw, ba, 0, 0, length);
						break;
					case SysType.StringArray:
						bw = ASCIIEncoding.ASCII.GetBytes(
							string.Join(";", (string[])value));
						Copy(bw, ba, 0, 0, length);
						break;
					case SysType.TimeSpan:
						break;
					case SysType.Type:
						bw = ASCIIEncoding.ASCII.GetBytes(((Type)value).FullName);
						Copy(bw, ba, 0, 0, length);
						break;
					case SysType.UInt16:
						if(length >= 2)
						{
							Copy((UInt16)value, ba, 0);
						}
						break;
					case SysType.UInt32:
						if(length >= 4)
						{
							Copy((UInt32)value, ba, 0);
						}
						break;
					case SysType.UInt64:
						if(length >= 8)
						{
							Copy((UInt64)value, ba, 0);
						}
						break;
					case SysType.DBNull:
					case SysType.Null:
					case SysType.Unknown:
						break;
				}
			}
			return ba;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Convert a string to a byte array and return the byte array.
		/// </summary>
		/// <param name="sourceString">
		/// The source string to convert.
		/// </param>
		/// <returns>
		/// An array of bytes.
		/// </returns>
		public static byte[] ToByteArray(string sourceString)
		{
			//			byte[] ba = new byte[0];									//	Output Array.
			//			int bc = 0;
			//			int bp = 0;																//	Byte Position.
			//			char[] ca = sourceString.ToCharArray();		//	Input Array.
			//
			//			if(ca.Length > 0)
			//			{
			//				ba = new byte[ca.Length];		//	Initialize output array.
			//				bc = ca.Length;
			//				for(bp = 0; bp < bc; bp ++)
			//				{
			//					//	Each Character.
			//					ba[bp] = (byte)ca[bp];		//	Get the byte.
			//				}
			//			}
			//
			//			return ba;
			return ASCIIEncoding.ASCII.GetBytes(sourceString);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToCamelCase																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a representation of the caller's string formatted as Camel Case
		/// text.
		/// </summary>
		/// <param name="value">
		/// The value to format.
		/// </param>
		/// <returns>
		/// Formatted result.
		/// </returns>
		public static string ToCamelCase(string value)
		{
			Group g;                //	Working Group.
			int lp = -1;            //	Last Position.
			MatchCollection mc;     //	Working Matches Collection.
			string rs = value;      //	Return value.
			StringBuilder sb = new StringBuilder();

			mc = Regex.Matches(rs, @"(?<b>\b\w)");
			foreach(Match m in mc)
			{
				g = m.Groups["b"];
				if(g != null)
				{
					if(g.Index > 0)
					{
						//	If some portion of the string was skipped since the last match,
						//	then retrieve the middle portion.
						sb.Append(value.Substring(lp + 1, g.Index - (lp + 1)));
					}
					sb.Append(g.Value.ToUpper());
					lp = g.Index;
				}
			}
			if(value.Length > lp + 1)
			{
				//	If the length of the source is longer that the start of the last
				//	word, then paste the end of the string.
				sb.Append(value.Substring(lp + 1, value.Length - (lp + 1)));
			}
			rs = Regex.Replace(sb.ToString(), " ", "");

			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToChar																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Character Value specified by the caller's Int.
		/// </summary>
		/// <param name="value">
		/// Integer value to be converted to Character.
		/// </param>
		/// <returns>
		/// Character value specified by caller's integer.
		/// </returns>
		public static char ToChar(int value)
		{
			char rv = '0';

			try
			{
				rv = Convert.ToChar(value);
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToDataType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert the incoming value to the specified data type.
		/// </summary>
		/// <param name="value">
		/// The Value to Convert.
		/// </param>
		/// <param name="type">
		/// The Destination Type.
		/// </param>
		/// <returns>
		/// The specified value, converted to the specified Type.
		/// </returns>
		public static object ToDataType(object value, Type type)
		{
			object ro = null;

			if(value.GetType().Equals(type))
			{
				//	If the value is already in the target type, then just return it.
				ro = value;
			}
			else
			{
				//	Otherwise, attempt to find a conversion.
				if(value.GetType().Equals(typeof(string)))
				{
					//	Incoming value is a string.
					if(type.FullName == "System.Boolean")
					{
						try
						{
							ro = Boolean.Parse((string)value);
						}
						catch
						{
							ro = false;
						}
					}
					else if(type.FullName == "System.DateTime")
					{
						try
						{
							ro = DateHelper.Parse((string)value);
						}
						catch { }
					}
					else if(type.FullName == "System.Decimal")
					{
						try
						{
							ro = Decimal.Parse((string)value);
						}
						catch
						{
							ro = (Decimal)0;
						}
					}
					else if(type.FullName == "System.Double")
					{
						try
						{
							ro = Double.Parse((string)value);
						}
						catch
						{
							ro = (Double)0;
						}
					}
					else if(type.FullName == "System.Int16")
					{
						try
						{
							ro = Int16.Parse((string)value);
						}
						catch
						{
							ro = (Int16)0;
						}
					}
					else if(type.FullName == "System.Int32")
					{
						try
						{
							ro = Int32.Parse((string)value);
						}
						catch
						{
							ro = (Int32)0;
						}
					}
					else if(type.FullName == "System.Int64")
					{
						try
						{
							ro = Int64.Parse((string)value);
						}
						catch
						{
							ro = (Int64)0;
						}
					}
					else if(type.FullName == "System.Single")
					{
						try
						{
							ro = Single.Parse((string)value);
						}
						catch
						{
							ro = (Single)0;
						}
					}
					else if(type.FullName == "Stellar.LongString")
					{
						ro = (LongString)((string)value);
					}
					else
					{
						ro = value;
					}
				}
				else
				{
					try
					{
						ro = Convert.ChangeType(value, type);
					}
					catch { }
				}
			}
			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToDateTime																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the DateTime representation of bytes in the specified offset
		/// of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <returns>
		/// DateTime value converted from content of buffer.
		/// </returns>
		public static DateTime ToDateTime(byte[] source, UInt32 offset)
		{
			return ToDateTime(source, offset, 8);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a Date Time, converted from binary values at the specified
		/// offset within the buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer containing the binary Date Time.
		/// </param>
		/// <param name="offset">
		/// Offset at which the Date Time value begins.
		/// </param>
		/// <param name="length">
		/// Number of bytes occupied by the value. Valid choices in this context
		/// are 4 and 8.
		/// </param>
		/// <returns>
		/// Date Time, converted from binary values.
		/// </returns>
		public static DateTime ToDateTime(byte[] source, UInt32 offset,
			UInt32 length)
		{
			DateTime dt = DateTime.MinValue;    //	Date Time.
			UInt64 udtl;                        //	Long Date Time.
			UInt32 udts;                        //	Short Date Time.

			if(length == 4)
			{
				udts = ToUInt32(source, offset);
				try
				{
					dt = Convert.ToDateTime(udts);
				}
				catch { }
			}
			else
			{
				udtl = ToUInt32(source, offset);
				try
				{
					dt = Convert.ToDateTime(udtl);
				}
				catch { }
			}
			return dt;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a value converted to DateTime, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to a DateTime.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a DateTime representation of that
		/// value. Otherwise, DateTime.MinValue.
		/// </returns>
		public static DateTime ToDateTime(object value)
		{
			return (DateTime)ToValueOfType(value, SysType.DateTime, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a value converted to DateTime, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to a DateTime.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a DateTime representation of that
		/// value. Otherwise, DateTime.MinValue.
		/// </returns>
		public static DateTime ToDateTime(string value)
		{
			DateTime rv = DateTime.MinValue;

			try
			{
				rv = DateTime.Parse(value);
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToDecimal																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Decimal representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// Decimal value converted from the abstract object.
		/// </returns>
		public static Decimal ToDecimal(object value)
		{
			return (Decimal)ToValueOfType(value, SysType.Decimal, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a number converted to Decimal, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to a Decimal.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a Decimal representation of that value.
		/// Otherwise, the caller's specified default value.
		/// </returns>
		public static Decimal ToDecimal(string value, Decimal defaultValue)
		{
			Decimal rv = defaultValue;
			try
			{
				rv = Convert.ToDecimal(GetNumber(value));
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToDouble																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Double floating point value from the specified location in the
		/// buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer from which to retrieve value.
		/// </param>
		/// <param name="offset">
		/// Offset at which to begin retrieval.
		/// </param>
		/// <returns>
		/// Double floating point value found at the specified location within the
		/// buffer.
		/// </returns>
		public static Double ToDouble(byte[] source, UInt32 offset)
		{
			return BitConverter.ToDouble(source, (int)offset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a Double floating point value from the caller specified value.
		/// </summary>
		/// <param name="value">
		/// Abstract value to convert to double.
		/// </param>
		/// <returns>
		/// Double floating point representation of the original value.
		/// </returns>
		public static Double ToDouble(object value)
		{
			return (double)ToValueOfType(value, SysType.Double, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a number converted to Double, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to Double.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a Double floating point representation
		/// of that value. Otherwise, the caller's specified default value.
		/// </returns>
		public static double ToDouble(string value, double defaultValue)
		{
			double rv = defaultValue;
			try
			{
				rv = Convert.ToDouble(GetNumber(value));
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToGuid																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Guid value from the specified location in the buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer from which to retrieve value.
		/// </param>
		/// <param name="offset">
		/// Offset at which to begin retrieval.
		/// </param>
		/// <returns>
		/// Guid value found at the specified location within the buffer.
		/// </returns>
		public static Guid ToGuid(byte[] source, UInt32 offset)
		{
			byte[] ba = new byte[16];
			Guid rv;

			Copy(source, ba, offset, 0, 16);
			rv = new Guid(ba);
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a Guid value from the specified object.
		/// </summary>
		/// <param name="source">
		/// Object to convert to Guid.
		/// </param>
		/// <param name="defaultValue">
		/// Default Value to use if object can not be converted.
		/// </param>
		/// <returns>
		/// Guid value converted from the caller's value.
		/// </returns>
		public static Guid ToGuid(object source, Guid defaultValue)
		{
			Guid rv = defaultValue;

			try
			{
				rv = (Guid)ToValueOfType(source, typeof(Guid), false);
			}
			catch { }
			return rv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a Guid value from the specified string.
		/// </summary>
		/// <param name="value">
		/// String Version of Guid.
		/// </param>
		/// <param name="defaultValue">
		/// Value to be returned if the string can not be converted.
		/// </param>
		/// <returns>
		/// Guid value converted from the caller's string.
		/// </returns>
		public static Guid ToGuid(string value, Guid defaultValue)
		{
			Guid rv = defaultValue;

			try
			{
				rv = new Guid(value);
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToHex																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a string of byte values represented in Hexadecimal.
		/// </summary>
		/// <param name="value">
		/// A single byte for which a Hex conversion is requested.
		/// </param>
		/// <returns>
		/// String of Hex Values translated from the caller's byte content.
		/// </returns>
		public static string ToHex(byte value)
		{
			string rs = Convert.ToString(value, 16).PadLeft(2, '0');
			//	if(rs.Length == 1)
			//	{
			//		rs = "0" + rs;
			//	}
			return rs;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a string of byte values represented in Hexadecimal.
		/// </summary>
		/// <param name="value">
		/// Array of bytes to convert to Hex Digits.
		/// </param>
		/// <param name="style">
		/// The Hexadecimal Style to apply to the return string.
		/// </param>
		/// <returns>
		/// String of Hex Values translated from the caller's byte content.
		/// </returns>
		/// <remarks>
		/// <p>Common Style Combinations:<br/>
		/// HexStyle.Single | HexStyle.Prefix. [0x1234].<br/>
		/// HexStyle.Single | HexStyle.GUID.
		/// [12345678-1234-1234-1234-123456789abc].<br/>
		/// HexStyle.Delimited | HexStyle.Prefix. [0x01\t0x02\t0x03].<br/>
		/// HexStyle.Delimited | HexStyle.Space. [01 02 03].<br/>
		/// HexStyle.Delimited | HexStyle.Prefix | HexStyle.Space | HexStyle.Comma.
		/// [0x01, 0x02, 0x03].
		/// </p>
		/// </remarks>
		public static string ToHex(byte[] value, HexStyle style)
		{
			int lc = 0;
			int lp = 0;
			int lpe = 0;    //	LP End.
			int lps = 0;    //	LP Start.
			StringBuilder sb = new StringBuilder();

			if(value != null)
			{
				lc = value.Length;
				if(style == HexStyle.Default ||
					(style & HexStyle.SingleKey) != 0)
				{
					//	Single Key.
					if((style & HexStyle.GUID) == 0)
					{
						if(lc != 0 && (style & HexStyle.Prefix) != 0)
						{
							//	If we want to use prefix on this item, then continue.
							sb.Append("0x");
						}
						for(lp = 0; lp < lc; lp++)
						{
							//	Append all of the characters.
							sb.Append(ToHex(value[lp]));
						}
					}
					else if((style & HexStyle.GUID) != 0)
					{
						//	GUID.
						lps = 0;
						lpe = 4;
						for(lp = lps; lp < lpe && lp < lc; lp++)
						{
							sb.Append(ToHex(value[lp]));
						}
						sb.Append("-");
						lps = lpe;
						lpe = lps + 2;
						for(lp = lps; lp < lpe && lp < lc; lp++)
						{
							sb.Append(ToHex(value[lp]));
						}
						sb.Append("-");
						lps = lpe;
						lpe = lps + 2;
						for(lp = lps; lp < lpe && lp < lc; lp++)
						{
							sb.Append(ToHex(value[lp]));
						}
						sb.Append("-");
						lps = lpe;
						lpe = lps + 2;
						for(lp = lps; lp < lpe && lp < lc; lp++)
						{
							sb.Append(ToHex(value[lp]));
						}
						sb.Append("-");
						lps = lpe;
						lpe = lc;
						for(lp = lps; lp < lpe && lp < lc; lp++)
						{
							sb.Append(ToHex(value[lp]));
						}
					}
				}
				else
				{
					//	Delimited multiple values.
					for(lp = 0; lp < lc; lp++)
					{
						if(sb.Length != 0)
						{
							if((style & (HexStyle.Comma | HexStyle.Space)) != 0)
							{
								if((style & HexStyle.Comma) != 0)
								{
									sb.Append(",");
								}
								if((style & HexStyle.Space) != 0)
								{
									sb.Append(" ");
								}
							}
							else
							{
								sb.Append("\t");
							}
						}
						if((style & HexStyle.Prefix) != 0)
						{
							sb.Append("0x");
						}
						sb.Append(ToHex(value[lp]));
					}
				}
			}
			return sb.ToString().ToLower();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a hexadecimal string representing the caller-specified value.
		/// </summary>
		/// <param name="value">
		/// Integer value to be converted.
		/// </param>
		/// <param name="style">
		/// The Hexadecimal Style to apply to the return string.
		/// </param>
		/// <returns>
		/// String of one or more Hex Values translated from the caller's value.
		/// </returns>
		public static string ToHex(int value, HexStyle style)
		{
			StringBuilder sb = new StringBuilder();

			//	TODO: Return multiple bytes if necessary.
			if((style & HexStyle.Byte) != 0)
			{
				value &= 0xff;
				sb.Append(ToHex((byte)value));
			}
			else if((style & HexStyle.Word) != 0)
			{
				value &= 0xffff;
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(4, '0'));
			}
			else if((style & HexStyle.DoubleWord) != 0)
			{
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(8, '0'));
			}
			else if((style & HexStyle.Fill) != 0)
			{
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(8, '0'));
			}
			if((style & HexStyle.Prefix) != 0)
			{
				sb.Insert(0, "0x");
			}
			return sb.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a hexadecimal string representing the caller-specified value.
		/// </summary>
		/// <param name="value">
		/// Integer value to be converted.
		/// </param>
		/// <param name="style">
		/// The Hexadecimal Style to apply to the return string.
		/// </param>
		/// <returns>
		/// String of one or more Hex Values translated from the caller's value.
		/// </returns>
		public static string ToHex(long value, HexStyle style)
		{
			StringBuilder sb = new StringBuilder();

			//	TODO: Return multiple bytes if necessary.
			if((style & HexStyle.Byte) != 0)
			{
				value &= 0xff;
				sb.Append(ToHex((byte)value));
			}
			else if((style & HexStyle.Word) != 0)
			{
				value &= 0xffff;
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(4, '0'));
			}
			else if((style & HexStyle.DoubleWord) != 0)
			{
				value &= 0xffffffff;
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(8, '0'));
			}
			else if((style & HexStyle.Fill) != 0)
			{
				sb.Append(Convert.ToString(value, 16).ToLower().PadLeft(16, '0'));
			}
			if((style & HexStyle.Prefix) != 0)
			{
				sb.Insert(0, "0x");
			}
			return sb.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a string of byte values represented in Hexadecimal.
		/// </summary>
		/// <param name="value">
		/// String of characters to convert to Hex Digits.
		/// </param>
		/// <param name="style">
		/// The Hexadecimal Style to apply to the return string.
		/// </param>
		/// <returns>
		/// String of Hex Values translated from the caller's byte content.
		/// </returns>
		public static string ToHex(string value, HexStyle style)
		{
			byte[] ba;
			long i64;
			int lc = 0;
			int lp = 0;
			int lpl = 0;
			string rs = "";
			StringBuilder sb = new StringBuilder();

			if((style & HexStyle.Numeric) != 0)
			{
				//	If the input is in numeric form, then we need to convert to hex
				//	first, then present the binary bytes to the hex formatter.
				i64 = Convert.ToInt64(value, 10);
				rs = Convert.ToString(i64, 16);
				sb.Append(rs);
				lc = sb.Length / 2;
				if(lc % 2 != 0)
				{
					sb.Insert(0, "0");
					lc = sb.Length / 2;
				}
				ba = new byte[lc];
				rs = sb.ToString();
				for(lp = 0; lp < lc; lp++, lpl += 2)
				{
					ba[lp] = Convert.ToByte(rs.Substring(lpl, 2), 16);
				}
				rs = ToHex(ba, style);
			}
			else
			{
				//	Input is in ASCII form.
				rs = ToHex(ToByteArray(value), style);
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToInt32																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Integer representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// Int32 value converted from the abstract object.
		/// </returns>
		public static Int32 ToInt32(object value)
		{
			return (Int32)ToValueOfType(value, SysType.Int32, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a number converted to Int32, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to an Int32.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, an Int32 representation of that value.
		/// Otherwise, the caller's specified default value.
		/// </returns>
		public static int ToInt32(string value, int defaultValue)
		{
			int rv = defaultValue;
			try
			{
				rv = Convert.ToInt32(GetNumber(value));
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToInt32Positive																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a number converted to a positive Int32, with no exception thrown
		/// in the case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to an Int32.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a positive Int32 representation of
		/// that value. Otherwise, the caller's specified default value.
		/// </returns>
		public static int ToInt32Positive(string value, int defaultValue)
		{
			int rv = defaultValue;
			try
			{
				rv = Convert.ToInt32(GetNumber(value));
				if(rv < 0)
				{
					rv = 0 - rv;
				}
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToInt64																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Integer representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// Int64 value converted from the abstract object.
		/// </returns>
		public static Int64 ToInt64(object value)
		{
			return (Int64)ToValueOfType(value, SysType.Int64, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a number converted to Int64, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to an Int64.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, an Int64 representation of that value.
		/// Otherwise, the caller's specified default value.
		/// </returns>
		public static long ToInt64(string value, long defaultValue)
		{
			long rv = defaultValue;
			try
			{
				rv = Convert.ToInt64(GetNumber(value));
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToPluralText																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Plural Text version of the caller-supplied singular text.
		/// </summary>
		/// <param name="value">
		/// Singular Text to be converted.
		/// </param>
		/// <returns>
		/// Pluralized Text.
		/// </returns>
		public static string ToPluralText(string value)
		{
			string rs = value;

			if(rs != null)
			{
				if(rs.Length == 1)
				{
					rs += "s";
				}
				else if(rs.Length > 1)
				{
					if(rs.Substring(rs.Length - 1, 1) != "s")
					{
						if(rs.Substring(rs.Length - 1, 1) == "y")
						{
							rs = rs.Substring(0, rs.Length - 1) + "ies";
						}
						else
						{
							rs += "s";
						}
					}
				}
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToRegEx																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Regular Expression Pattern, converted from the caller's
		/// string.
		/// </summary>
		/// <param name="pattern">
		/// Normal string to convert.
		/// </param>
		/// <returns>
		/// Regular Expression pattern.
		/// </returns>
		public static string ToRegEx(string pattern)
		{
			string ws;

			ws = pattern.Replace(@"\", @"\\");
			ws = ws.Replace("\"", "\\\"");
			ws = ws.Replace("(", @"\(");
			ws = ws.Replace(")", @"\)");
			ws = ws.Replace("<", @"\<");
			ws = ws.Replace(">", @"\>");
			ws = ws.Replace("[", @"\[");
			ws = ws.Replace("]", @"\]");
			ws = ws.Replace("{", @"\{");
			ws = ws.Replace("}", @"\}");
			ws = ws.Replace("?", @"\?");
			ws = ws.Replace("#", @"\#");
			ws = ws.Replace("*", @"\*");
			ws = ws.Replace("+", @"\+");
			ws = ws.Replace("^", @"\^");
			ws = ws.Replace("&", @"\&");
			ws = ws.Replace("$", @"\$");
			ws = ws.Replace("|", @"\|");
			ws = ws.Replace(":", @"\:");
			ws = ws.Replace(".", @"\.");
			ws = ws.Replace(",", @"\,");

			return ws;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToSingle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a single floating point value from the specified location in the
		/// caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer from which the value will be read.
		/// </param>
		/// <param name="offset">
		/// Offset at which to begin reading.
		/// </param>
		/// <returns>
		/// Single floating point value found at the specified location in the
		/// buffer.
		/// </returns>
		public static Single ToSingle(byte[] source, UInt32 offset)
		{
			return BitConverter.ToSingle(source, (int)offset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the Single representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// Single value converted from the abstract object.
		/// </returns>
		public static Single ToSingle(object value)
		{
			return (Single)ToValueOfType(value, SysType.Single, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a number converted to Single, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to a Single.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value can not be converted.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a Single representation of that value.
		/// Otherwise, the caller's specified default value.
		/// </returns>
		public static Single ToSingle(string value, Single defaultValue)
		{
			Single rv = defaultValue;
			try
			{
				rv = Convert.ToSingle(GetNumber(value));
			}
			catch { }
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToSocialSecurity																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a formatted value for the provided unformatted Social Security
		/// Number.
		/// </summary>
		/// <param name="value">
		/// Raw Social Security Number.
		/// </param>
		/// <returns>
		/// Formatted Social Security Number.
		/// </returns>
		public static string ToSocialSecurity(object value)
		{
			char[] fca;               //	Format Character Array.
			StringBuilder ffb = new StringBuilder();  //	Format Find String.
			StringBuilder frb = new StringBuilder();  //	Format Replacement String.
			string fs = "###-##-####";
			int i = 0;
			int ip = 0;
			string rs = "";
			StringBuilder sb;         //	Working String Builder.
			int sl = 0;

			rs = value.ToString();

			//	Build the Find String.
			sl = rs.Length;
			if(sl != 0)
			{
				sb = new StringBuilder();
				for(i = 0, ip = fs.Length - 1; i < sl && ip >= 0; i++, ip--)
				{
					while(fs.Substring(ip, 1) != "#")
					{
						sb.Insert(0, fs.Substring(ip, 1));
						ip--;
					}
					sb.Insert(0, "#");
				}
				if(i < sl)
				{
					sb.Insert(0, "-");
				}
				while(i < sl)
				{
					sb.Insert(0, "#");
					i++;
				}
				fs = sb.ToString();
			}
			for(i = 0; i < sl; i++)
			{
				ffb.Append(@"(\d{1})");
			}

			//	Build the Replace String.
			if(sl != 0)
			{
				i = 1;    //	Don't return a 0 group, since that means the entire match.
				fca = fs.ToCharArray();
				foreach(char c in fca)
				{
					if(c == '#')
					{
						//	If this is a number placeholder, then set the parameter.
						frb.Append("$" + i.ToString());
						i++;
					}
					else
					{
						//	Otherwise, just place the formatting text.
						frb.Append(c);
					}
				}
				rs = Regex.Replace(rs, ffb.ToString(), frb.ToString());
			}
			else
			{
				rs = "";
			}

			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a string of byte values.
		/// </summary>
		/// <param name="source">
		/// Array of bytes to convert to characters.
		/// </param>
		/// <returns>
		/// String of characters converted directly from bytes.
		/// </returns>
		/// <remarks>
		/// Note: This result might not be readable to the human eye. For a
		/// Hexadecimal representation of the values of a byte array, use the
		/// ToHex() method.
		/// </remarks>
		public static string ToString(byte[] source)
		{
			char[] ca = new char[0];
			int lc = 0;
			int lp = 0;
			StringBuilder sb = new StringBuilder();

			if(source != null)
			{
				lc = source.Length;
				ca = new char[lc];
				for(lp = 0; lp < lc; lp++)
				{
					ca[lp] = (char)source[lp];
				}
			}
			sb.Append(ca);
			return sb.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the String representation of bytes in the specified offset of
		/// the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <returns>
		/// String balue converted from content of buffer.
		/// </returns>
		/// <remarks>
		/// This method assumes that the byte at the specified offset contains a
		/// count of bytes to read.
		/// </remarks>
		public static string ToString(byte[] source, UInt32 offset)
		{
			return ToString(source, offset, true);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the String representation of bytes in the specified offset
		/// of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <param name="count">
		/// Value indicating whether the first byte at the offset of the buffer
		/// contains the count of characters to read.
		/// </param>
		/// <returns>
		/// String, converted from information at specified offset of buffer.
		/// </returns>
		public static string ToString(byte[] source, UInt32 offset, bool count)
		{
			string rs;

			if(count)
			{
				rs = ToString(source, offset + 1, (UInt32)source[offset]);
			}
			else
			{
				rs = ASCIIEncoding.ASCII.GetString(source, (int)offset,
					source.Length - (int)offset);
			}
			return rs;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the String representation of bytes in the specified offset
		/// of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <param name="length">
		/// Length of string to read.
		/// </param>
		/// <returns>
		/// String value converted from content of buffer.
		/// </returns>
		public static string ToString(byte[] source, UInt32 offset, UInt32 length)
		{
			return ASCIIEncoding.ASCII.GetString(source, (int)offset, (int)length);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the formatted String representation of the specified value.
		/// </summary>
		/// <param name="value">
		/// Instance of an object to inspect.
		/// </param>
		/// <returns>
		/// String representation of caller's object.
		/// </returns>
		public static string ToString(object value)
		{
			return (string)ToValueOfType(value, SysType.String, false);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the formatted String representation of the specified value.
		/// </summary>
		/// <param name="value">
		/// Instance of an object to inspect.
		/// </param>
		/// <param name="format">
		/// Display Format to apply to the string.
		/// </param>
		/// <returns>
		/// Formatted string representation of caller's object.
		/// </returns>
		/// <remarks>
		/// <p>
		/// Allowable formats for Boolean values are the following.
		/// <list type="bullet">
		/// <item>YesNo</item>. 'Yes' or 'No' are returned.
		/// <item>YN</item>. 'Y' or 'N' are returned.
		/// <item>TrueFalse</item>. 'True' or 'False'.
		/// <item>TF</item>. 'T' or 'F'.
		/// <item>10</item>. '1' or '0'.
		/// <item>(blank)</item>. The system default string is returned.
		/// </list>
		/// </p>
		/// </remarks>
		public static string ToString(object value, string format)
		{
			bool bv;            //	Working Boolean Value.
			string rs = "";     //	Return String.
			Type t;
			string tl;          //	Lower case value.

			if(value != null)
			{
				//	If the caller supplied a value, then continue.
				t = value.GetType();

				if(value is Boolean)
				{
					bv = (bool)value;
					tl = format.ToLower();
					switch(tl)
					{
						case "yesno":
							rs = (bv ? "Yes" : "No");
							break;
						case "yn":
							rs = (bv ? "Y" : "N");
							break;
						case "truefalse":
							rs = (bv ? "True" : "False");
							break;
						case "tf":
							rs = (bv ? "T" : "F");
							break;
						case "10":
							rs = (bv ? "1" : "0");
							break;
						default:
							rs = (bv ? Boolean.TrueString : Boolean.FalseString);
							break;
					}
				}
				else if(value is DateTime dateVal)
				{
					rs = dateVal.ToString(format);
				}
				else if(value is Decimal decimalVal)
				{
					rs = decimalVal.ToString(format);
				}
				else if(value is Double doubleVal)
				{
					rs = doubleVal.ToString(format);
				}
				else if(value is Int16 int16Val)
				{
					rs = int16Val.ToString(format);
				}
				else if(value is Int32 int32Val)
				{
					rs = int32Val.ToString(format);
				}
				else if(value is Int64 int64Val)
				{
					rs = int64Val.ToString(format);
				}
				else if(value is Single singleVal)
				{
					rs = singleVal.ToString(format);
				}
				else if(value is String stringVal)
				{
					rs = String.Format(stringVal, format);
				}
				else if(value is UInt16 uint16Val)
				{
					rs = uint16Val.ToString(format);
				}
				else if(value is UInt32 uint32Val)
				{
					rs = uint32Val.ToString(format);
				}
				else if(value is UInt64 uint64Val)
				{
					rs = uint64Val.ToString(format);
				}
				else
				{
					rs = value.ToString();
				}
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToTelephone																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a formatted value for the provided unformatted Telephone Number.
		/// </summary>
		/// <param name="value">
		/// Raw Telephone Number.
		/// </param>
		/// <returns>
		/// Formatted Telephone Number.
		/// </returns>
		public static string ToTelephone(object value)
		{
			bool bf = false;          //	Match Found.
			StringEventArgs ea;       //	Area Code Event.
			char[] fca;               //	Format Character Array.
			StringBuilder ffb = new StringBuilder();  //	Format Find String.
			StringBuilder frb = new StringBuilder();  //	Format Replacement String.
			string fs = "###-##-####-####";
			int i = 0;
			int ip = 0;
			Match mi;                 //	Working Match.
			string rs = "";
			StringBuilder sb;         //	Working String.
			string sn = "";           //	Number String.
			string sx = "";           //	Extension String.
			int sl = 0;               //	String Length.
			string ws = value.ToString();   //	Working String.

			mi = Regex.Match(ws, @"(?<f>\s+-\s+\D+$)");
			if(mi.Success && mi.Index > 0)
			{
				sn = ws.Substring(0, mi.Index);
				sx = ws.Substring(mi.Index, mi.Length);
				bf = true;
			}
			if(!bf)
			{
				mi = Regex.Match(ws, @"(?<f>(ext|x)\W+.*$)");
				if(mi.Success && mi.Index > 0)
				{
					sn = ws.Substring(0, mi.Index);
					sx = ws.Substring(mi.Index, mi.Length);
					bf = true;
				}
				else
				{
					sn = ws;
				}
			}

			rs = sn.ToString().ToLower();
			//	Resolve Letters.
			sb = new StringBuilder();
			fca = rs.ToCharArray();
			foreach(char c in fca)
			{
				switch(c)
				{
					case 'a':
					case 'b':
					case 'c':
						sb.Append("2");
						break;
					case 'd':
					case 'e':
					case 'f':
						sb.Append("3");
						break;
					case 'g':
					case 'h':
					case 'i':
						sb.Append("4");
						break;
					case 'j':
					case 'k':
					case 'l':
						sb.Append("5");
						break;
					case 'm':
					case 'n':
					case 'o':
						sb.Append("6");
						break;
					case 'p':
					case 'q':
					case 'r':
					case 's':
						sb.Append("7");
						break;
					case 't':
					case 'u':
					case 'v':
						sb.Append("8");
						break;
					case 'w':
					case 'x':
					case 'y':
					case 'z':
						sb.Append("9");
						break;
					default:
						sb.Append(c);
						break;
				}
			}
			rs = sb.ToString();
			rs = Regex.Replace(rs, @"(?<f>[^0-9]*)", "");

			//	Build the Find String.
			sl = rs.Length;
			if(sl != 0)
			{
				if(sl == 7)
				{
					if(RequestAreaCode != null)
					{
						ea = new StringEventArgs();
						RequestAreaCode(null, ea);
						if(ea.Value.Length != 0)
						{
							rs = Regex.Replace(ea.Value, @"(?<f>[^0-9]*)", "") + rs;
							sl = rs.Length;
						}
					}
				}
				if(sl == 10)
				{
					fs = "(###) ###-####";
				}
				else if(sl == 11 && rs.Substring(0, 1) == "1")
				{
					fs = "#-###-###-####";
				}
				else
				{
					sb = new StringBuilder();
					for(i = 0, ip = fs.Length - 1; i < sl && ip >= 0; i++, ip--)
					{
						while(fs.Substring(ip, 1) != "#")
						{
							sb.Insert(0, fs.Substring(ip, 1));
							ip--;
						}
						sb.Insert(0, "#");
					}
					if(i < sl)
					{
						sb.Insert(0, "-");
					}
					while(i < sl)
					{
						sb.Insert(0, "#");
						i++;
					}
					fs = sb.ToString();
				}
			}
			for(i = 0; i < sl; i++)
			{
				ffb.Append(@"(\d{1})");
			}

			//	Build the Replace String.
			if(sl != 0)
			{
				i = 1;    //	Don't return a 0 group, since that means the entire match.
				fca = fs.ToCharArray();
				foreach(char c in fca)
				{
					if(c == '#')
					{
						//	If this is a number placeholder, then set the parameter.
						frb.Append("$" + i.ToString());
						i++;
					}
					else
					{
						//	Otherwise, just place the formatting text.
						frb.Append(c);
					}
				}
				rs = Regex.Replace(rs, ffb.ToString(), frb.ToString());
			}
			else
			{
				rs = "";
			}
			if(sx.Length != 0)
			{
				if(rs.Length != 0)
				{
					rs += " ";
				}
				rs += sx.Trim();
			}

			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToTimeSpan																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value converted to TimeSpan, with no exception thrown in the
		/// case that the specified value is not valid.
		/// </summary>
		/// <param name="value">
		/// Value to convert to a TimeSpan.
		/// </param>
		/// <returns>
		/// If the caller's value was valid, a TimeSpan representation of that
		/// value. Otherwise, TimeSpan.MinValue.
		/// </returns>
		public static TimeSpan ToTimeSpan(object value)
		{
			return (TimeSpan)ToValueOfType(value, SysType.TimeSpan, false);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToTitleCase																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a representation of the caller's string formatted as Title Case
		/// text.
		/// </summary>
		/// <param name="value">
		/// The value to format.
		/// </param>
		/// <returns>
		/// Formatted result.
		/// </returns>
		public static string ToTitleCase(string value)
		{
			Group g;                //	Working Group.
			int lp = -1;            //	Last Position.
			MatchCollection mc;     //	Working Matches Collection.
			StringBuilder sb = new StringBuilder();
			string ws = value;

			mc = Regex.Matches(ws, @"(?i:(^|[^0-9a-z])(?<b>[0-9a-z]))");
			foreach(Match m in mc)
			{
				g = m.Groups["b"];
				if(g != null)
				{
					if(g.Index > 0)
					{
						//	If some portion of the string was skipped since the last match,
						//	then retrieve the middle portion.
						sb.Append(value.Substring(lp + 1, g.Index - (lp + 1)));
					}
					sb.Append(g.Value.ToUpper());
					lp = g.Index;
				}
			}
			if(value.Length > lp + 1)
			{
				//	If the length of the source is longer that the start of the last
				//	word, then paste the end of the string.
				sb.Append(value.Substring(lp + 1, value.Length - (lp + 1)));
			}

			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToUInt16																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Unsigned Integer representation of the bytes in the
		/// specified offset of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Source at which bytes will be read.
		/// </param>
		/// <returns>
		/// UInt value converted from content of buffer.
		/// </returns>
		public static UInt16 ToUInt16(byte[] source, UInt32 offset)
		{
			int co;           //	Current Offset.
			UInt16 uv = 0;    //	Unsigned Value.

			co = (int)offset;
			uv += (UInt16)(source[co++] * 0x0001);
			uv += (UInt16)(source[co++] * 0x0100);
			return uv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToUInt32																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Unsigned Integer representation of the bytes in the
		/// specified offset of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be read.
		/// </param>
		/// <returns>
		/// UInt value converted from content of buffer.
		/// </returns>
		public static UInt32 ToUInt32(byte[] source, UInt32 offset)
		{
			int co;           //	Current Offset.
			UInt32 uv = 0;    //	Unsigned Value.

			co = (int)offset;
			uv += ((UInt32)source[co++]) * 0x00000001;
			uv += ((UInt32)source[co++]) * 0x00000100;
			uv += ((UInt32)source[co++]) * 0x00010000;
			uv += ((UInt32)source[co++]) * 0x01000000;
			return uv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the Unsigned Integer representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// UInt32 value converted from the abstract object.
		/// </returns>
		public static UInt32 ToUInt32(object value)
		{
			return (UInt32)ToValueOfType(value, SysType.UInt32, false);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToUInt64																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Unsigned Integer representation of the bytes in the
		/// specified offset of the caller's buffer.
		/// </summary>
		/// <param name="source">
		/// Buffer to be copied.
		/// </param>
		/// <param name="offset">
		/// Offset in the Target at which bytes will be read.
		/// </param>
		/// <returns>
		/// UInt value converted from content of buffer.
		/// </returns>
		public static UInt64 ToUInt64(byte[] source, UInt32 offset)
		{
			int co;           //	Current Offset.
			UInt64 uv = 0;    //	Unsigned Value.

			co = (int)offset;
			uv += ((UInt64)source[co++]) * 0x0000000000000001;
			uv += ((UInt64)source[co++]) * 0x0000000000000100;
			uv += ((UInt64)source[co++]) * 0x0000000000010000;
			uv += ((UInt64)source[co++]) * 0x0000000001000000;
			uv += ((UInt64)source[co++]) * 0x0000000100000000;
			uv += ((UInt64)source[co++]) * 0x0000010000000000;
			uv += ((UInt64)source[co++]) * 0x0001000000000000;
			uv += ((UInt64)source[co++]) * 0x0100000000000000;
			return uv;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the Unsigned Integer representation of the specified object.
		/// </summary>
		/// <param name="value">
		/// Numeric value.
		/// </param>
		/// <returns>
		/// UInt64 value converted from the abstract object.
		/// </returns>
		public static UInt64 ToUInt64(object value)
		{
			return (UInt64)ToValueOfType(value, SysType.UInt64, false);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToValueOfType																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the caller's value, casted to the specified destination value.
		/// </summary>
		/// <param name="value">
		/// The Value to Convert.
		/// </param>
		/// <param name="type">
		/// The Destination Type of the Value.
		/// </param>
		/// <param name="allowNull">
		/// Value indicating whether null return value is allowed.
		/// </param>
		/// <returns>
		/// Caller's Value, casted to the specified Type.
		/// </returns>
		public static object ToValueOfType(object value, Type type, bool allowNull)
		{
			return ToValueOfType(value, GetSysType(type), allowNull);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the caller's value, casted to the specified destination value.
		/// </summary>
		/// <param name="value">
		/// The Value to Convert.
		/// </param>
		/// <param name="type">
		/// The Destination Type of the Value.
		/// </param>
		/// <param name="allowNull">
		/// Value indicating whether null return value is allowed.
		/// </param>
		/// <returns>
		/// Caller's Value, casted to the specified Type.
		/// </returns>
		public static object ToValueOfType(object value, SysType type,
			bool allowNull)
		{
			object ro = null;   //	Return Object.
			object o = value;   //	Working Object.
			SysType tt = type;
			SysType vt = GetSysType(value);

			//	Remove formatting on numbers.
			if(vt == SysType.String)
			{
				if(IsNumericType(tt))
				{
					o = Regex.Replace(o.ToString(), "[$, a-zA-Z]", "");
				}
			}

			if(value != null)
			{
				//	Process the To Type.
				if(!allowNull)
				{
					switch(tt)
					{
						case SysType.Boolean:
							ro = false;
							break;
						case SysType.Byte:
							ro = (byte)0;
							break;
						case SysType.ByteArray:
							//	If this is an unhandled type, then retain it.
							ro = new byte[0];
							break;
						case SysType.Char:
							ro = (char)0;
							break;
						case SysType.CharArray:
							ro = new char[0];
							break;
						case SysType.DateTime:
							ro = DateTime.MinValue;
							break;
						case SysType.DBNull:
							ro = DBNull.Value;
							break;
						case SysType.Decimal:
							ro = (decimal)0;
							break;
						case SysType.Double:
							ro = (double)0;
							break;
						case SysType.Enum:
							ro = (int)0;
							break;
						case SysType.Guid:
							ro = Guid.Empty;
							break;
						case SysType.Image:
#if WindowsOnly
							ro = new Bitmap(1, 1);
#endif
							break;
						case SysType.Int16:
							ro = (Int16)0;
							break;
						case SysType.Int32:
							ro = (Int32)0;
							break;
						case SysType.Int64:
							ro = (Int64)0;
							break;
						case SysType.LongString:
							ro = new LongString("");
							break;
						case SysType.Null:
							ro = null;
							break;
						case SysType.SByte:
							ro = (sbyte)0;
							break;
						case SysType.Single:
							ro = (Single)0;
							break;
						case SysType.String:
							ro = "";
							break;
						case SysType.StringArray:
							ro = new string[0];
							break;
						case SysType.TimeSpan:
							ro = TimeSpan.MinValue;
							break;
						case SysType.Type:
							ro = Type.GetType("System.Void");
							break;
						case SysType.UInt16:
							ro = (UInt16)0;
							break;
						case SysType.UInt32:
							ro = (UInt32)0;
							break;
						case SysType.UInt64:
							ro = (UInt64)0;
							break;
						default:
							//	If this is an unhandled type, then retain it.
							ro = o;
							break;
					}
				}
				switch(tt)
				{
					case SysType.Boolean:
						if(allowNull && vt == SysType.String && ((string)value).Length == 0)
						{
							ro = null;
						}
						else
						{
							ro = ToBoolean(o.ToString());
						}
						break;
					case SysType.Byte:
						try
						{
							ro = Convert.ToByte(o);
						}
						catch { }
						break;
					case SysType.ByteArray:
						//	If this is an unhandled type, then retain it.
						if(vt == tt)
						{
							ro = o;
						}
						else if(vt == SysType.String && ((string)value).Length == 0)
						{
							ro = null;
						}
						else
						{
							ro = o;
						}
						break;
					case SysType.Char:
						try
						{
							ro = Convert.ToChar(o);
						}
						catch { }
						break;
					case SysType.CharArray:
						//	If this is an unhandled type, then retain it.
						ro = o;
						break;
					case SysType.DateTime:
						try
						{
							ro = Convert.ToDateTime(o);
						}
						catch { }
						break;
					case SysType.DBNull:
						ro = DBNull.Value;
						break;
					case SysType.Decimal:
						//	If this is an unhandled type, then retain it.
						try
						{
							ro = Convert.ToDecimal(o);
						}
						catch { }
						break;
					case SysType.Double:
						try
						{
							ro = Convert.ToDouble(o);
						}
						catch { }
						break;
					case SysType.Enum:
						//	If this is an unhandled type, then retain it.
						ro = o;
						break;
					case SysType.Guid:
						if(vt == SysType.String)
						{
							try
							{
								ro = new Guid((string)value);
							}
							catch { }
						}
						else if(vt == SysType.Guid)
						{
							ro = value;
						}
						else if(vt == SysType.ByteArray)
						{
							ro = ((Guid)value).ToByteArray();
						}
						else
						{
							ro = Guid.Empty;
						}
						break;
					case SysType.Image:
						//	If this is an unhandled type, then retain it.
						if(vt == SysType.String && ((string)value).Length == 0)
						{
							ro = null;
						}
						else if(vt == SysType.String)
						{
							//	If the incoming value is a string, then convert from
							//	base-64.
							ro = Convert.FromBase64String((string)value);
						}
						else
						{
							ro = o;
						}
						break;
					case SysType.Int16:
						try
						{
							ro = Convert.ToInt16(o);
						}
						catch { }
						break;
					case SysType.Int32:
						try
						{
							ro = Convert.ToInt32(o);
						}
						catch { }
						break;
					case SysType.Int64:
						try
						{
							ro = Convert.ToInt64(o);
						}
						catch { }
						break;
					case SysType.LongString:
						if(vt == SysType.String && ((string)value).Length == 0 &&
							allowNull)
						{
							ro = null;
						}
						else if(IsNumeric(value))
						{
							ro = Conversion.GetNumber(value.ToString());
						}
						else
						{
							ro = (LongString)value.ToString();
						}
						break;
					case SysType.Null:
						ro = null;
						break;
					case SysType.SByte:
						try
						{
							ro = Convert.ToSByte(o);
						}
						catch { }
						break;
					case SysType.Single:
						try
						{
							ro = Convert.ToSingle(o);
						}
						catch { }
						break;
					case SysType.String:
						if(vt == SysType.String && ((string)value).Length == 0 &&
							allowNull)
						{
							ro = null;
						}
						else if(IsNumeric(value))
						{
							ro = Conversion.GetNumber(value.ToString());
						}
						else if(vt == SysType.String)
						{
							ro = (string)value;
						}
						else
						{
							ro = value.ToString();
						}
						break;
					case SysType.StringArray:
						//	If this is an unhandled type, then retain it.
						ro = o;
						break;
					case SysType.TimeSpan:
						//	If this is an unhandled type, then retain it.
						ro = o;
						break;
					case SysType.Type:
						//	If this is an unhandled type, then retain it.
						ro = o;
						break;
					case SysType.UInt16:
						try
						{
							ro = Convert.ToUInt16(o);
						}
						catch { }
						break;
					case SysType.UInt32:
						try
						{
							ro = Convert.ToUInt32(o);
						}
						catch { }
						break;
					case SysType.UInt64:
						try
						{
							ro = Convert.ToUInt64(o);
						}
						catch { }
						break;
					default:
						//	If this is an unhandled type, then retain it.
						if(vt == SysType.String && ((string)value).Length == 0 &&
							allowNull)
						{
							ro = null;
						}
						else
						{
							ro = o;
						}
						break;
				}
			}
			else if(!allowNull)
			{
				//	Value is null, but null is not allowed.
				switch(tt)
				{
					case SysType.Boolean:
						ro = false;
						break;
					case SysType.Byte:
						ro = (byte)0;
						break;
					case SysType.ByteArray:
						ro = new byte[0];
						break;
					case SysType.Char:
						ro = (char)0;
						break;
					case SysType.CharArray:
						ro = new char[0];
						break;
					case SysType.DateTime:
						ro = DateTime.MinValue;
						break;
					case SysType.DBNull:
						ro = System.DBNull.Value;
						break;
					case SysType.Decimal:
						ro = 0m;
						break;
					case SysType.Double:
						ro = (double)0;
						break;
					case SysType.Enum:
						ro = null;
						break;
					case SysType.Guid:
						ro = Guid.Empty;
						break;
					case SysType.Image:
#if WindowsOnly
						ro = new Bitmap(8, 8);
#endif
						break;
					case SysType.Int16:
						ro = (Int16)0;
						break;
					case SysType.Int32:
						ro = (Int32)0;
						break;
					case SysType.Int64:
						ro = (Int64)0;
						break;
					case SysType.LongString:
						ro = new LongString();
						break;
					case SysType.Null:
						ro = null;
						break;
					case SysType.SByte:
						ro = (SByte)0;
						break;
					case SysType.Single:
						ro = (Single)0;
						break;
					case SysType.String:
						ro = "";
						break;
					case SysType.StringArray:
						ro = new string[0];
						break;
					case SysType.TimeSpan:
						ro = new TimeSpan(0, 0, 0, 0, 0);
						break;
					case SysType.Type:
						ro = typeof(string);
						break;
					case SysType.UInt16:
						ro = (UInt16)0;
						break;
					case SysType.UInt32:
						ro = (UInt32)0;
						break;
					case SysType.UInt64:
						ro = (UInt64)0;
						break;
					case SysType.Unknown:
						ro = null;
						break;
				}
			}

			return ro;
		}
		//*-----------------------------------------------------------------------*

		//	TODO: ToVerboseNumber. Use TestMod as example.
		//*-----------------------------------------------------------------------*
		//*	ToVerboseNumber																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a verbose (written) version of the caller-supplied numeric
		/// value.
		/// </summary>
		/// <param name="value">
		/// Binary value.
		/// </param>
		/// <returns>
		/// Verbose representation of the caller's number.
		/// </returns>
		public static string ToVerboseNumber(int value)
		{
			int d = 100000;   //	Divisor.
			int dw = 0;       //	Working Divisor Copy.
			int i = 0;        //	Iterator.
			int l = 0;        //	Hundred Spot Lookup.
			int m = 1000000;  //	Mod.
			string ns;        //	Conversion - Get Number.
			int nv = 0;       //	Number Value.
			int r = 0;        //	Result.
			string s;         //	Current String.
			StringBuilder sb = new StringBuilder(); //	Return String.
			int tw;           //	Left Twenty.
			int v = value;    //	Value.

			//			Debug.WriteLine("Calculating Digits for " + v.ToString());
			//			for(i = 0; d > 0; i ++, d /= 10, m /= 10)
			//			{
			//				Debug.WriteLine(" i: " + i.ToString());
			//				Debug.WriteLine(" d: " + d.ToString());
			//				Debug.WriteLine(" m: " + m.ToString());
			//				r = (v % m) / d;
			//				Debug.WriteLine("R:" + r.ToString());
			//			}

			//	Build the Mod and Divisor values.
			d = 1;
			m = 10;
			while(m <= v)
			{
				m *= 10;
				d *= 10;
			}
			for(i = 0; d > 0; i++, d /= 10, m /= 10)
			{
				//	Get the current digit.
				r = (v % m) / d;

				//	Get the possible prefix or substitution.
				dw = d;
				l = 0;
				while(dw > 1)
				{
					l++;
					dw /= 10;
				}
				s = mvHundredspot[l];
				if(s.Length != 0)
				{
					ns = GetNumber(s);
					if(ns.Length != 0)
					{
						//	If we have a number, then we have to look in the corresponding
						//	lookup table.
						nv = ToInt32(ns);
						if(nv == 100)
						{
							if(sb.Length != 0)
							{
								sb.Append(" ");
							}
							sb.Append(mvOnespot[r]);
							sb.Append(" Hundred");
						}
						if(nv == 10)
						{
							//	Get the left twenty value.
							if(sb.Length != 0)
							{
								sb.Append(" ");
							}
							if(r <= 1)
							{
								//	If the Twenty spot is valid, then post it.
								//	Skip the next digit - we're using it here.
								d /= 10;
								m /= 10;
								tw = (r * 10) + ((v % m) / d);
								s = mvOnespot[tw];
								if(s.ToLower() != "zero")
								{
									sb.Append(s);
									sb.Append(" ");
								}
								sb.Append(mvHundredspot[l - 1]);
							}
							else
							{
								//	Otherwise, use the Ten Spot.
								sb.Append(mvTenspot[r]);
							}
							//							sb.Append(" ");
							//							sb.Append(s);
						}
					}
					else
					{
						//	Otherwise, this is a verbose string.
						if(r != 0)
						{
							if(sb.Length != 0)
							{
								sb.Append(" ");
							}
							sb.Append(mvOnespot[r]);
							sb.Append(" ");
							sb.Append(s);
						}
					}
				}
				else if(d == 10)
				{
					//	If this is a number in the Ten Spot, then use tens.
					if(r > 1)
					{
						if(sb.Length > 0)
						{
							sb.Append(" ");
						}
						sb.Append(mvTenspot[r]);
					}
					else
					{
						//	Skip the next digit - we're using it here.
						d /= 10;
						m /= 10;
						tw = (r * 10) + ((v % m) / d);
						if(tw != 0 || sb.Length == 0)
						{
							if(sb.Length > 0)
							{
								sb.Append(" ");
							}
							sb.Append(mvOnespot[tw]);
						}
					}
				}
				else
				{
					//	Otherwise, this is a verbose string.
					if(r != 0)
					{
						if(sb.Length != 0)
						{
							sb.Append(" ");
						}
						sb.Append(mvOnespot[r]);
						sb.Append(" ");
						sb.Append(s);
					}
					else if(sb.Length == 0)
					{
						sb.Append(mvOnespot[r]);
					}
				}
			}
			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	GetVerboseDigit																												*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a verbose value for the digit provided.
		///// </summary>
		///// <param name="value">
		///// The value for which a verbose value will be found.
		///// </param>
		///// <returns>
		///// Verbose version of the value provided.
		///// </returns>
		//private static string GetVerboseDigit(int value)
		//{
		//	string rs = "";

		//	switch(value)
		//	{
		//		case 0:
		//			rs = "Zero";
		//			break;
		//		case 1:
		//			rs = "One";
		//			break;
		//		case 2:
		//			rs = "Two";
		//			break;
		//		case 3:
		//			rs = "Three";
		//			break;
		//		case 4:
		//			rs = "Four";
		//			break;
		//		case 5:
		//			rs = "Five";
		//			break;
		//		case 6:
		//			rs = "Six";
		//			break;
		//		case 7:
		//			rs = "Seven";
		//			break;
		//		case 8:
		//			rs = "Eight";
		//			break;
		//		case 9:
		//			rs = "Nine";
		//			break;
		//		case 10:
		//			rs = "Ten";
		//			break;
		//		case 11:
		//			rs = "Eleven";
		//			break;
		//		case 12:
		//			rs = "Twelve";
		//			break;
		//		case 13:
		//			rs = "Thirteen";
		//			break;
		//		case 14:
		//			rs = "Fourteen";
		//			break;
		//		case 15:
		//			rs = "Fifteen";
		//			break;
		//		case 16:
		//			rs = "Sixteen";
		//			break;
		//		case 17:
		//			rs = "Seventeen";
		//			break;
		//		case 18:
		//			rs = "Eighteen";
		//			break;
		//		case 19:
		//			rs = "Nineteen";
		//			break;
		//		case 20:
		//			rs = "Twenty";
		//			break;
		//		case 30:
		//			rs = "Thirty";
		//			break;
		//		case 40:
		//			rs = "Fourty";
		//			break;
		//		case 50:
		//			rs = "Fifty";
		//			break;
		//		case 60:
		//			rs = "Sixty";
		//			break;
		//		case 70:
		//			rs = "Seventy";
		//			break;
		//		case 80:
		//			rs = "Eighty";
		//			break;
		//		case 90:
		//			rs = "Ninety";
		//			break;
		//		case 100:
		//			rs = "Hundred";
		//			break;
		//		case 1000:
		//			rs = "Thousand";
		//			break;
		//	}
		//	return rs;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToXmlContent																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an Xml inner String formatted for use as a standard Xml Object.
		/// </summary>
		/// <param name="value">
		/// String containing embedded Xml content.
		/// </param>
		/// <returns>
		/// Inner Xml string.
		/// </returns>
		public static string ToXmlContent(string value)
		{
			string rs = Regex.Replace(value, "&amp;", "&");
			rs = Regex.Replace(rs, "&lt;", "<");
			rs = Regex.Replace(rs, "&gt;", ">");
			rs = Regex.Replace(rs, "&quot;", "\"");
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToXmlEmbedded																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an Xml String formatted for embedded transport within
		/// a single element.
		/// </summary>
		/// <param name="value">
		/// Object to be converted to Xml.
		/// </param>
		/// <returns>
		/// String Xml Element Value.
		/// </returns>
		public static string ToXmlEmbedded(object value)
		{
			string rs = "";
			Type t;

			if(value != null)
			{
				t = value.GetType();
				if(value is Boolean)
				{
					rs = value.ToString().ToLower();
				}
				else if(value is DateTime dateTimeVal)
				{
					rs = dateTimeVal.ToString("s");
				}
				else
				{
					rs = value.ToString();
				}
			}
			rs = ToXmlEmbedded(rs);

			return rs;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return an Xml String formatted for embedded transport within
		/// a single element.
		/// </summary>
		/// <param name="value">
		/// String containing possible Xml content.
		/// </param>
		/// <returns>
		/// Standard string, compatible with inclusion within a single HTML or
		/// XML element.
		/// </returns>
		public static string ToXmlEmbedded(string value)
		{
			string rs = Regex.Replace(value, "&", "&amp;");
			rs = Regex.Replace(rs, "<", "&lt;");
			rs = Regex.Replace(rs, ">", "&gt;");
			rs = Regex.Replace(rs, "\"", "&quot;");
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Xor																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the XOR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// XORed value.
		/// </returns>
		public static UInt16 Xor(UInt16 value1, UInt16 value2)
		{
			return (UInt16)((int)value1 ^ (int)value2);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the XOR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// XORed value.
		/// </returns>
		public static UInt32 Xor(UInt32 value1, UInt32 value2)
		{
			return value1 ^ value2;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the XOR result of values 1 and 2.
		/// </summary>
		/// <param name="value1">
		/// Left operand.
		/// </param>
		/// <param name="value2">
		/// Right operand.
		/// </param>
		/// <returns>
		/// XORed value.
		/// </returns>
		public static UInt64 Xor(UInt64 value1, UInt64 value2)
		{
			return value1 ^ value2;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	SysType																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of common system types.
	/// </summary>
	public enum SysType
	{
		//	Note do not re-arrange this enumeration, since hard-coded type
		//	conversion relies upon existing arrangement. If any items are to
		//	be added, append them to the end of the list.
		/// <summary>
		/// Unknown System Type.
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// No Value.
		/// </summary>
		Null = 1,
		/// <summary>
		/// Boolean.
		/// </summary>
		Boolean = 2,
		/// <summary>
		/// Byte.
		/// </summary>
		Byte = 3,
		/// <summary>
		/// Byte Array.
		/// </summary>
		ByteArray = 4,
		/// <summary>
		/// Character.
		/// </summary>
		Char = 5,
		/// <summary>
		/// Character Array.
		/// </summary>
		CharArray = 6,
		/// <summary>
		/// Date and Time.
		/// </summary>
		DateTime = 7,
		/// <summary>
		/// DB Null.
		/// </summary>
		DBNull = 8,
		/// <summary>
		/// Decimal.
		/// </summary>
		Decimal = 9,
		/// <summary>
		/// Double precision floating point value.
		/// </summary>
		Double = 10,
		/// <summary>
		/// Enumeration.
		/// </summary>
		Enum = 11,
		/// <summary>
		/// Globally Unique Identifier.
		/// </summary>
		Guid = 12,
		/// <summary>
		/// Image.
		/// </summary>
		Image = 13,
		/// <summary>
		/// Signed 16-bit Integer.
		/// </summary>
		Int16 = 14,
		/// <summary>
		/// Signed 32-bit Integer.
		/// </summary>
		Int32 = 15,
		/// <summary>
		/// Signed 64-bit Integer.
		/// </summary>
		Int64 = 16,
		/// <summary>
		/// Long String value used for Strings of infinite length.
		/// </summary>
		LongString = 17,
		/// <summary>
		/// Signed 8-bit Integer.
		/// </summary>
		SByte = 18,
		/// <summary>
		/// Single precision floating point value.
		/// </summary>
		Single = 19,
		/// <summary>
		/// String.
		/// </summary>
		String = 20,
		/// <summary>
		/// Array of Strings.
		/// </summary>
		StringArray = 21,
		/// <summary>
		/// Time Span.
		/// </summary>
		TimeSpan = 22,
		/// <summary>
		/// System Type.
		/// </summary>
		Type = 23,
		/// <summary>
		/// Unsigned 16-bit Integer.
		/// </summary>
		UInt16 = 24,
		/// <summary>
		/// Unsigned 32-bit Integer.
		/// </summary>
		UInt32 = 25,
		/// <summary>
		/// Unsigned 64-bit Integer.
		/// </summary>
		UInt64 = 26
	}
	//*-------------------------------------------------------------------------*
}
