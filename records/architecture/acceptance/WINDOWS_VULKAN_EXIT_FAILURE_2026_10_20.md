# Windows Vulkan 退出访问冲突记录

## 修正前的事实

- 环境：Windows x64、SDK 10.0.402、Runtime 10.0.13、BGCS 3.0.1，Editor Release。
- 显卡：AMD Radeon(TM) Graphics / 31.0.14084.6002；NVIDIA GeForce RTX 3070 Ti Laptop GPU / 32.0.15.9649。
- Canvas Vulkan 隐藏窗口运行两次，分别完成 30、60 帧。空 Project 同样完成 60 帧。三次均记录状态保存、Run loop end、Dispose end，然后以 `0xC0000005` 退出。
- Windows Application Error 事件记录的故障模块为 `nvoglv64.dll`，版本 `32.0.15.9649`，首个样本偏移为 `0xF39EC4`。
- 同一 Editor 的 Canvas D3D11 / D3D12、另外两个 Project 的 D3D11 smoke 正常退出。
- 已从系统保存的 `dotnet.exe.33948.dmp` 取得原生线程和托管调用堆栈。主线程位于 `BgfxDevice.DrainDeferredResourcesForShutdown → SubmitNativeFrame`，BGFX 渲染线程在 Vulkan 驱动内访问空指针。`EditorHost` 的 Dispose 日志先于其外层 Shell 释放；原先的日志不能证明渲染设备已经完成释放。故障模块在驱动中仍不能单独证明根因属于驱动。

BGFX 部署 DLL SHA-256：`3b05bea5d01774ef75c8075016f7e17bf45f663ef660ce43fef6376665e0b478`。其 binding 身份为 `b40a736898c7195d47a53b40c4680d6d4e9eea15a54ab942e8ffd0c42f975a43`。该 DLL 的 202 个 SDK 必需导出全部存在；导出存在不证明运行期调用和释放正确。

## 复现命令

```powershell
C:/Users/23842/.dotnet/dotnet.exe `
  entry/InnoEngine.Editor/bin/Release/net10.0/InnoEngine.Editor.dll `
  C:/Dev/GameEngineDev/InnoEngine.Canvas `
  --graphics-api vulkan --window-visible false --smoke-frames 60
```

空 Project 位于本轮 D 盘独立验收目录的 `runtime-projects/EmptyVulkan`；该操作的实际根目录记录于 `artifacts/acceptance/bgcs301-2026-10-20/drive-output-root.txt`。

## 证据

原始证据均位于 `artifacts/acceptance/bgcs301-2026-10-20`：

- `editor-canvas-vulkan-smoke-01.log`
- `editor-canvas-vulkan-repeat-smoke-01.log`
- `editor-empty-vulkan-smoke-01.log`
- `vulkan-reproduction-results.json`
- `windows-gpu-faults.json`
- `editor-native-exports-final.json`
- `vulkan-empty-native-stack-03.log`：使用已安装的 CDB、本地符号读取全部线程；原生 BGFX 私有 PDB 缺失，不能把最近导出符号名当作准确函数名。

## 下一步诊断与边界

1. 核对关闭时重置的 view 与 destruction queue：现有实现先 `ResetPreviousViews`，随后在每个排空帧前 `touch(0)`。该提交会再次使用已经重置的 view，而排空销毁队列只需要推进 frame。
2. 修正移除关闭排空中的两处 touch，保留原有帧数、Flush 标志、资源退休和进程 owner 释放协议。现有 surface retirement drain 已使用不提交 view 的 frame 推进方式。
3. 新增公开边界回归，使用真实借用窗口、离屏 Graph、设备所有纹理及重复设备创建；不增加 Inspector、History 或 Renderer 的测试后门。
4. 不在共同 Rendering 层加入 NVIDIA、Vulkan 或宿主特判，不修改 extern 或生成绑定。
5. 在同一最终状态下重复空 Project、插件与 Sample 的 Vulkan 运行及有序退出，回归 D3D11 / D3D12。设备视觉、输入和音频仍独立验收。

## 对照与产品复验

修正前的 D 盘适配器二进制 SHA-256 为 `0724B0CC1304C2124A7F1426C3902795FDD2B82B5DDBEBED3F1B847491805F36`。新增 `BgfxShutdownTests` 使用真实 Vulkan 窗口和离屏 Graph，在该二进制下发生 testhost 崩溃；日志为 `shutdown-before-vulkan.log`，TRX 为 `shutdown-before-vulkan.trx`。不是依赖故意抛出的断言或反射构造内部状态。

包含修正的正式 Editor Release 构建通过，0 警告、0 错误，见 `windows-editor-release-03.log`。相同设备、驱动、Native 和项目下，以下七次 smoke 均运行 60 帧并以 0 退出：空 Project Vulkan 两次、Canvas / Rendering2D / FlappyBird Vulkan 各一次、Canvas D3D11 / D3D12 各一次。实际退出码、时间和逐次日志见 `editor-shutdown-fixed-results.json`，适配器哈希见 `shutdown-fixed-editor-adapter-hash.json`。

修正后的真实窗口回归在 Vulkan、D3D11、D3D12 上分别通过，各 1 项、0 失败、0 跳过，见 `shutdown-after-results.json` 和对应 TRX。包含修正的完整 SourceRelease 测试闭包再次构建通过，0 警告、0 错误，见 `managed-windows-release-build-02.log`；该构建复用首次正式部署的原生文件，未重新运行重复的逐测试产品部署，生产 Editor 验收始终使用完整正式构建。

这些结果支持关闭队列推进中重复提交已重置 view 是本次引擎侧触发条件。修正不修改驱动、第三方源码、BGCS 生成物或公共 Rendering 契约，不引入后端特判。完整套件及长时间设备验收继续分别登记；隐藏窗口 smoke 不证明视觉、焦点、音频或长期稳定性。
