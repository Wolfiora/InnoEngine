using System;
using System.Buffers;
using System.Text;

namespace Inno.Core.Input;

/// <summary>
/// Freezes provisional or committed text without retaining an operating-system input context.
/// </summary>
public sealed record TextCompositionState
{
    /// <summary>
    /// Captures one composition using Unicode scalar positions, independently of UTF-8 or UTF-16 storage.
    /// </summary>
    /// <param name="compositionId">
    /// Nonzero operation identity, increasing for each composition in its owning input route.
    /// </param>
    /// <param name="text">
    /// Well-formed Unicode text; empty provisional text is allowed.
    /// </param>
    /// <param name="selectionStart">
    /// Selection start in Unicode scalar values, or -1 when the input method supplies no selection.
    /// </param>
    /// <param name="selectionLength">
    /// Selection length in Unicode scalar values; must be zero when the start is unspecified.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The text is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The text contains an unpaired UTF-16 surrogate.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The identity is zero or the selection is outside the text.
    /// </exception>
    public TextCompositionState(
        ulong compositionId,
        string text,
        int selectionStart = -1,
        int selectionLength = 0
    ) {
        ArgumentOutOfRangeException.ThrowIfZero(compositionId);
        ArgumentNullException.ThrowIfNull(text);
        int scalarCount = 0;
        ReadOnlySpan<char> remaining = text.AsSpan();
        while (!remaining.IsEmpty)
        {
            if (Rune.DecodeFromUtf16(remaining, out _, out int consumed) != OperationStatus.Done)
                throw new ArgumentException("Composition text must contain well-formed Unicode.", nameof(text));
            remaining = remaining[consumed..];
            scalarCount++;
        }
        if (selectionStart < -1 || selectionStart > scalarCount)
            throw new ArgumentOutOfRangeException(nameof(selectionStart));
        if (selectionLength < 0 || (selectionStart == -1 ? selectionLength != 0 : selectionLength > scalarCount - selectionStart))
            throw new ArgumentOutOfRangeException(nameof(selectionLength));
        this.compositionId = compositionId;
        this.text = text;
        this.selectionStart = selectionStart;
        this.selectionLength = selectionLength;
    }

    /// <summary>
    /// Gets the input-route operation identity, never an object or persistent asset identity.
    /// </summary>
    public ulong compositionId { get; }
    /// <summary>
    /// Gets the complete text without normalization or native storage ownership.
    /// </summary>
    public string text { get; }
    /// <summary>
    /// Gets the scalar selection start, or -1 when unspecified.
    /// </summary>
    public int selectionStart { get; }
    /// <summary>
    /// Gets the scalar selection length.
    /// </summary>
    public int selectionLength { get; }
}
