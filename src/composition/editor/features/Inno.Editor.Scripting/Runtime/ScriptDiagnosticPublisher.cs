using System;
using System.Linq;
using System.Collections.Generic;
using Inno.Extensibility.Types;

using Inno.Core.Diagnostics;
using Inno.Scripting.Compiler;

namespace Inno.Editor.Scripting;

internal static class ScriptDiagnosticPublisher
{
    private const string C_COMPILER_DIAGNOSTICS = "Script Compiler";
    private const string C_IDE_PROJECTION_DIAGNOSTICS = "Script IDE Projection";
    private const string C_RELOAD_DIAGNOSTICS = "Script Reload";
    private const string C_UNLOAD_DIAGNOSTICS = "Script Unload";

    internal static void PublishCompilation(ScriptCompilationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        Diagnostics.Set(
            C_COMPILER_DIAGNOSTICS,
            result.diagnostics.Select(CreateCompilationDiagnostic));
    }

    internal static void PublishReloadFailure(
        Exception exception,
        ScriptCompilationResult? candidate
    ) {
        ArgumentNullException.ThrowIfNull(exception);
        Diagnostics.Set(
            C_RELOAD_DIAGNOSTICS,
            GetReloadDiagnostics(exception, candidate));
    }

    internal static void PublishIdeProjectionFailure(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Diagnostics.Set(
            C_IDE_PROJECTION_DIAGNOSTICS,
            Diagnostic.Warning("INNO-IDE-PROJECTION", exception.ToString()));
    }

    internal static void PublishUnloadFailure(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        Diagnostics.Set(
            C_UNLOAD_DIAGNOSTICS,
            Diagnostic.Error("INNO-ALC-UNLOAD", exception.Message));
    }

    internal static void ClearReload() => Diagnostics.Clear(C_RELOAD_DIAGNOSTICS);

    internal static void ClearIdeProjection() => Diagnostics.Clear(C_IDE_PROJECTION_DIAGNOSTICS);

    internal static void ClearUnload() => Diagnostics.Clear(C_UNLOAD_DIAGNOSTICS);

    internal static void ClearAll()
    {
        Diagnostics.Clear(C_COMPILER_DIAGNOSTICS);
        Diagnostics.Clear(C_IDE_PROJECTION_DIAGNOSTICS);
        Diagnostics.Clear(C_RELOAD_DIAGNOSTICS);
        Diagnostics.Clear(C_UNLOAD_DIAGNOSTICS);
    }

    private static IEnumerable<Diagnostic> GetReloadDiagnostics(
        Exception exception,
        ScriptCompilationResult? candidate
    ) {
        yield return Diagnostic.Error("INNO-RELOAD", exception.ToString());
        var pending = new Stack<Exception>();
        pending.Push(exception);
        while (pending.TryPop(out Exception? failure))
        {
            if (failure is ExtensionActivationException activation)
            {
                ScriptTypeSource? source = candidate?.typeSources.FirstOrDefault(declaration =>
                    (declaration.typeId is null || declaration.typeId == activation.typeId)
                    && string.Equals(declaration.assemblyName, activation.assemblyName, StringComparison.Ordinal)
                    && string.Equals(declaration.typeName, activation.typeName, StringComparison.Ordinal));
                DiagnosticLocation? location = source is null
                    ? null
                    : new DiagnosticLocation(source.sourcePath, source.line, source.column);
                yield return new Diagnostic(
                    "INNO-EXTENSION-ACTIVATION",
                    activation.ToString(),
                    DiagnosticSeverity.Error,
                    location: location,
                    semanticId: activation.typeId.ToString("D"));
                if (source is not null && candidate is not null)
                {
                    ScriptModuleDeployment? module = candidate.moduleDeployments.FirstOrDefault(
                        deployment => deployment.assemblyScopes.ContainsKey(activation.assemblyName));
                    foreach (ScriptTypeSource affected in candidate.typeSources)
                    {
                        if (affected.typeId is not Guid affectedTypeId || affectedTypeId == activation.typeId
                            || module is null || !module.assemblyScopes.ContainsKey(affected.assemblyName))
                            continue;
                        yield return new Diagnostic(
                            "INNO-CANDIDATE-REJECTED",
                            $"The candidate module '{module.moduleName}' containing '{affected.typeName}' was not activated because extension '{activation.typeName}' failed: {activation.InnerException?.Message}",
                            DiagnosticSeverity.Error,
                            location: new DiagnosticLocation(affected.sourcePath, affected.line, affected.column),
                            semanticId: affectedTypeId.ToString("D"));
                    }
                }
            }
            if (failure is AggregateException aggregate)
            {
                foreach (Exception inner in aggregate.InnerExceptions)
                    pending.Push(inner);
            }
            else if (failure.InnerException is not null)
                pending.Push(failure.InnerException);
        }
    }

    private static Diagnostic CreateCompilationDiagnostic(ScriptDiagnostic diagnostic)
    {
        DiagnosticLocation? location = string.IsNullOrWhiteSpace(diagnostic.filePath)
            ? null
            : new DiagnosticLocation(
                diagnostic.filePath,
                diagnostic.line,
                diagnostic.column);
        return diagnostic.severity switch
        {
            DiagnosticSeverity.Info => Diagnostic.Info(
                diagnostic.id,
                diagnostic.message,
                location),
            DiagnosticSeverity.Warning => Diagnostic.Warning(
                diagnostic.id,
                diagnostic.message,
                location),
            DiagnosticSeverity.Error => Diagnostic.Error(
                diagnostic.id,
                diagnostic.message,
                location),
            _ => Diagnostic.Error(
                diagnostic.id,
                diagnostic.message,
                location)
        };
    }

}
