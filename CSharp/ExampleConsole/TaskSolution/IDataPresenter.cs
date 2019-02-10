using System;
using System.Collections.Generic;

namespace TaskSolution
{
    // data presenter for displaying aggregated data (it may print to console, or display to GUI, or log to the database, for example) 
    public interface IDataPresenter
    {
        void SetTotalWords(long value);
        void SetTotalCharacters(long value);
        void SetShortest(IEnumerable<KeyValuePair<String, int>> items);
        void SetLongest(IEnumerable<KeyValuePair<String, int>> items);
        void SetMostFrequent(IEnumerable<KeyValuePair<String, int>> items);
        void SetCharactersFrequency(IEnumerable<KeyValuePair<char, int>> items);
    }
}
