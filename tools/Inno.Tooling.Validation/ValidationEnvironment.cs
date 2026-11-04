using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Validation;

internal static class ValidationEnvironment
{
    internal static async Task<string> CaptureAsync(
        string root,
        string dotnet,
        CancellationToken cancellation
    ) {
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(dotnet, root, cancellation).ConfigureAwait(false);
        var inputs = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["os"] = RuntimeInformation.OSDescription,
            ["architecture"] = RuntimeInformation.ProcessArchitecture.ToString(),
            ["sdk"] = sdk.sdkIdentity,
            ["host"] = sdk.hostPath
        };
        foreach (string name in new[] { "MSBuildSDKsPath", "DOTNET_ROLL_FORWARD", "DOTNET_ROOT", "NUGET_PACKAGES", "CC", "CXX", "CMAKE_GENERATOR", "SDKROOT", "MACOSX_DEPLOYMENT_TARGET" })
            inputs.Add(name, Environment.GetEnvironmentVariable(name) ?? string.Empty);
        return ValidationInputs.Identity(inputs);
    }
}
