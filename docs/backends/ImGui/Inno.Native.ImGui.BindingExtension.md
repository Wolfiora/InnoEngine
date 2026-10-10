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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L15) | Registers InnoEngine's explicit managed cimgui ABI layouts with BindGen-CS. |
| [`int Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.contractVersion`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L32) | Gets the BGCS plugin protocol version implemented by this extension. |
| [`string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.GetCacheFingerprint()`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L49) | Fingerprints source templates so edits invalidate incremental binding output. |
| [`string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.id`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L22) | Identifies the InnoEngine cimgui layout extension. |
| [`string Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.version`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L27) | Gets the extension implementation version. |
| [`void Inno.Native.ImGui.Bindings.Extension.ImGuiBindingPlugin.Configure(BGCS.Core.Extensibility.IBindingPluginHost host)`](../../../backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/ImGuiBindingPlugin.cs#L40) | Registers the managed-layout emitter with the BGCS host. |

## 项目依赖

- `BGCS`：公开引用边界由实际签名核对。
- `BGCS.Core`：公开引用边界由实际签名核对。
