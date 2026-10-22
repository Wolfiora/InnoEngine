using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingInputPlanning
{
    internal static ValueTask<string> ComputeAsync(
        NativeBuildContext context,
        NativeBindingGenerationRequest request,
        CancellationToken cancellation,
        NativeBindingProfile profile
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(request);
        cancellation.ThrowIfCancellationRequested();
        var declarations = new List<string> { "binding-source-inputs", request.targetId, request.outputMode.ToString() };
        foreach (NativeComponentDescriptor component in request.components)
        {
            BindingGenerationConfiguration definition = BindingDefinitionReader.Read(context, component, request, profile);
            declarations.Add(NativeBindingGenerationIdentity.Compute(definition.Load(context), definition, context, cancellation));
        }
        return ValueTask.FromResult(NativeBuildFingerprint.Create(context, declarations, [], cancellation));
    }
}
