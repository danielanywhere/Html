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
	//*	DataUrl																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Data URL handling and functionality.
	/// </summary>
	public class DataUrl
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//*	ToB64																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the Base-64 representation of the specified file content.
		/// </summary>
		/// <param name="data">
		/// Html content.
		/// </param>
		/// <param name="mimeType">
		/// Mime type of the data, as determined by file extension.
		/// </param>
		/// <returns>
		/// Base-64 string value where all external references have been fully
		/// embedded.
		/// For SVG, use attribute syntax xlink:href='{base64}'
		/// </returns>
		public static string ToB64(byte[] data, string mimeType)
		{
			StringBuilder builder = new StringBuilder();  //	base64 content.
			List<string> processedLinks = new List<string>();

			if(data?.Length > 0 && mimeType?.Length > 0)
			{
				builder.Append("data:");
				builder.Append(mimeType);
				builder.Append(";base64,");
				builder.Append(Convert.ToBase64String(data));
			}
			return builder.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the Base-64 representation of the specified file content.
		/// </summary>
		/// <param name="data">
		/// Html content.
		/// </param>
		/// <param name="mimeType">
		/// Mime type of the data, as determined by file extension.
		/// </param>
		/// <returns>
		/// Base-64 string value where all external references have been fully
		/// embedded.
		/// For SVG, use attribute syntax xlink:href='{base64}'
		/// </returns>
		public static string ToB64(string data, string mimeType)
		{
			return ToB64(System.Text.Encoding.UTF8.GetBytes(data), mimeType);
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
