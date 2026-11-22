using System;

namespace Inno.Rendering.Shaders.Tests;

internal sealed class RegistryFrontendProbe : IShaderSourceFrontend, IDisposable
{
    internal static int created;
    internal static int disposed;
    internal static bool conflict;
    private readonly string m_languageID;

    public RegistryFrontendProbe()
    {
        created++;
        m_languageID = conflict ? "inno.shader-language.bgfx-sc" : "tests.registry.language";
    }

    public string languageID => m_languageID;

    public ShaderSourceAnalysis Analyze(ShaderSourceRequest request)
        => new(
            new ShaderSourceFunction(
                request.entryPoint,
                ShaderSourceType.Atomic("float"),
                [],
                new ShaderSourcePosition(request.source.assetPath, 1, 1)
            ),
            [],
            []
        );

    public void Dispose() => disposed++;
}
