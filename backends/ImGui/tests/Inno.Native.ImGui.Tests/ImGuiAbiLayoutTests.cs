using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Inno.Native.ImGui.Tests;

public sealed unsafe class ImGuiAbiLayoutTests
{
    [Fact]
    public void GeneratedVectorStoragePreservesNativeOffsetsAndBorrowedData()
    {
        Assert.Equal(2 * sizeof(int) + sizeof(nint), sizeof(ImVectorInt));
        Assert.Equal(2 * sizeof(int), Marshal.OffsetOf<ImVectorInt>(nameof(ImVectorInt.Data)).ToInt32());

        int* values = stackalloc int[3] { 11, 23, 37 };
        ImVectorInt vector = new() { Size = 2, Capacity = 3, Data = values };
        Assert.Equal(2, vector.Size);
        Assert.Equal(3, vector.Capacity);
        Assert.Equal(11, vector.Data[0]);
        values[1] = 41;
        Assert.Equal(41, vector.Data[1]);
    }
}
