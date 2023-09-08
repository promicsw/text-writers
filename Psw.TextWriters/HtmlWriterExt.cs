// -----------------------------------------------------------------------------
// Copyright (c) 2023 Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Psw.TextWriters
{
    /// <summary>
    /// IndentTextWriter extensions for HTML.
    /// </summary>
    public static class HtmlWriterExt
    {
        private static string TagAttrString(string attributes) => string.IsNullOrEmpty(attributes) ? "" : $" {attributes}";

        /// <group>HTML Writing</group>
        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with optional attributes)<br/>
        /// - If content is null: then the empty tag will be written on a single line + newline.
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none)</param>
        /// <param name="content">Build the nested content</param>
        public static IndentTextWriter HtmlTag(this IndentTextWriter w, string tag, string attributes, Action<IndentTextWriter> content = null)
            => w.Block($"<{tag}{TagAttrString(attributes)}>", $"</{tag}>", content);


        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with no attributes)<br/>
        /// - If content is null: then the empty tag be written on a single line + newline.
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="content">Build the nested content</param>
        public static IndentTextWriter HtmlTag(this IndentTextWriter w, string tag, Action<IndentTextWriter> content = null) => w.HtmlTag(tag, "", content);


        /// <summary>
        /// Write a single line Html formated output where string content is wrapped in the given tag (with optional attributes)
        /// </summary>
        /// <param name="tag">Wrap content in the given tag (on a single line)</param>
        /// <param name="content">Text content to wrap in tag</param>
        /// <param name="attributes">Optional attributes for the opening tag (default = none)</param>
        public static IndentTextWriter HtmlLineTag(this IndentTextWriter w, string tag, string content, string attributes = null)
            => w.WriteLine($"<{tag}{TagAttrString(attributes)}>{content}</{tag}>");

        /// <summary>
        /// Write a Html self-closing tag with optional attributes (e.g &lt;hr>)
        /// </summary>
        /// <param name="tag">Self closing tag</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none)</param>
        public static IndentTextWriter HtmlSelfClosingTag(this IndentTextWriter w, string tag, string attributes)
            => w.WriteLine($"<{tag}{TagAttrString(attributes)}>");

        /// <summary>
        /// Write an anchor element of the form: &lt;a id="anchorID"&gt;&lt;/a&gt;
        /// </summary>
        public static IndentTextWriter HtmlAnchor(this IndentTextWriter w, string anchorID) => w. Write($"<a id=\"{anchorID}\"></a>");
    }
}
