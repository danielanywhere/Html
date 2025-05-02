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
using System.Text.RegularExpressions;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	DateHelper																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Miscellaneous Methods for extending Date and Time Functions.
	/// </summary>
	public class DateHelper
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	GetGroup																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the named group from the specified Match.
		/// </summary>
		/// <param name="match">
		/// Instance of a Regular Express Match for which the Group Value will
		/// be sought.
		/// </param>
		/// <param name="name">
		/// Name of the Group containing the value to return.
		/// </param>
		/// <returns>
		/// Value of specified group, if found. Otherwise, empty string.
		/// </returns>
		private static string GetGroup(Match match, string name)
		{
			string rs = "";

			if(match != null && match.Groups[name] != null)
			{
				rs = match.Groups[name].Value.ToLower();
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	Duration																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a formatted printable Duration between two times.
		/// </summary>
		/// <param name="fromTime">
		/// The Beginning Date / Time.
		/// </param>
		/// <param name="toTime">
		/// The Ending Date / Time.
		/// </param>
		/// <returns>
		/// Formatted string representing the number of Days, Hours, Minutes, and
		/// Seconds between the two times.
		/// </returns>
		public static string Duration(DateTime fromTime, DateTime toTime)
		{
			return toTime.Subtract(fromTime).ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetDate2																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a 2 Byte Date Value.
		/// </summary>
		/// <param name="value">
		/// Date to convert.
		/// </param>
		/// <returns>
		/// 2 Byte Integer containing specified Date.
		/// </returns>
		public static Int16 GetDate2(DateTime value)
		{
			byte d = (byte)value.Day;
			byte m = (byte)value.Month;
			UInt16 r = 0;
			UInt16 w = 0;
			byte y = (byte)(value.Year % 100);

			r = m;
			w = (UInt16)(d << 4);
			r |= w;
			w = (UInt16)(y << 9);
			r |= w;
			return (Int16)r;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a 2 Byte Date Value.
		/// </summary>
		/// <param name="value">
		/// Date to convert.
		/// </param>
		/// <returns>
		/// DateTime containing specified Date.
		/// </returns>
		public static DateTime GetDate2(Int16 value)
		{
			byte d = (byte)((value & 0x1f0) >> 4);
			byte m = (byte)(value & 0x0f);
			DateTime r = DateTime.MinValue;
			int y = (int)((value & 0xfe00) >> 9);

			r = DateTime.Parse(
				m.ToString().PadLeft(2, '0') + "/" +
				d.ToString().PadLeft(2, '0') + "/" +
				y.ToString().PadLeft(2, '0'));
			return r;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetDaysInMonth																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the number of Days in the specified Month.
		/// </summary>
		/// <param name="value">
		/// Reference Date.
		/// </param>
		/// <returns>
		/// Total Number of Days in the specified Month.
		/// </returns>
		public static int GetDaysInMonth(DateTime value)
		{
			DateTime dt = value.AddMonths(1);

			dt = dt.AddDays(0 - dt.Day);
			return dt.Day;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetDaysRemainingInMonth																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the number of Days remaining in the specified Month.
		/// </summary>
		/// <param name="value">
		/// Reference Date.
		/// </param>
		/// <returns>
		/// Number of days remaining in the specified Month, not counting the
		/// present day.
		/// </returns>
		public static int GetDaysRemainingInMonth(DateTime value)
		{
			int td = GetDaysInMonth(value);   //	Total Days in Month.

			return td - value.Day;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetDaysToTarget																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the number of days until the specified Target Date.
		/// </summary>
		/// <param name="value">
		/// Target Date to reference.
		/// </param>
		/// <returns>
		/// Number of days, not including today, to the specified date.
		/// </returns>
		public static int GetDaysToTarget(DateTime value)
		{
			DateTime dt = DateTime.Parse(
				GetNextDay(DateTime.Now).ToString("MM/dd/yyyy"));
			//			int rv = 0;
			TimeSpan ts = ToResolutionDay(value).Subtract(dt);

			return (int)ts.TotalDays;
		}
		//*-----------------------------------------------------------------------*

		//		//*-----------------------------------------------------------------------*
		//		//*	GetMonthName																													*
		//		//*-----------------------------------------------------------------------*
		//		/// <summary>
		//		/// Return the name of the Month specified by the Index.
		//		/// </summary>
		//		/// <param name="monthIndex">
		//		/// Index of the Month to return.
		//		/// </param>
		//		/// <returns>
		//		/// Name of the Month specified by the provided Index.
		//		/// </returns>
		//		public static string GetMonthName(int monthIndex)
		//		{
		//			string rs = "";
		//			switch(monthIndex)
		//			{
		//				case 1:
		//				case 2:
		//				case 3:
		//				case 4:
		//				case 5:
		//				case 6:
		//				case 7:
		//				case 8:
		//				case 9:
		//				case 10:
		//				case 11:
		//				case 12:
		//			}
		//		}
		//		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNextDate																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the next date from the specified reference and day of month.
		/// </summary>
		/// <param name="baseDate">
		/// Date from which to find the next month day.
		/// </param>
		/// <param name="day">
		/// Day of month of the next date to find.
		/// </param>
		/// <returns>
		/// Next date representing the specified day of month, from the reference
		/// provided by the caller.
		/// </returns>
		public static DateTime GetNextDate(DateTime baseDate, int day)
		{
			int mo = baseDate.Month;
			DateTime rv = ToResolutionDay(baseDate);
			int yr = baseDate.Year;

			if(rv.Day != day)
			{
				//	If the specified day is not the day in question, then get the
				//	next instance of that day.
				if(rv.Day > day)
				{
					//	If the reference day is in the next month, then return a
					//	day from the next month.
					mo++;
					if(mo > 12)
					{
						mo = 1;
						yr++;
					}
				}
				rv = DateTime.Parse(
					mo.ToString().PadLeft(2, '0') + "/" +
					day.ToString().PadLeft(2, '0') + "/" +
					yr.ToString());
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNextDay																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first minute of the following day from the date specified.
		/// </summary>
		/// <param name="value">
		/// Reference Date.
		/// </param>
		/// <returns>
		/// First moment of day following the date specified.
		/// </returns>
		public static DateTime GetNextDay(DateTime value)
		{
			DateTime rv = value;

			rv = rv.AddDays(1);
			if(rv.Hour > 0)
			{
				rv = rv.AddHours(0 - rv.Hour);
			}
			if(rv.Minute > 0)
			{
				rv = rv.AddMinutes(0 - rv.Minute);
			}
			if(rv.Second > 0)
			{
				rv = rv.AddSeconds(0 - rv.Second);
			}
			if(rv.Millisecond > 0)
			{
				rv = rv.AddMilliseconds(0 - rv.Millisecond);
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetPreviousDate																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Previous date from the specified reference and day of month.
		/// </summary>
		/// <param name="baseDate">
		/// Date from which to find the Previous month day.
		/// </param>
		/// <param name="day">
		/// Day of month of the Previous date to find.
		/// </param>
		/// <returns>
		/// Previous date representing the specified day of month, from the
		/// reference provided by the caller.
		/// </returns>
		public static DateTime GetPreviousDate(DateTime baseDate, int day)
		{
			int mo = baseDate.Month;
			DateTime rv = ToResolutionDay(baseDate);
			int yr = baseDate.Year;

			if(rv.Day != day)
			{
				//	If the specified day is not the day in question, then get the
				//	previous instance of that day.
				if(rv.Day < day)
				{
					//	If the reference day is in the previous month, then return a
					//	day from the previous month.
					mo--;
					if(mo < 1)
					{
						mo = 12;
						yr--;
					}
				}
				rv = DateTime.Parse(
					mo.ToString().PadLeft(2, '0') + "/" +
					day.ToString().PadLeft(2, '0') + "/" +
					yr.ToString());
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	NextMonth																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the first Day of the following Month.
		/// </summary>
		public static DateTime NextMonth
		{
			get
			{
				DateTime dt = ToResolutionDay(DateTime.Now.AddMonths(1));

				if(dt.Day > 1)
				{
					dt = dt.AddDays(0 - (dt.Day - 1));
				}
				return dt;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	NowMinute																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the Time Now to the Minute, omitting all finer resolution
		/// components.
		/// </summary>
		public static DateTime NowMinute
		{
			get
			{
				return DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Parse																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a DateTime object representing the caller's string
		/// representation of that date and time.
		/// </summary>
		/// <param name="value">
		/// Caller's Date and Time, in string format.
		/// </param>
		/// <returns>
		/// Relative Date and Time converted from caller's value.
		/// </returns>
		public static DateRelative Parse(string value)
		{
			return Parse(value, null);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a DateTime object representing the caller's string
		/// representation of that date and time.
		/// </summary>
		/// <param name="value">
		/// Caller's Date and Time, in string format.
		/// </param>
		/// <param name="reference">
		/// Reference Relative Date to be configured while parsing the value.
		/// </param>
		/// <returns>
		/// Relative Date and Time converted from caller's value.
		/// </returns>
		public static DateRelative Parse(string value, DateRelative reference)
		{
			bool bt = false;              //	Flag - Time Specified.
			DateRelative dex = null;      //	Expansion Date.
			RelativeDateType drt = RelativeDateType.None; //	Ref Date Type.
			DateRelative dt;
			DateTime dtn = DateTime.Now;  //	Current Date and Time.
			Match m;                      //	Active Match.
			int mp = -1;                  //	Match Position.
			Match mr = null;              //	Reference Match.
			string pt;                    //	Match Pattern.
			string sam = "";              //	Amount.
			string sap = "";              //	AM / PM String.
			string sda = "";              //	Day String.
			string smo = "";              //	Month String.
			string sre = "";              //	Remainder from current Match.
			string ssp = "";              //	Span String.
			DateTime tm = DateTime.MinValue;
			string ws = "";
			int yrn;                      //	Year Now.

			if(reference == null)
			{
				dt = new DateRelative();
			}
			else
			{
				dt = reference;
			}

			#region Original
			//			try
			//			{
			//				dt = DateTime.Parse(value);
			//			}
			//			catch
			//			{
			//				//	If we had an error, then we need to check to see if there is
			//				//	anything we can make of it.
			//
			//				pt = @"(?i:(?<dt>(?<![:0-9]+)(?![0-9]+:)[0-9/]*)*\s*(?<tm>[0-9: apm]+))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//					//	If we have some kind of date, then get that first.
			//					if(m.Groups["dt"] != null)
			//					{
			//						try
			//						{
			//							ws = m.Groups["dt"].Value;
			//							dt = DateTime.Parse(ws);
			//						}
			//						catch(Exception ex)
			//						{
			//							Debug.WriteLine("Could not solve Date for " +
			//								"[" + ws + "] in [" + value + "].\n" +
			//								ex.Message + "\n" + ex.StackTrace, "DateHelper.Parse");
			//						}
			//					}
			//					if(m.Groups["tm"] != null)
			//					{
			//						try
			//						{
			//							ws = m.Groups["tm"].Value.ToLower();
			//							if(ws.IndexOf(":") > 0)
			//							{
			//								tm = DateTime.Parse(m.Groups["tm"].Value);
			//							}
			//							else
			//							{
			//								//	If this time doesn't have a colon, then check to see if
			//								//	if is AM or PM.
			//								if(ws.IndexOf("p") >= 0)
			//								{
			//									tm = tm.AddHours(12);
			//									bp = true;
			//								}
			//								i = Conversion.ToInt32(ws, -1);
			//								if(bp && i == 12)
			//								{
			//									//	If we've noted 12 PM, then we've already added the
			//									//	first twelve hours. Don't add again.
			//									i = -1;
			//								}
			//								{
			//									if(i != -1)
			//									{
			//										tm = tm.AddHours(i);
			//									}
			//								}
			//							}
			//							
			//							if(DateTime.Compare(tm, DateTime.MinValue) != 0)
			//							{
			//								//	If we have an actual time, then add it to the date.
			//								dt = dt.AddHours(tm.Hour);
			//								dt = dt.AddMinutes(tm.Minute);
			//								dt = dt.AddSeconds(tm.Second);
			//							}
			//						}
			//						catch(Exception ex)
			//						{
			//							Debug.WriteLine("Could not solve Time for " +
			//								"[" + ws + "] in [" + value + "].\n" +
			//								ex.Message + "\n" + ex.StackTrace, "DateHelper.Parse");
			//						}
			//					}
			//				}
			//			}
			#endregion

			//	Attempt to find a Relative Date.

			ws = value.ToLower();
			ws = ws.Replace(".", "/").Replace("-", "/");
			ws = ws.Replace("@", " at ");

			ws = Conversion.TextNumberInline(ws);

			#region Old - Basic Verbose to Numerical
			//			ws = ws.Replace("eleven", "11");
			//			ws = ws.Replace("twelve", "12");
			//			ws = ws.Replace("thirteen", "13");
			//			ws = ws.Replace("fourteen", "14");
			//			ws = ws.Replace("fifteen", "15");
			//			ws = ws.Replace("sixteen", "16");
			//			ws = ws.Replace("seventeen", "17");
			//			ws = ws.Replace("eighteen", "18");
			//			ws = ws.Replace("nineteen", "19");
			//
			//			ws = ws.Replace("twenty one", "21");
			//			ws = ws.Replace("twenty two", "22");
			//			ws = ws.Replace("twenty three", "23");
			//			ws = ws.Replace("twenty four", "24");
			//			ws = ws.Replace("twenty five", "25");
			//			ws = ws.Replace("twenty six", "26");
			//			ws = ws.Replace("twenty seven", "27");
			//			ws = ws.Replace("twenty eight", "28");
			//			ws = ws.Replace("twenty nine", "29");
			//			ws = ws.Replace("twenty", "20");
			//
			//			ws = ws.Replace("thirty one", "31");
			//			ws = ws.Replace("thirty two", "32");
			//			ws = ws.Replace("thirty three", "33");
			//			ws = ws.Replace("thirty four", "34");
			//			ws = ws.Replace("thirty five", "35");
			//			ws = ws.Replace("thirty six", "36");
			//			ws = ws.Replace("thirty seven", "37");
			//			ws = ws.Replace("thirty eight", "38");
			//			ws = ws.Replace("thirty nine", "39");
			//			ws = ws.Replace("thirty", "30");
			//
			//			ws = ws.Replace("forty one", "41");
			//			ws = ws.Replace("forty two", "42");
			//			ws = ws.Replace("forty three", "43");
			//			ws = ws.Replace("forty four", "44");
			//			ws = ws.Replace("forty five", "45");
			//			ws = ws.Replace("forty six", "46");
			//			ws = ws.Replace("forty seven", "47");
			//			ws = ws.Replace("forty eight", "48");
			//			ws = ws.Replace("forty nine", "49");
			//			ws = ws.Replace("forty", "40");
			//
			//			ws = ws.Replace("fifty one", "51");
			//			ws = ws.Replace("fifty two", "52");
			//			ws = ws.Replace("fifty three", "53");
			//			ws = ws.Replace("fifty four", "54");
			//			ws = ws.Replace("fifty five", "55");
			//			ws = ws.Replace("fifty six", "56");
			//			ws = ws.Replace("fifty seven", "57");
			//			ws = ws.Replace("fifty eight", "58");
			//			ws = ws.Replace("fifty nine", "59");
			//			ws = ws.Replace("fifty", "50");
			//
			//			ws = ws.Replace("sixty one", "61");
			//			ws = ws.Replace("sixty two", "62");
			//			ws = ws.Replace("sixty three", "63");
			//			ws = ws.Replace("sixty four", "64");
			//			ws = ws.Replace("sixty five", "65");
			//			ws = ws.Replace("sixty six", "66");
			//			ws = ws.Replace("sixty seven", "67");
			//			ws = ws.Replace("sixty eight", "68");
			//			ws = ws.Replace("sixty nine", "69");
			//			ws = ws.Replace("sixty", "60");
			//
			//			ws = ws.Replace("seventy one", "71");
			//			ws = ws.Replace("seventy two", "72");
			//			ws = ws.Replace("seventy three", "73");
			//			ws = ws.Replace("seventy four", "74");
			//			ws = ws.Replace("seventy five", "75");
			//			ws = ws.Replace("seventy six", "76");
			//			ws = ws.Replace("seventy seven", "77");
			//			ws = ws.Replace("seventy eight", "78");
			//			ws = ws.Replace("seventy nine", "79");
			//			ws = ws.Replace("seventy", "70");
			//
			//			ws = ws.Replace("eighty one", "81");
			//			ws = ws.Replace("eighty two", "82");
			//			ws = ws.Replace("eighty three", "83");
			//			ws = ws.Replace("eighty four", "84");
			//			ws = ws.Replace("eighty five", "85");
			//			ws = ws.Replace("eighty six", "86");
			//			ws = ws.Replace("eighty seven", "87");
			//			ws = ws.Replace("eighty eight", "88");
			//			ws = ws.Replace("eighty nine", "89");
			//			ws = ws.Replace("eighty", "80");
			//
			//			ws = ws.Replace("ninety one", "91");
			//			ws = ws.Replace("ninety two", "92");
			//			ws = ws.Replace("ninety three", "93");
			//			ws = ws.Replace("ninety four", "94");
			//			ws = ws.Replace("ninety five", "95");
			//			ws = ws.Replace("ninety six", "96");
			//			ws = ws.Replace("ninety seven", "97");
			//			ws = ws.Replace("ninety eight", "98");
			//			ws = ws.Replace("ninety nine", "99");
			//			ws = ws.Replace("ninety", "90");
			//
			//			ws = ws.Replace("one", "1");
			//			ws = ws.Replace("two", "2");
			//			ws = ws.Replace("three", "3");
			//			ws = ws.Replace("four", "4");
			//			ws = ws.Replace("five", "5");
			//			ws = ws.Replace("six", "6");
			//			ws = ws.Replace("seven", "7");
			//			ws = ws.Replace("eight", "8");
			//			ws = ws.Replace("nine", "9");
			//			ws = ws.Replace("ten", "10");
			#endregion

			#region Day {index}
			//	Objective: Day {index}
			//	Pattern:
			//	(?i:day\s*(?<da>\d+)\s*(?<re>.*))
			pt = @"(?i:day\s*(?<da>\d+)\s*(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.DayX;
				}
			}
			#endregion

			#region (first|last) Day
			//	Objective: (first|last) Day
			//	Pattern:
			//	(?i:(?<ap>(first|last))\s*day(\s+(?<re>.*)|$))
			pt = @"(?i:(?<ap>(first|last))\s*day(\s+(?<re>.*)|$))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sap = GetGroup(m, "ap");
					if(sap == "first")
					{
						drt = RelativeDateType.FirstDay;
					}
					else if(sap == "last")
					{
						drt = RelativeDateType.LastDay;
					}
				}
			}
			#endregion

			#region {index} {periods} (from|ago)
			//	Objective: {index} {periods} (from|ago)
			//	Pattern:
			//	(?i:(?<am>-*\d+)\s*
			//	(?<sp>(years|year|yrs|yr|
			//	months|month|mos|mo|
			//	weeks|week|wks|wk|
			//	days|day|
			//	hours|hour|hrs|hr|
			//	minutes|minute|min))\s*
			//	(?<di>(from|ago|at))?
			//	(\s+(?<re>.*)|$))
			pt = @"(?i:(?<am>-*\d+)\s*(?<sp>(years|year|yrs|yr|months|month|mos|mo|weeks|week|wks|wk|days|day|hours|hour|hrs|hr|minutes|minute|min))\s*(?<di>(from|ago|at))?(\s+(?<re>.*)|$))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					ssp = GetGroup(m, "sp");
					if(ssp == "years" || ssp == "year" || ssp == "yrs" || ssp == "yr")
					{
						drt = RelativeDateType.Years;
					}
					else if(ssp == "months" || ssp == "month" || ssp == "mos" || ssp == "mo")
					{
						drt = RelativeDateType.Months;
					}
					else if(ssp == "weeks" || ssp == "week" || ssp == "wks" || ssp == "wk")
					{
						drt = RelativeDateType.Weeks;
					}
					else if(ssp == "days" || ssp == "day")
					{
						drt = RelativeDateType.Days;
					}
					else if(ssp == "hours" || ssp == "hour" || ssp == "hrs" || ssp == "hr")
					{
						drt = RelativeDateType.Hours;
						bt = true;
					}
					else if(ssp == "minutes" || ssp == "minute" || ssp == "min")
					{
						drt = RelativeDateType.Minutes;
						bt = true;
					}
				}
			}
			#endregion

			#region Day (before|after) (tomorrow|yesterday)
			//	Objective: Day (before|after) (tomorrow|yesterday)
			//	Pattern:
			//	(?i:day\s*(?<ap>(before|after))\s*
			//	(?<da>(tomorrow|yesterday))(?<re>.*))
			pt = @"(?i:day\s*(?<ap>(before|after))\s*(?<da>(tomorrow|yesterday))(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sap = GetGroup(m, "ap");
					sda = GetGroup(m, "da");
					if(sap == "after")
					{
						if(sda == "tomorrow")
						{
							drt = RelativeDateType.DayAfterTomorrow;
						}
						else if(sda == "yesterday")
						{
							drt = RelativeDateType.Today;
						}
					}
					else if(sap == "before")
					{
						if(sda == "tomorrow")
						{
							drt = RelativeDateType.Today;
						}
						else if(sda == "yesterday")
						{
							drt = RelativeDateType.DayBeforeYesterday;
						}
					}
				}
			}
			#endregion

			#region (tomorrow|yesterday|today|now)
			//	Objective: (tomorrow|yesterday|today|now)
			//	Pattern:
			//	(?i:(?<da>(tomorrow|yesterday|today|now))(?<re>.*))
			pt = @"(?i:(?<da>(tomorrow|yesterday|today|now))(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sda = GetGroup(m, "da");
					if(sda == "tomorrow")
					{
						drt = RelativeDateType.Tomorrow;
					}
					else if(sda == "yesterday")
					{
						drt = RelativeDateType.Yesterday;
					}
					else if(sda == "today")
					{
						drt = RelativeDateType.Today;
					}
					else if(sda == "now")
					{
						drt = RelativeDateType.Now;
					}
				}
			}
			#endregion

			#region (last|next|this) [monthnumber] daynumber
			//	Objective: (last|next|this) [monthnumber] daynumber
			//	Pattern:
			//	(?i:(?<ap>(last|next|this))\s*((?<mo>\d+)(/|-))*(?<da>\d+)(?<re>.*))
			pt = @"(?i:(?<ap>(last|next|this))\s*((?<mo>\d+)(/|-))*(?<da>\d+)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sap = GetGroup(m, "ap");
					smo = GetGroup(m, "mo");
					if(sap == "last")
					{
						if(smo.Length == 0)
						{
							drt = RelativeDateType.LastDayX;
						}
						else
						{
							drt = RelativeDateType.LastDate;
						}
					}
					else if(sap == "next")
					{
						if(smo.Length == 0)
						{
							drt = RelativeDateType.NextDayX;
						}
						else
						{
							drt = RelativeDateType.NextDate;
						}
					}
					else if(sap == "this")
					{
						if(smo.Length == 0)
						{
							drt = RelativeDateType.ThisDayX;
						}
						else
						{
							drt = RelativeDateType.ThisDate;
						}
					}
				}
			}
			#endregion

			#region (last|next|this) monthname [daynumber]
			//	Objective: (last|next) monthname [daynumber]
			//	Pattern:
			//	(?i:(?<ap>(last|next|this))\s*
			//	(?<mo>(month|mo|
			//	january|jan|
			//	february|feb|
			//	march|mar|
			//	april|apr|
			//	may|
			//	june|jun|
			//	july|jul|
			//	august|aug|
			//	september|sept|sep|
			//	october|oct|
			//	november|nov|
			//	december|dec))
			//	(\s+(?<da>\d+)|$)*
			//	(\s+(?<re>.*)|$))
			pt = @"(?i:(?<ap>(last|next|this))\s*(?<mo>(month|mo|january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))(\s+(?<da>\d+)|$)*(\s+(?<re>.*)|$))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sap = GetGroup(m, "ap");
					smo = GetGroup(m, "mo");
					if(sap == "last")
					{
						if(smo == "month" || smo == "mo")
						{
							drt = RelativeDateType.LastMonth;
						}
						if(smo == "january" || smo == "jan")
						{
							drt = RelativeDateType.LastJanuary;
						}
						else if(smo == "february" || smo == "feb")
						{
							drt = RelativeDateType.LastFebruary;
						}
						else if(smo == "march" || smo == "mar")
						{
							drt = RelativeDateType.LastMarch;
						}
						else if(smo == "april" || smo == "apr")
						{
							drt = RelativeDateType.LastApril;
						}
						else if(smo == "may")
						{
							drt = RelativeDateType.LastMay;
						}
						else if(smo == "june" || smo == "jun")
						{
							drt = RelativeDateType.LastJune;
						}
						else if(smo == "july" || smo == "jul")
						{
							drt = RelativeDateType.LastJuly;
						}
						else if(smo == "august" || smo == "aug")
						{
							drt = RelativeDateType.LastAugust;
						}
						else if(smo == "september" || smo == "sept" || smo == "sep")
						{
							drt = RelativeDateType.LastSeptember;
						}
						else if(smo == "october" || smo == "oct")
						{
							drt = RelativeDateType.LastOctober;
						}
						else if(smo == "november" || smo == "nov")
						{
							drt = RelativeDateType.LastNovember;
						}
						else if(smo == "december" || smo == "dec")
						{
							drt = RelativeDateType.LastDecember;
						}
					}
					else if(sap == "next")
					{
						if(smo == "month" || smo == "mo")
						{
							drt = RelativeDateType.NextMonth;
						}
						if(smo == "january" || smo == "jan")
						{
							drt = RelativeDateType.NextJanuary;
						}
						else if(smo == "february" || smo == "feb")
						{
							drt = RelativeDateType.NextFebruary;
						}
						else if(smo == "march" || smo == "mar")
						{
							drt = RelativeDateType.NextMarch;
						}
						else if(smo == "april" || smo == "apr")
						{
							drt = RelativeDateType.NextApril;
						}
						else if(smo == "may")
						{
							drt = RelativeDateType.NextMay;
						}
						else if(smo == "june" || smo == "jun")
						{
							drt = RelativeDateType.NextJune;
						}
						else if(smo == "july" || smo == "jul")
						{
							drt = RelativeDateType.NextJuly;
						}
						else if(smo == "august" || smo == "aug")
						{
							drt = RelativeDateType.NextAugust;
						}
						else if(smo == "september" || smo == "sept" || smo == "sep")
						{
							drt = RelativeDateType.NextSeptember;
						}
						else if(smo == "october" || smo == "oct")
						{
							drt = RelativeDateType.NextOctober;
						}
						else if(smo == "november" || smo == "nov")
						{
							drt = RelativeDateType.NextNovember;
						}
						else if(smo == "december" || smo == "dec")
						{
							drt = RelativeDateType.NextDecember;
						}
					}
					else if(sap == "this")
					{
						if(smo == "month" || smo == "mo")
						{
							drt = RelativeDateType.ThisMonth;
						}
						if(smo == "january" || smo == "jan")
						{
							drt = RelativeDateType.ThisJanuary;
						}
						else if(smo == "february" || smo == "feb")
						{
							drt = RelativeDateType.ThisFebruary;
						}
						else if(smo == "march" || smo == "mar")
						{
							drt = RelativeDateType.ThisMarch;
						}
						else if(smo == "april" || smo == "apr")
						{
							drt = RelativeDateType.ThisApril;
						}
						else if(smo == "may")
						{
							drt = RelativeDateType.ThisMay;
						}
						else if(smo == "june" || smo == "jun")
						{
							drt = RelativeDateType.ThisJune;
						}
						else if(smo == "july" || smo == "jul")
						{
							drt = RelativeDateType.ThisJuly;
						}
						else if(smo == "august" || smo == "aug")
						{
							drt = RelativeDateType.ThisAugust;
						}
						else if(smo == "september" || smo == "sept" || smo == "sep")
						{
							drt = RelativeDateType.ThisSeptember;
						}
						else if(smo == "october" || smo == "oct")
						{
							drt = RelativeDateType.ThisOctober;
						}
						else if(smo == "november" || smo == "nov")
						{
							drt = RelativeDateType.ThisNovember;
						}
						else if(smo == "december" || smo == "dec")
						{
							drt = RelativeDateType.ThisDecember;
						}
					}
				}
			}
			#endregion

			#region (last|next|this) dayname
			//	Objective: (last|next|this) dayname
			//	Pattern:
			//	(?i:(?<ap>(last|next))?\s*
			//	(?<da>(week|wk|
			//	monday|mon|
			//	tuesday|tue|
			//	wednesday|wed|
			//	thursday|thu|
			//	friday|fri|
			//	saturday|sat|
			//	sunday|sun))
			//	(\s+(?<re>.*)|$))
			pt = @"(?i:(?<ap>(last|next|this))?\s*(?<da>(year|yr|week|wk|monday|mon|tuesday|tue|wednesday|wed|thursday|thu|friday|fri|saturday|sat|sunday|sun))(\s+(?<re>.*)|$))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				//	If we have a match on this item, then check to see if this
				//	if the leftmost match.
				if(m.Index < mp || mp == -1)
				{
					//	We have a new reference.
					mp = m.Index;
					mr = m;
					sap = GetGroup(m, "ap");
					sda = GetGroup(m, "da");
					if(sap == "last")
					{
						if(sda == "year" || sda == "yr")
						{
							drt = RelativeDateType.LastYear;
						}
						else if(sda == "week" || sda == "wk")
						{
							drt = RelativeDateType.LastWeek;
						}
						else if(sda == "monday" || sda == "mon")
						{
							drt = RelativeDateType.LastMonday;
						}
						else if(sda == "tuesday" || sda == "tue")
						{
							drt = RelativeDateType.LastTuesday;
						}
						else if(sda == "wednesday" || sda == "wed")
						{
							drt = RelativeDateType.LastWednesday;
						}
						else if(sda == "thursday" || sda == "thu")
						{
							drt = RelativeDateType.LastThursday;
						}
						else if(sda == "friday" || sda == "fri")
						{
							drt = RelativeDateType.LastFriday;
						}
						else if(sda == "saturday" || sda == "sat")
						{
							drt = RelativeDateType.LastSaturday;
						}
						else if(sda == "sunday" || sda == "sun")
						{
							drt = RelativeDateType.LastSunday;
						}
					}
					else if(sap.Length == 0 || sap == "next")
					{
						if(sda == "year" || sda == "yr")
						{
							drt = RelativeDateType.NextYear;
						}
						else if(sda == "week" || sda == "wk")
						{
							drt = RelativeDateType.NextWeek;
						}
						else if(sda == "monday" || sda == "mon")
						{
							drt = RelativeDateType.NextMonday;
						}
						else if(sda == "tuesday" || sda == "tue")
						{
							drt = RelativeDateType.NextTuesday;
						}
						else if(sda == "wednesday" || sda == "wed")
						{
							drt = RelativeDateType.NextWednesday;
						}
						else if(sda == "thursday" || sda == "thu")
						{
							drt = RelativeDateType.NextThursday;
						}
						else if(sda == "friday" || sda == "fri")
						{
							drt = RelativeDateType.NextFriday;
						}
						else if(sda == "saturday" || sda == "sat")
						{
							drt = RelativeDateType.NextSaturday;
						}
						else if(sda == "sunday" || sda == "sun")
						{
							drt = RelativeDateType.NextSunday;
						}
					}
					else if(sap == "this")
					{
						if(sda == "monday" || sda == "mon")
						{
							drt = RelativeDateType.ThisMonday;
						}
						else if(sda == "tuesday" || sda == "tue")
						{
							drt = RelativeDateType.ThisTuesday;
						}
						else if(sda == "wednesday" || sda == "wed")
						{
							drt = RelativeDateType.ThisWednesday;
						}
						else if(sda == "thursday" || sda == "thu")
						{
							drt = RelativeDateType.ThisThursday;
						}
						else if(sda == "friday" || sda == "fri")
						{
							drt = RelativeDateType.ThisFriday;
						}
						else if(sda == "saturday" || sda == "sat")
						{
							drt = RelativeDateType.ThisSaturday;
						}
						else if(sda == "sunday" || sda == "sun")
						{
							drt = RelativeDateType.ThisSunday;
						}
					}
				}
			}
			#endregion

			#region Static Date
			//	Objective: year
			//	Pattern:
			//	^(?<yr>\d{4})$
			pt = @"^(?<yr>\d{4})$";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "yr").Length != 0)
				{
					bt = true;
				}
			}
			//	Objective: monthname [day(rd)] [year] [hour]:[minute] [am|pm]
			//	Pattern:
			//	(?i:(?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))(\s*(?<da>\d{1,2})(th|st|nd|rd)?)?(\s+(?<yr>\d+))?(\s+(?<hr>\d+))?(\s*:\s*(?<mi>\d+))?(\s*(?<ap>(am|a|pm|p)))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))(\s*(?<da>\d{1,2})(th|st|nd|rd)?)?(\s+(?<yr>\d+))?(\s+(?<hr>\d+))?(\s*:\s*(?<mi>\d+))?(\s*(?<ap>(am|a|pm|p)))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "yr").Length != 0 ||
					GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "mi").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: monthname day hour [am|pm]
			//	Pattern:
			//	(?i:(?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))(\s*(?<da>\d+)(th|st|nd|rd)?)(\s+(?<hr>\d{1,2}))(\s*:\s*(?<mi>\d+))?(\s*(?<ap>(am|a|pm|p)))?($|(?<re>\s+\D+.*)))
			pt = @"(?i:(?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))(\s*(?<da>\d+)(th|st|nd|rd)?)(\s+(?<hr>\d{1,2}))(\s*:\s*(?<mi>\d+))?(\s*(?<ap>(am|a|pm|p)))?($|(?<re>\s+\D+.*)))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
				GetGroup(m, "mi").Length != 0 ||
				GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm dd yy hh mm | mm/dd/yy hh:mm
			//	Pattern:
			//	(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s*(/|-|\s+)\s*(?<yr>\d+)\s+(?<hr>\d+)(:|\s+)(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s*(/|-|\s+)\s*(?<yr>\d+)\s+(?<hr>\d+)(:|\s+)(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "mi").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm dd hh:mm | mm/dd hh:mm
			//	Pattern:
			//	(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s+(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s+(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "mi").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm dd yy hh | mm/dd/yy hh
			//	Pattern:
			//	(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s*(/|-|\s+)\s*(?<yr>\d+)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s*(/|-|\s+)\s*(?<yr>\d+)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: dd(rd) hh:mm [am|pm]
			//	Pattern:
			//	(?i:((?<da>\d+)(th|st|nd|rd)?)\s+(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:((?<da>\d+)(th|st|nd|rd)?)\s+(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "mi").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm/dd/yy | mm/dd/yyyy
			//	Pattern:
			//	(?<mo>\d+)\s*(/|-)\s*(?<da>\d+)\s*(/|-)\s*(?<yr>\d+)(\s+|$)(?<re>.*)
			pt = @"(?<mo>\d+)\s*(/|-)\s*(?<da>\d+)\s*(/|-)\s*(?<yr>\d+)(\s+|$)(?<re>.*)";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
			}

			//	Objective: mm dd hh | mm/dd hh
			//	Pattern:
			//	(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<mo>\d+)\s*(/|-|\s+)\s*(?<da>\d+)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm/dd
			//	Pattern:
			//	(?<mo>\d+)\s*(/|-)\s*(?<da>\d+)(\s+|$)(?<re>.*)
			pt = @"(?<mo>\d+)\s*(/|-)\s*(?<da>\d+)(\s+|$)(?<re>.*)";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
			}

			//	Objective: dd(rd) hh [am|pm]
			//	Pattern:
			//	(?i:((?<da>\d+)(th|st|nd|rd)?)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:((?<da>\d+)(th|st|nd|rd)?)\s+(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: hh:mm
			//	Pattern:
			//	(?i:(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<hr>\d+):(?<mi>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
				}
				if(GetGroup(m, "hr").Length != 0 ||
					GetGroup(m, "mi").Length != 0 ||
					GetGroup(m, "ap").Length != 0)
				{
					bt = true;
				}
			}

			//	Objective: mm
			//	Pattern:
			//	:(?<mi>\d+)(\s+|$)(?<re>.*)
			pt = @":(?<mi>\d+)(\s+|$)(?<re>.*)";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
					bt = true;
				}
			}

			//	Objective: hh
			//	Pattern:
			//	(?i:(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))
			pt = @"(?i:(?<hr>\d+)\s*(?<ap>(am|a|pm|p))?(\s+|$)(?<re>.*))";
			m = Regex.Match(ws, pt);
			if(m.Success)
			{
				if(m.Index < mp || mp == -1)
				{
					//	If we have a match on this item, then check to see if this is the
					//	leftmost match.
					mp = m.Index;
					mr = m;
					drt = RelativeDateType.Static;
					bt = true;
				}
			}




			//			//	Objective: Parse the parts of a static date.
			//			//	Pattern:
			//			//	(?i:
			//			//	((?<mo>(january|jan|
			//			//	february|feb|
			//			//	march|mar|
			//			//	april|apr|
			//			//	may|
			//			//	june|jun|
			//			//	july|jul|
			//			//	august|aug|
			//			//	september|sept|sep|
			//			//	october|oct|
			//			//	november|nov|
			//			//	december|dec|\d+))(/|-|\s))?
			//			//	((?<da>\d+)(th|st|nd|rd)*)?(/|-|\s)?
			//			//	(?<yr>\d+)?(?<hr>\d+)?:*(?<mi>\d+)?\s*(?<ap>(am|a|pm|p))?
			//			//	(.*?\s+(?<re>.*))*)
			//			pt = @"(?i:((?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec|\d+))(/|-|\s))?((?<da>\d+)(th|st|nd|rd)*)?(/|-|\s)?(?<yr>\d+)?(?<hr>\d+)?:*(?<mi>\d+)?\s*(?<ap>(am|a|pm|p))?(.*?\s+(?<re>.*))*)";
			//			m = Regex.Match(ws, pt);
			//			if(m.Success)
			//			{
			//				if(GetGroup(m, "mo").Length != 0 ||
			//					GetGroup(m, "da").Length != 0 ||
			//					GetGroup(m, "yr").Length != 0 ||
			//					GetGroup(m, "hr").Length != 0 ||
			//					GetGroup(m, "mi").Length != 0)
			//				{
			//					//	If we have a match on this item, then check to see if this
			//					//	if the leftmost match.
			//					if(m.Index < mp || mp == -1)
			//					{
			//						//	We have a new reference.
			//						mp = m.Index;
			//						mr = m;
			//						drt = RelativeDateType.Static;
			//					}
			//				}
			//			}
			#endregion


			dt.DateType = drt;
			if(drt != RelativeDateType.None)
			{
				//	If we have a Date Type to process, then resolve further expansion.
				sre = GetGroup(mr, "re");
				if(sre.Length != 0)
				{
					dex = dt.Expand();
					Parse(sre, dex);
				}
			}
			if(!bt)
			{
				if(dex != null)
				{
					//					if(dex.HourIndex != -1)
					//					{
					//						dt.HourIndex = dex.HourIndex;
					//					}
					//					else
					//					{
					//						dt.HourIndex = 0;
					//					}
					//					if(dex.MinuteIndex != -1)
					//					{
					//						dt.MinuteIndex = dex.MinuteIndex;
					//					}
					//					else
					//					{
					//						dt.MinuteIndex = 0;
					//					}
					//					dt.TimeActive = (dt.HourIndex > 0 || dt.MinuteIndex > 0);
					//					if(dt.TimeActive && dt.Expansion != null)
					//					{
					//						dt.Expansion = null;
					//					}
				}
				else
				{
					dt.HourIndex = 0;
					dt.MinuteIndex = 0;
					dt.TimeActive = false;
				}
			}

			switch(drt)
			{
				case RelativeDateType.Now:
				case RelativeDateType.Today:
				case RelativeDateType.Tomorrow:
				case RelativeDateType.Yesterday:
				case RelativeDateType.DayAfterTomorrow:
				case RelativeDateType.DayBeforeYesterday:
				case RelativeDateType.FirstDay:
				case RelativeDateType.LastDay:
					break;

				case RelativeDateType.DayX:
				case RelativeDateType.ThisDayX:
				case RelativeDateType.LastDayX:
				case RelativeDateType.NextDayX:
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					break;

				case RelativeDateType.Minutes:
					sam = GetGroup(mr, "am");
					dt.MinuteIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;
				case RelativeDateType.Hours:
					sam = GetGroup(mr, "am");
					dt.HourIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;
				case RelativeDateType.Days:
					sam = GetGroup(mr, "am");
					dt.DayIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;
				case RelativeDateType.Weeks:
					sam = GetGroup(mr, "am");
					dt.WeekIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;
				case RelativeDateType.Months:
					sam = GetGroup(mr, "am");
					dt.MonthIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;
				case RelativeDateType.Years:
					sam = GetGroup(mr, "am");
					dt.YearIndex = Conversion.ToInt32Positive(sam, -1);
					dt.Ago = (GetGroup(mr, "di") == "ago");
					if(!dt.Ago && sam.IndexOf("-") >= 0)
					{
						dt.Ago = true;
					}
					break;

				case RelativeDateType.ThisDate:
				case RelativeDateType.LastDate:
				case RelativeDateType.NextDate:
					dt.MonthIndex = Conversion.ToInt32(GetGroup(mr, "mo"), -1);
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					break;

				case RelativeDateType.ThisWeek:
				case RelativeDateType.ThisMonday:
				case RelativeDateType.ThisTuesday:
				case RelativeDateType.ThisWednesday:
				case RelativeDateType.ThisThursday:
				case RelativeDateType.ThisFriday:
				case RelativeDateType.ThisSaturday:
				case RelativeDateType.ThisSunday:
					break;

				case RelativeDateType.ThisMonth:
				case RelativeDateType.ThisJanuary:
				case RelativeDateType.ThisFebruary:
				case RelativeDateType.ThisMarch:
				case RelativeDateType.ThisApril:
				case RelativeDateType.ThisMay:
				case RelativeDateType.ThisJune:
				case RelativeDateType.ThisJuly:
				case RelativeDateType.ThisAugust:
				case RelativeDateType.ThisSeptember:
				case RelativeDateType.ThisOctober:
				case RelativeDateType.ThisNovember:
				case RelativeDateType.ThisDecember:
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					break;


				case RelativeDateType.LastWeek:
				case RelativeDateType.LastMonday:
				case RelativeDateType.LastTuesday:
				case RelativeDateType.LastWednesday:
				case RelativeDateType.LastThursday:
				case RelativeDateType.LastFriday:
				case RelativeDateType.LastSaturday:
				case RelativeDateType.LastSunday:
					break;

				case RelativeDateType.LastMonth:
				case RelativeDateType.LastJanuary:
				case RelativeDateType.LastFebruary:
				case RelativeDateType.LastMarch:
				case RelativeDateType.LastApril:
				case RelativeDateType.LastMay:
				case RelativeDateType.LastJune:
				case RelativeDateType.LastJuly:
				case RelativeDateType.LastAugust:
				case RelativeDateType.LastSeptember:
				case RelativeDateType.LastOctober:
				case RelativeDateType.LastNovember:
				case RelativeDateType.LastDecember:
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					break;


				case RelativeDateType.NextWeek:
				case RelativeDateType.NextMonday:
				case RelativeDateType.NextTuesday:
				case RelativeDateType.NextWednesday:
				case RelativeDateType.NextThursday:
				case RelativeDateType.NextFriday:
				case RelativeDateType.NextSaturday:
				case RelativeDateType.NextSunday:
					break;

				case RelativeDateType.NextMonth:
				case RelativeDateType.NextJanuary:
				case RelativeDateType.NextFebruary:
				case RelativeDateType.NextMarch:
				case RelativeDateType.NextApril:
				case RelativeDateType.NextMay:
				case RelativeDateType.NextJune:
				case RelativeDateType.NextJuly:
				case RelativeDateType.NextAugust:
				case RelativeDateType.NextSeptember:
				case RelativeDateType.NextOctober:
				case RelativeDateType.NextNovember:
				case RelativeDateType.NextDecember:
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					break;

				case RelativeDateType.ThisYear:
				case RelativeDateType.LastYear:
				case RelativeDateType.NextYear:
					break;

				case RelativeDateType.Static:
					//	If we found a static date part, then let's set the date and time.
					dt.YearIndex = Conversion.ToInt32(GetGroup(mr, "yr"), -1);
					dt.DayIndex = Conversion.ToInt32(GetGroup(mr, "da"), -1);
					if(bt)
					{
						dt.HourIndex = Conversion.ToInt32(GetGroup(mr, "hr"), -1);
						dt.MinuteIndex = Conversion.ToInt32(GetGroup(mr, "mi"), -1);
					}
					//	//					else if(!dt.TimeActive)
					//					else
					//					{
					//						dt.HourIndex = 0;
					//						dt.MinuteIndex = 0;
					//	//						dt.TimeActive = false;
					//					}

					smo = GetGroup(mr, "mo");
					if(smo.Length != 0)
					{
						if(Conversion.ToInt32(smo, -1) == -1)
						{
							//	If the month is not a number, then convert to number.
							dt.MonthIndex = ToMonthOrdinal(smo);
						}
						else
						{
							dt.MonthIndex = Conversion.ToInt32(smo, -1);
						}
					}
					if(dt.MonthIndex != -1 && dt.DayIndex != -1 && dt.YearIndex != -1 &&
						dt.YearIndex < 100)
					{
						//	Special case for MM/dd/yy. Convert to absolute year.
						yrn = DateTime.Now.Year % 100;
						if(dt.YearIndex > yrn && yrn - dt.YearIndex < -20)
						{
							//	If a two digit year refers to the past, then use the previous
							//	century.
							yrn = (DateTime.Now.Year - (DateTime.Now.Year % 100)) - 100;
						}
						else
						{
							//	If a two digit year is within this century, then add this
							//	century.
							yrn = DateTime.Now.Year - (DateTime.Now.Year % 100);
						}
						dt.YearIndex += yrn;
					}
					if(bt)
					{
						sap = GetGroup(mr, "ap");
						if(sap.Length != 0)
						{
							//	If we specified AM / PM, then let's check to see how we
							//	should handle the conversion to 24 hour time.
							sap = sap.Substring(0, 1);
							if(dt.HourIndex == 12 && sap == "a")
							{
								//	If the caller said 12 AM, then this is 00:00.
								dt.HourIndex = 0;
							}
							else if(dt.HourIndex != 12 && sap == "p")
							{
								//	Otherwise, if the caller is expressing a PM time, then add
								//	12 hours to the noted time.
								dt.HourIndex += 12;
							}
						}
					}
					break;
			}

			#region Delete
			//			if(bc)
			//			{
			//				//	Objective: Day {index}
			//				//	Pattern:
			//				//	(?i:day\s*(?<da>\d+)\s*(?<re>.*))
			//				pt = @"(?i:day\s*(?<da>\d+)\s*(?<re>.*))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//					//	If we received a match for Day X, then resolve for this part.
			//					g = m.Groups["re"];
			//					if(g != null)
			//					{
			//						//	If a remainder is present, then resolve the inner part.
			//						dex = dt.Expand();
			//						Parse(g.Value, dex);
			//					}
			//					bc = false;
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: {index} {periods} (from|ago)
			//				//	Pattern:
			//				//	(?i:(?<am>-*\d+)\s*
			//				//	(?<sp>(years|year|yrs|yr|
			//				//	months|month|mos|mo|
			//				//	weeks|week|wks|wk|
			//				//	days|day|
			//				//	hours|hour|hrs|hr|
			//				//	minutes|minute|min))\s*
			//				//	(?<di>(from|ago|at)\s*
			//				//	(?<re>.*))*)
			//				pt = @"(?i:(?<am>-*\d+)\s*(?<sp>(years|year|yrs|yr|months|month|mos|mo|weeks|week|wks|wk|days|day|hours|hour|hrs|hr|minutes|minute|min))\s*(?<di>(from|ago|at)\s*(?<re>.*))*)";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//					//	If we received a match for the x from now, x ago possibilities,
			//					//	then let's resolve this part of the equation.
			//					g = m.Groups["re"];
			//					if(g != null)
			//					{
			//						//	If we have a remainder, then resolve the expansion.
			//						dex = dt.Expand();
			//						Parse(g.Value, dex);
			//					}
			//					bc = false;
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: Day (before|after) (tomorrow|yesterday)
			//				//	Pattern:
			//				//	(?i:day\s*(?<ap>(before|after))\s*
			//				//	(?<da>(tomorrow|yesterday))(?<re>.*))
			//				pt = @"(?i:day\s*(?<ap>(before|after))\s*(?<da>(tomorrow|yesterday))(?<re>.*))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: (tomorrow|yesterday|today|now)
			//				//	Pattern:
			//				//	(?i:(?<da>(tomorrow|yesterday|today|now))(?<re>.*))
			//				pt = @"(?i:(?<da>(tomorrow|yesterday|today|now))(?<re>.*))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: (last|next) [monthnumber] daynumber
			//				//	Pattern:
			//				//	(?i:(?<ap>(last|next))\s*(?<mo>\d+)*(/|-)*(?<da>\d+)(?<re>.*))
			//				pt = @"(?i:(?<ap>(last|next))\s*(?<mo>\d+)*(/|-)*(?<da>\d+)(?<re>.*))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: (last|next) dayname
			//				//	Pattern:
			//				//	(?i:(?<ap>(last|next))\s*
			//				//	(?<da>(monday|mon|
			//				//	tuesday|tue|
			//				//	wednesday|wed|
			//				//	thursday|thu|
			//				//	friday|fri|
			//				//	saturday|sat|
			//				//	sunday|sun))
			//				//	(.*?\s+(?<re>.*))*)
			//				pt = @"(?i:(?<ap>(last|next))\s*(?<da>(monday|mon|tuesday|tue|wednesday|wed|thursday|thu|friday|fri|saturday|sat|sunday|sun))(.*?\s+(?<re>.*))*)";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: (last|next) monthname [daynumber]
			//				//	Pattern:
			//				//	(?i:(?<ap>(last|next))\s*
			//				//	(?<mo>(january|jan|
			//				//	february|feb|
			//				//	march|mar|
			//				//	april|apr|
			//				//	may|
			//				//	june|jun|
			//				//	july|jul|
			//				//	august|aug|
			//				//	september|sept|sep|
			//				//	october|oct|
			//				//	november|nov|
			//				//	december|dec))\s*
			//				//	(?<da>\d+)*(?<re>.*))
			//				pt = @"(?i:(?<ap>(last|next))\s*(?<mo>(january|jan|february|feb|march|mar|april|apr|may|june|jun|july|jul|august|aug|september|sept|sep|october|oct|november|nov|december|dec))\s*(?<da>\d+)*(?<re>.*))";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//				}
			//			}

			//			if(bc)
			//			{
			//				//	Objective: Parse the parts of a static date.
			//				//	Pattern:
			//				//	(?i:
			//				//	((?<mo>\d+)?(/|-)?(?<da>\d+)?(/|-)?(?<yr>\d+)?\s+)*
			//				//	(?<hr>\d+)*:*(?<mi>\d+)?\s*(?<ap>[am|a|pm|p])?)
			//				pt = @"(?i:((?<mo>\d+)?(/|-)?(?<da>\d+)?(/|-)?(?<yr>\d+)?\s+)*(?<hr>\d+)*:*(?<mi>\d+)?\s*(?<ap>[am|a|pm|p])?)";
			//				m = Regex.Match(value, pt);
			//				if(m.Success)
			//				{
			//					//	If we found a static date part, then let's set the date and time.
			//					g = m.Groups["mo"];
			//					if(g != null)
			//					{
			//						smo = g.Value;
			//					}
			//					g = m.Groups["da"];
			//					if(g != null)
			//					{
			//						sda = g.Value;
			//					}
			//					g = m.Groups["yr"];
			//					if(g != null)
			//					{
			//						syr = g.Value;
			//					}
			//					g = m.Groups["hr"];
			//					if(g != null)
			//					{
			//						shr = g.Value;
			//					}
			//					g = m.Groups["mi"];
			//					if(g != null)
			//					{
			//						smi = g.Value;
			//					}
			//					g = m.Groups["ap"];
			//					if(g != null)
			//					{
			//						sap = g.Value.ToLower();
			//					}
			//					if(smo.Length != 0 && sda.Length == 0 && syr.Length == 0)
			//					{
			//						//	Special case for Day<sp>hr. If Month is the only date part
			//						//	containing a value, then that value should actually be assigned
			//						//	to the day.
			//						sda = smo;
			//						smo = "";
			//					}
			//					if(smo.Length == 0)
			//					{
			//						//	If the Month is blank, then use this month.
			//						smo = dtn.Month.ToString().PadLeft(2, '0');
			//					}
			//					if(sda.Length == 0)
			//					{
			//						//	If the Day is blank, then use the day today.
			//						sda = dtn.Day.ToString().PadLeft(2, '0');
			//					}
			//					if(syr.Length == 0)
			//					{
			//						//	If the Year is blank, then use this year.
			//						syr = dtn.Year.ToString();
			//					}
			//					if(shr.Length == 0 && smi.Length == 0)
			//					{
			//						//	If the Hour and Minute are blank, then use the first hour.
			//						shr = "00";
			//						smi = "00";
			//					}
			//					else if(shr.Length == 0)
			//					{
			//						//	If only the Hour is blank, then use this hour.
			//						shr = dtn.Hour.ToString().PadLeft(2, '0');
			//					}
			//					if(smi.Length == 0)
			//					{
			//						//	If the minute is blank, then use the first minute.
			//						smi = "00";
			//					}
			//					if(sap.Length != 0)
			//					{
			//						//	If we specified AM / PM, then let's check to see how we should
			//						//	handle the conversion to 24 hour time.
			//						if(shr == "12" && sap == "a")
			//						{
			//							//	If the caller said 12 AM, then this is 00:00.
			//							shr = "00";
			//						}
			//						else if(shr != "12" && sap == "p")
			//						{
			//							//	Otherwise, if the caller is expressing a PM time, then add
			//							//	12 hours to the noted time.
			//							shr = ((int)(Conversion.ToInt32(shr, 0) + 12)).
			//								ToString().PadLeft(2, '0');
			//						}
			//					}
			//					dt.Date = DateTime.Parse(smo + "/" + sda + "/" + syr + " " +
			//						shr + ":" + smi);
			//					bc = false;
			//				}
			//			}
			#endregion


			return dt;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToInt32																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Int32 value of the specified Day of Week.
		/// </summary>
		/// <param name="value">
		/// Day of Week enumeration member.
		/// </param>
		/// <returns>
		/// Ordinal value of the specified Day of Week.
		/// </returns>
		public static int ToInt32(System.DayOfWeek value)
		{
			int rv = 1;

			switch(value)
			{
				case DayOfWeek.Sunday:
					rv = 1;
					break;
				case DayOfWeek.Monday:
					rv = 2;
					break;
				case DayOfWeek.Tuesday:
					rv = 3;
					break;
				case DayOfWeek.Wednesday:
					rv = 4;
					break;
				case DayOfWeek.Thursday:
					rv = 5;
					break;
				case DayOfWeek.Friday:
					rv = 6;
					break;
				case DayOfWeek.Saturday:
					rv = 7;
					break;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToMonthName																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the full month name of the specified ordinal month.
		/// </summary>
		/// <param name="value">
		/// Month Ordinal.
		/// </param>
		/// <returns>
		/// Name of the Month represented by the caller's ordinal position.
		/// </returns>
		public static string ToMonthName(int value)
		{
			string rs = "";

			switch(value)
			{
				case 1:
					rs = "January";
					break;
				case 2:
					rs = "February";
					break;
				case 3:
					rs = "March";
					break;
				case 4:
					rs = "April";
					break;
				case 5:
					rs = "May";
					break;
				case 6:
					rs = "June";
					break;
				case 7:
					rs = "July";
					break;
				case 8:
					rs = "August";
					break;
				case 9:
					rs = "September";
					break;
				case 10:
					rs = "October";
					break;
				case 11:
					rs = "November";
					break;
				case 12:
					rs = "December";
					break;
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToMonthOrdinal																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ordinal value of the named Month.
		/// </summary>
		/// <param name="value">
		/// Named Month.
		/// </param>
		/// <returns>
		/// Ordinal of the Month represented by the caller's name.
		/// </returns>
		public static int ToMonthOrdinal(string value)
		{
			int rv = 0;

			switch(value.ToLower())
			{
				case "jan":
				case "january":
					rv = 1;
					break;
				case "feb":
				case "february":
					rv = 2;
					break;
				case "mar":
				case "march":
					rv = 3;
					break;
				case "apr":
				case "april":
					rv = 4;
					break;
				case "may":
					rv = 5;
					break;
				case "jun":
				case "june":
					rv = 6;
					break;
				case "jul":
				case "july":
					rv = 7;
					break;
				case "aug":
				case "august":
					rv = 8;
					break;
				case "sep":
				case "sept":
				case "september":
					rv = 9;
					break;
				case "oct":
				case "october":
					rv = 10;
					break;
				case "nov":
				case "november":
					rv = 11;
					break;
				case "dec":
				case "december":
					rv = 12;
					break;
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToResolutionDay																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Date Time limited to the resolution of one day.
		/// </summary>
		/// <param name="value">
		/// Natural Date Time.
		/// </param>
		/// <returns>
		/// Date Time value limited to the resolution of one day.
		/// </returns>
		public static DateTime ToResolutionDay(DateTime value)
		{
			return DateTime.Parse(value.ToString("MM/dd/yyyy"));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToResolutionHour																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Date Time limited to the resolution of one Hour.
		/// </summary>
		/// <param name="value">
		/// Natural Date Time.
		/// </param>
		/// <returns>
		/// Date Time value limited to the resolution of one Hour.
		/// </returns>
		public static DateTime ToResolutionHour(DateTime value)
		{
			return DateTime.Parse(value.ToString("MM/dd/yyyy HH:00"));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToResolutionSecond																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Date Time limited to the resolution of one second.
		/// </summary>
		/// <param name="value">
		/// Natural Date Time.
		/// </param>
		/// <returns>
		/// Date Time value limited to the resolution of one second.
		/// </returns>
		public static DateTime ToResolutionSecond(DateTime value)
		{
			return DateTime.Parse(value.ToString("MM/dd/yyyy HH:mm:ss"));
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*
}
