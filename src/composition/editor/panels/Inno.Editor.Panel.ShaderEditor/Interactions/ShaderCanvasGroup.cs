namespace Inno.Editor.Panel.ShaderEditor;

internal struct ShaderCanvasGroup
{
    /// <summary>
    /// Gets the stable identity used to reference this value across subsystem boundaries.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets the title text used by the current instance.
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// Gets graph nodes affected by this organization action.
    /// </summary>
    public string[] nodes { get; set; }
}
