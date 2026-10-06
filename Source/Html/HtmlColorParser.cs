/*
 * Copyright (c). 2026 Daniel Patterson, MCSD (danielanywhere).
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
using System.Text.RegularExpressions;

using static Html.HtmlUtil;

namespace Html
{
	//*-------------------------------------------------------------------------*
	//*	HtmlColorParser																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Parsing functionality for HTML style colors.
	/// </summary>
	public class HtmlColorParser
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Map of W3C/HTML standard named colors to 6-digit hex values.
		/// </summary>
		private static readonly Dictionary<string, string> mHtmlNamedColors =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{ "aliceblue", "#f0f8ff" }, { "antiquewhite", "#faebd7" },
			{ "aqua", "#00ffff" }, { "aquamarine", "#7fffd4" },
			{ "azure", "#f0ffff" }, { "beige", "#f5f5dc" }, { "bisque", "#ffe4c4" },
			{ "black", "#000000" }, { "blanchedalmond", "#ffebcd" },
			{ "blue", "#0000ff" }, { "blueviolet", "#8a2be2" },
			{ "brown", "#a52a2a" }, { "burlywood", "#deb887" },
			{ "cadetblue", "#5f9ea0" }, { "chartreuse", "#7fff00" },
			{ "chocolate", "#d2691e" }, { "coral", "#ff7f50" },
			{ "cornflowerblue", "#6495ed" }, { "cornsilk", "#fff8dc" },
			{ "crimson", "#dc143c" }, { "cyan", "#00ffff" },
			{ "darkblue", "#00008b" }, { "darkcyan", "#008b8b" },
			{ "darkgoldenrod", "#b8860b" }, { "darkgray", "#a9a9a9" },
			{ "darkgreen", "#006400" }, { "darkgrey", "#a9a9a9" },
			{ "darkkhaki", "#bdb76b" }, { "darkmagenta", "#8b008b" },
			{ "darkolivegreen", "#556b2f" }, { "darkorange", "#ff8c00" },
			{ "darkorchid", "#9932cc" }, { "darkred", "#8b0000" },
			{ "darksalmon", "#e9967a" }, { "darkseagreen", "#8fbc8f" },
			{ "darkslateblue", "#483d8b" },
			{ "darkslategray", "#2f4f4f" }, { "darkslategrey", "#2f4f4f" },
			{ "darkturquoise", "#00ced1" }, { "darkviolet", "#9400d3" },
			{ "deeppink", "#ff1493" }, { "deepskyblue", "#00bfff" },
			{ "dimgray", "#696969" }, { "dimgrey", "#696969" },
			{ "dodgerblue", "#1e90ff" }, { "firebrick", "#b22222" },
			{ "floralwhite", "#fffaf0" }, { "forestgreen", "#228b22" },
			{ "fuchsia", "#ff00ff" }, { "gainsboro", "#dcdcdc" },
			{ "ghostwhite", "#f8f8ff" }, { "gold", "#ffd700" },
			{ "goldenrod", "#daa520" }, { "gray", "#808080" },
			{ "green", "#008000" }, { "greenyellow", "#adff2f" },
			{ "grey", "#808080" }, { "honeydew", "#f0fff0" },
			{ "hotpink", "#ff69b4" }, { "indianred", "#cd5c5c" },
			{ "indigo", "#4b0082" }, { "ivory", "#fffff0" }, { "khaki", "#f0e68c" },
			{ "lavender", "#e6e6fa" }, { "lavenderblush", "#fff0f5" },
			{ "lawngreen", "#7cfc00" }, { "lemonchiffon", "#fffacd" },
			{ "lightblue", "#add8e6" }, { "lightcoral", "#f08080" },
			{ "lightcyan", "#e0ffff" }, { "lightgoldenrodyellow", "#fafad2" },
			{ "lightgray", "#d3d3d3" }, { "lightgreen", "#90ee90" },
			{ "lightgrey", "#d3d3d3" }, { "lightpink", "#ffb6c1" },
			{ "lightsalmon", "#ffa07a" }, { "lightseagreen", "#20b2aa" },
			{ "lightskyblue", "#87cefa" }, { "lightslategray", "#778899" },
			{ "lightslategrey", "#778899" }, { "lightsteelblue", "#b0c4de" },
			{ "lightyellow", "#ffffe0" }, { "lime", "#00ff00" },
			{ "limegreen", "#32cd32" }, { "linen", "#faf0e6" },
			{ "magenta", "#ff00ff" }, { "maroon", "#800000" },
			{ "mediumaquamarine", "#66cdaa" }, { "mediumblue", "#0000cd" },
			{ "mediumorchid", "#ba55d3" }, { "mediumpurple", "#9370db" },
			{ "mediumseagreen", "#3cb371" }, { "mediumslateblue", "#7b68ee" },
			{ "mediumspringgreen", "#00fa9a" }, { "mediumturquoise", "#48d1cc" },
			{ "mediumvioletred", "#c71585" }, { "midnightblue", "#191970" },
			{ "mintcream", "#f5fffa" }, { "mistyrose", "#ffe4e1" },
			{ "moccasin", "#ffe4b5" }, { "navajowhite", "#ffdead" },
			{ "navy", "#000080" }, { "oldlace", "#fdf5e6" }, { "olive", "#808000" },
			{ "olivedrab", "#6b8e23" }, { "orange", "#ffa500" },
			{ "orangered", "#ff4500" }, { "orchid", "#da70d6" },
			{ "palegoldenrod", "#eee8aa" }, { "palegreen", "#98fb98" },
			{ "paleturquoise", "#afeeee" }, { "palevioletred", "#db7093" },
			{ "papayawhip", "#ffefd5" }, { "peachpuff", "#ffdab9" },
			{ "peru", "#cd853f" }, { "pink", "#ffc0cb" }, { "plum", "#dda0dd" },
			{ "powderblue", "#b0e0e6" }, { "purple", "#800080" },
			{ "rebeccapurple", "#663399" }, { "red", "#ff0000" },
			{ "rosybrown", "#bc8f8f" }, { "royalblue", "#4169e1" },
			{ "saddlebrown", "#8b4513" }, { "salmon", "#fa8072" },
			{ "sandybrown", "#f4a460" }, { "seagreen", "#2e8b57" },
			{ "seashell", "#fff5ee" }, { "sienna", "#a0522d" },
			{ "silver", "#c0c0c0" }, { "skyblue", "#87ceeb" },
			{ "slateblue", "#6a5acd" },
			{ "slategray", "#708090" }, { "slategrey", "#708090" },
			{ "snow", "#fffafa" }, { "springgreen", "#00ff7f" },
			{ "steelblue", "#4682b4" }, { "tan", "#d2b48c" }, { "teal", "#008080" },
			{ "thistle", "#d8bfd8" }, { "tomato", "#ff6347" },
			{ "turquoise", "#40e0d0" }, { "violet", "#ee82ee" },
			{ "wheat", "#f5deb3" }, { "white", "#ffffff" },
			{ "whitesmoke", "#f5f5f5" }, { "yellow", "#ffff00" },
			{ "yellowgreen", "#9acd32" }
		};

		//*-----------------------------------------------------------------------*
		//* HslToHex																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the HTML hex representation of the supplied HSL values.
		/// </summary>
		/// <param name="h">
		/// Hue.
		/// </param>
		/// <param name="s">
		/// Saturation.
		/// </param>
		/// <param name="l">
		/// Light.
		/// </param>
		/// <returns>
		/// The HTML hex representation of the supplied HSL.
		/// </returns>
		private static string HslToHex(float h, float s, float l)
		{
			float b = 0f;
			int blue = 0;
			float c = (1f - Math.Abs(2f * l - 1f)) * s;
			float g = 0f;
			int green = 0;
			float x = c * (1f - Math.Abs((h / 60f) % 2f - 1f));
			float m = l - (c / 2f);
			float r = 0f;
			int red = 0;

			if(h >= 0 && h < 60)
			{
				r = c;
				g = x;
				b = 0;
			}
			else if(h >= 60 && h < 120)
			{
				r = x;
				g = c;
				b = 0;
			}
			else if(h >= 120 && h < 180)
			{
				r = 0;
				g = c;
				b = x;
			}
			else if(h >= 180 && h < 240)
			{
				r = 0;
				g = x;
				b = c;
			}
			else if(h >= 240 && h < 300)
			{
				r = x;
				g = 0;
				b = c;
			}
			else if(h >= 300 && h <= 360)
			{
				r = c;
				g = 0;
				b = x;
			}

			red = (int)Math.Round((r + m) * 255);
			green = (int)Math.Round((g + m) * 255);
			blue = (int)Math.Round((b + m) * 255);

			return $"#{red:x2}{green:x2}{blue:x2}";
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* NormalizeHex																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the normalized version of the caller's hex color string.
		/// </summary>
		/// <param name="value">
		/// The hex color string to normalize.
		/// </param>
		/// <returns>
		/// The caller's hex color string, normalized to 6 digits.
		/// </returns>
		private static string NormalizeHex(string value)
		{
			string raw = "";
			string result = "";

			if(value?.Length > 0)
			{
				raw = value.TrimStart('#').ToLower();
				// 3-digit hex (#f00 -> #ff0000)
				if(raw.Length == 3)
				{
					result = $"#{raw[0]}{raw[0]}{raw[1]}{raw[1]}{raw[2]}{raw[2]}";
				}
				// 4-digit hex with alpha (#f00f -> strip alpha -> #ff0000)
				if(raw.Length == 4)
				{
					result = $"#{raw[0]}{raw[0]}{raw[1]}{raw[1]}{raw[2]}{raw[2]}";
				}
				// 6-digit hex (#1234ef)
				if(raw.Length == 6)
				{
					result = $"#{raw}";
				}
				// 8-digit hex with alpha (#1234ef99 -> strip alpha -> #1234ef)
				if(raw.Length == 8)
				{
					result = $"#{raw.Substring(0, 6)}";
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ParseHsl																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the HTML hex representation of the HSL or HSLA string supplied
		/// by the caller.
		/// </summary>
		/// <param name="value">
		/// The HSL or HSLA value to parse.
		/// </param>
		/// <returns>
		/// HTML hex color representation of the caller's value, if found.
		/// Otherwise, an empty string.
		/// </returns>
		private static string ParseHsl(string value)
		{
			float h = 0f;
			float l = 0f;
			MatchCollection matches = null;
			string result = "";
			float s = 0f;

			if(value?.Length > 0)
			{
				matches = Regex.Matches(value, ResourceMain.rxHslValue);
				if(matches.Count > 2)
				{
					h = ParseHue(GetValue(matches[0], "pattern"));
					s = ParseSL(GetValue(matches[1], "pattern"));
					l = ParseSL(GetValue(matches[2], "pattern"));

					result = HslToHex(
						Math.Clamp(h, 0f, 360f),
						Math.Clamp(s, 0f, 1f),
						Math.Clamp(l, 0f, 1f));
				}
			}

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ParseHue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the caller's hue value.
		/// </summary>
		/// <param name="value">
		/// The value to parse.
		/// </param>
		/// <returns>
		/// The hue value corresponding with the caller's specification.
		/// </returns>
		private static float ParseHue(string value)
		{
			float percent = 0f;
			float result = 0f;

			if(value?.Length > 0)
			{
				if(value.EndsWith("deg"))
				{
					result = ToFloat(value.Replace("deg", "")) % 360f;
				}
				else if(value.EndsWith("rad"))
				{
					result = (ToFloat(value.Replace("rad", "")) *
						(180f / (float)Math.PI)) % 360f;
				}
				else if(value.EndsWith("turn"))
				{
					result = (ToFloat(value.Replace("turn", "")) * 360f) % 360f;
				}
				else if(value.EndsWith('%'))
				{
					percent = ToFloat(value.TrimEnd('%'));
					result = (percent * 3.6f) % 360f;
				}
				else if(value == "none")
				{
					result = 0f;
				}
				else
				{
					result = ToFloat(value) % 360f;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ParseRgb																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the caller's RGB or RGBA function and return the HTML hex color
		/// equivalent.
		/// </summary>
		/// <param name="value">
		/// The RGB or RGBA HTML color function to parse.
		/// </param>
		/// <returns>
		/// The HTML hex color representation of the caller's function.
		/// </returns>
		private static string ParseRgb(string value)
		{
			int b = 0;
			int g = 0;
			MatchCollection matches = null;
			int r = 0;
			string result = "";

			if(value?.Length > 0)
			{
				matches = Regex.Matches(value, ResourceMain.rxNumericOptionalPercent);
				if(matches.Count > 2)
				{
					r = ParseRgbComponent(GetValue(matches, 0, "pattern"));
					g = ParseRgbComponent(GetValue(matches, 1, "pattern"));
					b = ParseRgbComponent(GetValue(matches, 2, "pattern"));
					result = $"#{r:x2}{g:x2}{b:x2}";
				}
			}

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ParseRgbComponent																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the individual RGB component value.
		/// </summary>
		/// <param name="value">
		/// The value to inspect.
		/// </param>
		/// <returns>
		/// The integer representation of the individual component value.
		/// </returns>
		private static int ParseRgbComponent(string value)
		{
			float percent = 0f;
			int result = 0;

			if(value.EndsWith("%"))
			{
				percent = ToFloat(value.TrimEnd('%'));
				result = (int)Math.Clamp(Math.Round(percent * 2.55f), 0, 255);
			}
			else
			{
				result = (int)Math.Clamp(ToFloat(value), 0, 255);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ParseSL																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse and return a saturation or lightness value.
		/// </summary>
		/// <param name="value">
		/// The saturation or lightness value to inspect.
		/// </param>
		/// <returns>
		/// The saturation or lightness corresponding to the supplied value.
		/// </returns>
		private static float ParseSL(string value)
		{
			string number = "";
			float result = 0f;

			if(value?.Length > 0)
			{
				number = GetValue(value, ResourceMain.rxNumeric, "pattern");
				if(number.Length > 0)
				{
					result = ToFloat(number);
					if(result > 1f)
					{
						result /= 100f;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* ToHexColor																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the hex representation of the color value specified by the
		/// caller.
		/// </summary>
		/// <param name="colorValue">
		/// The color expression to convert to hex.
		/// </param>
		/// <returns>
		/// The hex HTML color representation of the caller's supplied color, if
		/// found. Otherwise, an empty string.
		/// </returns>
		public static string ToHexColor(string colorValue)
		{
			string hexResult = "";
			string cleanInput = "";
			string result = "";

			if(!string.IsNullOrWhiteSpace(colorValue))
			{
				cleanInput = colorValue.Trim().ToLower();
				if(cleanInput.StartsWith("#"))
				{
					// Hex Syntax.
					result = NormalizeHex(cleanInput);
				}
				else if(cleanInput.StartsWith("rgb"))
				{
					// RGB and RGBA Syntax.
					result = ParseRgb(cleanInput);
				}
				else if(cleanInput.StartsWith("hsl"))
				{
					// HSL and HSLA Syntax.
					result = ParseHsl(cleanInput);
				}
				else if(mHtmlNamedColors.TryGetValue(cleanInput, out hexResult))
				{
					// HTML Named Color.
					result = hexResult;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
