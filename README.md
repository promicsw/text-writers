# Indent Text Writer

A text writer for structured text generation with automatic indentation and HTML tag management via a fluent API:
- Text is output to an internal or external StringBuffer.
- May be further extended to generate any kind of structured output.


- [IndentTextWriter](Docs/IndentTextWriter.md): Reference document.

<!--
[![NuGet version (SoftCircuits.Silk)](https://img.shields.io/nuget/v/SoftCircuits.Silk.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.Silk/) -->

<!-- [![NuGet](https://img.shields.io/nuget/v/roguesharp)]() -->

## Example text writing:
```csharp
using Psw.TextWriters;

void BasicSample() {
    var w = new IndentTextWriter();
    w.WL("class ClassName")
     .WL("{").Indent()
     .WL("public int SomeValue { get; set; }")
     .WL()
     .WL("public int SomeMethod(int value) {").Indent()
     .WL("return value * 2;")
     .Outdent().WL("}")
     .Outdent().WL("}");

    Console.WriteLine(w.AsString());
}

// Same as BasicSample using the Block methods
void BlockSample() {
    var w = new IndentTextWriter();
    w.WL("class ClassName")
     .BlockCurly(b => b
         .WL("public int SomeValue { get; set; }")
         .WL()
         .W("public int SomeMethod(int value) ")
         .BlockCurly(b => b.WL("return value * 2;"))
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
Sample Html writing:
```csharp
void HtmlSample() {
    var w = new IndentTextWriter();
    w.HtmlTag("div", "class='some-class'", c =>
        c.HtmlLineTag("p", "Some paragraph text")
         .HtmlTag("div", c => c.WL("Inner text"))
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

