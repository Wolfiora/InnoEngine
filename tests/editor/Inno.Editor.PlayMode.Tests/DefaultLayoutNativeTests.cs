using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Extensibility.Catalogs;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Native.ImGui;
using Xunit;
using NativeGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

public sealed unsafe class DefaultLayoutNativeTests
{
    [Theory]
    [InlineData(1280f, 720f)]
    [InlineData(800f, 600f)]
    [Trait("Acceptance", "Visual")]
    public void StandardDockTreeLoadsAndResizesThroughThePresentationRuntime(
        float width,
        float height
    ) {
        using var host = new NativePanelHost();
        using var modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(typeof(DefaultLayoutNativeTests).Assembly) });
        using var types = new TypeCatalog(modules, new EmptyTypeSource());
        using var logs = new LogRouter();
        var context = new EditorContext(host.runtime.context.projectDirectory, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"));
        using var runtime = new ImGuiEditorRuntime(context, types, logs, []);
        ImGuiIO* io = NativeGui.Raw.GetIO();
        io->ConfigFlags |= (int)ImGuiConfigFlags.DockingEnable;
        io->DisplaySize = Unsafe.BitCast<Vector2, ImVec2C>(new(1600f, 900f));
        byte[] layout = Encoding.UTF8.GetBytes(File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "DefaultEditorLayout.ini")) + '\0');
        fixed (byte* pointer = layout)
            NativeGui.Raw.LoadIniSettingsFromMemory((sbyte*)pointer, (ulong)(layout.Length - 1));
        runtime.Start();
        string[] titles = ["Scene", "Game", "File", "Hierarchy", "Inspector", "Shader Editor", "Console", "Stats"];
        var rectangles = new Dictionary<string, (Vector2 position, Vector2 size, uint dock)>();
        for (int frame = 0; frame < 12; frame++)
        {
            if (frame == 6)
                io->DisplaySize = Unsafe.BitCast<Vector2, ImVec2C>(new(width, height));
            host.Frame(() =>
            {
                runtime.Draw();
                foreach (string title in titles)
                {
                    bool open = true;
                    _ = NativeGui.Begin(title, ref open, 0);
                    try
                    {
                        rectangles[title] = (NativeGui.GetWindowPos(), NativeGui.GetWindowSize(), NativeGui.GetWindowDockID());
                    }
                    finally
                    {
                        NativeGui.End();
                    }
                }
            });
        }
        foreach (var rectangle in rectangles.Values)
        {
            Assert.NotEqual(0u, rectangle.dock);
            Assert.True(rectangle.size.X > 0f && rectangle.size.Y > 0f);
            Assert.InRange(rectangle.position.X, -1f, width);
            Assert.InRange(rectangle.position.Y, -1f, height);
            Assert.InRange(rectangle.position.X + rectangle.size.X, 0f, width + 1f);
            Assert.InRange(rectangle.position.Y + rectangle.size.Y, 0f, height + 1f);
        }
        Assert.Equal(rectangles["Inspector"].dock, rectangles["Shader Editor"].dock);
        Assert.Equal(rectangles["Console"].dock, rectangles["Stats"].dock);
        Assert.True(rectangles["Scene"].position.X < rectangles["Game"].position.X);
        Assert.True(rectangles["File"].position.Y > rectangles["Scene"].position.Y);
        Assert.True(rectangles["Hierarchy"].position.X > rectangles["File"].position.X);
        Assert.True(rectangles["Inspector"].position.X > rectangles["Hierarchy"].position.X);
        Assert.True(rectangles["Console"].position.Y > rectangles["Inspector"].position.Y);
        Assert.InRange(rectangles["File"].size.X / width, 0.43f, 0.49f);
        Assert.InRange(rectangles["Hierarchy"].size.X / width, 0.25f, 0.30f);
        Assert.InRange(rectangles["Inspector"].size.X / width, 0.25f, 0.30f);
    }

    private sealed class EmptyTypeSource : ITypeCatalogSource
    {
        public IReadOnlyList<Type> GetTypes(Assembly assembly) => [];
        public TypeCatalogMetadata GetMetadata(Type type) => throw new NotSupportedException();
        public Type? ConstructGenericType(
            Type definition,
            IReadOnlyList<Type> arguments
        ) => throw new NotSupportedException();
        public bool CanCreateInstance(Type type) => false;
        public object CreateInstance(Type type) => throw new NotSupportedException();
    }
}
