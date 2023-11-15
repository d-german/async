using System.Collections.Immutable;

public class DictionaryBenchmark
{
   
    public int NumberOfElements { get; set; }

    private List<int> _keysToLookup;
    private Random _random;

    // Dictionaries for benchmarking
    private Dictionary<int, string> _dictionary;
    private SortedDictionary<int, string> _sortedDictionary;
    private ImmutableDictionary<int, string> _immutableDictionary;

   
    public  DictionaryBenchmark()
    {
        _random = new Random();
        _keysToLookup = new List<int>();
        _dictionary = new Dictionary<int, string>();
        _sortedDictionary = new SortedDictionary<int, string>();
        var immutableBuilder = ImmutableDictionary.CreateBuilder<int, string>();

        for (int i = 0; i < NumberOfElements; i++)
        {
            int key = _random.Next(0, NumberOfElements);
            var value = "Value" + key;

            _dictionary.TryAdd(key, value);
            _sortedDictionary.TryAdd(key, value);
            immutableBuilder.TryAdd(key, value);

            if (i % (NumberOfElements / 10) == 0) // Add 10 keys to the lookup list
            {
                _keysToLookup.Add(key);
            }
        }

        _immutableDictionary = immutableBuilder.ToImmutable();
    }

 
    public void AddToDictionary()
    {
        var tempDict = new Dictionary<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            tempDict.Add(i, "Value" + i);
        }
    }

  
    public void AddToSortedDictionary()
    {
        var tempDict = new SortedDictionary<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            tempDict.Add(i, "Value" + i);
        }
    }

   
    public void AddToImmutableDictionary()
    {
        var builder = ImmutableDictionary.CreateBuilder<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            builder.Add(i, "Value" + i);
        }
        var immutableDict = builder.ToImmutable();
    }

   
    public string LookupInDictionary()
    {
        string result = null;
        foreach (var key in _keysToLookup)
        {
            result = _dictionary[key];
        }
        return result;
    }

    
    public string LookupInSortedDictionary()
    {
        string result = null;
        foreach (var key in _keysToLookup)
        {
            result = _sortedDictionary[key];
        }
        return result;
    }

    
    public string LookupInImmutableDictionary()
    {
        string result = null;
        foreach (var key in _keysToLookup)
        {
            result = _immutableDictionary[key];
        }

        return result;
    }

}

public static class Program
{
    public static void Main(string[] args)
    {
        //BenchmarkRunner.Run<DictionaryBenchmark>();
    }
}
/*
| Method                      | NumberOfElements | Mean             | Error          | StdDev         | Gen0      | Gen1      | Gen2     | Allocated  |
|---------------------------- |----------------- |-----------------:|---------------:|---------------:|----------:|----------:|---------:|-----------:|
| AddToDictionary             | 1000             |     47,772.95 ns |     875.357 ns |     818.810 ns |   13.2446 |    4.3945 |        - |   173896 B |
| AddToSortedDictionary       | 1000             |    186,053.92 ns |   3,523.558 ns |   3,460.605 ns |    9.7656 |    2.4414 |        - |   127792 B |
| AddToImmutableDictionary    | 1000             |    224,978.66 ns |   1,178.526 ns |   1,102.394 ns |   10.2539 |    2.4414 |        - |   135776 B |
| LookupInDictionary          | 1000             |         62.53 ns |       0.223 ns |       0.197 ns |         - |         - |        - |          - |
| LookupInSortedDictionary    | 1000             |        523.78 ns |      10.486 ns |      10.298 ns |         - |         - |        - |          - |
| LookupInImmutableDictionary | 1000             |        162.71 ns |       1.528 ns |       1.429 ns |         - |         - |        - |          - |

| AddToDictionary             | 10000            |  1,039,281.67 ns |   6,910.863 ns |   6,464.426 ns |  221.6797 |  221.6797 | 221.6797 |  1661763 B |
| AddToSortedDictionary       | 10000            |  2,209,370.43 ns |  19,397.384 ns |  18,144.325 ns |   97.6563 |   33.2031 |        - |  1279793 B |
| AddToImmutableDictionary    | 10000            |  4,093,571.17 ns |  11,624.780 ns |  10,873.826 ns |  101.5625 |   39.0625 |        - |  1359778 B |
| LookupInDictionary          | 10000            |         31.90 ns |       0.337 ns |       0.281 ns |         - |         - |        - |          - |
| LookupInSortedDictionary    | 10000            |        891.89 ns |      11.515 ns |      10.771 ns |         - |         - |        - |          - |
| LookupInImmutableDictionary | 10000            |        296.56 ns |       4.921 ns |       4.603 ns |         - |         - |        - |          - |

| AddToDictionary             | 100000           | 19,772,558.06 ns | 393,925.876 ns | 525,879.730 ns | 1281.2500 | 1250.0000 | 687.5000 | 16372274 B |
| AddToSortedDictionary       | 100000           | 38,410,382.08 ns | 488,553.358 ns | 456,993.115 ns | 1031.2500 |  500.0000 |        - | 13519809 B |
| AddToImmutableDictionary    | 100000           | 37,373,383.33 ns | 443,013.880 ns | 345,876.097 ns | 1071.4286 |  500.0000 |        - | 14319815 B |
| LookupInDictionary          | 100000           |         31.08 ns |       0.279 ns |       0.247 ns |         - |         - |        - |          - |
| LookupInSortedDictionary    | 100000           |        629.23 ns |       5.480 ns |       5.126 ns |         - |         - |        - |          - |
| LookupInImmutableDictionary | 100000           |        365.26 ns |       6.843 ns |       6.401 ns |         - |         - |        - |          - |



*/