# class IndentTextWriter

A text writer for structured text generation with indentation and HTML tag management via a fluent C# API:
- Text is output to an internal or external StringBuffer.
- May be further extended to generate any kind of structured output.

|Member|Description|
|----|------|
|**Constructor:**||
|`C: IndentTextWriter(StringBuilder output = null, int indentSize = 4)`|Constructor with default settings for output StringBuilder and indent size<br/><br/>**Parameters:**<br/><code>output:</code> Internal (default) or external StringBuilder where output will be written to.<br/><code>indentSize:</code> Indent size (default = 4)<br/>|
|**Management:**||
|`P: int IndentSize`|Get or set the default indent size<br/>|
|`P: StringBuilder Output`|Get the attached or internal Output StringBuilder<br/>|
|`M: string AsString()`|Return Output as a String<br/>|
|`M: IndentTextWriter SaveAs(string fileName)`|Write Output to given file<br/>|
|**Writing:**||
|`M: IndentTextWriter Indent(int indent = -1)`|Perform an Indent with given or default (indent = -1) indent size.<br/>- Adds a newline if not currently at a newline.<br/>|
|`M: IndentTextWriter Outdent(int count = 1)`|Perform an Outdent for count number of times (count = -1 for all).<br/>- Adds a newline if not currently at a newline.<br/>|
|`M: IndentTextWriter Write(string text)`|Write text to Output with automated indentation if applicable.<br/>|
|`M: IndentTextWriter WriteLine(string text = "")`|Write text and newline to output with automated indentation if applicable.<br/>|
|`M: IndentTextWriter W(string text = "")`|Type-saver alias for Write: Write text to Output with automated indentation if applicable.<br/>|
|`M: IndentTextWriter WL(string text = "")`|Type-saver alias for WriteLine: Write text and newline to output with automated indentation if applicable.<br/>|
|`M: IndentTextWriter NL()`|Type-saver for WriteLine("").<br/>|
|`M: IndentTextWriter Replace(string oldValue, string newValue)`|Replace all occurrences of oldValue with newValue (newValue: "" or null to just remove oldValue)<br/>|
|`M: IndentTextWriter QuoteText(string text, bool escape = true)`|Write operation enclosing text withing quotes ".." optionally escaping the " character as &#92;"<br/>|
|`M: string EscapeText(string text)`|Return given text string with any " characters escaped as &#92;"<br/>|
|**Blocks:**||
|`M: IndentTextWriter Block(string open, string close, Action<IndentTextWriter> content)`|Write content (indented) and wrapped by the block open and close strings:<br/>- Writes: open + indent + content + outdent + close + newline <br/>- If content is null writes: open + close + newline<br/><br/>**Parameters:**<br/><code>open:</code> Block opening text (typically "\{")<br/><code>close:</code> Block closing text (typically "\}")<br/><code>content:</code> Build the nested content<br/>|
|`M: IndentTextWriter BlockCurly(Action<IndentTextWriter> content)`|Equivalent to: Block("\{", "\}", content)<br/>|
|`M: IndentTextWriter BlockSquare(Action<IndentTextWriter> content)`|Equivalent to: Block("[", "]", content)<br/>|
|**HTML:**||
|`M: IndentTextWriter HtmlTag(string tag, string attributes, Action<IndentTextWriter> content = null)`|Write Html formated output where content is indented and wrapped in the given tag (with optional attributes)<br/>- If content is null: then the empty tag be written on a single line + newline.<br/><br/>**Parameters:**<br/><code>tag:</code> Wrap content in the given tag with indenting<br/><code>attributes:</code> Optional attributes for the opening tag<br/><code>content:</code> Build the nested content<br/>|
|`M: IndentTextWriter HtmlTag(string tag, Action<IndentTextWriter> content = null)`|Write Html formated output where content is indented and wrapped in the given tag (with no attributes)<br/>- If content is null: then the empty tag be written on a single line + newline.<br/><br/>**Parameters:**<br/><code>tag:</code> Wrap content in the given tag with indenting<br/><code>content:</code> Build the nested content<br/>|
|`M: IndentTextWriter HtmlLineTag(string tag, string content, string attributes = null)`|Write a single line Html formated output where string content is wrapped in the given tag (with optional attributes)<br/><br/>**Parameters:**<br/><code>tag:</code> Wrap content in the given tag (on a single line)<br/><code>content:</code> Text content to wrap in tag<br/><code>attributes:</code> Optional attributes for the opening tag (default = null)<br/>|
|`M: IndentTextWriter HtmlSelfClosingTag(string tag, string attributes)`|Write a Html self-closing tag with optional attributes (e.g &lt;hr>)<br/>|

