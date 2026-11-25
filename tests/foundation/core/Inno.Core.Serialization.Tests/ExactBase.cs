namespace Inno.Core.Serialization.Tests;

// Open: ExactLeaf derives from it for the converter-selection tests.
internal class ExactBase
{
    public int value { get; set; }
    public string selectedBy { get; set; } = string.Empty;
}
