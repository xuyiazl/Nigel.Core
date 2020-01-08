using NUnit.Framework;
using Nigel.Core;
using System.Collections.Generic;
using System;

namespace Nige.Core.NUnitTest
{
    public class StringExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanParseKeyValuePairs()
        {
            string test = "city=shanghai,state=nn, zipcode=12345, Country=china";
            IDictionary<string, string> pairs = test.ToMap(',', '=', false, true);

            Assert.AreEqual(pairs["city"], "shanghai");
            Assert.AreEqual(pairs["state"], "nn");
            Assert.AreEqual(pairs["zipcode"], "12345");
            Assert.AreEqual(pairs["Country"], "china");
        }

        [Test]
        public void CanParseDelimitedData()
        {
            string[] pageNums = "search-classes-workshops-4-1-2-6".GetDelimitedChars("search-classes-workshops-", '-');

            Assert.AreEqual(4, pageNums.Length);
            Assert.AreEqual("4", pageNums[0]);
            Assert.AreEqual("1", pageNums[1]);
            Assert.AreEqual("2", pageNums[2]);
            Assert.AreEqual("6", pageNums[3]);
        }

        [Test]
        public void CanConvertSingleWordToSentenceCase()
        {
            string lowerCase = "newyork";
            string sentenceCase = lowerCase.ConvertToSentanceCase(' ');

            Assert.AreEqual("Newyork", sentenceCase);
        }

        [Test]
        public void CanConvertMultipleWordsToSentenceCase()
        {
            string lowerCase = "AMERICAN SAMOA";
            string sentenceCase = lowerCase.ConvertToSentanceCase(' ');

            Assert.AreEqual("American Samoa", sentenceCase);
        }

        [Test]
        public void CanTrucateNullOrEmpty()
        {
            string txt = null;
            string t = txt.Truncate(10);
            Assert.AreEqual(null, t);

            txt = string.Empty;
            string t2 = txt.Truncate(10);
            Assert.AreEqual(string.Empty, t2);
        }

        [Test]
        public void CanTrucate()
        {
            string txt = "1234567890";
            string t = txt.Truncate(5);
            Assert.AreEqual("12345", t);

            txt = "1234567890";
            string t2 = txt.Truncate(15);
            Assert.AreEqual("1234567890", t2);
        }

        [Test]
        public void ConvertLineSeparators()
        {
            var expected = "Expected text " + StringHelper.UnixLineSeparator +
                           "Line 2 " + StringHelper.UnixLineSeparator +
                           "Line 3";
            var starting = "Expected text " + StringHelper.DosLineSeparator +
                           "Line 2 " + StringHelper.DosLineSeparator +
                           "Line 3";
            Assert.AreEqual(expected,
                StringHelper.ConvertLineSeparators(starting, StringHelper.UnixLineSeparator),
                    "Dud not correctly convert line breaks from DOS to UNIX format");
        }

        [Test]
        public void Times()
        {
            string longWord = "abcde";
            string word = longWord.Times(5);

            Assert.AreEqual("abcdeabcdeabcdeabcdeabcde", word);
        }

        [Test]
        public void IncreaseTo()
        {
            string longWord = "abcde";

            Assert.AreEqual("abcde", longWord.IncreaseTo(5, false));
            Assert.AreEqual("abcde", longWord.IncreaseTo(3, false));
            Assert.AreEqual("abcdeabc", longWord.IncreaseTo(8, false));

            Assert.AreEqual("abcde", longWord.IncreaseTo(5, true));
            Assert.AreEqual("abc", longWord.IncreaseTo(3, true));
            Assert.AreEqual("abcdeabc", longWord.IncreaseTo(8, true));
        }

        [Test]
        public void ToUTF8Bytes()
        {
            string longWord = "abcde";

            Assert.AreEqual("abcde", longWord.ToUTF8Bytes());
        }

        [Test]
        public void ToBool()
        {
            Assert.AreEqual(true, "yes".ToBool());
            Assert.AreEqual(false, "no".ToBool());
            Assert.AreEqual(true, "true".ToBool());
            Assert.AreEqual(false, "false".ToBool());
            Assert.AreEqual(false, "0".ToBool());
            Assert.AreEqual(true, "1".ToBool());
        }

        [Test]
        public void ToInt()
        {
            Assert.AreEqual(100, "$100".ToInt());
            Assert.AreEqual(100, "гд100".ToInt());
            Assert.AreEqual(100, "100".ToInt());
        }

        [Test]
        public void ToDouble()
        {
            Assert.AreEqual(100, "$100".ToDouble());
            Assert.AreEqual(100, "гд100".ToDouble());
            Assert.AreEqual(100.5, "$100.50".ToDouble());
            Assert.AreEqual(100.5, "гд100.50".ToDouble());
            Assert.AreEqual(100.5, "100.5".ToDouble());
            Assert.AreEqual(100, "100".ToDouble());
        }

        [Test]
        public void ToFloat()
        {
            Assert.AreEqual(100, "$100".ToFloat());
            Assert.AreEqual(100, "гд100".ToFloat());
            Assert.AreEqual(100.5, "$100.50".ToFloat());
            Assert.AreEqual(100.5, "гд100.50".ToFloat());
            Assert.AreEqual(100.5, "100.5".ToFloat());
            Assert.AreEqual(100, "100".ToFloat());
        }

        [Test]
        public void ToTime()
        {
            Assert.AreEqual(TimeSpan.Parse("10:00:00"), "10:00:00".ToTime());
        }

        [Test]
        public void ToDateTime()
        {
            Assert.AreEqual(DateTime.Today, "${today}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(-1), "${yesterday}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(1), "${tommorrow}".ToDateTime());
            Assert.AreEqual(DateTime.Today, "${t}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(1), "${t+1}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(-1), "${t-1}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(1), "${today+1}".ToDateTime());
            Assert.AreEqual(DateTime.Today.AddDays(-1), "${today-1}".ToDateTime());
        }

        [Test]
        public void PreFixWith()
        {
            string prefix = "test";
            var list = new List<string>() {
            "list1",
            "list2",
            "list3"
            };
            var list1 = list.PreFixWith(prefix);

            Assert.AreEqual("testlist1", list1[0]);
            Assert.AreEqual("testlist2", list1[1]);
            Assert.AreEqual("testlist3", list1[2]);
        }

        [Test]
        public void NullToEmpty()
        {
            string value1 = null;
            string value2 = "test";
            string value3 = "";
            Assert.AreEqual(string.Empty, value1.NullToEmpty());
            Assert.AreEqual("test", value2.NullToEmpty());
            Assert.AreEqual("", value3.NullToEmpty());
        }

        [Test]
        public void NullToEmptyObject()
        {
            object value1 = null;
            object value2 = "test";
            object value3 = "";
            Assert.AreEqual(string.Empty, value1.NullToEmpty());
            Assert.AreEqual("test", value2.NullToEmpty());
            Assert.AreEqual("", value3.NullToEmpty());
        }

        [Test]
        public void IsStrictIDNumber()
        {
            Assert.AreEqual(true, "430181198704166421".IsStrictIDNumber());
            Assert.AreEqual(false, "43018119870416642x".IsStrictIDNumber());
        }
    }
}