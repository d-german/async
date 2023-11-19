### In-Class Assignment: Converting a Class for BenchmarkDotNet Usage

#### Objective:
Convert the provided `DictionaryBenchmark` class for use with BenchmarkDotNet. The primary task is to transform the existing constructor into a global setup method. Additionally, you will apply BenchmarkDotNet attributes to the class and methods, and modify the `Main` method to run the benchmarks.

#### Instructions:
1. **Install BenchmarkDotNet**:
   - If not already installed, add the BenchmarkDotNet NuGet package to your project.

2. **Modify the Class to Use BenchmarkDotNet**:
   - Add the necessary using directives for BenchmarkDotNet at the top of your file.

3. **Transform Constructor to Global Setup Method**:
   - Convert the existing constructor `public DictionaryBenchmark()` into a method with the `[GlobalSetup]` attribute.   

4. **Add Benchmark Attributes**:
   - Apply the `[Benchmark]` attribute to each method that should be benchmarked (`AddToDictionary`, `AddToSortedDictionary`, `AddToImmutableDictionary`, `LookupInDictionary`, `LookupInSortedDictionary`, `LookupInImmutableDictionary`).

5. **Memory Usage Tracking**:
   - Add the `[MemoryDiagnoser]` attribute to the `DictionaryBenchmark` class to enable memory allocation tracking.

6. **Enable Benchmarking in Main Method**:
   - Uncomment the line `BenchmarkRunner.Run<DictionaryBenchmark>();` in the `Main` method to run the benchmarks.

7. **Run the Benchmarks**:
   - Execute the program, and BenchmarkDotNet will perform the benchmarks as per the defined parameters and methods.

8. **Analyze the Results**:
   - Once the benchmarks are complete, analyze the output for performance insights.