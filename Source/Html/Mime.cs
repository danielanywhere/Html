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
	//*	Mime																																		*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Multi-purpose Internet Mail Extension functionality and features.
	/// </summary>
	public class Mime
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
		//*	MimeType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the MIME-type associated with the specified extension.
		/// </summary>
		/// <param name="extension">
		/// File extension to associate.
		/// </param>
		/// <returns>
		/// MIME-type associated with the specified file extension.
		/// </returns>
		public static string MimeType(string extension)
		{
			string result = "";

			if(extension?.Length > 0)
			{
				if(extension.StartsWith("."))
				{
					extension = extension.Substring(1);
				}
				extension = extension.ToLower();
				switch(extension)
				{
					case "3g2": //	3GPP2 audio/video container.
						result = "video/3gpp2";
						//	also, audio/3gpp2 for audio only.
						break;
					case "3gp": //	3GPP audio/video container.
						result = "video/3gpp";
						//	also, audio/3gpp for audio only.
						break;
					case "7z":  //	7-zip archive.
						result = "application/x-7z-compressed";
						break;
					case "aac": //	AAC audio.
						result = "audio/aac";
						break;
					case "abw": //	AbiWord document.
						result = "application/x-abiword";
						break;
					case "arc": //	Archive document (multiple files embedded).
						result = "application/x-freearc";
						break;
					case "avi": //	AVI: Audio Video Interleave.
						result = "video/x-msvideo";
						break;
					case "azw": //	Amazon Kindle eBook format.
						result = "application/vnd.amazon.ebook";
						break;
					case "bin": //	Any kind of binary data.
						result = "application/octet-stream";
						break;
					case "bmp": //	Windows OS/2 Bitmap Graphics.
						result = "image/bmp";
						break;
					case "bz":  //	BZip archive.
						result = "application/x-bzip";
						break;
					case "bz2": //	BZip2 archive.
						result = "application/x-bzip2";
						break;
					case "csh": //	C-Shell script.
						result = "application/x-csh";
						break;
					case "css": //	Cascading Style Sheets (CSS).
						result = "text/css";
						break;
					case "csv": //	Comma-separated values (CSV).
						result = "text/csv";
						break;
					case "doc": //	Microsoft Word (Legacy).
						result = "application/msword";
						break;
					case "docx":  //	Microsoft Word (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"wordprocessingml.document";
						break;
					case "eot": //	MS Embedded OpenType fonts.
						result = "application/vnd.ms-fontobject";
						break;
					case "epub":  //	Electronic publication (EPUB).
						result = "application/epub+zip";
						break;
					case "gz":  //	GZip Compressed Archive.
						result = "application/gzip";
						break;
					case "gif": //	Graphics Interchange Format (GIF).
						result = "image/gif";
						break;
					case "htm": //	HyperText Markup Language (HTML).
					case "html":
						result = "text/html";
						break;
					case "ico": //	Icon format.
						result = "image/vnd.microsoft.icon";
						break;
					case "ics": //	iCalendar format.
						result = "text/calendar";
						break;
					case "jar": //	Java Archive (JAR).
						result = "application/java-archive";
						break;
					case "jpeg":  //	JPEG image.
					case "jpg":
						result = "image/jpeg";
						break;
					case "js":  //	JavaScript.
						result = "text/javascript";
						break;
					case "json":  //	JSON format.
						result = "application/json";
						break;
					case "jsonld":  //	JSON-LD format.
						result = "application/ld+json";
						break;
					case "mid": //	Musical Instrument Digital Interface (MIDI).
					case "midi":
						result = "audio/midi";
						//	also, audio/x-midi
						break;
					case "mjs": //	JavaScript module.
						result = "text/javascript";
						break;
					case "mp3": //	MP3 audio.
						result = "audio/mpeg";
						break;
					case "mp4": //	MP4 video.
						result = "video/mp4";
						break;
					case "mpeg":  //	MPEG Video.
						result = "video/mpeg";
						break;
					case "mpkg":  //	Apple Installer Package.
						result = "application/vnd.apple.installer+xml";
						break;
					case "odp": //	OpenDocument presentation document.
						result = "application/vnd.oasis.opendocument.presentation";
						break;
					case "ods": //	OpenDocument spreadsheet document.
						result = "application/vnd.oasis.opendocument.spreadsheet";
						break;
					case "odt": //	OpenDocument text document.
						result = "application/vnd.oasis.opendocument.text";
						break;
					case "oga": //	OGG audio.
						result = "audio/ogg";
						break;
					case "ogv": //	OGG video.
						result = "video/ogg";
						break;
					case "ogx": //	OGG.
						result = "application/ogg";
						break;
					case "opus":  //	Opus audio.
						result = "audio/opus";
						break;
					case "otf": //	OpenType font.
						result = "font/otf";
						break;
					case "png": //	Portable Network Graphics.
						result = "image/png";
						break;
					case "pdf": //	Adobe Portable Document Format (PDF).
						result = "application/pdf";
						break;
					case "php": //	Hypertext Preprocessor (Personal Home Page).
						result = "application/x-httpd-php";
						break;
					case "ppt": //	Microsoft PowerPoint (Legacy).
						result = "application/vnd.ms-powerpoint";
						break;
					case "pptx":  //	Microsoft PowerPoint (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"presentationml.presentation";
						break;
					case "rar": //	RAR archive.
						result = "application/vnd.rar";
						break;
					case "rtf": //	Rich Text Format (RTF).
						result = "application/rtf";
						break;
					case "sh":  //	Bourne shell script.
						result = "application/x-sh";
						break;
					case "svg": //	Scalable Vector Graphics (SVG).
						result = "image/svg+xml";
						break;
					case "swf": //	Small web format (SWF) or Adobe Flash document.
						result = "application/x-shockwave-flash";
						break;
					case "tar": //	Tape Archive (TAR).
						result = "application/x-tar";
						break;
					case "tif": //	Tagged Image File Format (TIFF).
					case "tiff":
						result = "image/tiff";
						break;
					//case "ts":  //	MPEG transport stream.
					//	result = "video/mp2t";
					//	break;
					case "ts":  //	Typescript.
						result = "application/x-typescript";
						break;
					case "ttf": //	TrueType Font.
						result = "font/ttf";
						break;
					case "txt": //	Text, (generally ASCII or ISO 8859-n).
						result = "text/plain";
						break;
					case "vsd": //	Microsoft Visio.
						result = "application/vnd.visio";
						break;
					case "wav": //	Waveform Audio Format.
						result = "audio/wav";
						break;
					case "weba":  //	WEBM audio.
						result = "audio/webm";
						break;
					case "webm":  //	WEBM video.
						result = "video/webm";
						break;
					case "webp":  //	WEBP image.
						result = "image/webp";
						break;
					case "woff":  //	Web Open Font Format (WOFF).
						result = "font/woff";
						break;
					case "woff2": //	Web Open Font Format (WOFF).
						result = "font/woff2";
						break;
					case "xhtml": //	XHTML.
						result = "application/xhtml+xml";
						break;
					case "xls": //	Microsoft Excel (Legacy).
						result = "application/vnd.ms-excel";
						break;
					case "xlsx":  //	Microsoft Excel (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"spreadsheetml.sheet";
						break;
					case "xml": //	XML.
						result = "application/xml";
						//	also, text/xml if readable by casual users
						break;
					case "xul": //	XUL.
						result = "application/vnd.mozilla.xul+xml";
						break;
					case "zip": //	ZIP archive.
						result = "application/zip";
						break;
					default:  //	Any unidentified format is binary data.
						result = "application/octet-stream";
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MimeTypeExtension																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a file extension corresponding to the specified MIME-type.
		/// </summary>
		/// <param name="mimeType">
		/// MIME-type to represent.
		/// </param>
		/// <returns>
		/// Closest recognized file extension for the specified MIME-type.
		/// </returns>
		public static string MimeTypeExtension(string mimeType)
		{
			string result = "";

			switch(mimeType)
			{
				case "video/3gpp2":
					//	3GPP2 audio/video container.
					result = "3g2";
					//	also, audio/3gpp2 for audio only.
					break;
				case "video/3gpp":
					//	3GPP audio/video container.
					result = "3gp";
					//	also, audio/3gpp for audio only.
					break;
				case "application/x-7z-compressed":
					//	7-zip archive.
					result = "7z";
					break;
				case "audio/aac":
					//	AAC audio.
					result = "aac";
					break;
				case "application/x-abiword":
					//	AbiWord document.
					result = "abw";
					break;
				case "application/x-freearc":
					//	Archive document (multiple files embedded).
					result = "arc";
					break;
				case "video/x-msvideo":
					//	AVI: Audio Video Interleave.
					result = "avi";
					break;
				case "application/vnd.amazon.ebook":
					//	Amazon Kindle eBook format.
					result = "azw";
					break;
				case "application/octet-stream":
					//	Any kind of binary data.
					result = "bin";
					break;
				case "image/bmp":
					//	Windows OS/2 Bitmap Graphics.
					result = "bmp";
					break;
				case "application/x-bzip":
					//	BZip archive.
					result = "bz";
					break;
				case "application/x-bzip2":
					//	BZip2 archive.
					result = "bz2";
					break;
				case "application/x-csh":
					//	C-Shell script.
					result = "csh";
					break;
				case "text/css":
					//	Cascading Style Sheets (CSS).
					result = "css";
					break;
				case "text/csv":
					//	Comma-separated values (CSV).
					result = "csv";
					break;
				case "application/msword":
					//	Microsoft Word (Legacy).
					result = "doc";
					break;
				case "application/vnd.openxmlformats-officedocument." +
						"wordprocessingml.document":
					//	Microsoft Word (OpenXML).
					result = "docx";
					break;
				case "application/vnd.ms-fontobject":
					//	MS Embedded OpenType fonts.
					result = "eot";
					break;
				case "application/epub+zip":
					//	Electronic publication (EPUB).
					result = "epub";
					break;
				case "application/gzip":
					//	GZip Compressed Archive.
					result = "gz";
					break;
				case "image/gif":
					//	Graphics Interchange Format (GIF).
					result = "gif";
					break;
				case "text/html":
					//	HyperText Markup Language (HTML).
					result = "html";
					break;
				case "image/vnd.microsoft.icon":
					//	Icon format.
					result = "ico";
					break;
				case "text/calendar":
					//	iCalendar format.
					result = "ics";
					break;
				case "application/java-archive":
					//	Java Archive (JAR).
					result = "jar";
					break;
				case "image/jpeg":
					//	JPEG image.
					result = "jpeg";
					break;
				case "text/javascript":
					//	JavaScript.
					result = "js";
					break;
				case "application/json":
					//	JSON format.
					result = "json";
					break;
				case "application/ld+json":
					//	JSON-LD format.
					result = "jsonld";
					break;
				case "audio/midi":
					//	Musical Instrument Digital Interface (MIDI).
					result = "mid";
					//	also, audio/x-midi
					break;
				//case "text/javascript":
				//	//	JavaScript module.
				//	result = "mjs";
				//break;
				case "audio/mpeg":
					//	MP3 audio.
					result = "mp3";
					break;
				case "video/mp4":
					//	MP4 video.
					result = "mp4";
					break;
				case "video/mpeg":  //	MPEG Video.
					result = "mpeg";
					break;
				case "application/vnd.apple.installer+xml":
					//	Apple Installer Package.
					result = "mpkg";
					break;
				case "application/vnd.oasis.opendocument.presentation":
					//	OpenDocument presentation document.
					result = "odp";
					break;
				case "application/vnd.oasis.opendocument.spreadsheet":
					//	OpenDocument spreadsheet document.
					result = "ods";
					break;
				case "application/vnd.oasis.opendocument.text":
					//	OpenDocument text document.
					result = "odt";
					break;
				case "audio/ogg":
					//	OGG audio.
					result = "oga";
					break;
				case "video/ogg":
					//	OGG video.
					result = "ogv";
					break;
				case "application/ogg":
					//	OGG.
					result = "ogx";
					break;
				case "audio/opus":
					//	Opus audio.
					result = "opus";
					break;
				case "font/otf":
					//	OpenType font.
					result = "otf";
					break;
				case "image/png":
					//	Portable Network Graphics.
					result = "png";
					break;
				case "application/pdf":
					//	Adobe Portable Document Format (PDF).
					result = "pdf";
					break;
				case "application/x-httpd-php":
					//	Hypertext Preprocessor (Personal Home Page).
					result = "php";
					break;
				case "application/vnd.ms-powerpoint":
					//	Microsoft PowerPoint (Legacy).
					result = "ppt";
					break;
				case "application/vnd.openxmlformats-officedocument." +
						"presentationml.presentation":
					//	Microsoft PowerPoint (OpenXML).
					result = "pptx";
					break;
				case "application/vnd.rar":
					//	RAR archive.
					result = "rar";
					break;
				case "application/rtf":
					//	Rich Text Format (RTF).
					result = "rtf";
					break;
				case "application/x-sh":
					//	Bourne shell script.
					result = "sh";
					break;
				case "image/svg+xml":
					//	Scalable Vector Graphics (SVG).
					result = "svg";
					break;
				case "application/x-shockwave-flash":
					//	Small web format (SWF) or Adobe Flash document.
					result = "swf";
					break;
				case "application/x-tar":
					//	Tape Archive (TAR).
					result = "tar";
					break;
				case "image/tiff":
					//	Tagged Image File Format (TIFF).
					result = "tiff";
					break;
				//case "ts":  //	MPEG transport stream.
				//	result = "video/mp2t";
				//	break;
				case "application/x-typescript":
					//	Typescript.
					result = "ts";
					break;
				case "font/ttf":
					//	TrueType Font.
					result = "ttf";
					break;
				case "text/plain":
					//	Text, (generally ASCII or ISO 8859-n).
					result = "txt";
					break;
				case "application/vnd.visio":
					//	Microsoft Visio.
					result = "vsd";
					break;
				case "audio/wav":
					//	Waveform Audio Format.
					result = "wav";
					break;
				case "audio/webm":
					//	WEBM audio.
					result = "weba";
					break;
				case "video/webm":
					//	WEBM video.
					result = "webm";
					break;
				case "image/webp":
					//	WEBP image.
					result = "webp";
					break;
				case "font/woff":
					//	Web Open Font Format (WOFF).
					result = "woff";
					break;
				case "font/woff2":
					//	Web Open Font Format (WOFF).
					result = "woff2";
					break;
				case "application/xhtml+xml":
					//	XHTML.
					result = "xhtml";
					break;
				case "application/vnd.ms-excel":
					//	Microsoft Excel (Legacy).
					result = "xls";
					break;
				case "application/vnd.openxmlformats-officedocument." +
						"spreadsheetml.sheet":
					//	Microsoft Excel (OpenXML).
					result = "xlsx";
					break;
				case "application/xml":
					//	XML.
					result = "xml";
					//	also, text/xml if readable by casual users
					break;
				case "application/vnd.mozilla.xul+xml":
					//	XUL.
					result = "xul";
					break;
				case "application/zip":
					//	ZIP archive.
					result = "zip";
					break;
				default:
					//	Any unidentified format is binary data.
					result = "bin";
					break;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*
	}
	//*-------------------------------------------------------------------------*

}
