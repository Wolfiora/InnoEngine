# Inno.Native.ImGuizmo

[Native 索引](README.md) · [Editor Scene](../editor/Inno.Editor.Scene.md)

## 职责与公开入口

承载 BGCS 生成的 gizmo 互操作表，以及宿主原生库初始化。`ImGuizmo` 的公开操作与 ABI 类型由
`Bindings/bindgen.json`、`Bindings/common.json` 生成到 `Generated/Bindings.cs`；不手工修改生成物。
这些 API 仅属于原生/presentation 边界，不属于后端中立 Scene 或脚本 API。

## 初始化及所有权

静态初始化以当前程序集作为明确 owner，首先加载 cimgui 依赖，再加载 gizmo API。
内部 `ImGuizmoNativeContext` 同时拥有两次加载取得的句柄；生成的 FunctionTable 拥有此 context。
释放顺序是 API 后依赖，重复释放安全。初始化失败释放本次已取得的两个句柄，不遗留忽略的 dependency handle。
该内部类型不是公开扩展契约。

Native library 必须由 `Inno.Build.Toolchains.ImGuizmo` 的产物提供；初始化失败传播异常，不能通过禁用 gizmo 的静默 fallback 掩盖。
模块正常使用期间原生表属于进程寿命；释放表后不能再调用其函数。

## 验证入口

`tests/native/Inno.Native.ImGuizmo.Tests/ImGuizmoInitTests.cs` 通过公开 API 调用真实原生库，
验证依赖与符号初始化。此项不代替 Editor Scene 中手势和渲染的专项测试。
