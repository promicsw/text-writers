// -----------------------------------------------------------------------------
// Copyright (c) 2023 Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Psw.TextWriters
{
    /// <summary>
    /// A text writer for structured text generation incorporating indentation and HTML tag management via a fluent API:
    /// - Text is output to an internal or external StringBuffer.
    /// - May be further extended to generate any kind of structured output.
    /// </summary>
    public class IndentTextWriter
    {
        /// <group>Constructor</group>
        /// <summary>
        /// Constructor with default settings for output StringBuilder and indent size
        /// </summary>
        /// <param name="output">Internal (default) or external StringBuilder where output will be written to.</param>
        /// <param name="indentSize">Indent size (default = 4)</param>
        public IndentTextWriter(StringBuilder output = null, int indentSize = 4) {
            Output = output ?? new StringBuilder();
            IndentSize = indentSize;
        }

        /// <group>Management</group>
        /// <summary>
        /// Get or set the default indent size
        /// </summary>
        public int IndentSize { get; set; }

        /// <summary>
        /// Get the attached or internal Output StringBuilder
        /// </summary>
        public StringBuilder Output { get; private set; }

        /// <summary>
        /// Return Output as a String
        /// </summary>
        public string AsString() => Output.ToString();

        /// <summary>
        /// Write Output to given file
        /// </summary>
        public IndentTextWriter SaveAs(string fileName) {
            File.WriteAllText(fileName, AsString());
            return this;
        }

        /// <group>Writing</group>
        /// <summary>
        /// Perform an Indent with given or default (indent = -1) indent size.<br/>
        /// - Adds a newline if not currently at a newline.
        /// </summary>
        public IndentTextWriter Indent(int indent = -1) {
            if (indent == -1) indent = IndentSize;
            _indent += indent;
            _indentStack.Push(indent);
            if (!IsNewLine) NewLine();
            return this;
        }

        /// <summary>
        /// Perform an Outdent for count number of times (count = -1 for all).
        /// - Adds a newline if not currently at a newline.
        /// </summary>
        public IndentTextWriter Outdent(int count = 1) {
            int repeat = count < 0 || count > _indentStack.Count ? _indentStack.Count : count;
            while (repeat-- > 0) _indent -= _indentStack.Pop();
            if (!IsNewLine) NewLine();
            return this;
        }

        /// <summary>
        /// Write text to Output with automated indentation if applicable.
        /// </summary>
        public IndentTextWriter Write(string text) {
            _writeIndent();
            Output.Append(text);
            return this;
        }

        /// <summary>
        /// Write text and newline to output with automated indentation if applicable.
        /// </summary>
        public IndentTextWriter WriteLine(string text = "") {
            _writeIndent();
            Output.AppendLine(text);
            IsNewLine = true;
            return this;
        }

        /// <summary>
        /// Write given number of newlines.
        /// </summary>
        public IndentTextWriter NewLine(int noof = 1) {
            while (noof-- > 0) WriteLine();
            return this;
        }

        /// <summary>
        /// Query if last write operation ended with a new line
        /// </summary>
        public bool IsNewLine { get; private set; }


        /// <summary>
        /// Replace all occurrences of oldValue with newValue (newValue: "" or null to just remove oldValue)
        /// </summary>
        public IndentTextWriter Replace(string oldValue, string newValue) { Output.Replace(oldValue, newValue); return this; }

        /// <summary>
        /// Write operation enclosing text withing quotes ".." optionally escaping the " character as &#92;" 
        /// </summary>
        public IndentTextWriter QuoteText(string text, bool escape = true) => Write($"\"{(escape ? IndentTextWriter.EscapeText(text) : text)}\"");

        /// <summary>
        /// Return given text string with any " characters escaped as &#92;" 
        /// </summary>
        public static string EscapeText(string text) => text.Replace("\"", "\\\"");

        /// <summary>
        /// Escape all of the escapeChars in text (by inserting a &#92; before the character).
        /// </summary>
        public static string EscapeChars(string escapeChars, string text) {
            foreach (char c in escapeChars) {
                if (text.Contains($"{c}")) text = text.Replace($"{c}", $"\\{c}");
            }
            return text;
        }

        // Wrapping ---------------------------------------------------

        /// <summary>
        /// Wrap content with the open and close strings:<br/>
        /// - Writes: open + content + close 
        /// </summary>
        public IndentTextWriter Wrap(string open, string close, Action<IndentTextWriter> content) {
            Write(open);
            content?.Invoke(this);
            Write(close);
            return this;
        }

        // Blocks ---------------------------------------------------

        /// <group>Block Writing</group>
        /// <summary>
        /// Write indented content:<br/>
        /// - Writes: newline + indent + content + outdent + newline<br/> 
        /// </summary>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter WriteIndent(Action<IndentTextWriter> content) {
            Indent();
            content?.Invoke(this);
            Outdent();
            return this;
        }

        /// <summary>
        /// Write indented content wrapped by the block open and close strings:<br/>
        /// - Writes: open + newline + indent + content + outdent + newline + close + newline<br/> 
        /// - If content is null writes: open + close + newline<br/>
        /// </summary>
        /// <param name="open">Block opening text (typically "{")</param>
        /// <param name="close">Block closing text (typically "}") </param>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter Block(string open, string close, Action<IndentTextWriter> content) {
            if (content == null) WriteLine($"{open}{close}");
            else {
                WriteLine(open).Indent();
                content?.Invoke(this);
                Outdent().WriteLine(close);
            }
            return this;
        }

        /// <summary>
        /// Equivalent to: Block("{", "}", content) 
        /// </summary>
        public IndentTextWriter BlockCurly(Action<IndentTextWriter> content) => Block("{", "}", content);
        /// <summary>
        /// Equivalent to: Block("[", "]", content) 
        /// </summary>
        public IndentTextWriter BlockSquare(Action<IndentTextWriter> content) => Block("[", "]", content);

        // HTML Utilities -------------------------------------------
        /***
        protected static string TagAttrString(string attributes) => string.IsNullOrEmpty(attributes) ? "" : $" {attributes}";

        /// <group>HTML Writing</group>
        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with optional attributes)<br/>
        /// - If content is null: then the empty tag will be written on a single line + newline.
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none)</param>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter HtmlTag(string tag, string attributes, Action<IndentTextWriter> content = null)
            => Block($"<{tag}{TagAttrString(attributes)}>", $"</{tag}>", content);


        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with no attributes)<br/>
        /// - If content is null: then the empty tag be written on a single line + newline.
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter HtmlTag(string tag, Action<IndentTextWriter> content = null) => HtmlTag(tag, "", content);


        /// <summary>
        /// Write a single line Html formated output where string content is wrapped in the given tag (with optional attributes)
        /// </summary>
        /// <param name="tag">Wrap content in the given tag (on a single line)</param>
        /// <param name="content">Text content to wrap in tag</param>
        /// <param name="attributes">Optional attributes for the opening tag (default = none)</param>
        public IndentTextWriter HtmlLineTag(string tag, string content, string attributes = null) {
            WriteLine($"<{tag}{TagAttrString(attributes)}>{content}</{tag}>");
            return this;
        }

        /// <summary>
        /// Write a Html self-closing tag with optional attributes (e.g &lt;hr>)
        /// </summary>
        /// <param name="tag">Self closing tag</param>
        /// <param name="attributes">Optional attributes for the opening tag ("" or null for none)</param>
        public IndentTextWriter HtmlSelfClosingTag(string tag, string attributes) {
            WriteLine($"<{tag}{TagAttrString(attributes)}>");
            return this;
        }

        /// <summary>
        /// Write an anchor element of the form: &lt;a id="anchorID"&gt;&lt;/a&gt;
        /// </summary>
        public IndentTextWriter HtmlAnchor(string anchorID) { Write($"<a id=\"{anchorID}\"></a>"); return this; }
        ***/

        // Private Implementation -----------------------------------

        private Stack<int> _indentStack = new Stack<int>();
        private int _indent = 0;

        
        /// <summary>
        /// If just after a new line - add spaces for indent
        /// </summary>
        private void _writeIndent() {
            if (IsNewLine) {
                Output.Append(' ', _indent);
                IsNewLine = false;
            }
        }

    }
}
