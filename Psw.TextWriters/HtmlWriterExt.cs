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
    /// IndentTextWriter extensions for HTML writing and tag management:<br/>
    /// - Generally writes: &lt;tag optional-attributes&gt; Indented content... &lt;/tag&gt;<br/>
    /// - Also methods for single line and self closing tags.
    /// </summary>
    public static class HtmlWriterExt
    {
        private static string TagAttrString(string attributes) => string.IsNullOrEmpty(attributes) ? "" : $" {attributes}";

        /// <group>HTML Writing</group>
        /// <summary>
        /// Write indented content wrapped in the given HTML tag (with optional tag attributes):<br/>
        /// - If content is null: then the empty tag will be written on a single line + newline.
        /// </summary>
        /// <param name="tag">Tag to wrap the content in.</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none).</param>
        /// <param name="content">Build the nested content.</param>
        public static IndentTextWriter HtmlTag(this IndentTextWriter w, string tag, string attributes, Action<IndentTextWriter> content = null)
            => w.Block($"<{tag}{TagAttrString(attributes)}>", $"</{tag}>", content);


        /// <summary>
        /// Write indented content wrapped in the given HTML tag (with no attributes)<br/>
        /// </summary>
        /// <param name="tag">Tag to wrap the content in.</param>
        /// <param name="content">Build the nested content.</param>
        public static IndentTextWriter HtmlTag(this IndentTextWriter w, string tag, Action<IndentTextWriter> content = null) => w.HtmlTag(tag, "", content);


        /// <summary>
        /// Write content wrapped in the given HTML tag on a single (with optional attributes).
        /// </summary>
        /// <param name="tag">Tag to wrap the content in.</param>
        /// <param name="content">Text content.</param>
        /// <param name="attributes">Optional attributes for the opening tag (default = none).</param>
        public static IndentTextWriter HtmlLineTag(this IndentTextWriter w, string tag, string content, string attributes = null)
            => w.WriteLine($"<{tag}{TagAttrString(attributes)}>{content}</{tag}>");

        /// <summary>
        /// Write a self-closing HTML tag (no content) with optional attributes (e.g &lt;hr>, &lt;br>).
        /// </summary>
        /// <param name="tag">Self closing tag</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none)</param>
        public static IndentTextWriter HtmlSelfClosingTag(this IndentTextWriter w, string tag, string attributes)
            => w.WriteLine($"<{tag}{TagAttrString(attributes)}>");

        /// <summary>
        /// Write a HTML anchor element of the form: &lt;a id="anchorID"&gt;&lt;/a&gt;
        /// </summary>
        public static IndentTextWriter HtmlAnchor(this IndentTextWriter w, string anchorID) => w. Write($"<a id=\"{anchorID}\"></a>");
    }
}
