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

void HtmlSample() {
    var w = new IndentTextWriter();
    w.HtmlTag("div", "class='some-class'", c =>
        c.HtmlLineTag("p", "Some paragraph text")
         .HtmlTag("div", c => c.WL("Inner text"))
    );
    Console.WriteLine(w.AsString());
}