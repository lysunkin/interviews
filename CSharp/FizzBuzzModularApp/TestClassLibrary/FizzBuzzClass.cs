namespace TestClassLibrary
{
    public class FizzBuzzClass
    {
        private string _fizz;
        private string _buzz;

        public FizzBuzzClass() : this("Fizz", "Buzz")
        {

        }

        public FizzBuzzClass(string fizz, string buzz)
        {
            _fizz = fizz;
            _buzz = buzz;
        }

        public string GetFizzBuzz(int i)
        {
            if (i % 15 == 0)
            {
                return _fizz+_buzz;
            }
            else if (i % 3 == 0)
            {
                return _fizz;
            }
            else if (i % 5 == 0)
            {
                return _buzz;
            }
            else
            {
                return i.ToString();
            }
        }
    }
}
