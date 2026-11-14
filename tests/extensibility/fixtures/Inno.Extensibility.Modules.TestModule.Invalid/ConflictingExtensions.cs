using Inno.Extensibility.Types;

namespace Inno.Extensibility.Modules.TestModule;

/// <summary>
/// Declares the first half of an intentional stable-type-identity collision in a rejected candidate module.
/// </summary>
[StableTypeId("44a4cda2-a03e-4918-8db2-f37048a9e4f1")]
public sealed class FirstConflictingExtension;

/// <summary>
/// Repeats the first declaration's identity so candidate admission must preserve the previous valid generation.
/// </summary>
[StableTypeId("44a4cda2-a03e-4918-8db2-f37048a9e4f1")]
public sealed class SecondConflictingExtension;
