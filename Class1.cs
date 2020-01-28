using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Simbrella.SimKredit.Core.UnitTests
{
    public class Class1
    {
        private readonly Dictionary<int, char> _map = new Dictionary<int, char>();

        private readonly string[] _topFiveWords = { "AN", "THE", "OUT", "A", "AND" };

        public Class1()
        {
            int firstKey = 65;

            for (char i = 'A'; i <= 'Z'; i++)
            {
                _map.Add(firstKey, i);
                firstKey++;
            }
        }

        [Fact]
        public void Example()
        {
            var text = "BAKU".ToCharArray();

            Assert.Equal("QPZJ", new string(text.Select(letter => this.encryptLetter(letter, 15)).ToArray()));

            var text2 = "QPZJ".ToCharArray();

            Assert.Equal("BAKU", new string(text2.Select(x => this.decryptLetter(x, 15)).ToArray()));
        }

        private char encryptLetter(char letter, int shift)
        {
            var position = _map.FirstOrDefault(x => x.Value == letter).Key;

            var newPosition = position + shift;

            if (newPosition > 90)
            {
                newPosition = newPosition - 26;
            }

            return _map[newPosition];
        }

        [Fact]
        public void MainTask()
        {
            var sw = Stopwatch.StartNew();

            var text = File.ReadAllText("D:\\Downloads\\encdata.txt").ToUpperInvariant();

            var splittedText = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var decryptedWords = new List<string>();

            int offset = 0;

            do
            {
                decryptedWords.Clear();

                decryptedWords.AddRange(splittedText.Select(s => this.decryptWord(s, offset)));

                offset++;

            } while (!(decryptedWords.Intersect(_topFiveWords).Count() >= 5));

            var s1 = sw.ElapsedMilliseconds;

            File.Delete("D:\\Downloads\\decrypt.txt");

            File.WriteAllText("D:\\Downloads\\decrypt.txt", string.Join(" ", decryptedWords));

        }

        private string decryptWord(string word, int shift)
        {
            return new string(word.ToCharArray().Select(c => this.decryptLetter(c, shift)).ToArray());
        }

        private char decryptLetter(char letter, int shift)
        {
            var position = _map.FirstOrDefault(x => x.Value == letter).Key;

            var newPosition = position - shift;

            if (newPosition < 65)
            {
                newPosition = newPosition + 26;
            }

            return _map.TryGetValue(newPosition, out var c) ? c : letter;
        }
    }
}
