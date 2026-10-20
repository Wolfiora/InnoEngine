using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Inno.Rendering;

internal sealed class RecordedRenderCommandBuffer : IDisposable
{
    private const int C_INITIAL_BYTE_CAPACITY = 256;
    private const int C_INITIAL_REFERENCE_CAPACITY = 16;

    private byte[] m_bytes = [];
    private object?[] m_references = [];
    private int m_length;
    private int m_referenceCount;
    private bool m_disposed;

    /// <summary>
    /// Returns this recording's borrowed buffers and releases every recorded reference.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        m_disposed = true;
        byte[] bytes = m_bytes;
        object?[] references = m_references;
        m_bytes = [];
        m_references = [];
        m_length = 0;
        m_referenceCount = 0;
        if (bytes.Length != 0)
            ArrayPool<byte>.Shared.Return(bytes);
        if (references.Length != 0)
            ArrayPool<object?>.Shared.Return(references, clearArray: true);
    }

    internal Reader GetReader()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        return new Reader(m_bytes.AsSpan(0, m_length), m_references.AsSpan(0, m_referenceCount));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Write<T>(T value) where T : unmanaged
    {
        int size = Unsafe.SizeOf<T>();
        EnsureCapacity(size);
        MemoryMarshal.Write(m_bytes.AsSpan(m_length, size), in value);
        m_length += size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void WriteInstruction<T>(
        byte operation,
        T value
    ) where T : unmanaged {
        int size = Unsafe.SizeOf<T>();
        EnsureCapacity(checked(size + 1));
        m_bytes[m_length] = operation;
        MemoryMarshal.Write(m_bytes.AsSpan(m_length + 1, size), in value);
        m_length += size + 1;
    }

    internal void WriteReference(object? value)
    {
        if (m_referenceCount == m_references.Length)
        {
            object?[] next = ArrayPool<object?>.Shared.Rent(
                m_references.Length == 0 ? C_INITIAL_REFERENCE_CAPACITY : checked(m_references.Length * 2));
            m_references.AsSpan(0, m_referenceCount).CopyTo(next);
            if (m_references.Length != 0)
                ArrayPool<object?>.Shared.Return(m_references, clearArray: true);
            m_references = next;
        }
        Write(m_referenceCount);
        m_references[m_referenceCount++] = value;
    }

    internal void WriteSnapshot(ReadOnlySpan<byte> value)
    {
        Write(value.Length);
        // Four-byte alignment also permits replaying matrix snapshots as float spans.
        int start = checked(m_length + 3) & ~3;
        EnsureCapacity(checked(start - m_length + value.Length));
        value.CopyTo(m_bytes.AsSpan(start));
        m_length = checked(start + value.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int additionalBytes)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        int required = checked(m_length + additionalBytes);
        if (required > m_bytes.Length)
            Grow(required);
    }

    private void Grow(int required)
    {
        int capacity = m_bytes.Length == 0 ? C_INITIAL_BYTE_CAPACITY : checked(m_bytes.Length * 2);
        byte[] next = ArrayPool<byte>.Shared.Rent(Math.Max(required, capacity));
        m_bytes.AsSpan(0, m_length).CopyTo(next);
        if (m_bytes.Length != 0)
            ArrayPool<byte>.Shared.Return(m_bytes);
        m_bytes = next;
    }

    internal ref struct Reader
    {
        private readonly ReadOnlySpan<byte> m_bytes;
        private readonly ReadOnlySpan<object?> m_references;
        private int m_position;

        internal Reader(
            ReadOnlySpan<byte> bytes,
            ReadOnlySpan<object?> references
        ) {
            m_bytes = bytes;
            m_references = references;
        }

        internal readonly bool hasCommands => m_position < m_bytes.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Read<T>() where T : unmanaged
        {
            int size = Unsafe.SizeOf<T>();
            T value = MemoryMarshal.Read<T>(m_bytes.Slice(m_position, size));
            m_position += size;
            return value;
        }

        internal T? ReadReference<T>() where T : class => (T?)m_references[Read<int>()];

        internal RenderBindingId ReadBinding()
        {
            string? value = ReadReference<string>();
            return value is null ? default : new RenderBindingId(value);
        }

        internal ReadOnlySpan<byte> ReadSnapshot()
        {
            int length = Read<int>();
            int start = checked(m_position + 3) & ~3;
            ReadOnlySpan<byte> value = m_bytes.Slice(start, length);
            m_position = checked(start + length);
            return value;
        }
    }
}
