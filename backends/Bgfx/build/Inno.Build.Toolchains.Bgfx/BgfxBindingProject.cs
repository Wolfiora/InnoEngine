using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Bgfx;

internal static class BgfxBindingProject
{
    internal static async Task<string> PrepareAsync(
        NativeBuildContext context,
        BgfxBuildRequest request,
        BgfxNativeBuildProfile profile,
        string source,
        CancellationToken cancellationToken
    ) {
        context.RequireBindings(request.component);
        string definition = Path.Combine(request.component.GetToolchainRoot(context.engineRoot), "Native", "SharedBindings");
        string build = await NativeCMakeExecutor.BuildAsync(context, request.component, definition,
            "inno_bgfx_binding", request.bindingCmakeArguments, cancellationToken).ConfigureAwait(false);
        string archive = NativeCMakeExecutor.FindOutput(context, build,
            "inno_bgfx_binding" + context.RequireToolchain().linkPlan.GetFileExtension(NativeLibraryKind.Static));
        var linking = profile.CreateBindingLinkSettings(archive);
        string script = Path.Combine(context.GetNativeBuildRoot(request.component), "BindingProject.lua");
        string text = "dofile(" + Literal(Path.Combine(source, "scripts", "genie.lua")) + ")\n"
            + "project \"bgfx-shared-lib\"\nconfiguration {}\n"
            + "links { " + Literal(linking.reference) + " }\n"
            + "linkoptions { " + string.Join(", ", linking.arguments.Select(Literal)) + " }\n";
        AtomicFile.WriteAllBytes(script, Encoding.UTF8.GetBytes(text));
        return script;
    }

    private static string Literal(string value)
    {
        if (value.AsSpan().IndexOfAnyInRange('\0', '\x1f') >= 0)
            throw new InvalidDataException("A GENie input path cannot contain control characters.");
        string portable = value.Replace('\\', '/');
        string delimiter = "=";
        while (portable.Contains("]" + delimiter + "]", StringComparison.Ordinal))
            delimiter += "=";
        return "[" + delimiter + "[" + portable + "]" + delimiter + "]";
    }
}
