using System;
using System.Collections.Generic;
using System.Timers;

namespace TaskSolution
{
    // aggregates data and manages the data integrity
    abstract public class DataAggregatorBase
    {
        protected SortedDictionary<string, int> _words = new SortedDictionary<string, int>(new ByLengthComparer());
        protected SortedDictionary<char, int> _characters = new SortedDictionary<char, int>();
        protected List<char> _currentWord = new List<char>();
        protected long _numWords = 0;
        protected long _numChars = 0;

        readonly object thisLock = new object();

        private void AddWord(String word)
        {
            if (word == null || string.IsNullOrEmpty(word))
                return;

            _numWords++;

            lock (thisLock)
            {
                word = word.ToLower();
                if (_words.ContainsKey(word))
                    _words[word] = _words[word] + 1;
                else
                    _words[word] = 1;
            }
        }

        public void AddChar(char ch)
        {
            // TODO: I'm still unsure what to do if the input stream is too large:
            // should I just stop or print current stats and reset all counters?
            if (_numChars == long.MaxValue)
                throw new Exception("the number of characters exceeds the limit");

            _numChars++;

            lock (thisLock)
            {
                if (_characters.ContainsKey(ch))
                    _characters[ch] = _characters[ch] + 1;
                else
                    _characters[ch] = 1;

                if (Char.IsSeparator(ch) || Char.IsPunctuation(ch))
                {
                    AddWord(string.Concat(_currentWord));
                    _currentWord = new List<char>();
                }
                else
                    _currentWord.Add(ch);
            }
        }

        abstract public void LogStatus();

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            lock (thisLock)
            {
                LogStatus();
            }
        }
    }
}
