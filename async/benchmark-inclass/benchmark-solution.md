```csharp
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
[MemoryDiagnoser]
public class DictionaryBenchmark
{
    [Params(1000, 10000, 100000)]
    public int NumberOfElements { get; set; }

    private List<int> _keysToLookup;
    private Random _random;
    private Dictionary<int, string> _dictionary;
    private SortedDictionary<int, string> _sortedDictionary;
    private ImmutableDictionary<int, string> _immutableDictionary;

    [GlobalSetup]
    public void Setup()
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

            if (i % (NumberOfElements / 10) == 0)
            {
                _keysToLookup.Add(key);
            }
        }

        _immutableDictionary = immutableBuilder.ToImmutable();
    }

    [Benchmark]
    public void AddToDictionary()
    {
        var tempDict = new Dictionary<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            tempDict.Add(i, "Value" + i);
        }
    }

    [Benchmark]
    public void AddToSortedDictionary()
    {
        var tempDict = new SortedDictionary<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            tempDict.Add(i, "Value" + i);
        }
    }

    [Benchmark]
    public void AddToImmutableDictionary()
    {
        var builder = ImmutableDictionary.CreateBuilder<int, string>();
        for (int i = 0; i < NumberOfElements; i++)
        {
            builder.Add(i, "Value" + i);
        }
        var immutableDict = builder.ToImmutable();
    }

    [Benchmark]
    public string LookupInDictionary()
    {
        string result = null;
        foreach (var key in _keysToLookup)
        {
            result = _dictionary[key];
        }
        return result;
    }

    [Benchmark]
    public string LookupInSortedDictionary()
    {
        string result = null;
        foreach (var key in _keysToLookup)
        {
            result = _sortedDictionary[key];
        }
        return result;
    }

    [Benchmark]
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
        BenchmarkRunner.Run<DictionaryBenchmark>();
    }
}

```