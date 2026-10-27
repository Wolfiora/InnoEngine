using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal sealed class WikiProjectReferences
{
    private readonly string m_root;
    private readonly string m_configuration;
    private readonly string m_dotnetHost;
    private readonly string? m_target;
    private readonly string? m_artifactsPath;
    private readonly Dictionary<string, CSharpCompilation> m_compilations = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string[]> m_sources = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string> m_outputs = new(StringComparer.OrdinalIgnoreCase);
    private IReadOnlyList<MetadataReference>? m_frameworkReferences;

    internal WikiProjectReferences(
        string root,
        string configuration,
        string dotnetHost,
        string? target = null,
        string? artifactsPath = null
    ) {
        if (!Path.IsPathFullyQualified(dotnetHost) || !File.Exists(dotnetHost))
            throw new ArgumentException("Wiki SDK inspection requires an existing absolute dotnet executable.", nameof(dotnetHost));
        m_root = root;
        m_configuration = configuration;
        m_dotnetHost = dotnetHost;
        m_target = target;
        m_artifactsPath = artifactsPath;
    }

    internal bool IsExcludedProduct(string project)
    {
        if (m_target is null)
            return false;
        string? target = XDocument.Load(project).Descendants("InnoProductTarget").FirstOrDefault()?.Value;
        return target is not null && !string.Equals(target, m_target, StringComparison.Ordinal);
    }

    internal IReadOnlyList<MetadataReference> GetFrameworkReferences(CancellationToken cancellationToken)
    {
        if (m_frameworkReferences is not null)
            return m_frameworkReferences;
        string project = RepositorySourceInventory.Files(Path.Combine(m_root, "src", "foundation"), "*.csproj")
            .Where(path => XDocument.Load(path).Descendants("TargetFramework").Any(static item => item.Value == "net10.0"))
            .Order(StringComparer.Ordinal).First();
        _ = Evaluate(project, cancellationToken, frameworkOnly: true);
        return m_frameworkReferences ?? throw new InvalidDataException("The selected SDK did not provide framework references.");
    }

    internal CSharpCompilation GetCompilation(
        string project,
        CancellationToken cancellationToken
    ) {
        CSharpCompilation compilation = Evaluate(project, cancellationToken);
        string target = m_outputs[project];
        if (!File.Exists(target))
            throw new InvalidDataException($"Build the selected Wiki project contract before validation: '{target}'.");
        return compilation.AddReferences(MetadataReference.CreateFromFile(target,
            documentation: File.Exists(Path.ChangeExtension(target, ".xml"))
                ? new AssemblyDocumentationProvider(Path.ChangeExtension(target, ".xml")) : null));
    }

    internal CSharpCompilation GetSourceCompilation(
        string project,
        CancellationToken cancellationToken
    ) {
        CSharpCompilation contract = Evaluate(project, cancellationToken);
        var trees = new List<SyntaxTree>();
        foreach (string source in m_sources[project])
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!File.Exists(source))
                throw new InvalidDataException($"The evaluated Wiki source is missing: '{source}'.");
            trees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(source),
                (CSharpParseOptions)contract.SyntaxTrees.Single().Options, source,
                cancellationToken: cancellationToken));
        }
        return contract.RemoveAllSyntaxTrees().AddSyntaxTrees(trees)
            .WithAssemblyName(Path.GetFileNameWithoutExtension(m_outputs[project]));
    }

    private CSharpCompilation Evaluate(
        string project,
        CancellationToken cancellationToken,
        bool frameworkOnly = false
    ) {
        if (m_compilations.TryGetValue(project, out CSharpCompilation? compilation))
            return compilation;
        using var bounded = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        bounded.CancelAfter(TimeSpan.FromMinutes(1));
        string output;
        try
        {
            // Resolve compile references only. Design-time evaluation must never prepare native products.
            output = ToolchainEnvironment.CaptureOutputAsync(m_dotnetHost,
                ["msbuild", project, "-nologo", "-m:1", "-nr:false", "-t:ResolveReferences;AddImplicitDefineConstants",
                    "-getItem:ReferencePath,Compile", "-getProperty:TargetPath,DefineConstants,LangVersion,Nullable,AllowUnsafeBlocks",
                    "-p:DesignTimeBuild=true", "-p:BuildProjectReferences=false", "-p:InnoProductRestore=false",
                    "-p:Configuration=" + m_configuration,
                    .. m_artifactsPath is null ? Array.Empty<string>() : new[] { "-p:ArtifactsPath=" + m_artifactsPath }],
                m_root, bounded.Token, DotNetSdkEnvironment.Create(m_dotnetHost)).GetAwaiter().GetResult();
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new InvalidDataException($"Wiki reference resolution exceeded its one-minute budget: '{project}'.");
        }
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        string target = properties.GetProperty("TargetPath").GetString()!;
        string[] references = document.RootElement.GetProperty("Items").GetProperty("ReferencePath")
            .EnumerateArray().Select(static item => item.GetProperty("FullPath").GetString()!)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        if (m_frameworkReferences is null)
        {
            string[] framework = document.RootElement.GetProperty("Items").GetProperty("ReferencePath").EnumerateArray()
                .Where(static item => item.TryGetProperty("FrameworkReferenceName", out JsonElement name)
                    && name.GetString() == "Microsoft.NETCore.App")
                .Select(static item => item.GetProperty("FullPath").GetString()!).ToArray();
            if (framework.Length != 0)
                m_frameworkReferences = framework.Select(static path => (MetadataReference)MetadataReference.CreateFromFile(path)).ToArray();
        }
        if (frameworkOnly)
            return CSharpCompilation.Create("WikiFramework", references: m_frameworkReferences);
        foreach (string reference in references)
            if (!File.Exists(reference))
                throw new FileNotFoundException($"Wiki project '{project}' requires its evaluated compile reference: '{reference}'. Build this target/configuration before validation.", reference);
        string language = properties.GetProperty("LangVersion").GetString()!;
        if (!LanguageVersionFacts.TryParse(language, out LanguageVersion version))
            throw new InvalidDataException($"Wiki project '{project}' selects unsupported C# language version '{language}'. The validator must support that version explicitly.");
        var parse = new CSharpParseOptions(version, preprocessorSymbols:
            properties.GetProperty("DefineConstants").GetString()!.Split(';', StringSplitOptions.RemoveEmptyEntries));
        NullableContextOptions nullable = properties.GetProperty("Nullable").GetString() switch
        {
            "enable" => NullableContextOptions.Enable,
            "annotations" => NullableContextOptions.Annotations,
            "warnings" => NullableContextOptions.Warnings,
            _ => NullableContextOptions.Disable
        };
        compilation = CSharpCompilation.Create("WikiProject",
            [CSharpSyntaxTree.ParseText(string.Empty, parse)],
            references.Select(static path => MetadataReference.CreateFromFile(path,
                documentation: File.Exists(Path.ChangeExtension(path, ".xml"))
                    ? new AssemblyDocumentationProvider(Path.ChangeExtension(path, ".xml")) : null)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                allowUnsafe: properties.GetProperty("AllowUnsafeBlocks").GetString() == "true",
                nullableContextOptions: nullable));
        m_compilations.Add(project, compilation);
        m_outputs.Add(project, target);
        m_sources.Add(project, document.RootElement.GetProperty("Items").GetProperty("Compile")
            .EnumerateArray().Select(static item => item.GetProperty("FullPath").GetString()!)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
        return compilation;
    }

}
