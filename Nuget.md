# Indent Text Writer

A Text Writer, with a fluent API, for structured text generation incorporating:
- Automatic Indentation management.
- Extensions for HTML writing with tag management.
- Extensions for essential Markdown writing.
- Text is output to an internal or external StringBuffer.
- May be further extended to generate any kind of structured output.

## Example text writing:
```csharp
using Psw.TextWriters;

void BasicSample() {
    var w = new IndentTextWriter();
    w.WriteLine("class ClassName")
     .WriteLine("{").Indent()
     .WriteLine("public int SomeValue { get; set; }")
     .WriteLine()
     .WriteLine("public int SomeMethod(int value) {").Indent()
     .WriteLine("return value * 2;")
     .Outdent().WriteLine("}")
     .Outdent().WriteLine("}");

    Console.WriteLine(w.AsString());
}

// Same as BasicSample using the Block methods
void BlockSample() {
    var w = new IndentTextWriter();
    w.WriteLine("class ClassName")
     .BlockCurly(b => b
         .WriteLine("public int SomeValue { get; set; }")
         .WriteLine()
         .Write("public int SomeMethod(int value) ")
         .BlockCurly(b => b.WriteLine("return value * 2;"))
     );

    Console.WriteLine(w.AsString());
}

```
Output of the above:
```con
class ClassName
{
    public int SomeValue { get; set; }

    public int SomeMethod(int value) {
        return value * 2;
    }
}
```

## Sample Html writing:

```csharp
void HtmlSample() {
    var w = new IndentTextWriter();
    w.HtmlTag("div", "class='some-class'", c => c
        .HtmlLineTag("p", "Some paragraph text")
        .HtmlTag("div", c => c.WriteLine("Inner text"))
    );
    Console.WriteLine(w.AsString());
}
```
Output of above:
```html
<div class='some-class'>
    <p>Some paragraph text</p>
    <div>
        Inner text
    </div>
</div>
```
## Sample Markdown writing:
```csharp
void MarkdownSample() {
    var w = new IndentTextWriter();
    w.WriteLine("# Markdown output:")
     .Write("This is ").MdBold("bold").MdLB()
     .Write("This is ").MdItalic("italic").MdLB()
     .Write("This is ").MdBoldItalic("bold and italic").MdLB()
     .Write("This is ").MdCode("code").NewLine(2)
     .Write("Link: ").MdLink("link title", "https://www.example.com").MdLB()
     .Write("Image: ").MdImage("alt text", "/assets/images/san-juan-mountains.jpg").MdLB()
     .Write("Image with title: ").MdImage("alt text", "/assets/images/san-juan-mountains.jpg", "San Juan Mountains").MdLB()
     .Write("LinkImage: ").MdLinkImage("alt text", "mountains.jpg", "https://www.pics.com/mountains", "Mountains").MdLB();

    w.NewLine().WriteLine("## Markdown Table")
     .MdTableHeader("LCR", "Left", "Center", "Right")
     .MdTableRow("Col 1 Row 1", "Col 2 Row 1", "Col 3 Row 1")
     .MdTableRow("Col 1 Row 2", "Col 2 Row 2", "Col 3 Row 2")
     .MdTableRow(
        c => c.Write("This is ").MdBold("bold").MdBr()
              .Write("Bold Link: ").MdBold(m => m.MdLink("bold link title", "https://www.example.com")),
        c => c.Write("This is ").MdItalic("italic"),
        c => c.Write("Link: ").MdLink("link title", "https://www.example.com")
      );

    w.NewLine(2).WriteLine("## Badges")
     .MdBadgeNugetV("Psw.TextWriters").NewLine().MdBadgeNugetDt("Psw.TextWriters").NewLine()
     .MdBadgeCSharp().NewLine().MdBadgeLicenseMIT().NewLine()
     .MdBadge("subject", "status", "pink");

    Console.WriteLine(w.AsString());
}
```
Output of above:
```
# Markdown output:
This is **bold**
This is *italic*
This is ***bold and italic***
This is `code`

Link: [link title](https://www.example.com)
Image: ![alt text](/assets/images/san-juan-mountains.jpg)
Image with title: ![alt text](/assets/images/san-juan-mountains.jpg "San Juan Mountains")
LinkImage: [![alt text](mountains.jpg "Mountains")](https://www.pics.com/mountains)

## Markdown Table
| Left | Center | Right |
| :--- | :---: | ---: |
| Col 1 Row 1 | Col 2 Row 1 | Col 3 Row 1 |
| Col 1 Row 2 | Col 2 Row 2 | Col 3 Row 2 |
| This is **bold**<br/>Bold Link: **[bold link title](https://www.example.com)** | This is *italic* | Link: [link title](https://www.example.com) |


## Badges
[![Nuget](https://img.shields.io/nuget/v/Psw.TextWriters?style=flat)](https://www.nuget.org/packages/Psw.TextWriters/)
[![Nuget](https://img.shields.io/nuget/dt/Psw.TextWriters?style=flat)](https://www.nuget.org/packages/Psw.TextWriters/)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=flat&logo=c-sharp&logoColor=white)
[![MIT license](https://img.shields.io/badge/License-MIT-blue.svg?style=flat)](https://lbesson.mit-license.org/)
[![Generic bandge](https://img.shields.io/badge/subject-status-pink.svg?style=flat)](https://shields.io/)
```


