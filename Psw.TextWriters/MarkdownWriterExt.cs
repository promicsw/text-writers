// -----------------------------------------------------------------------------
// Copyright (c) 2023 Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Psw.TextWriters
{
    /// <summary>
    /// IdentTextWriter extensions for Markdown writing.
    /// </summary>
    public static class MarkdownWriterExt
    {
        /// <group>Basic Markdown</group>
        /// <summary>
        /// Escape all markdown characters in given text (by inserting a &#92; before the character):<br/>
        /// - Call this static method as MarkdownWriterExt.MdEscape(...)
        /// </summary>
        /// <param name="text">Text to escape</param>
        /// <param name="escapeChars">Characters to escape</param>
        /// <returns>Escaped text</returns>
        //public static string MdEscape(string text, string escapeChars="\\`*_{}[]<>()#+-.!|") => IndentTextWriter.EscapeChars(escapeChars, text);

        /// <group>Basic Markdown</group>
        /// <summary>
        /// Markdown for a Horizontal Rule, and ensures a blank line before and after.
        /// </summary>
        public static IndentTextWriter MdHR(this IndentTextWriter w) => w.NewLine(w.IsNewLine ? 1 : 2).WriteLine("---").NewLine();

        /// <summary>
        /// Create a HTML break tag: &lt;br/&gt;
        /// </summary>
        public static IndentTextWriter MdBr(this IndentTextWriter w) => w.Write("<br/>");

        /// <summary>
        /// Markdown for a Line Break by writing two spaces and a newline. 
        /// </summary>
        public static IndentTextWriter MdLB(this IndentTextWriter w) => w.WriteLine("  ");

        /// <summary>
        /// Creates a HTML anchor element with given anchorID - for in-page navigation : &lt;a id="anchorID"&gt;&lt;/a&gt;.
        /// </summary>
        public static IndentTextWriter MdAnchor(this IndentTextWriter w, string anchorID) => w.HtmlAnchor(anchorID);

        /// <summary>
        /// Wraps text in Bold markdown.
        /// </summary>
        public static IndentTextWriter MdBold(this IndentTextWriter w, string text) => w.Write($"**{text}**");
        /// <summary>
        /// Wraps content in Bold markdown.
        /// </summary>
        /// <param name="content">Delegate to build the content.</param> 
        public static IndentTextWriter MdBold(this IndentTextWriter w, Action<IndentTextWriter> content) => w.Wrap("**", "**", content);

        /// <summary>
        /// Wraps text in Bold markdown.
        /// </summary>
        public static IndentTextWriter MdItalic(this IndentTextWriter w, string text) => w.Write($"*{text}*");
        /// <summary>
        /// Wraps content in Italic markdown.
        /// </summary>
        /// <param name="content">Delegate to build the content.</param>
        public static IndentTextWriter MdItalic(this IndentTextWriter w, Action<IndentTextWriter> content) => w.Wrap("*", "*", content);

        /// <summary>
        /// Wraps text in Bold and Italic markdown.
        /// </summary>
        public static IndentTextWriter MdBoldItalic(this IndentTextWriter w, string text) => w.Write($"***{text}***");
        /// <summary>
        /// Wraps content in Bold and Italic markdown.
        /// </summary>
        /// <param name="content">Delegate to build the content.</param>
        public static IndentTextWriter MdBoldItalic(this IndentTextWriter w, Action<IndentTextWriter> content) => w.Wrap("***", "***", content);

        /// <summary>
        /// Wraps text in Code markdown.
        /// </summary>
        public static IndentTextWriter MdCode(this IndentTextWriter w, string text) => w.Write($"`{text}`");
        /// <summary>
        /// Wraps content in Code markdown.
        /// </summary>
        /// <param name="content">Delegate to build the content.</param>
        public static IndentTextWriter MdCode(this IndentTextWriter w, Action<IndentTextWriter> content) => w.Wrap("`", "`", content);

        /// <summary>
        /// Markdown for a link.
        /// </summary>
        /// <param name="title">Link title.</param>
        /// <param name="url">Link Url.</param>
        public static IndentTextWriter MdLink(this IndentTextWriter w, string title, string url) => w.Write($"[{title}]({url})");

        private static string _ImageMd(string altText, string imagePathOrUrl, string title)
            => $"![{altText}]({imagePathOrUrl}{(string.IsNullOrWhiteSpace(title) ? "" : $" \"{title}\"")})";

        /// <summary>
        /// Markdown for an Image.
        /// </summary>
        /// <param name="altText">Image alt-text (i.e. if image can't be displayed).</param>
        /// <param name="imagePathOrUrl">Path or Url to image</param>
        /// <param name="title">Optional image tooltip title.</param>
        public static IndentTextWriter MdImage(this IndentTextWriter w, string altText, string imagePathOrUrl, string title = null)  
            => w.Write(_ImageMd(altText, imagePathOrUrl, title));

        /// <summary>
        /// Markdown for a Image and Link. 
        /// </summary>
        /// <param name="altText">Image alt-text (i.e. if image can't be displayed).</param>
        /// <param name="imagePathOrUrl">Path or Url to image</param>
        /// <param name="imageLinkUrl">Url for the image link.</param>
        /// <param name="title">Optional image tool-tip title.</param>
        public static IndentTextWriter MdLinkImage(this IndentTextWriter w, string altText, string imagePathOrUrl, string imageLinkUrl, string title = null)
            => w.MdLink(_ImageMd(altText, imagePathOrUrl, title), imageLinkUrl);

        // Table ----------------------------------------------------

        /// <group>Table building</group>
        /// <summary>
        /// Markdown for the table header.
        /// </summary>
        /// <param name="align">
        /// String of alignment characters for each column: L = left, C = Center, R = Right.
        /// </param>
        /// <param name="headers">Header for each column (as variable number or arguments)</param>
        public static IndentTextWriter MdTableHeader(this IndentTextWriter w, string align, params string[] headers) {
            bool first = true;
            foreach (var h in headers) {
                w.MdTableCol(h, first);
                first = false;
            }
            w.WriteLine();

            first = true;
            foreach (var c in align) {
                switch (c) {
                    case 'L': w.MdTableCol(":---", first); break;
                    case 'C': w.MdTableCol(":---:", first); break;
                    case 'R': w.MdTableCol("---:", first); break;
                    default : w.MdTableCol("---", first); break;
                }
                first = false;
            }
            w.WriteLine();

            return w;
        }

        /// <summary>
        /// Markdown for a column with given text (writes: "| text |" if first else "space text |")<br/>
        /// - Add a newline after the last column to complete a row.
        /// </summary>
        /// <param name="first">Set to true if this is the first column</param>
        public static IndentTextWriter MdTableCol(this IndentTextWriter w, string text, bool first = false) => w.MdTableCol(t => t.Write(text), first);
        /// <summary>
        /// Markdown for a column with given content (writes: "| content |" if first else "space content |")<br/>
        /// - Add a newline after the last column to complete a row.
        /// </summary>
        /// <param name="content">Delegate to build the content.</param>
        /// <param name="first">Set to true if this is the first column</param>
        public static IndentTextWriter MdTableCol(this IndentTextWriter w, Action<IndentTextWriter> content, bool first = false) => w.Wrap(first ? "| " : " ", " |", content);

        /// <summary>
        /// Markdown for a complete table row with a delegate to build the content for each column.
        /// </summary>
        /// <param name="cols">Delegate to build the content for each column (as variable number or arguments).</param>
        public static IndentTextWriter MdTableRow(this IndentTextWriter w, params Action<IndentTextWriter>[] cols) {
            bool first = true;
            foreach (var col in cols) {
                w.MdTableCol(col, first);
                first = false;
            }
            w.WriteLine();
            return w;
        }

        /// <summary>
        /// Markdown for a complete table row with the text for each column.
        /// </summary>
        /// <param name="cols">Text for each column (as variable number or arguments).</param>
        public static IndentTextWriter MdTableRow(this IndentTextWriter w, params string[] cols) {
            bool first = true;
            foreach (string col in cols) {
                w.MdTableCol(col, first);
                first = false;
            }
            w.WriteLine();
            return w;
        }

        // Badges ----------------------------------------------------
        // Styles: plastic, flat-square, flat, social, for-the-badge

        /// <group>Badges</group>
        /// <groupdescr>Badge Styles: flat (default), flat-square, plastic, social, for-the-badge</groupdescr>
        /// <summary>
        /// Create a generic badge with: subject, status and color:<br/>
        /// - E.g. "Tests", "Passing", "green"
        /// </summary>
        /// <param name="w"></param>
        /// <param name="subject">Badge subject</param>
        /// <param name="status">Subject status</param>
        /// <param name="color">Color name</param>
        public static IndentTextWriter MdBadge(this IndentTextWriter w, string subject, string status, string color = "blue", string style = "flat")
            => w.Write($"[![Generic bandge](https://img.shields.io/badge/{subject}-{status}-{color}.svg?style={style})](https://shields.io/)");

        /// <summary>
        /// Create a Nuget Version badge (auto retrieves the version from Nuget).
        /// </summary>
        /// <param name="packageName">Full name of package on Nuget</param>
        public static IndentTextWriter MdBadgeNugetV(this IndentTextWriter w, string packageName, string style = "flat")
            => w.Write($"[![Nuget](https://img.shields.io/nuget/v/{packageName}?style={style})](https://www.nuget.org/packages/{packageName}/)");

        /// <summary>
        /// Create a Nuget Downloads badge (auto retrieves the downloads from Nuget).
        /// </summary>
        /// <param name="packageName">Full name of package on Nuget</param>
        public static IndentTextWriter MdBadgeNugetDt(this IndentTextWriter w, string packageName, string style = "flat")
           => w.Write($"[![Nuget](https://img.shields.io/nuget/dt/{packageName}?style={style})](https://www.nuget.org/packages/{packageName}/)");

        /// <summary>
        /// Creates a C# badge
        /// </summary>
        public static IndentTextWriter MdBadgeCSharp(this IndentTextWriter w, string style = "flat")
            => w.Write($"![C#](https://img.shields.io/badge/c%23-%23239120.svg?style={style}&logo=c-sharp&logoColor=white)");

        /// <summary>
        /// Creates a MIT license badge.
        /// </summary>
        public static IndentTextWriter MdBadgeLicenseMIT(this IndentTextWriter w, string style = "flat")
            => w.Write($"[![MIT license](https://img.shields.io/badge/License-MIT-blue.svg?style={style})](https://lbesson.mit-license.org/)");
    }
}
