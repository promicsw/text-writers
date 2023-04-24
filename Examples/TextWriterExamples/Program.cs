// -----------------------------------------------------------------------------
// Copyright (c) 2023 Promic Software. All rights reserved.
// Licensed under the MIT License (MIT).
// -----------------------------------------------------------------------------

using Psw.TextWriters;

BasicSample();
BlockSample();
HtmlSample();

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
    var w = new IndentTextWriter();
    w.HtmlTag("div", "class='some-class'", c => c
        .HtmlLineTag("p", "Some paragraph text")
        .HtmlTag("div", c => c.WriteLine("Inner text"))
    );
    Console.WriteLine(w.AsString());
}