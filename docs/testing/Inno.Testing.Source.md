# Inno.Testing.Source

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

项目：`tests/source/Inno.Testing.Source/Inno.Testing.Source.csproj`。

## 职责与边界

为需要真实源码和 SDK 输出的测试解析显式输入，不进入生产或发布闭包。消费项目通过 `tests/RepositoryInput.targets` 声明 checkout metadata 和项目引用，不重复编译共享 C# 文件。输出 owner 在 SDK 完成路径求值后、生成程序集属性前记录，涵盖显式 `ArtifactsPath` 与 `UseArtifactsOutput`；普通输出不伪造独立 owner。

## 公开 API

| API | 语义与失败 |
| --- | --- |
| `TestRepository.GetRoot(Assembly testAssembly)` | 返回所属测试程序集明确声明的绝对 checkout。空程序集抛出 `ArgumentNullException`；缺失、重复、相对路径或没有引擎 Solution 标记的声明抛出 `InvalidOperationException`。不猜测执行目录、进程当前目录或其他 checkout。 |
| `TestRepository.GetArtifactsPath(Assembly testAssembly)` | 返回编译该测试程序集时 SDK 选定的绝对输出 owner；没有独立输出声明时返回 null，继续使用正常 SDK 输出求值。空程序集、重复、空或相对声明明确失败，不从旧 bin/obj 猜测。 |

## 使用与生命周期

调用方显式提供自己的程序集。该方法只读取公开 metadata 和文件标记，不保存 Assembly、Type、实例或委托，不创建目录或修改环境。

```csharp
using System.IO;
using Inno.Testing.Source;

internal static class SourceInputs
{
    internal static bool HasSolution()
    {
        string root = TestRepository.GetRoot(typeof(SourceInputs).Assembly);
        return File.Exists(Path.Combine(root, "InnoEngine.sln"));
    }
}
```

示例所在测试工程必须导入 `tests/RepositoryInput.targets`。直接复制测试 DLL 到另一目录不改变它声明的源码输入；源码已不可用时测试应明确失败。

## 相邻页面

- [进程测试宿主](Inno.Testing.Processes.md)：隔离进程与超时、输出排空。
- [构建契约测试](Inno.Build.Tests.md)：SDK、生成、发布和源码 owner。
