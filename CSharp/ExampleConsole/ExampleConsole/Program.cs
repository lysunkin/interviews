using System;
using TaskSolution;

/*
 * Nota bene:
 * 1) implemented as a console application, i.e. less support code and easier to see the main implementation
 * 2) execution can be stopped by pressing any key
 * 3) information is logged every 3 seconds - arbitrary value, more frequest logging will distract attention from monitoring the data
 * 4) I'm still unsure what to do if the input stream is too large: should I just stop or print current stats and reset all counters?
 */

namespace ExampleConsole
{
    // entry point
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                new DataProcessor().run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return 1;
            }

            return 0;
        }
    }
}
