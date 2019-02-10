using ExampleConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskSolution;

namespace TestProject
{
    class TestDataAggregator : DataAggregatorBase
    {
        public override void LogStatus()
        {
            Assert.AreEqual(12, _numWords);

            Assert.AreEqual(62, _numChars);

            IEnumerable<KeyValuePair<String, int>> first5 = _words.Take(5);
            Assert.AreEqual(5, first5.Count());
            Assert.AreEqual("s", first5.First().Key);

            IEnumerable<KeyValuePair<String, int>> last5 = _words.Reverse().Take(5);
            Assert.AreEqual(5, last5.Count());
            Assert.AreEqual("1234567890", last5.First().Key);

            IEnumerable<KeyValuePair<String, int>> mostFrequent = _words.OrderByDescending(t => t.Value).Take(10);

            // checking the boundary values: first and last
            // first one must be "the", because this in the only word found twice
            Assert.AreEqual("the", mostFrequent.First().Key);
            Assert.AreEqual(2, mostFrequent.First().Value);

            // last one can occur only once
            Assert.AreEqual(1, mostFrequent.Last().Value);

            IEnumerable<KeyValuePair<char, int>> revChars = _characters.OrderByDescending(t => t.Value);

            // checking a few most frequent symbols
            Assert.AreEqual(10, _characters[' ']);
            Assert.AreEqual(4, _characters['o']);
            Assert.AreEqual(3, _characters['e']);
            Assert.AreEqual(2, _characters['a']);
        }
    }
}
