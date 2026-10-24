using System;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains.Bgfx;

internal sealed class BgfxBuildRequest
{
    internal BgfxBuildRequest(
        NativeBuildContext operation,
        BgfxNativeBuildProfile profile,
        bool includeTools
    ) {
        ArgumentNullException.ThrowIfNull(operation);
        NativeToolchainSelection initial = operation.RequireToolchain();
        ArgumentNullException.ThrowIfNull(profile);
        if (profile.targetId != initial.targetId)
            throw new ArgumentException("The BGFX profile does not match the frozen toolchain target.", nameof(profile));
        context = operation.WithToolchain(profile.SelectTools(operation, includeTools));
        component = includeTools ? BgfxToolsBuild.componentDescriptor : BgfxNativeBuild.componentDescriptor;
        NativeComponentBuildOptions options = context.RequireComponentOptions(component);
        if (options.libraryKind != NativeLibraryKind.Shared || options.cmakeArguments.Count != 0)
            throw new NotSupportedException("The standalone graphics recipe requires shared linkage and its explicit generation profile.");
        artifactToken = profile.artifactPathToken;
        ArgumentException.ThrowIfNullOrWhiteSpace(artifactToken);
        string[] arguments = profile.generatorArguments?.ToArray()
            ?? throw new ArgumentException("The BGFX profile has no generator arguments.", nameof(profile));
        if (arguments.Length == 0 || arguments.Any(static value => value is null))
            throw new ArgumentException("The BGFX profile requires assigned generator arguments.", nameof(profile));
        generatorArguments = includeTools ? ["--with-shared-lib", "--with-tools", .. arguments]
            : ["--with-shared-lib", .. arguments];
        bindingCmakeArguments = includeTools ? [] : profile.bindingCmakeArguments.ToArray();
        invocation = profile.CreateBuildInvocation(context, includeTools)
            ?? throw new ArgumentException("The BGFX profile returned no SDK invocation.", nameof(profile));
        NativeToolchainSelection tools = context.RequireToolchain();
        compiler = tools.ResolveExecutable(invocation.tool);
        externalDirectory = Path.Combine(context.engineRoot, "extern");
        string bgfx = Path.Combine(externalDirectory, "bgfx");
        string bx = Path.Combine(externalDirectory, "bx");
        string bimg = Path.Combine(externalDirectory, "bimg");
        BgfxBuildUtils.ValidateSubmodules(bgfx, bx, bimg);
        genie = tools.ResolveExecutable("genie");
        string[] declarations = ["artifact=" + artifactToken,
            .. generatorArguments.Select(static value => "generate=" + value),
            .. bindingCmakeArguments.Select(static value => "bridge=" + value),
            "compiler=" + compiler, .. invocation.arguments.Select(static value => "compile=" + value)];
        string implementation = component.GetToolchainRoot(context.engineRoot);
        recipe = NativeBuildRecipe.CreateForComponent(context, component,
            includeTools ? "bgfx-tools" : "bgfx", tools.targetId,
            new[] { bgfx, bx, bimg, genie }.Concat(includeTools ? [] :
                [Path.Combine(implementation, "Native", "SharedBindings", "CMakeLists.txt")]), declarations,
            new[] { "BgfxBuildConstants.cs", "BgfxBuildInvocation.cs", "BgfxBuildRequest.cs", "BgfxBuildSession.cs",
                "BgfxBuildUtils.cs", "BgfxNativeBuild.cs", "BgfxNativeBuildProfile.cs", "BgfxToolsBuild.cs" }
                .Concat(includeTools ? [] : ["BgfxBindingProject.cs"])
                .Select(file => Path.Combine(implementation, file)));
    }

    internal NativeBuildContext context { get; }
    internal NativeComponentDescriptor component { get; }
    internal NativeBuildRecipe recipe { get; }
    internal string[] generatorArguments { get; }
    internal string[] bindingCmakeArguments { get; }
    internal BgfxBuildInvocation invocation { get; }
    internal string compiler { get; }
    internal string genie { get; }
    internal string externalDirectory { get; }
    internal string artifactToken { get; }
}
