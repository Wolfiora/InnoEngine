namespace Inno.Core.Graphs;

/// <summary>
/// Resolves generation-scoped node definitions by stable identifier.
/// </summary>
public interface IGraphNodeDefinitionResolver
{
    /// <summary>
    /// Tries to resolve an active node definition.
    /// </summary>
    /// <param name="definitionID">
    /// Stable definition identifier.
    /// </param>
    /// <param name="definition">
    /// Receives the generation-scoped definition.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when active; otherwise <see langword="false"/>.
    /// </returns>
    bool TryResolve(
        string definitionID,
        out GraphNodeDefinition? definition
    );
}
