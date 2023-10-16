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
    /// A Text Writer, with a fluent API, for structured text generation incorporating:<br/>
    /// - Automatic Indentation management (using IndenSize and IndentChar).<br/>
    /// - Extensions for HTML writing with tag management.<br/>
    /// - Extensions for essential Markdown writing.<br/>
    /// - Text is output to an internal or external StringBuffer.<br/>
    /// - May be further extended to generate any kind of structured output.
    /// </summary>
    /// <mdoc>
    /// > **Default Settings:** IndentSize = 4 and IndentChar = space.
    /// </mdoc>
    public class IndentTextWriter
    {

        /// <group>Constructor</group>
        /// <summary>
        /// Create an IndentTextWriter using an internal StringBuffer to write Output to.
        /// </summary>
        public IndentTextWriter() => Output =  new StringBuilder();

        /// <summary>
        /// Create an IndentTextWriter using an external StringBuffer to write Output to:<br/>
        /// - Or creates an internal one of output is null.
        /// </summary>
        public IndentTextWriter(StringBuilder output) => Output = output ?? new StringBuilder();


        /// <group>Management</group>
        /// <summary>
        /// Get or set the Indent Size (default = 4):<br/>
        /// - May set to 0 (or a negative value) for no indentation.<br/>
        /// - See also: SetIndentSize.
        /// </summary>
        public int IndentSize {
            get => _indentSize;
            set => _indentSize = value < 0 ? 0 : value;  // Ensure >= 0
        }

        /// <summary>
        /// Get or set the Indent Char (default = space ' ' ):<br/>
        /// - See also: SetIndentChar.
        /// </summary>
        public char IndentChar = ' ';

        /// <summary>
        /// Get the attached or internal Output StringBuilder.
        /// </summary>
        public StringBuilder Output { get; private set; }

        /// <summary>
        /// Return Output as a String.
        /// </summary>
        public string AsString() => Output.ToString();

        /// <summary>
        /// Write Output to given file.
        /// </summary>
        public IndentTextWriter SaveAs(string fileName) {
            File.WriteAllText(fileName, AsString());
            return this;
        }

        /// <summary>
        /// Set the Indent Size via a fluent service (default = 4):<br/>
        /// - May set to 0 (or a negative value) for no indentation.
        /// </summary>
        public IndentTextWriter SetIndentSize(int indentSize) { _indentSize = indentSize; return this; }

        /// <summary>
        /// Set the Indent Char via a fluent service (default = space ' ' ).
        /// </summary>
        public IndentTextWriter SetIndentChar(char indentChar) { IndentChar = indentChar; return this; }

        /// <group>Writing</group>
        /// <summary>
        /// Perform an Indent with the current (indent = -1) or once-off indent size.<br/>
        /// - Adds a newline if not currently at a newline.
        /// </summary>
        /// <param name="indent">
        /// Set to a value greater or equal to zero for a once-off override of the current IndentSize. 
        /// </param>
        public IndentTextWriter Indent(int indent = -1) {
            if (indent < 0) indent = _indentSize;
            _indent += indent;
            _indentStack.Push(indent);
            if (!IsNewLine) NewLine();
            return this;
        }

        /// <summary>
        /// Perform an Outdent for count number of times (count = -1 for all).<br/>
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
        /// Query if last write operation ended with a new line.
        /// </summary>
        public bool IsNewLine { get; private set; }


        /// <summary>
        /// Replace all occurrences of oldValue with newValue in the current Output.
        /// </summary>
        /// <param name="oldValue">Text to replace / remove.</param>
        /// <param name="newValue">Text to replace oldValue with (blank/null to just remove oldValue).</param>
        public IndentTextWriter Replace(string oldValue, string newValue) { Output.Replace(oldValue, newValue); return this; }

        /// <summary>
        /// Write text wrapped within given quoteChar (default = "). 
        /// </summary>
        /// <param name="quoteChar">Quote character to use (default = ").</param>
        /// <param name="escape">Optionally escape the quoteChar if present in text (by inserting a &#92; before the character).</param>
        public IndentTextWriter QuoteText(string text, char quoteChar = '"', bool escape = true) 
            => Write($"{quoteChar}{(escape ? IndentTextWriter.EscapeText($"{quoteChar}", text) : text)}{quoteChar}");


        /// <summary>
        /// Escape all of the escapeChars in text (by inserting a &#92; before the character).
        /// </summary>
        public static string EscapeText(string escapeChars, string text) {
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
        /// - Writes: open + newline + indent + content + outdent + newline + close + newline.<br/> 
        /// - If content is null writes: open + close + newline<br/>
        /// </summary>
        /// <param name="open">Block opening text.</param>
        /// <param name="close">Block closing text. </param>
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


        // Private Implementation -----------------------------------

        private int _indentSize = 4;
        private Stack<int> _indentStack = new Stack<int>();
        private int _indent = 0;

        
        /// <summary>
        /// If just after a new line - add spaces for indent
        /// </summary>
        private void _writeIndent() {
            if (IsNewLine) {
                Output.Append(IndentChar, _indent);
                IsNewLine = false;
            }
        }

    }
}
