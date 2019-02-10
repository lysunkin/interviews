using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskSolution
{
    public class SampleDataAggregator : DataAggregatorBase
    {
        private IDataPresenter _presenter;

        public SampleDataAggregator(IDataPresenter presenter)
        {
            this._presenter = presenter;
        }

        public override void LogStatus()
        {
            _presenter.SetTotalWords(_numWords);
            _presenter.SetTotalCharacters(_numChars);

            IEnumerable<KeyValuePair<String, int>> first5 = _words.Take(5);
            _presenter.SetShortest(first5);

            IEnumerable<KeyValuePair<String, int>> last5 = _words.Reverse().Take(5);
            _presenter.SetLongest(last5);

            IEnumerable<KeyValuePair<String, int>> mostFrequent = _words.OrderByDescending(t => t.Value).Take(10);
            _presenter.SetMostFrequent(mostFrequent);

            IEnumerable<KeyValuePair<char, int>> revChars = _characters.OrderByDescending(t => t.Value);
            _presenter.SetCharactersFrequency(revChars);
        }
    }
}
