using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingDefinitionReader
{
    internal static BindingGenerationConfiguration Read(
        NativeBuildContext context,
        NativeComponentDescriptor component,
        NativeBindingGenerationRequest request
    ) {
        string definition = Path.GetFullPath(component.bindingDefinition!, context.engineRoot);
        string directory = Path.GetDirectoryName(definition)!;
        XElement project = XDocument.Load(definition).Root
            ?? throw new InvalidDataException("A binding definition is empty.");
        if (project.Name != "Project" || project.Attributes().Any())
            throw new InvalidDataException("Binding definitions must be declarative Project data.");
        Dictionary<string, string> properties = new(StringComparer.Ordinal);
        Dictionary<string, (string config, string? bridge)> targets = new(StringComparer.Ordinal);
        foreach (XElement group in project.Elements())
        {
            if (group.Attributes().Any())
                throw new InvalidDataException("Binding definitions cannot evaluate conditions.");
            if (group.Name == "PropertyGroup")
            {
                foreach (XElement property in group.Elements())
                {
                    if (property.Name.LocalName is not ("InnoBindingHostConfig" or "InnoBindingHostBridge" or "InnoBindingExtensionProject")
                        || property.Attributes().Any() || property.HasElements || !properties.TryAdd(property.Name.LocalName, property.Value))
                        throw new InvalidDataException("A binding definition has an unknown or repeated property.");
                }
            }
            else if (group.Name == "ItemGroup")
            {
                foreach (XElement target in group.Elements())
                {
                    if (target.Name != "InnoBindingTarget" || target.Attributes().Count() != 1
                        || target.Attribute("Include") is not XAttribute identity)
                        throw new InvalidDataException("A binding target requires one explicit identity.");
                    XElement[] fields = target.Elements().ToArray();
                    if (fields.Any(field => field.Attributes().Any() || field.HasElements || field.Name != "Config" && field.Name != "Bridge")
                        || fields.Count(field => field.Name == "Config") != 1 || fields.Count(field => field.Name == "Bridge") > 1
                        || !targets.TryAdd(identity.Value, (target.Element("Config")!.Value, target.Element("Bridge")?.Value)))
                        throw new InvalidDataException("A binding target has invalid or duplicated configuration fields.");
                }
            }
            else
                throw new InvalidDataException("A binding definition cannot contain executable build logic.");
        }
        if (!properties.TryGetValue("InnoBindingHostConfig", out string? hostConfig))
            throw new InvalidDataException("A binding definition requires its explicit host configuration.");
        properties.TryGetValue("InnoBindingHostBridge", out string? hostBridge);
        (string config, string? bridge) selected = request.outputMode == NativeBindingOutputMode.HostSource
            ? (hostConfig, hostBridge) : targets.TryGetValue(request.targetId, out var selectedTarget)
                ? selectedTarget : throw new NotSupportedException($"The component has no binding configuration for '{request.targetId}'.");
        if (hostBridge is not null && selected.bridge is null)
            throw new InvalidDataException("A facade target must explicitly declare its corresponding bridge configuration.");
        return new(component, definition, Resolve(selected.config), ResolveOptional(selected.bridge),
            ResolveOptional(hostBridge), ResolveOptional(properties.GetValueOrDefault("InnoBindingExtensionProject")), request);

        string? ResolveOptional(string? path) => path is null ? null : Resolve(path);
        string Resolve(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || Path.IsPathFullyQualified(path) || path.Contains('$') || path.Contains('@')
                || path.Contains(':') || path.Replace('\\', '/').Split('/').Any(static part => part is "" or "." or ".."))
                throw new InvalidDataException("Binding definition paths must be literal relative children.");
            string absolute = Path.GetFullPath(path, directory);
            if (!File.Exists(absolute))
                throw new FileNotFoundException("A declared binding input is missing.", absolute);
            return absolute;
        }
    }
}
