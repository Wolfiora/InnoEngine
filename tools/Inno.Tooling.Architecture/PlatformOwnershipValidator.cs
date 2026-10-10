using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class PlatformOwnershipValidator
{
    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        string[] roots = ["src", "backends", "platforms", "build", "tools", "tests"];
        var projects = roots.SelectMany(owner => RepositorySourceInventory.Files(Path.Combine(root, owner), "*.csproj"))
            .ToDictionary(path => Path.GetFullPath(path), StringComparer.OrdinalIgnoreCase);
        foreach (string path in projects.Keys)
        {
            string relative = Path.GetRelativePath(root, path).Replace('\\', '/');
            XDocument project = XDocument.Load(path);
            bool product = project.Descendants("OutputType").Any(static item => item.Value == "Exe");
            if (product && relative.StartsWith("platforms/", StringComparison.Ordinal)
                && (!project.Descendants("InnoProductId").Any() || !project.Descendants("InnoProductTarget").Any()))
                failures.Add($"{relative}: a platform product must declare its product and exact target before SDK evaluation.");
            foreach (XElement compile in project.Descendants("Compile").Where(static item => item.Attribute("Include") is not null))
            {
                string include = compile.Attribute("Include")!.Value.Replace('\\', '/');
                bool generated = include.Contains("Generated", StringComparison.Ordinal)
                    || include.Contains("$(BindGen", StringComparison.Ordinal);
                if (!generated && (compile.Attribute("Link") is not null || include.StartsWith("../", StringComparison.Ordinal)))
                    failures.Add($"{relative}: hand-authored implementation cannot have a second owner through Compile Link.");
            }
            foreach (XElement reference in project.Descendants("ProjectReference"))
            {
                string? include = reference.Attribute("Include")?.Value;
                if (include is null || include.Contains("$(", StringComparison.Ordinal))
                    continue;
                string target = Path.GetFullPath(include.Replace('\\', '/'), Path.GetDirectoryName(path)!);
                string dependency = Path.GetRelativePath(root, target).Replace('\\', '/');
                if (!projects.ContainsKey(target))
                    failures.Add($"{relative}: explicit project dependency is missing: {dependency}.");
                ValidateDependency(relative, dependency, failures);
            }
        }
        foreach (string source in RepositorySourceInventory.Files(Path.Combine(root, "src"), "*.cs"))
        {
            string relative = Path.GetRelativePath(root, source).Replace('\\', '/');
            // Core IO owns portable filesystem operations; these calls choose OS primitives, never product targets.
            if (relative.StartsWith("src/foundation/core/Inno.Core.IO/", StringComparison.Ordinal)
                || relative.StartsWith("src/composition/adapters/", StringComparison.Ordinal))
                continue;
            var syntax = CSharpSyntaxTree.ParseText(File.ReadAllText(source)).GetRoot();
            foreach (InvocationExpressionSyntax call in syntax.DescendantNodes().OfType<InvocationExpressionSyntax>())
                if (call.Expression is MemberAccessExpressionSyntax access
                    && access.Expression.ToString() == "OperatingSystem" && access.Name.Identifier.ValueText.StartsWith("Is", StringComparison.Ordinal))
                    failures.Add($"{relative}: shared code must receive capabilities instead of selecting a product or backend by host OS.");
        }
    }

    internal static void ValidateDependency(
        string relative,
        string dependency,
        ICollection<string> failures
    ) {
        bool shared = relative.StartsWith("src/", StringComparison.Ordinal)
            && !relative.StartsWith("src/composition/adapters/", StringComparison.Ordinal);
        string[] segments = relative.Split('/');
        bool platformBase = relative.StartsWith("platforms/", StringComparison.Ordinal)
            && segments.Length > 2 && segments[2] is "build" or "runtime";
        bool runtimeIntegration = relative.Contains("/integrations/", StringComparison.Ordinal)
            && relative.Contains(".Bgfx.Runtime/", StringComparison.Ordinal);
        if (runtimeIntegration && (dependency.Contains("/build/", StringComparison.Ordinal)
            || dependency.StartsWith("build/", StringComparison.Ordinal)
            || dependency.Contains("/Inno.Native.Bgfx/", StringComparison.Ordinal)
            || dependency.StartsWith("platforms/", StringComparison.Ordinal)
                && dependency.Contains("/integrations/", StringComparison.Ordinal)
                && dependency.Contains(".Bgfx/", StringComparison.Ordinal)))
            failures.Add($"{relative}: runtime surface integrations cannot depend on build tools or native BGFX {dependency}.");
        if (platformBase && dependency.StartsWith("build/bindings/", StringComparison.Ordinal))
            failures.Add($"{relative}: platform modules must borrow the neutral binding generator contract {dependency}.");
        if (relative.StartsWith("build/toolchains/", StringComparison.Ordinal)
            && (dependency.StartsWith("build/bindings/", StringComparison.Ordinal)
                || dependency.StartsWith("build/tasks/", StringComparison.Ordinal)))
            failures.Add($"{relative}: neutral toolchains cannot depend on a generator implementation or MSBuild tasks {dependency}.");
        if (relative.StartsWith("build/bindings/", StringComparison.Ordinal)
            && dependency.StartsWith("build/tasks/", StringComparison.Ordinal))
            failures.Add($"{relative}: binding generation cannot depend on MSBuild tasks {dependency}.");
        if (platformBase && dependency.StartsWith("backends/", StringComparison.Ordinal))
            failures.Add($"{relative}: platform base modules cannot select backend implementations {dependency}.");
        if (platformBase && dependency.Contains("/integrations/", StringComparison.Ordinal))
            failures.Add($"{relative}: platform base modules cannot depend on integration implementations {dependency}.");
        if (relative.Contains("/integrations/", StringComparison.Ordinal)
            && dependency.StartsWith("build/distributions/", StringComparison.Ordinal))
            failures.Add($"{relative}: integrations cannot own distribution registration {dependency}.");
        if (relative.Contains("/integrations/", StringComparison.Ordinal) && relative.Contains(".Bgfx/", StringComparison.Ordinal)
            && dependency.Contains("/Inno.Native.Bgfx/", StringComparison.Ordinal))
            failures.Add($"{relative}: build integrations must not expose or load native BGFX bindings {dependency}.");
        if (shared && dependency.StartsWith("platforms/", StringComparison.Ordinal))
            failures.Add($"{relative}: shared mechanisms cannot reference a concrete platform {dependency}.");
        if (relative.StartsWith("backends/", StringComparison.Ordinal) && !relative.Contains("/tests/", StringComparison.Ordinal)
            && dependency.StartsWith("platforms/", StringComparison.Ordinal))
            failures.Add($"{relative}: reusable backends cannot depend on platform packages {dependency}.");
        if (relative.StartsWith("platforms/", StringComparison.Ordinal) && dependency.StartsWith("platforms/", StringComparison.Ordinal)
            && relative.Split('/')[1] != dependency.Split('/')[1])
            failures.Add($"{relative}: one concrete platform cannot depend on another {dependency}.");
        if (relative.StartsWith("build/composition/", StringComparison.Ordinal)
            && (dependency.StartsWith("platforms/", StringComparison.Ordinal)
                || dependency.StartsWith("backends/", StringComparison.Ordinal)
                || dependency.StartsWith("build/distributions/", StringComparison.Ordinal)))
            failures.Add($"{relative}: neutral build composition must receive concrete contributions {dependency}.");
        if (relative.StartsWith("src/composition/editor/hosting/", StringComparison.Ordinal)
            && (dependency.StartsWith("backends/", StringComparison.Ordinal)
                || dependency.StartsWith("build/distributions/", StringComparison.Ordinal)
                || dependency.StartsWith("src/composition/adapters/", StringComparison.Ordinal)))
            failures.Add($"{relative}: shared Editor hosting must receive concrete services {dependency}.");
    }
}
