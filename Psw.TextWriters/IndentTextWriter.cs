// -----------------------------------------------------------------------------
// Copyright (c) Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using System.Text;

namespace Psw.TextWriters
{
    /// <summary>
    /// Provides a text writer for structured text generation with automatic indentation management via a fluent API:
    /// - Text is output to an internal or external StringBuffer.
    /// - Includes some methods for writing structured HTML with automatic tag management.
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

        /// <summary>
        /// Get or set the default indent size
        /// </summary>
        public int IndentSize { get; set; }

        /// <group>Properties</group>
        /// <summary>
        /// Get the attached or internal Output StringBuilder
        /// </summary>
        public StringBuilder Output { get; private set; }

        /// <group>Core Methods</group>
        /// <summary>
        /// Perform an Indent with given or default (indent = -1) indent size.<br/>
        /// - Adds a newline if not currently at a newline.
        /// </summary>
        public IndentTextWriter Indent(int indent = -1) {
            if (indent == -1) indent = IndentSize;
            _indent += indent;
            _indentStack.Push(indent);
            if (!IsNewLine) NL();
            return this;
        }

        /// <summary>
        /// Perform an Outdent for count number of times (count = -1 for all).
        /// - Adds a newline if not currently at a newline.
        /// </summary>
        public IndentTextWriter Outdent(int count = 1) {
            int repeat = count < 0 || count > _indentStack.Count ? _indentStack.Count : count;
            while (repeat-- > 0) _indent -= _indentStack.Pop();
            if (!IsNewLine) NL();
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
        /// Type-saver alias for Write: Write text to Output with automated indentation if applicable.
        /// </summary>
        public IndentTextWriter W(string text = "") => Write(text);

        /// <summary>
        /// Type-saver alias for WriteLine: Write text and newline to output with automated indentation if applicable.
        /// </summary>
        public IndentTextWriter WL(string text = "") => WriteLine(text);

        /// <summary>
        /// Type-saver for WriteLine("").
        /// </summary>
        public IndentTextWriter NL() => WriteLine();

        /// <summary>
        /// Replace all occurrences of oldValue with newValue (newValue: "" or null to just remove oldValue)
        /// </summary>
        public IndentTextWriter Replace(string oldValue, string? newValue) { Output.Replace(oldValue, newValue); return this; }

        /// <summary>
        /// Write operation enclosing text withing quotes ".." optionally escaping the " character as &#92;" 
        /// </summary>
        public IndentTextWriter QuoteText(string text, bool escape = true) => Write($"\"{(escape ? EscapeText(text) : text)}\"");

        /// <summary>
        /// Return given text string with any " characters escaped as &#92;" 
        /// </summary>
        public string EscapeText(string text) => text.Replace("\"", "\\\"");

        /// <summary>
        /// Return Output as a String
        /// </summary>
        public string AsString() => Output.ToString();

        /// <summary>
        /// Write output to given file name (fname)
        /// </summary>
        public IndentTextWriter SaveAs(string fname) {
            File.WriteAllText(fname, AsString());
            return this;
        }

        // HTML Utilities -------------------------------------------

        /// <group>HTML Utilities</group>
        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with optional attributes)
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="attributes">Optional attributes for the opening tag</param>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter HtmlTag(string tag, string attributes, Action<IndentTextWriter> content = null) {
            WL($"<{tag}{(!string.IsNullOrEmpty(attributes) ? $" {attributes}" : "")}>");
            if (content != null) {
                Indent();
                content(this);
                Outdent();
            }
            WL($"</{tag}>");
            return this;
        }

        /// <summary>
        /// Write Html formated output where content is indented and wrapped in the given tag (with no attributes)
        /// </summary>
        /// <param name="tag">Wrap content in the given tag with indenting</param>
        /// <param name="content">Build the nested content</param>
        public IndentTextWriter HtmlTag(string tag, Action<IndentTextWriter> content = null) => HtmlTag(tag, "", content);


        /// <summary>
        /// Write a single line Html formated output where string content is wrapped in the given tag (with optional attributes)
        /// </summary>
        /// <param name="tag">Wrap content in the given tag (on a single line)</param>
        /// <param name="content">Text content to wrap in tag</param>
        /// <param name="attributes">Optional attributes for the opening tag (default = null)</param>
        public IndentTextWriter HtmlLineTag(string tag, string content, string attributes = null) {
            WL($"<{tag}{(!string.IsNullOrEmpty(attributes) ? $" {attributes}" : "")}>{content}</{tag}>");
            return this;
        }

        /// <summary>
        /// Write a Html self-closing tag with optional attributes (e.g &lt;hr>)
        /// </summary>
        public IndentTextWriter HtmlSelfClosingTag(string tag, string attributes) {
            WL($"<{tag}{(!string.IsNullOrEmpty(attributes) ? $" {attributes}" : "")}>");
            return this;
        }

        // Private Implementation -----------------------------------

        private Stack<int> _indentStack = new Stack<int>();
        private int _indent = 0;

        /// <summary>
        /// Query if last write operation ended with a new line
        /// </summary>
        protected bool IsNewLine { get; private set; }

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
