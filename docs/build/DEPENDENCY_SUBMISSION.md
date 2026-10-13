# NuGet 依赖图提交

[统一构建](README.md) · [Wiki 首页](../README.md) · [绑定生成](Inno.Build.Bindings.md)

## 当前依赖来源

BGCS 生成器与 Runtime 完全由 NuGet 恢复。`build/msbuild/Inno.Packages.props` 固定包族版本，项目的 `packages.lock.json` 保存解析版本和内容哈希。引擎不要求 BGCS 源码仓库。

Native 第三方源码属于 `extern/`，不是 NuGet 依赖图的根。Browser Player 的 Wasm workload 由实际发布工程使用，默认开发入口还原不应要求它。

## 工作流

[`nuget-dependencies.yml`](../../.github/workflows/nuget-dependencies.yml) 使用引擎 SDK，对 `SourceDebug` 图执行 locked restore。默认 Debug/Release 仅还原两个开发入口；SourceDebug 才包含中立源码、构建库和测试。

```powershell
 dotnet restore InnoEngine.sln -p:Configuration=SourceDebug --locked-mode
```

PR 检查还原；push 和手动运行提交解析后的依赖 snapshot。提交权限仅属于该步骤，fork PR 不执行写入步骤。工作流不发布包，不编译 Native，不代表产品运行验收通过。

## 失败定位

- `NU1004`：项目声明与锁文件不一致。更新真实项目图的锁文件后重新 locked restore。
- 包不可取得：核对精确版本、NuGet source 和网络；不回退到源码项目。
- 默认入口要求 Wasm workload：核对产品属性是否泄漏到工具或共同库。
- snapshot 提交失败：查看 GitHub token 权限和服务返回；本地还原成功不能证明远端提交成功。

仓库设置和远端 workflow 状态单独检查，当前结果见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。
