using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using Xunit;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Annotations;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;
using Inno.Plugins;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;
using Inno.Testing.Scripting;

namespace Inno.Editor.Scripting.Tests;

public abstract class ScriptingPipelineFixture : IDisposable
{
    private protected readonly ScriptingFixture m_fixture = new();

    public void Dispose() => m_fixture.Dispose();

    private protected static void PumpEditorUntil(
        ScriptingFixture fixture,
        EditorInteractionRuntime runtime,
        Func<bool> completed,
        Func<string> status,
        bool focused = true,
        int timeoutMilliseconds = 15_000
    ) {
        long started = Environment.TickCount64;
        int frame = 0;
        while (!completed())
        {
            Assert.True(Environment.TickCount64 - started < timeoutMilliseconds, status());
            fixture.assets.Update();
            using (fixture.editorSession.EnterExecutionScope())
            {
                runtime.Update(new EditorFrame(1f / 60f, frame++ / 60f, focused));
            }
            Thread.Sleep(5);
        }
    }

    private protected static int ReadVersion(Type type) => (int)type.GetProperty("version")!.GetValue(Activator.CreateInstance(type))!;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private protected static WeakReference CaptureActiveType(
        ScriptingFixture fixture,
        string typeName,
        int expectedVersion
    ) {
        Type type = fixture.ResolveActiveType(typeName);
        Assert.Equal(expectedVersion, ReadVersion(type));
        return new WeakReference(type);
    }

    private protected static void CompleteUnloadVerification(ScriptReloadHost reload)
    {
        Exception? failure = null;
        while (!reload.AdvanceUnloadVerification(out failure))
        {
            Thread.Yield();
        }
        Assert.Null(failure);
    }

    private protected static byte[] CreateScriptSourceMeta(
        SerializationRegistry serialization,
        Guid persistentID
    ) => serialization.Serialize(
        new ScriptingAssetSourceMeta
        {
            persistentId = persistentID,
            sourceKind = (int)AssetSourceKind.File,
            importerId = "inno.editor.csharp-script"
        }
    );

    private protected static void WritePluginPackage(
        string projectRoot,
        string fileName,
        SerializationRegistry serialization,
        PluginManifest manifest,
        IReadOnlyDictionary<string, byte[]> entries
    ) {
        string path = Path.Combine(projectRoot, "Plugins", fileName);
        using FileStream stream = File.Create(path);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        WritePluginPackageEntry(archive, "Plugin.inno", serialization.Serialize(manifest));
        foreach ((string entryPath, byte[] bytes)in entries)
        {
            WritePluginPackageEntry(archive, entryPath, bytes);
        }
    }

    private protected static void WritePluginPackageEntry(
        ZipArchive archive,
        string path,
        byte[] bytes
    ) {
        ZipArchiveEntry entry = archive.CreateEntry(path, CompressionLevel.Optimal);
        if (path.EndsWith("/", StringComparison.Ordinal))
        {
            return;
        }
        using Stream output = entry.Open();
        output.Write(bytes);
    }

    private protected static string FormatDiagnostics(ScriptCompilationResult result)
        => string.Join(
            Environment.NewLine,
            result.diagnostics.Select(static diagnostic => $"{diagnostic.id}: {diagnostic.message}")
        );
}
