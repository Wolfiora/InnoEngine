namespace Inno.Build.Composition;

/// <summary>
/// Creates a platform packager without borrowing authoring or content compiler services.
/// </summary>
/// <returns>
/// A non-null packaging implementation retained by the pipeline without a separate lifetime.
/// </returns>
public delegate IGameBuildTarget BuildTargetFactory();
