using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;

namespace Inno.Tooling.Architecture;

internal sealed class AssemblyDocumentationProvider : DocumentationProvider
{
    private readonly Lazy<IReadOnlyDictionary<string, string>> m_members;

    internal AssemblyDocumentationProvider(string path)
    {
        byte[] document = File.ReadAllBytes(path);
        m_members = new(() => ReadMembers(path, document));
    }

    /// <summary>
    /// Compares the identity of this immutable documentation snapshot.
    /// </summary>
    /// <param name="other">
    /// The other provider instance to compare.
    /// </param>
    /// <returns>
    /// True only for the same snapshot instance.
    /// </returns>
    public override bool Equals(object? other) => ReferenceEquals(this, other);

    /// <summary>
    /// Gets the identity hash used by Roslyn's provider cache.
    /// </summary>
    /// <returns>
    /// A hash of this snapshot instance.
    /// </returns>
    public override int GetHashCode() => RuntimeHelpers.GetHashCode(this);

    /// <summary>
    /// Reads one member contract from the captured assembly documentation.
    /// </summary>
    /// <param name="documentationMemberID">
    /// The compiler's exact documentation identifier for the member.
    /// </param>
    /// <param name="preferredCulture">
    /// The requested culture; repository contracts use the published English document.
    /// </param>
    /// <param name="cancellationToken">
    /// A token checked before the lookup.
    /// </param>
    /// <returns>
    /// The member XML, or an empty string when the assembly has no contract for this identifier.
    /// </returns>
    protected override string GetDocumentationForSymbol(
        string documentationMemberID,
        CultureInfo preferredCulture,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        return m_members.Value.TryGetValue(documentationMemberID, out string? documentation) ? documentation : string.Empty;
    }

    private static IReadOnlyDictionary<string, string> ReadMembers(
        string path,
        byte[] document
    ) {
        try
        {
            using var input = new MemoryStream(document, writable: false);
            return XDocument.Load(input).Descendants("member")
                .Where(static element => element.Attribute("name") is not null)
                .GroupBy(static element => (string)element.Attribute("name")!, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key, static group => group.First().ToString(), StringComparer.Ordinal);
        }
        catch (System.Xml.XmlException failure)
        {
            throw new InvalidDataException($"The required inherited assembly documentation is invalid: '{path}'.", failure);
        }
    }
}
