# Inno.Assets.Pipeline.DuplicateImporterFixture

[测试与 fixture 索引](README.md)

[测试索引](README.md) · [Wiki 首页](../../README.md) · [消费测试](Inno.Assets.Pipeline.Tests.md)

## 职责与边界

两个 importer 故意声明相同的稳定扩展 ID，支持候选注册冲突与原 registry 保留测试。它不是可安装的 importer 产品。

本项目是验证 fixture，没有独立 xUnit 用例；其 public 类型是测试数据协议，不是引擎扩展 API，也不进入产品发行或脚本导出清单。

## 实际行为

DuplicateImporterA 与 DuplicateImporterB 使用不同文件扩展名，但同一个 AssetImporter ID；不能通过扩展名差异接受重复语义身份。

`DuplicateImporterAsset` 是具有稳定类型 ID 的无附加字段资产。两个 importer 的 `supportedExtensions` 分别返回 `.duplicateid` 与 `.duplicateid-b`；受保护的 `ImportAsync(context, output, cancellationToken)` 创建该资产交给借入的 output，并将 `context.sourceBytes` 写成 `runtime` artifact。取消与写入失败由真实 writer 传播。消费测试必须在注册阶段拒绝冲突，不能把成功执行 ImportAsync 作为重复 ID 合法的证据。公开／protected 签名继续纳入文档覆盖。

## 运行与初始化

由消费测试负责构建、加载或启动，不对 fixture 项目运行 dotnet test 作为验证。仓库根目录执行：

```powershell
dotnet test tests/assets/Inno.Assets.Pipeline.Tests/Inno.Assets.Pipeline.Tests.csproj --configuration Debug --disable-build-servers -m:1 -nodeReuse:false
```

## 所有权与失败

消费测试拥有临时目录、模块或子进程，并在失败、取消或测试结束后释放。故意无效的候选必须被拒绝；不能将其异常当成环境问题绕过，也不能让 fixture 对象进入生产静态缓存。有关 reload 的测试仍需完成完整 GC barrier 及弱引用检查。

## 相邻页面

- [项目文件](../../../tests/assets/fixtures/Inno.Assets.Pipeline.DuplicateImporterFixture/Inno.Assets.Pipeline.DuplicateImporterFixture.csproj)：真实引用及构建配置。
- [消费测试](Inno.Assets.Pipeline.Tests.md)：断言、执行环境与场景入口。
- [当前验收](../../../.agents/records/acceptance/current.md)：本轮实际运行结果；本页不声明测试已经通过。

<!-- api: T:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterAsset 9127ec564cd88b446fd28b023229cf3cb43780bd32bf8a60e8658745571f7d8f -->
<!-- api: M:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterAsset.#ctor f7792fe43a38d65dbe79167242d98891e8d11b762311dd10449297faf6feaa25 -->
<!-- api: T:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterA 05f3fe076c8e4bf2bd012da83360d2cec7c3614c357b0021591f3cee9974f154 -->
<!-- api: P:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterA.supportedExtensions 83b34452fb500846beacac5bf173163dd5dba0ac8ed1381ae51664383d008031 -->
<!-- api: M:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterA.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterAsset},System.Threading.CancellationToken) 10c9fa3779605279d89bfcb5977b530650557bb6ad3cd64e6dc9738060a570b7 -->
<!-- api: M:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterA.#ctor 127d132f1d1f6394537f91e7ccabc487339ea4a2e9c92bf0c7e62db99a88ce6b -->
<!-- api: T:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterB 4017e4a99f4e8be41182bcc7c9040cc5dbdd230711af5e9780d1c6e4ed3cc9f0 -->
<!-- api: P:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterB.supportedExtensions 663ce49d83c8d6ea31fcdf206966951926bfad5f2d4b9ccd0a4495ae8a4aae5d -->
<!-- api: M:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterB.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterAsset},System.Threading.CancellationToken) d3001f5504dfc2591adf18ca94842e417cbe62294e801e025835b35cab319d94 -->
<!-- api: M:Inno.Assets.Pipeline.DuplicateImporterFixture.DuplicateImporterB.#ctor e07700b16d5756947748d39aecb4b6296fe244b7ccaac497c590c6e6005d8a60 -->
