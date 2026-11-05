# Inno.Extensibility.Modules.TestModule.Invalid

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [消费测试](Inno.Extensibility.Modules.Tests.md)

## 职责与边界

故意包含冲突 Stable Type ID 的无效模块，验证候选准备失败不会破坏当前可用 generation。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

FirstConflictingExtension 与 SecondConflictingExtension 声明相同 Stable Type ID。冲突应在候选注册阶段拒绝；该 fixture 的预期用途就是触发失败，不应修正成合法产品。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/extensibility/Inno.Extensibility.Modules.Tests/Inno.Extensibility.Modules.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../../tests/extensibility/fixtures/Inno.Extensibility.Modules.TestModule.Invalid/Inno.Extensibility.Modules.TestModule.Invalid.csproj)：真实引用及构建配置。
- [消费测试](Inno.Extensibility.Modules.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../../../.agents/records/acceptance/current.md)：本轮实际运行结果；本页不声明测试已经通过。

<!-- api: T:Inno.Extensibility.Modules.TestModule.FirstConflictingExtension 20c98b7e8918fb3dfe898ca49022cd80023c808a4a453a3baaae1c3739e9d324 -->
<!-- api: M:Inno.Extensibility.Modules.TestModule.FirstConflictingExtension.#ctor 6ce374dcae268a9c4317ba352189ae5e75a78e22196cf03428ee81aa1a8101c7 -->
<!-- api: T:Inno.Extensibility.Modules.TestModule.SecondConflictingExtension ab5c608cf0a278a97dfae7368d17c25e62b6ee1d35b086a4d110c4d767644f03 -->
<!-- api: M:Inno.Extensibility.Modules.TestModule.SecondConflictingExtension.#ctor 950509cc73cbc4cb5a99a49fb028b9e74aa7bfa7f4975b941d4b04eda0e2a6c8 -->
