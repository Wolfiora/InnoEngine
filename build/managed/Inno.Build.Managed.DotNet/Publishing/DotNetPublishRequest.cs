using System.Collections.Generic;
using Inno.Build.Managed;
using Inno.Build.Toolchains;

namespace Inno.Build.Managed.DotNet;

internal sealed record DotNetPublishRequest(
    DotNetSdkDescriptor sdk,
    ManagedDeploymentId deployment,
    ManagedDeploymentRequest publication,
    IReadOnlyList<string> properties
);
