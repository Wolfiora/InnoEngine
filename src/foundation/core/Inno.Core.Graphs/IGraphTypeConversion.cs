namespace Inno.Core.Graphs;

/// <summary>
/// Determines whether one graph value type can be converted to another.
/// </summary>
public interface IGraphTypeConversion
{
    /// <summary>
    /// Tests a directed value conversion.
    /// </summary>
    /// <param name="sourceTypeID">
    /// Source value type identifier.
    /// </param>
    /// <param name="destinationTypeID">
    /// Destination value type identifier.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the conversion is valid; otherwise <see langword="false"/>.
    /// </returns>
    bool CanConvert(
        string sourceTypeID,
        string destinationTypeID
    );
}
