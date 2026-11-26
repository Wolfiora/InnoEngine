using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Inno.Testing.Validation;

/// <summary>
/// Coordinates fixture input changes with a real check announced by the validation runner.
/// </summary>
public sealed class ValidationCheckObserver : TextWriter
{
    private readonly TextWriter m_previous = Console.Out;
    private readonly Action m_onInstructions;
    private readonly AsyncLocal<bool> m_observing = new();

    public ValidationCheckObserver(Action onInstructions)
    {
        m_onInstructions = onInstructions;
        m_observing.Value = true;
        Console.SetOut(this);
    }

    public override Encoding Encoding => m_previous.Encoding;

    public override void Write(char value) => m_previous.Write(value);

    public override void WriteLine(string? value)
    {
        m_previous.WriteLine(value);
        if (m_observing.Value && value?.StartsWith("Validation check: instructions;", StringComparison.Ordinal) == true)
        {
            m_onInstructions();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Console.SetOut(m_previous);
        }
        base.Dispose(disposing);
    }
}
