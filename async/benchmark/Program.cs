using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace benchmark;
[MemoryDiagnoser]
public class FibonacciBenchmarks
{
    // Define a parameter for the benchmark
    [Params(40)] 
    public long N;

    [Benchmark]
    public long BenchmarkFibonacci()
    {
        // Call the Fibonacci method with the benchmark parameter
        return Fibonacci(N);
    }
    
    [Benchmark]
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
}

static class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<FibonacciBenchmarks>();
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



*/