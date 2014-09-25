﻿using AngleSharp;
using AngleSharp.Parser.Css;
using AngleSharp.Parser.Html;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using UnitTests.Mocks;

namespace UnitTests
{
    [TestClass]
    public class AsyncParsingTests
    {
        [TestMethod]
        public void TestAsyncCssParsingFromStream()
        {
            var text = "h1 { color: red; } h2 { color: blue; } p { font-family: Arial; } div { margin: 10 }";
            var source = new DelayedStream(Encoding.UTF8.GetBytes(text));
            var parser = new CssParser(source, Configuration.Default);
            var task = parser.ParseAsync();
            Assert.IsFalse(task.IsCompleted);
            Assert.IsNotNull(parser.Result);
            task.Wait();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsNotNull(parser.Result);

            Assert.AreEqual(4, parser.Result.Rules.Length);
        }

        [TestMethod]
        public void TestAsyncHtmlParsingFromStream()
        {
            var text = "<html><head><title>My test</title></head><body><p>Some text</p></body></html>";
            var source = new DelayedStream(Encoding.UTF8.GetBytes(text));
            var parser = new HtmlParser(source, Configuration.Default);
            var task = parser.ParseAsync();
            Assert.IsFalse(task.IsCompleted);
            Assert.IsNotNull(parser.Result);
            task.Wait();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsNotNull(parser.Result);

            Assert.AreEqual("My test", parser.Result.Title);
            Assert.AreEqual(1, parser.Result.Body.ChildElementCount);
            Assert.AreEqual("Some text", parser.Result.Body.Children[0].TextContent);
        }

        [TestMethod]
        public void TestAsyncCssParsingFromString()
        {
            var source = "h1 { color: red; } h2 { color: blue; } p { font-family: Arial; } div { margin: 10 }";
            var parser = new CssParser(source, Configuration.Default);
            var task = parser.ParseAsync();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsNotNull(parser.Result);

            Assert.AreEqual(4, parser.Result.Rules.Length);
        }

        [TestMethod]
        public void TestAsyncHtmlParsingFromString()
        {
            var source = "<html><head><title>My test</title></head><body><p>Some text</p></body></html>";
            var parser = new HtmlParser(source, Configuration.Default);
            var task = parser.ParseAsync();
            Assert.IsTrue(task.IsCompleted);
            Assert.IsNotNull(parser.Result);

            Assert.AreEqual("My test", parser.Result.Title);
            Assert.AreEqual(1, parser.Result.Body.ChildElementCount);
            Assert.AreEqual("Some text", parser.Result.Body.Children[0].TextContent);
        }
    }
}
