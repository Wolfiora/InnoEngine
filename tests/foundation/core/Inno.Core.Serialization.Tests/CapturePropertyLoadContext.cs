using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Inno.Core.Serialization.Tests;

internal sealed class CapturePropertyLoadContext() : AssemblyLoadContext(isCollectible: true)
{
    protected override Assembly? Load(AssemblyName name)
        => Default.Assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name.Name);
}
