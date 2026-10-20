namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies one component-local root without exposing a generator-specific contract.
/// </summary>
/// <param name="id">
/// An ordinal identity scoped to this component's selected generation.
/// </param>
/// <param name="directory">
/// The checkout-relative or absolute external location; empty for generated output.
/// </param>
/// <param name="generated">
/// Whether this root denotes the selected native output directory.
/// </param>
/// <param name="frozen">
/// Whether source files must be copied before compilation.
/// </param>
public sealed record NativeBindingSourceRoot(
    string id,
    string directory,
    bool generated,
    bool frozen
);
