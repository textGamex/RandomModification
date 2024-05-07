using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using RandomMod.Core.Game.Parser;

namespace RandomMod.Benchmark;

public class Benchmark
{
    public static void Main()
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}
