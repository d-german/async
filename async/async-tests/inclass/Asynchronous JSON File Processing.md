### In-Class Programming Assignment: Asynchronous JSON File Processing

**Objective**: Modify the given class to include an asynchronous test method for loading a JSON file and calculating the average of numbers.

**Provided Class**:
```csharp
[TestFixture]
public class JsonFileTest
{
    // JSON Content: { "numbers": [1, 2, 3, 4, 5] }

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
}
```

**Assignment Steps**:

1. **Copy the Existing Test Method**: Begin by copying the entire `LoadJsonFileAndCalculateAverageSynchronously` method.

2. **Create a New Test Method**:
    - Paste the copied method into the `JsonFileTest` class.
    - Rename this new method to `LoadJsonFileAndCalculateAverageAsynchronously`.

3. **Modify the Method Signature**:
    - Change the return type of the new method from `void` to `async Task`.

4. **Update File Reading to Asynchronous**:
    - Find the line with `ReadAllText(JsonFilePath)`.
    - Replace `ReadAllText` with `ReadAllTextAsync`.
    - Add the `await` keyword before `ReadAllTextAsync`.

5. **Keep the Remaining Logic Same**:
    - Retain the parsing and calculation logic exactly as it is in the synchronous method.
    - Ensure the calculation of the average and the assertion (`Assert.That`) remains unchanged.

6. **Finalize the Method**:
    - Verify that the new asynchronous method is correctly structured and follows C# asynchronous programming conventions.