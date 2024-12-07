namespace Tests;

using Payload;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void TestGreet()
    {
        var greeter = new Hello();
        Assert.That(!String.IsNullOrEmpty(greeter.Greet()), "empty greeting");
    }

    [Test]
    public void TestWeather()
    {
        var w = new WeatherProvider();
        var actual = w.GetForecasts();
        Assert.That(actual != null, "data is empty");
    }
}
