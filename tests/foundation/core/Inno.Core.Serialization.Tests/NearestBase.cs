namespace Inno.Core.Serialization.Tests;

// Open: NearestMid derives from it for the converter-selection tests.
internal class NearestBase
{
    public int value { get; set; }
    public string selectedBy { get; set; } = string.Empty;
}
