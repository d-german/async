### In-Class Programming Assignment: Asynchronous JSON File Processing

**Objective**: Modify the given class to include an asynchronous test method for loading a JSON file and calculating the average of numbers.

**Solution**:
```csharp
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
```
