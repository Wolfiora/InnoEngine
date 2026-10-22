# Inno.Integration.WindowsX64.Build

[分类索引](README.md) · [Wiki 首页](../../README.md) · [职责与文件树](../../architecture/NATIVE_BUILD_PLATFORM_OWNERSHIP.md)

## 职责、依赖与生命周期

将 WindowsX64 的 SDK、绑定 Profile、组件参数、产品执行器和部署布局组合为一个完整 `BuildPlatformContribution`。依赖仅指向本平台基础模块、共享标准产品库、绑定接入及所选 backend integration。

唯一公开入口 `WindowsX64BuildIntegration.Create(BuildCompositionContext)` 只构造不可变贡献，不发现工具、不创建缓存。平台基础模块不反向引用本库。Host、目标与 managed deployment 分别选择；失败边界属于 SDK 解析和实际执行操作。

## 使用示例

```csharp
using Inno.Build.Composition;
using Inno.Integration.WindowsX64.Build;

static BuildPlatformContribution Compose(BuildCompositionContext context)
    => WindowsX64BuildIntegration.Create(context);
```

示例借用已预检的宿主和发行上下文；实际执行需要明确工具链、绑定生成器与完整源码输入。失败、取消和不可用能力不发布部分结果。此构建协议不进入游戏脚本导出，也不保存 collectible generation 状态。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Integration.WindowsX64.Build.WindowsX64BuildIntegration`

<!-- api: T:Inno.Integration.WindowsX64.Build.WindowsX64BuildIntegration 732835dd5f8f07b55295d3df86f30b9684a7d60f46ac895e8fc8f89c8b8a5837 -->
<!-- api: M:Inno.Integration.WindowsX64.Build.WindowsX64BuildIntegration.Create(Inno.Build.Composition.BuildCompositionContext) e3f6e9b579e4043889118bd1f457b60aa5ab75ec2f297a65079f611496c75d6e -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-7efbaf74425d943c"></a>[`public static Inno.Build.Composition.BuildPlatformContribution Inno.Integration.WindowsX64.Build.WindowsX64BuildIntegration.Create(Inno.Build.Composition.BuildCompositionContext context)`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Build/WindowsX64BuildIntegration.cs#L26) | Creates the complete platform registration without discovering tools or writing outputs.<br>`context`：The explicit operation host.<br>返回：A platform contribution containing its selected native and binding capabilities. |
| <a id="api-8a8bc98ff2897de7"></a>[`public static class Inno.Integration.WindowsX64.Build.WindowsX64BuildIntegration`](../../../platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Build/WindowsX64BuildIntegration.cs#L15) | Composes this platform's binding, component and deployment policies. |
