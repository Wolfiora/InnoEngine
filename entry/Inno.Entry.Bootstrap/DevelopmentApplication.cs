using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Inno.Entry.Bootstrap;

internal sealed class DevelopmentApplication : IDisposable
{
    private readonly AssemblyDependencyResolver m_resolver;

    internal DevelopmentApplication(string assemblyPath)
    {
        m_resolver = new AssemblyDependencyResolver(assemblyPath);
        AssemblyLoadContext.Default.Resolving += Resolve;
        AssemblyLoadContext.Default.ResolvingUnmanagedDll += ResolveNative;
        try
        {
            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    internal Assembly assembly { get; }

    internal T GetEntry<T>(string typeName) where T : Delegate
    {
        Type type = assembly.GetType(typeName, throwOnError: true)!;
        MethodInfo method = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static)
            ?? throw new MissingMethodException(typeName, "Run");
        return method.CreateDelegate<T>();
    }

    /// <summary>
    /// Detaches resolution callbacks after the borrowed application has completed its shutdown.
    /// </summary>
    public void Dispose()
    {
        AssemblyLoadContext.Default.Resolving -= Resolve;
        AssemblyLoadContext.Default.ResolvingUnmanagedDll -= ResolveNative;
    }

    private Assembly? Resolve(
        AssemblyLoadContext context,
        AssemblyName name
    ) {
        string? path = m_resolver.ResolveAssemblyToPath(name);
        return path is null ? null : context.LoadFromAssemblyPath(path);
    }

    private nint ResolveNative(
        Assembly assembly,
        string name
    ) {
        string? path = m_resolver.ResolveUnmanagedDllToPath(name);
        return path is null ? 0 : System.Runtime.InteropServices.NativeLibrary.Load(path);
    }
}
