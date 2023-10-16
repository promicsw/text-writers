// -----------------------------------------------------------------------------
// Copyright (c) 2023 Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using Psw.TextWriters;

BasicSample();
BlockSample();
HtmlSample();
MarkdownSample();

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

void HtmlSample() {
    var w = new IndentTextWriter().SetIndentSize(2);
    w.HtmlTag("div", "class='some-class'", c => c
        .HtmlLineTag("p", "Some paragraph text")
        .HtmlTag("div", c => c.WriteLine("Inner text"))
    );
    Console.WriteLine(w.AsString());
}

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

    var outPath = @"D:\PromicSW_GitHub\TextWriters\Examples\TextWriterExamples\Output\";
    //w.SaveAs(outPath + "mdsample.md");
}