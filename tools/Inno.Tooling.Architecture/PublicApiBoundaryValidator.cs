using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal static class PublicApiBoundaryValidator
{
    internal static void Validate(
        string root,
        string configuration,
        ICollection<string> failures,
        IReadOnlyDictionary<string, string>? evaluatedOutputs = null
    ) {
        Project[] projects = new[] { "entry", "src", "backends", "platforms", "build" }
            .SelectMany(folder => RepositorySourceInventory.Files(Path.Combine(root, folder), "*.csproj"))
            .Where(path => !path.Split(Path.DirectorySeparatorChar).Any(segment => segment is "bin" or "obj"))
            // Binding extensions run only while generating source; they are not runtime or solution assemblies.
            .Where(path => !path.Split(Path.DirectorySeparatorChar).Contains("Bindings", StringComparer.Ordinal))
            .Where(path => !path.Split(Path.DirectorySeparatorChar).Contains("tests", StringComparer.Ordinal))
            .Where(path => !XDocument.Load(path).Descendants("OutputType").Any(static item => item.Value == "Exe"))
            .Select(path => Project.Read(path, configuration, evaluatedOutputs)).ToArray();
        var byName = projects.ToDictionary(project => project.name, StringComparer.Ordinal);
        var references = new Dictionary<string, PortableExecutableReference>(StringComparer.OrdinalIgnoreCase);
        string[] platform = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") ?? string.Empty).Split(Path.PathSeparator);
        foreach (string path in platform.Where(File.Exists))
            references[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
        // Public generator and analyzer contracts require the compiler metadata used by this audit.
        foreach (string path in new[] { typeof(Compilation).Assembly.Location, typeof(CSharpCompilation).Assembly.Location })
            references[Path.GetFileNameWithoutExtension(path)] = MetadataReference.CreateFromFile(path);
        foreach (Project project in projects)
        {
            if (File.Exists(project.output))
                references[project.name] = MetadataReference.CreateFromFile(project.output);
            else if (!project.product)
                failures.Add($"{Path.GetRelativePath(root, project.path)}: public API symbol audit requires a {configuration} solution build; output is missing.");
        }
        CSharpCompilation compilation = CSharpCompilation.Create("Inno.Architecture.SymbolAudit", references: references.Values,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        foreach (Project project in projects.Where(project => project.product
            || project.path.Contains(Path.DirectorySeparatorChar + "src" + Path.DirectorySeparatorChar, StringComparison.Ordinal)))
        {
            IAssemblySymbol assembly;
            bool compiledContract = false;
            if (references.TryGetValue(project.name, out PortableExecutableReference? assemblyReference)
                && compilation.GetAssemblyOrModuleSymbol(assemblyReference) is IAssemblySymbol compiledAssembly)
            {
                assembly = compiledAssembly;
                compiledContract = true;
            }
            else if (project.product)
            {
                // Foreign product SDKs are not needed to audit the public composition contract.
                SyntaxTree[] sources = RepositorySourceInventory.Files(Path.GetDirectoryName(project.path)!, "*.cs")
                    .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
                assembly = CSharpCompilation.Create(project.name, sources, references.Values,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true)).Assembly;
            }
            else
                continue;
            var dependencies = new HashSet<string>(StringComparer.Ordinal);
            foreach (INamedTypeSymbol type in Types(assembly.GlobalNamespace))
            {
                if (!Exposed(type))
                    continue;
                if (project.name.StartsWith("Inno.Adapter.", StringComparison.Ordinal)
                    && project.name.Split('.').Length == 3 && type.TypeKind == TypeKind.Enum
                    && type.Name.EndsWith("Backend", StringComparison.Ordinal))
                {
                    failures.Add($"{Path.GetRelativePath(root, project.path)}: replaceable backend selections require open IDs and provider catalogs, not closed enums.");
                }
                Inspect(type.BaseType, type);
                foreach (INamedTypeSymbol contract in type.Interfaces)
                    Inspect(contract, type);
                foreach (ITypeParameterSymbol parameter in type.TypeParameters)
                    foreach (ITypeSymbol constraint in parameter.ConstraintTypes)
                        Inspect(constraint, type);
                foreach (ISymbol member in type.GetMembers().Where(Exposed))
                {
                    switch (member)
                    {
                        case IMethodSymbol method:
                            Inspect(method.ReturnType, member);
                            foreach (IParameterSymbol parameter in method.Parameters)
                                Inspect(parameter.Type, member);
                            foreach (ITypeParameterSymbol parameter in method.TypeParameters)
                                foreach (ITypeSymbol constraint in parameter.ConstraintTypes)
                                    Inspect(constraint, member);
                            break;
                        case IPropertySymbol property:
                            Inspect(property.Type, member);
                            foreach (IParameterSymbol parameter in property.Parameters)
                                Inspect(parameter.Type, member);
                            break;
                        case IFieldSymbol field: Inspect(field.Type, member); break;
                        case IEventSymbol signal: Inspect(signal.Type, member); break;
                    }
                }
            }
            if (project.name.StartsWith("Inno.Editor.", StringComparison.Ordinal))
            {
                XElement[] referenceGroups = project.references.Select(reference => reference.Parent!).Distinct().ToArray();
                XElement[] implementationGroups = referenceGroups.Where(group => group.Elements("ProjectReference").Any(IsCompilePrivate)).ToArray();
                XElement[] publicGroups = referenceGroups.Where(group => group.Elements("ProjectReference").Any(reference => !IsCompilePrivate(reference))).ToArray();
                if (implementationGroups.Length > 1 || publicGroups.Length > 1 ||
                    referenceGroups.Any(group => group.Elements("ProjectReference").Any(IsCompilePrivate)
                        && group.Elements("ProjectReference").Any(reference => !IsCompilePrivate(reference))) ||
                    (referenceGroups.Length == 2 && publicGroups.Length == 1 && ReferenceEquals(referenceGroups[0], publicGroups[0])))
                {
                    failures.Add($"{Path.GetRelativePath(root, project.path)}: Editor references require one implementation group followed by one public API group.");
                }
                foreach (string dependency in dependencies.Where(byName.ContainsKey).Order(StringComparer.Ordinal))
                {
                    XElement? declaration = project.references.FirstOrDefault(element =>
                        Path.GetFileNameWithoutExtension(element.Attribute("Include")?.Value.Replace('\\', '/') ?? string.Empty) == dependency);
                    if (declaration is null || ((string?)declaration.Attribute("PrivateAssets"))?.Split(';').Contains("compile") == true)
                        failures.Add($"{Path.GetRelativePath(root, project.path)}: public/protected API exposes {dependency}; declare a direct public ProjectReference.");
                }
                foreach (XElement reference in project.references)
                {
                    string dependency = Path.GetFileNameWithoutExtension(reference.Attribute("Include")?.Value.Replace('\\', '/') ?? string.Empty);
                    // Source-only foreign product inspection cannot prove that generated public contracts are absent.
                    if (compiledContract && byName.ContainsKey(dependency)
                        && !dependencies.Contains(dependency) && !IsCompilePrivate(reference))
                        failures.Add($"{Path.GetRelativePath(root, project.path)}: {dependency} is an implementation dependency; declare PrivateAssets=compile.");
                }
            }

            void Inspect(
                ITypeSymbol? value,
                ISymbol member
            ) {
                if (value is null)
                    return;
                if (value.TypeKind == TypeKind.Error)
                {
                    failures.Add($"{Path.GetRelativePath(root, project.path)}: {member.ToDisplayString()} has an unresolved public signature type {value.ToDisplayString()}.");
                    return;
                }
                switch (value)
                {
                    case IArrayTypeSymbol array: Inspect(array.ElementType, member); return;
                    case IPointerTypeSymbol pointer: Inspect(pointer.PointedAtType, member); return;
                    case IFunctionPointerTypeSymbol pointer:
                        Inspect(pointer.Signature.ReturnType, member);
                        foreach (IParameterSymbol parameter in pointer.Signature.Parameters)
                            Inspect(parameter.Type, member);
                        return;
                    case INamedTypeSymbol named:
                        Inspect(named.ContainingType, member);
                        foreach (ITypeSymbol argument in named.TypeArguments)
                            Inspect(argument, member);
                        break;
                }
                string? dependency = value.ContainingAssembly?.Identity.Name;
                if (dependency is null || dependency == project.name)
                    return;
                dependencies.Add(dependency);
                if (dependency.StartsWith("Inno.Native.Bgfx", StringComparison.Ordinal) ||
                    dependency == "Inno.Native.MiniAudio" || dependency == "Inno.Native.Sdl3")
                    failures.Add($"{Path.GetRelativePath(root, project.path)}: {member.ToDisplayString()} exposes native symbol {value.ToDisplayString()} from {dependency}.");
                if ((project.name == "Inno.Shell" || project.name.StartsWith("Inno.Editor.", StringComparison.Ordinal) || project.name == "Inno.Player") &&
                    dependency.StartsWith("Inno.Adapter.", StringComparison.Ordinal) &&
                    dependency.Split('.').Length >= 4 && !dependency.EndsWith("Default", StringComparison.Ordinal))
                    failures.Add($"{Path.GetRelativePath(root, project.path)}: {member.ToDisplayString()} exposes concrete adapter {value.ToDisplayString()}.");
            }
        }
    }

    private static bool Exposed(ISymbol symbol)
        => symbol.DeclaredAccessibility is Accessibility.Public or Accessibility.Protected or Accessibility.ProtectedOrInternal;

    private static bool IsCompilePrivate(XElement reference)
        => ((string?)reference.Attribute("PrivateAssets"))?.Split(';').Contains("compile") == true;

    private static IEnumerable<INamedTypeSymbol> Types(INamespaceOrTypeSymbol owner)
    {
        foreach (ISymbol member in owner.GetMembers())
        {
            if (member is INamespaceSymbol space)
                foreach (INamedTypeSymbol type in Types(space))
                    yield return type;
            else if (member is INamedTypeSymbol type && Exposed(type))
            {
                yield return type;
                foreach (INamedTypeSymbol nested in Types(type))
                    yield return nested;
            }
        }
    }

    private sealed record Project(
        string path,
        string name,
        string output,
        XElement[] references,
        bool product
    ) {
        internal static Project Read(
            string path,
            string configuration,
            IReadOnlyDictionary<string, string>? evaluatedOutputs
        ) {
            XDocument document = XDocument.Load(path);
            string name = document.Descendants("AssemblyName").FirstOrDefault()?.Value ?? Path.GetFileNameWithoutExtension(path);
            string framework = document.Descendants("TargetFramework").FirstOrDefault()?.Value ?? "net10.0";
            string? target = document.Descendants("InnoProductTarget").FirstOrDefault()?.Value;
            string output = evaluatedOutputs is null
                ? Path.Combine(Path.GetDirectoryName(path)!, target is null ? "bin" : Path.Combine("bin", target),
                    configuration, framework, name + ".dll")
                : evaluatedOutputs.TryGetValue(Path.GetFullPath(path), out string? selected) ? selected : string.Empty;
            return new Project(path, name, output,
                document.Descendants("ProjectReference").ToArray(), document.Descendants("InnoProductId").Any());
        }
    }
}
