using System.Threading;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Supplies the host-selected framework contract for one compilation or IDE projection operation.
/// </summary>
public interface IScriptCompilationEnvironmentProvider
{
    /// <summary>
    /// Captures current SDK inputs without choosing a framework inside the scripting compiler.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels input preparation before any script artifact is published.
    /// </param>
    /// <returns>
    /// Complete immutable inputs; missing or inconsistent SDK inputs must fail explicitly.
    /// </returns>
    ScriptCompilationInputs Capture(CancellationToken cancellationToken = default);
}
