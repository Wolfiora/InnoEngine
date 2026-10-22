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
        NativeBindingGenerationRequest request,
        NativeBindingProfile profile
    ) {
        string definition = Path.GetFullPath(component.bindingDefinition!, context.engineRoot);
        string directory = Path.GetDirectoryName(definition)!;
        XElement project = XDocument.Load(definition).Root
            ?? throw new InvalidDataException("A binding definition is empty.");
        if (project.Name != "Project" || project.Attributes().Any())
            throw new InvalidDataException("Binding definitions must be declarative Project data.");
        Dictionary<string, string> properties = new(StringComparer.Ordinal);
        foreach (XElement group in project.Elements())
        {
            if (group.Attributes().Any())
                throw new InvalidDataException("Binding definitions cannot evaluate conditions.");
            if (group.Name == "PropertyGroup")
            {
                foreach (XElement property in group.Elements())
                {
                    if (property.Name.LocalName is not ("InnoNativeComponentId" or "InnoBindingConfig")
                        || property.Attributes().Any() || property.HasElements || !properties.TryAdd(property.Name.LocalName, property.Value))
                        throw new InvalidDataException("A binding definition has an unknown or repeated property.");
                }
            }
            else
                throw new InvalidDataException("A binding definition cannot contain executable build logic.");
        }
        if (!properties.TryGetValue("InnoNativeComponentId", out string? componentId) || componentId != component.id)
            throw new InvalidDataException("The binding definition must declare the requested component identity.");
        if (!properties.TryGetValue("InnoBindingConfig", out string? hostConfig))
            throw new InvalidDataException("A binding definition requires its common binding configuration.");
        if (request.targetId != profile.targetId)
            throw new NotSupportedException($"The selected binding profile does not serve '{request.targetId}'.");
        return new(component, definition, Resolve(hostConfig), request, profile);
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
