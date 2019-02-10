using System;
using System.Timers;

namespace TaskSolution
{
    // iterates through data and submits information to data aggregator
    public class DataProcessor
    {
        private const int BUFF_SIZE = 80;
        private const int LOG_INTERVAL = 3000;

        private static Timer _timer;

        public void run()
        {
            ConsoleDataPresenter presenter = new ConsoleDataPresenter();
            SampleDataAggregator dtAggregator = new SampleDataAggregator(presenter);

            _timer = new Timer();
            _timer.Interval = LOG_INTERVAL;
            _timer.Elapsed += dtAggregator.OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            using (DevTest.LorumIpsumStream sr = new DevTest.LorumIpsumStream())
            {
                while (sr.CanRead && !Console.KeyAvailable)
                {
                    byte[] buffer = new byte[BUFF_SIZE];
                    int itemsRead = sr.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < itemsRead; i++)
                    {
                        if (buffer[i] != 0)
                            dtAggregator.AddChar(Convert.ToChar(buffer[i]));
                    }
                }
            }

            // print final results
            dtAggregator.LogStatus();
        }
    }
}
