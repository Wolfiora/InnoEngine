using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class NativeBindingOwnershipValidator
{
    internal static void Validate(
        string repositoryRoot,
        IEnumerable<string> projects,
        ICollection<string> failures
    ) {
        Dictionary<string, string> components = new(StringComparer.Ordinal);
        foreach (string projectPath in projects)
        {
            if (!Path.GetFileName(projectPath).StartsWith("Inno.Native.", StringComparison.Ordinal))
                continue;
            string bindings = Path.Combine(Path.GetDirectoryName(projectPath)!, "Bindings");
            XDocument project = XDocument.Load(projectPath);
            bool generated = project.Descendants("BindGenGeneratedBindings").Any(static value => value.Value == "true");
            if (!generated && !Directory.Exists(bindings))
                continue;
            string relative = Path.GetRelativePath(repositoryRoot, projectPath).Replace('\\', '/');
            string definition = Path.Combine(bindings, "bindings.props");
            if (!File.Exists(definition))
            {
                failures.Add($"{relative}: generated bindings require the component's Bindings/bindings.props owner.");
                continue;
            }
            if (project.Descendants("Import").Count(static import =>
                ((string?)import.Attribute("Project"))?.Replace('\\', '/') == "Bindings/bindings.props") != 1)
                failures.Add($"{relative}: the native project must import its binding definition exactly once.");
            try
            {
                XElement root = XDocument.Load(definition).Root
                    ?? throw new InvalidDataException("The binding definition is empty.");
                if (root.Name != "Project" || root.HasAttributes)
                    throw new InvalidDataException("The binding definition must contain declarative Project data.");
                Dictionary<string, string> properties = new(StringComparer.Ordinal);
                foreach (XElement group in root.Elements())
                {
                    if (group.HasAttributes)
                        throw new InvalidDataException("Binding definitions cannot evaluate conditions.");
                    if (group.Name == "PropertyGroup")
                    {
                        foreach (XElement property in group.Elements())
                        {
                            if (property.Name.LocalName is not ("InnoNativeComponentId" or "InnoBindingConfig")
                                || property.HasAttributes || property.HasElements
                                || !properties.TryAdd(property.Name.LocalName, property.Value))
                                throw new InvalidDataException("The binding definition has an unknown or repeated property.");
                        }
                    }
                    else
                        throw new InvalidDataException("Binding definitions cannot contain executable build logic.");
                }
                if (!properties.TryGetValue("InnoNativeComponentId", out string? componentId)
                    || string.IsNullOrWhiteSpace(componentId))
                    throw new InvalidDataException("A binding definition requires its component identity.");
                if (!components.TryAdd(componentId, relative))
                    failures.Add($"{relative}: binding component '{componentId}' already belongs to '{components[componentId]}'.");
                if (!properties.TryGetValue("InnoBindingConfig", out string? hostConfig))
                    throw new InvalidDataException("A binding definition requires its common Config input.");
                ValidateConfigPath(bindings, hostConfig);
            }
            catch (Exception failure) when (failure is InvalidDataException or XmlException)
            {
                failures.Add($"{relative}: {failure.Message}");
            }
        }
    }

    private static void ValidateConfigPath(
        string bindings,
        string path
    ) {
        if (string.IsNullOrWhiteSpace(path) || Path.IsPathFullyQualified(path)
            || path.IndexOfAny(['$', '@', ':']) >= 0
            || path.Replace('\\', '/').Split('/').Any(static part => part is "" or "." or ".."))
            throw new InvalidDataException("Binding Config inputs must be literal relative children of their owner.");
        if (!File.Exists(Path.GetFullPath(path, bindings)))
            throw new InvalidDataException($"The declared binding Config input '{path}' is missing.");
    }
}
