# Inno.Integration.BrowserWasm32.Build

[分类索引](README.md) · [Wiki 首页](../../README.md) · [职责与文件树](../../architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)

## 职责、依赖与生命周期

将 BrowserWasm32 的 SDK、绑定 Profile、组件参数、产品执行器和部署布局组合为一个完整 `BuildPlatformContribution`。依赖仅指向本平台基础模块、共享标准产品库、绑定接入及所选 backend integration。

唯一公开入口 `BrowserWasm32BuildIntegration.Create(BuildCompositionContext)` 只构造不可变贡献，不发现工具、不创建缓存。平台基础模块不反向引用本库。Host、目标与 managed deployment 分别选择；失败边界属于 SDK 解析和实际执行操作。

## 使用示例

```csharp
using Inno.Build.Composition;
using Inno.Integration.BrowserWasm32.Build;

static BuildPlatformContribution Compose(BuildCompositionContext context)
    => BrowserWasm32BuildIntegration.Create(context);
```

示例借用已预检的宿主和发行上下文；实际执行需要明确工具链、绑定生成器与完整源码输入。失败、取消和不可用能力不发布部分结果。此构建协议不进入游戏脚本导出，也不保存 collectible generation 状态。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Integration.BrowserWasm32.Build.BrowserWasm32BuildIntegration`

<!-- api: T:Inno.Integration.BrowserWasm32.Build.BrowserWasm32BuildIntegration c0b5a69b391c0b155af03b8812093990f44d17c1e0d4ecbd5877ac4705f81996 -->
<!-- api: M:Inno.Integration.BrowserWasm32.Build.BrowserWasm32BuildIntegration.Create(Inno.Build.Composition.BuildCompositionContext) 1f5023c22c04aaec424d533d6580a3092e1d1293b565c4e14029d04dee042af2 -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-80471a27b60d76a7"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Integration.BrowserWasm32.Build.BrowserWasm32BuildIntegration.Create(Inno.Build.Composition.BuildCompositionContext context)`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Build/BrowserWasm32BuildIntegration.cs#L26) | Creates the complete platform registration without discovering tools or writing outputs.<br>`context`：The explicit operation host.<br>返回：A platform contribution containing its selected native and binding capabilities. |
| <a id="api-821b2874adf9863f"></a>[`public static class Inno.Integration.BrowserWasm32.Build.BrowserWasm32BuildIntegration`](../../../platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Build/BrowserWasm32BuildIntegration.cs#L15) | Composes this platform's binding, component and deployment policies. |
