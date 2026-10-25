using System.Numerics;
using System;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Native.ImGui.Tests;

[Collection("ImGuiContext")]
public sealed unsafe class ImGuiInitTests
{
    private readonly ITestOutputHelper output;

    public ImGuiInitTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void CreateAndDestroyContext_ShouldSucceed()
    {
        var context = ImGui.Raw.CreateContext(null);
        Assert.True(context != null);

        var version = Marshal.PtrToStringUTF8((nint)ImGui.Raw.GetVersion());
        output.WriteLine($"ImGui.GetVersion: {version}");
        Assert.False(string.IsNullOrWhiteSpace(version));

        ImGui.Raw.DestroyContext(context);
    }

    [Fact]
    public void TextDisabled_UsesTheLoadedCimguiSymbol()
    {
        var context = ImGui.Raw.CreateContext(null);
        try
        {
            ImGuiIO* io = ImGui.Raw.GetIO();
            io->DisplaySize = new ImVec2C { X = 320f, Y = 200f };
            io->DeltaTime = 1f / 60f;
            io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasTextures;
            io->Fonts->RendererHasTextures = 1;

            ImGui.NewFrame();
            bool open = true;
            _ = ImGui.Begin("Native Text Test", ref open);
            ReadOnlySpan<byte> text = "Plugin status\0"u8;
            fixed (byte* bytes = text)
                ImGui.Raw.TextDisabled((sbyte*)bytes);
            ImGui.End();
            ImGui.Render();
        }
        finally
        {
            ImGui.Raw.DestroyContext(context);
        }
    }

}
