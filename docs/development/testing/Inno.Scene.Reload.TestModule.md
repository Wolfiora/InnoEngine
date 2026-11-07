# Inno.Scene.Reload.TestModule

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [消费测试](Inno.Scene.Tests.md)

## 职责与边界

提供可回收 Scene Component 与 System，验证 Missing 状态保留、恢复和恢复失败。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

Component 保存 value、asset 和 rejectRestore。rejectRestore 为 true 时，OnSerializableRestored 回调抛出异常，检验恢复事务不能部分覆盖当前状态。System 的 value 同样通过 SerializableProperty 持久化；两种类型各有独立 Stable Type ID。

公开测试协议包括 `ReloadableSceneComponent.value : int`、`asset : TextAsset?`、`rejectRestore : bool` 及 `ReloadableSceneSystem.value : int`，均可读写。`asset` 是场景序列化中的资产引用，不转移资产释放责任；`rejectRestore` 默认为 false，恢复验证失败抛出 `InvalidOperationException`。调用方应通过真实场景 session 管理 component/system 的生命周期，而不是在恢复后继续持有旧代 CLR 对象。类型与属性的公开签名同样保持覆盖。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/scene/Inno.Scene.Tests/Inno.Scene.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../../tests/scene/fixtures/Inno.Scene.Reload.TestModule/Inno.Scene.Reload.TestModule.csproj)：真实引用及构建配置。
- [消费测试](Inno.Scene.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../../../.agents/records/acceptance/current.md)：本轮实际运行结果；本页不声明测试已经通过。

<!-- api: T:Inno.Scene.Reload.TestModule.ReloadableSceneComponent 6ffd61c58ffbc074b1ca6ec4d4cfb655e917f8dfb014e48a24a3e0e35537ebb8 -->
<!-- api: P:Inno.Scene.Reload.TestModule.ReloadableSceneComponent.value 4728696bccab2e5cf10044587e4a89202a356b63b5f53a28db0b10ef04f5dae7 -->
<!-- api: P:Inno.Scene.Reload.TestModule.ReloadableSceneComponent.asset 3fc367d5e615f15f52238b7f1a595caecb8c5e5754b45e9bef0fd99758bbc5de -->
<!-- api: P:Inno.Scene.Reload.TestModule.ReloadableSceneComponent.rejectRestore 6ae794e4a6a5c0f8e49400b64ce766d8b8b12ce39816211048b0fdc2888f40cd -->
<!-- api: M:Inno.Scene.Reload.TestModule.ReloadableSceneComponent.#ctor 272b7e1934d74f97fe894fbcce2ffc41fb0b7665688a5cd3d5017990d25d0ae9 -->
<!-- api: T:Inno.Scene.Reload.TestModule.ReloadableSceneSystem e2d4ddb21ed77e9af2f20f506db965b4f7a3b89057a082917c02ab3899b9a78d -->
<!-- api: P:Inno.Scene.Reload.TestModule.ReloadableSceneSystem.value 2997be094ea460a2cb8590e9d787ad7958c22e9ff67b64cb3a927896a373f57e -->
<!-- api: M:Inno.Scene.Reload.TestModule.ReloadableSceneSystem.#ctor 040d2449a905033a1b9a84cb56724cb4c1bfe8236b8166b3a5e2ba60a136cd44 -->
