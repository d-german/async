using System.Diagnostics;

namespace async_tests
{
    public class AsyncTests
    {
        private Stopwatch _stopWatch = null!;

        [SetUp]
        public void Init()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        [TearDown]
        public void Cleanup()
        {
            _stopWatch.Stop();
            Console.WriteLine(_stopWatch.ElapsedMilliseconds);
        }

        // Recursively calculates Fibonacci numbers
        private static long Fibonacci(long n)
        {
            if (n is 0 or 1) return n;

            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        private static Task<long> FibonacciAsync(long n)
        {
            return Task.Run(() => Fibonacci(n));
        }

        private static string GetString(string value)
        {
            return value.ToUpper();
        }

        private static async Task<string> GetStringAsync(string value, int waitTime = 0)
        {
            // Simulating an I/O-bound operation by asynchronously waiting.
            // This is typical in scenarios where the task is waiting for an external resource
            // (like a database query or a web service call) without using the CPU intensively.
            await Task.Delay(waitTime);
           
            Console.WriteLine(value);
           
            return GetString(value);
        }


        [Test]
        public async Task Fibonacci30Test()
        {
            Assert.That(await Task.Run(() => Fibonacci(30)), Is.EqualTo(832040));
        }

        [Test]
        public async Task CapturedValueModificationTest()
        {
            var captured = 5;

            void MultiplyByTwo()
            {
                captured *= 2;
            }

            async Task MultiplyByTwoAsync() //note: not returning anything
            {
                await Task.Run(() => captured *= 2);
            }

            Task MultiplyByTwo2Async() //note: have to return task
            {
                return Task.Run(() => captured *= 2);
            }

            MultiplyByTwo();
            Assert.That(captured, Is.EqualTo(10));

            await MultiplyByTwoAsync();
            Assert.That(captured, Is.EqualTo(20));

            await MultiplyByTwo2Async();
            Assert.That(captured, Is.EqualTo(40));
        }

        [Test]
        public async Task Fibonacci18AsyncTest()
        {
            // code
            Assert.That(2584, Is.EqualTo(await FibonacciAsync(18)));
            // code
        }

        [Test]
        public async Task FibonacciAsyncRangeTest()
        {
            long total = 0;

            for (var i = 10; i < 20; i++)
            {
                total += await FibonacciAsync(i);
            }

            Assert.That(total, Is.EqualTo(10857));
        }

        [Test]
        public void FibonacciSyncRangeTest()
        {
            long total = 0;

            for (var i = 10; i < 20; i++)
            {
                total += Fibonacci(i); //Note this could lockup the current thread (UI)
            }

            Assert.That(total, Is.EqualTo(10857));
        }

        [Test]
        public void Fibonacci20Test()
        {
            Assert.That(6765, Is.EqualTo(Fibonacci(20)));
        }

        [Test]
        public void GetStringAsyncRaceConditionTest() // broken without await
        {
            var task1 = GetStringAsync("time out.", 1000);
            var task2 = GetStringAsync("Hello", 2000);
            var task3 = GetStringAsync("World", 3000);
            _ = Task.WhenAny(task3, task2, task1);
            Assert.Multiple(() =>
            {
                Assert.That(task3.IsCompleted, Is.False);
                Assert.That(task1.IsCompleted, Is.False);
                Assert.That(task2.IsCompleted, Is.False);
            });
        }

        [Test]
        public async Task GetStringAsyncTimeoutTest() //use WhenAny for timeouts
        {
            var task1 = GetStringAsync("Hello", 2000);
            var task2 = GetStringAsync("World", 3000);
            var task3 = GetStringAsync("time out.");
            var res = await Task.WhenAny(task3, task2, task1);
            Assert.Multiple(() =>
            {
                Assert.That(res.Result, Is.EqualTo("TIME OUT."));
                Assert.That(task3.IsCompleted, Is.True);
                Assert.That(task1.IsCompleted, Is.False);
                Assert.That(task2.IsCompleted, Is.False);
            });
        }

        [Test]
        public async Task GetStringAsyncParallelTest() //use WhenAll parallel async
        {
            var task1 = GetStringAsync("Hello", 1000);
            var task2 = GetStringAsync("World", 2000);

            var res = await Task.WhenAll(task2, task1); //ordered by position

            CollectionAssert.AreEqual(new[] {"WORLD", "HELLO"}, res);

            Assert.That($"{task1.Result} {task2.Result}", Is.EqualTo("HELLO WORLD"));
        }

        [Test]
        public async Task GetStringAsyncSerialTest() //serial async
        {
            var s1 = await GetStringAsync("one", 300);
            var s2 = await GetStringAsync(s1 + " two", 1000);
            var s3 = await GetStringAsync(s2 + " three", 200);
            Assert.That(s3, Is.EqualTo("ONE TWO THREE"));
        }

        [Test]
        public async Task GetStringAsyncConcatenationTest()
        {
            Assert.That($"{await GetStringAsync("One")} {await GetStringAsync("Two")} {await GetStringAsync("Three")}", Is.EqualTo("ONE TWO THREE"));
        }

        [Test]
        public async Task TaskCancellationTest() // Canceling tasks
        {
            var numbers = new List<int>();

            async Task DoWork(CancellationToken cancellationToken)
            {
                for (var i = 0; i < 10; i++)
                {
                    numbers.Add(i);
                    await Task.Delay(1000, cancellationToken);
                }
            }

            var cancelSource = new CancellationTokenSource(5000); // This tells it to cancel in 5 seconds

            try
            {
                await DoWork(cancelSource.Token);
            }
            catch (TaskCanceledException)
            {
                CollectionAssert.AreEqual(new[] {0, 1, 2, 3, 4}, numbers);
            }
        }

        [Test]
        public async Task ControlledCancellationTest()
        {
            var cancelSource = new CancellationTokenSource();
            var token = cancelSource.Token;
            var numIterations = 0;

            var task = Task.Run(() =>
            {
                for (var i = 0; i < 100000 && !token.IsCancellationRequested; i++)
                {
                    numIterations++;

                    if (numIterations >= 10)
                    {
                        cancelSource.Cancel();
                    }
                }
            }, token);

            await task;

            Assert.That(numIterations, Is.EqualTo(10));
        }
    }
}
