# Inno.Native.ImGuizmo

[分类索引](README.md) · [Editor Scene](../../modules/editor/Inno.Editor.Scene.md) · [Wiki 首页](../../README.md)

## 职责与公开入口

承载 BGCS 生成的 gizmo 互操作表，以及宿主原生库初始化。`ImGuizmo` 的公开操作与 ABI 类型由
`Bindings/bindings.props` 选择的 `Bindings/bindgen.json` 及实际目标配置 生成到 `Generated/Bindings.cs`；不手工修改生成物。
这些 API 仅属于原生/presentation 边界，不属于后端中立 Scene 或脚本 API。

## 初始化及所有权

静态初始化以当前程序集作为明确 owner，首先加载 cimgui 依赖，再加载 gizmo API。
内部 `ImGuizmoModuleProvider` 组合两个 `DynamicModuleProvider`，生成的 module context 拥有该 provider。
释放顺序是 API 后依赖，重复释放安全。API 卸载失败时保留其依赖，禁止继续使用该 provider。初始化失败按 SDK 的 provider 所有权协议释放本次取得的资源。
该内部类型不是公开扩展契约。

Native library 必须由 `Inno.Build.Toolchains.ImGuizmo` 的产物提供；初始化失败传播异常，不能通过禁用 gizmo 的静默 fallback 掩盖。
模块正常使用期间原生表属于进程寿命；释放表后不能再调用其函数。

## 帧内使用与关闭顺序

宿主先创建 ImGui context 并连接 `SetImGuiContext`。每帧在 ImGui `NewFrame` 后调用
`BeginFrame`，在目标窗口中选择 draw list、设置屏幕矩形和投影方式，再执行 `Manipulate`。
同帧多个对象用 `PushID(int)` / `PopID` 隔离当前 UI 状态；场景修改和 History 仍由上层事务负责。

矩阵参数指向连续 16 个 `float`；平移、旋转和缩放向量各需 3 个。
生成的 `ref float` overload 只固定首元素，**不会验证后续数组容量**。
原生 ImGui 占位结构不是可独立构造的有效窗口或 context。

`GetStyle` 返回借用的全局样式，禁止调用 `StyleDestroy`。
只有 `StyleStyle` 创建的原始堆地址可以由唯一 owner 销毁。
所有调用、样式修改和 context 切换在 presentation owner thread 串行执行；
先停止绘制与输入，再结束相关 ImGui context 的使用，最后释放函数表和动态库。

当前消费配置仅保留单字符串、整数和指针值 ID 入口。每个字符串在一次调用中完整编码为 UTF-8；
不公开需要同一原生缓冲区 begin/end 的自动字符串区间投影。
样式创建的 Native allocator 与 `StyleDestroy` 清理操作、全局样式的借用语义都在生成配置中明确声明。

## 最小调用示例

下面的辅助函数仅用于已经初始化的 presentation owner thread：宿主已完成当前帧的
`BeginFrame`、窗口选择、`SetDrawlist`、`SetRect` 与投影设置。
它验证连续矩阵容量并固定调用期间的内存；返回后由调用方把修改后的矩阵提交给自己的编辑事务。
示例不创建 ImGui context，也不持有 Scene 对象。项目需要启用 `AllowUnsafeBlocks`。

```csharp
using System;
using Inno.Native.ImGuizmo;

internal static class GizmoTranslation
{
    internal static unsafe bool Draw(
        int scopeId,
        ReadOnlySpan<float> view,
        ReadOnlySpan<float> projection,
        Span<float> transform
    ) {
        if (view.Length != 16 || projection.Length != 16 || transform.Length != 16)
            throw new ArgumentException("Each matrix must contain exactly sixteen floats.");

        ImGuizmo.PushID(scopeId);
        try
        {
            fixed (float* viewPointer = view)
            fixed (float* projectionPointer = projection)
            fixed (float* transformPointer = transform)
                return ImGuizmo.Raw.Manipulate(viewPointer, projectionPointer,
                    ImGuizmoOperation.Translate, ImGuizmoMode.World,
                    transformPointer, null, null, null, null) != 0;
        }
        finally
        {
            ImGuizmo.PopId();
        }
    }
}
```

## 验证入口

[ImGuizmo 原生初始化测试](../../../backends/ImGui/tests/Inno.Native.ImGuizmo.Tests/ImGuizmoInitTests.cs) 通过公开 API 调用真实原生库，
验证依赖与符号初始化。此项不代替 Editor Scene 中手势和渲染的专项测试。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGuizmo/Inno.Native.ImGuizmo.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## API 参考

[完整声明、行为与调用约束](Inno.Native.ImGuizmo/api.md)

<!-- api-include: Inno.Native.ImGuizmo/api.md -->

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
