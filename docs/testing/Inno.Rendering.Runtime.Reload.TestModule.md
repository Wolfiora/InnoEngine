# Inno.Rendering.Runtime.Reload.TestModule

[测试索引](README.md) · [Wiki 首页](../README.md) · [消费测试](Inno.Rendering.Runtime.Tests.md)

## 职责与边界

提供可回收程序集中的 Pipeline、Feature、frame payload 与 contributor，验证渲染帧和 reload 的引用退休。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

ReloadableContributor 可在 PrepareFrame 注销自身，并在 AddRenderPasses 加入捕获实例的回调后主动失败。测试据此检查快照切换、失败图回滚和旧 ALC 释放；ReloadableFramePayload 则用于检查帧数据队列是否残留旧代对象。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/rendering/Inno.Rendering.Runtime.Tests/Inno.Rendering.Runtime.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../tests/rendering/fixtures/Inno.Rendering.Runtime.Reload.TestModule/Inno.Rendering.Runtime.Reload.TestModule.csproj)：真实引用及构建配置。
- [消费测试](Inno.Rendering.Runtime.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：本轮实际运行结果；本页不声明测试已经通过。
