using Xunit;

namespace Inno.Tooling.Validation.Integration.Tests;

/// <summary>
/// Isolates the process console while observing the public validation entry's live progress.
/// </summary>
[CollectionDefinition(nameof(ValidationProgressCollection), DisableParallelization = true)]
public sealed class ValidationProgressCollection;
