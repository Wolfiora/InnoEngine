# Inno.UI

[UI 索引](README.md) · [Text](../text/Inno.Text.md)

`IUiService` / `UI` 门面提供 Context 生命周期、语言标记的文档加载、纯文本/内容片段/attribute/class 变更、RGBA8 命名纹理注册、`Update`、`Render` 与事件提取。字体由导入文档声明并在 `LoadDocument` 前注册，脚本不再逐项覆盖字体。语言选择由显式标识与已注册前端决定。`UiRenderFrame` 只发布 mesh/texture 增量、稳定 handle、释放请求与有序 draw commands，供渲染 Plugin 持久缓存。

每个 Context 的生命周期由调用方明确拥有。无活动 `UiExecutionContext` 时门面失败；持有服务的 Runtime/Provider 在其 owner 生命周期中关闭 Context。唯一 `Properties/ScriptingApi.cs` 限定脚本导出类型；后端 SPI 与原生 ABI 不导出。基础层不依赖 Scene、Rendering 或 Editor。

## 公开类型与工作流

| 类型 | 语义 |
| --- | --- |
| `UiContextOptions`、`UiContextHandle`、`UiDocumentHandle` | 独立 surface 的尺寸/density 与不透明代际句柄；调用方负责关闭。 |
| `UiDocumentAsset`、`UiDocumentSource`、`UiDocumentFontFace` | 导入文档与字体资产依赖，或运行期生成带语言标记的内容。 |
| `UiTextureData`、`UiFrameTexture` | 只接受紧密排列 RGBA8；帧纹理含 ID、revision 和像素副本。 |
| `UiVertex`、`UiMeshUpdate`、`UiTextureUpdate`、`UiDrawCommand`、`UiRenderFrame` | 后端中立资源增量与绘制命令；不是图形设备 handle。 |
| `UiEventType`、`UiEvent` | 后端更新后提取的 DOM 事件；`UiEvent` 派生自 Core `Event`，包含文档 handle 与元素 ID。 |
| `UI`、`IUiService`、`UiExecutionContext` | 文档生命周期、DOM 更改、纹理注册、独立 Context 输入更新、Render/DrainEvents。 |
| `IUiBackend`、`UiInputSnapshot`、`UiFontRegistration` | Runtime/adapter 边界，不属于脚本导出。 |

```csharp
using InnoEngine.UI;

static UiDocumentHandle Show(UiContextHandle context, UiDocumentAsset source)
{
    UiDocumentHandle document = UI.LoadDocument(context, source);
    UI.ShowDocument(context, document);
    return document;
}
```

上述调用需位于 UI Session scope；先 `Update` 再 `DrainEvents`/`Render`。`DrainEvents` 是后端到调用方的传输边界；调用方使用 Core `EventDispatcher` / `EventHub` 组织订阅，并在所属会话退出前释放订阅。多个 UI 区域分别将命中的局部输入传给 `Update(context, input)`。Context 关闭后句柄失效，跨 Session/Plugin reload 不持久化；持久状态应保存 Asset ID 与稳定业务 ID。文档通过内容与资产契约读取，命名纹理由调用方显式注册。具体语言支持范围见所属后端页面。导入字体在文档关闭或 Context 销毁后释放资产 lease；具体后端决定原生字体缓存何时清空。

## API 参考

[完整声明、行为与调用约束](Inno.UI/api.md)

<!-- api-include: Inno.UI/api.md -->

## 项目依赖

- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Text](../text/Inno.Text.md)：公开引用边界由实际签名核对。
- [Inno.Input](../input/Inno.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
