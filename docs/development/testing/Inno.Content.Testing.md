# Inno.Content.Testing

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [消费测试](Inno.Content.Tests.md)

## 职责与边界

为测试提供实现真实 IRuntimeContentStore 的内存内容源，以控制内容清单、有效载荷和损坏注入。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

ContentTestStore 构造时复制输入 bytes 并冻结长度与哈希。FromDirectory 将目录内容复制到内存，不保留磁盘路径。CorruptPayload 只替换后续读取体而保留原声明，支持完整性失败测试；此前打开的读取仍持有原 bytes。Dispose 清除 store 引用，调用者负责释放借出的 lease 与 stream。

## 公开测试契约

| API | 行为与所有权 |
| --- | --- |
| `ContentTestStore(payloads)` | 独立复制每个内容体，冻结索引及内容摘要；重复 key 按字典构造的规则失败。调用者拥有 store。 |
| `descriptor`、`index` | 返回构造时冻结的描述和清单；损坏注入不改写这些声明。 |
| `FromDirectory(directory)` | 读取目录全部文件并复制到内存；文件系统错误直接传出，返回的 store 不再依赖该目录。 |
| `CorruptPayload(key, bytes)` | 复制替换已有内容体，保留旧元数据；未知 key 抛出 `KeyNotFoundException`，释放后抛出 `ObjectDisposedException`。 |
| `Acquire(key)` | 返回调用者必须释放的 `ContentReadLease`；打开的 stream 同样由读取方释放。旧 lease 固定原 bytes，不随后续损坏注入变化。 |
| `Dispose()` | 使 store 不再接受读取或损坏注入，并释放其内容引用；已借出的 lease/stream 由原读取方负责。 |

这些是测试支持库的公开契约，仍执行签名覆盖；并非可安装的引擎插件 API。测试应串行协调此可变 store 的读写与释放。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/content/Inno.Content.Tests/Inno.Content.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../../tests/content/fixtures/Inno.Content.Testing/Inno.Content.Testing.csproj)：真实引用及构建配置。
- [消费测试](Inno.Content.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../../../.agents/records/acceptance/current.md)：本轮实际运行结果；本页不声明测试已经通过。

<!-- api: T:Inno.Content.Testing.ContentTestStore 7fac2dbd02b7a0da0a955fa296d0218ac8a2abbdbd7ad208da59ce8c0f7e116f -->
<!-- api: M:Inno.Content.Testing.ContentTestStore.#ctor(System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{Inno.Content.ContentKey,System.Byte[]}}) 750f42cb7bfcfc6fa9779605a4262afd83d2fe21dffe79ebebeac0803db23d76 -->
<!-- api: P:Inno.Content.Testing.ContentTestStore.descriptor 397256064169f0d551851facda4c3afbe904976bdd229fd3460b89e7906dfea8 -->
<!-- api: P:Inno.Content.Testing.ContentTestStore.index 3dde2eed367d49cff36c70d37d43d8f6b698ceded2791f92610b9deec8539fd6 -->
<!-- api: M:Inno.Content.Testing.ContentTestStore.FromDirectory(System.String) 5d5e0a0ba323ff3a978cd88d348a51a7651f201f984a16693e8756db9b66078b -->
<!-- api: M:Inno.Content.Testing.ContentTestStore.CorruptPayload(Inno.Content.ContentKey,System.ReadOnlySpan{System.Byte}) 4d0e67cc2a17c4cb8a012e205eea3469ee237d5ffbc8e63e70f101431cbfdb3d -->
<!-- api: M:Inno.Content.Testing.ContentTestStore.Acquire(Inno.Content.ContentKey) 12bd4bc76af23bc20fc839bbfc17e7a3fee6fafc8cd936b8bf0651f2c7962daf -->
<!-- api: M:Inno.Content.Testing.ContentTestStore.Dispose dd77bb3873d0d4546a2ed50432c0d8c77537e7f036aa0e70dfd27c1662d59d3e -->
