namespace Inno.Rendering.Shaders;

/// <summary>
/// Reads dependencies only from the current source candidate and records their import dependencies.
/// </summary>
public interface IShaderSourceResolver
{
    /// <summary>
    /// Resolves and reads an include without bypassing asset mount permissions.
    /// </summary>
    /// <param name="includingFile">
    /// Resolved path of the including source.
    /// </param>
    /// <param name="include">
    /// Source-local include spelling.
    /// </param>
    /// <returns>
    /// A resolved immutable source snapshot.
    /// </returns>
    ShaderSourceFile ReadInclude(
        string includingFile,
        string include
    );
}
