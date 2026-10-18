# Inno.Content.Testing

[测试索引](README.md) · [Wiki 首页](../README.md) · [消费测试](Inno.Content.Tests.md)

## 职责与边界

为测试提供实现真实 IRuntimeContentStore 的内存内容源，以控制内容清单、有效载荷和损坏注入。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

ContentTestStore 构造时复制输入 bytes 并冻结长度与哈希。FromDirectory 将目录内容复制到内存，不保留磁盘路径。CorruptPayload 只替换后续读取体而保留原声明，支持完整性失败测试；此前打开的读取仍持有原 bytes。Dispose 清除 store 引用，调用者负责释放借出的 lease 与 stream。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/content/Inno.Content.Tests/Inno.Content.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../tests/content/fixtures/Inno.Content.Testing/Inno.Content.Testing.csproj)：真实引用及构建配置。
- [消费测试](Inno.Content.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../architecture/CURRENT_ACCEPTANCE.md)：本轮实际运行结果；本页不声明测试已经通过。
