using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Inno.Scripting.Compiler;

internal static class ScriptApiDocumentationBuilder
{
    internal const string C_SCHEMA = "inno-script-documentation/2";

    internal sealed record Inputs(
        string identity,
        IReadOnlyDictionary<string, byte[]?> documents
    );

    internal static Inputs Capture(IReadOnlyList<ScriptApiTypeExport> exports)
    {
        var documents = new Dictionary<string, byte[]?>(StringComparer.Ordinal);
        var identity = new StringBuilder(C_SCHEMA);
        foreach (Assembly assembly in exports.Select(static export => export.type.Assembly).Distinct()
            .OrderBy(static assembly => assembly.GetName().Name, StringComparer.Ordinal))
        {
            string path = Path.ChangeExtension(assembly.Location, ".xml");
            byte[]? bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
            documents.Add(path, bytes);
            identity.Append('\n').Append(assembly.GetName().Name).Append(':')
                .Append(bytes is null ? "missing" : Convert.ToHexStringLower(SHA256.HashData(bytes)));
        }
        return new(Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(identity.ToString()))), documents);
    }

    internal static void Verify(Inputs inputs)
    {
        foreach (var input in inputs.documents)
        {
            bool exists = File.Exists(input.Key);
            if (exists != (input.Value is not null) || exists && !File.ReadAllBytes(input.Key).AsSpan().SequenceEqual(input.Value))
                throw new IOException("Script API source documentation changed during reference preparation: " + input.Key);
        }
    }

    internal static void Write(
        string outputPath,
        string assemblyName,
        IReadOnlyList<ScriptApiTypeExport> exports,
        IReadOnlyDictionary<string, string> namespaceMappings,
        IReadOnlyList<ScriptApiTypeMapping> typeMappings,
        Inputs inputs
    ) {
        var outputMembers = new List<XElement>();
        var emittedNames = new HashSet<string>(StringComparer.Ordinal);
        (string source, string target)[] replacements = CreateIdentityReplacements(namespaceMappings, typeMappings);
        foreach (IGrouping<Assembly, ScriptApiTypeExport> group in exports.GroupBy(
                     static export => export.type.Assembly))
        {
            string documentationPath = Path.ChangeExtension(group.Key.Location, ".xml");
            byte[]? bytes = inputs.documents[documentationPath];
            if (bytes is null)
                continue;
            using var stream = new MemoryStream(bytes, writable: false);
            XDocument source = XDocument.Load(stream, LoadOptions.PreserveWhitespace);
            XElement[] members = source.Root?
                .Element("members")?
                .Elements("member")
                .ToArray() ?? [];
            foreach (ScriptApiTypeExport export in group)
            {
                Type type = export.type;
                string typeName = GetDocumentationTypeName(type);
                foreach (XElement member in members.Where(member => BelongsToType(member, typeName)))
                {
                    var copy = new XElement(member);
                    RewriteDocumentationIdentities(copy, replacements);
                    string? name = copy.Attribute("name")?.Value;
                    if (name is not null && emittedNames.Add(name))
                        outputMembers.Add(copy);
                }
            }
        }
        foreach (ScriptApiTypeExport export in exports.Where(export => !string.Equals(
                     export.name,
                     GetRuntimeTypeName(export.type),
                     StringComparison.Ordinal)))
        {
            ScriptApiTypeMapping mapping = typeMappings.Single(value =>
                string.Equals(
                    value.implementationNamespace,
                    export.type.Namespace ?? string.Empty,
                    StringComparison.Ordinal) &&
                string.Equals(value.implementationName, GetRuntimeTypeName(export.type), StringComparison.Ordinal) &&
                string.Equals(value.apiName, export.name, StringComparison.Ordinal));
            string facadeTypeName = mapping.apiNamespace + "." + mapping.apiName;
            AddFallbackDocumentation(
                outputMembers,
                emittedNames,
                "T:" + facadeTypeName,
                $"Provides the {mapping.apiName} values exposed by the script API.");
            foreach (FieldInfo field in export.type.GetFields(
                         BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                AddFallbackDocumentation(
                    outputMembers,
                    emittedNames,
                    "F:" + facadeTypeName + "." + field.Name,
                    $"Provides the {field.Name} value from {mapping.apiName}.");
            }
        }

        // Origin data is emitted from the same resolved exports as the logical facade.
        // Documentation consumers must still select members from the actual reference assembly.
        var origins = exports.Select(export =>
        {
            string sourceId = "T:" + GetDocumentationTypeName(export.type);
            string logicalId = sourceId;
            foreach ((string sourceName, string targetName) in replacements)
                logicalId = logicalId.Replace(sourceName, targetName, StringComparison.Ordinal);
            return new XElement("type", new XAttribute("id", logicalId),
                new XAttribute("source", sourceId),
                new XAttribute("assembly", export.type.Assembly.GetName().Name
                    ?? throw new InvalidOperationException("An exported type has no assembly identity.")));
        }).OrderBy(static origin => (string?)origin.Attribute("id"), StringComparer.Ordinal);
        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("doc",
                new XElement("assembly", new XElement("name", assemblyName)),
                new XElement("provenance", new XAttribute("format", C_SCHEMA), origins),
                new XElement("members", outputMembers.OrderBy(
                    static member => member.Attribute("name")?.Value,
                    StringComparer.Ordinal))));
        string temporaryPath = outputPath + ".tmp";
        document.Save(temporaryPath);
        File.Move(temporaryPath, outputPath, overwrite: true);
    }

    private static bool BelongsToType(
        XElement member,
        string typeName
    ) {
        string? name = member.Attribute("name")?.Value;
        if (string.IsNullOrEmpty(name) || name.Length < 3 || name[1] != ':')
            return false;
        ReadOnlySpan<char> declarationName = name.AsSpan(2);
        return name[0] == 'T'
            ? declarationName.Equals(typeName, StringComparison.Ordinal)
            : declarationName.Length > typeName.Length &&
              declarationName[typeName.Length] == '.' &&
              declarationName.StartsWith(typeName, StringComparison.Ordinal);
    }

    private static string GetDocumentationTypeName(Type type) => (type.FullName ?? type.Name).Replace('+', '.');

    private static string GetRuntimeTypeName(Type type)
    {
        int aritySeparator = type.Name.IndexOf('`');
        return aritySeparator < 0 ? type.Name : type.Name[..aritySeparator];
    }

    private static void AddFallbackDocumentation(
        ICollection<XElement> outputMembers,
        ISet<string> emittedNames,
        string memberName,
        string summary
    ) {
        if (!emittedNames.Add(memberName))
            return;
        outputMembers.Add(new XElement(
            "member",
            new XAttribute("name", memberName),
            new XElement("summary", summary)));
    }

    private static void RewriteDocumentationIdentities(
        XElement member,
        IReadOnlyList<(string source, string target)> replacements
    ) {
        foreach (XAttribute attribute in member.DescendantsAndSelf().Attributes())
        {
            if (attribute.Name.LocalName is not ("name" or "cref"))
                continue;
            string value = attribute.Value;
            foreach ((string source, string target) in replacements)
                value = value.Replace(source, target, StringComparison.Ordinal);
            attribute.Value = value;
        }
    }

    private static (string source, string target)[] CreateIdentityReplacements(
        IReadOnlyDictionary<string, string> namespaceMappings,
        IReadOnlyList<ScriptApiTypeMapping> typeMappings
    ) {
        // Freeze the ordered rewrite once per document; unchanged names need no replacement.
        return namespaceMappings
            .OrderByDescending(static pair => pair.Key.Length)
            .Select(static pair => (source: pair.Key, target: pair.Value))
            .Concat(typeMappings
                .OrderByDescending(static mapping => mapping.apiNamespace.Length + mapping.implementationName.Length)
                .Select(static mapping => (
                    source: mapping.apiNamespace + "." + mapping.implementationName,
                    target: mapping.apiNamespace + "." + mapping.apiName)))
            .Where(static replacement => !string.Equals(replacement.source, replacement.target, StringComparison.Ordinal))
            .ToArray();
    }
}
