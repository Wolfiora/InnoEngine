namespace Inno.Scripting.Compiler;

internal struct ScriptSourceTypeDeclaration
{
    /// <summary>
    /// Creates a validated script source type declaration instance.
    /// </summary>
    /// <param name="typeName">
    /// The type name text validated by the script source type declaration operation.
    /// </param>
    /// <param name="declarationKind">
    /// The declaration kind text validated by the script source type declaration operation.
    /// </param>
    /// <param name="partial">
    /// Whether partial behavior is enabled while script source type declaration executes.
    /// </param>
    /// <param name="line">
    /// The line consumed by script source type declaration; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="column">
    /// The column consumed by script source type declaration; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public ScriptSourceTypeDeclaration(
        string typeName,
        string declarationKind,
        bool partial,
        int line,
        int column
    ) {
        this.typeName = typeName;
        this.declarationKind = declarationKind;
        this.partial = partial;
        this.line = line;
        this.column = column;
    }

    /// <summary>
    /// Gets text used for stable identity, presentation, or diagnostics by this contract.
    /// </summary>
    public string typeName { get; set; }

    /// <summary>
    /// Gets text used for stable identity, presentation, or diagnostics by this contract.
    /// </summary>
    public string declarationKind { get; set; }

    /// <summary>
    /// Gets whether the caller-visible condition represented by this property is satisfied.
    /// </summary>
    public bool partial { get; set; }

    /// <summary>
    /// Gets the scalar measurement or identity associated with the current state.
    /// </summary>
    public int line { get; set; }

    /// <summary>
    /// Gets the scalar measurement or identity associated with the current state.
    /// </summary>
    public int column { get; set; }
}
