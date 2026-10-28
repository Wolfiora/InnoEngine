using System;
using System.IO;
using System.Text;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void EncodedScalarBytesMatchTheBinaryWriterContract()
    {
        object?[] values =
        [
            null, false, true, byte.MaxValue, sbyte.MinValue, short.MinValue, ushort.MaxValue,
            int.MinValue, uint.MaxValue, long.MinValue, ulong.MaxValue,
            float.NegativeInfinity, float.PositiveInfinity, float.NaN, -0f, float.Epsilon,
            double.NegativeInfinity, double.PositiveInfinity, double.NaN, -0d, double.Epsilon,
            decimal.MinValue, decimal.MaxValue, new decimal(0, 0, 0, true, 28),
            string.Empty, "English / 中文 / 🧪", Guid.Empty, Guid.Parse("62ce926a-e339-493d-bb74-743ed44ef37a")
        ];
        foreach (object? value in values)
            Assert.Equal(EncodeWithBinaryWriter(value), m_serialization.Encode(writer => writer.Write("value", value, value?.GetType() ?? typeof(string))));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(63)]
    [InlineData(127)]
    [InlineData(128)]
    [InlineData(16_383)]
    [InlineData(16_384)]
    public void EncodedStringsKeepUtf8AndSevenBitLengthSemantics(int length)
    {
        string[] values =
        [
            new string('a', length), new string('中', length),
            new string((char)0xD800, length), "🧪" + new string('a', length)
        ];
        foreach (string value in values)
            Assert.Equal(EncodeWithBinaryWriter(value), m_serialization.Encode(writer => writer.Write("value", value)));
    }

    [Fact]
    public void LargeBinaryEncodingAllocatesOnlyTheOwnedCaptureAndResult()
    {
        var source = new byte[1_000_000];
        source[0] = 73;
        source[^1] = 19;
        for (int index = 0; index < 4; index++)
            _ = m_serialization.Encode(writer => writer.Write("value", source));
        long before = GC.GetAllocatedBytesForCurrentThread();
        byte[] data = [];
        const int repetitions = 16;
        for (int index = 0; index < repetitions; index++)
            data = m_serialization.Encode(writer => writer.Write("value", source));
        long allocated = (GC.GetAllocatedBytesForCurrentThread() - before) / repetitions;
        // Retain the per-call bound while amortizing one-time runtime/catalog admission during the suite.
        Assert.InRange(allocated, source.Length * 2, source.Length * 2 + 8192);
        source[0] = 0;
        byte[] restored = m_serialization.Decode(data, reader => reader.Read<byte[]>("value"));
        Assert.Equal(73, restored[0]);
        Assert.Equal(19, restored[^1]);
    }

    private static byte[] EncodeWithBinaryWriter(object? value)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write("INNO-BINARY-CURRENT");
        writer.Write((byte)20);
        writer.Write(1);
        writer.Write("value");
        switch (value)
        {
            case null:
                writer.Write((byte)0);
                break;
            case bool scalar:
                writer.Write((byte)1);
                writer.Write(scalar);
                break;
            case byte scalar:
                writer.Write((byte)2);
                writer.Write(scalar);
                break;
            case sbyte scalar:
                writer.Write((byte)3);
                writer.Write(scalar);
                break;
            case short scalar:
                writer.Write((byte)4);
                writer.Write(scalar);
                break;
            case ushort scalar:
                writer.Write((byte)5);
                writer.Write(scalar);
                break;
            case int scalar:
                writer.Write((byte)6);
                writer.Write(scalar);
                break;
            case uint scalar:
                writer.Write((byte)7);
                writer.Write(scalar);
                break;
            case long scalar:
                writer.Write((byte)8);
                writer.Write(scalar);
                break;
            case ulong scalar:
                writer.Write((byte)9);
                writer.Write(scalar);
                break;
            case float scalar:
                writer.Write((byte)10);
                writer.Write(scalar);
                break;
            case double scalar:
                writer.Write((byte)11);
                writer.Write(scalar);
                break;
            case decimal scalar:
                writer.Write((byte)12);
                writer.Write(scalar);
                break;
            case string scalar:
                writer.Write((byte)13);
                writer.Write(scalar);
                break;
            case Guid scalar:
                writer.Write((byte)14);
                writer.Write(scalar.ToByteArray());
                break;
            default: throw new InvalidOperationException("The independent byte oracle received an unsupported scalar.");
        }
        writer.Flush();
        return stream.ToArray();
    }
}
