# Inno.Build.SupportPacks

[Build 索引](README.md) · [Player](../runtime/Inno.Player.md)

该可执行项目提供手动生成 Support Pack 的命令行入口，发布实现由 [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md) 复用。它生产按 RID 隔离的 source-independent Player Support Pack。原生库按目标扩展名过滤：macOS 只收 `.dylib`，Windows 只收 `.dll`。Support Pack 是引擎发行物，不是 Project Asset；正式 Editor distribution 必须随包携带目标目录。

输出主动排除 `.pdb`、`.dbg`、`.map`、`.xml`、源码、Editor、Build、Compiler、Reload、Assets Pipeline、Plugins Authoring 和 toolchain。Game Export 只消费生成结果；源码工作区缺少目标 Pack 时，宿主会先调用发布器生成它。

源码开发环境可在构建 Editor 后显式生成当前平台 Pack：

```shell
/path/to/dotnet build/support/Inno.Build.SupportPacks/bin/Debug/net9.0/Inno.Build.SupportPacks.dll \
  --engine-root /path/to/InnoEngine \
  --output /path/to/InnoEngine/src/composition/editor/host/Inno.Editor.Application/bin/Debug/net9.0/SupportPacks \
  --target macos-arm64 \
  --dotnet /path/to/dotnet
```

Editor 默认从 `AppContext.BaseDirectory/SupportPacks` 读取；构建机也可用 `INNO_SUPPORT_PACK_ROOT` 指向预生成的发行目录。源码工作区中的 Editor 与 Build CLI 在目标目录不存在时自动运行同一发布器，校验通过后继续导出。已存在但损坏的 Pack 明确报错，不会被静默覆盖。对 macOS arm64 或 Windows x64 执行 `dotnet publish Inno.Editor.Application` 时，发布目标自动生成对应 Pack，并将 Editor 所需的 Release 原生库、BGFX shaderc/texturec 与 shader include，以及 .NET Ref Pack 放入发布目录；发布目录离开源码树后可直接运行脚本和导出。Build CLI 在自身 composition root 直接部署 Scene importer 等 authoring 实现；`Inno.Build` 的 implementation-only reference 不被当作 CLI 的传递部署闭包。

Support Pack 不再保存或比较由宿主进程加载状态推导出的全局脚本 API 契约。导出时，Compiler 先执行当前逻辑脚本 API 校验，再直接使用所选 Pack 的真实 `Inno.*` 程序集编译部署脚本：未使用的 API 差异不会阻断导出；项目实际使用但 Pack 不具备的 API 会在打包前产生源码诊断。更换 Pack 会改变编译缓存身份，不会沿用旧目标的脚本 DLL。

源码开发阶段，缺失的 Pack 自动生成；已有 Pack 保持原行为，Player 实现或原生库变化后可手动重建。正式 Editor 发布目录直接携带已生成 Pack。Support Pack 仍独立存在，因为导出产物需要自包含 .NET Runtime、Player composition、发行版原生库与平台目录结构，而这些都不属于 Editor 进程的可部署闭包。
