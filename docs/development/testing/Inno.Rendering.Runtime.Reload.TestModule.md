# Inno.Rendering.Runtime.Reload.TestModule

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [消费测试](Inno.Rendering.Runtime.Tests.md)

## 职责与边界

提供可回收程序集中的 Pipeline、Feature、frame payload 与 contributor，验证渲染帧和 reload 的引用退休。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

ReloadableContributor 可在 PrepareFrame 注销自身，并在 AddRenderPasses 加入捕获实例的回调后主动失败。测试据此检查快照切换、失败图回滚和旧 ALC 释放；ReloadableFramePayload 则用于检查帧数据队列是否残留旧代对象。

## 公开测试契约

`ReloadableFramePayload` 是无附加状态的可回收对象。`ReloadableContributor(unregister, rejectGraph)` 借用宿主注销回调；`PrepareFrame(frameIndex)` 调用该回调注销当前对象，`AddRenderPasses(graph, frameIndex)` 向借入的 graph 添加真实有副作用的 pass。`rejectGraph=true` 时添加后抛出 `InvalidOperationException`，让消费测试验证完整回滚。回调闭包持有当前实例，必须随失败图或退休帧释放，不能跨 generation 存入静态引用。

这些测试协议仍保留公开签名映射；内部 pipeline/feature 的实现由上面的行为说明解释，不把它们提升为公开产品承诺。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/rendering/Inno.Rendering.Runtime.Tests/Inno.Rendering.Runtime.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../../tests/rendering/fixtures/Inno.Rendering.Runtime.Reload.TestModule/Inno.Rendering.Runtime.Reload.TestModule.csproj)：真实引用及构建配置。
- [消费测试](Inno.Rendering.Runtime.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../../../.agents/records/acceptance/current.md)：本轮实际运行结果；本页不声明测试已经通过。

<!-- api: T:Inno.Rendering.Runtime.Reload.TestModule.ReloadableContributor 873467654e9292db29f090ab2e0bef30c92a6cd0845c3afdc343ab89cb833bb8 -->
<!-- api: M:Inno.Rendering.Runtime.Reload.TestModule.ReloadableContributor.#ctor(System.Action{Inno.Rendering.IRenderFrameGraphContributor},System.Boolean) 676cfc497fc375ca5490e3e352cff23a4b19096ebb1c2a5dc7e1c2300ab6ff07 -->
<!-- api: M:Inno.Rendering.Runtime.Reload.TestModule.ReloadableContributor.PrepareFrame(System.UInt64) 4a5cd61cb7e21f1d257044768897bc63d0a62ffc4c649ea3c7d650be14e1a50c -->
<!-- api: M:Inno.Rendering.Runtime.Reload.TestModule.ReloadableContributor.AddRenderPasses(Inno.Rendering.RenderGraphBuilder,System.UInt64) 315674d4ab869b98898a2de2ec287c6b8cf3a756be7d1959b2417fc6c3f96c38 -->
<!-- api: T:Inno.Rendering.Runtime.Reload.TestModule.ReloadableFramePayload 2917615b6176105309cda8a912b0c6dc699f2163799d89179ba5d2413d147ad0 -->
<!-- api: M:Inno.Rendering.Runtime.Reload.TestModule.ReloadableFramePayload.#ctor dca3e4a35e23562cfbbf9721a4a2c334d842e026e1f5757a28ef1075a43f87cf -->
