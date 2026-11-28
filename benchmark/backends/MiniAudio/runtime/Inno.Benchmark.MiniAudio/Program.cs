using System;
using System.Collections.Generic;
using System.IO;
using Inno.Benchmark.Runner;
using Inno.Native.LibraryLoading;

namespace Inno.Benchmark.MiniAudio;

internal static class Program
{
    private static int Main(string[] args)
        => BenchmarkRunner.Run(typeof(Program).Assembly, args, new Dictionary<string, Action<ProbeReport>>
        {
            ["processor-buffer"] = ProcessorBufferProbe.Measure
        }, Available);

    private static bool Available()
    {
        try
        {
            _ = NativeDllLoader.FindNativeLibrary("miniaudio-release");
            return true;
        }
        catch (IOException error)
        {
            Console.Error.WriteLine("Unavailable: " + error.Message);
            return false;
        }
    }
}
