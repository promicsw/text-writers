# class IndentTextWriter
A Text Writer, with a fluent API, for structured text generation incorporating:<br/>
- Automatic Indentation management (using IndenSize and IndentChar).<br/>
- Extensions for HTML writing with tag management.<br/>
- Extensions for essential Markdown writing.<br/>
- Text is output to an internal or external StringBuffer.<br/>
- May be further extended to generate any kind of structured output.

> **Default Settings:** IndentSize = 4 and IndentChar = space.

| Members | Description |
| :---- | :------ |
| ***Constructor:*** |  |
| ``C: IndentTextWriter()`` | Create an IndentTextWriter using an internal StringBuffer to write Output to.<br/> |
| ``C: IndentTextWriter(StringBuilder output)`` | Create an IndentTextWriter using an external StringBuffer to write Output to:<br/>- Or creates an internal one of output is null.<br/> |
| ***Management:*** |  |
| ``P: int IndentSize`` | Get or set the Indent Size (default = 4):<br/>- May set to 0 (or a negative value) for no indentation.<br/>- See also: SetIndentSize.<br/> |
| ``F: char IndentChar`` | Get or set the Indent Char (default = space ' ' ):<br/>- See also: SetIndentChar.<br/> |
| ``P: StringBuilder Output`` | Get the attached or internal Output StringBuilder.<br/> |
| ``M: string AsString()`` | Return Output as a String.<br/> |
| ``M: IndentTextWriter SaveAs(string fileName)`` | Write Output to given file.<br/> |
| ``M: IndentTextWriter SetIndentSize(int indentSize)`` | Set the Indent Size via a fluent service (default = 4):<br/>- May set to 0 (or a negative value) for no indentation.<br/> |
| ``M: IndentTextWriter SetIndentChar(char indentChar)`` | Set the Indent Char via a fluent service (default = space ' ' ).<br/> |
| ***Writing:*** |  |
| ``M: IndentTextWriter Indent(int indent = -1)`` | Perform an Indent with the current (indent = -1) or once-off indent size.<br/>- Adds a newline if not currently at a newline.<br/><br/>**Parameters:**<br/><code>indent:</code> Set to a value greater or equal to zero for a once-off override of the current IndentSize.<br/> |
| ``M: IndentTextWriter Outdent(int count = 1)`` | Perform an Outdent for count number of times (count = -1 for all).<br/>- Adds a newline if not currently at a newline.<br/> |
| ``M: IndentTextWriter Write(string text)`` | Write text to Output with automated indentation if applicable.<br/> |
| ``M: IndentTextWriter WriteLine(string text = "")`` | Write text and newline to output with automated indentation if applicable.<br/> |
| ``M: IndentTextWriter NewLine(int noof = 1)`` | Write given number of newlines.<br/> |
| ``P: bool IsNewLine`` | Query if last write operation ended with a new line.<br/> |
| ``M: IndentTextWriter Replace(string oldValue, string newValue)`` | Replace all occurrences of oldValue with newValue in the current Output.<br/><br/>**Parameters:**<br/><code>oldValue:</code> Text to replace / remove.<br/><code>newValue:</code> Text to replace oldValue with (blank/null to just remove oldValue).<br/> |
| ``M: IndentTextWriter QuoteText(string text, char quoteChar = '"', bool escape = true)`` | Write text wrapped within given quoteChar (default = ").<br/><br/>**Parameters:**<br/><code>quoteChar:</code> Quote character to use (default = ").<br/><code>escape:</code> Optionally escape the quoteChar if present in text (by inserting a &#92; before the character).<br/> |
| ``M: static  string EscapeText(string escapeChars, string text)`` | Escape all of the escapeChars in text (by inserting a &#92; before the character).<br/> |
| ``M: IndentTextWriter Wrap(string open, string close, Action<IndentTextWriter> content)`` | Wrap content with the open and close strings:<br/>- Writes: open + content + close<br/> |
| ***Block Writing:*** |  |
| ``M: IndentTextWriter WriteIndent(Action<IndentTextWriter> content)`` | Write indented content:<br/>- Writes: newline + indent + content + outdent + newline<br/><br/>**Parameters:**<br/><code>content:</code> Build the nested content<br/> |
| ``M: IndentTextWriter Block(string open, string close, Action<IndentTextWriter> content)`` | Write indented content wrapped by the block open and close strings:<br/>- Writes: open + newline + indent + content + outdent + newline + close + newline. <br/>- If content is null writes: open + close + newline<br/><br/>**Parameters:**<br/><code>open:</code> Block opening text.<br/><code>close:</code> Block closing text.<br/><code>content:</code> Build the nested content<br/> |
| ``M: IndentTextWriter BlockCurly(Action<IndentTextWriter> content)`` | Equivalent to: Block("\{", "\}", content)<br/> |
| ``M: IndentTextWriter BlockSquare(Action<IndentTextWriter> content)`` | Equivalent to: Block("[", "]", content)<br/> |
# HTML Extensions
IndentTextWriter extensions for HTML writing and tag management:<br/>
- Generally writes: &lt;tag optional-attributes&gt; Indented content... &lt;/tag&gt;<br/>
- Also methods for single line and self closing tags.

| Extensions | Description |
| :---- | :------ |
| ***HTML Writing:*** |  |
| ``E:  HtmlTag(string tag, string attributes, Action<IndentTextWriter> content = null)`` | Write indented content wrapped in the given HTML tag (with optional tag attributes):<br/>- If content is null: then the empty tag will be written on a single line + newline.<br/><br/>**Parameters:**<br/><code>tag:</code> Tag to wrap the content in.<br/><code>attributes:</code> Optional attributes for the opening tag ("" or null for none).<br/><code>content:</code> Build the nested content.<br/> |
| ``E:  HtmlTag(string tag, Action<IndentTextWriter> content = null)`` | Write indented content wrapped in the given HTML tag (with no attributes)<br/><br/>**Parameters:**<br/><code>tag:</code> Tag to wrap the content in.<br/><code>content:</code> Build the nested content.<br/> |
| ``E:  HtmlLineTag(string tag, string content, string attributes = null)`` | Write content wrapped in the given HTML tag on a single (with optional attributes).<br/><br/>**Parameters:**<br/><code>tag:</code> Tag to wrap the content in.<br/><code>content:</code> Text content.<br/><code>attributes:</code> Optional attributes for the opening tag (default = none).<br/> |
| ``E:  HtmlSelfClosingTag(string tag, string attributes)`` | Write a self-closing HTML tag (no content) with optional attributes (e.g &lt;hr>, &lt;br>).<br/><br/>**Parameters:**<br/><code>tag:</code> Self closing tag<br/><code>attributes:</code> Optional attributes for the opening tag ("" or null for none)<br/> |
| ``E:  HtmlAnchor(string anchorID)`` | Write a HTML anchor element of the form: &lt;a id="anchorID"&gt;&lt;/a&gt;<br/> |
# Markdown Extensions
IdentTextWriter extensions for essential Markdown writing.

| Extensions | Description |
| :---- | :------ |
| ***Basic Markdown:*** |  |
| ``E:  MdHR(this w)`` | Markdown for a Horizontal Rule, and ensures a blank line before and after.<br/> |
| ``E:  MdBr(this w)`` | Create a HTML break tag: &lt;br/&gt;<br/> |
| ``E:  MdLB(this w)`` | Markdown for a Line Break by writing two spaces and a newline.<br/> |
| ``E:  MdAnchor(string anchorID)`` | Creates a HTML anchor element with given anchorID (for in-page navigation) : &lt;a id="anchorID"&gt;&lt;/a&gt;.<br/> |
| ``E:  MdBold(string text)`` | Wraps text in Bold markdown.<br/> |
| ``E:  MdBold(Action<IndentTextWriter> content)`` | Wraps content in Bold markdown.<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/> |
| ``E:  MdItalic(string text)`` | Wraps text in Italic markdown.<br/> |
| ``E:  MdItalic(Action<IndentTextWriter> content)`` | Wraps content in Italic markdown.<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/> |
| ``E:  MdBoldItalic(string text)`` | Wraps text in Bold and Italic markdown.<br/> |
| ``E:  MdBoldItalic(Action<IndentTextWriter> content)`` | Wraps content in Bold and Italic markdown.<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/> |
| ``E:  MdCode(string text)`` | Wraps text in Code markdown with single back tick.<br/> |
| ``E:  MdCode(Action<IndentTextWriter> content)`` | Wraps content in Code markdown with single back tick.<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/> |
| ``E:  MdCode2(string text)`` | Wraps text in Code markdown with double back ticks (use if text contains any back ticks).<br/> |
| ``E:  MdCode2(Action<IndentTextWriter> content)`` | Wraps content in Code markdown with double back ticks (use if content contains any back ticks).<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/> |
| ``E:  MdLink(string title, string url)`` | Markdown for a link.<br/><br/>**Parameters:**<br/><code>title:</code> Link title.<br/><code>url:</code> Link Url.<br/> |
| ``E:  MdImage(string altText, string imagePathOrUrl, string title = null)`` | Markdown for an Image.<br/><br/>**Parameters:**<br/><code>altText:</code> Image alt-text (i.e. if image can't be displayed).<br/><code>imagePathOrUrl:</code> Path or Url to image.<br/><code>title:</code> Optional image tool-tip title.<br/> |
| ``E:  MdLinkImage(string altText, string imagePathOrUrl, string imageLinkUrl, string title = null)`` | Markdown for a Image and Link.<br/><br/>**Parameters:**<br/><code>altText:</code> Image alt-text (i.e. if image can't be displayed).<br/><code>imagePathOrUrl:</code> Path or Url to image.<br/><code>imageLinkUrl:</code> Url for the image link.<br/><code>title:</code> Optional image tool-tip title.<br/> |
| ***Table building:*** |  |
| ``E:  MdTableHeader(string align, params string[] headers)`` | Markdown for a table header.<br/><br/>**Parameters:**<br/><code>align:</code> String of alignment characters for each column: L = left, C = Center, R = Right.<br/><code>headers:</code> Header for each column (as a variable number or arguments).<br/> |
| ``E:  MdTableCol(string text, bool first = false)`` | Markdown for a column with given text (writes: "\| text \|" if first else "space text \|")<br/>- Add a newline after the last column to complete a row.<br/><br/>**Parameters:**<br/><code>first:</code> Set to true if this is the first column.<br/> |
| ``E:  MdTableCol(Action<IndentTextWriter> content, bool first = false)`` | Markdown for a column with given content (writes: "\| content \|" if first else "space content \|")<br/>- Add a newline after the last column to complete a row.<br/><br/>**Parameters:**<br/><code>content:</code> Delegate to build the content.<br/><code>first:</code> Set to true if this is the first column.<br/> |
| ``E:  MdTableRow(params Action<IndentTextWriter>[] cols)`` | Markdown for a complete table row with a delegate to build the content for each column.<br/><br/>**Parameters:**<br/><code>cols:</code> Delegate to build the content for each column (as a variable number or arguments).<br/> |
| ``E:  MdTableRow(params string[] cols)`` | Markdown for a complete table row with given text for each column.<br/><br/>**Parameters:**<br/><code>cols:</code> Text for each column (as a variable number or arguments).<br/> |
| ***Badges:*** | *__Badge Styles:__ flat (default), flat-square, plastic, social, for-the-badge* |
| ``E:  MdBadge(string subject, string status, string color = "blue", string style = "flat")`` | Create a generic badge with: subject, status and color:<br/>- E.g. "Tests", "Passing", "green"<br/><br/>**Parameters:**<br/><code>subject:</code> Badge subject.<br/><code>status:</code> Subject status.<br/><code>color:</code> Color name.<br/> |
| ``E:  MdBadgeNugetV(string packageName, string style = "flat")`` | Create a Nuget Version badge (auto retrieves the version from Nuget).<br/><br/>**Parameters:**<br/><code>packageName:</code> Full name of package on Nuget.<br/> |
| ``E:  MdBadgeNugetDt(string packageName, string style = "flat")`` | Create a Nuget Downloads badge (auto retrieves the downloads from Nuget).<br/><br/>**Parameters:**<br/><code>packageName:</code> Full name of package on Nuget.<br/> |
| ``E:  MdBadgeCSharp(string style = "flat")`` | Create a C# badge<br/> |
| ``E:  MdBadgeLicenseMIT(string style = "flat")`` | Create a MIT license badge.<br/> |
