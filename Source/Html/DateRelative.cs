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
using System.Data;
using System.IO;
using System.Text;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	DateRelative																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Class for handling relative Dates such as 'Day 1 6 Months Ago', etc.
	/// </summary>
	public class DateRelative
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	GetCell																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string value of a cell.
		/// </summary>
		/// <param name="dr">
		/// Data Row Instance.
		/// </param>
		/// <param name="cn">
		/// Name of the Column to retrieve.
		/// </param>
		/// <returns>
		/// String value of the specified cell.
		/// </returns>
		private static string GetCell(DataRow dr, string cn)
		{
			string rs = "";

			try
			{
				if(dr[cn] != DBNull.Value)
				{
					rs = (string)dr[cn];
				}
			}
			catch { }
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetLevel																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of the instance at this level.
		/// </summary>
		/// <returns>
		/// String representation of the Relative Date Definition at this level.
		/// </returns>
		private string GetLevel()
		{
			DateTime dtn = DateTime.Now;  //	Now Reference.
			int hr = 0;                   //	Working Hour.
			StringBuilder sb = new StringBuilder();

			switch(mDateType)
			{
				case RelativeDateType.Now:
					sb.Append("Now");
					break;

				case RelativeDateType.Today:
					sb.Append("Today");
					break;

				case RelativeDateType.Tomorrow:
					sb.Append("Tomorrow");
					break;

				case RelativeDateType.Yesterday:
					sb.Append("Yesterday");
					break;

				case RelativeDateType.DayAfterTomorrow:
					sb.Append("the Day after Tomorrow");
					break;

				case RelativeDateType.DayBeforeYesterday:
					sb.Append("the Day before Yesterday");
					break;

				case RelativeDateType.FirstDay:
					sb.Append("First Day of");
					break;

				case RelativeDateType.LastDay:
					sb.Append("Last Day of");
					break;

				case RelativeDateType.DayX:
					sb.Append("Day ");
					sb.Append(mDayIndex.ToString());
					break;

				case RelativeDateType.ThisDayX:
					sb.Append("Day ");
					sb.Append(mDayIndex.ToString());
					sb.Append(" of This Month");
					break;

				case RelativeDateType.LastDayX:
					sb.Append("Day ");
					sb.Append(mDayIndex.ToString());
					sb.Append(" of Last Month");
					break;

				case RelativeDateType.NextDayX:
					sb.Append("Day ");
					sb.Append(mDayIndex.ToString());
					sb.Append(" of Next Month");
					break;

				case RelativeDateType.Minutes:
					sb.Append(mMinuteIndex.ToString());
					sb.Append(" Minutes ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.Hours:
					sb.Append(mHourIndex.ToString());
					sb.Append(" Hours ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.Days:
					sb.Append(mDayIndex.ToString());
					sb.Append(" Days ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.Weeks:
					sb.Append(mWeekIndex.ToString());
					sb.Append(" Weeks ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.Months:
					sb.Append(mMonthIndex.ToString());
					sb.Append(" Months ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.Years:
					sb.Append(mYearIndex.ToString());
					sb.Append(" Years ");
					sb.Append((mAgo ? "Ago" : "From"));
					break;

				case RelativeDateType.ThisDate:
					if(mMonthIndex != -1)
					{
						sb.Append("This ");
						sb.Append(DateHelper.ToMonthName(mMonthIndex));
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					else
					{
						sb.Append("Day ");
						sb.Append(mDayIndex.ToString());
						sb.Append(" This Month");
					}
					break;

				case RelativeDateType.LastDate:
					if(mMonthIndex != -1)
					{
						sb.Append("Last ");
						sb.Append(DateHelper.ToMonthName(mMonthIndex));
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					else
					{
						sb.Append("Day ");
						sb.Append(mDayIndex.ToString());
						sb.Append(" Last Month");
					}
					break;

				case RelativeDateType.NextDate:
					if(mMonthIndex != -1)
					{
						sb.Append("Next ");
						sb.Append(DateHelper.ToMonthName(mMonthIndex));
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					else
					{
						sb.Append("Day ");
						sb.Append(mDayIndex.ToString());
						sb.Append(" Next Month");
					}
					break;

				case RelativeDateType.ThisWeek:
					sb.Append("This Week");
					break;

				case RelativeDateType.LastWeek:
					sb.Append("Last Week");
					break;

				case RelativeDateType.ThisMonday:
					sb.Append("This Monday");
					break;

				case RelativeDateType.ThisTuesday:
					sb.Append("This Tuesday");
					break;

				case RelativeDateType.ThisWednesday:
					sb.Append("This Wednesday");
					break;

				case RelativeDateType.ThisThursday:
					sb.Append("This Thursday");
					break;

				case RelativeDateType.ThisFriday:
					sb.Append("This Friday");
					break;

				case RelativeDateType.ThisSaturday:
					sb.Append("This Saturday");
					break;

				case RelativeDateType.ThisSunday:
					sb.Append("This Sunday");
					break;

				case RelativeDateType.ThisMonth:
					sb.Append("This Month");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisJanuary:
					sb.Append("This January");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisFebruary:
					sb.Append("This February");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisMarch:
					sb.Append("This March");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisApril:
					sb.Append("This April");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisMay:
					sb.Append("This May");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisJune:
					sb.Append("This June");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisJuly:
					sb.Append("This July");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisAugust:
					sb.Append("This August");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisSeptember:
					sb.Append("This September");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisOctober:
					sb.Append("This October");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisNovember:
					sb.Append("This November");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisDecember:
					sb.Append("This December");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastMonday:
					sb.Append("Last Monday");
					break;

				case RelativeDateType.LastTuesday:
					sb.Append("Last Tuesday");
					break;

				case RelativeDateType.LastWednesday:
					sb.Append("Last Wednesday");
					break;

				case RelativeDateType.LastThursday:
					sb.Append("Last Thursday");
					break;

				case RelativeDateType.LastFriday:
					sb.Append("Last Friday");
					break;

				case RelativeDateType.LastSaturday:
					sb.Append("Last Saturday");
					break;

				case RelativeDateType.LastSunday:
					sb.Append("Last Sunday");
					break;

				case RelativeDateType.LastMonth:
					sb.Append("Last Month");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastJanuary:
					sb.Append("Last January");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastFebruary:
					sb.Append("Last February");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastMarch:
					sb.Append("Last March");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastApril:
					sb.Append("Last April");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastMay:
					sb.Append("Last May");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastJune:
					sb.Append("Last June");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastJuly:
					sb.Append("Last July");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastAugust:
					sb.Append("Last August");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastSeptember:
					sb.Append("Last September");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastOctober:
					sb.Append("Last October");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastNovember:
					sb.Append("Last November");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.LastDecember:
					sb.Append("Last December");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextWeek:
					sb.Append("Next Week");
					break;

				case RelativeDateType.NextMonday:
					sb.Append("Next Monday");
					break;

				case RelativeDateType.NextTuesday:
					sb.Append("Next Tuesday");
					break;

				case RelativeDateType.NextWednesday:
					sb.Append("Next Wednesday");
					break;

				case RelativeDateType.NextThursday:
					sb.Append("Next Thursday");
					break;

				case RelativeDateType.NextFriday:
					sb.Append("Next Friday");
					break;

				case RelativeDateType.NextSaturday:
					sb.Append("Next Saturday");
					break;

				case RelativeDateType.NextSunday:
					sb.Append("Next Sunday");
					break;

				case RelativeDateType.NextMonth:
					sb.Append("Next Month");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextJanuary:
					sb.Append("Next January");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextFebruary:
					sb.Append("Next February");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextMarch:
					sb.Append("Next March");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextApril:
					sb.Append("Next April");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextMay:
					sb.Append("Next May");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextJune:
					sb.Append("Next June");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextJuly:
					sb.Append("Next July");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextAugust:
					sb.Append("Next August");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextSeptember:
					sb.Append("Next September");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextOctober:
					sb.Append("Next October");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextNovember:
					sb.Append("Next November");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.NextDecember:
					sb.Append("Next December");
					if(mDayIndex != -1)
					{
						//	If a day is expressed, then show it.
						sb.Append(" ");
						sb.Append(mDayIndex.ToString());
					}
					break;

				case RelativeDateType.ThisYear:
					sb.Append("This Year");
					break;

				case RelativeDateType.LastYear:
					sb.Append("Last Year");
					break;

				case RelativeDateType.NextYear:
					sb.Append("Next Year");
					break;

				case RelativeDateType.Static:
					//	Styles of Date:
					//	01 - Day 1 of this Month
					//	04/01 - April 1 of this Year.
					//	04/01/2001 - April 1 of 2005.
					//	Styles of Time:
					//	13 - 1:00 PM on given day
					//	12p - 12:00 PM on given day
					//	:35 - This hour at 35 minutes.
					//	12:35p - 12:35 PM on given day.
					if(mDayIndex != -1 && mMonthIndex == -1 && mYearIndex == -1)
					{
						//	Day X of this Month.
						sb.Append(dtn.Month.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(mDayIndex.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(dtn.Year.ToString().PadLeft(2, '0'));
					}
					else if(mDayIndex != -1 && mMonthIndex != -1 && mYearIndex == -1)
					{
						//	Month X Day Y of this Year.
						sb.Append(mMonthIndex.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(mDayIndex.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(dtn.Year.ToString().PadLeft(2, '0'));
					}
					else if(mDayIndex != -1 && mMonthIndex != -1 && mYearIndex != -1)
					{
						//	Month X Day Y of Year Z.
						sb.Append(mMonthIndex.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(mDayIndex.ToString().PadLeft(2, '0'));
						sb.Append("/");
						sb.Append(mYearIndex.ToString());
					}
					if(mHourIndex != -1 || mMinuteIndex != -1)
					{
						if(mDayIndex != -1 || mMonthIndex != -1 || mYearIndex != -1)
						{
							sb.Append(" ");
						}
						sb.Append("at ");
						if(mHourIndex != -1)
						{
							hr = mHourIndex % 12;
							if(hr == 0)
							{
								hr = 12;
							}
							sb.Append(hr.ToString().PadLeft(2, '0'));
						}
						else
						{
							sb.Append(dtn.Hour.ToString().PadLeft(2, '0'));
						}
						sb.Append(":");
						if(mMinuteIndex != -1)
						{
							sb.Append(mMinuteIndex.ToString().PadLeft(2, '0'));
						}
						else
						{
							sb.Append("00");
						}
						if(mHourIndex >= 12)
						{
							sb.Append(" PM");
						}
						else
						{
							sb.Append(" AM");
						}
					}
					break;
			}

			return sb.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HasStaticExpansion																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified Relative Date has
		///	Expansion values of the Static Relative Date Type.
		/// </summary>
		/// <param name="value">
		/// Instance of a Relative Date to inspect.
		/// </param>
		/// <returns>
		/// True if this or one of the Expansion values are of a Static Relative
		/// Date Type. False otherwise.
		/// </returns>
		private static bool HasStaticExpansion(DateRelative value)
		{
			bool rv = (value.DateType == RelativeDateType.Static);

			if(!rv && value.Expansion != null)
			{
				rv = HasStaticExpansion(value.Expansion);
			}
			return rv;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ReadDataRow																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read the contents of the specified Row into the caller-provided
		/// Relative Date Value.
		/// </summary>
		/// <param name="value">
		/// Instance of a Relative Date Value to fill from Row Content.
		/// </param>
		/// <param name="table">
		/// Instance of the Data Table containing serialized information.
		/// </param>
		/// <param name="rowIndex">
		/// Index of the Table Row containing the information to fill the Relative
		/// Date Value with.
		/// </param>
		private static void ReadDataRow(DateRelative value, DataTable table,
			int rowIndex)
		{
			DataRow dr;

			if(value != null && table != null && rowIndex >= 0 &&
				table.Rows.Count > rowIndex)
			{
				dr = table.Rows[rowIndex];
				value.Ago = Conversion.ToBoolean(GetCell(dr, "ag"));
				value.DateType =
					(RelativeDateType)Conversion.ToInt32(GetCell(dr, "tp"), 0);
				value.DayIndex = Conversion.ToInt32(GetCell(dr, "da"), -1);
				value.HourIndex = Conversion.ToInt32(GetCell(dr, "hr"), -1);
				value.MinuteIndex = Conversion.ToInt32(GetCell(dr, "mi"), -1);
				value.MonthIndex = Conversion.ToInt32(GetCell(dr, "mo"), -1);
				value.WeekIndex = Conversion.ToInt32(GetCell(dr, "wk"), -1);
				value.YearIndex = Conversion.ToInt32(GetCell(dr, "yr"), -1);
				if(table.Rows.Count > rowIndex + 1)
				{
					ReadDataRow(value.Expand(), table, rowIndex + 1);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Implicit DateTime = DateRelative																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the DateRelative to a DateTime value.
		/// </summary>
		/// <param name="value">
		/// The Value to cast.
		/// </param>
		/// <returns>
		/// Converted Value.
		/// </returns>
		public static implicit operator System.DateTime(DateRelative value)
		{
			System.DateTime dt = DateTime.MinValue;

			if(value != null)
			{
				dt = value.Date;
			}
			return dt;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Ago																																		*
		//*-----------------------------------------------------------------------*
		private bool mAgo = false;
		/// <summary>
		/// Get/Set a value indicating whether this Date occurs in the Past.
		/// </summary>
		/// <remarks>
		/// If true, the target date is x time ago. Otherwise, the target date is
		/// x time from y.
		/// </remarks>
		public bool Ago
		{
			get { return mAgo; }
			set { mAgo = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Date																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the specific Date and Time represented by the caller's Relative
		/// Value.
		/// </summary>
		public DateTime Date
		{
			get
			{
				DateTime dt;

				if(mTimeActive)
				{
					dt = DateHelper.NowMinute;
				}
				else
				{
					dt = DateTime.Today;
				}
				try
				{
					dt = DateTime.Parse(dt.Month.ToString() + "/" +
						dt.Day.ToString() + "/" + dt.Year.ToString() + " " +
						dt.Hour.ToString().PadLeft(2, '0') + ":" +
						dt.Minute.ToString().PadLeft(2, '0'));
					dt = dt.Add(Offset);
				}
				catch { }

				return dt;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DateType																															*
		//*-----------------------------------------------------------------------*
		private RelativeDateType mDateType = RelativeDateType.None;
		/// <summary>
		/// Get/Set the Relative Date Type of this instance.
		/// </summary>
		public RelativeDateType DateType
		{
			get { return mDateType; }
			set { mDateType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DayIndex																															*
		//*-----------------------------------------------------------------------*
		private int mDayIndex = -1;
		/// <summary>
		/// Get/Set the relative Day Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {DayIndex} Days (from|ago), etc.
		/// </remarks>
		public int DayIndex
		{
			get { return mDayIndex; }
			set { mDayIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Expand																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Expand this Relative Date to contain more information.
		/// </summary>
		/// <returns>
		/// Newly created Relative Date that is an expanded child of this instance.
		/// </returns>
		public DateRelative Expand()
		{
			mExpansion = new DateRelative();
			return mExpansion;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Expansion																															*
		//*-----------------------------------------------------------------------*
		private DateRelative mExpansion = null;
		/// <summary>
		/// Get/Set the Expanded expression of this Relative Date.
		/// </summary>
		public DateRelative Expansion
		{
			get { return mExpansion; }
			set { mExpansion = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FromXml																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a Relative Date Value from an Xml Definition.
		/// </summary>
		/// <param name="value">
		/// Xml formatted string value compatible with Date Relative serialization.
		/// </param>
		/// <returns>
		/// Newly created Relative Date value, constructed from information in the
		///	caller's value.
		/// </returns>
		public static DateRelative FromXml(string value)
		{
			DataSet ds = new DataSet("DateRelative");
			DateRelative ro = new DateRelative();
			StringReader sr;

			if(value.ToLower().Substring(0, 5) != "<?xml")
			{
				value = "<?xml version=\"1.0\" standalone=\"yes\"?>" + value;
			}
			sr = new StringReader(value);
			try
			{
				ds.ReadXml(sr);
				if(ds.Tables.Count != 0)
				{
					ReadDataRow(ro, ds.Tables[0], 0);
				}
			}
			catch { }

			return ro;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HourIndex																															*
		//*-----------------------------------------------------------------------*
		private int mHourIndex = -1;
		/// <summary>
		/// Get/Set the relative Hour Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {HourIndex} Hours (from|ago), etc.
		/// </remarks>
		public int HourIndex
		{
			get { return mHourIndex; }
			set { mHourIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//		//*-----------------------------------------------------------------------*
		//		//*	Index																																	*
		//		//*-----------------------------------------------------------------------*
		//		private int mIndex = -1;
		//		/// <summary>
		//		/// Get/Set the relative Indexing component of this instance.
		//		/// </summary>
		//		/// <remarks>
		//		/// This is used in any case where an offset is found, such as in the
		//		/// cases of {index} Days (from|ago), {index} Months (from|ago), etc.
		//		/// </remarks>
		//		public int Index
		//		{
		//			get { return mIndex; }
		//			set { mIndex = value; }
		//		}
		//		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MinuteIndex																														*
		//*-----------------------------------------------------------------------*
		private int mMinuteIndex = -1;
		/// <summary>
		/// Get/Set the relative Minute Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {MinuteIndex} Minutes (from|ago), etc.
		/// </remarks>
		public int MinuteIndex
		{
			get { return mMinuteIndex; }
			set { mMinuteIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MonthIndex																														*
		//*-----------------------------------------------------------------------*
		private int mMonthIndex = -1;
		/// <summary>
		/// Get/Set the relative Month Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {MonthIndex} Months (from|ago), etc.
		/// </remarks>
		public int MonthIndex
		{
			get { return mMonthIndex; }
			set { mMonthIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Offset																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get the Time Span Offset represented by this instance.
		/// </summary>
		public TimeSpan Offset
		{
			get
			{
				//				TimeSpan ct;
				DateTime dtn;
				DateTime dtt;
				DateTime dtw;
				int mo = 0;             //	Working Month.
				TimeSpan sp = new TimeSpan(0, 0, 0);
				int yr = 0;             //	Working Year.

				if(mTimeActive)
				{
					dtn = DateHelper.NowMinute;
					dtw = dtn;
				}
				else
				{
					dtn = DateTime.Today;
					dtw = dtn;
				}

				//	Initialize the Today Time.
				dtt = DateTime.Parse(dtn.ToString("MM/dd/yyyy"));

				//				ct = new TimeSpan(dtn.Day, dtn.Hour, dtn.Minute, 0);

				//				//	Originate the offset time at the beginning of today.
				//				sp = sp.Subtract(ct);

				if(mExpansion != null)
				{
					sp = sp.Add(mExpansion.Offset);
				}
				switch(mDateType)
				{
					case RelativeDateType.Now:
						break;

					case RelativeDateType.Today:
						if(!HasStaticExpansion(this))
						{
							sp = sp.Subtract(new TimeSpan(dtn.Hour, dtn.Minute, dtn.Second));
						}
						break;

					case RelativeDateType.Tomorrow:
						sp = sp.Add(new TimeSpan(1, 0, 0, 0));
						break;

					case RelativeDateType.Yesterday:
						sp = sp.Add(new TimeSpan(-1, 0, 0, 0));
						break;

					case RelativeDateType.DayAfterTomorrow:
						sp = sp.Add(new TimeSpan(2, 0, 0, 0));
						break;

					case RelativeDateType.DayBeforeYesterday:
						sp = sp.Add(new TimeSpan(-2, 0, 0, 0));
						break;

					//	TODO: First Day of Expanded Month.
					case RelativeDateType.FirstDay:
						break;

					//	TODO: Last Day of Expanded Month.
					case RelativeDateType.LastDay:
						break;

					case RelativeDateType.DayX:
					case RelativeDateType.ThisDayX:
						sp = sp.Add(new TimeSpan(mDayIndex, 0, 0, 0));
						break;

					case RelativeDateType.LastDayX:
						dtw = dtt.AddMonths(-1);
						dtw = dtw.AddDays(0 - dtt.Day);
						dtw = dtw.AddDays(mDayIndex);
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.NextDayX:
						dtw = dtt.AddMonths(1);
						dtw = dtw.AddDays(0 - dtt.Day);
						dtw = dtw.AddDays(mDayIndex);
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.Minutes:
						if(Ago)
						{
							sp = sp.Add(new TimeSpan(0, 0 - mMinuteIndex, 0));
						}
						else
						{
							sp = sp.Add(new TimeSpan(0, mMinuteIndex, 0));
						}
						break;
					case RelativeDateType.Hours:
						if(Ago)
						{
							sp = sp.Add(new TimeSpan(0 - mHourIndex, 0, 0));
						}
						else
						{
							sp = sp.Add(new TimeSpan(mHourIndex, 0, 0));
						}
						break;
					case RelativeDateType.Days:
						if(Ago)
						{
							sp = sp.Add(new TimeSpan(0 - mDayIndex, 0, 0, 0));
						}
						else
						{
							sp = sp.Add(new TimeSpan(mDayIndex, 0, 0, 0));
						}
						break;
					case RelativeDateType.Weeks:
						if(Ago)
						{
							sp = sp.Add(new TimeSpan(7 * (0 - mWeekIndex), 0, 0, 0));
						}
						else
						{
							sp = sp.Add(new TimeSpan(7 * mWeekIndex, 0, 0, 0));
						}
						break;
					case RelativeDateType.Months:
						if(Ago)
						{
							dtw = dtt.AddMonths(0 - mMonthIndex);
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							dtw = dtt.AddMonths(mMonthIndex);
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;
					case RelativeDateType.Years:
						if(Ago)
						{
							dtw = dtt.AddYears(0 - mYearIndex);
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							dtw = dtt.AddYears(mYearIndex);
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisDate:
						try
						{
							if(mMonthIndex != -1)
							{
								//	If we've specified a month, then we want the date for this
								//	year.
								dtw = DateTime.Parse(mMonthIndex.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								//	Otherwise, we want the date for this month.
								dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.LastDate:
						try
						{
							if(mMonthIndex != -1)
							{
								//	If we've specified a month, then we want the date for last
								//	year.
								yr = dtt.Year - 1;
								dtw = DateTime.Parse(mMonthIndex.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									yr.ToString());
							}
							else
							{
								//	Otherwise, we want the date for last month.
								mo = dtt.Month - 1;
								yr = dtt.Year;
								if(mo <= 0)
								{
									mo = 12;
									yr--;
								}
								dtw = DateTime.Parse(mo.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									yr.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.NextDate:
						try
						{
							if(mMonthIndex != -1)
							{
								//	If we've specified a month, then we want the date for next
								//	year.
								yr = dtt.Year + 1;
								dtw = DateTime.Parse(mMonthIndex.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									yr.ToString());
							}
							else
							{
								//	Otherwise, we want the date for next month.
								mo = dtt.Month + 1;
								yr = dtt.Year;
								if(mo > 12)
								{
									mo = 1;
									yr++;
								}
								dtw = DateTime.Parse(mo.ToString() + "/" +
									mDayIndex.ToString() + "/" +
									yr.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisWeek:
						dtw = dtt;
						while(dtw.DayOfWeek != DayOfWeek.Monday)
						{
							dtw = dtw.AddDays(-1);
						}
						if(DateTime.Compare(dtt, dtw) > 0)
						{
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisMonday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Monday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Monday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Monday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisTuesday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Tuesday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Tuesday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Tuesday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisWednesday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Wednesday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Wednesday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Wednesday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisThursday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Thursday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Thursday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Thursday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisFriday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Friday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Friday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Friday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisSaturday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Saturday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Saturday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Saturday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisSunday:
						dtw = dtt;
						if(DateHelper.ToInt32(dtt.DayOfWeek) >
							DateHelper.ToInt32(DayOfWeek.Sunday))
						{
							//	If the current day is larger than Monday, then we
							//	need to scroll backward.
							while(dtw.DayOfWeek != DayOfWeek.Sunday)
							{
								dtw = dtw.AddDays(-1);
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						else
						{
							//	Otherwise, we need to scroll forward.
							while(dtw.DayOfWeek != DayOfWeek.Sunday)
							{
								dtw = dtw.AddDays(1);
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						break;

					case RelativeDateType.ThisMonth:
						sp = sp.Add(new TimeSpan(0 - dtt.Day, 0, 0, 0));
						if(mDayIndex != -1)
						{
							sp = sp.Add(new TimeSpan(mDayIndex, 0, 0, 0));
						}
						break;

					case RelativeDateType.ThisJanuary:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("01/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("01/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisFebruary:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("02/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("02/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisMarch:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("03/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("03/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisApril:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("04/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("04/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisMay:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("05/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("05/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisJune:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("06/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("06/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisJuly:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("07/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("07/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisAugust:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("08/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("08/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisSeptember:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("09/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("09/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisOctober:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("10/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("10/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisNovember:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("11/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("11/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.ThisDecember:
						try
						{
							if(mDayIndex != -1)
							{
								dtw = DateTime.Parse("12/" + mDayIndex.ToString() + "/" +
									dtt.Year.ToString());
							}
							else
							{
								dtw = DateTime.Parse("12/01/" + dtt.Year.ToString());
							}
							if(DateTime.Compare(dtt, dtw) > 0)
							{
								sp = sp.Subtract(dtt.Subtract(dtw));
							}
							else
							{
								sp = sp.Add(dtw.Subtract(dtt));
							}
						}
						catch { }
						break;

					case RelativeDateType.LastWeek:
						sp = sp.Add(new TimeSpan(-7, 0, 0, 0));
						break;

					case RelativeDateType.LastMonday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Monday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastTuesday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Tuesday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastWednesday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Wednesday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastThursday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Thursday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastFriday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Friday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastSaturday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Saturday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastSunday:
						dtw = dtt.AddDays(-1);
						while(dtw.DayOfWeek != DayOfWeek.Sunday)
						{
							dtw = dtw.AddDays(-1);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastMonth:
						dtw = dtt.AddMonths(-1);
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(0 - dtt.Day);
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastJanuary:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("01/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastFebruary:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("02/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastMarch:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("03/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastApril:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("04/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastMay:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("05/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastJune:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("06/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastJuly:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("07/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastAugust:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("08/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastSeptember:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("09/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastOctober:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("10/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastNovember:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("11/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.LastDecember:
						yr = dtt.Year - 1;
						dtw = DateTime.Parse("12/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Subtract(dtt.Subtract(dtw));
						break;

					case RelativeDateType.NextWeek:
						sp = sp.Add(new TimeSpan(7, 0, 0, 0));
						break;

					case RelativeDateType.NextMonday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Monday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextTuesday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Tuesday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextWednesday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Wednesday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextThursday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Thursday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextFriday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Friday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextSaturday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Saturday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextSunday:
						dtw = dtt.AddDays(1);
						while(dtw.DayOfWeek != DayOfWeek.Sunday)
						{
							dtw = dtw.AddDays(1);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextMonth:
						dtw = dtt.AddMonths(1);
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(0 - dtt.Day);
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextJanuary:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("01/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextFebruary:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("02/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextMarch:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("03/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextApril:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("04/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextMay:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("05/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextJune:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("06/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextJuly:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("07/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextAugust:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("08/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextSeptember:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("09/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextOctober:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("10/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextNovember:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("11/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;

					case RelativeDateType.NextDecember:
						yr = dtt.Year + 1;
						dtw = DateTime.Parse("12/01/" + yr.ToString());
						if(mDayIndex != -1)
						{
							dtw = dtw.AddDays(mDayIndex);
						}
						sp = sp.Add(dtw.Subtract(dtt));
						break;


					case RelativeDateType.ThisYear:
						break;

					case RelativeDateType.LastYear:
						try
						{
							yr = dtt.Year - 1;
							dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
								dtt.Day.ToString() + "/" + yr.ToString());
							if(mMonthIndex != -1)
							{
								dtw = dtw.AddMonths(0 - dtt.Month);
								dtw = dtw.AddMonths(mMonthIndex);
								if(mDayIndex != -1)
								{
									dtw = dtw.AddDays(0 - dtw.Day);
									dtw = dtw.AddDays(mDayIndex);
								}
							}
							sp = sp.Subtract(dtt.Subtract(dtw));
						}
						catch { }
						break;

					case RelativeDateType.NextYear:
						try
						{
							yr = dtt.Year + 1;
							dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
								dtt.Day.ToString() + "/" + yr.ToString());
							if(mMonthIndex != -1)
							{
								dtw = dtw.AddMonths(0 - dtt.Month);
								dtw = dtw.AddMonths(mMonthIndex);
								if(mDayIndex != -1)
								{
									dtw = dtw.AddDays(0 - dtw.Day);
									dtw = dtw.AddDays(mDayIndex);
								}
							}
							sp = sp.Add(dtw.Subtract(dtt));
						}
						catch { }
						break;

					case RelativeDateType.Static:
						try
						{
							if(mYearIndex != -1 || mMonthIndex != -1 || mDayIndex != -1)
							{
								//	If a Year, a Month, or a Day are expressed, then continue.
								if(mYearIndex != -1)
								{
									if(mMonthIndex == -1)
									{
										mMonthIndex = 1;
									}
									if(mDayIndex == -1)
									{
										mDayIndex = 1;
									}
								}
								if(mYearIndex == -1)
								{
									//	If no year is expressed, then zoom in.
									if(mMonthIndex == -1)
									{
										//	If no month is expressed, then process only the day
										//	of this month.
										dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
											mDayIndex.ToString() + "/" + dtt.Year.ToString());
										if(DateTime.Compare(dtt, dtw) > 0)
										{
											sp = sp.Subtract(dtt.Subtract(dtw));
										}
										else
										{
											sp = sp.Add(dtw.Subtract(dtt));
										}
									}
									else
									{
										//	If a month is given, then process month and day.
										dtw = DateTime.Parse(mMonthIndex.ToString() + "/" +
											mDayIndex.ToString() + "/" + dtt.Year.ToString());
										if(DateTime.Compare(dtt, dtw) > 0)
										{
											sp = sp.Subtract(dtt.Subtract(dtw));
										}
										else
										{
											sp = sp.Add(dtw.Subtract(dtt));
										}
									}
								}
								else if(mMonthIndex != -1 && mDayIndex != -1)
								{
									//	Otherwise, the presence of a year also requires the
									//	presence of Month and Day.
									try
									{
										dtw = DateTime.Parse(mMonthIndex.ToString() + "/" +
											mDayIndex.ToString() + "/" + mYearIndex.ToString());
										if(DateTime.Compare(dtt, dtw) > 0)
										{
											sp = sp.Subtract(dtt.Subtract(dtw));
										}
										else
										{
											sp = sp.Add(dtw.Subtract(dtt));
										}
									}
									catch { }
								}
							}
							//	Process the time.
							if(mTimeActive)
							{
								if(mHourIndex != -1 && mMinuteIndex != -1)
								{
									dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
										dtt.Day.ToString() + "/" + dtt.Year.ToString() + " " +
										mHourIndex.ToString() + ":" + mMinuteIndex.ToString());
									if(DateTime.Compare(dtn, dtw) > 0)
									{
										sp = sp.Subtract(dtn.Subtract(dtw));
									}
									else
									{
										sp = sp.Add(dtw.Subtract(dtn));
									}
								}
								else if(mHourIndex != -1)
								{
									dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
										dtt.Day.ToString() + "/" + dtt.Year.ToString() + " " +
										mHourIndex.ToString() + ":00");
									if(DateTime.Compare(dtn, dtw) > 0)
									{
										sp = sp.Subtract(dtn.Subtract(dtw));
									}
									else
									{
										sp = sp.Add(dtw.Subtract(dtn));
									}
								}
								else if(mMinuteIndex != -1)
								{
									dtw = DateTime.Parse(dtt.Month.ToString() + "/" +
										dtt.Day.ToString() + "/" + dtt.Year.ToString() + " " +
										dtn.Hour.ToString() + ":" + mMinuteIndex.ToString());
									if(DateTime.Compare(dtn, dtw) > 0)
									{
										sp = sp.Subtract(dtn.Subtract(dtw));
									}
									else
									{
										sp = sp.Add(dtw.Subtract(dtn));
									}
								}
							}
						}
						catch { }
						break;
				}

				return sp;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TimeActive																														*
		//*-----------------------------------------------------------------------*
		private bool mTimeActive = true;
		/// <summary>
		/// Get/Set a value indicating whether the current Time is used on
		/// calculations.
		/// </summary>
		public bool TimeActive
		{
			get { return mTimeActive; }
			set { mTimeActive = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this instance.
		/// </summary>
		/// <returns>
		/// String representation of the current values in this instance. If
		/// Relative=false, a formatted date will be returned. Otherwise, if
		/// Relative=true, a human readable relative date string will be
		/// returned.
		/// </returns>
		public override string ToString()
		{
			string rs = GetLevel();

			if(mExpansion != null)
			{
				//	If expansions are present, then convert those to string as well.
				rs += " " + mExpansion.ToString();
			}
			return rs;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToXml																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an Xml representation of the values in the provided Relative
		/// Date.
		/// </summary>
		/// <param name="value">
		/// Instance of a Relative Date to convert.
		/// </param>
		/// <returns>
		/// String formatted as Xml and representing the specified object, as well
		/// as any expanded instances.
		/// </returns>
		public static string ToXml(DateRelative value)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("<DateRelative>");
			ToXml(value, sb);
			sb.Append("</DateRelative>");
			return sb.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Build an Xml representation of the values in the provided Relative
		/// Date and its expansion components.
		/// </summary>
		/// <param name="value">
		/// Instance of a Relative Date to convert.
		/// </param>
		/// <param name="builder">
		/// Instance of a string builder configured to capture xml data.
		/// </param>
		private static void ToXml(DateRelative value, StringBuilder builder)
		{
			builder.Append("<e ");
			builder.Append("ag=\"" + (value.Ago ? "1" : "0") + "\" ");
			builder.Append("tp=\"" + ((int)value.DateType).ToString() + "\" ");
			builder.Append("da=\"" + value.DayIndex.ToString() + "\" ");
			builder.Append("hr=\"" + value.HourIndex.ToString() + "\" ");
			builder.Append("mi=\"" + value.MinuteIndex.ToString() + "\" ");
			builder.Append("mo=\"" + value.MonthIndex.ToString() + "\" ");
			builder.Append("wk=\"" + value.WeekIndex.ToString() + "\" ");
			builder.Append("yr=\"" + value.YearIndex.ToString() + "\" ");
			builder.Append("/>");
			if(value.Expansion != null)
			{
				ToXml(value.Expansion, builder);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WeekIndex																															*
		//*-----------------------------------------------------------------------*
		private int mWeekIndex = -1;
		/// <summary>
		/// Get/Set the relative Week Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {WeekIndex} Weeks (from|ago), etc.
		/// </remarks>
		public int WeekIndex
		{
			get { return mWeekIndex; }
			set { mWeekIndex = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	YearIndex																															*
		//*-----------------------------------------------------------------------*
		private int mYearIndex = -1;
		/// <summary>
		/// Get/Set the relative Year Indexing component of this instance.
		/// </summary>
		/// <remarks>
		/// This is used in any case where an offset is found, such as in the
		/// cases of {YearIndex} Years (from|ago), etc.
		/// </remarks>
		public int YearIndex
		{
			get { return mYearIndex; }
			set { mYearIndex = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	RelativeDateType																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of Relative Date Types.
	/// </summary>
	public enum RelativeDateType
	{
		/// <summary>
		/// No Relative Date is defined.
		/// </summary>
		None = 0,
		/// <summary>
		/// Static Date is used.
		/// </summary>
		Static,
		/// <summary>
		/// X Years (from|ago) Y.
		/// </summary>
		Years,
		/// <summary>
		/// X Months (from|ago) Y.
		/// </summary>
		Months,
		/// <summary>
		/// X Weeks (from|ago) Y.
		/// </summary>
		Weeks,
		/// <summary>
		/// X Days (from|ago) Y.
		/// </summary>
		Days,
		/// <summary>
		/// X Hours (from|ago) Y.
		/// </summary>
		Hours,
		/// <summary>
		/// X Minutes (from|ago) Y.
		/// </summary>
		Minutes,
		/// <summary>
		/// 12:00 AM Today.
		/// </summary>
		Today,
		/// <summary>
		/// Current Date and Time.
		/// </summary>
		Now,
		/// <summary>
		/// 12:00 AM Tomorrow.
		/// </summary>
		Tomorrow,
		/// <summary>
		/// 12:00 AM the Day After Tomorrow.
		/// </summary>
		DayAfterTomorrow,
		/// <summary>
		/// 12:00 AM Yesterday.
		/// </summary>
		Yesterday,
		/// <summary>
		/// 12:00 AM the Day Before Yesterday.
		/// </summary>
		DayBeforeYesterday,
		/// <summary>
		/// Day X on the Month specified.
		/// </summary>
		DayX,
		/// <summary>
		/// The Last Day of the Month or Week specified.
		/// </summary>
		LastDay,
		/// <summary>
		/// The First Day of the Month or Week specified.
		/// </summary>
		FirstDay,
		/// <summary>
		/// Day X of This Month, or Day X of Month Y this Year.
		/// </summary>
		ThisDate,
		/// <summary>
		/// This Month on Day X.
		/// </summary>
		ThisDayX,
		/// <summary>
		/// This Year on Date Y.
		/// </summary>
		ThisYear,
		/// <summary>
		/// This Month on Date Y.
		/// </summary>
		ThisMonth,
		/// <summary>
		/// This Week at Time Y.
		/// </summary>
		ThisWeek,
		/// <summary>
		/// This Monday at Time Y.
		/// </summary>
		ThisMonday,
		/// <summary>
		/// This Tuesday at Time Y.
		/// </summary>
		ThisTuesday,
		/// <summary>
		/// This Wednesday at Time Y.
		/// </summary>
		ThisWednesday,
		/// <summary>
		/// This Thursday at Time Y.
		/// </summary>
		ThisThursday,
		/// <summary>
		/// This Friday at Time Y.
		/// </summary>
		ThisFriday,
		/// <summary>
		/// This Saturday at Time Y.
		/// </summary>
		ThisSaturday,
		/// <summary>
		/// This Sunday at Time Y.
		/// </summary>
		ThisSunday,
		/// <summary>
		/// This January on Day X.
		/// </summary>
		ThisJanuary,
		/// <summary>
		/// This February on Day X.
		/// </summary>
		ThisFebruary,
		/// <summary>
		/// This March on Day X.
		/// </summary>
		ThisMarch,
		/// <summary>
		/// This April on Day X.
		/// </summary>
		ThisApril,
		/// <summary>
		/// This May on Day X.
		/// </summary>
		ThisMay,
		/// <summary>
		/// This June on Day X.
		/// </summary>
		ThisJune,
		/// <summary>
		/// This July on Day X.
		/// </summary>
		ThisJuly,
		/// <summary>
		/// This August on Day X.
		/// </summary>
		ThisAugust,
		/// <summary>
		/// This September on Day X.
		/// </summary>
		ThisSeptember,
		/// <summary>
		/// This October on Day X.
		/// </summary>
		ThisOctober,
		/// <summary>
		/// This November on Day X.
		/// </summary>
		ThisNovember,
		/// <summary>
		/// This December on Day X.
		/// </summary>
		ThisDecember,
		/// <summary>
		/// Next [Month/]Day.
		/// </summary>
		NextDate,
		/// <summary>
		/// Next Month - Day X.
		/// </summary>
		NextDayX,
		/// <summary>
		/// Next Year on Date Y.
		/// </summary>
		NextYear,
		/// <summary>
		/// Next Month on Date Y.
		/// </summary>
		NextMonth,
		/// <summary>
		/// Next Week at Time Y.
		/// </summary>
		NextWeek,
		/// <summary>
		/// Next Monday at Time Y.
		/// </summary>
		NextMonday,
		/// <summary>
		/// Next Tuesday at Time Y.
		/// </summary>
		NextTuesday,
		/// <summary>
		/// Next Wednesday at Time Y.
		/// </summary>
		NextWednesday,
		/// <summary>
		/// Next Thursday at Time Y.
		/// </summary>
		NextThursday,
		/// <summary>
		/// Next Friday at Time Y.
		/// </summary>
		NextFriday,
		/// <summary>
		/// Next Saturday at Time Y.
		/// </summary>
		NextSaturday,
		/// <summary>
		/// Next Sunday at Time Y.
		/// </summary>
		NextSunday,
		/// <summary>
		/// Next January on Day X.
		/// </summary>
		NextJanuary,
		/// <summary>
		/// Next February on Day X.
		/// </summary>
		NextFebruary,
		/// <summary>
		/// Next March on Day X.
		/// </summary>
		NextMarch,
		/// <summary>
		/// Next April on Day X.
		/// </summary>
		NextApril,
		/// <summary>
		/// Next May on Day X.
		/// </summary>
		NextMay,
		/// <summary>
		/// Next June on Day X.
		/// </summary>
		NextJune,
		/// <summary>
		/// Next July on Day X.
		/// </summary>
		NextJuly,
		/// <summary>
		/// Next August on Day X.
		/// </summary>
		NextAugust,
		/// <summary>
		/// Next September on Day X.
		/// </summary>
		NextSeptember,
		/// <summary>
		/// Next October on Day X.
		/// </summary>
		NextOctober,
		/// <summary>
		/// Next November on Day X.
		/// </summary>
		NextNovember,
		/// <summary>
		/// Next December on Day X.
		/// </summary>
		NextDecember,
		/// <summary>
		/// Last [Month/]Day.
		/// </summary>
		LastDate,
		/// <summary>
		/// Last Month - Day X.
		/// </summary>
		LastDayX,
		/// <summary>
		/// Last Year on Date Y.
		/// </summary>
		LastYear,
		/// <summary>
		/// Last Month on Date Y.
		/// </summary>
		LastMonth,
		/// <summary>
		/// Last Week at Time Y.
		/// </summary>
		LastWeek,
		/// <summary>
		/// Last Monday at Time Y.
		/// </summary>
		LastMonday,
		/// <summary>
		/// Last Tuesday at Time Y.
		/// </summary>
		LastTuesday,
		/// <summary>
		/// Last Wednesday at Time Y.
		/// </summary>
		LastWednesday,
		/// <summary>
		/// Last Thursday at Time Y.
		/// </summary>
		LastThursday,
		/// <summary>
		/// Last Friday at Time Y.
		/// </summary>
		LastFriday,
		/// <summary>
		/// Last Saturday at Time Y.
		/// </summary>
		LastSaturday,
		/// <summary>
		/// Last Sunday at Time Y.
		/// </summary>
		LastSunday,
		/// <summary>
		/// Last January on Day X.
		/// </summary>
		LastJanuary,
		/// <summary>
		/// Last Febroary on Day X.
		/// </summary>
		LastFebruary,
		/// <summary>
		/// Last March on Day X.
		/// </summary>
		LastMarch,
		/// <summary>
		/// Last April on Day X.
		/// </summary>
		LastApril,
		/// <summary>
		/// Last May on Day X.
		/// </summary>
		LastMay,
		/// <summary>
		/// Last June on Day X.
		/// </summary>
		LastJune,
		/// <summary>
		/// Last July on Day X.
		/// </summary>
		LastJuly,
		/// <summary>
		/// Last August on Day X.
		/// </summary>
		LastAugust,
		/// <summary>
		/// Last September on Day X.
		/// </summary>
		LastSeptember,
		/// <summary>
		/// Last October on Day X.
		/// </summary>
		LastOctober,
		/// <summary>
		/// Last November on Day X.
		/// </summary>
		LastNovember,
		/// <summary>
		/// Last December on Day X.
		/// </summary>
		LastDecember
	}
	//*-------------------------------------------------------------------------*
}
