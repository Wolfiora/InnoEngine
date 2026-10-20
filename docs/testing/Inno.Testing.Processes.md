# Inno.Testing.Processes

[测试与验证](README.md) · [Wiki 首页](../README.md) · [进程宿主测试](Inno.Testing.Processes.Tests.md)

项目：`tests/process/Inno.Testing.Processes/Inno.Testing.Processes.csproj`。

## 职责与边界

仅供测试使用的 BCL 进程宿主。Native 窗口测试通过项目引用复用同一实现，生产程序集不依赖此项目。调用方负责先构建并部署真实契约程序集；宿主不生成绑定、不准备原生产品。

## 公开 API

| API | 行为与失败 |
| --- | --- |
| `NativeProcessTestHost.Run(string assembly, string contract, int timeoutMilliseconds = 90000)` | 在独立进程执行命名契约，同时收集 stdout/stderr。非零退出或输出无法排空时抛出 `InvalidOperationException`；超时抛出 `TimeoutException`，包含命令、PID、耗时、已捕获输出和清理结果。拒绝相对程序集路径、空契约和非正预算。 |

## 使用与生命周期

使用当前测试宿主的 `DOTNET_HOST_PATH`，未提供时使用 `dotnet`。默认执行预算 90 秒。超时终止进程树并最多等待 10 秒，随后最多等待 5 秒排空输出。成功返回前要求退出码为零且两条输出流均已关闭。

```csharp
using System.IO;
using Inno.Testing.Processes;

internal static class ContractRunner
{
    internal static void Run(string directory)
    {
        NativeProcessTestHost.Run(Path.Combine(directory, "ContractHost.dll"), "window-lifecycle");
    }
}
```

这是同步测试边界，不保存插件类型、代际对象或委托。GPU 和窗口线程仍由子进程中的实际契约负责。
