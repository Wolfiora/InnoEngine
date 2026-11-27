namespace Inno.Build.Composition;

/// <summary>
/// Creates a platform packager without borrowing authoring or content compiler services.
/// </summary>
/// <remarks>
/// The build distribution explicitly registers target factories and the required toolchains. Installing
/// a runtime module does not select a new publication target.
/// </remarks>
/// <returns>
/// A non-null packaging implementation retained by the pipeline without a separate lifetime.
/// </returns>
public delegate IGameBuildTarget BuildTargetFactory();
