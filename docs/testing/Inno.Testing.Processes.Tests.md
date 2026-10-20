# Inno.Testing.Processes.Tests

[测试与验证](README.md) · [Wiki 首页](../README.md) · [进程宿主](Inno.Testing.Processes.md)

项目：`tests/process/Inno.Testing.Processes.Tests/Inno.Testing.Processes.Tests.csproj`。

## 职责、依赖与入口

通过 `NativeProcessTestHost.Run` 的公开契约验证子进程执行。工程引用 `Inno.Testing.Processes`，保留 xUnit/VSTest；内置可执行 fixture 仅供此测试使用，不是开发启动入口。

```powershell
dotnet test tests/process/Inno.Testing.Processes.Tests/Inno.Testing.Processes.Tests.csproj --configuration Release
```

## 覆盖范围

- 成功退出前完整读取两条输出流。
- 非零退出保留 stdout、stderr、命令和契约标识。
- 超时保留部分输出，并终止子孙进程。
- 拒绝相对程序集路径及无效执行预算。

测试不依赖 BGCS、GPU 或原生产品，不访问内部成员。fixture 的入口为内部实现，无稳定公开领域 API。实际执行结果由当前验收记录。
