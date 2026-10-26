# NuGet 依赖还原与提交

[统一构建](README.md) · [Wiki 首页](../README.md) · [绑定生成](Inno.Build.Bindings.md)

## 依赖声明

工程通过标准 `PackageReference` 声明依赖，由 .NET SDK / NuGet 执行解析。BGCS 包族版本统一声明在 `build/msbuild/Inno.Packages.props`，各消费者使用精确版本范围；不引用 BGCS 源码。

普通类库、工具、测试与产品工程使用同一标准还原流程，不生成或提交独立依赖锁文件，不维护宿主、RID 或发布模式的清单命名规则。包的传递依赖、目标资产与 SDK 隐式工具包由 NuGet 解析。

## 日常开发

```powershell
dotnet restore InnoEngine.sln -p:Configuration=SourceDebug
dotnet build entry/InnoEngine.Editor/InnoEngine.Editor.csproj
```

普通 `dotnet build` 与 Rider Build 会自动执行需要的 Restore，不要求额外依赖准备脚本。默认 Debug/Release 的 Solution 还原面向两个开发入口；SourceDebug 包含完整源码、构建库及测试工程。

修改包版本后重新执行普通 Restore、构建和相关测试。实际还原结果属于 SDK 求值的 `ProjectAssetsFile`，一般位于 `obj`，也可以由明确的 `ArtifactsPath` 隔离；不提交这些中间文件。

## 产品与工具隔离

平台继续贡献真实 RID、目标 SDK 和发布模板。NativeAOT、裁剪与 Wasm 发布所需的隐式包由 SDK 选择，不能为了简化项目布局而剥除发布属性。

Task 私有还原与编译保持独立的 MSBuild 求值配置。托管输入冻结读取实际还原结果、包内容和 SDK reference pack；不从其他输出 owner 的中间文件推断依赖。

Native 第三方源码属于 `extern/`。BGCS 生成缓存、构建缓存、进程协调 lease 和产品读取保护各自保持原有 owner，与 NuGet 依赖声明分离。

## CI

[`nuget-dependencies.yml`](../../.github/workflows/nuget-dependencies.yml) 使用仓库声明的 SDK 执行普通 Solution Restore，并保留日志和 binlog。push 与手动运行可以提交 SDK 实际解析的依赖 snapshot；PR 不执行写入步骤。

Engine CI 分别验证托管源码、Editor Debug/Release 和实际 NativeAOT RID 的普通还原。托管测试后检查 tracked 与 untracked 源码变化。包身份回归通过真实 SDK Restore，核对声明的 BGCS 版本、归档 SHA-512 与 SDK 元数据；assets 中的 NuGet 内容身份与 `.nupkg.metadata` 对照，不与包含签名的 ZIP 原始哈希混淆。

这种方式使用标准 NuGet 依赖解析，不承诺冻结所有未来包源状态；SDK 基线、包版本声明、实际解析身份和构建测试共同作为当前可验证输入。

## 失败定位

- 包不可取得：检查声明版本、NuGet source 与网络，不回退到源码项目。
- SDK 或 targeting pack 缺失：安装工程声明的 SDK / workload，不从其他已安装版本猜测替代。
- 默认入口要求 Wasm workload：检查产品属性是否泄漏到工具或共同库。
- snapshot 提交失败：检查 GitHub 权限和服务返回；本地还原成功不表示远端提交成功。

当前检查范围见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
