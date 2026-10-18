# Inno.Content.CacheProbe

[测试索引](README.md) · [Wiki 首页](../README.md) · [消费测试](Inno.Adapter.Content.FileSystem.Tests.md)

## 职责与边界

由内容缓存多进程测试启动的受控子进程，用于验证跨进程 reader、修复及退休顺序。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

父测试传入源目录与缓存目录。进程输出 ready，等待 prepare，然后取得 pinned.bin 的 ContentReadLease 与读取流；输出缓存 generation 后等待 release，最后读取原内容并退出。退出码 2/3 表示握手不匹配，1 表示异常，0 表示协议完成。默认 Solution 构建是库；测试宿主通过 InnoContentProbeExecutable=true 构建隔离的进程产物，不增加日常 IDE 启动项。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/Inno.Adapter.Content.FileSystem.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../tests/content/fixtures/Inno.Content.CacheProbe/Inno.Content.CacheProbe.csproj)：真实引用及构建配置。
- [消费测试](Inno.Adapter.Content.FileSystem.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：本轮实际运行结果；本页不声明测试已经通过。
