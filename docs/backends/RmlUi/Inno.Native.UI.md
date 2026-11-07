# Inno.Native.UI

[分类索引](README.md) · [Wiki 首页](../../README.md) · [UI Toolchain](Inno.Build.Toolchains.UI.md)

手写的 `Native/include/RmlUiRuntime.hpp` 是唯一语义边界：它用 PImpl 与中立 DTO 隐藏 RmlUi。`Bindings/bindings.props` 选择正式 SDK 配置，同一次生成将 facade 降为 C ABI，再输出独立的 `Native/Generated/` 与 `Generated/Bindings.cs`。桥接库使用固定的 RmlUi 6.0 源码与对应 Text/字体依赖，产物为配置专属 `inno-ui` 动态库。原生 handle 只由 UI adapter 持有；游戏脚本及渲染 Plugin 消费 `Inno.UI` 中立数据，不直接调用 ABI。

## ABI、初始化与失败

构建 profile 选择动态或静态符号解析。BGCS 生成的 `UiNative` 覆盖 runtime/context/document 生命周期、DOM 修改、输入、字体、命名纹理、帧几何、裁剪、像素及事件复制；`InnoUi*` 数据结构只为 ABI 使用。桥接 status、领域 Result 和每次调用独立的错误对象分别返回；错误消息读取后通过对应 Release 释放，稳定 `Result` 不泄漏 RmlUi 名称。动态 profile 的类型初始化通过 `NativeDllLoader` 读取应用已部署的 `native/ui/<platform>`，缺库和 ABI 错误直接失败，不查询源码仓库。

RmlUi 字体引擎是进程级资源，内部 `RmlUiProcessHost` 显式管理引用计数、owner thread、Context 和最后释放。Context 销毁时释放文档与像素，但最后一个 backend 关闭仍可能收到字体纹理释放回调，因此原生层保留所需渲染接口壳直至最终 shutdown。文档仅在根布局某轴仍为 `auto` 时设置该轴的 Context `100%`，显式 CSS 不被覆盖；viewport 改变继续由所属实现重新布局。Context/Document ID 在进程内单调分配，拒绝跨 backend 旧句柄。这些为内部机制，不是上层公开协议。

参见 [UI adapter](Inno.Adapter.UI.RmlUi.md) 和 [绑定验收工具](../../development/build/Inno.Build.Cli.md)。实际执行环境与结果见[当前验收](../../../.agents/records/acceptance/current.md)；macOS 和其他设备的运行证据需分别提供。

## 调用顺序与资源增量

1. 在 UI 进程宿主线程创建 Runtime，再创建 Context。所有后续调用由该线程串行协调；`Create` 检查宿主线程，不代表每个后续函数都会再次阻止错误线程访问。
2. 注册所需字体和命名纹理，加载并显示文档。Context/document ID 仅属于本 Runtime；它们不是跨 reload 的对象身份。
3. 输入传给当前 Context，调用 `Update`，再调用 `Render` 取得本帧计数。`Render` 收集数据，不直接执行 GPU 绘制。
4. 按更新索引读取 `GetMeshInfo` / `GetTextureInfo`，再按返回的资源 ID 复制变化数据；复制命令和释放 ID。未变化的资源可继续复用消费方缓存。
5. 全部增量处理完才调用 `FinishFrame`。它清除更新/释放列表，事件队列由 `ClearEvents` 独立清理。不要在下一次 Render 后继续使用上一帧的更新索引。
6. 关闭时先停止输入和绘制，销毁 Context，最后销毁 Runtime。Runtime 也会清理仍存活的 Context；其内部 renderer 的最终退休受进程宿主协调。

DOM 修改的 `changed` 表示是否找到了元素并执行设置，不是比较旧值后的差异判定。目标元素不存在时允许 `Success` 且 `changed=0`。命名纹理注册接收未预乘的 RGBA 字节，内部复制并预乘 alpha，不能预先重复预乘。

## 最小事件读取示例

下面的后端代码借用已创建的 Runtime/Context，在 owner thread 读取目标元素 ID。事件长度不含结尾零字节，复制容量必须多留一个字节。完成全部复制后清空队列；若复制失败，示例保留队列供 owner 处理失败。它不替代领域层的事件路由或对象身份解析。

```csharp
using System;
using System.Runtime.InteropServices;
using System.Text;
using Inno.Native.UI;

internal static unsafe class NativeEventReader
{
    internal static string[] ReadTargetIds(
        InnoUIRmlUiAdapterRuntime* runtime,
        ulong context
    ) {
        ulong count = 0;
        Result result = default;
        BgcsInnoRmluiError* error = null;
        int status = UiNative.Raw.GetEventCount(runtime, context, &count, &result, &error);
        Check(status, result, error);
        string[] targets = new string[checked((int)count)];
        for (int index = 0; index < targets.Length; index++)
        {
            InnoUIRmlUiAdapterEventInfo info = default;
            status = UiNative.Raw.GetEventInfo(runtime, context, (ulong)index,
                (InnoUIRmlUiAdapterEventInfoNative*)&info, &result, &error);
            Check(status, result, error);
            int length = checked((int)info.TargetIdLength);
            byte[] bytes = new byte[checked(length + 1)];
            fixed (byte* destination = bytes)
            {
                status = UiNative.Raw.CopyEventTargetId(runtime, context, (ulong)index,
                    (sbyte*)destination, (ulong)bytes.Length, &result, &error);
                Check(status, result, error);
            }
            targets[index] = Encoding.UTF8.GetString(bytes.AsSpan(0, length));
        }
        status = UiNative.Raw.ClearEvents(runtime, context, &result, &error);
        Check(status, result, error);
        return targets;
    }

    private static void Check(
        int status,
        Result result,
        BgcsInnoRmluiError* error
    ) {
        try
        {
            if (status != 0 || error != null)
                throw new InvalidOperationException(error == null ? $"Bridge status: {status}"
                    : Marshal.PtrToStringUTF8((nint)UiNative.Raw.BgcsInnoRmluiErrorMessage(error)));
            if (result != Result.Success)
                throw new InvalidOperationException($"UI call failed: {result}");
        }
        finally
        {
            if (error != null)
                UiNative.Raw.BgcsInnoRmluiErrorRelease(error);
        }
    }
}
```

## 静态目标

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/ui/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../../.agents/records/acceptance/current.md)为准。

## 源码归属

当前唯一源码 owner：`backends/RmlUi/native/Inno.Native.UI/Inno.Native.UI.csproj`。共同领域与平台产品通过明确契约组合。

## API 参考

[完整声明、行为与调用约束](Inno.Native.UI/api.md)

<!-- api-include: Inno.Native.UI/api.md -->

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。
