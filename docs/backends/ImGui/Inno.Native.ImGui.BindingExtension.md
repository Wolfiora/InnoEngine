# Inno.Native.ImGui.BindingExtension

[分类索引](README.md) · [Wiki 首页](../../README.md) · [ImGui](Inno.Native.ImGui.md) · [绑定生成](../../native/BindingGeneration.md)

## 职责、依赖与输出

此库属于 Inno.Native.ImGui 的 `Bindings/Extension`，提供消费方自己的 managed ABI layout。
它引用独立 BGCS 生成契约，不要求 BGCS 了解引擎。其宿主输出属于 `artifacts/build-tools/bindings`，
不会作为运行时 Player 依赖。两个模板属于同一组件的 `Managed` 目录并随扩展库部署。
此项目在 Solution 的 `native` 分组中，与其所属原生组件一起维护；没有运行时入站引用并不代表它是废弃项目。

## 全部公开 API

| 类型/成员 | 行为 |
| --- | --- |
| `ImGuiBindingPlugin` | 实现 BGCS 的 `IBindingPlugin` 与 `ICacheFingerprintProvider`。 |
| `id` | 当前 cimgui layout 扩展的唯一注册身份。 |
| `version` | 实现修订信息，参与生成缓存身份。 |
| `contractVersion` | 当前 BGCS 扩展 ABI 握手，拒绝不匹配的插件。 |
| `Configure(IBindingPluginHost)` | 向候选 host 注册一个 IBindingEmitter；不修改全局生成器。 |
| `GetCacheFingerprint()` | 对 ImTextureID 与 ImVector 模板内容生成散列；缺失模板明确失败。 |

没有 protected 扩展点。内部 emitter 不是稳定公开 API。

## 工作流、所有权与失败

```csharp
using Inno.Native.ImGui.Bindings.Extension;

string fingerprint = new ImGuiBindingPlugin().GetCacheFingerprint();
```

BGCS 创建本次生成的候选 registry 与输出 staging；扩展只向指定输出写入组件模板，
由同一 single-file composer 合并到 Bindings.cs。它要求单文件输出；缺失模板、无效路径或输出约束
都会使请求失败。只有全部生成和校验成功，GenerateBindingsTask 才提交 native/managed bundle。

模板变化使 fingerprint 失效，不同 native target 与 fingerprint 仍各自拥有输出。
插件实例、emitter 和模板文件句柄不得进入 Player 或 Editor extension generation。
验证使用 `Inno.Build.Cli verify-native` 的重生成、比较及绑定编译流程。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/Inno.Native.ImGui.BindingExtension.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin`

<!-- api: T:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin 22cd2044e92b4e5a8b047c35b3e864e17cdea28bb14310d830c986737b4213a2 -->
<!-- api: P:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.id fd1e7e0736b7b5fc11a13819de8fa061a45ad3d78cf679e6faa466efa3a83ebf -->
<!-- api: P:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.version 86c1de4b5035861f59cca45ef6cba348644a201860a0cd012dd0502581aaf15e -->
<!-- api: P:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.contractVersion f4e5cebd20f78c71b51795749d1bae3b64a6aa35d969d50d763959a658cc3ce7 -->
<!-- api: M:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.Configure(BGCS.Core.Extensibility.IBindingPluginHost) f398452638eca3da5f35ec787d002ce7ae6141e4d59e54d54036fee7f4fb62ca -->
<!-- api: M:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.GetCacheFingerprint d6e03485b98b07fe70f66d4d2e0f5fbdaceec8746d199aee901846245bc693fa -->
<!-- api: M:Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.#ctor b8005a05e4874fbbafbb6dca6a8efc856545b5d6cdd0fa0b736e0f441f9d34ef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6efd1181bbabf400"></a>[`public Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.ImGuiBindingPlugin()`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-c8f0c28caa20cbaf"></a>[`public void Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.Configure(BGCS.Core.Extensibility.IBindingPluginHost host)`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L40) | Registers the managed-layout emitter with the BGCS host.<br>`host`：The BGCS plugin service registry. | — | — |
| <a id="api-9b278cc7a06beac8"></a>[`public string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.GetCacheFingerprint()`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L49) | Fingerprints source templates so edits invalidate incremental binding output.<br>返回：A deterministic hash sequence for the two managed layout templates. | — | — |
| <a id="api-cfa42bd49211e316"></a>[`public int Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.contractVersion { get; }`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L32) | Gets the BGCS plugin protocol version implemented by this extension. | — | — |
| <a id="api-3b9391cc139b3794"></a>[`public string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.id { get; }`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L22) | Identifies the InnoEngine cimgui layout extension. | — | — |
| <a id="api-dba1e971e034ab8c"></a>[`public string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.version { get; }`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L27) | Gets the extension implementation version. | — | — |
| <a id="api-4e6d6c842609b636"></a>[`public sealed class Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin : BGCS.Core.Extensibility.IBindingPlugin, BGCS.Core.Extensibility.ICacheFingerprintProvider`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L15) | Registers InnoEngine's explicit managed cimgui ABI layouts with BindGen-CS. | — | — |

## 项目依赖

- `BGCS`：公开引用边界由实际签名核对。
- `BGCS.Core`：公开引用边界由实际签名核对。
