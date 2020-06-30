using System;

using BenchmarkDotNet.Running;

namespace WebProxyServiceNS.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
