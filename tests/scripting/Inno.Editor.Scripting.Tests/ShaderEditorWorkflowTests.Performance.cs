using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text.Json;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Native.ImGui;
using Inno.Rendering.Shaders;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ShaderEditorWorkflowTests
{
    [Fact]
    [Trait("Acceptance", "Performance")]
    public unsafe void TwoThousandShaderNodesProduceRepeatableFrameMeasurements()
    {
        NativeGui.Raw.GetIO()->BackendFlags |= (int)ImGuiBackendFlags.RendererHasVtxOffset;
        string source = Path.Combine(m_root, "Assets");
        string? inputRoot = Environment.GetEnvironmentVariable("INNO_UI_PERFORMANCE_INPUT_ROOT");
        string? input = inputRoot is null ? null : Path.Combine(Path.GetFullPath(inputRoot), "shader-graph");
        const string name = "Performance.ishader";
        if (input is not null && Directory.Exists(input))
            CopyPerformanceInput(input, source);
        else
        {
            GraphDocument graph = ShaderGraphTemplates.CreateRaster(m_serialization, AssetSerializationContext.Create(m_assets));
            int remaining = 2_000 - graph.nodes.Count;
            for (int index = 0; index < remaining; index++)
            {
                bool constant = index % 2 == 0;
                var node = new GraphNodeRecord(new("performance.node." + index.ToString("D4")),
                    constant ? "inno.shader.constant" : "inno.shader.construct")
                {
                    position = new(index % 20 * 310f, index / 20 * 180f)
                };
                if (!constant)
                    node.SetValue("type", ShaderGraphDocument.Encode("float3", m_serialization, SerializationContext.empty));
                graph.AddNode(node);
                if (!constant)
                    graph.AddEdge(new(new("performance.edge." + index.ToString("D4")),
                        new(new("performance.node." + (index - 1).ToString("D4")), new("value")),
                        new(node.id, new("component.0"))));
            }
            File.WriteAllBytes(Path.Combine(source, name), GraphDocumentCodec.Encode(graph, m_serialization));
        }
        Assert.True(m_assets.Import(AssetPath.Project(name)));
        Assert.True(m_assets.TryGetFileSystemEntry(AssetPath.Project(name), out AssetFileEntry entry));
        if (input is not null && !Directory.Exists(input))
            CopyPerformanceInput(source, input);
        byte[] original = SHA256.HashData(File.ReadAllBytes(Path.Combine(source, name)));
        SelectAndDraw(entry);
        var controller = Controller(entry);
        Assert.Equal(2_000, controller.document.nodes.Count);
        Assert.True(controller.document.edges.Count >= 900);
        for (int warmup = 0; warmup < 5; warmup++)
            DrawPerformanceCanvas();
        string? requested = Environment.GetEnvironmentVariable("INNO_UI_PERFORMANCE_REPETITIONS");
        int repetitions = requested is null ? 5 : int.Parse(requested, System.Globalization.CultureInfo.InvariantCulture);
        Assert.InRange(repetitions, 1, 5);
        using Process process = Process.GetCurrentProcess();
        var samples = new List<object>();
        for (int repetition = 0; repetition < repetitions; repetition++)
        {
            var milliseconds = new double[10];
            var vertices = new int[10];
            var indices = new int[10];
            long bytesBefore = GC.GetAllocatedBytesForCurrentThread();
            long totalBytesBefore = GC.GetTotalAllocatedBytes(precise: true);
            TimeSpan cpuBefore = process.TotalProcessorTime;
            int gen0 = GC.CollectionCount(0);
            int gen1 = GC.CollectionCount(1);
            int gen2 = GC.CollectionCount(2);
            for (int frame = 0; frame < milliseconds.Length; frame++)
            {
                long start = Stopwatch.GetTimestamp();
                DrawPerformanceCanvas();
                milliseconds[frame] = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
                vertices[frame] = NativeGui.Raw.GetDrawData()->TotalVtxCount;
                indices[frame] = NativeGui.Raw.GetDrawData()->TotalIdxCount;
            }
            long allocated = GC.GetAllocatedBytesForCurrentThread() - bytesBefore;
            double[] sorted = milliseconds.Order().ToArray();
            samples.Add(new
            {
                repetition, milliseconds, vertices, indices, median = (sorted[4] + sorted[5]) / 2,
                p95 = sorted[9], allocatedBytes = allocated,
                totalAllocatedBytes = GC.GetTotalAllocatedBytes(precise: true) - totalBytesBefore,
                cpuMilliseconds = (process.TotalProcessorTime - cpuBefore).TotalMilliseconds,
                gen0Collections = GC.CollectionCount(0) - gen0,
                gen1Collections = GC.CollectionCount(1) - gen1,
                gen2Collections = GC.CollectionCount(2) - gen2
            });
        }
        m_performanceOutput.WriteLine(JsonSerializer.Serialize(new
        {
            workload = "2000-shader-nodes", nodeCount = controller.document.nodes.Count,
            edgeCount = controller.document.edges.Count, configuration = "Debug", samples,
            framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            viewport = "1200x800", warmupFrames = 5, measuredFramesPerRepetition = 10,
            sharedInput = input, consumer = "native-command-offset-validation", supportsVertexOffsets = true
        }));
        Assert.Equal(original, SHA256.HashData(File.ReadAllBytes(Path.Combine(source, name))));
    }

    private unsafe void DrawPerformanceCanvas()
    {
        Draw(new Vector2(1200f, 800f));
        ImDrawData* data = NativeGui.Raw.GetDrawData();
        for (int listIndex = 0; listIndex < data->CmdListsCount; listIndex++)
        {
            ImDrawList* list = data->CmdLists.Data[listIndex];
            for (int commandIndex = 0; commandIndex < list->CmdBuffer.Size; commandIndex++)
            {
                ImDrawCmd command = list->CmdBuffer.Data[commandIndex];
                ulong end = (ulong)command.IdxOffset + command.ElemCount;
                if (end > (ulong)list->IdxBuffer.Size)
                    throw new InvalidOperationException("The shader canvas exceeded its native index buffer.");
                for (ulong index = command.IdxOffset; index < end; index++)
                    if ((ulong)command.VtxOffset + list->IdxBuffer.Data[index] >= (ulong)list->VtxBuffer.Size)
                        throw new InvalidOperationException("The shader canvas exceeded its native vertex buffer.");
            }
        }
    }

    private static void CopyPerformanceInput(
        string source,
        string destination
    ) {
        Directory.CreateDirectory(destination);
        foreach (string directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, directory)));
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
            File.Copy(file, Path.Combine(destination, Path.GetRelativePath(source, file)), overwrite: false);
    }
}
