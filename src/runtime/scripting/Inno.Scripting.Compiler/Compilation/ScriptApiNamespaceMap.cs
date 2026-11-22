using System.Collections.Immutable;

namespace Inno.Scripting.Compiler;

internal sealed class ScriptApiNamespaceMap
{
    internal ScriptApiNamespaceMap(
        string apiNamespace,
        ImmutableArray<string> implementationNamespaces
    ) {
        this.apiNamespace = apiNamespace;
        this.implementationNamespaces = implementationNamespaces;
    }

    internal string apiNamespace { get; }
    internal ImmutableArray<string> implementationNamespaces { get; }
}
