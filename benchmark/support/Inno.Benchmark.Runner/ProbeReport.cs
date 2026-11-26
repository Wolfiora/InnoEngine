using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Inno.Benchmark.Runner;

public sealed class ProbeReport
{
    private readonly StringBuilder m_rows = new("| Probe | Value |\n|---|---|\n");

    public void Row(
        string name,
        double value
    ) {
        string text = $"| {name} | {value:0.###} |";
        m_rows.AppendLine(text);
        Console.Error.WriteLine(text);
    }

    public static double Time(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    public static double Median(Action action)
    {
        var samples = new double[9];
        for (int index = 0; index < samples.Length; index++)
        {
            samples[index] = Time(action);
        }
        Array.Sort(samples);
        return samples[samples.Length / 2];
    }

    public static long Settle()
    {
        for (int index = 0; index < 3; index++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        return GC.GetTotalMemory(forceFullCollection: true);
    }

    internal void Write(string path)
    {
        File.WriteAllText(path, m_rows.ToString());
        Console.Write(m_rows.ToString());
    }
}
