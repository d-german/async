using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace benchmark;

[MemoryDiagnoser]
public class FibonacciBenchmarks
{
    private int[] Values;

    [GlobalSetup]
    public void Setup()
    {
        Values = GetRandomArray(ArraySize);
    }

    // Define a parameter for the benchmark
    //[Params(10)]
    public long N;

    [Params(1000, 10000, 100000)] // Array sizes to test
    public int ArraySize;

    //[Benchmark]
    public long BenchmarkFibonacci()
    {
        // Call the Fibonacci method with the benchmark parameter
        return Fibonacci(N);
    }

    //[Benchmark]
    public async Task<long> BenchmarkFibonacciAsync()
    {
        // Await the asynchronous Fibonacci method
        return await FibonacciAsync(N);
    }

    private static long Fibonacci(long n)
    {
        if (n is 0 or 1) return n;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    private static Task<long> FibonacciAsync(long n)
    {
        // Your existing implementation of FibonacciAsync
        return Task.Factory.StartNew(() => Fibonacci(n),
            CancellationToken.None,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
    }

    [Benchmark]
    public void AvgTest()
    {
        _ = Values.Average();
    }

    [Benchmark]
    public void AvgAsParallelTest()
    {
        _ = Values.AsParallel().Average();
    }

    private int[] GetRandomArray(int size)
    {
        var random = new Random();
        return Enumerable.Range(1, size)
            .Select(x => random.Next(1, size))
            .ToArray();
    }
}

static class Program
{
    static void Main(string[] args)
    {
        _ = BenchmarkRunner.Run<FibonacciBenchmarks>();
    }
}

/*

| Method                  | N  | Mean      | Error     | StdDev    | Gen0   | Gen1   | Gen2   | Allocated |
|------------------------ |--- |----------:|----------:|----------:|-------:|-------:|-------:|----------:|
| BenchmarkFibonacci      | 15 |  2.463 us | 0.0299 us | 0.0265 us |      - |      - |      - |         - |
| BenchmarkFibonacciAsync | 15 | 55.732 us | 1.0726 us | 1.5036 us | 0.7324 | 0.3052 | 0.3052 |     561 B |

| Method                  | N  | Mean     | Error     | StdDev    | Allocated |
|------------------------ |--- |---------:|----------:|----------:|----------:|
| BenchmarkFibonacci      | 30 | 5.277 ms | 0.0508 ms | 0.0476 ms |       2 B |
| BenchmarkFibonacciAsync | 30 | 5.684 ms | 0.0680 ms | 0.0636 ms |     626 B |

| Method                  | N  | Mean    | Error   | StdDev  | Allocated |
|------------------------ |--- |--------:|--------:|--------:|----------:|
| BenchmarkFibonacci      | 50 | 79.83 s | 0.220 s | 0.195 s |    2.3 KB |
| BenchmarkFibonacciAsync | 50 | 52.57 s | 0.125 s | 0.111 s |   2.55 KB |


| Method            | ArraySize | Mean       | Error     | StdDev    | Gen0   | Allocated |
|------------------ |---------- |-----------:|----------:|----------:|-------:|----------:|
| AvgTest           | 1000      |   3.612 us | 0.0715 us | 0.0794 us |      - |      32 B |
| AvgAsParallelTest | 1000      |  15.671 us | 0.1105 us | 0.1034 us | 1.0071 |   13304 B |

| AvgTest           | 10000     |  35.910 us | 0.6812 us | 0.6690 us |      - |      32 B |
| AvgAsParallelTest | 10000     |  27.174 us | 0.3897 us | 0.3645 us | 1.0071 |   13304 B |

| AvgTest           | 100000    | 351.338 us | 6.2697 us | 6.1577 us |      - |      32 B |
| AvgAsParallelTest | 100000    |  70.935 us | 0.8591 us | 0.7616 us | 0.9766 |   13304 B |


*/