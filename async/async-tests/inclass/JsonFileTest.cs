using static System.IO.File;
using static System.Text.Json.JsonDocument;

namespace async_tests.inclass;

[TestFixture]
public class JsonFileTest
{
    // {
    //     "numbers": [1, 2, 3, 4, 5]
    // }

    private const string NumbersArrayKey = "numbers";
    private const string JsonFilePath = "inclass/data.json";

    [Test]
    public void LoadJsonFileAndCalculateAverageSynchronously()
    {
        var numbers = Parse(ReadAllText(JsonFilePath))
            .RootElement.GetProperty(NumbersArrayKey)
            .EnumerateArray()
            .Select(element => element.GetInt32())
            .ToArray();

        Assert.That(numbers.Average(), Is.EqualTo(3));
    }

    [Test]
    public async Task LoadJsonFileAndCalculateAverageAsynchronously()
    {
        var numbers = Parse(await ReadAllTextAsync(JsonFilePath))
            .RootElement.GetProperty(NumbersArrayKey)
            .EnumerateArray()
            .Select(element => element.GetInt32())
            .ToArray();

        Assert.That(numbers.Average(), Is.EqualTo(3));
    }
}