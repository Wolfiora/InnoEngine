using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Inno.Tooling.Validation;

internal static class ValidationTestHosts
{
    internal static string[] ExcludedSources(ValidationProject[] projects)
    {
        HashSet<string> referenced = projects.SelectMany(static project => project.references).ToHashSet(StringComparer.Ordinal);
        ValidationProject[] candidates = projects.Where(project => (
            project.test && !referenced.Contains(project.path)
            && project.environment.Length != 0 && project.environment is not ("any" or "native-host")
            && !Applies(project.environment.Split(';'))
        )).ToArray();
        if (candidates.Length == 0)
        {
            return [];
        }
        var excluded = new HashSet<string>(StringComparer.Ordinal);
        foreach (ValidationProject candidate in candidates)
        {
            string directory = candidate.path[..(candidate.path.LastIndexOf('/') + 1)];
            foreach (string input in candidate.inputs.Where(path => (
                path.StartsWith(directory, StringComparison.Ordinal)
                && path.StartsWith("tests/", StringComparison.Ordinal) && path.EndsWith(".cs", StringComparison.Ordinal)
            ))
            ) {
                if (!projects.Any(other => (
                    other != candidate && (Array.IndexOf(other.inputs, input) >= 0
                                           || input.StartsWith(other.path[..(other.path.LastIndexOf('/') + 1)], StringComparison.Ordinal))
                ))) {
                    excluded.Add(input);
                }
            }
        }
        return excluded.Order(StringComparer.Ordinal).ToArray();
    }

    internal static Dictionary<string, string[]> Read(string assembly)
    {
        using FileStream stream = File.OpenRead(assembly);
        using var image = new PEReader(stream);
        MetadataReader metadata = image.GetMetadataReader();
        var contracts = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (TypeDefinitionHandle handle in metadata.TypeDefinitions)
        {
            TypeDefinition type = metadata.GetTypeDefinition(handle);
            string name = Name(handle);
            string[] typeHosts = Hosts(type.GetCustomAttributes());
            foreach (MethodDefinitionHandle methodHandle in type.GetMethods())
            {
                MethodDefinition method = metadata.GetMethodDefinition(methodHandle);
                string[] hosts = Hosts(method.GetCustomAttributes());
                if (hosts.Length == 0)
                {
                    hosts = typeHosts;
                }
                if (hosts.Length != 0)
                {
                    string key = name + "." + metadata.GetString(method.Name);
                    if (contracts.TryGetValue(key, out string[]? existing) && !existing.SequenceEqual(hosts))
                    {
                        throw new InvalidDataException($"Overloaded test methods declare ambiguous OS contracts: {key}");
                    }
                    contracts[key] = hosts;
                }
            }
        }
        return contracts;

        string Name(TypeDefinitionHandle handle)
        {
            TypeDefinition type = metadata.GetTypeDefinition(handle);
            string name = metadata.GetString(type.Name);
            TypeDefinitionHandle parent = type.GetDeclaringType();
            return parent.IsNil ? metadata.GetString(type.Namespace) + "." + name : Name(parent) + "+" + name;
        }

        string[] Hosts(CustomAttributeHandleCollection handles)
        {
            var hosts = new HashSet<string>(StringComparer.Ordinal);
            foreach (CustomAttributeHandle handle in handles)
            {
                CustomAttribute attribute = metadata.GetCustomAttribute(handle);
                EntityHandle parent = attribute.Constructor.Kind switch
                {
                    HandleKind.MemberReference => metadata.GetMemberReference((MemberReferenceHandle)attribute.Constructor).Parent,
                    HandleKind.MethodDefinition => metadata
                        .GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor)
                        .GetDeclaringType(),
                    _ => default
                };
                if (parent.Kind == HandleKind.TypeReference)
                {
                    TypeReference type = metadata.GetTypeReference((TypeReferenceHandle)parent);
                    if (metadata.GetString(type.Namespace) != "System.Runtime.Versioning"
                        || metadata.GetString(type.Name) != "SupportedOSPlatformAttribute"
                    ) {
                        continue;
                    }
                    BlobReader value = metadata.GetBlobReader(attribute.Value);
                    if (value.ReadUInt16() != 1)
                    {
                        throw new InvalidDataException("Invalid compiled test host contract.");
                    }
                    string host = value.ReadSerializedString() ?? string.Empty;
                    if (host is not ("windows" or "macos" or "linux"))
                    {
                        throw new InvalidDataException(
                            $"Unsupported test host contract '{host}'; declare an explicit supported OS without a version."
                        );
                    }
                    hosts.Add(host);
                }
            }
            return hosts.Order(StringComparer.Ordinal).ToArray();
        }
    }

    internal static bool Applies(string[] hosts)
    {
        if (hosts.Any(static host => host is not ("windows" or "macos" or "linux")))
        {
            throw new InvalidDataException("Unknown test host requirement.");
        }
        return hosts.Any(static host => host switch
        {
            "windows" => OperatingSystem.IsWindows(),
            "macos" => OperatingSystem.IsMacOS(),
            "linux" => OperatingSystem.IsLinux(),
            _ => throw new InvalidDataException("Unknown test host requirement.")
        });
    }
}
