# Inno.Editor.ImGui.Tests

[测试索引](README.md) · [Wiki 首页](../README.md) · [当前验收](../architecture/CURRENT_ACCEPTANCE.md)

## 职责与边界

验证以下项目的公开行为与组合契约。该程序集属于测试闭包，不进入 Editor 或 Player 发行；测试类、fixture 和断言辅助类型不属于稳定产品 API。

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)
- [Inno.Adapter.Presentation.ImGui.Sdl3](../backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)
- [Inno.Adapter.Platform.Sdl3](../backends/Sdl3/Inno.Adapter.Platform.Sdl3.md)
- [Inno.Native.Sdl3](../backends/Sdl3/Inno.Native.Sdl3.md)
- [Inno.Integration.WindowsX64.Sdl3](../platform/WindowsX64/Inno.Integration.WindowsX64.Sdl3.md)
- [Inno.Integration.MacOSArm64.Sdl3](../platform/MacOSArm64/Inno.Integration.MacOSArm64.Sdl3.md)
- [Inno.Editor.ImGui](../editor/Inno.Editor.ImGui.md)
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)

## 运行

从仓库根目录运行：

```powershell
dotnet test tests/editor/Inno.Editor.ImGui.Tests/Inno.Editor.ImGui.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

包含 Native 或窗口相关消费；执行前准备匹配目标与配置的原生产物。缺库、缺 SDK 或无法建立真实窗口属于未通过/未执行，不能记为通过。

可以用 `--filter FullyQualifiedName~方法名` 选择下表的场景；完整项目验证仍使用上面的命令。

## 源码场景

下表是当前测试文件索引，并列举各文件的部分入口，完整断言与参数化用例位于链接源码。列出场景不表示本轮已执行；结果只在当前验收中登记。

| 测试文件 | 场景入口示例 |
| --- | --- |
| [ImGuiInteractionOptionsTests.cs](../../tests/editor/Inno.Editor.ImGui.Tests/ImGuiInteractionOptionsTests.cs) | `ProductInteractionConventionsAreExplicitAndImmutable`、`WheelDirectionCannotSilentlyDisableOrAmplifyInput` |
| [ViewportMetricsTests.cs](../../tests/editor/Inno.Editor.ImGui.Tests/ViewportMetricsTests.cs) | `FrameRefreshesDrawableMetricsWithoutAPlatformScaleCallback`、`MetricRefreshPreservesRequestedViewportGeometry`、`DetachedWindowAppliesRequestsAfterPlatformEventReconciliation`、`LogicalDpiTransitionRetainsThePreviousScaleUntilImGuiStartsItsFrame` |
| [ViewportRetirementTests.cs](../../tests/editor/Inno.Editor.ImGui.Tests/ViewportRetirementTests.cs) | `FaultedRetirementRetainsWindowRendererAndApplicationOwners`、`DetachedWindowRemainsAliveUntilRenderingAcknowledgesRetirement`、`IncompleteDrainRetainsContextOwnershipAndCanFinishAtTheNextSafetyPoint`、`CreationFailureIsReportedAtAManagedSafetyPointAfterCleanup` |
| [WidgetLayoutTests.cs](../../tests/editor/Inno.Editor.ImGui.Tests/WidgetLayoutTests.cs) | `PropertyRows_WrapWithinTwoToThreeColumnsWithoutScrollbars`、`BoundedSelectors_OpenDownwardAndScrollOnlyWhenRequired`、`SelectorsNearWindowBottom_UseOnlyRemainingDownwardSpace`、`ContextMenu_SearchFillsItsActualContentWidthWithoutScrollbars` |

## 生命周期与失败

测试通过公开入口及可替换契约检查行为，fixture 只负责自身创建的会话、临时目录和子进程。需要窗口、设备或 owner thread 的测试应保持其声明的执行线程与释放顺序。取消或失败后检查相应断言与清理结果，不通过删除共享锁文件解除等待。

## 扩展与维护

新增行为用例放在最接近所属契约的测试文件。测试不能依赖反射穿透、`InternalsVisibleTo` 或扩大的生产可见性。修改契约后同时核对本项目及其直接生产依赖页面；测试中的具体类型不加入游戏脚本导出清单。

## 相邻页面

- [项目文件](../../tests/editor/Inno.Editor.ImGui.Tests/Inno.Editor.ImGui.Tests.csproj)：框架、引用和实际构建配置。
- [架构索引](../architecture/README.md)：所属层及禁止依赖。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：实际运行状态与环境限制。

## 明确宿主的 Native 验证

WindowsX64 与 MacOSArm64 窗口 fixture 分别使用真实 Win32/Cocoa 集成，不借用另一个平台的 surface。CI 通过 `tests/Invoke-IsolatedTests.ps1` 的 `Target` 和 `ProductProperties` 参数选择产品闭包；`ProcessIsolation=Required` 用例分别运行，避免污染第三方进程级状态。未准备产品时的缺库不是通过证据。

### 主线程与独立退休用例

macOS 的 SDL/viewport 用例由测试程序集的主线程进程入口执行，保留实际 SDL、ImGui 与可替换公开 renderer 断言。故障退休用例独立退出，不强制释放仍被保留的 owner；普通 xUnit worker 不承担 Cocoa 主线程职责。该验证不等于人工焦点、视觉或拖动观感验收。
